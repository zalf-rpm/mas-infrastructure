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

#include "sole.hpp"
#include "common.h"
#include "restorer.h"

using namespace mas::infrastructure::common;

struct HostPortResolver::Impl {

  struct Heartbeat final : public mas::schema::persistence::HostPortResolver::Registrar::Heartbeat::Server {
    kj::String base64VatId;
    kj::String alias;
    HostPortResolver::Impl &hprImpl;

    Heartbeat(HostPortResolver::Impl &hprImpl, kj::StringPtr base64VatId, kj::StringPtr alias)
        : hprImpl(hprImpl), base64VatId(kj::str(base64VatId)), alias(kj::str(alias)) {}

    virtual ~Heartbeat() = default;

    kj::Promise<void> beat(BeatContext context) override {
      hprImpl.keepAlive(base64VatId, alias);
      return kj::READY_NOW;
    }
  };

  struct Registrar final : public mas::schema::persistence::HostPortResolver::Registrar::Server {
    HostPortResolver::Impl &hprImpl;
    kj::Timer &timer;

    Registrar(HostPortResolver::Impl &hprImpl, kj::Timer &timer) : hprImpl(hprImpl), timer(timer) {}
    virtual ~Registrar() = default;

    // register @0 (base64VatId :Text, host :Text, port :UInt16, alias :Text) -> (heartbeat :Capability, secsHeartbeatInterval :UInt32);
    kj::Promise<void> register_(RegisterContext context) override {
      auto params = context.getParams();
      if (params.hasBase64VatId() && params.hasHost() && params.getPort() > 0) {
        return hprImpl.addAndStoreMapping(params.getBase64VatId(), params.getHost(),
                                  params.getPort(), params.getAlias()
        ).then([this, context](bool success) mutable {
          auto params = context.getParams();
          auto res = context.getResults();
          if (success) {
            auto a = kj::heap<Heartbeat>(hprImpl, params.getBase64VatId(), params.getAlias());
            res.setHeartbeat(kj::mv(a));
            res.setSecsHeartbeatInterval(hprImpl.secsKeepAliveTimeout / kj::SECONDS);
          }
        });
      }
      return kj::READY_NOW;
    }
  };

  HostPortResolver &self;
  Restorer* restorerPtr{nullptr};
  mas::schema::persistence::Restorer::Client restorerClient{nullptr};
  mas::schema::storage::Store::Container::Client containerClient{nullptr};
  bool isContainerSet{false};
  kj::Timer &timer;
  kj::String id;
  kj::String name{kj::str("Host-Port-Resolver")};
  kj::String description;
  kj::Duration secsKeepAliveTimeout{10 * 60 * kj::SECONDS};
  uint16_t count{0};

  kj::HashMap<kj::String, kj::Tuple<kj::String, uint16_t, uint8_t>> id2HostPort;
  // the mapping from id to host and port (and keep alive count, which should never get 0)

  Impl(HostPortResolver &self, kj::Timer& timer, kj::StringPtr name, kj::StringPtr description, uint32_t secsKeepAliveTimeout)
  : self(self)
  , timer(timer)
  , id(kj::str(sole::uuid4().str()))
  , name(kj::str(name))
  , description(kj::str(description))
  , secsKeepAliveTimeout(secsKeepAliveTimeout * kj::SECONDS) {}

  ~Impl() = default;

  kj::Promise<void> garbageCollectMappings(bool runOnce = false) {
    KJ_DBG("garbageCollectMappings", runOnce, count++);
    kj::Vector<kj::String> toBeGarbageCollectedKeys;
    for (auto& entry : id2HostPort) {
      auto aliveCount = kj::get<2>(entry.value);
      if (aliveCount == 0) toBeGarbageCollectedKeys.add(kj::str(entry.key));
      else if(aliveCount > 0) kj::get<2>(entry.value) = aliveCount - 1;
    }

    auto proms = kj::heapArrayBuilder<kj::Promise<bool>>(
        (isContainerSet ? toBeGarbageCollectedKeys.size() : 0) + (runOnce ? 0 : 1));
    for(kj::StringPtr key : toBeGarbageCollectedKeys) {
      if (isContainerSet){
        auto req = containerClient.removeEntryRequest();
        req.setKey(key);
        proms.add(req.send().then([](auto &&resp){ return resp.getSuccess(); }));
      }
      id2HostPort.erase(key);
    }

    if (runOnce) return kj::joinPromises(proms.finish()).ignoreResult();
    else {
      proms.add(timer.afterDelay(secsKeepAliveTimeout).then([](){ return false; }));
      return kj::joinPromises(proms.finish()).ignoreResult().then(
          [this]() { return garbageCollectMappings(); });
    }
  }

