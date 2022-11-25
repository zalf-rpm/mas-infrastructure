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

#include "cluster_monica_instance_factory.h"

#include <iostream>
#include <fstream>
#include <string>
#include <tuple>
#include <vector>
#include <algorithm>
#include <cstdlib>

#include <kj/debug.h>

#include <kj/common.h>
#define KJ_MVCAP(var) var = kj::mv(var)

#include <capnp/ez-rpc.h>
#include <capnp/message.h>
#include <capnp/any.h>
#include <capnp/dynamic.h>

#include <capnp/rpc-twoparty.h>
#include <kj/thread.h>


#include "tools/debug.h"
#include "tools/date.h"
#include "tools/helper.h"
#include "tools/algorithms.h"
//#include "json11/json11-helper.h"
#include "sole.hpp"

#include "common.h"

#include "model.capnp.h"
#include "common.capnp.h"

//using namespace std;
using namespace monica;
using namespace Tools;
using namespace mas;

kj::HashMap<kj::String, SlurmMonicaInstanceFactory::RegEntry>::Entry emptyRegEntry(kj::String key) {
  return { kj::mv(key), kj::mv(SlurmMonicaInstanceFactory::RegEntry()) };
}

SlurmMonicaInstanceFactory::SlurmMonicaInstanceFactory(kj::String monicaId,
                                                       kj::String pathToMonicaCapnpServerExe,
                                                       kj::String factoryAddress,
                                                       int factoryPort)
: monicaId(kj::mv(monicaId))
, pathToMonicaCapnpServerExe(kj::mv(pathToMonicaCapnpServerExe)) 
, factoryAddress(kj::mv(factoryAddress))
, factoryPort(factoryPort)
{}

SlurmMonicaInstanceFactory::~SlurmMonicaInstanceFactory() noexcept(false) {

}

// registerModelInstance @5 (instance :Capability, registrationToken :Text = "") -> (unregister :Common.Callback);
kj::Promise<void> SlurmMonicaInstanceFactory::registerModelInstance(RegisterModelInstanceContext context) {
  auto instance = context.getParams().getInstance();
  auto regToken = context.getParams().getRegistrationToken();
    
  auto v = splitString(std::string(regToken.cStr()) + ":0", ":");
  auto regToken2 = kj::heapString(v.at(0).c_str());
  auto procId = satoi(v.at(1));

  KJ_IF_MAYBE(reg, registry.find(regToken2)) {
    auto regToken3 = std::string(regToken2.cStr());
    
    instancesRegistered++;
    std::cout << "regs: " << instancesRegistered << std::endl;
    
    capnp::Capability::Client capHolder = kj::heap<CapHolderImpl>(instance, kj::heapString(regToken2), false, regToken3);
    reg->instanceCaps.insert(procId, capHolder);

    rpc::Common::Callback::Client unregCap = kj::heap<CallbackImpl>([this, regToken3]() mutable {
      registry.erase(regToken3);
    }, true, regToken3);
    reg->unregisterCaps.add(unregCap);
    reg->fulFillCount--;
    
    if (reg->fulFillCount == 0) reg->promFulfiller->fulfill();
    
    context.getResults().setUnregister(unregCap);
  }

  return kj::READY_NOW;
}

// modelId @4 () -> (id :Text);
kj::Promise<void> SlurmMonicaInstanceFactory::modelId(ModelIdContext context) {
  context.getResults().setId(monicaId);
  return kj::READY_NOW;
}

// info @0 () -> (info :IdInformation);
kj::Promise<void> SlurmMonicaInstanceFactory::info(InfoContext context) {
  context.initResults();
  auto info = context.getResults().getInfo();
  auto id = sole::uuid4().str();
  info.setId(id);
  info.setName(kj::str("SlurmMonicaInstanceFactory(", id, ")"));
  //info.setDescription("");
  return kj::READY_NOW;
}

// newInstance @0 () -> (instance :Common.CapHolder);
kj::Promise<void> SlurmMonicaInstanceFactory::newInstance(NewInstanceContext context) {
  //kj::String registrationToken = kj::heapString("aaaa");
  kj::String registrationToken = kj::heapString(sole::uuid4().str());

  auto& reg = registry.findOrCreate(registrationToken, [&registrationToken]() {
    return emptyRegEntry(kj::heapString(registrationToken)); 
                                    });
   
#if _WIN32
  auto call = kj::str("start",
                      " \"monica-capnp-server-", instancesStarted, "\"",
                      " ", pathToMonicaCapnpServerExe,
                      " -d",
                      " -cf",
                      " -fa ", factoryAddress, 
                      " -fp ", factoryPort,
                      " -rt ", registrationToken, ":0");
  KJ_SYSCALL(system(call.cStr()));
  instancesStarted++;
  std::cout << "start: " << instancesStarted << std::endl;
#else
  KJ_SYSCALL(system(""));
#endif

  reg.fulFillCount++;

  auto pfp = kj::newPromiseAndFulfiller<void>();
  reg.promFulfiller = kj::mv(pfp.fulfiller);
  
  return pfp.promise.then([context, this, KJ_MVCAP(registrationToken)]() mutable {
    KJ_IF_MAYBE(reg, registry.find(registrationToken)) {
      KJ_IF_MAYBE(cap, reg->instanceCaps.find(0)) {
        context.getResults().setInstance(cap->castAs<rpc::Common::CapHolder<capnp::AnyPointer>>());
      }
    }
  });
}

