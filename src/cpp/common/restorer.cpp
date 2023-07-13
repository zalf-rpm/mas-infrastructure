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
#include "../json11/json11.hpp"

using namespace mas::infrastructure::common;

struct Restorer::Impl {

  struct ReleaseSturdyRef final : public mas::schema::persistence::Persistent::ReleaseSturdyRef::Server {
    kj::Function<kj::Promise<bool>()> releaseFunc;

    ReleaseSturdyRef(kj::Function<kj::Promise<bool>()> releaseFunc) : releaseFunc(kj::mv(releaseFunc)) {}

    virtual ~ReleaseSturdyRef() = default;

    kj::Promise<void> release(ReleaseContext context) override {
      return releaseFunc().then([context](auto &&res) mutable {
        context.getResults().setSuccess(res);
      });
    }
  };

  kj::String host;
  uint16_t port{ 0 };
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

    setVatIdFromSignPK();
  }

  ~Impl() = default;

  void setVatIdFromSignPK() {
    // the Curve25519 byte array is little endian
    vatId[0] = byteArrayToUInt64(signPKArray.slice(0, 8));
    vatId[1] = byteArrayToUInt64(signPKArray.slice(8, 16));
    vatId[2] = byteArrayToUInt64(signPKArray.slice(16, 24));
    vatId[3] = byteArrayToUInt64(signPKArray.slice(24, 32));
  }

  typedef enum _endian {little_endian, big_endian} EndianType;
  static EndianType checkCPUEndian()
  {
      unsigned short x = 0x0001;
      unsigned char c = *(unsigned char *)(&x);
      EndianType CPUEndian;
      return c == 0x01 ? little_endian : big_endian;
  }


  static uint64_t byteArrayToUInt64(kj::ArrayPtr<unsigned char> bytes) {
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


  static void byteArrayToUInt32(kj::ArrayPtr<unsigned char> bytes, uint32_t& value) {
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


  static kj::Array<unsigned char> toKJArray(unsigned char* data, size_t len)
  {
    auto arr = kj::heapArray<unsigned char>(len);
    for (size_t i = 0; i < len; i++) arr[i] = data[i];
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
    mas::schema::persistence::SturdyRef::Token::Reader srToken, kj::StringPtr ownerGuid = kj::StringPtr()) {
    
    auto getCap = [this](auto srData) -> kj::Maybe<capnp::Capability::Client> {
      if(srData->isCapSet) return srData->cap;
      else if(srData->unsaveSRToken.size() > 0) { // restore an unsave action
        auto srt = kj::str(srData->restoreToken);
        auto usrt = kj::str(srData->unsaveSRToken);
        auto unsaveAction = kj::heap<ReleaseSturdyRef>([this, KJ_MVCAP(srt), KJ_MVCAP(usrt)]() {
          auto usrt2 = kj::str(usrt);
          return unsave(srt).then([this, KJ_MVCAP(usrt2)](auto &&success){
            return unsave(usrt2).then([success](auto &&success2){ return success && success2; });
          });
        });
        srData->cap = kj::mv(unsaveAction);
        return srData->cap; 
      } else { // restore a service object
        try {
          auto cap = restoreCallback(srData->restoreToken);
          srData->cap = cap;
          srData->isCapSet = true;
          return cap;
        } catch (const std::exception& e) {}
      }
      return nullptr;
    };
    
    auto loadFromStoreAndGetCap =
        [this, getCap](kj::StringPtr srToken, kj::StringPtr ownerGuid = nullptr)
        -> kj::Promise<kj::Maybe<capnp::Capability::Client>> {
      auto req = store.getEntryRequest();
      req.setKey(srToken);
      auto valueProm = req.send().getEntry().getValueRequest().send();
      return valueProm.then([this, getCap, srToken](auto&& resp) -> SRData*  {
        if(resp.getIsUnset()) return nullptr;
        auto value = resp.getValue();
        auto srData = kj::heap<SRData>();
        std::string err;
        auto json = json11::Json::parse(value.getTextValue().cStr(), err);
        for(const auto& kv : json.object_items()){
          auto key = kv.first;
          if(key == "ownerGuid") srData->ownerGuid = kj::str(kv.second.string_value());
          else if(key == "restoreToken") srData->restoreToken = kj::str(kv.second.string_value());
          else if(key == "unsaveSRToken") srData->unsaveSRToken = kj::str(kv.second.string_value());
        }
        auto& srEntry = issuedSRTokens.insert(kj::str(srToken), {
          kj::mv(srData->ownerGuid),
          nullptr,
          false,
          kj::mv(srData->restoreToken),
          kj::mv(srData->unsaveSRToken)
        });
        return &(srEntry.value);
      }).then([getCap, ownerGuid](SRData* srData) -> kj::Maybe<capnp::Capability::Client> {
        if(srData != nullptr && srData->ownerGuid == ownerGuid) return getCap(srData);
        else return {nullptr};
      });
    };

    if(ownerGuid != nullptr && srToken.hasData()) {
      // and we know about that owner
      KJ_IF_MAYBE(ownerSignPKArray, ownerGuidToSignPK.find(ownerGuid)) {
        // prepare owner public key
        auto ownerSignPK = (unsigned char*)malloc(ownerSignPKArray->size()*sizeof(unsigned char));
        KJ_DEFER(free(ownerSignPK));
        fromKJArray<kj::byte>(*ownerSignPKArray, ownerSignPK);

        // decode owner signed sturdy ref token
        // because of signing assume the sr token has been encoded base64
        auto charArr = srToken.getData().asChars();
        auto srTokenArr = kj::decodeBase64(charArr);
        auto signedSRToken = (unsigned char*)malloc(charArr.size() * sizeof(unsigned char));
        KJ_DEFER(free(signedSRToken));
        fromKJArray<const kj::byte>(srTokenArr, signedSRToken);

        // verify owner signed sturdy ref token
        auto unsignedSRToken = (unsigned char*)malloc(charArr.size() * sizeof(unsigned char) + 1);
        KJ_DEFER(free(unsignedSRToken));
        unsigned long long unsignedSRTokenLen;
        if(crypto_sign_open(unsignedSRToken, &unsignedSRTokenLen, signedSRToken, charArr.size(),
                            ownerSignPK) == 0) {
          unsignedSRToken[unsignedSRTokenLen] = '\0';
          kj::StringPtr unsignedSRTokenPtr((const char*)unsignedSRToken, unsignedSRTokenLen);
          KJ_IF_MAYBE(srData, issuedSRTokens.find(unsignedSRTokenPtr)) {
            // check if the stored owner is the claimed owner
            if (srData->ownerGuid == ownerGuid) return getCap(srData);
          } else if(isStoreSet) { // try to load from store
            return loadFromStoreAndGetCap(unsignedSRTokenPtr, ownerGuid);
          }
        }
      }
      // if we don't know about that owner or the owner was not the one who sealed the token
      return nullptr;
    }

    if (srToken.hasText()) {
      // if there is no owner
      KJ_IF_MAYBE(srData, issuedSRTokens.find(srToken.getText())) {
        return getCap(srData);
      } else if (isStoreSet) { // try to load from store
        return loadFromStoreAndGetCap(srToken.getText());
      }
    }

    return nullptr;
  }


  kj::String signSRTokenByVatAndEncodeBase64(kj::StringPtr srToken) {
    auto signSK = (unsigned char*)malloc(signSKArray.size() * sizeof(unsigned char));
    KJ_DEFER(free(signSK));
    fromKJArray<kj::byte>(signSKArray, signSK);

    unsigned long long signedSRTokenLen = srToken.size() + crypto_sign_BYTES;
    auto signedSRToken = (unsigned char*)malloc(signedSRTokenLen * sizeof(unsigned char));
    KJ_DEFER(free(signedSRToken));
    
    crypto_sign(signedSRToken, &signedSRTokenLen, (unsigned char*)srToken.cStr(), srToken.size(), signSK);
    //KJ_DBG("signedSRToken as hex:", kj::encodeHex(toKJArray(signedSRToken, signedSRTokenLen)));
    return kj::encodeBase64Url(toKJArray(signedSRToken, signedSRTokenLen));
  }

  kj::Promise<bool> unsave(kj::StringPtr srToken) {
    bool success = issuedSRTokens.erase(srToken);
    if(isStoreSet) {
      auto req = store.removeEntryRequest();
      req.setKey(srToken);
      return req.send().then([success](auto &&res) { return res.getSuccess() && success; });
    }
    return success;
  }

};

Restorer::Restorer()
: impl(kj::heap<Impl>()) {
}

Restorer::~Restorer() = default;

int Restorer::getPort() const { return impl->port; }
kj::Promise<void> Restorer::setPort(int p) { 
  impl->port = p; 
  if(impl->isStoreSet){
    auto req = impl->store.getEntryRequest();
    req.setKey("port");
    auto svReq = req.send().getEntry().setValueRequest();
    svReq.initValue().setUint16Value(p);
    return svReq.send().ignoreResult();
  }
  return kj::READY_NOW;
}

kj::StringPtr Restorer::getHost() const { return impl->host; }

void Restorer::setHost(kj::StringPtr h) { impl->host = kj::str(h); }


mas::schema::storage::Store::Container::Client Restorer::getStore() { return impl->store; }

void Restorer::setStorageContainer(mas::schema::storage::Store::Container::Client s) { 
  impl->store = s; 
  impl->isStoreSet = true;
}

kj::Promise<void> Restorer::initPortFromContainer() { 
  auto proms = kj::heapArrayBuilder<kj::Promise<void>>(1);

  auto portReq = impl->store.getEntryRequest();
  portReq.setKey("port");

  try {
    proms.add(portReq.send().getEntry().getValueRequest().send().then([this](auto&& resp) {
      if(!resp.getIsUnset()) impl->port = resp.getValue().getUint16Value();
    }));
  } catch(kj::Exception& e) {
    KJ_LOG(INFO, "Couldn't initialize storage from container.", e);
    return kj::READY_NOW;
  }

  return kj::joinPromises(proms.finish());
}

kj::Promise<void> Restorer::initVatIdFromContainer() { 
  auto proms = kj::heapArrayBuilder<kj::Promise<bool>>(2);

  auto vatSignPKReq = impl->store.getEntryRequest();
  vatSignPKReq.setKey("vatSignPK");

  auto vatSignSKReq = impl->store.getEntryRequest();
  vatSignSKReq.setKey("vatSignSK");
  try {
    proms.add(vatSignPKReq.send().getEntry().getValueRequest().send().then([this](auto&& resp) {
      // if there is no sign public key in the storage, we store the one generated by default
      if(resp.getIsUnset()){
        auto vatSignPKReq = impl->store.addEntryRequest();
        vatSignPKReq.setKey("vatSignPK");
        vatSignPKReq.setReplaceExisting(true);
        auto pkb = vatSignPKReq.initValue().initUint8ListValue((capnp::uint)impl->signPKArray.size());
        for(capnp::uint i = 0; i < pkb.size(); i++) pkb.set(i, impl->signPKArray[i]);
        return vatSignPKReq.send().then([](auto&& resp){ return resp.getSuccess(); });
      } else { // ok we got a sign public key
        auto bytes = resp.getValue().getUint8ListValue();
        auto arr = kj::heapArray<unsigned char>(bytes.size());
        for (capnp::uint i = 0; i < bytes.size(); i++) arr[i] = (unsigned char)bytes[i];
        impl->signPKArray = kj::mv(arr);
        return kj::Promise<bool>(true);
      } 
    }));

    proms.add(vatSignSKReq.send().getEntry().getValueRequest().send().then([this](auto&& resp) {
      // if there is no sign secret key in the storage, we store the one generated by default
      if(resp.getIsUnset()){
        auto vatSignSKReq = impl->store.addEntryRequest();
        vatSignSKReq.setKey("vatSignSK");
        vatSignSKReq.setReplaceExisting(true);
        auto skb = vatSignSKReq.initValue().initUint8ListValue((capnp::uint)impl->signSKArray.size());
        for(capnp::uint i = 0; i < skb.size(); i++) skb.set(i, impl->signSKArray[i]);
        return vatSignSKReq.send().then([](auto&& resp){ return resp.getSuccess(); });
      } else { // ok we got a sign secret key
        auto bytes = resp.getValue().getUint8ListValue();
        auto arr = kj::heapArray<unsigned char>(bytes.size());
        for (capnp::uint i = 0; i < bytes.size(); i++) arr[i] = (unsigned char)bytes[i];
        impl->signSKArray = kj::mv(arr);
        return kj::Promise<bool>(true);
      } 
    }));

  } catch(kj::Exception& e) {
    KJ_LOG(INFO, "Couldn't initialize vat id from container.", e);
  }

  return kj::joinPromises(proms.finish()).then([this](auto&& readKeys){
    if (readKeys[0] && readKeys[1]) impl->setVatIdFromSignPK();
  });
}

void Restorer::setRestoreCallback(kj::Function<capnp::Capability::Client(kj::StringPtr restoreToken)> callback) {
    impl->restoreCallback = kj::mv(callback);
}

kj::Promise<void> Restorer::restore(RestoreContext context) {
  auto params = context.getParams();
  auto srt = params.getLocalRef();
  kj::StringPtr ownerGuid = 
    params.hasSealedBy() && params.getSealedBy().hasGuid()
    ? params.getSealedBy().getGuid()
    : kj::StringPtr();
  return impl->getCapFromSRToken(srt, ownerGuid).then([context](auto&& maybeCap) mutable {
    KJ_IF_MAYBE(cap, maybeCap) { 
      context.getResults().setCap(*cap);
    }
  });
}


kj::String Restorer::sturdyRefStr(kj::StringPtr srToken) const {
  //KJ_DBG("signPKArray as hex:", kj::encodeHex(_signPKArray));
  //auto srTokenBase64 = kj::encodeBase64Url(srToken.asBytes());
  auto vatIdBase64 = kj::encodeBase64Url(impl->signPKArray);
  return kj::str("capnp://", vatIdBase64, "@", impl->host, ":", impl->port,
                 srToken == nullptr ? "" : "/", srToken == nullptr ? "" : srToken);
}

void Restorer::sturdyRef(mas::schema::persistence::SturdyRef::Builder& srb, kj::StringPtr srToken) const {
  auto vpb = srb.initVat();
  setVatId(vpb.initId());
  auto ab = vpb.initAddress();
  ab.setHost(impl->host);
  ab.setPort(impl->port);
  srb.initLocalRef().setText(srToken);
}

kj::Tuple<bool, kj::String> Restorer::verifySRToken(kj::StringPtr srTokenBase64, kj::StringPtr vatIdBase64) {
  auto vatIdPKArray = kj::decodeBase64(vatIdBase64.asArray());
  auto vatIdPK = (unsigned char*)malloc(vatIdPKArray.size() * sizeof(unsigned char));
  KJ_DEFER(free(vatIdPK));
  impl->fromKJArray<kj::byte>(vatIdPKArray, vatIdPK);

  auto unsignedSRToken = (unsigned char*)malloc(srTokenBase64.size() * sizeof(unsigned char));
  KJ_DEFER(free(unsignedSRToken));
  unsigned long long unsignedSRTokenLen;
  
  auto srTokenArray = kj::decodeBase64(srTokenBase64.asArray());
  auto srToken = (unsigned char*)malloc(srTokenArray.size() * sizeof(unsigned char));
  KJ_DEFER(free(srToken));
  impl->fromKJArray<kj::byte>(srTokenArray, srToken);
  return crypto_sign_open(unsignedSRToken, &unsignedSRTokenLen, srToken, srTokenArray.size(), vatIdPK) == 0
    ? kj::tuple(true, kj::str((const char*)unsignedSRToken))
    : kj::tuple(false, kj::str());
}


kj::Promise<void> Restorer::save(capnp::Capability::Client cap,
                                 mas::schema::persistence::SturdyRef::Builder sturdyRefBuilder,
                                 mas::schema::persistence::SturdyRef::Builder unsaveSRBuilder,
                                 kj::StringPtr fixedSRToken,
                                 kj::StringPtr sealForOwnerGuid,
                                 bool createUnsave,
                                 kj::StringPtr restoreToken) {

  int promisesCount = impl->isStoreSet ? (createUnsave ? 2 : 1) : 0;
  auto storePromises = kj::heapArrayBuilder<kj::Promise<bool>>(promisesCount);

  auto srToken = fixedSRToken == nullptr ? kj::str(sole::uuid4().str()) : kj::str(fixedSRToken);
  auto &srEntry = impl->issuedSRTokens.insert(kj::str(srToken), {
    kj::str(sealForOwnerGuid),
    kj::mv(cap),
    true,
    kj::str(restoreToken)
  });
  // store sturdy ref data for later restoral 
  if(impl->isStoreSet && restoreToken != nullptr){
    auto srdJson = srEntry.value.toJson();
    KJ_DBG(srToken, srdJson);
    auto req = impl->store.getEntryRequest();
    req.setKey(srToken);
    auto svReq = req.send().getEntry().setValueRequest();
    svReq.initValue().setTextValue(srdJson);
    storePromises.add(svReq.send().then([](auto resp){ 
      return resp.getSuccess();
    }));
  }

  if(createUnsave) {
    auto unsaveSRToken = kj::str(sole::uuid4().str());
    auto srt = kj::str(srToken);
    auto usrt = kj::str(unsaveSRToken);
    auto unsaveAction = kj::heap<Impl::ReleaseSturdyRef>(
        [this, KJ_MVCAP(srt), KJ_MVCAP(usrt)]() {
      auto usrt2 = kj::str(usrt);
      return impl->unsave(srt).then([this, KJ_MVCAP(usrt2)](auto &&success){
        return impl->unsave(usrt2).then([success](auto &&success2) { return success && success2; });
      });
    }); 
    // for storing the unsave data, the restoreToken is actually the srToken, because we 
    // create the unsaveAction ourselves for the capability behind the srToken
    auto &usrEntry = impl->issuedSRTokens.insert(kj::str(unsaveSRToken), {
        kj::str(sealForOwnerGuid),
        kj::mv(unsaveAction),
        true,
        kj::str(srToken),
        kj::str(unsaveSRToken)
    });
    sturdyRef(unsaveSRBuilder, unsaveSRToken);

    if(impl->isStoreSet && restoreToken != nullptr){
      auto srdJson = usrEntry.value.toJson();
      KJ_DBG(unsaveSRToken, srdJson);
      auto req = impl->store.getEntryRequest();
      req.setKey(unsaveSRToken);
      auto svReq = req.send().getEntry().setValueRequest();
      svReq.initValue().setTextValue(srdJson);
      storePromises.add(svReq.send().then([](auto resp){ 
        return resp.getSuccess();
      }));
    }
  }

  sturdyRef(sturdyRefBuilder, srToken);

  return kj::joinPromises(storePromises.finish()).ignoreResult();
}

kj::Promise<Restorer::SaveStrResult>
Restorer::saveStr(capnp::Capability::Client cap, kj::StringPtr fixedSRToken,
                  kj::StringPtr sealForOwnerGuid, bool createUnsave,
                  kj::StringPtr restoreToken, bool storeSturdyRefs) {
  int promisesCount = impl->isStoreSet && storeSturdyRefs ? (createUnsave ? 2 : 1) : 0;
  auto storePromises = kj::heapArrayBuilder<kj::Promise<bool>>(promisesCount);
  auto srToken = fixedSRToken == nullptr ? kj::str(sole::uuid4().str()) : kj::str(fixedSRToken);
  try {
    auto &srEntry = impl->issuedSRTokens.insert(kj::str(srToken), {
        kj::str(sealForOwnerGuid),
        kj::mv(cap),
        true,
        kj::str(restoreToken)
    });
    // store sturdy ref data for later restoral
    if (impl->isStoreSet && storeSturdyRefs) {
      auto srdJson = srEntry.value.toJson();
      auto req = impl->store.getEntryRequest();
      req.setKey(srToken);
      auto svReq = req.send().getEntry().setValueRequest();
      svReq.initValue().setTextValue(srdJson);
      storePromises.add(svReq.send().then([](auto resp) {
        return resp.getSuccess();
      }));
    }
  } catch(const kj::Exception& e) {
    // catch because user can supply a fixed sturdy ref token
    kj::throwRecoverableException(KJ_EXCEPTION(FAILED, srToken, "already used")); 
  }
  kj::String unsaveSRStr;
  kj::String unsaveSRToken;
  if(createUnsave) {
    unsaveSRToken = kj::str(sole::uuid4().str());
    auto srt = kj::str(srToken);
    auto usrt = kj::str(unsaveSRToken);
    auto unsaveAction = kj::heap<Impl::ReleaseSturdyRef>([this, KJ_MVCAP(srt), KJ_MVCAP(usrt)]() {
      auto usrt2 = kj::str(usrt);
      return impl->unsave(srt).then([this, KJ_MVCAP(usrt2)](auto success){
        return impl->unsave(usrt2).then([success](auto success2){ return success && success2; });
      });
    });
    auto &usrEntry = impl->issuedSRTokens.insert(kj::str(unsaveSRToken), {
        kj::str(sealForOwnerGuid),
        kj::mv(unsaveAction),
        true,
        kj::str(srToken),
        kj::str(unsaveSRToken)
    });
    unsaveSRStr = sturdyRefStr(unsaveSRToken);

    if(impl->isStoreSet && storeSturdyRefs){
      auto srdJson = usrEntry.value.toJson();
      auto req = impl->store.getEntryRequest();
      req.setKey(unsaveSRToken);
      auto svReq = req.send().getEntry().setValueRequest();
      svReq.initValue().setTextValue(srdJson);
      storePromises.add(svReq.send().then([](auto resp){ 
        return resp.getSuccess();
      }));
    }
  }
  
  return kj::joinPromises(storePromises.finish()).ignoreResult().then(
    [this, KJ_MVCAP(srToken), KJ_MVCAP(unsaveSRToken)]() mutable {
      return SaveStrResult({sturdyRefStr(srToken), kj::mv(srToken),
                            sturdyRefStr(unsaveSRToken), kj::mv(unsaveSRToken)});
  });
}

kj::Promise<bool> Restorer::unsave(kj::StringPtr srToken) {
  return impl->unsave(srToken);
}

void Restorer::setOwnerSignPublicKey(kj::StringPtr ownerGuid, kj::ArrayPtr<unsigned char> ownerSignPublicKey) {
  impl->ownerGuidToSignPK.insert(kj::heapString(ownerGuid), kj::heapArray(ownerSignPublicKey));
}

void Restorer::setVatId(mas::schema::persistence::VatId::Builder vidb) const {
  vidb.setPublicKey0(impl->vatId[0]);
  vidb.setPublicKey1(impl->vatId[1]);
  vidb.setPublicKey2(impl->vatId[2]);
  vidb.setPublicKey3(impl->vatId[3]);
}

