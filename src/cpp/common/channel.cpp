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
//using namespace Tools;
using namespace mas::rpc::common;

//-----------------------------------------------------------------------------

Channel::Channel(mas::rpc::common::Restorer* restorer, kj::String name, uint bufferSize) 
: _restorer(restorer)
, _name(kj::mv(name))
//, _buffer(bufferSize)
{
  //for(int i = 0; i < bufferSize; i++) _buffer.add(nullptr);//capnp::AnyPointer::Reader());//nullptr);
}

void Channel::closedReader(Reader& reader){
  cout << "Channel::closedReader" << endl;
  // for(int i = 0; i < _readers.size(); i++){
  //   if(_readers[i].get() == &reader)

  // }
  // _readers.removeLast(reader);
}

void Channel::closedWriter(Writer& writer){
  cout << "Channel::closed_writer" << endl;
  
  // self._writers.remove(writer)
  // if self._auto_close_semantics == "fbp" and len(self._writers) == 0:
  //     for r in self._readers:
  //         r.send_close_on_empty_buffer = True
  //     # as we just received a done message which should be distributed and would
  //     # fill the buffer, unblock all readers, so they send the done message
  //     while len(self._blocking_read_fulfillers) > 0:
  //         self._blocking_read_fulfillers.popleft().fulfill()
  
}

kj::Promise<void> Channel::reader(ReaderContext context){
  _readers.add(kj::heap<Reader>(*this));
  context.getResults().setR(_readers.back());
  return kj::READY_NOW;
}


// Reader& Channel::createReader(){
//   auto ownr = kj::heap<Reader>(*this);
//   auto r = ownr.get();
//   _readers.add(kj::mv(ownr));
//   return *r;
// }

kj::Promise<void> Channel::writer(WriterContext context){
  _writers.add(kj::heap<Writer>(*this));
  context.getResults().setW(_writers.back());
  return kj::READY_NOW;
}


// Writer& Channel::createWriter(){
//   auto ow = kj::heap<Writer>(*this);
//   auto w = ow.get();
//   _writers.add(kj::mv(ow));
//   return *w;
// }


// kj::Promise<void> Channel::save(SaveContext context) {
//   std::cout << "Channel::save message received" << std::endl;
//   if(_restorer)
//   {
//     auto srs = _restorer->save(_client);
//     context.getResults().setSturdyRef(srs.first);
//     context.getResults().setUnsaveSR(srs.second);
//   }
//   return kj::READY_NOW;
// }

//-----------------------------------------------------------------------------

kj::Promise<void> Reader::read(ReadContext context) {
  KJ_REQUIRE(!_closed, "Reader already closed.", _closed);

  auto& c = _channel;

  auto setResults = [context, this](AnyPointerMsg::Reader msg) mutable {
    KJ_REQUIRE(!_closed, "Reader already closed.", _closed);

    // if(_sendCloseOnEmptyBuffer){
    //   KJ_DBG(kj::str("setResults"));
    //   context.getResults().setDone();
    //   c.closedReader(*this);
    // } else {
      KJ_DBG(kj::str("setResultsFromBuffer"));
      context.getResults().setValue(msg.getValue());
    //}
  };


  if (!c._blockingWriteFulfillers.empty()){
    auto& bwf = c._blockingWriteFulfillers.front();
    bwf->fulfill(context.getResults());
    c._blockingWriteFulfillers.pop_front();
  } else {
    auto paf = kj::newPromiseAndFulfiller<AnyPointerMsg::Reader>();
    c._blockingReadFulfillers.push_back(kj::mv(paf.fulfiller)); 
    return paf.promise.then(setResults);
  }

  return kj::READY_NOW;
}

