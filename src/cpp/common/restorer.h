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
#include <kj/tuple.h>


//#include "model.capnp.h"
#include "common.capnp.h"
#include "persistence.capnp.h"
#include "storage.capnp.h"

namespace mas { 
namespace infrastructure { 
namespace common {

class Restorer final : public mas::schema::persistence::Restorer::Server
{
  public:
  Restorer();
  virtual ~Restorer() noexcept(false);

  // restore @0 (srToken :Text) -> (cap :Capability);
  kj::Promise<void> restore(RestoreContext context) override;

  int getPort() const;
  kj::Promise<void> setPort(int p);

  kj::StringPtr getHost() const;
  void setHost(kj::StringPtr h);
  
  mas::schema::storage::Store::Container::Client getStore();
  void setStorageContainer(mas::schema::storage::Store::Container::Client s);

  kj::Promise<void> initPortFromContainer();
  // initialize port from container if possible

  kj::Promise<void> initVatIdFromContainer();
  // initialize vatId from container if possible

  void setRestoreCallback(kj::Function<capnp::Capability::Client(kj::StringPtr restoreToken)> callback);

  kj::String sturdyRefStr(kj::StringPtr srToken = nullptr) const;

  void sturdyRef(mas::schema::persistence::SturdyRef::Builder& srb, kj::StringPtr srToken) const;

  kj::Promise<void> save(capnp::Capability::Client cap, 
    mas::schema::persistence::SturdyRef::Builder sturdyRefBuilder,
    mas::schema::persistence::SturdyRef::Builder unsaveSRBuilder = nullptr,
    kj::StringPtr fixedSRToken = nullptr, 
    kj::StringPtr sealForOwner = nullptr, bool createUnsave = true,
    kj::StringPtr restoreToken = nullptr);

  struct SaveStrResult {
    kj::String sturdyRef;
    kj::String srToken;
    kj::String unsaveSR;
    kj::String unsaveSRToken;
  };
  kj::Promise<SaveStrResult> saveStr(capnp::Capability::Client cap, 
    kj::StringPtr fixedSRToken = nullptr,
    kj::StringPtr sealForOwner = nullptr, bool createUnsave = true,
    kj::StringPtr restoreToken = nullptr,
    bool storeSturdyRefs = true);

  void unsave(kj::StringPtr srToken);

  kj::Tuple<bool, kj::String> verifySRToken(kj::StringPtr srToken, kj::StringPtr vatIdBase64);

  void setOwnerSignPublicKey(kj::StringPtr ownerGuid, kj::ArrayPtr<unsigned char> ownerSignPublicKey);

  void setVatId(mas::schema::persistence::VatId::Builder vatIdBuilder) const;

private:
  struct Impl;
  kj::Own<Impl> impl;
};


} // namespace common
} // namespace infrastructure
} // namespace mas
