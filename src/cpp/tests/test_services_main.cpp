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
#include <chrono>
#include <thread>

#include <kj/debug.h>
#include <kj/common.h>
#include <kj/main.h>
#include <kj/string.h>

#include <capnp/ez-rpc.h>

#include "common/rpc-connection-manager.h"
#include "common/common.h"
#include "storage.capnp.h"
#include "management.capnp.h"
#include "registry.capnp.h"
#include "x.capnp.h"

class TestServicesMain
{
public:
  TestServicesMain(kj::ProcessContext &context) 
  : context(context), 
  ioContext(kj::setupAsyncIo()) 
  {}

  kj::MainBuilder::Validity setName(kj::StringPtr n) { name = kj::str(n); return true; }

  kj::MainBuilder::Validity setSr(kj::StringPtr sr) { this->sr = kj::str(sr); return true; }

  kj::MainBuilder::Validity startTest()
  {
    KJ_LOG(INFO, "starting");

    auto a = conMan.tryConnectB(ioContext, sr).castAs<A>();

    //capnp::EzRpcClient client("localhost", 9999);
    //auto& waitScope = client.getWaitScope();
    //A::Client a = client.getMain<A>();

    auto mReq = a.mRequest();
    mReq.setN(5);
    std::cout << "res: " << mReq.send().wait(ioContext.waitScope).getR() << std::endl;

    std::cout << std::endl;

    //auto registry = conMan.tryConnectB(ioContext, sr.asPtr()).castAs<mas::schema::registry::Registry>();
    //auto info = registry.infoRequest().send().wait(ioContext.waitScope);
    //KJ_LOG(INFO, info.getId(), info.getName());

    // auto req = store.newContainerRequest();
    // req.setName("test-container");
    // req.setDescription("test container descr");
    // auto newContainerP = req.send().wait(ioContext.waitScope);
    // auto newContainer = newContainerP.getContainer();
    // auto cinfo = newContainer.infoRequest().send().wait(ioContext.waitScope);
    // KJ_LOG(INFO, cinfo.getId(), cinfo.getName());

    // {
    //   auto oreq = newContainer.addEntryRequest();
    //   oreq.setKey("test-key");
    //   auto val = oreq.initValue();
    //   val.setTextValue("test text value");
    //   auto oprom = oreq.send().wait(ioContext.waitScope);
    //   auto succ = oprom.getSuccess();
    //   KJ_LOG(INFO, succ);
    // }
    
    // auto greq = newContainer.getEntryRequest();
    // greq.setKey("test-key");
    // auto gprom = greq.send().wait(ioContext.waitScope);
    // auto entry = gprom.getEntry();
    // KJ_LOG(INFO, entry.getKeyRequest().send().wait(ioContext.waitScope).getKey(), entry.getValueRequest().send().wait(ioContext.waitScope).getValue().getTextValue());

    //std::this_thread::sleep_for(std::chrono::milliseconds(10));

    KJ_LOG(INFO, "stopping");
    return true;
  }

  kj::MainFunc getMain()
  {
    return kj::MainBuilder(context, "Test services v0.1", "test services")
      .addOptionWithArg({'n', "name"}, KJ_BIND_METHOD(*this, setName),
                        "<name>", "Give test a name.")
      .expectArg("<sturdy_ref>", KJ_BIND_METHOD(*this, setSr))
      .callAfterParsing(KJ_BIND_METHOD(*this, startTest))
      .build();
  }

private:
  mas::infrastructure::common::ConnectionManager conMan;
  kj::String name;
  kj::String sr;
  kj::ProcessContext &context;
  kj::AsyncIoContext ioContext;
};

KJ_MAIN(TestServicesMain)
