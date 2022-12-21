/* This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. */

/*
Authors:
Michael Berg <michael.berg@zalf.de>

Maintainers:
Currently maintained by the authors.

This file is part of the ZALF model and simulation infrastructure.
Copyright (C) Leibniz Centre for Agricultural Landscape Research (ZALF)
*/

#include "rpc-connection-manager.h"

#include <algorithm>
#include <chrono>
#include <thread>

#ifdef WIN32
//#include <winsock.h>
#include <ws2tcpip.h>
#define CLOSE_SOCKET(s) closesocket(s)
#define SOCKLEN_T int
#else
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <unistd.h>
#include <sys/socket.h>
#include <netinet/in.h>
#include <arpa/inet.h>
#define CLOSE_SOCKET(s) close(s)
#define SOCKLEN_T socklen_t
#endif

#include <kj/async-io.h>
#include <kj/debug.h>
#include <kj/encoding.h>
#include <kj/map.h>
#include <kj/timer.h>
#define KJ_MVCAP(var) var = kj::mv(var)

#include <capnp/ez-rpc.h>
#include <capnp/message.h>
#include <capnp/rpc-twoparty.h>

#include "common.capnp.h"
#include "persistence.capnp.h"

#include "common.h"
#include "restorer.h"

using namespace mas::infrastructure::common;

//-----------------------------------------------------------------------------

struct ClientContext {
    kj::Own<kj::AsyncIoStream> stream;
    capnp::TwoPartyVatNetwork network;
    capnp::RpcSystem<capnp::rpc::twoparty::VatId> rpcSystem;
    capnp::Capability::Client bootstrap{ nullptr };

    ClientContext(kj::Own<kj::AsyncIoStream>&& stream, capnp::ReaderOptions readerOpts)
        : stream(kj::mv(stream))
        , network(*this->stream, capnp::rpc::twoparty::Side::CLIENT, readerOpts)
        , rpcSystem(makeRpcClient(network)) {}

    capnp::Capability::Client getMain() {
        capnp::word scratch[4];
        memset(scratch, 0, sizeof(scratch));
        capnp::MallocMessageBuilder message(scratch);
        auto hostId = message.getRoot<capnp::rpc::twoparty::VatId>();
        hostId.setSide(capnp::rpc::twoparty::Side::SERVER);
        return rpcSystem.bootstrap(hostId);
    }
};

//-----------------------------------------------------------------------------

struct ServerContext {
    kj::Own<kj::AsyncIoStream> stream;
    capnp::TwoPartyVatNetwork network;
    capnp::RpcSystem<capnp::rpc::twoparty::VatId> rpcSystem;

    ServerContext(
        kj::Own<kj::AsyncIoStream>&& stream,
        capnp::ReaderOptions readerOpts,
        capnp::Capability::Client mainInterface)
        : stream(kj::mv(stream))
        , network(*this->stream, capnp::rpc::twoparty::Side::SERVER, readerOpts)
        , rpcSystem(makeRpcServer(network, mainInterface)) {}
};

//-----------------------------------------------------------------------------

struct ConnectionManager::Impl {

    kj::String locallyUsedHost;
    int port{ 0 };

    struct ErrorHandler : public kj::TaskSet::ErrorHandler {
        void taskFailed(kj::Exception&& exception) override {
            kj::throwFatalException(kj::mv(exception));
        }
    };

    ErrorHandler eh;
    kj::TaskSet tasks;
    kj::HashMap<kj::String, kj::Own<ClientContext>> connections;
    capnp::Capability::Client serverMainInterface{ nullptr };
    kj::Timer* timer{nullptr};
    kj::Own<Restorer> restorer;
    kj::NullDisposer disposer;

    Impl(Restorer *restorer)
    : tasks(eh) {

        if (restorer) this->restorer = kj::Own<Restorer>(restorer, disposer);
        else this->restorer = kj::heap<Restorer>();
    }

    ~Impl() noexcept(false)  {}


    void acceptLoop(kj::Own<kj::ConnectionReceiver> &&listener, capnp::ReaderOptions readerOpts) {
        auto ptr = listener.get();
        tasks.add(ptr->accept().then(kj::mvCapture(kj::mv(listener),
            [readerOpts, this]
            (kj::Own<kj::ConnectionReceiver> &&listener,
                kj::Own<kj::AsyncIoStream> &&connection){
                acceptLoop(kj::mv(listener), readerOpts);

                KJ_LOG(INFO, "connection from client");
                auto server = kj::heap<ServerContext>(kj::mv(connection), readerOpts, serverMainInterface);

                // Arrange to destroy the server context when all references are gone, or when the
                // EzRpcServer is destroyed (which will destroy the TaskSet).
                tasks.add(server->network.onDisconnect().attach(kj::mv(server)));
                // tasks.add(server->network.onDisconnect().then([]() {
                //	KJ_LOG(INFO, "disconnecting ok");
                // }, [](auto&& err) {
                //	KJ_LOG(INFO, "diconnecting error", err);
                // }).attach(kj::mv(server)));
            }
        )));
    }
};

