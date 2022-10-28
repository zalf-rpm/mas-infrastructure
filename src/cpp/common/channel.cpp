/* This Source Code Form is subject to the terms of the Mozilla Public
* License, v. 2.0. If a copy of the MPL was not distributed with this
* file, You can obtain one at http://mozilla.org/MPL/2.0/. */

/*
Authors:
Michael Berg <michael.berg@zalf.de>

Maintainers:
Currently maintained by the authors.

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

Channel::Channel(mas::infrastructure::common::Restorer* restorer, kj::StringPtr name, uint bufferSize)
: _restorer(restorer)
, _name(kj::str(name))
, _bufferSize(std::max(1U, bufferSize))
{
}

void Channel::closedReader(kj::StringPtr readerId){
  KJ_LOG(INFO, "Channel::closedReader");
  _readers.erase(readerId);
  // now that all readers disconnected, turn of auto-closing readers
  if (kj::size(_readers) == 0) _sendCloseOnEmptyBuffer = false;
  KJ_LOG(INFO, "Channel::closedReader: number of readers left:", kj::size(_readers));
}

void Channel::closedWriter(kj::StringPtr writerId){
  KJ_LOG(INFO, "Channel::closed_writer");
  _writers.erase(writerId);
  KJ_LOG(INFO, "Channel::closedWriter: number of writers left:", kj::size(_writers), _autoCloseSemantics);

  if(_autoCloseSemantics == AnyPointerChannel::CloseSemantics::FBP && kj::size(_writers) == 0){
    _sendCloseOnEmptyBuffer = true;
    KJ_LOG(INFO, "Channel::closedWriter: FBP semantics and no writers left -> sending done to readers");

    // as we just received a done message which should be distributed and would
    // fill the buffer, unblock all readers, so they send the done message
    while(kj::size(_blockingReadFulfillers) > 0){
      auto& brf = _blockingReadFulfillers.back();
      brf->fulfill(kj::Maybe<AnyPointerMsg::Reader>());
      _blockingReadFulfillers.pop_back();
      KJ_LOG(INFO, "Channel::closedWriter: sent done to reader on last finished writer");
    }
    KJ_LOG(INFO, "Channel::closedWriter: blockingReadFulfillers.size():", kj::size(_blockingReadFulfillers));
    KJ_LOG(INFO, "Channel::closedWriter: blockingWriteFulfillers.size():", kj::size(_blockingWriteFulfillers));
  }
}

kj::Promise<void> Channel::reader(ReaderContext context){
  auto r = kj::heap<Reader>(*this);
  auto id = r->id();
  AnyPointerChannel::ChanReader::Client rc = kj::mv(r);
  _readers.insert(kj::str(id), rc);
  context.getResults().setR(rc);
  return kj::READY_NOW;
}

kj::Promise<void> Channel::writer(WriterContext context){
  auto w = kj::heap<Writer>(*this);
  auto id = w->id();
  AnyPointerChannel::ChanWriter::Client wc = kj::mv(w);
  _writers.insert(kj::str(id), wc);
  context.getResults().setW(wc);
  return kj::READY_NOW;
}

//-----------------------------------------------------------------------------

Reader::Reader(Channel& c) 
: _channel(c)
, _id(kj::str(sole::uuid4().str())) {}

kj::Promise<void> Reader::read(ReadContext context) {
  KJ_REQUIRE(!_closed, "Reader::read: Reader already closed.", _closed);

  auto& c = _channel;
  auto& b = c._buffer;

  // buffer not empty, send next value
  if(b.size() > 0){ 
    auto&& v = b.back();
    context.getResults().setValue(v.get()->getValue());
    b.pop_back();
    
    // unblock a writer unless we're about to close down
    if(!c._blockingWriteFulfillers.empty() && !c._sendCloseOnEmptyBuffer){
      auto&& bwf = c._blockingWriteFulfillers.back();
      bwf->fulfill();
      c._blockingWriteFulfillers.pop_back();
    }
  } else { 
    // buffer is empty, but we are supposed to close down
    if(c._sendCloseOnEmptyBuffer){
      context.getResults().setDone();
      c.closedReader(id());

      // if there are other readers waiting close them as well
      while(!c._blockingReadFulfillers.empty()){
        auto&& brf = c._blockingReadFulfillers.back(); 
        brf->fulfill(nullptr);
        c._blockingReadFulfillers.pop_back();
      }
    } else { // block because no value to read
      auto paf = kj::newPromiseAndFulfiller<kj::Maybe<AnyPointerMsg::Reader>>();
      c._blockingReadFulfillers.push_front(kj::mv(paf.fulfiller)); 
      return paf.promise.then([context, this](kj::Maybe<AnyPointerMsg::Reader> msg) mutable {
        KJ_REQUIRE(!_closed, "Reader already closed.", _closed);

        if(_channel._sendCloseOnEmptyBuffer && msg == nullptr){
          //KJ_DBG("setResults");
          context.getResults().setDone();
          KJ_LOG(INFO, "Reader::read::promise_lambda: sending done to reader");
          _channel.closedReader(id());
        } else {
          KJ_IF_MAYBE(m, msg){
            //KJ_DBG("Reader::read setResults");
            context.getResults().setValue(m->getValue());
            KJ_LOG(INFO, "Reader::read::promise_lambda: sending value to reader");
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
  KJ_REQUIRE(!_closed, "Writer::write: Writer already closed.", _closed);
  
  auto v = context.getParams();
  auto& c = _channel;
  auto& b = c._buffer;

  // if we received a done, this writer can be removed
  if(v.isDone()){
    c.closedWriter(id());
  } else if (c._blockingReadFulfillers.size() > 0) { // there's a reader waiting
    auto&& brf = c._blockingReadFulfillers.back(); 
    brf->fulfill(v);
    c._blockingReadFulfillers.pop_back();
  } else if (b.size() < c._bufferSize) { // there space to store the message
    b.push_front(capnp::clone(v));
  } else { // block until buffer has space 
    auto paf = kj::newPromiseAndFulfiller<void>();
    c._blockingWriteFulfillers.push_front(kj::mv(paf.fulfiller)); 
    return paf.promise.then([context, this]() mutable {
      KJ_REQUIRE(!_closed, "Writer::write::promise_lambda: Writer already closed.", _closed);
      auto v = context.getParams();
      _channel._buffer.push_front(capnp::clone(v));
      KJ_LOG(INFO, "Writer:write::promise_lambda: wrote value to buffer");
    });
  }

  return kj::READY_NOW;
}
