/* This Source Code Form is subject to the terms of the Mozilla Public
* License, v. 2.0. If a copy of the MPL was not distributed with this
* file, You can obtain one at http://mozilla.org/MPL/2.0/. */

/*
Authors:
Michael Berg <michael.berg-mohnicke@zalf.de>

Maintainers:
Currently maintained by the authors.

This file is part of the ZALF model and simulation infrastructure.
Copyright (C) Leibniz Centre for Agricultural Landscape Research (ZALF)
*/

#pragma once

#include <kj/array.h>
#include <kj/async.h>
#include <kj/function.h>
#include <kj/string.h>
#include <kj/timer.h>
#include <kj/tuple.h>

#include "common.capnp.h"
#include "persistence.capnp.h"
#include "storage.capnp.h"
#include "restorer.h"

namespace mas::infrastructure::common {

class HostPortResolver final : public mas::schema::persistence::HostPortResolver::Server
{
public:
  explicit HostPortResolver(kj::Timer& timer, kj::StringPtr name, kj::StringPtr description, uint32_t secsKeepAliveTimeout);
  ~HostPortResolver();

  kj::Promise<void> info(InfoContext context) override;

  // restore @0 (srToken :Text) -> (cap :Capability);
  kj::Promise<void> restore(RestoreContext context) override;

  // resolve @0 (id :Text) -> (host :Text, port :UInt16);
  kj::Promise<void> resolve(ResolveContext context) override;

  void setRestorer(Restorer* r, mas::schema::persistence::Restorer::Client client);

  void setStorageContainer(mas::schema::storage::Store::Container::Client client);

  mas::schema::persistence::HostPortResolver::Registrar::Client createRegistrar();

  kj::Promise<void> garbageCollectMappings(bool runOnce = false);

private:
  struct Impl;
  kj::Own<Impl> impl;
};

} // namespace mas::infrastructure::common
