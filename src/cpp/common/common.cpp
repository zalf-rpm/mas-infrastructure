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

#include "common.h"

#include <iostream>
#include <fstream>
#include <string>
#include <tuple>
#include <vector>
#include <algorithm>
//#include <bit>

#include <kj/debug.h>
#include <kj/thread.h>
#include <kj/common.h>
#include <kj/string.h>
#define KJ_MVCAP(var) var = kj::mv(var)
#include <kj/encoding.h>

#include <capnp/capability.h>
#include <capnp/ez-rpc.h>
#include <capnp/message.h>
#include <capnp/schema.h>
#include <capnp/dynamic.h>
#include <capnp/list.h>
#include <capnp/rpc-twoparty.h>

#include "sole.hpp"

using namespace std;
//using namespace Tools;
using namespace mas::infrastructure::common;

//-----------------------------------------------------------------------------

Identifiable::Identifiable(kj::StringPtr id, kj::StringPtr name, kj::StringPtr description) {
  if(id == nullptr) _id = kj::str(sole::uuid4().str().c_str());
  if(name == nullptr) _name = kj::str(_id);
}

kj::Promise<void> Identifiable::info(InfoContext context) {
  auto rs = context.getResults();
  rs.setId(_id);
  rs.setName(_name);
  rs.setDescription(_description);
  return kj::READY_NOW;
}

//-----------------------------------------------------------------------------

// CallbackImpl::CallbackImpl(kj::Function<void()> callback, 
//                            bool execCallbackOnDel,
//                            kj::StringPtr id)
//   : callback(kj::mv(callback))
//   , execCallbackOnDel(execCallbackOnDel)
//   , id(kj::str(id)) {}

// CallbackImpl::~CallbackImpl() noexcept(false) {
//   if (execCallbackOnDel && !alreadyCalled) callback();
// }

// kj::Promise<void> CallbackImpl::call(CallContext context) {
//   callback();
//   alreadyCalled = true;
//   return kj::READY_NOW;
// }

//-----------------------------------------------------------------------------

// Action::Action(kj::Function<kj::Promise<void>()> action, 
//                bool execActionOnDel,
//                kj::StringPtr id)
//   : action(kj::mv(action))
//   , execActionOnDel(execActionOnDel)
//   , id(kj::str(id)) {}

// Action::~Action() noexcept(false) {
//   //if (execActionOnDel && !alreadyCalled)
//   //  action();
// }

// kj::Promise<void> Action::do() {
//   alreadyCalled = true;
//   return action();
// }

//-----------------------------------------------------------------------------

// CapHolderImpl::CapHolderImpl(capnp::Capability::Client cap,
//                              kj::StringPtr sturdyRef,
//                              bool releaseOnDel,
//                              kj::StringPtr id)
//   : _cap(cap)
//   , sturdyRef(kj::str(sturdyRef))
//   , releaseOnDel(releaseOnDel)
//   , id(kj::str(id)) {}

// CapHolderImpl::~CapHolderImpl() noexcept(false) {
//   if (releaseOnDel && !alreadyReleased) {
//     auto c = _cap.castAs<mas::schema::common::Stopable>();
//     alreadyReleased = true;
//     c.stopRequest().send().ignoreResult();
//   }
// }

// kj::Promise<void> CapHolderImpl::cap(CapContext context) {
//   context.getResults().getObject().setAs<capnp::Capability>(_cap);
//   return kj::READY_NOW;
// }

// kj::Promise<void> CapHolderImpl::release(ReleaseContext context) {
//   if (!alreadyReleased) {
//     auto c = _cap.castAs<mas::schema::common::Stopable>();
//     return c.stopRequest().send().then([this](auto&&) {
//       cout << "capholderimpl::release" << endl;
//       alreadyReleased = true;
//                                        });
//   }
//   return kj::READY_NOW;
// }

//kj::Promise<void> CapHolderImpl::save(SaveContext context) {
//  context.getResults().setSturdyRef(sturdyRef);
//  return kj::READY_NOW;
//}

//-----------------------------------------------------------------------------

// CapHolderListImpl::CapHolderListImpl(kj::Vector<capnp::Capability::Client>&& caps,
//                                      kj::StringPtr sturdyRef,
//                                      bool releaseOnDel,
//                                      kj::StringPtr id)
// : caps(kj::mv(caps))
// , sturdyRef(kj::str(sturdyRef))
// , releaseOnDel(releaseOnDel)
// , id(kj::str(id)) {}

// CapHolderListImpl::~CapHolderListImpl() noexcept(false) {
//   if (releaseOnDel && !alreadyReleased) {
//     for (auto cap : caps) {
//       auto c = cap.castAs<mas::schema::common::Stopable>();
//       c.stopRequest().send().ignoreResult();
//     }
//     alreadyReleased = true;
//   }
// }

