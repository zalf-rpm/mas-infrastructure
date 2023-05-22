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

#include <kj/debug.h>
#include <kj/common.h>
#include <kj/string.h>
#include <kj/vector.h>
#include <kj/map.h>
#include <kj/tuple.h>
#include <kj/array.h>

#include <capnp/rpc-twoparty.h>
#include <kj/thread.h>

//#include "model.capnp.h"
#include "common.capnp.h"
#include "fbp.capnp.h"
#include "persistence.capnp.h"

namespace mas::infrastructure::common {

class Identifiable : public mas::schema::common::Identifiable::Server {
public:
  Identifiable(kj::StringPtr id = nullptr, kj::StringPtr name = nullptr, kj::StringPtr description = nullptr);

  virtual ~Identifiable() noexcept(false) {}

  kj::Promise<void> info(InfoContext context) override;

private:
  kj::String _id;
  kj::String _name;
  kj::String _description;
};

//-----------------------------------------------------------------------------

// class CallbackImpl final : public mas::schema::common::Callback::Server {
// public:
//   CallbackImpl(kj::Function<void()> callback, 
//               bool execCallbackOnDel = false,
//               kj::StringPtr id = "<-");

//   virtual ~CallbackImpl() noexcept(false);

//   kj::Promise<void> call(CallContext context) override;

// private:
//   kj::String id{ kj::str("<-") };
//   kj::Function<void()> callback;
//   bool execCallbackOnDel{ false };
//   bool alreadyCalled{ false };
// };

//-----------------------------------------------------------------------------

// class Action final : public mas::schema::common::Action::Server {
// public:
//   Action(kj::Function<kj::Promise<void>()> action, bool execActionOnDel = false, kj::StringPtr id = "<-"); 

//   virtual ~Action() noexcept(false);

//   kj::Promise<void> do_(DoContext context) override;

// private:
//   kj::String id{ kj::str("<-") };
//   kj::Function<kj::Promise<void>()> action;
//   bool execActionOnDel{ false };
//   bool alreadyCalled{ false };
// };

//-----------------------------------------------------------------------------

// class CapHolderImpl final : public mas::schema::common::CapHolder<capnp::AnyPointer>::Server {
// public:
//   CapHolderImpl(capnp::Capability::Client cap,
//                 kj::StringPtr sturdyRef,
//                 bool releaseOnDel = false,
//                 kj::StringPtr id = "-");

//   virtual ~CapHolderImpl() noexcept(false);

//   kj::Promise<void> cap(CapContext context) override;

//   kj::Promise<void> release(ReleaseContext context) override;

//   //kj::Promise<void> save(SaveContext context) override;

// private:
//   kj::String id;
//   capnp::Capability::Client _cap;
//   kj::String sturdyRef;
//   bool releaseOnDel{ false };
//   bool alreadyReleased{ false };
// };

//-----------------------------------------------------------------------------

// class CapHolderListImpl final : public mas::schema::common::CapHolder<capnp::AnyPointer>::Server {
// public:
//   CapHolderListImpl(kj::Vector<capnp::Capability::Client>&& caps,
//                     kj::StringPtr sturdyRef,
//                     bool releaseOnDel = false,
//                     kj::StringPtr id = "[-]");

//   virtual ~CapHolderListImpl() noexcept(false);

//   kj::Promise<void> cap(CapContext context) override;

//   kj::Promise<void> release(ReleaseContext context) override;

//   //kj::Promise<void> save(SaveContext context) override;

// private:
//   kj::String id;
//   kj::Vector<capnp::Capability::Client> caps;
//   kj::String sturdyRef;
//   bool releaseOnDel{ false };
//   bool alreadyReleased{ false };
// };

//-----------------------------------------------------------------------

kj::Maybe<capnp::AnyPointer::Reader> getIPAttr(mas::schema::fbp::IP::Reader ip, kj::StringPtr attrName);
  
kj::Maybe<capnp::AnyPointer::Builder> 
copyAndSetIPAttrs(mas::schema::fbp::IP::Reader oldIp, mas::schema::fbp::IP::Builder newIp, 
  kj::StringPtr newAttrName = nullptr);//, kj::Maybe<capnp::AnyPointer::Reader> newValue = nullptr);

//-----------------------------------------------------------------------

kj::Vector<kj::String> splitString(kj::StringPtr s, kj::StringPtr splitElements);

kj::String trimString(kj::StringPtr s, kj::StringPtr whitespaces);

} // namespace mas::infrastructure::common
