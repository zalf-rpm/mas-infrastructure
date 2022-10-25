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
//#include <bit>

#include <kj/debug.h>
#include <kj/thread.h>
#include <kj/common.h>
#include <kj/string.h>
#define KJ_MVCAP(var) var = kj::mv(var)

#include <capnp/capability.h>
#include <capnp/ez-rpc.h>
#include <capnp/message.h>
#include <capnp/schema.h>
#include <capnp/dynamic.h>
#include <capnp/list.h>
#include <capnp/rpc-twoparty.h>

#include <sodium.h>

#include "sole.hpp"

//#include "tools/debug.h"
//#include "tools/date.h"

using namespace std;
//using namespace Tools;
using namespace mas::rpc::common;

//-----------------------------------------------------------------------------

namespace {

  typedef enum _endian {little_endian, big_endian} EndianType;

  EndianType checkCPUEndian()
  {
      unsigned short x = 0x0001;
      unsigned char c = *(unsigned char *)(&x);
      EndianType CPUEndian;
      return c == 0x01 ? little_endian : big_endian;
  }

  void byteArrayToUInt64(unsigned char* bytes, uint64_t& value) {
    //constexpr bool littleEndian(std::endian::native == std::endian::little);
    bool littleEndian = checkCPUEndian() == little_endian;
    const int start = littleEndian ? 0 : 7;
    const int end = littleEndian ? 8 : -1;
    const int inc = littleEndian ? 1 : -1;
    value = 0;
    for (int i = start; i != end; i += inc) {
      value += (uint64_t)bytes[i] << (i * 8);
    }
  }

  void byteArrayToUInt32(unsigned char* bytes, uint32_t& value) {
    //constexpr bool littleEndian(std::endian::native == std::endian::little);
    bool littleEndian = checkCPUEndian() == little_endian;
    const int start = littleEndian ? 0 : 3;
    const int end = littleEndian ? 4 : -1;
    const int inc = littleEndian ? 1 : -1;
    value = 0;
    for (int i = start; i != end; i += inc) {
      value += (uint32_t)bytes[i] << (i * 8);
    }
  }

  constexpr char hexmap[] = 
    {'0', '1', '2', '3', '4', '5', '6', '7',
     '8', '9', 'a', 'b', 'c', 'd', 'e', 'f'};

  std::string hexStr(unsigned char *data, int len)
  {
    std::string s(len * 2, ' ');
    for (int i = 0; i < len; ++i) {
      s[2 * i]     = hexmap[(data[i] & 0xF0) >> 4];
      s[2 * i + 1] = hexmap[data[i] & 0x0F];
    }
    return s;
  }
}

Restorer::Restorer() 
: _signPK{new unsigned char[crypto_sign_PUBLICKEYBYTES]}
, _signSK{new unsigned char[crypto_sign_SECRETKEYBYTES]}
{
  if (sodium_init() == -1) {
    throw std::runtime_error("sodium_init failed");
  }

  crypto_sign_keypair(_signPK, _signSK);

  byteArrayToUInt64(&_signPK[0], _vatId[3]);
  byteArrayToUInt64(&_signPK[8], _vatId[2]);
  byteArrayToUInt64(&_signPK[16], _vatId[1]);
  byteArrayToUInt64(&_signPK[24], _vatId[0]);
}

Restorer::~Restorer() {
  delete[] _signPK;
  delete[] _signSK;
}

kj::Promise<void> Restorer::restore(RestoreContext context) {
  auto params = context.getParams();
  auto srt = params.getLocalRef().getAs<capnp::Text>();
  kj::StringPtr ownerGuid = 
    params.hasSealedFor() && params.getSealedFor().hasGuid() 
    ? params.getSealedFor().getGuid()
    : kj::StringPtr();
  KJ_IF_MAYBE(cap, getCapFromSRToken(srt, ownerGuid)) context.getResults().setCap(*cap);
  return kj::READY_NOW;
}

