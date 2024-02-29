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
#include <vector>

#include <kj/common.h>
#include <kj/debug.h>
#include <kj/main.h>
#include <kj/string.h>
#include <kj/thread.h>
#include <kj/tuple.h>

#include <capnp/rpc-twoparty.h>

#include "common/rpc-connection-manager.h"
#include "common/common.h"
#include "x.capnp.h"

class ReaderSrv final : public Y::Server
{
public:
  ReaderSrv() {}

  virtual ~ReaderSrv() noexcept(false) {}

  kj::Promise<void> m(MContext context) override {
    auto hello = context.getParams().getHello();
    if (i % 10000 == 0) { std::cout << "."; std::cout.flush(); }
    i++;
    if (hello == "done") exit(0);
    return kj::READY_NOW;
  }

  int i{0};
};

int i = 0;
void m(std::string hello)  {
    if (i % 10000 == 0) { std::cout << "."; std::cout.flush(); }
    i++;
    if (hello == "done") exit(0);
}

kj::AsyncIoProvider::PipeThread runServer(kj::AsyncIoProvider& ioProvider) {
	return ioProvider.newPipeThread([](kj::AsyncIoProvider& ioProvider, kj::AsyncIoStream& stream, kj::WaitScope& waitScope) {
			capnp::TwoPartyVatNetwork network(stream, capnp::rpc::twoparty::Side::SERVER);
			auto server = makeRpcServer(network, kj::heap<ReaderSrv>());
			network.onDisconnect().wait(waitScope);
		});
}

struct ThreadContext {
	kj::AsyncIoProvider::PipeThread serverThread;
	capnp::TwoPartyVatNetwork network;
	capnp::RpcSystem<capnp::rpc::twoparty::VatId> rpcSystem;

	ThreadContext(kj::AsyncIoProvider::PipeThread&& serverThread)
		: serverThread(kj::mv(serverThread))
		, network(*this->serverThread.pipe, capnp::rpc::twoparty::Side::SERVER)
		, rpcSystem(makeRpcClient(network)) {
	}
};


kj::Tuple<kj::ForkedPromise<void>, Y::Client> createYThread(kj::AsyncIoProvider& ioProvider) {
	auto serverThread = runServer(ioProvider);
	auto tc = kj::heap<ThreadContext>(kj::mv(serverThread));

	capnp::MallocMessageBuilder vatIdMessage(8);
	auto vatId = vatIdMessage.initRoot<capnp::rpc::twoparty::VatId>();
	vatId.setSide(capnp::rpc::twoparty::Side::CLIENT);
	auto client = tc->rpcSystem.bootstrap(vatId).castAs<Y>();

	auto prom = tc->network.onDisconnect().attach(kj::mv(tc));
	return kj::tuple(prom.fork(), kj::mv(client));
}

class TestReaderWriterMain
{
public:
  TestReaderWriterMain(kj::ProcessContext &context) : context(context), ioContext(kj::setupAsyncIo()) {}

  kj::MainBuilder::Validity setName(kj::StringPtr n) { name = n; return true; }

  kj::MainBuilder::Validity setMode(kj::StringPtr name) { mode = name; return true; }

  kj::MainBuilder::Validity setCount(kj::StringPtr name) { count = std::stoi(name.cStr()); return true; }

  kj::MainBuilder::Validity startWriter()
  {
    KJ_LOG(INFO, "TestWriterSrvMain::startWriter: starting writer");
    
    
    if (mode == "function") 
    {
      for(auto i = 1; i <= count; i++) m("Hello_" + std::to_string(i));
      m("done");
    } 
    else 
    {
      Y::Client outp(nullptr);
      kj::Promise<void> prom(nullptr);

      if (mode == "single_threaded_client") 
      {
        outp = kj::heap<ReaderSrv>();
      } 
      else if (mode == "multi_threaded_client") 
      {
        auto promAndClient = createYThread(*ioContext.provider);
        outp = kj::mv(kj::get<1>(promAndClient));
        prom = kj::get<0>(promAndClient).addBranch();
      }

      for(auto i = 1; i <= count; i++)
      {
        auto wreq = outp.mRequest();
        wreq.setHello("Hello_" + std::to_string(i));
        wreq.send().wait(ioContext.waitScope);
      }

      auto wreq = outp.mRequest();
      wreq.setHello("done");
      wreq.send().wait(ioContext.waitScope);
    }

    KJ_LOG(INFO, "TestReaderWriterMain::startWriter: stopping writer");
    return true;
  }

  kj::MainFunc getMain()
  {
    return kj::MainBuilder(context, "Test writer v0.1", "writer to test c++ channel")
      .addOptionWithArg({'n', "name"}, KJ_BIND_METHOD(*this, setName),
                        "<writer-name>", "Give writer a name.")
      .addOptionWithArg({'c', "count"}, KJ_BIND_METHOD(*this, setCount),
                        "<message-count>", "Set how many messages should be sent.")
      .expectArg("<function | single_thread_client | multi_thread_client>", KJ_BIND_METHOD(*this, setMode))
      .callAfterParsing(KJ_BIND_METHOD(*this, startWriter))
      .build();
  }

private:
  mas::infrastructure::common::ConnectionManager conMan;
  kj::StringPtr name;
  kj::StringPtr mode;
  int count{0};
  kj::ProcessContext &context;
  kj::AsyncIoContext ioContext;
};

KJ_MAIN(TestReaderWriterMain)
