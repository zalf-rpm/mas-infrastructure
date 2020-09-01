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

//#include "cluster_monica_instance_factory.h"

#include <iostream>
#include <fstream>
#include <string>
#include <tuple>
#include <vector>
#include <map>
#include <algorithm>

#include <kj/debug.h>
#include <kj/map.h>
#include <kj/common.h>
#define KJ_MVCAP(var) var = kj::mv(var)

#include <capnp/ez-rpc.h>
#include <capnp/message.h>
#include <capnp/rpc-twoparty.h>
#include <kj/thread.h>

//#include "tools/helper.h"
//#include "tools/debug.h"

#include "common.h"

#include "model.capnp.h"
#include "common.capnp.h"
//#include "cluster_admin_service.capnp.h"
#include "red.h"

using namespace std;
using namespace mas;

kj::WaitScope* waitScope = nullptr;

red_integer add(red_integer a, red_integer b) {
  cout << "a: " << a << " b: " << b << endl;
  return redInteger(redCInt32(a) + redCInt32(b));
}

int id = 0;
//map<long, mas::rpc::Climate::TimeSeries::Client> clients;
kj::HashMap<long, mas::rpc::Climate::TimeSeries::Client> clients;

red_block getHeader(red_integer p_timeSeries) {
  auto m = clients.find(redCInt32(p_timeSeries));
  cout << "here timeSeries: " << redCInt32(p_timeSeries) << endl;
  KJ_IF_MAYBE(timeSeries, clients.find(redCInt32(p_timeSeries))) {
    cout << "now here" << endl;
    auto headerp = timeSeries->headerRequest().send();    
    auto headerr = headerp.wait(*waitScope);  
    auto header = headerr.getHeader();  
    cout << "further" << endl;
    auto series = redMakeSeries(RED_TYPE_BLOCK, header.size());
    cout << "header: [" << endl;      
    for (auto h : header) {
      cout << int(h) << endl;
      redAppend(series, redInteger(int(h)));
    }
    cout << "]" << endl;
    redProbe(series);
    return series;
  }
  return redBlock(red_integer(111), red_integer(222), 0);
}



int main(int argc, const char* argv[]) {

  redOpen();
  redRoutine(redWord("c-add"), "[a [integer!] b [integer!]]", (void*)&add);

  setlocale(LC_ALL, "");
  setlocale(LC_NUMERIC, "C");

  //cout << "timeSeries symbol id: " << redSymbol("timeSeries") << endl;
  //cout << "timeSeries symbol id: " << redSymbol("timeSeries") << endl;

  capnp::EzRpcClient timeSeriesClient("localhost", 11002);
  waitScope = &(timeSeriesClient.getWaitScope());
  
  auto entry = clients.insert(id++, timeSeriesClient.getMain<rpc::Climate::TimeSeries>());
  cout << "entry.key: " << entry.key << endl;
  redSet(redSymbol("timeSeries"), redInteger(entry.key));
  redRoutine(redWord("get-header"), "[time-series [integer!]]", (red_block*)&getHeader);

  redDoFile("/C/Users/berg.ZALF-AD/GitHub/climate_data_server_cpp/src/test.red");

  clients.clear();

  //auto headerp = timeSeries.headerRequest().send();
  //auto headerr = headerp.wait(waitScope);
  //auto header = headerr.getHeader();
  //cout << "header: [" << endl;
  //for (auto h : header) {
  //  cout << int(h) << endl;
  //}
  //cout << "]" << endl;

  /*
  auto ri = redInteger(11);
  auto rf = redFloat(1);
  auto rci = redCInt32(ri);
  cout << redCInt32(redInteger(11)) << endl;
  cout << "1+1: " << redCInt32(redDo("1 + 1")) << endl;
  cout << redCInt32(redDo("c-add 2 3")) << endl;
  //redDo("view [text {hello}]");
  redDoFile("/C/Users/berg.ZALF-AD/GitHub/climate_data_server_cpp/src/test.red");
  cout << "here" << endl;
  */
  redClose();

  /*
  capnp::EzRpcClient adminMasterClient("10.10.24.186", 8000);
  auto& waitScope = adminMasterClient.getWaitScope();
  // Request the bootstrap capability from the server.
  rpc::Cluster::AdminMaster::Client adminMaster = adminMasterClient.getMain<rpc::Cluster::AdminMaster>();
  auto request = adminMaster.availableModelsRequest();
  auto response = request.send().wait(waitScope);
  auto factories = response.getFactories();
  if (factories.size() > 0) {
    auto factory = factories[0];
    //auto capHolder = factory.newInstanceRequest().send().wait(waitScope).getInstance().asGeneric<rpc::Model::EnvInstance>();
    auto instanceCapHolder = factory.newInstanceRequest().send().wait(waitScope).getInstance();
    auto modelRes = instanceCapHolder.capRequest().send().wait(waitScope);
    auto monica = modelRes.getCap().getAs<rpc::Model::EnvInstance>();
    //auto monica = cap.getAs<rpc::Model::EnvInstance>();
    //auto monica = capHolder.capRequest().send().wait(waitScope).getCap(); //<rpc::Model::EnvInstance>().;
    //auto monicaId = monica.infoRequest().send().wait(waitScope).getInfo().getId();
    monica.infoRequest().send().then([](auto&& res) {
      cout << "monicaId: " << res.getInfo().getId().cStr() << endl;
                                     }).wait(waitScope);
                                     //auto monicaId = monicaRes.getInfo().getId().cStr();
    //cout << "monicaId: " << monicaId << endl;
    //auto sturdyRef = capHolder.saveRequest().send().wait(waitScope).getSturdyRef();
    instanceCapHolder.saveRequest().send().then([](auto&& res) {
      cout << "sturdyRef: " << res.getSturdyRef().cStr() << endl;
                                                }).wait(waitScope);
    //auto sturdyRefP = instanceCapHolder.saveRequest().send().wait(waitScope);// .getSturdyRef();
    //auto sturdyRefStr = sturdyRefP.getSturdyRef().cStr();
    //cout << "sturdyRef: " << sturdyRefStr << endl;
    //auto prom = capHolder.releaseRequest().send().wait(waitScope);
    //cout << "blabal" << endl;
  }
  */

  return 0;
}
