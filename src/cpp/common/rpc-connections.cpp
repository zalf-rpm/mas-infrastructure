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

#include <iostream>
#include <string>
#include <tuple>
#include <vector>
#include <algorithm>

#define KJ_MVCAP(var) var = kj::mv(var)

#include <capnp/ez-rpc.h>
#include <capnp/message.h>
#include <capnp/rpc-twoparty.h>
#include <kj/async-io.h>
//#include <kj/thread.h>

#include "tools/debug.h"

#include "rpc-connections.h"

//#include "model.capnp.h"
#include "common.capnp.h"
#include "persistence.capnp.h"

//#include "common/sole.hpp"

using namespace std;
using namespace Tools;
using namespace mas;
using namespace mas::rpc::persistence;
using namespace capnp;
using namespace mas::infrastructure::common;

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
							auto restorerCap = bootstrapCap.castAs<Restorer>();
							auto req = restorerCap.restoreRequest();
							req.setSrToken(srToken);
							return req.send().then([](auto&& res) { return res.getCap(); });
						}
						return kj::Promise<Capability::Client>(bootstrapCap);
					}
        );
			}
		}
	}

	return nullptr;
}


kj::Promise<kj::uint> ConnectionManager::bind(kj::AsyncIoContext& ioContext, 
	capnp::Capability::Client mainInterface, std::string address, kj::uint port) 	{

	_serverMainInterface = mainInterface;

	auto paf = kj::newPromiseAndFulfiller<uint>();
	kj::ForkedPromise<uint> portPromise = paf.promise.fork();

	auto& network = ioContext.provider->getNetwork();
	auto bindAddress = address + (port < 0 ? "" : string(":") + to_string(port));
	//uint defaultPort = 0;

	auto&& portFulfiller = paf.fulfiller;
	tasks.add(network.parseAddress(bindAddress, port)
		.then([KJ_MVCAP(portFulfiller), this](kj::Own<kj::NetworkAddress>&& addr) mutable {
		auto listener = addr->listen();
		portFulfiller->fulfill(listener->getPort());
		acceptLoop(kj::mv(listener), capnp::ReaderOptions());
	}));

	return portPromise.addBranch();
}

void ConnectionManager::acceptLoop(kj::Own<kj::ConnectionReceiver>&& listener, capnp::ReaderOptions readerOpts) {
	auto ptr = listener.get();
	tasks.add(ptr->accept().then(kj::mvCapture(kj::mv(listener),
		[readerOpts, this](kj::Own<kj::ConnectionReceiver>&& listener,
			kj::Own<kj::AsyncIoStream>&& connection) {
				acceptLoop(kj::mv(listener), readerOpts);

				cout << "connection from client" << endl;
				auto server = kj::heap<ServerContext>(kj::mv(connection), readerOpts, _serverMainInterface);

				// Arrange to destroy the server context when all references are gone, or when the
				// EzRpcServer is destroyed (which will destroy the TaskSet).
				tasks.add(server->network.onDisconnect().attach(kj::mv(server)));
		})));
}