kj::Maybe<capnp::Capability::Client> Restorer::getCapFromSRToken(kj::StringPtr srToken, kj::StringPtr ownerGuid) 
{
  if(ownerGuid != nullptr) 
  {
    // and we know about that owner
    KJ_IF_MAYBE(ownerSignPK, _ownerGuidToSignPK.find(ownerGuid)) 
    {
      unsigned char unsignedSRToken[srToken.size()];
      unsigned long long unsignedSRTokenLen;
      if(crypto_sign_open(unsignedSRToken, &unsignedSRTokenLen, (unsigned char*)srToken.cStr(), srToken.size(), *ownerSignPK) == 0)
      {
        // and that known owner was actually the one who sealed the token 
        KJ_IF_MAYBE(ownerAndCap, _issuedSRTokens.find((const char*)unsignedSRToken)) 
        {
          if(kj::get<0>(*ownerAndCap) == ownerGuid) return kj::get<1>(*ownerAndCap);
        }
      }
    }
    // if we don't know about that owner or the owner was not the one who sealed the token
    return nullptr;
  }
    
  // if there is no owner
  KJ_IF_MAYBE(ownerAndCap, _issuedSRTokens.find(srToken)) 
  {
    return kj::get<1>(*ownerAndCap);
  }
}

kj::String Restorer::sturdyRefStr(kj::StringPtr srToken) const {
  auto vatIdHexStr = hexStr(_signPK, crypto_sign_PUBLICKEYBYTES);
  return kj::str("capnp://", vatIdHexStr, "@", _host, ":", _port, "/", srToken);
}

void Restorer::sturdyRef(mas::schema::persistence::SturdyRef::Builder srb, kj::StringPtr srToken) const {
  auto trb = srb.initTransient();
  auto vpb = trb.initVat();
  auto ib = vpb.initId();
  ib.setPublicKey0(0);
  ib.setPublicKey1(1);
  ib.setPublicKey2(2);
  ib.setPublicKey3(3);
  auto ab = vpb.initAddress();
  ab.setHost(_host.c_str());
  ab.setPort(_port);
  trb.initLocalRef().setAs<capnp::Text>(srToken);
}

kj::String Restorer::signSRTokenByVat(kj::StringPtr srToken)
{
  unsigned long long signedSRTokenLen = srToken.size() + crypto_sign_BYTES;
  unsigned char signedSRToken[signedSRTokenLen];
  crypto_sign(signedSRToken, &signedSRTokenLen, (unsigned char*)srToken.cStr(), srToken.size(), _signSK);
  return kj::str((const char*)signedSRToken);
}

void Restorer::save(capnp::Capability::Client cap, 
  mas::schema::persistence::SturdyRef::Builder sturdyRefBuilder,
  mas::schema::persistence::SturdyRef::Builder unsaveSRBuilder,
  kj::StringPtr sealForOwner, bool createUnsave) {
    
  auto vatSignedSRToken = signSRTokenByVat(kj::str(sole::uuid4().str()));
  _issuedSRTokens.insert(kj::str(vatSignedSRToken), kj::tuple(sealForOwner, cap));
  if(createUnsave)
  {
    auto vatSignedUnsaveSRToken = signSRTokenByVat(kj::str(sole::uuid4().str()));
    string vssrt = vatSignedSRToken.cStr();
    string vsusrt = vatSignedUnsaveSRToken.cStr();
    auto unsaveAction = kj::heap<Action>([this, vssrt, vsusrt]() { 
      unsave(vssrt.c_str()); unsave(vsusrt.c_str());
    }); 
    schema::common::Action::Client unsaveActionClient = kj::mv(unsaveAction);
    _issuedSRTokens.insert(kj::str(vatSignedUnsaveSRToken), kj::tuple(kj::heapString(sealForOwner), unsaveActionClient));
    sturdyRef(unsaveSRBuilder, vatSignedUnsaveSRToken);
  }

  sturdyRef(sturdyRefBuilder, vatSignedSRToken);
}


void Restorer::unsave(kj::StringPtr srToken) {
  _issuedSRTokens.erase(srToken);
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
  auto list = rs.getObject().initAs<capnp::List<mas::schema::common::ListEntry<mas::schema::common::CapHolder<capnp::AnyPointer>>>>((unsigned int)caps.size());
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

kj::Maybe<capnp::AnyPointer::Builder> 
mas::rpc::common::copyAndSetIPAttrs(mas::schema::common::IP::Reader oldIp, mas::schema::common::IP::Builder newIp, 
        kj::StringPtr newAttrName)//, kj::Maybe<capnp::AnyPointer::Reader> newValue)
{
  // if there are not attributes and nothing new to set, nothing to copy
  if (!oldIp.hasAttributes() && newAttrName == nullptr) return nullptr;

  int newIndex = -1;
  int oldAttrsSize = kj::size(oldIp.getAttributes());
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