//-----------------------------------------------------------------------------

ConnectionManager::ConnectionManager(Restorer *restorer)
: impl(kj::heap<Impl>(restorer)) {
}

ConnectionManager::~ConnectionManager() {}


kj::StringPtr ConnectionManager::getLocallyUsedHost() const { return impl->locallyUsedHost; }
void ConnectionManager::setLocallyUsedHost(kj::StringPtr h) { impl->locallyUsedHost = kj::str(h); }


kj::Promise<capnp::Capability::Client> ConnectionManager::tryConnect(kj::AsyncIoContext &ioc, kj::StringPtr sturdyRefStr,
    int retryCount, int retrySecs, bool printRetryMsgs) {
    try {
        return connect(ioc, sturdyRefStr);
    } catch (std::exception e) {
        if (!impl->timer) impl->timer = &(ioc.provider->getTimer());
        return impl->timer->afterDelay(retrySecs * kj::SECONDS).then([&]() {
            if(retryCount == 0) {
                if(printRetryMsgs) KJ_LOG(INFO, "Couldn't connect to sturdy_ref at", sturdyRefStr, "!");
                    return kj::Promise<capnp::Capability::Client>(nullptr);
            }
            retryCount -= 1;
            if(printRetryMsgs) KJ_LOG(INFO, "Trying to connect to", sturdyRefStr, "again in", retrySecs, "s!");
            retrySecs += 1;
            return tryConnect(ioc, sturdyRefStr); });
    }
}

capnp::Capability::Client ConnectionManager::tryConnectB(kj::AsyncIoContext &ioc, kj::StringPtr sturdyRefStr,
    int retryCount, int retrySecs, bool printRetryMsgs) {
    while (true) {
        try {
            return connect(ioc, sturdyRefStr).wait(ioc.waitScope);
        } catch (kj::Exception e) {
            if (retryCount == 0) {
                if (printRetryMsgs) KJ_LOG(INFO, "Couldn't connect to sturdy_ref at", sturdyRefStr, "!");
                return nullptr;
            }
            KJ_DBG(e);
            std::this_thread::sleep_for(std::chrono::milliseconds(retrySecs * 1000));
            retryCount -= 1;
            if (printRetryMsgs) KJ_LOG(INFO, "Trying to connect to", sturdyRefStr, "again in", retrySecs, "s!");
            retrySecs += 1;
        }
    }
}

