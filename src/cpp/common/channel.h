/* This Source Code Form is subject to the terms of the Mozilla Public
* License, v. 2.0. If a copy of the MPL was not distributed with this
* file, You can obtain one at http://mozilla.org/MPL/2.0/. */

/*
Authors:
Michael Berg <michael.berg-mohnicke@zalf.de>

Maintainers:
Currently maintained by the authors.

Copyright (C) Leibniz Centre for Agricultural Landscape Research (ZALF)
*/

#pragma once

#include <functional>
#include <string>
#include <deque>

#include <kj/debug.h>
#include <kj/common.h>
#include <kj/string.h>
#include <kj/vector.h>
#include <kj/map.h>
#include <kj/memory.h>
#include <kj/thread.h>
#include <kj/async.h>

#include <capnp/any.h>
#include <capnp/rpc-twoparty.h>

#include "common.h"
#include "common.capnp.h"

namespace mas {
  namespace rpc {
    namespace common {

      class Reader;
      class Writer;

      typedef mas::schema::common::Channel<capnp::AnyPointer> AnyPointerChannel;
      typedef typename mas::schema::common::Channel<capnp::AnyPointer>::Msg AnyPointerMsg;

      class Channel final : public AnyPointerChannel::Server
      {
      public:
        Channel(mas::rpc::common::Restorer* restorer, kj::StringPtr name, uint bufferSize);

        virtual ~Channel() noexcept(false) {}

        void closedReader(kj::StringPtr readerId);

        void closedWriter(kj::StringPtr writerId);

        kj::Promise<void> reader(ReaderContext context) override;

        kj::Promise<void> writer(WriterContext context) override;

      private:
        mas::rpc::common::Restorer* _restorer{nullptr};
        kj::String _name;
        kj::HashMap<kj::String, AnyPointerChannel::ChanReader::Client> _readers;
        kj::HashMap<kj::String, AnyPointerChannel::ChanWriter::Client> _writers;
        std::deque<kj::Own<kj::PromiseFulfiller<kj::Maybe<AnyPointerMsg::Reader>>>> _blockingReadFulfillers;
        std::deque<kj::Own<kj::PromiseFulfiller<void>>> _blockingWriteFulfillers;
        uint _bufferSize{1};
        std::deque<kj::Own<kj::Decay<AnyPointerMsg::Reader>>> _buffer;
        AnyPointerChannel::CloseSemantics _autoCloseSemantics {AnyPointerChannel::CloseSemantics::FBP};
        bool _sendCloseOnEmptyBuffer{false};
        friend class Reader;
        friend class Writer;
      };
      

      //-----------------------------------------------------------------------------

      class Reader final : public mas::schema::common::Channel<capnp::AnyPointer>::ChanReader::Server {
      public:
        Reader(Channel& c);

        virtual ~Reader() noexcept(false) {}

        kj::Promise<void> read(ReadContext context) override;

        kj::StringPtr id() const { return _id; }

      private:
        Channel& _channel;
        bool _closed{false};
        kj::String _id;
      };

      //-----------------------------------------------------------------------------

      class Writer final : public mas::schema::common::Channel<capnp::AnyPointer>::ChanWriter::Server {
      public:
        Writer(Channel& c);

        virtual ~Writer() noexcept(false) {}

        kj::Promise<void> write(WriteContext context) override;

        kj::StringPtr id() const { return _id; }

      private:
        Channel& _channel;
        bool _closed{false};
        kj::String _id;
      };
    }
  }
}
