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

#include "restorer.h"

#include <string.h>

#include <kj/async.h>
#include <kj/common.h>
#include <kj/debug.h>
#include <kj/encoding.h>
#include <kj/string.h>
#include <kj/thread.h>
#define KJ_MVCAP(var) var = kj::mv(var)

#include <capnp/capability.h>
#include <capnp/compat/json.h>
#include <capnp/message.h>

#include <sodium.h>

#include "sole.hpp"
#include "common.h"
#include "json11/json11.hpp"

using namespace mas::infrastructure::common;

//-----------------------------------------------------------------------------

struct Restorer::Impl {
  kj::String host;
  int port{ 0 };
  uint64_t vatId[4]{ 0, 0, 0, 0 };
  kj::Array<unsigned char> signPKArray;
  kj::Array<unsigned char> signSKArray;

  kj::Function<capnp::Capability::Client(kj::StringPtr)> restoreCallback;
  // a callback to the service this restorer is connected to, to restore a capability after a restart

  struct SRData {
    kj::String toJson() const { 
      return kj::str("{", 
        "\"ownerGuid\":\"", ownerGuid, "\",",
        "\"restoreToken\":\"", restoreToken, "\",",
        "\"unsaveSRToken\":\"", unsaveSRToken,"\"}");
    }

    void fromJson(json11::Json json) {
      const auto& obj = json.object_items();
      for(const auto& kv : obj) {
        const auto& key = kv.first;
        if(key == "ownerGuid") ownerGuid = kj::str(kv.second.string_value());
        else if(key == "restoreToken") restoreToken = kj::str(kv.second.string_value());
        else if(key == "unsaveSRToken") unsaveSRToken = kj::str(kv.second.string_value());
      }
      isCapSet = false;
    }

    kj::String ownerGuid;
    // if not empty the owner global uid allowed to restore the capability

    capnp::Capability::Client cap{nullptr};
    // the capability to a service object

    bool isCapSet{ false };
    // is the capability null or not, because for instance the restorer restarted

    kj::String restoreToken;
    // the token to be used to restore the capability from the associated service

    kj::String unsaveSRToken; 
    // set to the unsave sr token if the cap was an unsave action
    // in that case the restore token is the sr token of the associated capability
  };
  kj::HashMap<kj::String, SRData> issuedSRTokens;
  // the sturdy ref tokens issued by this restorer

  kj::HashMap<kj::String, kj::Array<unsigned char>> ownerGuidToSignPK;
  // the public keys of the owners of the capabilities

  mas::schema::storage::Store::Container::Client store{nullptr};
  // a capability to a store to persist the issued sturdy ref tokens

  bool isStoreSet{ false };

  Impl() {
    if (sodium_init() == -1) {
      throw std::runtime_error("sodium_init failed");
    }

    unsigned char signPK[crypto_sign_PUBLICKEYBYTES];
    unsigned char signSK[crypto_sign_SECRETKEYBYTES];
    crypto_sign_keypair(signPK, signSK);
    signPKArray = toKJArray(signPK, crypto_sign_PUBLICKEYBYTES);
    signSKArray = toKJArray(signSK, crypto_sign_SECRETKEYBYTES);

    // the Curve25519 byte array is little endian
    vatId[0] = byteArrayToUInt64(signPKArray.slice(0, 8));
    vatId[1] = byteArrayToUInt64(signPKArray.slice(8, 16));
    vatId[2] = byteArrayToUInt64(signPKArray.slice(16, 24));
    vatId[3] = byteArrayToUInt64(signPKArray.slice(24, 32));
  }

  ~Impl() noexcept(false)  {}

  typedef enum _endian {little_endian, big_endian} EndianType;
  EndianType checkCPUEndian()
  {
      unsigned short x = 0x0001;
      unsigned char c = *(unsigned char *)(&x);
      EndianType CPUEndian;
      return c == 0x01 ? little_endian : big_endian;
  }


