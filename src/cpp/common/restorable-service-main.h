/* This Source Code Form is subject to the terms of the Mozilla Public
* License, v. 2.0. If a copy of the MPL was not distributed with this
* file, You can obtain one at http://mozilla.org/MPL/2.0/. */

/*
Authors:
Michael Berg <michael.berg@zalf.de>

Maintainers:
Currently maintained by the authors.

This file is part of the ZALF model and simulation infrastructure.
Copyright (C) Leibniz Centre for Agricultural Landscape Research (ZALF)
*/

#pragma once

#include <kj/async-io.h>
#include <kj/common.h>
#include <kj/main.h>
#include <kj/string.h>
#include <kj/tuple.h>

#include "common.capnp.h"
#include "fbp.capnp.h"
#include "registry.capnp.h"

#include "restorer.h"
#include "rpc-connection-manager.h"

namespace mas::infrastructure::common {

class RestorableServiceMain
{
public:
  RestorableServiceMain(kj::ProcessContext &context, kj::StringPtr serviceVersion,
    kj::StringPtr serviceBriefDescription, kj::StringPtr serviceExtendedDescription = nullptr);

  kj::MainBuilder::Validity setName(kj::StringPtr n);
  kj::MainBuilder::Validity setDescription(kj::StringPtr d);
  kj::MainBuilder::Validity setHost(kj::StringPtr name);
  kj::MainBuilder::Validity setLocalHost(kj::StringPtr h);
  kj::MainBuilder::Validity setPort(kj::StringPtr name);
  kj::MainBuilder::Validity setCheckPort(kj::StringPtr portStr);
  kj::MainBuilder::Validity setCheckIP(kj::StringPtr ip);
  kj::MainBuilder::Validity setRestorerContainerSR(kj::StringPtr sr);
  kj::MainBuilder::Validity setServiceContainerSR(kj::StringPtr sr);
  kj::MainBuilder::Validity setRegistrarSR(kj::StringPtr sr);
  kj::MainBuilder::Validity setRegName(kj::StringPtr n);
  kj::MainBuilder::Validity setRegCategory(kj::StringPtr cat);
  kj::MainBuilder::Validity setOutputSturdyRefs();
  kj::MainBuilder::Validity setStartupInfoWriterSR(kj::StringPtr sr);
  kj::MainBuilder::Validity setInitServiceFromContainer(kj::StringPtr init);

  // connects the restorer as bootstrap, unless serviceAasBootstrap = true
  void startRestorerSetup(mas::schema::common::Identifiable::Client serviceClient, bool serviceAsBootstrap = false);

  kj::MainBuilder& addRestorableServiceOptions();

protected:
  kj::MainBuilder mainBuilder;
  mas::schema::persistence::Restorer::Client restorerClient{nullptr};
  mas::schema::registry::Registrar::Unregister::Client serviceUnregisterAction{nullptr};
  Restorer* restorer{nullptr};
  bool initRestorerFromContainer{true};
  bool initServiceFromContainer{true};
  kj::ProcessContext &context;
  kj::AsyncIoContext ioContext;
  kj::Own<mas::infrastructure::common::ConnectionManager> conMan;
  kj::String name{kj::str("Unnamed Service")};
  kj::String description;
  kj::String host{kj::str("*")};
  kj::String localHost{kj::str("localhost")};
  int port{0};
  kj::String checkIP;
  int checkPort{0};
  kj::Maybe<mas::schema::storage::Store::Container::Client> restorerContainerClient;
  kj::Maybe<mas::schema::storage::Store::Container::Client> serviceContainerClient;
  kj::String restorerContainerSR;
  kj::String serviceContainerSR;
  kj::String registrarSR;
  kj::String regName;
  kj::String regCategory{kj::str("unknown")};
  bool outputSturdyRefs{false};
  kj::String startupInfoWriterSRId{kj::str("xxx")};
  kj::String startupInfoWriterSR;
  using P = mas::schema::common::Pair<capnp::Text, capnp::AnyPointer>;
  kj::Maybe<mas::schema::fbp::Channel<P>::ChanWriter::Client> startupInfoWriterClient;
};

} // namespace mas::infrastructure::common
