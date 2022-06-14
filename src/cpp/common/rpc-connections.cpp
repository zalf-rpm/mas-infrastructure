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

#include <iostream>
#include <string>
#include <tuple>
#include <vector>
#include <algorithm>
#include <chrono>
#include <thread>

#ifdef WIN32
//#include <winsock.h>
#else
//#include <sys/socket.h>
#endif

#define KJ_MVCAP(var) var = kj::mv(var)

#include <capnp/ez-rpc.h>
#include <capnp/message.h>
#include <capnp/rpc-twoparty.h>

#include <kj/async-io.h>
#include <kj/debug.h>
//#include <kj/thread.h>

#include "common.capnp.h"
#include "persistence.capnp.h"

//#include "common/sole.hpp"

using namespace std;
using namespace mas;
using namespace mas::schema::persistence;
using namespace capnp;
using namespace mas::infrastructure::common;

kj::Promise<capnp::Capability::Client> ConnectionManager::tryConnect(kj::AsyncIoContext& ioc, std::string sturdyRefStr,
	int retryCount, int retrySecs, bool printRetryMsgs){
	try {
		return connect(ioc, sturdyRefStr);
	} catch(std::exception e) {
		if(!_timer) _timer = &(ioc.provider->getTimer());
		return _timer->afterDelay(retrySecs*kj::SECONDS).then([&]() {
			if(retryCount == 0) {
				if(printRetryMsgs) KJ_LOG(DBG, "Couldn't connect to sturdy_ref at", sturdyRefStr, "!");
					return kj::Promise<capnp::Capability::Client>(nullptr);
			}
			retryCount -= 1;
			if(printRetryMsgs) KJ_LOG(DBG, "Trying to connect to", sturdyRefStr, "again in", retrySecs, "s!");
			retrySecs += 1;
			return tryConnect(ioc, sturdyRefStr);
		});
	}
}

capnp::Capability::Client ConnectionManager::tryConnectB(kj::AsyncIoContext& ioc, std::string sturdyRefStr,
	int retryCount, int retrySecs, bool printRetryMsgs){
	while(true)
	{
		try {
			return connect(ioc, sturdyRefStr).wait(ioc.waitScope);
		} catch(std::exception e) {
			if(retryCount == 0) {
				if(printRetryMsgs) KJ_LOG(DBG, "Couldn't connect to sturdy_ref at", sturdyRefStr, "!");
					return nullptr;
			}
			std::this_thread::sleep_for(std::chrono::milliseconds(retrySecs*1000));
			retryCount -= 1;
			if(printRetryMsgs) KJ_LOG(DBG, "Trying to connect to", sturdyRefStr, "again in", retrySecs, "s!");
			retrySecs += 1;
		}
	}
}


