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
#include <fstream>
#include <string>
#include <tuple>
#include <vector>
#include <algorithm>

#include <kj/debug.h>
#include <kj/common.h>
#define KJ_MVCAP(var) var = kj::mv(var)

#include <capnp/ez-rpc.h>
#include <capnp/message.h>
#include <capnp/rpc-twoparty.h>

#include "tools/algorithms.h"

#include "rpc-connections.h"
#include "common.h"

#include "channel.h"
#include "common.capnp.h"

using namespace std;
using namespace Tools;
using namespace mas;

int main(int argc, const char* argv[]) {
  setlocale(LC_ALL, "");
  setlocale(LC_NUMERIC, "C");

  infrastructure::common::ConnectionManager _conMan;
  auto ioContext = kj::setupAsyncIo();

  vector<string> readerSrts;
  vector<string> writerSrts;
  uint bufferSize = 1;
  string host = "*";
  int port = 0;

  auto printHelp = [=]() {
    cout
      << "channel [options]" << endl
      << endl
      << "options:" << endl
      << "host=" << host << endl
      << "port=" << port << endl
      << "buffer_size=" << bufferSize << endl
      << "reader_srts=[[]]" << endl
      << "writer_srts=[[]]" << endl;
  };

  if (argc >= 1) {
    for (auto i = 1; i < argc; i++) {
      auto kv = splitString(argv[i], "=");
      if(kv.size() < 2) continue;
      auto key = kv.at(0);
      auto value = kv.at(1);
      if (key == "reader_srts") readerSrts.push_back(value);
      else if (key == "writer_srts") writerSrts.push_back(value);
      else if (key == "buffer_size") bufferSize = max(1U, uint(stoi(value)));
      else if (key == "host") host = value;
      else if (key == "port") port = max(0, stoi(value));
    }

    KJ_LOG(INFO, "starting channel");

    auto restorer = kj::heap<rpc::common::Restorer>();
    auto& restorerRef = *restorer;
    schema::persistence::Restorer::Client restorerClient = kj::mv(restorer);
    auto channel = kj::heap<rpc::common::Channel>(&restorerRef, bufferSize);
    auto& channelRef = *channel;
    rpc::common::Channel::Client channelClient = kj::mv(channel);
    //runMonicaRef.setClient(runMonicaClient);
    KJ_LOG(INFO, "created monica");

    KJ_LOG(INFO, "Channel: trying to bind to host: " + host + " port: " + to_string(port));
    auto proms = _conMan.bind(ioContext, restorerClient, host, port);
    auto hostPromise = proms.first.fork().addBranch();
    auto hostStr = hostPromise.wait(ioContext.waitScope);
    restorerRef.setHost("10.10.24.210");//addrStr);
    auto portPromise = proms.second.fork().addBranch();
    auto port = portPromise.wait(ioContext.waitScope);
    restorerRef.setPort(port);
    KJ_LOG(INFO, "Channel: bound to host: " + host + " port: " + to_string(port));

    auto restorerSR = restorerRef.sturdyRef();
    auto channelSRs = restorerRef.save(channelClient);
    KJ_LOG(INFO, "Channel: channel_sr: " + channelSRs.first);
    KJ_LOG(INFO, "Channel: restorer_sr: " + restorerSR);

    // Run forever, accepting connections and handling requests.
    kj::NEVER_DONE.wait(ioContext.waitScope);

    /*
    //create monica server implementation
    rpc::Cluster::ModelInstanceFactory::Client instanceFactoryClient = 
      kj::heap<SlurmMonicaInstanceFactory>(kj::str(monicaId), 
                                           kj::str(monicaPath), 
                                           kj::str(factoryAddress), 
                                           factoryPort);

    capnp::Capability::Client unregister(nullptr);

    //create client connection to proxy
    try {
      capnp::EzRpcClient client(runtimeAddress, runtimePort);

      auto& cWaitScope = client.getWaitScope();

      // Request the bootstrap capability from the server.
      rpc::Cluster::Runtime::Client cap = client.getMain<rpc::Cluster::Runtime>();

      // Make a call to the capability.
      auto request = cap.registerModelInstanceFactoryRequest();
      request.setAModelId(monicaId);
      request.setAFactory(instanceFactoryClient);
      auto response = request.send().wait(cWaitScope);
      unregister = kj::mv(response.getUnregister());

      if (!hideServer) {
        capnp::EzRpcServer server(instanceFactoryClient, address + (port < 0 ? "" : string(":") + to_string(port)));

        // Write the port number to stdout, in case it was chosen automatically.
        auto& waitScope = server.getWaitScope();
        port = server.getPort().wait(waitScope);
        if (port == 0) {
          // The address format "unix:/path/to/socket" opens a unix domain socket,
          // in which case the port will be zero.
          std::cout << "Listening on Unix socket..." << std::endl;
        } else {
          std::cout << "Listening on port " << port << "..." << std::endl;
        }

        // Run forever, accepting connections and handling requests.
        kj::NEVER_DONE.wait(waitScope);
      } else {
        kj::NEVER_DONE.wait(cWaitScope);
      }
    } catch (exception e) {
      cerr << "Couldn't connect to runtime at address: " << runtimeAddress << ":" << runtimePort << endl
        << "Exception: " << e.what() << endl;
    }

    debug() << "stopped Cap'n Proto Slurm MONICA instance factory" << endl;
    */
  }

  return 0;
}
