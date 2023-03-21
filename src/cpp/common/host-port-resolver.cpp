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

#include "host-port-resolver.h"

#include <string.h>

#include <kj/async.h>
#include <kj/common.h>
#include <kj/debug.h>
#include <kj/encoding.h>
#include <kj/string.h>
#include <kj/thread.h>
#include <kj/tuple.h>
#define KJ_MVCAP(var) var = kj::mv(var)

#include <capnp/capability.h>
#include <capnp/compat/json.h>
#include <capnp/message.h>

#include <sodium.h>

#include "sole.hpp"
#include "common.h"
#include "restorer.h"
#include "../json11/json11.hpp"

using namespace mas::infrastructure::common;

//-----------------------------------------------------------------------------

HPRRegistrar::HPRRegistrar(HostPortResolver *hpr, kj::Timer& timer) : hpr(hpr), timer(timer) {}

HPRRegistrar::~HPRRegistrar() noexcept(false) {}

kj::Promise<void> HPRRegistrar::register_(RegisterContext context) {
  auto params = context.getParams();
  if (params.hasBase64VatId() && params.hasHost() && params.getPort() > 0 && hpr != nullptr) {
    hpr->addMapping(params.getBase64VatId(), params.getHost(), params.getPort(),
                    params.hasAlias() ? params.getAlias() : nullptr);
  }

  kj::String b64VatId = kj::str(params.getBase64VatId());
  kj::String alias = params.hasAlias() ? kj::str(params.getAlias()) : kj::str();
  auto res = context.getResults();
  auto a = kj::heap<common::Action>([this, KJ_MVCAP(b64VatId), KJ_MVCAP(alias)]() mutable {
    if (hpr != nullptr) hpr->keepAlive(b64VatId, alias);
    return kj::READY_NOW;
  });
  res.setHeartbeat(kj::mv(a));
  res.setSecsHeartbeatInterval(10*60);
  return kj::READY_NOW;
}

//-----------------------------------------------------------------------------

struct HostPortResolver::Impl {
  Restorer* restorerPtr{nullptr};
  mas::schema::persistence::Restorer::Client restorerClient{nullptr};
  kj::Timer& timer;
  kj::String id;
  kj::String name{kj::str("Host-Port-Resolver")};
  kj::String description;

  kj::HashMap<kj::String, kj::Tuple<kj::String, uint16_t, uint8_t>> id2HostPort;
  // the mapping from id to host and port (and keep alive count, which should never get 0)

  Impl(kj::Timer& timer, kj::StringPtr name, kj::StringPtr description)
  : timer(timer)
  , id(kj::str(sole::uuid4().str()))
  , name(kj::str(name))
  , description(kj::str(description)) {}

  ~Impl() noexcept(false) {}
};

//-----------------------------------------------------------------------------

HostPortResolver::HostPortResolver(kj::Timer& timer, kj::StringPtr name, kj::StringPtr description)
: impl(kj::heap<Impl>(timer, name, description)) {
}

HostPortResolver::~HostPortResolver() {}

kj::Promise<void> HostPortResolver::info(InfoContext context) {
  KJ_LOG(INFO, "info message received");
  auto rs = context.getResults();
  rs.setId(impl->id);
  rs.setName(impl->name);
  rs.setDescription(impl->description);
  return kj::READY_NOW;
}

kj::Promise<void> HostPortResolver::restore(RestoreContext context) {
  auto req = impl->restorerClient.restoreRequest();
  auto params = context.getParams();
  req.getLocalRef().set(params.getLocalRef());
  if (params.hasSealedFor()) req.setSealedFor(params.getSealedFor());
  return req.send().then([context](auto &&resp) mutable {
    context.getResults().setCap(resp.getCap());
  });
}

kj::Promise<void> HostPortResolver::resolve(ResolveContext context) {
  auto params = context.getParams();
  if (params.hasId()) {
    KJ_IF_MAYBE(hp, impl->id2HostPort.find(params.getId())) {
      auto res = context.getResults();
      res.setHost(kj::get<0>(*hp));
      res.setPort(kj::get<1>(*hp));
    }
  }
  return kj::READY_NOW;
}

void HostPortResolver::setRestorer(Restorer *restorer, mas::schema::persistence::Restorer::Client client){
  impl->restorerPtr = restorer;
  impl->restorerClient = client;
}

void HostPortResolver::addMapping(kj::StringPtr b64VatId, kj::StringPtr host, uint16_t port, kj::StringPtr alias) {
  impl->id2HostPort.insert(kj::str(b64VatId), kj::tuple(kj::str(host), port, 1));
  if (alias != nullptr) impl->id2HostPort.insert(kj::str(alias), kj::tuple(kj::str(host), port, 1));
}

void HostPortResolver::keepAlive(kj::StringPtr b64VatId, kj::StringPtr alias) {
  KJ_IF_MAYBE(hp, impl->id2HostPort.find(b64VatId)) kj::get<2>(*hp) = 1;
  if (alias != nullptr && alias.size() > 0) KJ_IF_MAYBE(hp, impl->id2HostPort.find(alias)) kj::get<2>(*hp) = 1;
}

//-----------------------------------------------------------------------------

