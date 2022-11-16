/* This Source Code Form is subject to the terms of the Mozilla Public
* License, v. 2.0. If a copy of the MPL was not distributed with this
* file, You can obtain one at http://mozilla.org/MPL/2.0/. */

/*
Authors:
Michael Berg <michael.berg@zalf.de>

Maintainers:
Currently maintained by the authors.

This file is part of the MONICA model.
Copyright (C) Leibniz Centre for Agricultural Landscape Research (ZALF)
*/

#include "rpc-connections.h"

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

#define KJ_MVCAP(var) var = kj::mv(var)

#include <capnp/ez-rpc.h>
#include <capnp/message.h>
#include <capnp/rpc-twoparty.h>

#include <kj/async-io.h>
#include <kj/debug.h>
#include <kj/encoding.h>
//#include <kj/thread.h>

#include "common.capnp.h"
#include "persistence.capnp.h"

//#include "common/sole.hpp"
#include "common.h"

using namespace std;
using namespace mas;
using namespace mas::schema::persistence;
using namespace capnp;
using namespace mas::infrastructure::common;


ConnectionManager::ConnectionManager(Restorer* restorer)
: tasks(eh)
{
	if(restorer) _restorer = kj::Own<Restorer>(restorer, _disposer);
	else _restorer = kj::heap<Restorer>();
}


kj::Promise<capnp::Capability::Client> ConnectionManager::tryConnect(kj::AsyncIoContext& ioc, kj::StringPtr sturdyRefStr,
	int retryCount, int retrySecs, bool printRetryMsgs){
	try {
		return connect(ioc, sturdyRefStr);
	} catch(std::exception e) {
		if(!_timer) _timer = &(ioc.provider->getTimer());
		return _timer->afterDelay(retrySecs*kj::SECONDS).then([&]() {
			if(retryCount == 0) {
				if(printRetryMsgs) KJ_LOG(INFO, "Couldn't connect to sturdy_ref at", sturdyRefStr, "!");
					return kj::Promise<capnp::Capability::Client>(nullptr);
			}
			retryCount -= 1;
			if(printRetryMsgs) KJ_LOG(INFO, "Trying to connect to", sturdyRefStr, "again in", retrySecs, "s!");
			retrySecs += 1;
			return tryConnect(ioc, sturdyRefStr);
		});
	}
}

capnp::Capability::Client ConnectionManager::tryConnectB(kj::AsyncIoContext& ioc, kj::StringPtr sturdyRefStr,
	int retryCount, int retrySecs, bool printRetryMsgs){
	while(true)
	{
		try {
			return connect(ioc, sturdyRefStr).wait(ioc.waitScope);
		} catch(std::exception e) {
			if(retryCount == 0) {
				if(printRetryMsgs) KJ_LOG(INFO, "Couldn't connect to sturdy_ref at", sturdyRefStr, "!");
					return nullptr;
			}
			std::this_thread::sleep_for(std::chrono::milliseconds(retrySecs*1000));
			retryCount -= 1;
			if(printRetryMsgs) KJ_LOG(INFO, "Trying to connect to", sturdyRefStr, "again in", retrySecs, "s!");
			retrySecs += 1;
		}
	}
}


kj::Promise<capnp::Capability::Client> ConnectionManager::connect(kj::AsyncIoContext& ioc, kj::StringPtr sturdyRefStr) {
	capnp::ReaderOptions readerOpts;

	// we assume that a sturdy ref url looks always like 
	// capnp://vat-id_base64-curve25519-public-key@host:port/sturdy-ref-token_base64
	if (sturdyRefStr.startsWith("capnp://")) {
		// right now we only support tcp connections
		KJ_IF_MAYBE(atPos, sturdyRefStr.findFirst('@')) {
			auto vatIdBase64 = kj::str(sturdyRefStr.slice(8, *atPos));
			kj::String addressPort;
			kj::String srTokenBase64;
			KJ_IF_MAYBE(slashPos, sturdyRefStr.findFirst('/')){
				addressPort = kj::str(sturdyRefStr.slice(*atPos + 1, *slashPos));
				srTokenBase64 = kj::str(sturdyRefStr.slice(*slashPos + 1));
			} else {
				addressPort = kj::str(sturdyRefStr.slice(*atPos + 1));
			}

			if (!addressPort.size() == 0) {
				KJ_IF_MAYBE(clientContext, _connections.find(addressPort)) {
					Capability::Client bootstrapCap = (*clientContext)->bootstrap;

					// no token, just return the bootstrap capability
					if (srTokenBase64.size() > 0) {
						auto srTokenArr = kj::decodeBase64(srTokenBase64);
						kj::String srToken = kj::str(srTokenArr.asChars());

						// token has been signed by vat with vatId
						//if(kj::get<0>(_restorer->verifySRToken(srTokenBase64, vatIdBase64))){
						auto restorerClient = bootstrapCap.castAs<mas::schema::persistence::Restorer>();
						auto req = restorerClient.restoreRequest();
						req.initLocalRef().setAs<capnp::Text>(srToken);//Base64);
						return req.send().then([](auto&& res) { return res.getCap(); });
						//} else { // signature doesn't fit, don't trust the token
						//	return nullptr;
						//}
					} 
					return bootstrapCap;
				} else {
					return ioc.provider->getNetwork().parseAddress(addressPort).then(
						[](kj::Own<kj::NetworkAddress>&& addr) {
							return addr->connect().attach(kj::mv(addr));
						}
					).then(
						[readerOpts, this, KJ_MVCAP(addressPort), KJ_MVCAP(srTokenBase64), KJ_MVCAP(vatIdBase64)]
						(kj::Own<kj::AsyncIoStream>&& stream) {
							auto cc = kj::heap<ClientContext>(kj::mv(stream), readerOpts);
							Capability::Client bootstrapCap = cc->getMain();
							cc->bootstrap = bootstrapCap;
							_connections.insert(kj::str(addressPort), kj::mv(cc));

							if (srTokenBase64.size() > 0) {
								KJ_LOG(INFO, "restoring token", srTokenBase64);
								//auto restorerCap = bootstrapCap.castAs<mas::schema::persistence::Restorer>();
								//auto req = restorerCap.restoreRequest();
								//req.initLocalRef().setAs<capnp::Text>(srToken);
								if(kj::get<0>(_restorer->verifySRToken(srTokenBase64, vatIdBase64))){
									KJ_LOG(INFO, "making restore request");
									auto restorerClient = bootstrapCap.castAs<mas::schema::persistence::Restorer>();
									auto req = restorerClient.restoreRequest();
									req.initLocalRef().setAs<capnp::Text>(srTokenBase64);
									return req.send().then([](auto&& res) { 
										KJ_LOG(INFO, "send returned");
										return res.getCap(); 
									});
								} else { // signature doesn't fit, don't trust the token
									return kj::Promise<Capability::Client>(nullptr);
								}
							}
							return kj::Promise<Capability::Client>(bootstrapCap);
						}
					);
				}
			}
		}
	}

	return nullptr;
}


