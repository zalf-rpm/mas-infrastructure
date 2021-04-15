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

#include "common.h"

#include <iostream>
#include <fstream>
#include <string>
#include <tuple>
#include <vector>
#include <algorithm>

#include <kj/debug.h>
#include <kj/thread.h>
#include <kj/common.h>
#define KJ_MVCAP(var) var = kj::mv(var)

#include <capnp/capability.h>
#include <capnp/ez-rpc.h>
#include <capnp/message.h>
#include <capnp/schema.h>
#include <capnp/dynamic.h>
#include <capnp/list.h>
#include <capnp/rpc-twoparty.h>

#include "tools/debug.h"
#include "tools/date.h"

#include "model.capnp.h"

#include "common.capnp.h"

using namespace std;
using namespace Monica;
using namespace Tools;
using namespace mas;


CallbackImpl::CallbackImpl(std::function<void()> callback, 
                           bool execCallbackOnDel,
                           std::string id)
  : callback(kj::mv(callback))
  , execCallbackOnDel(execCallbackOnDel)
  , id(id) {}

CallbackImpl::~CallbackImpl() noexcept(false) {
  if (execCallbackOnDel && !alreadyCalled)
    callback();
}

kj::Promise<void> CallbackImpl::call(CallContext context) {
  callback();
  alreadyCalled = true;
  return kj::READY_NOW;
}

//-----------------------------------------------------------------------------

CapHolderImpl::CapHolderImpl(capnp::Capability::Client cap,
                             kj::String sturdyRef,
                             bool releaseOnDel,
                             std::string id)
  : _cap(cap)
  , sturdyRef(kj::mv(sturdyRef))
  , releaseOnDel(releaseOnDel)
  , id(id) {}

CapHolderImpl::~CapHolderImpl() noexcept(false) {
  if (releaseOnDel && !alreadyReleased) {
    auto c = _cap.castAs<rpc::common::Stopable>();
    alreadyReleased = true;
    c.stopRequest().send().ignoreResult();
  }
}

kj::Promise<void> CapHolderImpl::cap(CapContext context) {
  context.getResults().getObject().setAs<capnp::Capability>(_cap);
  return kj::READY_NOW;
}

kj::Promise<void> CapHolderImpl::release(ReleaseContext context) {
  if (!alreadyReleased) {
    auto c = _cap.castAs<rpc::common::Stopable>();
    return c.stopRequest().send().then([this](auto&&) {
      cout << "capholderimpl::release" << endl;
      alreadyReleased = true;
                                       });
  }
  return kj::READY_NOW;
}

kj::Promise<void> CapHolderImpl::save(SaveContext context) {
  context.getResults().setSturdyRef(sturdyRef);
  return kj::READY_NOW;
}

//-----------------------------------------------------------------------------

CapHolderListImpl::CapHolderListImpl(kj::Vector<capnp::Capability::Client>&& caps,
                                     kj::String sturdyRef,
                                     bool releaseOnDel,
                                     std::string id)
  : caps(kj::mv(caps))
  , sturdyRef(kj::mv(sturdyRef))
  , releaseOnDel(releaseOnDel)
  , id(id) {}

CapHolderListImpl::~CapHolderListImpl() noexcept(false) {
  if (releaseOnDel && !alreadyReleased) {
    for (auto cap : caps) {
      auto c = cap.castAs<rpc::common::Stopable>();
      c.stopRequest().send().ignoreResult();
    }
    alreadyReleased = true;
  }
}

kj::Promise<void> CapHolderListImpl::cap(CapContext context) {
  auto rs = context.getResults();
  auto list = rs.getObject().initAs<capnp::List<rpc::common::ListEntry<rpc::common::CapHolder<capnp::AnyPointer>>>>((uint)caps.size());
  int i = 0;
  for (auto& cap : caps) {
    auto entryB = list[i];
    entryB.setEntry(cap.castAs<rpc::common::CapHolder<capnp::AnyPointer>>());
    i++;
  }
  return kj::READY_NOW;
}

kj::Promise<void> CapHolderListImpl::release(ReleaseContext context) {
  if (!alreadyReleased) {
    for (auto cap : caps) {
      auto c = cap.castAs<rpc::common::CapHolder<capnp::AnyPointer>>();
      c.releaseRequest().send().ignoreResult();
    }
    alreadyReleased = true;
  }
  return kj::READY_NOW;
}

kj::Promise<void> CapHolderListImpl::save(SaveContext context) {
  context.getResults().setSturdyRef(sturdyRef);
  return kj::READY_NOW;
}