kj::Promise<capnp::Capability::Client> ConnectionManager::connect(kj::AsyncIoContext &ioc, kj::StringPtr sturdyRefStr) {
    capnp::ReaderOptions readerOpts;

    auto restoreSR = [this](capnp::Capability::Client bootstrapCap, kj::StringPtr srTokenBase64) {
        KJ_LOG(INFO, "restoring token", srTokenBase64);
        auto srTokenArr = kj::decodeBase64(srTokenBase64);
        kj::String srToken = kj::str(srTokenArr.asChars());
        auto restorerClient = bootstrapCap.castAs<mas::schema::persistence::Restorer>();
        auto req = restorerClient.restoreRequest();
        req.initLocalRef().setAs<capnp::Text>(srToken);
        KJ_LOG(INFO, "making restore request");
        return req.send().then([](auto &&res) {
            KJ_LOG(INFO, "send returned");
            return res.getCap();
        });
    };

    // we assume that a sturdy ref url looks always like
    // capnp://vat-id_base64-curve25519-public-key@host:port/sturdy-ref-token_base64
    if (sturdyRefStr.startsWith("capnp://")) {
        auto srStr = sturdyRefStr.slice(8);
        // right now we only support tcp connections
        KJ_IF_MAYBE (atPos, srStr.findFirst('@')) {
            auto vatIdBase64 = sturdyRefStr.slice(0, *atPos);
            srStr = srStr.slice(*atPos + 1);
            kj::String addressPort;
            kj::String srTokenBase64;
            KJ_IF_MAYBE (slashPos, srStr.findFirst('/')) {
                addressPort = kj::str(srStr.slice(0, *slashPos));
                srTokenBase64 = kj::str(srStr.slice(*slashPos + 1));
            } else {
                addressPort = kj::str(srStr.slice(*atPos + 1));
            }

            if (!addressPort.size() == 0) {
                if (addressPort == kj::str(impl->locallyUsedHost, ":", impl->port)) {
                    KJ_LOG(INFO, "connecting to local server");
                    if (srTokenBase64.size() > 0) return restoreSR(impl->serverMainInterface, srTokenBase64);
                    else return kj::Promise<capnp::Capability::Client>(impl->serverMainInterface);
                }

                KJ_IF_MAYBE (clientContext, impl->connections.find(addressPort)) {
                    if (srTokenBase64.size() > 0) return restoreSR((*clientContext)->bootstrap, srTokenBase64);
                    return (*clientContext)->bootstrap;
                } else {
                    return ioc.provider->getNetwork().parseAddress(addressPort).then(
                        [restoreSR](kj::Own<kj::NetworkAddress> &&addr) { 
                            return addr->connect().attach(kj::mv(addr)); 
                        }).then(
                        [restoreSR, readerOpts, this, KJ_MVCAP(addressPort), KJ_MVCAP(srTokenBase64)]
                        (kj::Own<kj::AsyncIoStream> &&stream) {
                            auto cc = kj::heap<ClientContext>(kj::mv(stream), readerOpts);
                            capnp::Capability::Client bootstrapCap = cc->getMain();
                            cc->bootstrap = bootstrapCap;
                            impl->connections.insert(kj::str(addressPort), kj::mv(cc));

                            if (srTokenBase64.size() > 0) {
                                return restoreSR(bootstrapCap, srTokenBase64);
                            }
                            return kj::Promise<capnp::Capability::Client>(bootstrapCap); 
                        }
                    );
                }
            }
        }
    }

    return nullptr;
}

kj::Promise<kj::uint> ConnectionManager::bind(kj::AsyncIoContext &ioContext,
    capnp::Capability::Client mainInterface, kj::StringPtr host, kj::uint port) {
    impl->serverMainInterface = mainInterface;

    auto portPaf = kj::newPromiseAndFulfiller<uint>();

    auto &network = ioContext.provider->getNetwork();
    auto bindAddress = kj::str(host, port < 0 ? kj::str("") : kj::str(":", port));

    impl->tasks.add(network.parseAddress(bindAddress, port).then(
        [portFulfiller = kj::mv(portPaf.fulfiller), this](kj::Own<kj::NetworkAddress> &&addr) mutable {
            auto listener = addr->listen();
            impl->port = listener->getPort();
            portFulfiller->fulfill(impl->port);
            impl->acceptLoop(kj::mv(listener), capnp::ReaderOptions()); 
        }
    ));

    return kj::mv(portPaf.promise);
}


kj::Tuple<bool, kj::String>
mas::infrastructure::common::getLocalIP(kj::StringPtr connectToHost, kj::uint connectToPort) {
    if (connectToHost == "") connectToHost = "8.8.8.8";
    if (connectToPort == 0) connectToPort = 53;

    // taken from https://gist.github.com/listnukira/4045436
    char myIP[16];
    unsigned int myPort;
    struct sockaddr_in server_addr, my_addr;
    int sockfd;

    // Connect to server
    if ((sockfd = (int)socket(AF_INET, SOCK_STREAM, 0)) < 0) {
        return kj::tuple(false, kj::str("Can't open stream socket."));
    }

    // Set server_addr
    memset(&server_addr, 0, sizeof(server_addr));
    server_addr.sin_family = AF_INET;
    server_addr.sin_addr.s_addr = inet_addr(connectToHost.cStr());
    server_addr.sin_port = htons(connectToPort);

    // Connect to server
    if (connect(sockfd, (struct sockaddr *)&server_addr, sizeof(server_addr)) < 0) {
        CLOSE_SOCKET(sockfd);
        return kj::tuple(false, kj::str("Can't connect to server (", connectToHost, ":", connectToPort, ")"));
    }

    // Get my ip address and port
    memset(&my_addr, 0, sizeof(my_addr));
    SOCKLEN_T len = sizeof(my_addr);
    getsockname(sockfd, (struct sockaddr *)&my_addr, &len);
    inet_ntop(AF_INET, &my_addr.sin_addr, myIP, sizeof(myIP));
    myPort = ntohs(my_addr.sin_port);
    CLOSE_SOCKET(sockfd);
    return kj::tuple(true, kj::str(myIP));
}