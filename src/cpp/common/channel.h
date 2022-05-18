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

#include <kj/debug.h>
#include <kj/common.h>
#include <kj/string.h>
#include <kj/vector.h>
#include <kj/map.h>
#include <kj/memory.h>
#include <capnp/any.h>
#include <kj/async.h>

#include <capnp/rpc-twoparty.h>
#include <kj/thread.h>

#include <functional>
#include <string>

#include "common.capnp.h"

namespace mas {
  namespace rpc {
    namespace common {

      class Reader;
      class Writer;

      //template<typename T>
      class Channel final : public mas::schema::common::Channel<capnp::AnyPointer>::Server
      {
      public:
        Channel(mas::rpc::common::Restorer* restorer, uint bufferSize = 1);

        virtual ~Channel() noexcept(false) {}

        void closedReader(Reader& reader);

        void closedWriter(Writer& writer);

        Reader& createReader();

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
        //std::string _host{ "" };
        //int _port{ 0 };
        //kj::HashMap<kj::String, capnp::Capability::Client> _issuedSRTokens;
        kj::Vector<kj::Own<Reader>> _readers;
        kj::Vector<kj::Own<Writer>> _writers;
        kj::Vector<capnp::AnyPointer::Reader> _buffer;
        uint _bufferSize{1};
        kj::Vector<kj::Own<kj::PromiseFulfiller<void>>> _blockingReadFulfillers;
        kj::Vector<kj::Own<kj::PromiseFulfiller<void>>> _blockingWriteFulfillers;
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
