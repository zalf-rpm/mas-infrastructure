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

#include "channel.h"

#include <iostream>
#include <fstream>
#include <string>
#include <tuple>
#include <vector>
#include <algorithm>

#include <kj/debug.h>
#include <kj/thread.h>
#include <kj/common.h>
#include <kj/async.h>
#include <kj/exception.h>
#define KJ_MVCAP(var) var = kj::mv(var)

#include <capnp/capability.h>
#include <capnp/ez-rpc.h>
#include <capnp/message.h>
#include <capnp/schema.h>
#include <capnp/dynamic.h>
#include <capnp/list.h>
#include <capnp/rpc-twoparty.h>
#include <capnp/any.h>

#include "sole.hpp"

using namespace std;
using namespace mas::infrastructure::common;

//-----------------------------------------------------------------------------

struct Channel::Impl {
  Channel& self;
  mas::infrastructure::common::Restorer* restorer{nullptr};
  kj::String id;
  kj::String name{kj::str("Channel")};
  kj::String description;
  kj::HashMap<kj::String, AnyPointerChannel::ChanReader::Client> readers;
  kj::HashMap<kj::String, AnyPointerChannel::ChanWriter::Client> writers;
  std::deque<kj::Own<kj::PromiseFulfiller<kj::Maybe<AnyPointerMsg::Reader>>>> blockingReadFulfillers;
  std::deque<kj::Own<kj::PromiseFulfiller<void>>> blockingWriteFulfillers;
  uint bufferSize{1};
  std::deque<kj::Own<kj::Decay<AnyPointerMsg::Reader>>> buffer;
  AnyPointerChannel::CloseSemantics autoCloseSemantics {AnyPointerChannel::CloseSemantics::FBP};
  bool sendCloseOnEmptyBuffer{false};
  AnyPointerChannel::Client client{nullptr};
  mas::schema::common::Action::Client unregisterAction{nullptr};
  
  Impl(Channel& self, mas::infrastructure::common::Restorer* restorer, kj::StringPtr name, kj::StringPtr description, 
    uint bufferSize)
  : self(self)
  , id(kj::str(sole::uuid4().str()))
  , name(kj::str(name))
  , description(kj::str(description))
  , bufferSize(std::max(1U, bufferSize))
  {
    setRestorer(restorer);
  }

  ~Impl() noexcept(false)  {}

  void setRestorer(mas::infrastructure::common::Restorer* restorer){ 
    if(restorer != nullptr){
      this->restorer = restorer; 
      // restorer->setRestoreCallback([this](kj::StringPtr containerId) -> capnp::Capability::Client {
      //   if(containerId == nullptr) return client;
      //   else return loadContainer(containerId);
      // });
    }
  }

};

//-----------------------------------------------------------------------------

Channel::Channel(kj::StringPtr name, kj::StringPtr description, uint bufferSize, Restorer* restorer)
: impl(kj::heap<Impl>(*this, restorer, name, description, bufferSize))
{
}


Channel::~Channel() {}


kj::Promise<void> Channel::info(InfoContext context) {
  KJ_LOG(INFO, "info message received");
  auto rs = context.getResults();
  rs.setId(impl->id);
  rs.setName(impl->name);
  rs.setDescription(impl->description);
  return kj::READY_NOW;
}


kj::Promise<void> Channel::save(SaveContext context) {
  KJ_LOG(INFO, "save message received");
  if(impl->restorer) {
    return impl->restorer->save(impl->client, context.getResults().initSturdyRef(), context.getResults().initUnsaveSR());
  }
  return kj::READY_NOW;
}


void Channel::closedReader(kj::StringPtr readerId){
  impl->readers.erase(readerId);
  // now that all readers disconnected, turn of auto-closing readers
  if (kj::size(impl->readers) == 0) impl->sendCloseOnEmptyBuffer = false;
  KJ_LOG(INFO, "number of readers left:", kj::size(impl->readers));
}

void Channel::closedWriter(kj::StringPtr writerId){
  impl->writers.erase(writerId);
  KJ_LOG(INFO, "number of writers left:", kj::size(impl->writers), impl->autoCloseSemantics);

  if(impl->autoCloseSemantics == AnyPointerChannel::CloseSemantics::FBP && kj::size(impl->writers) == 0){
    impl->sendCloseOnEmptyBuffer = true;
    KJ_LOG(INFO, "FBP semantics and no writers left -> sending done to readers");

    // as we just received a done message which should be distributed and would
    // fill the buffer, unblock all readers, so they send the done message
    while(kj::size(impl->blockingReadFulfillers) > 0){
      auto& brf = impl->blockingReadFulfillers.back();
      brf->fulfill(kj::Maybe<AnyPointerMsg::Reader>());
      impl->blockingReadFulfillers.pop_back();
      KJ_LOG(INFO, "sent done to reader on last finished writer");
    }
    KJ_LOG(INFO, kj::size(impl->blockingReadFulfillers));
    KJ_LOG(INFO, kj::size(impl->blockingWriteFulfillers));
  }
}

