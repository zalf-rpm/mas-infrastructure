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

#pragma once

#include <kj/debug.h>
#include <kj/vector.h>
#include <kj/common.h>
#include <kj/thread.h>
#include <kj/async.h>
#include <kj/map.h>

#include <capnp/rpc-twoparty.h>
#include <capnp/capability.h>

//#include "climate/climate-common.h"

#include <functional>
#include <map>

#include "model.capnp.h"
#include "common.capnp.h"
#include "cluster_admin_service.capnp.h"

namespace Monica {

class SlurmMonicaInstanceFactory final : public mas::rpc::Cluster::ModelInstanceFactory::Server {
public:
  SlurmMonicaInstanceFactory(kj::String monicaVersion,
                             kj::String pathToMonicaCapnpServerExe,
                             kj::String factoryAddress,
                             int factoryPort);

  virtual ~SlurmMonicaInstanceFactory() noexcept(false);

  // registerModelInstance @5 [ModelInstance] (instance :ModelInstance, registrationToken :Text = "") -> (unregister :Common.Callback);
  kj::Promise<void> registerModelInstance(RegisterModelInstanceContext context) override;

  // modelId @4 () -> (id :Text);
  kj::Promise<void> modelId(ModelIdContext context) override;

  // info @0 () -> (info :IdInformation);
  kj::Promise<void> info(InfoContext context) override;

  // newInstance @0 () -> (instance :Common.CapHolder);
  kj::Promise<void> newInstance(NewInstanceContext context) override;

  // newInstances @1 (numberOfInstances :Int16) -> (instances :Common.CapHolder(X));#(Common.CapHolder));
  kj::Promise<void> newInstances(NewInstancesContext context) override;

  // newCloudViaZmqPipelineProxies @2 (numberOfInstances :Int16) -> (proxyAddresses :Common.CapHolder(Common.ZmqPipelineAddresses));
  kj::Promise<void> newCloudViaZmqPipelineProxies(NewCloudViaZmqPipelineProxiesContext context) override;

  // newCloudViaProxy @3 (numberOfInstances :Int16) -> (proxy :Common.CapHolder);
  kj::Promise<void> newCloudViaProxy(NewCloudViaProxyContext context) override;

  // restoreSturdyRef @6 (sturdyRef :Text) -> (cap :Common.CapHolder);
  kj::Promise<void> restoreSturdyRef(RestoreSturdyRefContext context) override;
   
  struct RegEntry {
    kj::HashMap<int, capnp::Capability::Client> instanceCaps;
    kj::Vector<capnp::Capability::Client> unregisterCaps;
    kj::Own<kj::PromiseFulfiller<void>> promFulfiller;
    int fulFillCount{ 0 };
  };
private:
  kj::String monicaId{ kj::str("MONICA vXXX") };
  kj::String pathToMonicaCapnpServerExe{ kj::str("monica-capnp-server") };
  kj::String factoryAddress{ kj::str("localhost") };
  int factoryPort{ 10000 };
  kj::HashMap<kj::String, RegEntry> registry;
  int instancesStarted{ 0 };
  int instancesRegistered{ 0 };
};

//-----------------------------------------------------------------------------

class RunMonicaImpl final : public mas::rpc::Model::EnvInstance::Server {
  // Implementation of the Model::Instance Cap'n Proto interface

  bool _startedServerInDebugMode{ false };

public:
  RunMonicaImpl(bool startedServerInDebugMode = false) : _startedServerInDebugMode(startedServerInDebugMode) {}

  kj::Promise<void> info(InfoContext context) override;

  kj::Promise<void> run(RunContext context) override;
};

} // namespace Monica
