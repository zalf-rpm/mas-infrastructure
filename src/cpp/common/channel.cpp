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
using namespace mas::rpc::common;

//-----------------------------------------------------------------------------

Channel::Channel(mas::rpc::common::Restorer* restorer, kj::StringPtr name)
: _restorer(restorer)
, _name(kj::str(name))
{
}

void Channel::closedReader(kj::StringPtr readerId){
  KJ_LOG(INFO, "Channel::closedReader");
  _readers.erase(readerId);
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
      auto& brf = _blockingReadFulfillers.front();
      brf->fulfill(kj::Maybe<AnyPointerMsg::Reader>());
      _blockingReadFulfillers.pop_front();
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

  // there's a write waiting
  if (!c._blockingWriteFulfillers.empty()){
    auto& bwf = c._blockingWriteFulfillers.front();
    bwf->fulfill(context.getResults());
    c._blockingWriteFulfillers.pop_front();
  } else if(c._sendCloseOnEmptyBuffer) { // there are no writers left, just close readers
    context.getResults().setDone();
    c.closedReader(id());
  } else { // there's no write for our read
    auto paf = kj::newPromiseAndFulfiller<kj::Maybe<AnyPointerMsg::Reader>>();
    c._blockingReadFulfillers.push_back(kj::mv(paf.fulfiller)); 
    return paf.promise.then([context, this](kj::Maybe<AnyPointerMsg::Reader> msg) mutable {
      KJ_REQUIRE(!_closed, "Reader already closed.", _closed);

      if(_channel._sendCloseOnEmptyBuffer && msg == nullptr){
        //KJ_LOG(DBG, kj::str("setResults"));
        context.getResults().setDone();
        KJ_LOG(INFO, "Reader::read::promise_lambda: sending done to reader");
        _channel.closedReader(id());
      } else {
        KJ_IF_MAYBE(m, msg){
          //KJ_LOG(INFO, kj::str("Reader::read setResults"));
          context.getResults().setValue(m->getValue());
          KJ_LOG(INFO, "Reader::read::promise_lambda: sending value to reader");
        }
      }
    });
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

  // if we receive a message on a new writer reopen an already closed channel
  if (c._sendCloseOnEmptyBuffer) c._sendCloseOnEmptyBuffer = false;

  // if we received a done, this writer can be removed
  if(v.isDone()){
    c.closedWriter(id());
  } else if (!c._blockingReadFulfillers.empty()) { // we got a read waiting
    auto& brf = c._blockingReadFulfillers.front();
    brf->fulfill(kj::mv(v));
    c._blockingReadFulfillers.pop_front();
  } else { // we have to wait for a read to our write
    auto paf = kj::newPromiseAndFulfiller<AnyPointerMsg::Builder>();
    c._blockingWriteFulfillers.push_back(kj::mv(paf.fulfiller)); 
    return paf.promise.then([context, this](AnyPointerMsg::Builder msg) mutable {
      KJ_REQUIRE(!_closed, "Writer::write::promise_lambda: Writer already closed.", _closed);
      auto v = context.getParams();
      msg.setValue(v.getValue());
      KJ_LOG(INFO, "Writer:write::promise_lambda: sending value to reader");
    });
  }

  return kj::READY_NOW;
}

//-----------------------------------------------------------------------------




