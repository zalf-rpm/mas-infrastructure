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

#include "common/rpc-connection-manager.h"
#include "common/common.h"

class TestReaderMain
{
public:
  TestReaderMain(kj::ProcessContext &context) : context(context), ioContext(kj::setupAsyncIo()) {}

  kj::MainBuilder::Validity setName(kj::StringPtr n) { name = n; return true; }

  kj::MainBuilder::Validity setInSr(kj::StringPtr name) { inSr = name; return true; }

  kj::MainBuilder::Validity startReader()
  {
    KJ_LOG(INFO, "TestReaderMain::startReader: starting reader");

    typedef mas::schema::common::IP IP;
    typedef mas::schema::common::Channel<IP> Channel;
    auto inp = conMan.tryConnectB(ioContext, inSr.cStr()).castAs<Channel::ChanReader>();

    int i = 0;
    while(true){
      auto msg = inp.readRequest().send().wait(ioContext.waitScope);
      if (msg.isDone()) { std::cout << "done" << std::endl; break; }
      else 
      {
        auto inIp = msg.getValue();
        auto txt = inIp.getContent().getAs<capnp::Text>();
        if (i % 10000 == 0) { std::cout << "."; std::cout.flush(); }
        //std::this_thread::sleep_for(std::chrono::milliseconds(10));
      }
      i++;
    }

    KJ_LOG(INFO, "TestReaderMain::startReader: stopping reader");
    return true;
  }

  kj::MainFunc getMain()
  {
    return kj::MainBuilder(context, "Test reader v0.1", "reader to test c++ channel")
      .addOptionWithArg({'n', "name"}, KJ_BIND_METHOD(*this, setName),
                        "<reader-name>", "Give reader a name.")
      .expectArg("<input_sturdy_ref>", KJ_BIND_METHOD(*this, setInSr))
      .callAfterParsing(KJ_BIND_METHOD(*this, startReader))
      .build();
  }

private:
  mas::infrastructure::common::ConnectionManager conMan;
  kj::StringPtr name;
  kj::StringPtr inSr;
  kj::ProcessContext &context;
  kj::AsyncIoContext ioContext;
};

KJ_MAIN(TestReaderMain)
