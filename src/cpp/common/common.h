/* This Source Code Form is subject to the terms of the Mozilla Public
* License, v. 2.0. If a copy of the MPL was not distributed with this
* file, You can obtain one at http://mozilla.org/MPL/2.0/. */

/*
Authors:
Michael Berg <michael.berg-mohnicke@zalf.de>

Maintainers:
Currently maintained by the authors.

This file is part of the MONICA model.
Copyright (C) Leibniz Centre for Agricultural Landscape Research (ZALF)
*/

#pragma once

#include <kj/debug.h>
#include <kj/common.h>
#include <kj/string.h>
#include <kj/vector.h>
#include <kj/map.h>
#include <kj/tuple.h>
#include <kj/array.h>

#include <capnp/rpc-twoparty.h>
#include <kj/thread.h>

#include <functional>
#include <string>
#include <vector>

//#include "model.capnp.h"
#include "common.capnp.h"
#include "persistence.capnp.h"

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

  int getPort() const { return _port; }
  void setPort(int p) { _port = p; }

  kj::StringPtr getHost() const { return _host; }
  void setHost(kj::StringPtr h) { _host = kj::str(h); }
  
  kj::String sturdyRefStr(kj::StringPtr srToken = nullptr) const;

  void sturdyRef(mas::schema::persistence::SturdyRef::Builder& srb, kj::StringPtr srToken) const;

  void save(capnp::Capability::Client cap, 
    mas::schema::persistence::SturdyRef::Builder sturdyRefBuilder,
    mas::schema::persistence::SturdyRef::Builder unsaveSRBuilder = nullptr,
    kj::StringPtr sealForOwner = nullptr, bool createUnsave = true);

  kj::Tuple<kj::String, kj::String> saveStr(capnp::Capability::Client cap, 
    kj::StringPtr sealForOwner = nullptr, bool createUnsave = true);

  void unsave(kj::StringPtr srToken);

  kj::Tuple<bool, kj::String> verifySRToken(kj::StringPtr srToken, kj::StringPtr vatIdBase64);

  void setOwnerSignPublicKey(kj::StringPtr ownerGuid, kj::ArrayPtr<unsigned char> ownerSignPublicKey);

private:
  kj::Maybe<capnp::Capability::Client> getCapFromSRToken(kj::StringPtr srToken, kj::StringPtr ownerGuid = kj::StringPtr());
  kj::String signSRTokenByVatAndEncodeBase64(kj::StringPtr srToken);

  kj::String _host;
  int _port{ 0 };
  uint64_t _vatId[4]{ 0, 0, 0, 0 };
  kj::Array<unsigned char> _signPKArray;
  kj::Array<unsigned char> _signSKArray;
  kj::HashMap<kj::String, kj::Tuple<kj::String, capnp::Capability::Client>> _issuedSRTokens;
  kj::HashMap<kj::String, kj::Array<unsigned char>> _ownerGuidToSignPK;
  std::vector<std::function<void()>> _actions;
};

//-----------------------------------------------------------------------------

class Identifiable final : public mas::schema::common::Identifiable::Server {
public:
  Identifiable(std::string id = "", std::string name = "", std::string description = "");

  virtual ~Identifiable() noexcept(false) {}

  kj::Promise<void> info(InfoContext context) override;

private:
  std::string _id{ "" };
  std::string _name{ "" };
  std::string _description{ "" };
};

//-----------------------------------------------------------------------------

class CallbackImpl final : public mas::schema::common::Callback::Server {
public:
  CallbackImpl(std::function<void()> callback, 
              bool execCallbackOnDel = false,
              std::string id = "<-");

  virtual ~CallbackImpl() noexcept(false);

  kj::Promise<void> call(CallContext context) override;

private:
  std::string id{ "<-" };
  std::function<void()> callback;
  bool execCallbackOnDel{ false };
  bool alreadyCalled{ false };
};

//-----------------------------------------------------------------------------

class Action final : public mas::schema::common::Action::Server {
public:
  Action(kj::Function<void()> action, bool execActionOnDel = false, kj::StringPtr id = "<-"); 

  virtual ~Action() noexcept(false);

  kj::Promise<void> do_(DoContext context) override;

private:
  kj::String id{ kj::str("<-") };
  kj::Function<void()> action;
  bool execActionOnDel{ false };
  bool alreadyCalled{ false };
};

//-----------------------------------------------------------------------------

class CapHolderImpl final : public mas::schema::common::CapHolder<capnp::AnyPointer>::Server {
public:
  CapHolderImpl(capnp::Capability::Client cap,
                kj::String sturdyRef,
                bool releaseOnDel = false,
                std::string id = "-");

  virtual ~CapHolderImpl() noexcept(false);

  kj::Promise<void> cap(CapContext context) override;

  kj::Promise<void> release(ReleaseContext context) override;

  //kj::Promise<void> save(SaveContext context) override;

private:
  std::string id{ "-" };
  capnp::Capability::Client _cap;
  kj::String sturdyRef;
  bool releaseOnDel{ false };
  bool alreadyReleased{ false };
};

//-----------------------------------------------------------------------------

class CapHolderListImpl final : public mas::schema::common::CapHolder<capnp::AnyPointer>::Server {
public:
  CapHolderListImpl(kj::Vector<capnp::Capability::Client>&& caps,
                    kj::String sturdyRef,
                    bool releaseOnDel = false,
                    std::string id = "[-]");

  virtual ~CapHolderListImpl() noexcept(false);

  kj::Promise<void> cap(CapContext context) override;

  kj::Promise<void> release(ReleaseContext context) override;

  //kj::Promise<void> save(SaveContext context) override;

private:
  std::string id{ "[-]" };
  kj::Vector<capnp::Capability::Client> caps;
  kj::String sturdyRef;
  bool releaseOnDel{ false };
  bool alreadyReleased{ false };
};

//-----------------------------------------------------------------------

kj::Maybe<capnp::AnyPointer::Reader> getIPAttr(schema::common::IP::Reader ip, kj::StringPtr attrName);
  
kj::Maybe<capnp::AnyPointer::Builder> 
copyAndSetIPAttrs(schema::common::IP::Reader oldIp, schema::common::IP::Builder newIp, 
  kj::StringPtr newAttrName = nullptr);//, kj::Maybe<capnp::AnyPointer::Reader> newValue = nullptr);

} // namespace common
} // namespace infrastructure
} // namespace mas
