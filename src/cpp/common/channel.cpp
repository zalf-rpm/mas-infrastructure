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

#include "sole.hpp"

using namespace std;
//using namespace Tools;
using namespace mas::rpc::common;

//-----------------------------------------------------------------------------

Channel::Channel(mas::rpc::common::Restorer* restorer, uint bufferSize) 
: _restorer(restorer)
{
  _buffer.resize(bufferSize);
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

Reader& Channel::createReader(){
  auto ownr = kj::heap<Reader>(*this);
  auto r = ownr.get();
  _readers.add(kj::mv(ownr));
  return *r;
}

Writer& Channel::createWriter(){
  auto ow = kj::heap<Writer>(*this);
  auto w = ow.get();
  _writers.add(kj::mv(ow));
  return *w;
}


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

  auto& b = _channel._buffer;

  auto setResultsFromBuffer = [&](){
    KJ_REQUIRE(!_closed, "Reader already closed.", _closed);

    auto bufSize = b.size();
    if(_sendCloseOnEmptyBuffer && bufSize == 0){
      context.getResults().setDone();
      _channel.closedReader(*this);
    } else if(bufSize > 0) {
      context.getResults().setValue(b.front());
      //b.remove(b.front());
    }
  };

  // read value non-blocking
  if(b.size() > 0){
    setResultsFromBuffer();
    cout << _channel._name.cStr() << "r ";

    // unblock writers unless we're about to close down
    if(b.size() == 0 && !_sendCloseOnEmptyBuffer) {
      // unblock potentially waiting writers
      while(b.size() < _channel._bufferSize && _channel._blockingWriteFulfillers.size() > 0){
        auto& bwf = _channel._blockingWriteFulfillers.front();
        //_channel._blockingWriteFulfillers.remove(bwf);
        bwf->fulfill();
      }
    }
  } else { // block because no value to read
    // if the channel is supposed to close down, just generate a close message
    if(_sendCloseOnEmptyBuffer){
        cout << "Reader::read_context -> len(b) == 0 -> send done" << endl;
        setResultsFromBuffer();

        // as the buffer is empty and we're supposed to shut down as any other reader
        // fulfill waiting readers with done messages
        while(_channel._blockingReadFulfillers.size() > 0){
          auto& brf = _channel._blockingReadFulfillers.front();
          //_channel._blockingReadFulfillers.remove(brf);
          brf->fulfill();
        }
    } else {
      auto paf = kj::newPromiseAndFulfiller<void>();
      _channel._blockingReadFulfillers.add(kj::mv(paf.fulfiller)); 

      cout << "[" << _channel._name.cStr() << "r" << _channel._blockingReadFulfillers.size() << "] ";
      return paf.promise.then(setResultsFromBuffer);
    }
  }

  return kj::READY_NOW;
}


//-----------------------------------------------------------------------------

kj::Promise<void> Writer::write(WriteContext context) {
  KJ_REQUIRE(!_closed, "Writer already closed.", _closed);

  const auto& v = context.getParams();
  auto& c = _channel;
  auto& b = c._buffer;
        
  auto appendFromBuffer = [&](){
    KJ_REQUIRE(!_closed, "Writer already closed.", _closed);
    b.add(v.getValue());
  };

  // if we received a done, this writer can be removed
  if(v.isDone()){
    c.closedWriter(*this);
    return kj::READY_NOW;
  }
  
  // write value non-block
  if(b.size() < c._bufferSize){
    appendFromBuffer();
    cout << c._name.cStr() << "w ";
    // unblock potentially waiting readers
    while(b.size() > 0 && c._blockingReadFulfillers.size() > 0){
      //c._blockingReadFulfillers.popleft().fulfill();
    }
  } else { // block until buffer has space
    auto paf = kj::newPromiseAndFulfiller<void>();
    _channel._blockingWriteFulfillers.add(kj::mv(paf.fulfiller)); 
    cout << "[" << c._name.cStr() << "w" << c._blockingWriteFulfillers.size() << "] ";
    return paf.promise.then(appendFromBuffer);
  }

  return kj::READY_NOW;
}

//-----------------------------------------------------------------------------




