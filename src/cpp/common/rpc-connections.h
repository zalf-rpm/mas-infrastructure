/* This Source Code Form is subject to the terms of the Mozilla Public
* License, v. 2.0. If a copy of the MPL was not distributed with this
* file, You can obtain one at http://mozilla.org/MPL/2.0/. */

/*
Authors:
Michael Berg <michael.berg-mohnicke@zalf.de>

Maintainers:
Currently maintained by the authors.

This file is part of the MONICA model.
Copyright (C) Leibniz Centre for Agricultural Landscape Research (ZALF)
*/

#pragma once

#include <string>

#include <capnp/message.h>
#include <capnp/rpc-twoparty.h>
#include <kj/async-io.h>
#include <kj/map.h>

//#include <kj/debug.h>
//#include <kj/common.h>
//#define KJ_MVCAP(var) var = kj::mv(var)

namespace mas {
	namespace infrastructure {
		namespace common {

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

			class ConnectionManager {

				struct ErrorHandler : public kj::TaskSet::ErrorHandler {
					void taskFailed(kj::Exception&& exception) override {
						kj::throwFatalException(kj::mv(exception));
					}
				};

			public:
				ConnectionManager() : tasks(eh) {}

				kj::Promise<capnp::Capability::Client> connect(kj::AsyncIoContext& ioContext, std::string sturdyRefStr);

				std::pair<kj::Promise<std::string>, kj::Promise<kj::uint>> bind(kj::AsyncIoContext& ioContext, capnp::Capability::Client mainInterface, std::string address, kj::uint port = 0U);

			private:
				void acceptLoop(kj::Own<kj::ConnectionReceiver>&& listener, capnp::ReaderOptions readerOpts);

				ErrorHandler eh;
				kj::TaskSet tasks;
				kj::HashMap<kj::String, kj::Own<ClientContext>> _connections;
				capnp::Capability::Client _serverMainInterface{ nullptr };
			};
		}
	}

	

}