// kj::Promise<void> CapHolderListImpl::cap(CapContext context) {
//   auto rs = context.getResults();
//   auto list = rs.getObject().initAs<capnp::List<mas::schema::common::ListEntry<mas::schema::common::CapHolder<capnp::AnyPointer>>>>((unsigned int)caps.size());
//   int i = 0;
//   for (auto& cap : caps) {
//     auto entryB = list[i];
//     entryB.setEntry(cap.castAs<mas::schema::common::CapHolder<capnp::AnyPointer>>());
//     i++;
//   }
//   return kj::READY_NOW;
// }

// kj::Promise<void> CapHolderListImpl::release(ReleaseContext context) {
//   if (!alreadyReleased) {
//     auto promises = kj::heapArrayBuilder<kj::Promise<void>>(caps.size());
//     int i = 0;
//     for (auto cap : caps) {
//       auto c = cap.castAs<mas::schema::common::CapHolder<capnp::AnyPointer>>();
//       promises.add(c.releaseRequest().send().ignoreResult());
//     }
//     alreadyReleased = true;
//     return kj::joinPromises(promises.finish());
//   }
//   return kj::READY_NOW;
// }

//kj::Promise<void> CapHolderListImpl::save(SaveContext context) {
//  context.getResults().setSturdyRef(sturdyRef);
//  return kj::READY_NOW;
//}

//-----------------------------------------------------------------------------

kj::Maybe<capnp::AnyPointer::Reader> mas::infrastructure::common::getIPAttr(mas::schema::fbp::IP::Reader ip, kj::StringPtr attrName)
{
  if(ip.hasAttributes() && attrName != nullptr) {
    for(const auto& kv : ip.getAttributes()) if (kv.getKey() == attrName) return kv.getValue();
  }
  return nullptr;
}

kj::Maybe<capnp::AnyPointer::Builder> 
mas::infrastructure::common::copyAndSetIPAttrs(mas::schema::fbp::IP::Reader oldIp, mas::schema::fbp::IP::Builder newIp, 
        kj::StringPtr newAttrName) { //, kj::Maybe<capnp::AnyPointer::Reader> newValue)
  // if there are not attributes and nothing new to set, nothing to copy
  if (!oldIp.hasAttributes() && newAttrName == nullptr) return nullptr;

  kj::Maybe<capnp::uint> newIndex;
  auto oldAttrsSize = oldIp.getAttributes().size();
  // if there are attributes and a new value to set, find the index to be replaced
  if(oldIp.hasAttributes() && newAttrName != nullptr) {
    auto attrs = oldIp.getAttributes();
    oldAttrsSize = attrs.size();
    for(capnp::uint i = 0; i < oldAttrsSize; i++) {
      auto kv = attrs[i];
      if(kv.getKey() == newAttrName) {
        newIndex = i;
        break;
      }
    }
  }

  // init space for attributes in new IP
  auto newAttrsSize = oldAttrsSize;
  if(newIndex == nullptr && newAttrName != nullptr) { // && newValue != nullptr)
      newAttrsSize += 1;
      newIndex = newAttrsSize - 1;
  }
  auto newAttrs = newIp.initAttributes(newAttrsSize);

  // copy old attributes
  if(oldIp.hasAttributes()) {
    auto oldAttrs = oldIp.getAttributes();
    for(capnp::uint i = 0; i < oldAttrsSize; i++) {
      const auto& kv = oldAttrs[i]; 
      KJ_IF_MAYBE(ni, newIndex) if (i == *ni) continue;
      newAttrs[i].setKey(kv.getKey());
      newAttrs[i].initValue().set(kv.getValue());
    }
  }
  
  // set new attribute if there
  KJ_IF_MAYBE(ni, newIndex) {
    newAttrs[*ni].setKey(newAttrName);
    //KJ_IF_MAYBE(nv, newValue) newAttrs[ni].initValue().set(*nv);
    return newAttrs[*ni].initValue();
  }

  return nullptr;
}

//-----------------------------------------------------------------------------

kj::Vector<kj::String> mas::infrastructure::common::splitString(kj::StringPtr s, kj::StringPtr splitElements) {
  kj::Vector<kj::String> result;
  while(s.size() > 0) {
    size_t minPos = s.size();
    for(auto c : splitElements) {
      KJ_IF_MAYBE(pos, s.findFirst(c)) minPos = kj::min(minPos, *pos);
    }
    result.add(kj::str(s.slice(0, minPos)));
    s = s.slice(minPos + (minPos == s.size() ? 0 : 1));
  }
	return kj::mv(result);
}