/*
kj::Promise<void> Reader::read(ReadContext context) {
  KJ_REQUIRE(!_closed, "Reader already closed.", _closed);

  auto& c = _channel;
  auto& b = c._buffer;

  auto setResultsFromBuffer = [&](){
    KJ_REQUIRE(!_closed, "Reader already closed.", _closed);

    if(_sendCloseOnEmptyBuffer && c._bri == c._bwi){
      KJ_DBG(kj::str("setResultsFromBuffer: c._bri: ", c._bri, " c._bwi: ", c._bwi));
      context.getResults().setDone();
      c.closedReader(*this);
    } else if(c._bri < c._bwi) {
      KJ_DBG(kj::str("setResultsFromBuffer: c._bri: ", c._bri, " bri: ", c._bri % c._bufferSize));
      //auto x = kj::mv(b[c._bri % c._bufferSize]);
      //auto res = context.getResults();
      //res.getValue().setAs<X>(x);
      //context.getResults().getValue().setAs<capnp::AnyPointer>(b[c._bri % c._bufferSize]);
      context.getResults().getValue().setAs<S>(b[c._bri % c._bufferSize]);
      c._bri++;
    }
  };

  // read value non-blocking
  if(c._bri < c._bwi){
    setResultsFromBuffer();
    KJ_DBG(kj::str(c._name, "r "));

    // unblock writers unless we're about to close down
    if(c._bri == c._bwi && !_sendCloseOnEmptyBuffer) {
      // unblock potentially waiting writers
      while(c._blockingWriteFulfillers.size() > 0){
        auto& bwf = c._blockingWriteFulfillers.front();
        bwf->fulfill();
        c._blockingWriteFulfillers.pop_front();

        //leave loop if no more buffer space
        auto bwi = c._bwi % c._bufferSize;
        auto filled = c._bwi - c._bri;
        if(bwi >= c._bufferSize || filled >= c._bufferSize) break;
      }
    }
  } else { // block because no value to read
    // if the channel is supposed to close down, just generate a close message
    if(_sendCloseOnEmptyBuffer){
        KJ_DBG("Reader::read_context -> len(b) == 0 -> send done");
        setResultsFromBuffer();

        // as the buffer is empty and we're supposed to shut down as any other reader
        // fulfill waiting readers with done messages
        while(c._blockingReadFulfillers.size() > 0){
          auto& brf = c._blockingReadFulfillers.front();
          brf->fulfill();
          c._blockingReadFulfillers.pop_front();
        }
    } else {
      auto paf = kj::newPromiseAndFulfiller<void>();
      c._blockingReadFulfillers.push_back(kj::mv(paf.fulfiller)); 

      KJ_DBG(kj::str("[", c._name, "r", c._blockingReadFulfillers.size(), "] "));
      return paf.promise.then(setResultsFromBuffer);
    }
  }

  return kj::READY_NOW;
}
*/

//-----------------------------------------------------------------------------

kj::Promise<void> Writer::write(WriteContext context) {
  KJ_REQUIRE(!_closed, "Writer already closed.", _closed);

  auto v = context.getParams();
  auto& c = _channel;
        
  auto setResults = [v, this](AnyPointerMsg::Builder msg) mutable {
    KJ_REQUIRE(!_closed, "Writer already closed.", _closed);
    msg.setValue(v.getValue());
  };

  // if we received a done, this writer can be removed
  if(v.isDone()){
    c.closedWriter(*this);
    return kj::READY_NOW;
  }

  if (!c._blockingReadFulfillers.empty()){
    auto& bwf = c._blockingReadFulfillers.front();
    bwf->fulfill(kj::mv(v));
    c._blockingReadFulfillers.pop_front();
  } else {
    auto paf = kj::newPromiseAndFulfiller<AnyPointerMsg::Builder>();
    c._blockingWriteFulfillers.push_back(kj::mv(paf.fulfiller)); 
    return paf.promise.then(kj::mvCapture(v, setResults));
  }

  return kj::READY_NOW;
}

/*
kj::Promise<void> Writer::write(WriteContext context) {
  KJ_REQUIRE(!_closed, "Writer already closed.", _closed);

  const auto& v = context.getParams();
  auto& c = _channel;
  auto& b = c._buffer;
        
  auto insertIntoBuffer = [&](){
    KJ_REQUIRE(!_closed, "Writer already closed.", _closed);
    //auto val = v.getValue().getAs<X>();
    //auto val = v.getValue().getAs<X>();
    b[c._bwi % c._bufferSize].setC(v.getValue().getAs<S>().getC());//.getAs<X>();
    c._bwi++;
  };

  // if we received a done, this writer can be removed
  if(v.isDone()){
    c.closedWriter(*this);
    return kj::READY_NOW;
  }

  auto bwi = c._bwi % c._bufferSize;
  auto filled = c._bwi - c._bri;

  // write value non-block
  if(bwi < c._bufferSize && filled < c._bufferSize){
    insertIntoBuffer();
    KJ_DBG(kj::str(c._name, "w "));
    // unblock potentially waiting readers
    while(c._bri < c._bwi && c._blockingReadFulfillers.size() > 0){
      auto& brf = c._blockingReadFulfillers.front();
      brf->fulfill();
      c._blockingReadFulfillers.pop_front();
    }
  } else { // block until buffer has space
    auto paf = kj::newPromiseAndFulfiller<void>();
    c._blockingWriteFulfillers.push_back(kj::mv(paf.fulfiller)); 
    KJ_DBG(kj::str("[", c._name, "w", c._blockingWriteFulfillers.size(), "] "));
    return paf.promise.then(insertIntoBuffer);
  }

  return kj::READY_NOW;
}
*/

//-----------------------------------------------------------------------------




