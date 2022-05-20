/* This Source Code Form is subject to the terms of the Mozilla Public
* License, v. 2.0. If a copy of the MPL was not distributed with this
* file, You can obtain one at http://mozilla.org/MPL/2.0/. */

/*
Authors:
Michael Berg <michael.berg-mohnicke@zalf.de>

Maintainers:
Currently maintained by the authors.

This file is part of the MONICA model.
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
#include "x.capnp.h"

namespace mas {
  namespace rpc {
    namespace common {

      class Reader;
      class Writer;

      typedef mas::schema::common::Channel<capnp::AnyPointer> AnyPointerChannel;

      //template<typename T>
      class Channel final : public AnyPointerChannel::Server
      {
      public:
        Channel(mas::rpc::common::Restorer* restorer, kj::String name, uint bufferSize = 1);

        virtual ~Channel() noexcept(false) {}

        void closedReader(Reader& reader);

        void closedWriter(Writer& writer);

        kj::Promise<void> reader(ReaderContext context) override;
        //Reader& createReader();

        kj::Promise<void> writer(WriterContext context) override;
        Writer& createWriter();

        /*
        // restore @0 (srToken :Text) -> (cap :Capability);
        kj::Promise<void> restore(RestoreContext context) override;

        int getPort() const { return _port; }
        void setPort(int p) { _port = p; }

        std::string getHost() const { return _host; }
        void setHost(std::string h) { _host = h; }

        std::string sturdyRef(std::string srToken = "") const;

        std::pair<std::string, std::string> save(capnp::Capability::Client cap);

        void unsave(std::string srToken);
        */

        //kj::Promise<void> save(SaveContext context) override;

      private:
        mas::rpc::common::Restorer* _restorer{nullptr};
        kj::String _name;
        kj::Vector<AnyPointerChannel::ChanReader::Client> _readers;
        kj::Vector<AnyPointerChannel::ChanWriter::Client> _writers;
        //kj::Vector<capnp::AnyPointer::Reader> _buffer;
        //kj::Vector<X::Client> _buffer;
        kj::Vector<S::Builder> _buffer;
        uint _bri{0}; // buffer read index -> points to cell to read next from
        uint _bwi{0}; // buffer write index -> points to cell to write next to
        uint _bufferSize{1};
        std::deque<kj::Own<kj::PromiseFulfiller<void>>> _blockingReadFulfillers;
        uint _brfInsertIndex{0};
        std::deque<kj::Own<kj::PromiseFulfiller<void>>> _blockingWriteFulfillers;
        uint _bwfInsertIndex{0};
        friend class Reader;
        friend class Writer;
      };

      //-----------------------------------------------------------------------------

      //template<typename T>
      class Reader final : public mas::schema::common::Channel<capnp::AnyPointer>::ChanReader::Server {
      public:
        Reader(Channel& c) : _channel(c) {}

        virtual ~Reader() noexcept(false) {}

        kj::Promise<void> read(ReadContext context) override;

      private:
        Channel& _channel;
        bool _closed{false};
        bool _sendCloseOnEmptyBuffer{false};
      };

      //-----------------------------------------------------------------------------

      class Writer final : public mas::schema::common::Channel<capnp::AnyPointer>::ChanWriter::Server {
      public:
        Writer(Channel& c) : _channel(c) {}

        virtual ~Writer() noexcept(false) {}

        kj::Promise<void> write(WriteContext context) override;

      private:
        Channel& _channel;
        bool _closed{false};
      };
    }
  }
}