kj::Promise<capnp::Capability::Client> ConnectionManager::connect(kj::AsyncIoContext& ioc, std::string sturdyRefStr) {
	capnp::ReaderOptions readerOpts;

	// we assume that a sturdy ref url looks always like capnp://hash-digest-or-insecure@host:port/sturdy-ref-token
	if (sturdyRefStr.substr(0, 8) == "capnp://") {
		auto atPos = sturdyRefStr.find_first_of("@", 8);
		if (atPos == string::npos) //unix domain sockets unimplemented
			return nullptr; 
		auto hashDigest = sturdyRefStr.substr(8, atPos - 8);
		auto slashPos = sturdyRefStr.find_first_of("/", atPos);
		auto addressPort = sturdyRefStr.substr(atPos + 1, slashPos == string::npos ? slashPos : slashPos - atPos - 1);
		auto srToken = slashPos == string::npos ? "" : sturdyRefStr.substr(slashPos + 1);

		if (!addressPort.empty()) {
			KJ_IF_MAYBE(clientContext, _connections.find(addressPort)) {
				Capability::Client bootstrapCap = (*clientContext)->bootstrap;

				if (!srToken.empty()) {
					auto restorerClient = bootstrapCap.castAs<Restorer>();
					auto req = restorerClient.restoreRequest();
					req.setSrToken(srToken);
					return req.send().then([](auto&& res) { return res.getCap(); });
				} 
				return bootstrapCap;
			} else {
				return ioc.provider->getNetwork().parseAddress(addressPort).then(
					[](kj::Own<kj::NetworkAddress>&& addr) {
						return addr->connect().attach(kj::mv(addr));
					}
				).then(
					[readerOpts, this, addressPort, srToken](kj::Own<kj::AsyncIoStream>&& stream) {
						auto cc = kj::heap<ClientContext>(kj::mv(stream), readerOpts);
						Capability::Client bootstrapCap = cc->getMain();
						cc->bootstrap = bootstrapCap;
						_connections.insert(kj::str(addressPort), kj::mv(cc));

						if (!srToken.empty()) {
							KJ_LOG(INFO, "ConnectionManager::connect: restoring token", srToken);
							auto restorerCap = bootstrapCap.castAs<Restorer>();
							auto req = restorerCap.restoreRequest();
							req.setSrToken(srToken);
							KJ_LOG(INFO, "ConnectionManager::connect: making restore request");
							return req.send().then([](auto&& res) { 
								KJ_LOG(INFO, "ConnectionManager::connect: send returned");
								return res.getCap(); 
							});
						}
						return kj::Promise<Capability::Client>(bootstrapCap);
					}
        		);
			}
		}
	}

	return nullptr;
}


std::pair<kj::Promise<std::string>, kj::Promise<kj::uint>> ConnectionManager::bind(kj::AsyncIoContext& ioContext, 
	capnp::Capability::Client mainInterface, std::string address, kj::uint port) 	{

	_serverMainInterface = mainInterface;

	auto portPaf = kj::newPromiseAndFulfiller<uint>();
	kj::ForkedPromise<uint> portPromise = portPaf.promise.fork();

	auto ipPaf = kj::newPromiseAndFulfiller<string>();
	kj::ForkedPromise<string> ipPromise = ipPaf.promise.fork();

	auto& network = ioContext.provider->getNetwork();
	auto bindAddress = address + (port < 0 ? "" : string(":") + to_string(port));
	//uint defaultPort = 0;

	auto&& portFulfiller = portPaf.fulfiller;
	auto&& ipFulfiller = ipPaf.fulfiller;
	tasks.add(network.parseAddress(bindAddress, port)
		.then([KJ_MVCAP(portFulfiller), KJ_MVCAP(ipFulfiller), this](kj::Own<kj::NetworkAddress>&& addr) mutable {
		auto listener = addr->listen();
		portFulfiller->fulfill(listener->getPort());
		//sockaddr saddr;
		//kj::uint socklen;
		//listener->getsockname(&saddr, &socklen);
		ipFulfiller->fulfill("unknown");// saddr.sa_data);
		acceptLoop(kj::mv(listener), capnp::ReaderOptions());
	}));

	return make_pair(ipPromise.addBranch(), portPromise.addBranch());
}

void ConnectionManager::acceptLoop(kj::Own<kj::ConnectionReceiver>&& listener, capnp::ReaderOptions readerOpts) {
	auto ptr = listener.get();
	tasks.add(ptr->accept().then(kj::mvCapture(kj::mv(listener),
		[readerOpts, this](kj::Own<kj::ConnectionReceiver>&& listener,
			kj::Own<kj::AsyncIoStream>&& connection) {
				acceptLoop(kj::mv(listener), readerOpts);

				KJ_LOG(INFO, "ConnectionManager::acceptLoop: connection from client");
				auto server = kj::heap<ServerContext>(kj::mv(connection), readerOpts, _serverMainInterface);

				// Arrange to destroy the server context when all references are gone, or when the
				// EzRpcServer is destroyed (which will destroy the TaskSet).
				tasks.add(server->network.onDisconnect().attach(kj::mv(server)));
		})));
}