kj::Promise<kj::uint> ConnectionManager::bind(kj::AsyncIoContext& ioContext, 
	capnp::Capability::Client mainInterface, kj::StringPtr host, kj::uint port) 
{
	_serverMainInterface = mainInterface;

	auto portPaf = kj::newPromiseAndFulfiller<uint>();
	//kj::ForkedPromise<uint> portPromise = portPaf.promise.fork();

	auto& network = ioContext.provider->getNetwork();
	auto bindAddress = kj::str(host, port < 0 ? kj::str("") : kj::str(":", port));

	tasks.add(network.parseAddress(bindAddress, port)
		.then([portFulfiller = kj::mv(portPaf.fulfiller), this]
			(kj::Own<kj::NetworkAddress>&& addr) mutable {
		auto listener = addr->listen();
		portFulfiller->fulfill(listener->getPort());
		acceptLoop(kj::mv(listener), capnp::ReaderOptions());
	}));

	return kj::mv(portPaf.promise);
	//return portPromise.addBranch();
}

void ConnectionManager::acceptLoop(kj::Own<kj::ConnectionReceiver>&& listener, capnp::ReaderOptions readerOpts) {
	auto ptr = listener.get();
	tasks.add(ptr->accept().then(kj::mvCapture(kj::mv(listener),
		[readerOpts, this](kj::Own<kj::ConnectionReceiver>&& listener,
			kj::Own<kj::AsyncIoStream>&& connection) {
				acceptLoop(kj::mv(listener), readerOpts);

				KJ_LOG(INFO, "connection from client");
				auto server = kj::heap<ServerContext>(kj::mv(connection), readerOpts, _serverMainInterface);

				// Arrange to destroy the server context when all references are gone, or when the
				// EzRpcServer is destroyed (which will destroy the TaskSet).
				tasks.add(server->network.onDisconnect().attach(kj::mv(server)));
				//tasks.add(server->network.onDisconnect().then([]() { 
				//	KJ_LOG(INFO, "disconnecting ok"); 
				//}, [](auto&& err) { 
				//	KJ_LOG(INFO, "diconnecting error", err);  
				//}).attach(kj::mv(server)));
		})));
}


kj::Tuple<bool, kj::String> 
mas::infrastructure::common::getLocalIP(kj::StringPtr connectToHost, kj::uint connectToPort)
{
	if(connectToHost == "") connectToHost = "8.8.8.8";
	if(connectToPort == 0) connectToPort = 53;

	// taken from https://gist.github.com/listnukira/4045436
	char myIP[16];
	unsigned int myPort;
	struct sockaddr_in server_addr, my_addr;
	int sockfd;

	// Connect to server
	if ((sockfd = (int)socket(AF_INET, SOCK_STREAM, 0)) < 0) return kj::tuple(false, kj::str("Can't open stream socket."));

	// Set server_addr
	memset(&server_addr, 0, sizeof(server_addr));
	server_addr.sin_family = AF_INET;
	server_addr.sin_addr.s_addr = inet_addr(connectToHost.cStr());
	server_addr.sin_port = htons(connectToPort);

	// Connect to server
	if (connect(sockfd, (struct sockaddr*)&server_addr, sizeof(server_addr)) < 0) {
		CLOSE_SOCKET(sockfd);
		return kj::tuple(false, kj::str("Can't connect to server (", connectToHost, ":", connectToPort, ")"));
	}

	// Get my ip address and port
	memset(&my_addr, 0, sizeof(my_addr));
	SOCKLEN_T len = sizeof(my_addr);	
	getsockname(sockfd, (struct sockaddr*)&my_addr, &len);
	inet_ntop(AF_INET, &my_addr.sin_addr, myIP, sizeof(myIP));
	myPort = ntohs(my_addr.sin_port);
	CLOSE_SOCKET(sockfd);
	return kj::tuple(true, kj::str(myIP));
}