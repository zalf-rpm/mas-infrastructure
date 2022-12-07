/* This Source Code Form is subject to the terms of the Mozilla Public
* License, v. 2.0. If a copy of the MPL was not distributed with this
* file, You can obtain one at http://mozilla.org/MPL/2.0/. */

/*
Authors:
Michael Berg <michael.berg@zalf.de>

Maintainers:
Currently maintained by the authors.

This file is part of the ZALF model and simulation infrastructure.
Copyright (C) Leibniz Centre for Agricultural Landscape Research (ZALF)
*/

#include <kj/debug.h>
#include <kj/common.h>
#include <kj/main.h>
#include <kj/string.h>
#include <kj/vector.h>

#include "tools/algorithms.h"

#include "rpc-connection-manager.h"
#include "common.h"
#include "sole.hpp"

#include "channel.h"
#include "common.capnp.h"

namespace mas { 
namespace infrastructure { 
namespace common {

class ChannelMain
{
public:
  ChannelMain(kj::ProcessContext& context) 
  : restorer(kj::heap<Restorer>())
  , conMan(kj::heap<ConnectionManager>())
  , context(context)
  , ioContext(kj::setupAsyncIo()) {}

  kj::MainBuilder::Validity setName(kj::StringPtr n) { name = kj::str(n); return true; }

  kj::MainBuilder::Validity setHost(kj::StringPtr name) { host = kj::str(name); return true; }

  kj::MainBuilder::Validity setLocalHost(kj::StringPtr h) { localHost = kj::str(h); return true; }

  kj::MainBuilder::Validity setPort(kj::StringPtr name) { port = std::max(0, std::stoi(name.cStr())); return true; }

  kj::MainBuilder::Validity setCheckPort(kj::StringPtr portStr) { checkPort = portStr.parseAs<int>(); return true; }

  kj::MainBuilder::Validity setCheckIP(kj::StringPtr ip) { checkIP = kj::str(ip); return true; }

  kj::MainBuilder::Validity setBufferSize(kj::StringPtr name) { bufferSize = std::max(1, std::stoi(name.cStr())); return true; }

  kj::MainBuilder::Validity setReaderSrts(kj::StringPtr name) {
    for(auto& s : splitString(name, ",")) readerSrts.add(kj::str(s));
    return true;
  }

  kj::MainBuilder::Validity setWriterSrts(kj::StringPtr name) {
    for(auto& s : splitString(name, ",")) writerSrts.add(kj::str(s));
    return true;
  }

  kj::MainBuilder::Validity startChannel() {
    if(readerSrts.size() == 0) readerSrts.add(kj::str(sole::uuid4().str()));
    if(writerSrts.size() == 0) writerSrts.add(kj::str(sole::uuid4().str()));

    KJ_LOG(INFO, "Channel::startChannel: starting channel");

    auto& restorerRef = *restorer;
    mas::schema::persistence::Restorer::Client restorerClient = kj::mv(restorer);
    auto channel = kj::heap<Channel>(&restorerRef, name, bufferSize);
    auto& channelRef = *channel;
    AnyPointerChannel::Client channelClient = kj::mv(channel);
    KJ_LOG(INFO, "Channel::startChannel: created channel");
    
    KJ_LOG(INFO, "Channel::startChannel trying to bind to", host, port);
    auto portPromise = conMan->bind(ioContext, restorerClient, host, port);
    auto succAndIP = infrastructure::common::getLocalIP(checkIP, checkPort);
    if(kj::get<0>(succAndIP)) restorerRef.setHost(kj::get<1>(succAndIP));
    else restorerRef.setHost(localHost);
    auto port = portPromise.wait(ioContext.waitScope);
    restorerRef.setPort(port);
    KJ_LOG(INFO, "Channel::startChannel bound to", host, port);

    auto restorerSR = restorerRef.sturdyRefStr("");
    auto channelSR = kj::get<0>(restorerRef.saveStr(channelClient, nullptr, nullptr, false).wait(ioContext.waitScope));
    KJ_LOG(INFO, channelSR);

    for(const auto& srt : readerSrts){ 
      auto reader = channelClient.readerRequest().send().wait(ioContext.waitScope).getR();
      auto readerSR = kj::get<0>(restorerRef.saveStr(reader, srt.asPtr(), nullptr, false).wait(ioContext.waitScope));  
      KJ_LOG(INFO, readerSR);
    }
    for(const auto& srt : writerSrts){
      auto writer = channelClient.writerRequest().send().wait(ioContext.waitScope).getW();
      auto writerSR = kj::get<0>(restorerRef.saveStr(writer, srt.asPtr(), nullptr, false).wait(ioContext.waitScope));
      KJ_LOG(INFO, writerSR);
    }

    KJ_LOG(INFO, restorerSR);

    // Run forever, accepting connections and handling requests.
    kj::NEVER_DONE.wait(ioContext.waitScope);
  }

  kj::MainFunc getMain()
  {
    return kj::MainBuilder(context, "Channel v0.1", "Offers a channel service.")
      .addOptionWithArg({'n', "name"}, KJ_BIND_METHOD(*this, setName),
                        "<channel-name>", "Give channel a name.")
      .addOptionWithArg({'b', "buffer-size"}, KJ_BIND_METHOD(*this, setBufferSize),
                        "<buffer-size=1>", "Set buffer size of channel.")
      .addOptionWithArg({'h', "host"}, KJ_BIND_METHOD(*this, setHost),
                        "<host-IP>", "Set host IP.")
      .addOptionWithArg({'p', "port"}, KJ_BIND_METHOD(*this, setPort),
                        "<port>", "Set port.")
      .addOptionWithArg({"local_host (default: localhost)"}, KJ_BIND_METHOD(*this, setLocalHost),
                        "<IP_or_host_address>", "Use this host for sturdy reference creation.")
      .addOptionWithArg({"check_IP"}, KJ_BIND_METHOD(*this, setCheckIP),
                        "<IPv4 (default: 8.8.8.8)>", "IP to connect to in order to find local outside IP.")
      .addOptionWithArg({"check_port"}, KJ_BIND_METHOD(*this, setCheckPort),
                        "<port (default: 53)>", "Port to connect to in order to find local outside IP.")
      .addOptionWithArg({'r', "reader_srts"}, KJ_BIND_METHOD(*this, setReaderSrts),
                        "<Sturdy_ref_token_1,[Sturdy_ref_token_2],...>", "Create readers for given sturdy ref tokens.")
      .addOptionWithArg({'w', "writer_srts"}, KJ_BIND_METHOD(*this, setWriterSrts),
                        "<Sturdy_ref_token_1,[Sturdy_ref_token_2],...>", "Create writers for given sturdy ref tokens.")      
      .callAfterParsing(KJ_BIND_METHOD(*this, startChannel))
      .build();
  }

private:
  kj::Own<Restorer> restorer;
  kj::Own<ConnectionManager> conMan;
  kj::String name;
  kj::String host{kj::str("*")};
  kj::String localHost{kj::str("localhost")};
  int port{0};
  int checkPort{0};
  kj::String checkIP;
  uint bufferSize{1};
  kj::Vector<kj::String> readerSrts;
  kj::Vector<kj::String> writerSrts;
  kj::ProcessContext &context;
  kj::AsyncIoContext ioContext;
};

} // namespace common
} // namespace infrastructure
} // namespace mas

KJ_MAIN(mas::infrastructure::common::ChannelMain)