kj::Promise<void> Channel::reader(ReaderContext context){
  auto r = kj::heap<Reader>(*this);
  auto id = r->id();
  AnyPointerChannel::ChanReader::Client rc = kj::mv(r);
  impl->readers.insert(kj::str(id), rc);
  context.getResults().setR(rc);
  return kj::READY_NOW;
}

kj::Promise<void> Channel::writer(WriterContext context){
  auto w = kj::heap<Writer>(*this);
  auto id = w->id();
  AnyPointerChannel::ChanWriter::Client wc = kj::mv(w);
  impl->writers.insert(kj::str(id), wc);
  context.getResults().setW(wc);
  return kj::READY_NOW;
}


AnyPointerChannel::Client Channel::getClient() { return impl->client; }
void Channel::setClient(AnyPointerChannel::Client c) { impl->client = c; }

mas::schema::common::Action::Client Channel::getUnregisterAction() { return impl->unregisterAction; }
void Channel::setUnregisterAction(mas::schema::common::Action::Client unreg) { impl->unregisterAction = unreg; }

void Channel::setRestorer(mas::infrastructure::common::Restorer* restorer){ 
  impl->setRestorer(restorer);
}

//-----------------------------------------------------------------------------

Reader::Reader(Channel& c) 
: _channel(c)
, _id(kj::str(sole::uuid4().str())) {}

kj::Promise<void> Reader::read(ReadContext context) {
  KJ_REQUIRE(!_closed, "Reader already closed.", _closed);

  auto& c = _channel;
  auto& b = c.impl->buffer;

  // buffer not empty, send next value
  if(b.size() > 0){ 
    auto&& v = b.back();
    context.getResults().setValue(v.get()->getValue());
    b.pop_back();
    
    // unblock a writer unless we're about to close down
    if(!c.impl->blockingWriteFulfillers.empty() && !c.impl->sendCloseOnEmptyBuffer){
      auto&& bwf = c.impl->blockingWriteFulfillers.back();
      bwf->fulfill();
      c.impl->blockingWriteFulfillers.pop_back();
    }
  } else { 
    // buffer is empty, but we are supposed to close down
    if(c.impl->sendCloseOnEmptyBuffer){
      context.getResults().setDone();
      c.closedReader(id());

      // if there are other readers waiting close them as well
      while(!c.impl->blockingReadFulfillers.empty()){
        auto&& brf = c.impl->blockingReadFulfillers.back(); 
        brf->fulfill(nullptr);
        c.impl->blockingReadFulfillers.pop_back();
      }
    } else { // block because no value to read
      auto paf = kj::newPromiseAndFulfiller<kj::Maybe<AnyPointerMsg::Reader>>();
      c.impl->blockingReadFulfillers.push_front(kj::mv(paf.fulfiller)); 
      return paf.promise.then([context, this](kj::Maybe<AnyPointerMsg::Reader> msg) mutable {
        KJ_REQUIRE(!_closed, "Reader already closed.", _closed);

        if(_channel.impl->sendCloseOnEmptyBuffer && msg == nullptr){
          //KJ_DBG("setResults");
          context.getResults().setDone();
          KJ_LOG(INFO, "promise_lambda: sending done to reader");
          _channel.closedReader(id());
        } else {
          KJ_IF_MAYBE(m, msg){
            //KJ_DBG("Reader::read setResults");
            context.getResults().setValue(m->getValue());
            KJ_LOG(INFO, "promise_lambda: sending value to reader");
          }
        }
      });
    }
  }

  return kj::READY_NOW;
}

//-----------------------------------------------------------------------------

Writer::Writer(Channel& c) 
: _channel(c)
, _id(kj::str(sole::uuid4().str())) {}

kj::Promise<void> Writer::write(WriteContext context) {
  KJ_REQUIRE(!_closed, "Writer already closed.", _closed);
  
  auto v = context.getParams();
  auto& c = _channel;
  auto& b = c.impl->buffer;

  // if we received a done, this writer can be removed
  if(v.isDone()){
    c.closedWriter(id());
  } else if (c.impl->blockingReadFulfillers.size() > 0) { // there's a reader waiting
    auto&& brf = c.impl->blockingReadFulfillers.back(); 
    brf->fulfill(v);
    c.impl->blockingReadFulfillers.pop_back();
  } else if (b.size() < c.impl->bufferSize) { // there space to store the message
    b.push_front(capnp::clone(v));
  } else { // block until buffer has space 
    auto paf = kj::newPromiseAndFulfiller<void>();
    c.impl->blockingWriteFulfillers.push_front(kj::mv(paf.fulfiller)); 
    return paf.promise.then([context, this]() mutable {
      KJ_REQUIRE(!_closed, "promise_lambda: Writer already closed.", _closed);
      auto v = context.getParams();
      _channel.impl->buffer.push_front(capnp::clone(v));
      KJ_LOG(INFO, "promise_lambda: wrote value to buffer");
    });
  }

  return kj::READY_NOW;
}
