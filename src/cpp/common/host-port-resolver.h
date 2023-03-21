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

class HostPortResolver;
class HPRRegistrar final : public mas::schema::persistence::HostPortResolver::Registrar::Server
{
public:
  HPRRegistrar(HostPortResolver *hpr, kj::Timer& timer);
  virtual ~HPRRegistrar() noexcept(false);

  // register @0 (base64VatId :Text, host :Text, port :UInt16, alias :Text) -> (heartbeat :Capability, secsHeartbeatInterval :UInt32);
  kj::Promise<void> register_(RegisterContext context) override;
private:
  HostPortResolver *hpr{nullptr};
  kj::Timer& timer;
};

class HostPortResolver final : public mas::schema::persistence::HostPortResolver::Server
{
  public:
  explicit HostPortResolver(kj::Timer& timer, kj::StringPtr name, kj::StringPtr description);
  virtual ~HostPortResolver() noexcept(false);

  kj::Promise<void> info(InfoContext context) override;

  // restore @0 (srToken :Text) -> (cap :Capability);
  kj::Promise<void> restore(RestoreContext context) override;

  // resolve @0 (id :Text) -> (host :Text, port :UInt16);
  kj::Promise<void> resolve(ResolveContext context) override;

  void setRestorer(Restorer* r, mas::schema::persistence::Restorer::Client client);

private:
  void addMapping(kj::StringPtr b64VatId, kj::StringPtr host, uint16_t port, kj::StringPtr alias = nullptr);

  void keepAlive(kj::StringPtr b64VatId, kj::StringPtr alias = nullptr);

  struct Impl;
  kj::Own<Impl> impl;
  friend class HPRRegistrar;
};

} // namespace mas::infrastructure::common
