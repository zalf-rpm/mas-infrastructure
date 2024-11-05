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
#include "test/x.capnp.h"
#include "climate/climate.capnp.h"
#include "soil/soil.capnp.h"
#include "model/model.capnp.h"

using RSM = mas::infrastructure::common::RestorableServiceMain;
using ST = mas::schema::common::StructuredText;

class Y final : public mas::schema::test::Y::Server
{
public:
  Y() = default;
  virtual ~Y() noexcept(false) = default;

  kj::Promise<void> m(MContext context) override {
    std::cout << i++ << ": Hello " << context.getParams().getHello().cStr() << std::endl;
    return kj::READY_NOW;
  }

  int i{0};
};

class EnvInstance final : public mas::schema::model::EnvInstance<ST, ST>::Server
{
public:
  EnvInstance() {}

  virtual ~EnvInstance() noexcept(false) {}

  kj::Promise<void> run(RunContext context) override {
    auto env = context.getParams().getEnv();
    auto v = env.getRest().getValue();
    std::cout << "v: " << v.cStr() << std::endl;
    if (env.hasTimeSeries()) {
      auto ts = context.getParams().getEnv().getTimeSeries().castAs<mas::schema::climate::TimeSeries>();
      return ts.infoRequest().send().then([c = kj::mv(context)](auto &&resp) mutable {
        std::cout << "ts id: " << resp.getId().cStr() << std::endl;
        std::stringstream oss;
        for (int i = 0; i < 10000; i++) oss << '.';
        auto rs = c.getResults();
        auto res = rs.initResult();
        res.initStructure().setJson();
        //context.getResults().setRes(oss.str());
        res.setValue(oss.str());
        if (c.getParams().getEnv().hasSoilProfile()){
          auto s = c.getParams().getEnv().getSoilProfile().castAs<mas::schema::soil::Service>();
          return s.infoRequest().send().then([c = kj::mv(c)](auto &&resp) {
            std::cout << "soil id: " << resp.getId().cStr() << std::endl;
          });
        }
        return (kj::Promise<void>)kj::READY_NOW;
      });
    } else {
      std::stringstream oss;
      for (int i = 0; i < 10000; i++) oss << '.';
      auto rs = context.getResults();
      auto res = rs.initResult();
      res.initStructure().setJson();
      //context.getResults().setRes(oss.str());
      res.setValue(oss.str());
      return kj::READY_NOW;
    }
  }

  int i{0};
};

class AServ final : public mas::rpc::test::A<ST, ST>::Server
{
public:
  AServ() {}

  virtual ~AServ() noexcept(false) {}

  kj::Promise<void> method(MethodContext context) override {
    auto param = context.getParams().getParam();
    auto v = param.getRest().getValue();
    std::cout << "v: " << v.cStr() << std::endl;
    if (param.hasTimeSeries()) {
      auto ts = context.getParams().getParam().getTimeSeries().castAs<mas::schema::climate::TimeSeries>();
      return ts.infoRequest().send().then([c = kj::mv(context)](auto &&resp) mutable {
        std::cout << "ts id: " << resp.getId().cStr() << std::endl;
        std::stringstream oss;
        for (int i = 0; i < 10000; i++) oss << '.';
        auto rs = c.getResults();
        auto res = rs.initRes();
        res.initStructure().setJson();
        //context.getResults().setRes(oss.str());
        res.setValue(oss.str());
        if (c.getParams().getParam().hasSoilProfile()){
        auto s = c.getParams().getParam().getSoilProfile().castAs<mas::schema::soil::Service>();
          return s.infoRequest().send().then([c = kj::mv(c)](auto &&resp) {
            std::cout << "soil id: " << resp.getId().cStr() << std::endl;
          });
        }
        return (kj::Promise<void>)kj::READY_NOW;
      });
    } else {
      std::stringstream oss;
      for (int i = 0; i < 10000; i++) oss << '.';
      auto rs = context.getResults();
      auto res = rs.initRes();
      res.initStructure().setJson();
      //context.getResults().setRes(oss.str());
      res.setValue(oss.str());
      return kj::READY_NOW;
    }
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

    mas::schema::test::Y::Client a = kj::heap<Y>();
    //mas::rpc::test::A<ST, ST>::Client a = kj::heap<AServ>();
    //mas::schema::model::EnvInstance<ST, ST>::Client a = kj::heap<EnvInstance>();
    auto ownedRestorer = kj::heap<mas::infrastructure::common::Restorer>();
    restorer = ownedRestorer.get();
    conMan = kj::heap<mas::infrastructure::common::ConnectionManager>(ioContext, restorer);
    restorerClient = kj::mv(ownedRestorer);
    auto portPromise = conMan->bind(restorerClient, host, 9920);
    portPromise.then([this](auto port){
      return restorer->setPort(port).then([port](){
                                            return port;
                                          }, [](auto&& e){
                                            KJ_LOG(ERROR, "Error while trying to set port.", e);
                                            return 0;
                                          }
      );
    }).wait(ioContext.waitScope);
    auto sr = restorer->saveStr(a, "monica", nullptr, false).wait(ioContext.waitScope).sturdyRef;
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
