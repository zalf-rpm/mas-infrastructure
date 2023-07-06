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

// moving this below storage-service.h on windows causes Problems,
// because of IPrintDialogServices in commdlg.h defining INTERFACE as macro
#ifdef WIN32
//#include <winsock.h>
#include <ws2tcpip.h>
#define CLOSE_SOCKET(s) closesocket(s)
#define SOCKLEN_T int
#else

#include <cstdio>
#include <cstdlib>
#include <cstring>
#include <unistd.h>
#include <sys/socket.h>
#include <netinet/in.h>
#include <arpa/inet.h>

#define CLOSE_SOCKET(s) close(s)
#define SOCKLEN_T socklen_t
#endif

#include "rpc-connection-manager.h"

#include <algorithm>
#include <chrono>
#include <thread>

#include <kj/async-io.h>
#include <kj/debug.h>
#include <kj/encoding.h>
#include <kj/map.h>
#include <kj/timer.h>
#include <kj/compat/url.h>
#include <capnp/ez-rpc.h>
#include <capnp/message.h>
#include <capnp/rpc-twoparty.h>

#include "common.capnp.h"
#include "persistence.capnp.h"

#include "common.h"
#include "restorer.h"

#define KJ_MVCAP(var) var = kj::mv(var)

using namespace mas::infrastructure::common;

struct ClientContext {
  kj::Own<kj::AsyncIoStream> stream;
  capnp::TwoPartyVatNetwork network;
  capnp::RpcSystem<capnp::rpc::twoparty::VatId> rpcSystem;
  capnp::Capability::Client bootstrap{nullptr};

  ClientContext(kj::Own<kj::AsyncIoStream> &&stream, capnp::ReaderOptions readerOpts)
      : stream(kj::mv(stream)), network(*this->stream, capnp::rpc::twoparty::Side::CLIENT, readerOpts),
        rpcSystem(makeRpcClient(network)) {}

  capnp::Capability::Client getMain() {
    capnp::word scratch[4];
    memset(scratch, 0, sizeof(scratch));
    capnp::MallocMessageBuilder message(scratch);
    auto hostId = message.getRoot<capnp::rpc::twoparty::VatId>();
    hostId.setSide(capnp::rpc::twoparty::Side::SERVER);
    return rpcSystem.bootstrap(hostId);
  }
};

struct ServerContext {
  kj::Own<kj::AsyncIoStream> stream;
  capnp::TwoPartyVatNetwork network;
  capnp::RpcSystem<capnp::rpc::twoparty::VatId> rpcSystem;

  ServerContext(
      kj::Own<kj::AsyncIoStream> &&stream,
      capnp::ReaderOptions readerOpts,
      capnp::Capability::Client mainInterface)
      : stream(kj::mv(stream)), network(*this->stream, capnp::rpc::twoparty::Side::SERVER, readerOpts),
        rpcSystem(makeRpcServer(network, mainInterface)) {}
};

struct ConnectionManager::Impl {
  kj::String locallyUsedHost;
  kj::uint port{0};

  struct ErrorHandler : public kj::TaskSet::ErrorHandler {
    void taskFailed(kj::Exception &&exception) override {
      kj::throwFatalException(kj::mv(exception));
    }
  };

  ErrorHandler eh;
  kj::TaskSet tasks;
  kj::HashMap<kj::String, kj::Own<ClientContext>> connections;
  capnp::Capability::Client serverMainInterface{nullptr};
  kj::Timer *timer{nullptr};
  kj::Own<Restorer> restorer;
  kj::NullDisposer disposer;
  kj::AsyncIoContext *ioContext{nullptr};

  explicit Impl(kj::AsyncIoContext &ioc, Restorer *restorer)
      : tasks(eh), ioContext(&ioc) {

    if (restorer) this->restorer = kj::Own<Restorer>(restorer, disposer);
    else this->restorer = kj::heap<Restorer>();
  }

  ~Impl() = default;