  uint64_t byteArrayToUInt64(kj::ArrayPtr<unsigned char> bytes) {
    //constexpr bool littleEndian(std::endian::native == std::endian::little);
    bool littleEndian = checkCPUEndian() == little_endian;
    const int start = littleEndian ? 0 : 7;
    const int end = littleEndian ? 8 : -1;
    const int inc = littleEndian ? 1 : -1;
    uint64_t value = 0;
    for (int i = start; i != end; i += inc) {
      value += (uint64_t)bytes[i] << (i * 8);
    }
    return value;
  }


  void byteArrayToUInt32(kj::ArrayPtr<unsigned char> bytes, uint32_t& value) {
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


  kj::Array<unsigned char> toKJArray(unsigned char* data, int len)
  {
    auto arr = kj::heapArray<unsigned char>(len);
    for (int i = 0; i < len; i++) arr[i] = data[i];
    return kj::mv(arr);
  }


  template<typename T>
  unsigned char* fromKJArray(kj::ArrayPtr<T> arr, unsigned char* data = nullptr)
  {
    auto len = kj::size(arr);
    if (data == nullptr) data = new unsigned char[len];
    for (int i = 0; i < len; i++) data[i] = arr[i];
    return data;
  }


  kj::Promise<kj::Maybe<capnp::Capability::Client>> getCapFromSRToken(
    kj::StringPtr srToken, kj::StringPtr ownerGuid = kj::StringPtr()) {
    
    auto getCap = [this](auto srData) -> kj::Maybe<capnp::Capability::Client> {
      if(srData->isCapSet) return srData->cap;
      else if(srData->restoreToken.size() > 0){
        if(srData->unsaveSRToken.size() > 0) { // restore an unsave action
          auto srt = kj::str(srData->restoreToken);
          auto usrt = kj::str(srData->unsaveSRToken);
          auto unsaveAction = kj::heap<Action>([this, KJ_MVCAP(srt), KJ_MVCAP(usrt)]() { 
            issuedSRTokens.erase(srt); issuedSRTokens.erase(usrt);
          });
          srData->cap = kj::mv(unsaveAction);
          return srData->cap; 
        } else { // restore a service object
          try {
            auto cap = restoreCallback(srData->restoreToken);
            srData->cap = cap;
            return cap;
          } catch (std::exception e) {}
        }
      }
      return nullptr;
    };
    
    auto loadFromStoreAndGetCap = [this, getCap](kj::StringPtr srToken) -> kj::Promise<kj::Maybe<capnp::Capability::Client>> {
      auto req = store.getObjectRequest();
      req.setKey(srToken);
      return req.send().then([this, getCap](auto&& resp) -> SRData*  {
        auto obj = resp.getObject();
        //return kj::Maybe<capnp::Capability::Client>(nullptr);
        auto srData = kj::heap<SRData>();
        std::string err;
        for(const auto& kv : json11::Json::parse(obj.getValue().getTextValue().cStr(), err).object_items()){
          auto key = kv.first;
          if(key == "ownerGuid") srData->ownerGuid = kj::str(kv.second.string_value());
          else if(key == "restoreToken") srData->restoreToken = kj::str(kv.second.string_value());
          else if(key == "unsaveSRToken") srData->unsaveSRToken = kj::str(kv.second.string_value());
        }
        auto& srEntry = issuedSRTokens.insert(kj::str(obj.getKey()), 
          {kj::mv(srData->ownerGuid), nullptr, false, kj::mv(srData->restoreToken), kj::mv(srData->unsaveSRToken)});
        return &(srEntry.value);
      }).then([getCap](SRData* srData) -> kj::Maybe<capnp::Capability::Client> {
        if(srData != nullptr) return getCap(srData);
        else return kj::Maybe<capnp::Capability::Client>(nullptr);
      });
    };

    if(ownerGuid != nullptr) {
      // and we know about that owner
      KJ_IF_MAYBE(ownerSignPKArray, ownerGuidToSignPK.find(ownerGuid)) {
        // prepare owner public key
        unsigned char* ownerSignPK = (unsigned char*)malloc(ownerSignPKArray->size()*sizeof(unsigned char));
        KJ_DEFER(free(ownerSignPK));
        fromKJArray<kj::byte>(*ownerSignPKArray, ownerSignPK);

        // decode owner signed sturdy ref token
        unsigned char* signedSRToken = (unsigned char*)malloc(srToken.size() * sizeof(unsigned char));
        KJ_DEFER(free(signedSRToken));
        fromKJArray<const kj::byte>(srToken.asBytes(), signedSRToken);

        // verify owner signed sturdy ref token
        unsigned char* unsignedSRToken = (unsigned char*)malloc(srToken.size() * sizeof(unsigned char) + 1);
        KJ_DEFER(free(unsignedSRToken));
        unsigned long long unsignedSRTokenLen;
        if(crypto_sign_open(unsignedSRToken, &unsignedSRTokenLen, signedSRToken, srToken.size(), ownerSignPK) == 0) {
          unsignedSRToken[unsignedSRTokenLen] = '\0';
          // and that known owner was actually the one who sealed the token 
          // we stored the vat signed sturdy ref as base64
          kj::StringPtr unsignedSRTokenPtr((const char*)unsignedSRToken, unsignedSRTokenLen);
          KJ_IF_MAYBE(srData, issuedSRTokens.find(unsignedSRTokenPtr)) {
            return getCap(srData);
          } else if(isStoreSet) { // try to load from store
            return loadFromStoreAndGetCap(unsignedSRTokenPtr);
          }
        }
      }
      // if we don't know about that owner or the owner was not the one who sealed the token
      return nullptr;
    }
      
    // if there is no owner
    KJ_IF_MAYBE(srData, issuedSRTokens.find(srToken)) {
      return getCap(srData);
    } else if(isStoreSet) { // try to load from store
      return loadFromStoreAndGetCap(srToken);
    }

    return nullptr;
  }


  kj::String signSRTokenByVatAndEncodeBase64(kj::StringPtr srToken) {
    unsigned char* signSK = (unsigned char*)malloc(signSKArray.size() * sizeof(unsigned char));
    KJ_DEFER(free(signSK));
    fromKJArray<kj::byte>(signSKArray, signSK);

    unsigned long long signedSRTokenLen = srToken.size() + crypto_sign_BYTES;
    unsigned char* signedSRToken = (unsigned char*)malloc(signedSRTokenLen * sizeof(unsigned char));
    KJ_DEFER(free(signedSRToken));
    
    crypto_sign(signedSRToken, &signedSRTokenLen, (unsigned char*)srToken.cStr(), srToken.size(), signSK);
    //KJ_DBG("signedSRToken as hex:", kj::encodeHex(toKJArray(signedSRToken, signedSRTokenLen)));
    return kj::encodeBase64Url(toKJArray(signedSRToken, signedSRTokenLen));
  }
};

//-----------------------------------------------------------------------------

Restorer::Restorer()
: impl(kj::heap<Impl>()) {
}

Restorer::~Restorer() {}

int Restorer::getPort() const { return impl->port; }
void Restorer::setPort(int p) { impl->port = p; }


kj::StringPtr Restorer::getHost() const { return impl->host; }
void Restorer::setHost(kj::StringPtr h) { impl->host = kj::str(h); }


mas::schema::storage::Store::Container::Client Restorer::getStore() { return impl->store; }
void Restorer::setStore(mas::schema::storage::Store::Container::Client s) { 
  impl->store = s; 
  impl->isStoreSet = true;
}

void Restorer::setRestoreCallback(kj::Function<capnp::Capability::Client(kj::StringPtr restoreToken)> callback) {
    impl->restoreCallback = kj::mv(callback);
}

kj::Promise<void> Restorer::restore(RestoreContext context) {
  auto params = context.getParams();
  auto srt = params.getLocalRef().getAs<capnp::Text>();
  kj::StringPtr ownerGuid = 
    params.hasSealedFor() && params.getSealedFor().hasGuid() 
    ? params.getSealedFor().getGuid()
    : kj::StringPtr();
  return impl->getCapFromSRToken(srt, ownerGuid).then([context](auto&& maybeCap) mutable {
    KJ_IF_MAYBE(cap, maybeCap) { 
      context.getResults().setCap(*cap);
    }
  });
}


kj::String Restorer::sturdyRefStr(kj::StringPtr srToken) const {
  //KJ_DBG("signPKArray as hex:", kj::encodeHex(_signPKArray));
  auto srTokenBase64 = kj::encodeBase64Url(srToken.asBytes());
  auto vatIdBase64 = kj::encodeBase64Url(impl->signPKArray);
  return kj::str("capnp://", vatIdBase64, "@", impl->host, ":", impl->port, srTokenBase64.size() > 0 ? "/" : "", srTokenBase64);
}

void Restorer::sturdyRef(mas::schema::persistence::SturdyRef::Builder& srb, kj::StringPtr srToken) const {
  auto trb = srb.initTransient();
  auto vpb = trb.initVat();
  setVatId(vpb.initId());
  auto ab = vpb.initAddress();
  ab.setHost(impl->host);
  ab.setPort(impl->port);
  trb.initLocalRef().setAs<capnp::Text>(srToken);
}

kj::Tuple<bool, kj::String> Restorer::verifySRToken(kj::StringPtr srTokenBase64, kj::StringPtr vatIdBase64)
{
  auto vatIdPKArray = kj::decodeBase64(vatIdBase64.asArray());
  unsigned char* vatIdPK = (unsigned char*)malloc(vatIdPKArray.size() * sizeof(unsigned char));
  KJ_DEFER(free(vatIdPK));
  impl->fromKJArray<kj::byte>(vatIdPKArray, vatIdPK);

  unsigned char* unsignedSRToken = (unsigned char*)malloc(srTokenBase64.size() * sizeof(unsigned char));
  KJ_DEFER(free(unsignedSRToken));
  unsigned long long unsignedSRTokenLen;
  
  auto srTokenArray = kj::decodeBase64(srTokenBase64.asArray());
  unsigned char* srToken = (unsigned char*)malloc(srTokenArray.size() * sizeof(unsigned char));
  KJ_DEFER(free(srToken));
  impl->fromKJArray<kj::byte>(srTokenArray, srToken);
  return crypto_sign_open(unsignedSRToken, &unsignedSRTokenLen, srToken, srTokenArray.size(), vatIdPK) == 0
    ? kj::tuple(true, kj::str((const char*)unsignedSRToken))
    : kj::tuple(false, kj::str());
}


kj::Promise<void> Restorer::save(capnp::Capability::Client cap, 
  mas::schema::persistence::SturdyRef::Builder sturdyRefBuilder,
  mas::schema::persistence::SturdyRef::Builder unsaveSRBuilder,
  kj::StringPtr fixedSRToken, kj::StringPtr sealForOwner, bool createUnsave,
  kj::StringPtr restoreToken) 
{
  auto storePromises = kj::heapArrayBuilder<kj::Promise<bool>>(createUnsave ? 2 : 1);

  auto srToken = fixedSRToken == nullptr ? kj::str(sole::uuid4().str()) : kj::str(fixedSRToken);
  auto &srEntry = impl->issuedSRTokens.insert(kj::str(srToken), 
    {kj::str(sealForOwner), kj::mv(cap), true, kj::str(restoreToken)});
  // store sturdy ref data for later restoral 
  if(impl->isStoreSet){
    auto srdJson = srEntry.value.toJson();
    KJ_DBG(srToken, srdJson);
    auto req = impl->store.addObjectRequest();
    auto objb = req.initObject();
    objb.setKey(srToken);
    objb.initValue().setTextValue(srdJson);
    storePromises.add(req.send().then([](auto resp){ 
      return resp.getSuccess();
    }));
  }

  if(createUnsave) {
    auto unsaveSRToken = kj::str(sole::uuid4().str());
    auto srt = kj::str(srToken);
    auto usrt = kj::str(unsaveSRToken);
    auto unsaveAction = kj::heap<Action>([this, KJ_MVCAP(srt), KJ_MVCAP(usrt)]() { 
      unsave(srt); unsave(usrt);
    }); 
    auto &usrEntry = impl->issuedSRTokens.insert(kj::str(unsaveSRToken), 
      {kj::str(sealForOwner), kj::mv(unsaveAction), true, kj::str(restoreToken), kj::str(unsaveSRToken)});
    sturdyRef(unsaveSRBuilder, unsaveSRToken);

    if(impl->isStoreSet){
      auto srdJson = usrEntry.value.toJson();
      KJ_DBG(unsaveSRToken, srdJson);
      auto req = impl->store.addObjectRequest();
      auto objb = req.initObject();
      objb.setKey(unsaveSRToken);
      objb.initValue().setTextValue(srdJson);
      storePromises.add(req.send().then([](auto resp){ 
        return resp.getSuccess();
      }));
    }
  }

  sturdyRef(sturdyRefBuilder, srToken);

  return kj::joinPromises(storePromises.finish()).ignoreResult();
}

kj::Promise<kj::Tuple<kj::String, kj::String>> Restorer::saveStr(capnp::Capability::Client cap, 
  kj::StringPtr fixedSRToken, kj::StringPtr sealForOwner, bool createUnsave,
  kj::StringPtr restoreToken) {

  auto storePromises = kj::heapArrayBuilder<kj::Promise<bool>>(createUnsave ? 2 : 1);
  auto srToken = fixedSRToken == nullptr ? kj::str(sole::uuid4().str()) : kj::str(fixedSRToken);
  try {  
    auto &srEntry = impl->issuedSRTokens.insert(kj::str(srToken), {kj::str(sealForOwner), kj::mv(cap), true, kj::str(restoreToken)});
    // store sturdy ref data for later restoral 
    if(impl->isStoreSet){
      auto srdJson = srEntry.value.toJson();
      auto req = impl->store.addObjectRequest();
      auto objb = req.initObject();
      objb.setKey(srToken);
      objb.initValue().setTextValue(srdJson);
      storePromises.add(req.send().then([](auto resp){ return resp.getSuccess();}));
    }
  } catch(kj::Exception e) { 
    // catch because user can supply a fixed sturdy ref token
    kj::throwRecoverableException(KJ_EXCEPTION(FAILED, srToken, "already used")); 
  }
  kj::String unsaveSRStr;
  if(createUnsave) {
    auto unsaveSRToken = kj::str(sole::uuid4().str());
    auto srt = str(srToken);
    auto usrt = str(unsaveSRToken);
    auto unsaveAction = kj::heap<Action>([this, KJ_MVCAP(srt), KJ_MVCAP(usrt)]() { 
      unsave(srt); unsave(usrt);
    }); 
    auto& usrEntry = impl->issuedSRTokens.insert(kj::str(unsaveSRToken), {kj::str(sealForOwner), kj::mv(unsaveAction), true, 
      kj::str(restoreToken), kj::str(unsaveSRToken)});
    unsaveSRStr = sturdyRefStr(unsaveSRToken);

    if(impl->isStoreSet){
      auto srdJson = usrEntry.value.toJson();
      auto req = impl->store.addObjectRequest();
      auto objb = req.initObject();
      objb.setKey(unsaveSRToken);
      objb.initValue().setTextValue(srdJson);
      storePromises.add(req.send().then([](auto resp){ return resp.getSuccess();}));
    }
  }

  
  return kj::joinPromises(storePromises.finish()).ignoreResult().then(
    [this, KJ_MVCAP(srToken), KJ_MVCAP(unsaveSRStr)]() mutable { 
      return kj::tuple(sturdyRefStr(srToken), kj::mv(unsaveSRStr)); 
  });
  //return kj::tuple(sturdyRefStr(srToken), kj::mv(unsaveSRStr));
}

void Restorer::unsave(kj::StringPtr srToken) 
{
  impl->issuedSRTokens.erase(srToken);
}

void Restorer::setOwnerSignPublicKey(kj::StringPtr ownerGuid, kj::ArrayPtr<unsigned char> ownerSignPublicKey)
{
  impl->ownerGuidToSignPK.insert(kj::heapString(ownerGuid), kj::heapArray(ownerSignPublicKey));
}

void Restorer::setVatId(mas::schema::persistence::VatId::Builder vidb) const
{
  vidb.setPublicKey0(impl->vatId[0]);
  vidb.setPublicKey1(impl->vatId[1]);
  vidb.setPublicKey2(impl->vatId[2]);
  vidb.setPublicKey3(impl->vatId[3]);
}

//-----------------------------------------------------------------------------