// newInstances @1 (numberOfInstances :Int16) -> (instances :Common.CapHolder(X));#(Common.CapHolder));
kj::Promise<void> SlurmMonicaInstanceFactory::newInstances(NewInstancesContext context) {
  int instanceCount = context.getParams().getNumberOfInstances();
  kj::String registrationToken = kj::heapString(sole::uuid4().str());
  std::cout << "regToken: |" << registrationToken.cStr() << "|" << std::endl;

  auto& reg = registry.findOrCreate(registrationToken, [&registrationToken]() {
    return emptyRegEntry(kj::heapString(registrationToken));
                                    });

  for (int i = 0; i < instanceCount; i++) {
#if _WIN32
    //KJ_SYSCALL(system(start C:\\Users\\berg.ZALF - AD\\GitHub\\monica\\_cmake_vs2019_win64\\Debug\\monica - capnp - server.exe - i - cf - fa localhost - fp 10000 - rt aaaa : 0"));
    auto call = kj::str("start",
                        " \"monica-capnp-server-", instancesStarted, "\"",
                        " ", pathToMonicaCapnpServerExe,
                        " -d",
                        " -cf",
                        " -fa ", factoryAddress,
                        " -fp ", factoryPort,
                        " -rt ", registrationToken, ":", i);
    KJ_SYSCALL(system(call.cStr()));
    instancesStarted++;
#else
    KJ_SYSCALL(system(""));
#endif

    reg.fulFillCount++;
  }
  std::cout << "start: " << instancesStarted << std::endl;

  auto pfp = kj::newPromiseAndFulfiller<void>();
  reg.promFulfiller = kj::mv(pfp.fulfiller);
  
  return pfp.promise.then([context, this, KJ_MVCAP(registrationToken)]() mutable {
    KJ_IF_MAYBE(reg, registry.find(registrationToken)) {
      kj::Vector<capnp::Capability::Client> caps;
      for (auto& entry : reg->instanceCaps) {
        caps.add(entry.value);
      }
      std::string regToken = registrationToken.cStr();
      capnp::Capability::Client capHolder = kj::heap<CapHolderListImpl>(kj::mv(caps), kj::mv(registrationToken), false, regToken);
      reg->instanceCaps.insert(-1, capHolder);
      auto ch = capHolder.castAs<rpc::Common::CapHolder<capnp::List<rpc::Common::ListEntry<rpc::Common::CapHolder<capnp::AnyPointer>>>>>();
      context.getResults().setInstances(ch);
    }
  });
}

// newCloudViaZmqPipelineProxies @2 (numberOfInstances :Int16) -> (proxyAddresses :Common.CapHolder(Common.ZmqPipelineAddresses));
kj::Promise<void> SlurmMonicaInstanceFactory::newCloudViaZmqPipelineProxies(NewCloudViaZmqPipelineProxiesContext context) {

  return kj::READY_NOW;
}

// newCloudViaProxy @3 (numberOfInstances :Int16) -> (proxy :Common.CapHolder);
kj::Promise<void> SlurmMonicaInstanceFactory::newCloudViaProxy(NewCloudViaProxyContext context) {

  return kj::READY_NOW;
}

// restoreSturdyRef @6 (sturdyRef :Text) -> (cap :Common.CapHolder);
kj::Promise<void> SlurmMonicaInstanceFactory::restoreSturdyRef(RestoreSturdyRefContext context) {
  auto registrationToken = context.getParams().getSturdyRef();
  KJ_IF_MAYBE(reg, registry.find(registrationToken)) {
    rpc::Common::CapHolder<capnp::AnyPointer>::Client cap(nullptr);
    KJ_IF_MAYBE(cap, reg->instanceCaps.size() > 1 
                ? reg->instanceCaps.find(-1)
                : reg->instanceCaps.find(0)) {
      context.getResults().setCap(cap->castAs<rpc::Common::CapHolder<capnp::AnyPointer>>());
    }
  }
  return kj::READY_NOW;
}

//-----------------------------------------------------------------------------



























//std::map<std::string, DataAccessor> daCache;

struct DataAccessor {

};