  void acceptLoop(kj::Own<kj::ConnectionReceiver> &&listener, capnp::ReaderOptions readerOpts) {
    auto ptr = listener.get();
    tasks.add(ptr->accept()
                  .then(kj::mvCapture(kj::mv(listener),
                                      [readerOpts, this]
                                          (kj::Own<kj::ConnectionReceiver> &&listener,
                                           kj::Own<kj::AsyncIoStream> &&connection) {
                                        acceptLoop(kj::mv(listener), readerOpts);

                                        KJ_LOG(INFO, "connection from client");
                                        auto server = kj::heap<ServerContext>(kj::mv(connection), readerOpts,
                                                                              serverMainInterface);

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

ConnectionManager::ConnectionManager(kj::AsyncIoContext &ioc, Restorer *restorer)
    : impl(kj::heap<Impl>(ioc, restorer)) {
}

ConnectionManager::~ConnectionManager() = default;

kj::AsyncIoContext& ConnectionManager::ioContext() const {
  KJ_REQUIRE_NONNULL(impl->ioContext);
  return *(impl->ioContext);
}

kj::StringPtr ConnectionManager::getLocallyUsedHost() const { return impl->locallyUsedHost; }

void ConnectionManager::setLocallyUsedHost(kj::StringPtr h) { impl->locallyUsedHost = kj::str(h); }


kj::Promise<capnp::Capability::Client> ConnectionManager::tryConnect(kj::StringPtr sturdyRefStr,
                                                                     int retryCount, int retrySecs,
                                                                     bool printRetryMsgs) {
  try {
    return connect(sturdyRefStr);
  } catch (const std::exception &e) {
    if (!impl->timer) impl->timer = &(impl->ioContext->provider->getTimer());
    return impl->timer->afterDelay(retrySecs * kj::SECONDS).then([&]() {
      if (retryCount == 0) {
        if (printRetryMsgs) KJ_LOG(INFO, "Couldn't connect to sturdy_ref at", sturdyRefStr, "!");
        return kj::Promise<capnp::Capability::Client>(nullptr);
      }
      retryCount -= 1;
      if (printRetryMsgs) KJ_LOG(INFO, "Trying to connect to", sturdyRefStr, "again in", retrySecs, "s!");
      retrySecs += 1;
      return tryConnect(sturdyRefStr);
    });
  }
}

capnp::Capability::Client ConnectionManager::tryConnectB(kj::StringPtr sturdyRefStr,
                                                         int retryCount, int retrySecs, bool printRetryMsgs) {
  while (true) {
    try {
      return connect(sturdyRefStr).wait(impl->ioContext->waitScope);
    } catch (const kj::Exception &e) {
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

kj::Promise<capnp::Capability::Client> ConnectionManager::connect(kj::Url url) {
  auto restoreSR =
      [](capnp::Capability::Client bootstrapCap, kj::StringPtr srToken, kj::StringPtr ownerGuid) {
        KJ_LOG(INFO, "restoring token", srToken);
        auto restorerClient = bootstrapCap.castAs<mas::schema::persistence::Restorer>();
        auto req = restorerClient.restoreRequest();
        req.initLocalRef().setText(srToken);
        KJ_LOG(INFO, "making restore request");
        return req.send().then([](auto &&res) {
          KJ_LOG(INFO, "send returned");
          return res.getCap();
        });
      };

  auto connectTo =
      [this, restoreSR](kj::Url url, kj::StringPtr host, kj::uint port, kj::StringPtr ownerGuid) mutable {
        KJ_IF_MAYBE (clientContext, impl->connections.find(host)) {
          if (!url.path.empty()) return restoreSR((*clientContext)->bootstrap, url.path[0], ownerGuid);
          return kj::Promise<capnp::Capability::Client>((*clientContext)->bootstrap);
        } else {
          return impl->ioContext->provider->getNetwork().parseAddress(host, port).then(
              [restoreSR](kj::Own<kj::NetworkAddress> &&addr) {
                return addr->connect().attach(kj::mv(addr));
              }).then(
              [restoreSR, this, KJ_MVCAP(host), KJ_MVCAP(url), KJ_MVCAP(ownerGuid)](
                  kj::Own<kj::AsyncIoStream> &&stream) {
                capnp::ReaderOptions readerOpts;
                auto cc = kj::heap<ClientContext>(kj::mv(stream), readerOpts);
                capnp::Capability::Client bootstrapCap = cc->getMain();
                cc->bootstrap = bootstrapCap;
                impl->connections.insert(kj::str(host), kj::mv(cc));

                if (!url.path.empty()) {
                  return restoreSR(bootstrapCap, url.path[0], ownerGuid);
                }
                return kj::Promise<capnp::Capability::Client>(bootstrapCap);
              }
          );
        }
      };

  // we assume that a sturdy ref url looks always like
  // capnp://vat-id_base64-curve25519-public-key@host:port/sturdy-ref-token
  // ?owner_guid=optional_owner_global_unique_id
  // &b_iid=optional_bootstrap_interface_id
  // &sr_iid=optional_the_sturdy_refs_remote_interface_id
  if (url.scheme != "capnp") return nullptr;

  kj::StringPtr ownerGuid;
  uint64_t bootstrapInterfaceId = 0;
  uint64_t sturdyRefInterfaceId = 0;
  for (const auto &qp: url.query) {
    if (qp.name == "owner_guid") ownerGuid = qp.value;
    if (qp.name == "b_iid") bootstrapInterfaceId = qp.value.parseAs<uint64_t>();
    if (qp.name == "sr_iid") sturdyRefInterfaceId = qp.value.parseAs<uint64_t>();
  }

  if (url.host == kj::str(impl->locallyUsedHost, ":", impl->port)) {
    KJ_LOG(INFO, "connecting to local server");
    if (!url.path.empty()) return restoreSR(impl->serverMainInterface, url.path[0], ownerGuid);
    else return {impl->serverMainInterface};
  }

  //parse port out of host address because under windows parseAddress below seams to have problems
  //with the host address containing the port
  //do both, let kj parse the host address for a port, but additionally provide the port hint
  //kj::String address;
  kj::uint port = 0;
  KJ_IF_MAYBE (colonPos, url.host.findFirst(':')) {
    //address = kj::str(addressPort.slice(0, *colonPos));
    port = url.host.slice(*colonPos + 1).parseAs<kj::uint>();
  }

  // is a host port resolver
  if (bootstrapInterfaceId == 0xaa8d91fab6d01d9f) {
    auto bsUrl = url.clone();
    bsUrl.path.clear();
    bsUrl.query.clear();
    kj::StringPtr aliasOrBase64VatId;
    KJ_IF_MAYBE(info, bsUrl.userInfo) {
      aliasOrBase64VatId = info->username;
    }

    return connect(kj::mv(bsUrl)).then(
        [KJ_MVCAP(aliasOrBase64VatId), KJ_MVCAP(url), KJ_MVCAP(ownerGuid), connectTo](
            capnp::Capability::Client &&resolverCap) mutable {
          auto client = resolverCap.castAs<mas::schema::persistence::HostPortResolver>();
          auto req = client.resolveRequest();
          req.setId(aliasOrBase64VatId);
          return req.send().then([KJ_MVCAP(ownerGuid), connectTo, KJ_MVCAP(url)](
              auto &&resp) mutable {
            return connectTo(kj::mv(url), kj::str(resp.getHost(), ":", resp.getPort()), resp.getPort(), ownerGuid);
          });
        });
  }
  //else
  return connectTo(kj::mv(url), url.host, port, ownerGuid);
}

kj::Promise<capnp::Capability::Client> ConnectionManager::connect(kj::StringPtr sturdyRefStr) {
  KJ_IF_MAYBE(url, kj::Url::tryParse(sturdyRefStr)) {
    if (url->scheme != "capnp") return nullptr;
    return connect(kj::mv(*url));
  }
  return nullptr;
}

kj::Promise<kj::uint> ConnectionManager::bind(capnp::Capability::Client mainInterface,
                                              kj::StringPtr host, kj::uint port) {
  impl->serverMainInterface = mainInterface;

  auto portPaf = kj::newPromiseAndFulfiller<kj::uint>();

  auto &network = impl->ioContext->provider->getNetwork();
  auto bindAddress = kj::str(host, port == 0 ? kj::str("") : kj::str(":", port));

  impl->tasks.add(network.parseAddress(bindAddress, port).then(
      [portFulfiller = kj::mv(portPaf.fulfiller), this](kj::Own<kj::NetworkAddress> &&addr) mutable {
        auto listener = addr->listen();
        impl->port = listener->getPort();
        auto port = impl->port;
        portFulfiller->fulfill(kj::mv(port));
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
  if ((sockfd = (int) socket(AF_INET, SOCK_STREAM, 0)) < 0) {
    return kj::tuple(false, kj::str("Can't open stream socket."));
  }

  // Set server_addr
  memset(&server_addr, 0, sizeof(server_addr));
  server_addr.sin_family = AF_INET;
  server_addr.sin_addr.s_addr = inet_addr(connectToHost.cStr());
  server_addr.sin_port = htons(connectToPort);

  // Connect to server
  if (connect(sockfd, (struct sockaddr *) &server_addr, sizeof(server_addr)) < 0) {
    CLOSE_SOCKET(sockfd);
    return kj::tuple(false, kj::str("Can't connect to server (", connectToHost, ":", connectToPort, ")"));
  }

  // Get my ip address and port
  memset(&my_addr, 0, sizeof(my_addr));
  SOCKLEN_T len = sizeof(my_addr);
  getsockname(sockfd, (struct sockaddr *) &my_addr, &len);
  inet_ntop(AF_INET, &my_addr.sin_addr, myIP, sizeof(myIP));
  myPort = ntohs(my_addr.sin_port);
  CLOSE_SOCKET(sockfd);
  return kj::tuple(true, kj::str(myIP));
}