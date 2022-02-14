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
#include <kj/common.h>
#include <kj/string.h>
#include <kj/vector.h>

#include <capnp/rpc-twoparty.h>
#include <kj/thread.h>

#include <functional>
#include <string>

#include "model.capnp.h"
#include "common.capnp.h"

namespace Monica {

class CallbackImpl final : public mas::rpc::common::Callback::Server {
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

class CapHolderImpl final : public mas::rpc::common::CapHolder<capnp::AnyPointer>::Server {
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

class CapHolderListImpl final : public mas::rpc::common::CapHolder<capnp::AnyPointer>::Server {
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


} // namespace Monica
