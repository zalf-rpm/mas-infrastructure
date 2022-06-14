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

#include <iostream>
#include <string>
#include <vector>
#include <chrono>
#include <thread>

#include <kj/debug.h>
#include <kj/common.h>
#include <kj/main.h>
#include <kj/string.h>

#include "rpc-connections.h"
#include "common.h"
#include "x.capnp.h"

class ReaderSrv final : public Y::Server
{
public:
  ReaderSrv() {}

  virtual ~ReaderSrv() noexcept(false) {}

  kj::Promise<void> m(MContext context) override {
    auto hello = context.getParams().getHello();
    if (i % 10000 == 0) { std::cout << "."; std::cout.flush(); }
    i++;
    if (hello == "done") exit(0);
    return kj::READY_NOW;
  }

  int i{0};
};

class TestReaderSrvMain
{
public:
  TestReaderSrvMain(kj::ProcessContext &context) : context(context), ioContext(kj::setupAsyncIo()) {}

  kj::MainBuilder::Validity setName(kj::StringPtr n) { name = n; return true; }

  kj::MainBuilder::Validity setHost(kj::StringPtr name) { host = name; return true; }

  kj::MainBuilder::Validity setPort(kj::StringPtr name) { port = std::max(0, std::stoi(name.cStr())); return true; }

  kj::MainBuilder::Validity startReader()
  {
    KJ_LOG(INFO, "TestReaderSrvMain::startReader: starting reader");

    auto rdr = kj::heap<ReaderSrv>();
    auto proms = conMan.bind(ioContext, kj::mv(rdr), host, port);
    kj::NEVER_DONE.wait(ioContext.waitScope);

    KJ_LOG(INFO, "TestReaderSrvMain::startReader: stopping reader");
    return true;
  }

  kj::MainFunc getMain()
  {
    return kj::MainBuilder(context, "Test reader v0.1", "reader to test c++ channel")
      .addOptionWithArg({'n', "name"}, KJ_BIND_METHOD(*this, setName),
                        "<reader-name>", "Give reader a name.")
      .addOptionWithArg({'h', "host"}, KJ_BIND_METHOD(*this, setHost),
                        "<host-IP>", "Set host IP.")
      .addOptionWithArg({'p', "port"}, KJ_BIND_METHOD(*this, setPort),
                        "<port>", "Set port.")
      .callAfterParsing(KJ_BIND_METHOD(*this, startReader))
      .build();
  }

private:
  mas::infrastructure::common::ConnectionManager conMan;
  kj::StringPtr name;
  kj::StringPtr host;
  int port{0};
  kj::ProcessContext &context;
  kj::AsyncIoContext ioContext;
};

KJ_MAIN(TestReaderSrvMain)
