/* This Source Code Form is subject to the terms of the Mozilla Public
* License, v. 2.0. If a copy of the MPL was not distributed with this
* file, You can obtain one at http://mozilla.org/MPL/2.0/. */

/*
Authors:
Michael Berg <michael.berg-mohnicke@zalf.de>

Maintainers:
Currently maintained by the authors.

This file is part of the ZALF model and simulation infrastructure.
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
#include "restorer.h"
#include "common.capnp.h"
#include "fbp.capnp.h"

namespace mas::infrastructure::common {

class Reader;
class Writer;

typedef mas::schema::fbp::Channel<capnp::AnyPointer> AnyPointerChannel;
typedef typename AnyPointerChannel::Msg AnyPointerMsg;

class Channel final : public AnyPointerChannel::Server
{
public:
  Channel(kj::StringPtr name, kj::StringPtr description, uint64_t bufferSize, Restorer* restorer = nullptr);

  ~Channel();

  kj::Promise<void> info(InfoContext context) override;

  kj::Promise<void> save(SaveContext context) override;

  void closedReader(kj::StringPtr readerId);

  void closedWriter(kj::StringPtr writerId);

  kj::Promise<void> reader(ReaderContext context) override;

  kj::Promise<void> writer(WriterContext context) override;

  AnyPointerChannel::Client getClient();
  void setClient(AnyPointerChannel::Client c);

  //mas::schema::common::Action::Client getUnregisterAction();
  //void setUnregisterAction(mas::schema::common::Action::Client unreg);

  void setRestorer(Restorer* r);

private:
  struct Impl;
  kj::Own<Impl> impl;

  friend class Reader;
  friend class Writer;
};

class Reader final : public AnyPointerChannel::ChanReader::Server {
public:
  Reader(Channel& c);

  ~Reader() = default;

  kj::Promise<void> read(ReadContext context) override;

  kj::StringPtr id() const { return _id; }

private:
  Channel& _channel;
  bool _closed{false};
  kj::String _id;
};

class Writer final : public AnyPointerChannel::ChanWriter::Server {
public:
  explicit Writer(Channel& c);

  ~Writer() = default;

  kj::Promise<void> write(WriteContext context) override;

  kj::StringPtr id() const { return _id; }

private:
  Channel& _channel;
  bool _closed{false};
  kj::String _id;
};

} // namespace mas::infrastructure::common
