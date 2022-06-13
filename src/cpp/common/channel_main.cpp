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

#include "tools/algorithms.h"

#include "rpc-connections.h"
#include "common.h"
#include "sole.hpp"

#include "channel.h"
#include "common.capnp.h"

class ChannelMain
{
public:
  ChannelMain(kj::ProcessContext &context) : context(context) {}

  kj::MainBuilder::Validity setName(kj::StringPtr n) { name = n; return true; }

  kj::MainBuilder::Validity setHost(kj::StringPtr name) { host = name; return true; }

  kj::MainBuilder::Validity setPort(kj::StringPtr name) { port = std::max(0, std::stoi(name.cStr())); return true; }

  kj::MainBuilder::Validity setReaderSrts(kj::StringPtr name)
  {
    readerSrts = Tools::splitString(name.cStr(), ",");
    return true;
  }

  kj::MainBuilder::Validity setWriterSrts(kj::StringPtr name)
  {
    writerSrts = Tools::splitString(name.cStr(), ",");
    return true;
  }

  kj::MainBuilder::Validity startChannel()
  {
    auto ioContext = kj::setupAsyncIo();

    if(readerSrts.empty()) readerSrts.push_back(sole::uuid4().str());
    if(writerSrts.empty()) writerSrts.push_back(sole::uuid4().str());

    KJ_LOG(INFO, "starting channel");

    auto restorer = kj::heap<mas::rpc::common::Restorer>();
    auto& restorerRef = *restorer;
    mas::schema::persistence::Restorer::Client restorerClient = kj::mv(restorer);
    auto channel = kj::heap<mas::rpc::common::Channel>(&restorerRef, name);//, bufferSize);
    auto& channelRef = *channel;
    mas::rpc::common::AnyPointerChannel::Client channelClient = kj::mv(channel);
    KJ_LOG(INFO, "created monica");
    
    KJ_LOG(INFO, "Channel: trying to bind to", host, port);
    auto proms = conMan.bind(ioContext, restorerClient, host, port);
    auto hostPromise = proms.first.fork().addBranch();
    auto hostStr = hostPromise.wait(ioContext.waitScope);
    restorerRef.setHost(host.cStr());//addrStr);
    auto portPromise = proms.second.fork().addBranch();
    auto port = portPromise.wait(ioContext.waitScope);
    restorerRef.setPort(port);
    KJ_LOG(INFO, "Channel: bound to", host, port);

    auto restorerSR = restorerRef.sturdyRef();
    auto channelSRs = restorerRef.save(channelClient);
    KJ_LOG(INFO, "Channel:", channelSRs.first);

    for(auto srt : readerSrts){ 
      auto reader = channelClient.readerRequest().send().wait(ioContext.waitScope).getR();
      auto readerSRs = restorerRef.save(reader, srt, false);  
      KJ_LOG(INFO, "Channel:", readerSRs.first);
    }
    for(auto srt : writerSrts){
      auto writer = channelClient.writerRequest().send().wait(ioContext.waitScope).getW();
      auto writerSRs = restorerRef.save(writer, srt, false);
      KJ_LOG(INFO, "Channel:", writerSRs.first);
    }

    KJ_LOG(INFO, "Channel:", restorerSR);

    // Run forever, accepting connections and handling requests.
    kj::NEVER_DONE.wait(ioContext.waitScope);
  }

  kj::MainFunc getMain()
  {
    return kj::MainBuilder(context, "Channel v0.1", "Offers a channel service.")
      .addOptionWithArg({'n', "name"}, KJ_BIND_METHOD(*this, setName),
                        "<channel-name>", "Give channel a name.")
      .addOptionWithArg({'h', "host"}, KJ_BIND_METHOD(*this, setHost),
                        "<host-IP>", "Set host IP.")
      .addOptionWithArg({'p', "port"}, KJ_BIND_METHOD(*this, setPort),
                        "<port>", "Set port.")
      .addOptionWithArg({'r', "reader_srts"}, KJ_BIND_METHOD(*this, setReaderSrts),
                        "<Sturdy_ref_token_1,[Sturdy_ref_token_2],...>", "Create readers for given sturdy ref tokens.")
      .addOptionWithArg({'w', "writer_srts"}, KJ_BIND_METHOD(*this, setWriterSrts),
                        "<Sturdy_ref_token_1,[Sturdy_ref_token_2],...>", "Create writers for given sturdy ref tokens.")      
      .callAfterParsing(KJ_BIND_METHOD(*this, startChannel))
      .build();
  }

private:
  mas::infrastructure::common::ConnectionManager conMan;
  kj::StringPtr name;
  kj::StringPtr host;
  int port{0};
  std::vector<std::string> readerSrts;
  std::vector<std::string> writerSrts;
  kj::ProcessContext &context;
};

KJ_MAIN(ChannelMain)