//*
DataAccessor fromCapnpData(
  const Date& startDate,
  const Date& endDate,
  capnp::List<rpc::Climate::Element>::Reader header,
  capnp::List<capnp::List<float>>::Reader data) {
  //typedef rpc::Climate::Element E;

  if (data.size() == 0)
    return DataAccessor();

  /*
  DataAccessor da(startDate, endDate);
  //vector<double> d(data[0].size());
  for (int i = 0; i < header.size(); i++) {
    auto vs = data[i];
    vector<double> d(data[0].size());
    //transform(vs.begin(), vs.end(), d.begin(), [](float f) { return f; });
    for (int k = 0; k < vs.size(); k++)
      d[k] = vs[k];
    switch (header[i]) {
      case E::TMIN: da.addClimateData(ACD::tmin, std::move(d)); break;
      case E::TAVG: da.addClimateData(ACD::tavg, std::move(d)); break;
      case E::TMAX: da.addClimateData(ACD::tmax, std::move(d)); break;
      case E::PRECIP: da.addClimateData(ACD::precip, std::move(d)); break;
      case E::RELHUMID: da.addClimateData(ACD::relhumid, std::move(d)); break;
      case E::WIND: da.addClimateData(ACD::wind, std::move(d)); break;
      case E::GLOBRAD: da.addClimateData(ACD::globrad, std::move(d)); break;
      default:;
    }
  }
  */

  return DataAccessor();// da;
}
//*/

kj::Promise<void> RunMonicaImpl::info(InfoContext context) //override
{
  auto rs = context.getResults();
  rs.initInfo();
  rs.getInfo().setId("some monica_id");
  rs.getInfo().setName("monica capnp server");
  rs.getInfo().setDescription("some description");
  return kj::READY_NOW;
}

kj::Promise<void> RunMonicaImpl::run(RunContext context) //override
{
  debug() << ".";

  auto envR = context.getParams().getEnv();

  auto runMonica = [context, envR, this](DataAccessor da = DataAccessor()) mutable {
    std::string err;
    auto rest = envR.getRest();
    /*
    if (!rest.getStructure().isJson()) {
      return Monica::Output(string("Error: 'rest' field is not valid JSON!"));
    }

    const Json& envJson = Json::parse(rest.getValue().cStr(), err);
    //cout << "runMonica: " << envJson["customId"].dump() << endl;

    Env env;
    auto errors = env.merge(envJson);

    EResult<DataAccessor> eda;
    if (da.isValid()) {
      eda.result = da;
    } else if (!env.climateData.isValid()) {
      if (!env.climateCSV.empty()) {
        eda = readClimateDataFromCSVStringViaHeaders(env.climateCSV, env.csvViaHeaderOptions);
      } else if (!env.pathsToClimateCSV.empty()) {
        eda = readClimateDataFromCSVFilesViaHeaders(env.pathsToClimateCSV, env.csvViaHeaderOptions);
      }
    }

    monica::Output out;
    if (eda.success()) {
      env.climateData = eda.result;

      env.debugMode = _startedServerInDebugMode && env.debugMode;

      env.params.userSoilMoistureParameters.getCapillaryRiseRate =
        [](string soilTexture, int distance) {
        return Soil::readCapillaryRiseRates().getRate(soilTexture, distance);
      };

      out = monica::runMonica(env);
    }

    out.errors = eda.errors;
    out.warnings = eda.warnings;

    return out;
    */
    return "";
  };

  if (envR.hasTimeSeries()) {
    auto ts = envR.getTimeSeries();
    auto rangeProm = ts.rangeRequest().send();
    auto headerProm = ts.headerRequest().send();
    auto dataTProm = ts.dataTRequest().send();

    return rangeProm
      .then([KJ_MVCAP(headerProm), KJ_MVCAP(dataTProm)](auto&& rangeResponse) mutable {
      return headerProm
        .then([KJ_MVCAP(rangeResponse), KJ_MVCAP(dataTProm)](auto&& headerResponse) mutable {
        return dataTProm
          .then([KJ_MVCAP(rangeResponse), KJ_MVCAP(headerResponse)](auto&& dataTResponse) mutable {
          auto sd = rangeResponse.getStartDate();
          auto ed = rangeResponse.getEndDate();
          return fromCapnpData(
            Date(sd.getDay(), sd.getMonth(), sd.getYear()),
            Date(ed.getDay(), ed.getMonth(), ed.getYear()),
            headerResponse.getHeader(), dataTResponse.getData());
        });
      });
    }).then([context, runMonica](DataAccessor da) mutable {
      auto out = runMonica(da);
      auto rs = context.getResults();
      rs.initResult();
      rs.getResult().setValue(out);
            });
  } else {
    auto out = runMonica();
    auto rs = context.getResults();
    rs.initResult();
    rs.getResult().setValue(out);
    return kj::READY_NOW;
  }
}