  void keepAlive(kj::StringPtr b64VatId, kj::StringPtr alias) {
    KJ_IF_MAYBE(hp, id2HostPort.find(b64VatId)) kj::get<2>(*hp) = 1;
    if (alias != nullptr && alias.size() > 0) KJ_IF_MAYBE(hp, id2HostPort.find(alias)) kj::get<2>(*hp) = 1;
  }

  kj::Promise<bool> addAndStoreMapping(kj::StringPtr b64VatId, kj::StringPtr host, uint16_t port, kj::StringPtr alias) {
    addMapping(b64VatId, host, port, alias);
    if (isContainerSet){
      auto req = containerClient.addEntryRequest();
      req.setKey(b64VatId);
      req.setReplaceExisting(true);
      auto rps = req.initValue().initAnyValueAs<mas::schema::persistence::HostPortResolver::Registrar::RegisterParams>();
      rps.setBase64VatId(b64VatId);
      rps.setHost(host);
      rps.setPort(port);
      if (alias != nullptr) rps.setAlias(alias);
      return req.send().then(
          [](auto &&resp) { return resp.getSuccess(); },
          [](auto &&ex) {
            KJ_LOG(INFO, "Couldn't add mapping to storage container.", ex);
            return false;
          });
    }
    return {true};
  }

  void addMapping(kj::StringPtr b64VatId, kj::StringPtr host, uint16_t port, kj::StringPtr alias) {
    auto updateFunc = [](auto &existingValue, auto &&newValue){ existingValue = kj::mv(newValue); };
    id2HostPort.upsert(kj::str(b64VatId), kj::tuple(kj::str(host), port, 1), updateFunc);
    if (alias != nullptr) id2HostPort.upsert(kj::str(alias), kj::tuple(kj::str(host), port, 1), updateFunc);
  }

  kj::Promise<bool> initMappingsFromContainer() {
    if (isContainerSet) {
      auto req = containerClient.downloadEntriesRequest();
      return req.send().then([this](auto &&resp) {
        for(const auto pair : resp.getEntries()){
          auto fst = pair.getFst();
          capnp::AnyStruct::Reader val = pair.getSnd().getAnyValue();
          auto ps = val.as<mas::schema::persistence::HostPortResolver::Registrar::RegisterParams>();
          addMapping(ps.getBase64VatId(), ps.getHost(), ps.getPort(), ps.getAlias());
        }
        return true;
      }, [](auto &&ex) {
        KJ_LOG(ERROR, "Couldn't request list entries.", ex);
        return false;
      });
    }
    return {false};
  }

  mas::schema::persistence::HostPortResolver::Registrar::Client createRegistrar() {
    return kj::heap<Registrar>(*this, timer);
  }
};

HostPortResolver::HostPortResolver(kj::Timer& timer, kj::StringPtr name, kj::StringPtr description, uint32_t secsKeepAliveTimeout)
: impl(kj::heap<Impl>(*this, timer, name, description, secsKeepAliveTimeout)) {
}

HostPortResolver::~HostPortResolver() = default;

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
  req.setLocalRef(params.getLocalRef());
  if (params.hasSealedBy()) req.setSealedBy(params.getSealedBy());
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

void HostPortResolver::setStorageContainer(mas::schema::storage::Store::Container::Client client) {
  impl->containerClient = client;
  impl->isContainerSet = true;
}

mas::schema::persistence::HostPortResolver::Registrar::Client HostPortResolver::createRegistrar() {
  return impl->createRegistrar();
}

kj::Promise<void> HostPortResolver::garbageCollectMappings(bool runOnce) { return impl->garbageCollectMappings(runOnce); }
