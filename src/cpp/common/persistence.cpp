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

#include "persistence.h"

//#include "model.capnp.h"
#include "common.capnp.h"
#include "persistence.capnp.h"

//#include "common/sole.hpp"

using namespace std;
using namespace Tools;
using namespace mas;
using namespace mas::rpc::Persistence;
using namespace capnp;
using namespace Persistence;

capnp::Capability::Client Persistence::resolveSturdyRefez(std::string sturdyRefStr) {

	if (sturdyRefStr.substr(0, 8) == "capnp://") {
		auto atPos = sturdyRefStr.find_first_of("@", 8);
		auto hashDigest = sturdyRefStr.substr(8, atPos);
		auto slashPos = sturdyRefStr.find_first_of("@", atPos == string::npos ? 8 : atPos);
		auto addressPort = sturdyRefStr.substr(atPos == string::npos ? 8 : atPos, slashPos);
		auto srToken = slashPos == string::npos ? "" : sturdyRefStr.substr(slashPos);
	
		capnp::EzRpcClient client(addressPort);
		auto& waitScope = client.getWaitScope();
		Restorer<capnp::Text>::Client restorerCap = client.getMain<Restorer<capnp::Text>>();
		if (!srToken.empty()) {
			auto req = restorerCap.restoreRequest();
			req.setSrToken(srToken);
			auto cap = req.send().wait(waitScope).getCap();

			return cap;
		}

		return restorerCap;
	}

	return nullptr;
}

kj::Promise<capnp::Capability::Client> ConnectionManager::connect(kj::AsyncIoContext& ioc, std::string sturdyRefStr) {
	capnp::ReaderOptions readerOpts;

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
				Capability::Client c = clientContext->get()->bootstrap;

				if (!srToken.empty()) {
					auto restorerClient = c.castAs<Restorer<capnp::Text>>();
					auto req = restorerClient.restoreRequest();
          req.setSrToken(srToken);
          return req.send().then([](auto&& res) { return res.getCap(); });
				}
				return c;
			} else {
				return ioc.provider->getNetwork().parseAddress(addressPort).then(
					[](kj::Own<kj::NetworkAddress>&& addr) {
						return addr->connect().attach(kj::mv(addr));
					}
				).then(
					[readerOpts, this, addressPort, srToken](kj::Own<kj::AsyncIoStream>&& stream) {
						auto cc = kj::heap<ClientContext>(kj::mv(stream), readerOpts);
						Capability::Client bootstrapCap = cc->getMain();
						cc.get()->bootstrap = bootstrapCap;
						_connections.insert(kj::str(addressPort), kj::mv(cc));
						//return booststrapCap;

						if (!srToken.empty()) {
							auto restorerCap = bootstrapCap.castAs<Restorer<capnp::Text>>();
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




kj::Promise<capnp::Capability::Client> Persistence::resolveSturdyRef(std::string sturdyRefStr, kj::Network& network, capnp::ReaderOptions readerOpts) {

	if (sturdyRefStr.substr(0, 8) == "capnp://") {
		auto atPos = sturdyRefStr.find_first_of("@", 8);
		auto hashDigest = sturdyRefStr.substr(8, atPos);
		auto slashPos = sturdyRefStr.find_first_of("@", atPos == string::npos ? 8 : atPos);
		auto addressPort = sturdyRefStr.substr(atPos == string::npos ? 8 : atPos, slashPos);
		auto srToken = slashPos == string::npos ? "" : sturdyRefStr.substr(slashPos);

		/*
    kj::ForkedPromise<kj::Own<ClientContext>> setupPromise(
      network.parseAddress(addressPort)
      .then([](kj::Own<kj::NetworkAddress>&& addr) {
        return addr->connect().attach(kj::mv(addr));
        }).then([readerOpts](kj::Own<kj::AsyncIoStream>&& stream) {
          return kj::mv(kj::heap<ClientContext>(kj::mv(stream), readerOpts));
          }).fork());
					*/

    auto setupPromise = network.parseAddress(addressPort).then(
      [](kj::Own<kj::NetworkAddress>&& addr) {
        return addr->connect().attach(kj::mv(addr));
      }
    ).then(
      [readerOpts](kj::Own<kj::AsyncIoStream>&& stream) {
        return kj::mv(kj::heap<ClientContext>(kj::mv(stream), readerOpts));
      }
      );

      auto bootstrapCapPromise = setupPromise.then(
      [](kj::Own<ClientContext>&& context) {
        return context->getMain();
      }
    );

    if (!srToken.empty()) {

      auto restorerClientPromise = bootstrapCapPromise.then(
        [](Capability::Client bootstrapCap) {
          return bootstrapCap.castAs<Restorer<capnp::Text>>();
        }
      );

			auto restorerCapPromise = restorerClientPromise.then(
				[srToken](Restorer<capnp::Text>::Client cap) {
					auto req = cap.restoreRequest();
					req.setSrToken(srToken);
					return req.send().then([](auto&& res) { return res.getCap(); });
				}
			);

			return restorerCapPromise;
    }

		return bootstrapCapPromise;
	}

	return nullptr;
}
