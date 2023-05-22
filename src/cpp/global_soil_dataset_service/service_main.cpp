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

#include <string>
#include <vector>

#include <kj/debug.h>
#include <kj/common.h>
#include <kj/main.h>
#include <kj/string.h>

#include "common/rpc-connection-manager.h"
#include "common/common.h"
#include "service.h"

class ServiceMain {
public:
  explicit ServiceMain(kj::ProcessContext &context) : context(context), ioContext(kj::setupAsyncIo()) {}

  kj::MainBuilder::Validity setName(kj::StringPtr n) {
    name = n;
    return true;
  }

  kj::MainBuilder::Validity setOutSr(kj::StringPtr name) {
    outSr = name;
    return true;
  }

  kj::MainBuilder::Validity setCount(kj::StringPtr name) {
    count = std::stoi(name.cStr());
    return true;
  }

  kj::MainBuilder::Validity startWriter() {
    KJ_LOG(INFO, "TestWriterMain::startWriter: starting writer");

    typedef mas::schema::common::IP IP;
    typedef mas::schema::common::Channel <IP> Channel;
    auto outp = conMan.tryConnectB(ioContext, outSr.cStr()).castAs<Channel::ChanWriter>();

    for (auto i = 1; i <= count; i++) {
      auto wreq = outp.writeRequest();
      auto outIp = wreq.initValue();
      outIp.initContent().setAs<capnp::Text>("Hello_" + std::to_string(i));
      wreq.send().wait(ioContext.waitScope);
    }

    auto wreq = outp.writeRequest();
    wreq.setDone();
    wreq.send().wait(ioContext.waitScope);

    KJ_LOG(INFO, "TestWriterMain::startWriter: stopping writer");
    return true;
  }

  kj::MainFunc getMain() {
    return kj::MainBuilder(context, "Test writer v0.1", "writer to test c++ channel")
        .addOptionWithArg({'n', "name"}, KJ_BIND_METHOD(*this, setName),
                          "<writer-name>", "Give writer a name.")
        .addOptionWithArg({'c', "count"}, KJ_BIND_METHOD(*this, setCount),
                          "<message-count>", "Set how many messages should be sent.")
        .expectArg("<output_sturdy_ref>", KJ_BIND_METHOD(*this, setOutSr))
        .callAfterParsing(KJ_BIND_METHOD(*this, startWriter))
        .build();
  }

private:
  mas::infrastructure::common::ConnectionManager conMan;
  kj::StringPtr name;
  kj::StringPtr outSr;
  int count{0};
  kj::ProcessContext &context;
  kj::AsyncIoContext ioContext;
};

KJ_MAIN(ServiceMain)
