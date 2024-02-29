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
#include <kj/async-io.h>

//#include "common/rpc-connection-manager.h"
//#include "common/common.h"
#include "x.capnp.h"

class AServ final : public A::Server
{
public:
  AServ() {}

  virtual ~AServ() noexcept(false) {}

  kj::Promise<void> m(MContext context) override {
    //auto hello = context.getParams().getHello();
    //if (i % 10000 == 0) { std::cout << "."; std::cout.flush(); }
    //i++;
    //if (hello == "done") exit(0);
    context.getResults().setR(context.getParams().getN() + 0.1);
    return kj::READY_NOW;
  }

  int i{0};
};

class AMain
{
public:
  AMain(kj::ProcessContext& context) 
    : context(context)
    , ioContext(kj::setupAsyncIo()) 
  {}

  kj::MainBuilder::Validity setHost(kj::StringPtr name) { host = kj::str(name); return true; }

  kj::MainBuilder::Validity setPort(kj::StringPtr name) { port = std::max(0, std::stoi(name.cStr())); return true; }

  kj::MainBuilder::Validity start()
  {
    KJ_LOG(INFO, "starting A");

    A::Client a = kj::heap<AServ>();
    
    //capnp::EzRpcServer server(a, kj::str("*"), 9999);
    //auto& waitScope = server.getWaitScope();
    //kj::NEVER_DONE.wait(waitScope);
    
    //auto portProm = conMan.bind(ioContext, kj::mv(a), host, 9999);
    //std::cout << portProm.wait(ioContext.waitScope) << std::endl;
    kj::NEVER_DONE.wait(ioContext.waitScope);

    KJ_LOG(INFO, "stopping A");
    return true;
  }

  kj::MainFunc getMain()
  {
    return kj::MainBuilder(context, "Test A", "")
      .addOptionWithArg({'h', "host"}, KJ_BIND_METHOD(*this, setHost),
                        "<host-IP>", "Set host IP.")
      .addOptionWithArg({'p', "port"}, KJ_BIND_METHOD(*this, setPort),
                        "<port>", "Set port.")
      .callAfterParsing(KJ_BIND_METHOD(*this, start))
      .build();
  }

private:
  //mas::infrastructure::common::ConnectionManager conMan;
  kj::String host{ kj::str("*") };
  int port{0};
  kj::ProcessContext &context;
  kj::AsyncIoContext ioContext;
};

KJ_MAIN(AMain)
