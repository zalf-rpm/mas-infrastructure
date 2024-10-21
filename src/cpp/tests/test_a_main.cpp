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
#include <sstream>
#include <string>
#include <vector>
#include <chrono>
#include <thread>

#include <kj/debug.h>
#include <kj/common.h>
#include <kj/main.h>
#include <kj/string.h>

#include <capnp/ez-rpc.h>

#include "common/restorable-service-main.h"
#include "common/common.h"
#include "test/a.capnp.h"

using RSM = mas::infrastructure::common::RestorableServiceMain;

class AServ final : public mas::rpc::test::A::Server
{
public:
  AServ() {}

  virtual ~AServ() noexcept(false) {}

  kj::Promise<void> method(MethodContext context) override {
    std::stringstream oss;
    for(int i = 0; i < 10000; i++) oss << '.';
    context.getResults().setRes(oss.str());
    return kj::READY_NOW;
  }

  int i{0};
};

class AMain : public RSM
{
public:
  AMain(kj::ProcessContext& context) 
    : RSM(context, "Test Server v", "")
  {}

  kj::MainBuilder::Validity setSRT(kj::StringPtr name) { srt = kj::str(name); return true; }

  kj::MainBuilder::Validity start()
  {
    KJ_LOG(INFO, "starting A");

    mas::rpc::test::A::Client a = kj::heap<AServ>();
    auto ownedRestorer = kj::heap<mas::infrastructure::common::Restorer>();
    restorer = ownedRestorer.get();
    conMan = kj::heap<mas::infrastructure::common::ConnectionManager>(ioContext, restorer);
    restorerClient = kj::mv(ownedRestorer);
    auto portPromise = conMan->bind(restorerClient, host, 9999);
    portPromise.then([this](auto port){
      return restorer->setPort(port).then([port](){
                                            return port;
                                          }, [](auto&& e){
                                            KJ_LOG(ERROR, "Error while trying to set port.", e);
                                            return 0;
                                          }
      );
    }).wait(ioContext.waitScope);
    auto sr = restorer->saveStr(a, "aaaa", nullptr, false).wait(ioContext.waitScope).sturdyRef;
    std::cout << "sr=" << sr.cStr() << std::endl;

    // Run forever, accepting connections and handling requests.
    kj::NEVER_DONE.wait(ioContext.waitScope);

    KJ_LOG(INFO, "stopping A");
    return true;
  }

  kj::MainFunc getMain()
  {
    return addRestorableServiceOptions()
        .addOptionWithArg({'t', "srt"}, KJ_BIND_METHOD(*this, setSRT),
                          "<sturdy-ref token>", "Set a fixed sturdy ref token.")
        .callAfterParsing(KJ_BIND_METHOD(*this, start))
        .build();
  }

private:
  kj::String srt;
};

KJ_MAIN(AMain)
