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

#include <kj/debug.h>

#include <kj/common.h>
#define KJ_MVCAP(var) var = kj::mv(var)

#include <capnp/ez-rpc.h>
#include <capnp/message.h>
#include <capnp/rpc-twoparty.h>
#include <kj/thread.h>

//#include "tools/helper.h"
#include "tools/debug.h"

#include "common.h"

#include "model.capnp.h"
#include "common.capnp.h"
#include "cluster_admin_service.capnp.h"

using namespace std;
using namespace Monica;
using namespace Tools;
using namespace mas;

string appName = "slurm-monica-instance-factory";
string version = "0.0.1-beta";

int main(int argc, const char* argv[]) {

  setlocale(LC_ALL, "");
  setlocale(LC_NUMERIC, "C");

  string runtimeAddress = "localhost";
  int runtimePort = 9000;
  string factoryAddress = "localhost";
  int factoryPort = 10000;

  string address = "*";
  int port = -1;

  bool hideServer = false;
  string monicaId = "MONICA vXXX";
  string monicaPath = "monica-capnp-server";
  bool startedServerInDebugMode = false;

  auto printHelp = [=]() {
    cout
      << appName << "[options]" << endl
      << endl
      << "options:" << endl
      << endl
      << " -h | --help "
      "... this help output" << endl
      << " -v | --version "
      "... outputs " << appName << " version and ZeroMQ version being used" << endl
      << endl
      << " -d | --debug "
      "... show debug outputs" << endl
      << " -i | --hide "
      "... hide server = factory can only be used via registered runtime" << endl
      << " -id | --monica-id ... MONICA_ID (default: " << monicaId << ")] "
      "... the monica id to present to the clients" << endl
      << " -mp | --monica-path ... MONICA_PATH (default: " << monicaPath << ")] "
      "... the path to the monica capnp server executeable" << endl
      << " -a | --address ... ADDRESS (default: " << address << ")] "
      "... runs server bound to given address, may be '*' to bind to all local addresses" << endl
      << " -p | --port ... PORT (default: none)] "
      "... runs the server bound to the port, PORT may be ommited to choose port automatically." << endl
      << " -ra | --runtime-address "
      "... ADDRESS (default: " << runtimeAddress << ")] "
      "... connects server to runtime running at given address" << endl
      << " -rp | --runtime-port ... PORT (default: " << runtimePort << ")] "
      "... connects server to runtime running on given port." << endl
      << " -fa | --factory-address "
      "... ADDRESS (default: " << factoryAddress << ")] "
      "... connect spawned monicas to the factory running at given address" << endl
      << " -fp | --factory-port ... PORT (default: " << factoryPort << ")] "
      "... connect spawned monicas to the factory running on given port." << endl;
  };

  if (argc >= 1) {
    for (auto i = 1; i < argc; i++) {
      string arg = argv[i];
      if (arg == "-d" || arg == "--debug") {
        activateDebug = true;
        startedServerInDebugMode = true;
      } else if (arg == "-i" || arg == "--hide") {
        hideServer = true;
      } else if (arg == "-id" || arg == "--monica-id") {
        if (i + 1 < argc && argv[i + 1][0] != '-')
          monicaId = argv[++i];
      } else if (arg == "-mp" || arg == "--monica-path") {
        if (i + 1 < argc && argv[i + 1][0] != '-')
          monicaPath = argv[++i];
      } else if (arg == "-a" || arg == "--address") {
        if (i + 1 < argc && argv[i + 1][0] != '-')
          address = argv[++i];
      } else if (arg == "-p" || arg == "--port") {
        if (i + 1 < argc && argv[i + 1][0] != '-')
          port = stoi(argv[++i]);
      } else if (arg == "-ra" || arg == "--runtime-address") {
        if (i + 1 < argc && argv[i + 1][0] != '-')
          runtimeAddress = argv[++i];
      } else if (arg == "-rp" || arg == "--runtime-port") {
        if (i + 1 < argc && argv[i + 1][0] != '-')
          runtimePort = stoi(argv[++i]);
      } else if (arg == "-fa" || arg == "--factory-address") {
        if (i + 1 < argc && argv[i + 1][0] != '-')
          factoryAddress = argv[++i];
      } else if (arg == "-fp" || arg == "--factory-port") {
        if (i + 1 < argc && argv[i + 1][0] != '-')
          factoryPort = stoi(argv[++i]);
      } else if (arg == "-h" || arg == "--help")
        printHelp(), exit(0);
      else if (arg == "-v" || arg == "--version")
        cout << appName << " version " << version << endl, exit(0);
    }

    debug() << "starting Cap'n Proto Slurm MONICA instance factory" << endl;

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
  }

  return 0;
}
