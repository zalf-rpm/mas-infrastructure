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

#include "sole.hpp"

#include "tools/debug.h"
#include "tools/date.h"

using namespace std;
using namespace Tools;
using namespace mas::rpc::common;

//-----------------------------------------------------------------------------

kj::Promise<void> Restorer::restore(RestoreContext context) {
  auto srt = context.getParams().getSrToken();
  KJ_IF_MAYBE(cap, _issuedSRTokens.find(srt)) {
    context.getResults().setCap(*cap);
  }
  return kj::READY_NOW;
}

std::string Restorer::sturdyRef(std::string srToken) const {
  if(srToken.empty()) return "capnp://insecure@" + _host + ":" + to_string(_port);
  else return "capnp://insecure@" + _host + ":" + to_string(_port) + "/" + srToken;
}

std::pair<std::string, std::string> Restorer::save(capnp::Capability::Client cap) {
  auto srToken = sole::uuid4().str();
  _issuedSRTokens.insert(kj::str(srToken), cap);
  auto unsaveSRToken = sole::uuid4().str();
  auto unsaveAction = kj::heap<Action>([this, srToken, unsaveSRToken]() { unsave(srToken); unsave(unsaveSRToken); }); 
  schema::common::Action::Client unsaveActionClient = kj::mv(unsaveAction);
  _issuedSRTokens.insert(kj::str(unsaveSRToken), unsaveActionClient);
  return make_pair(sturdyRef(srToken), sturdyRef(unsaveSRToken));
}

void Restorer::unsave(std::string srToken) {
  _issuedSRTokens.erase(srToken.c_str());
}

//-----------------------------------------------------------------------------

Identifiable::Identifiable(std::string id, std::string name, std::string description)
  : _id(id), _name(name), _description(description) {
    if(_id.empty()) _id = sole::uuid4().str();
    if(_name.empty()) _name = _id;
  }

kj::Promise<void> Identifiable::info(InfoContext context) {
  auto rs = context.getResults();
  rs.setId(_id);
  rs.setName(_name);
  rs.setDescription(_description);
  return kj::READY_NOW;
}

//-----------------------------------------------------------------------------

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

Action::Action(std::function<void()> action, 
                           bool execActionOnDel,
                           std::string id)
  : action(kj::mv(action))
  , execActionOnDel(execActionOnDel)
  , id(id) {}

Action::~Action() noexcept(false) {
  if (execActionOnDel && !alreadyCalled)
    action();
}

kj::Promise<void> Action::do_(DoContext context) {
  action();
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
    auto c = _cap.castAs<mas::schema::common::Stopable>();
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
    auto c = _cap.castAs<mas::schema::common::Stopable>();
    return c.stopRequest().send().then([this](auto&&) {
      cout << "capholderimpl::release" << endl;
      alreadyReleased = true;
                                       });
  }
  return kj::READY_NOW;
}

//kj::Promise<void> CapHolderImpl::save(SaveContext context) {
//  context.getResults().setSturdyRef(sturdyRef);
//  return kj::READY_NOW;
//}

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
      auto c = cap.castAs<mas::schema::common::Stopable>();
      c.stopRequest().send().ignoreResult();
    }
    alreadyReleased = true;
  }
}

kj::Promise<void> CapHolderListImpl::cap(CapContext context) {
  auto rs = context.getResults();
  auto list = rs.getObject().initAs<capnp::List<mas::schema::common::ListEntry<mas::schema::common::CapHolder<capnp::AnyPointer>>>>((uint)caps.size());
  int i = 0;
  for (auto& cap : caps) {
    auto entryB = list[i];
    entryB.setEntry(cap.castAs<mas::schema::common::CapHolder<capnp::AnyPointer>>());
    i++;
  }
  return kj::READY_NOW;
}

kj::Promise<void> CapHolderListImpl::release(ReleaseContext context) {
  if (!alreadyReleased) {
    for (auto cap : caps) {
      auto c = cap.castAs<mas::schema::common::CapHolder<capnp::AnyPointer>>();
      c.releaseRequest().send().ignoreResult();
    }
    alreadyReleased = true;
  }
  return kj::READY_NOW;
}

//kj::Promise<void> CapHolderListImpl::save(SaveContext context) {
//  context.getResults().setSturdyRef(sturdyRef);
//  return kj::READY_NOW;
//}

//-----------------------------------------------------------------------------

kj::Maybe<capnp::AnyPointer::Reader> mas::rpc::common::getIPAttr(mas::schema::common::IP::Reader ip, kj::StringPtr attrName)
{
  if(ip.hasAttributes() && attrName != nullptr)
  {
    for(const auto& kv : ip.getAttributes()) if (kv.getKey() == attrName) return kv.getValue();
  }
  return nullptr;
}

kj::Maybe<capnp::AnyPointer::Builder> mas::rpc::common::copyAndSetIPAttrs(mas::schema::common::IP::Reader oldIp, mas::schema::common::IP::Builder newIp, 
        kj::StringPtr newAttrName)//, kj::Maybe<capnp::AnyPointer::Reader> newValue)
{
  // if there are not attributes and nothing new to set, nothing to copy
  if (!oldIp.hasAttributes() && newAttrName == nullptr) return nullptr;

  int newIndex = -1;
  int oldAttrsSize = 0;
  // if there are attributes and a new value to set, find the index to be replaced
  if(oldIp.hasAttributes() && newAttrName != nullptr)
  {
    auto attrs = oldIp.getAttributes();
    oldAttrsSize = attrs.size();
    for(int i = 0; i < oldAttrsSize; i++)
    {
      auto kv = attrs[i];
      if(kv.getKey() == newAttrName)
      {
        newIndex = i;
        break;
      }
    }
  }

  // init space for attributes in new IP
  auto newAttrsSize = oldAttrsSize;
  if(newIndex < 0 && newAttrName != nullptr)// && newValue != nullptr)
  {
      newAttrsSize += 1;
      newIndex = newAttrsSize - 1;
  }
  auto newAttrs = newIp.initAttributes(newAttrsSize);

  // copy old attributes
  if(oldIp.hasAttributes())
  {
    auto oldAttrs = oldIp.getAttributes();
    for(int i = 0; i < oldAttrsSize; i++)
    {
      const auto& kv = oldAttrs[i]; 
      if (i != newIndex)
      {
        newAttrs[i].setKey(kv.getKey());
        newAttrs[i].initValue().set(kv.getValue());
      }
    }
  }
  
  // set new attribute if there
  if (newIndex > -1) 
  {
    newAttrs[newIndex].setKey(newAttrName);
    //KJ_IF_MAYBE(nv, newValue) newAttrs[newIndex].initValue().set(*nv);
    return newAttrs[newIndex].initValue();
  }

  return nullptr;
}





