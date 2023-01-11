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

#include <iostream>

#include <kj/debug.h>
#include <kj/common.h>
#include <kj/main.h>
#include <kj/string.h>
#include <kj/vector.h>

#include "tools/algorithms.h"

#include "rpc-connection-manager.h"
#include "common.h"
#include "sole.hpp"
#include "restorable-service-main.h"

#include "channel.h"
#include "common.capnp.h"

namespace mas { 
namespace infrastructure { 
namespace common {

using RSM = mas::infrastructure::common::RestorableServiceMain;

class ChannelMain : public RSM
{
public:
  ChannelMain(kj::ProcessContext& context) 
  : RSM(context, "Channel v0.1", "Offers a channel service.")
  {}

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

    auto ownedChannel = kj::heap<Channel>(name, description, bufferSize);
    auto channel = ownedChannel.get();
    AnyPointerChannel::Client channelClient = kj::mv(ownedChannel);
    KJ_LOG(INFO, "created channel");

    startRestorerSetup(channelClient);
    channel->setRestorer(restorer);

    auto channelSR = restorer->saveStr(channelClient, nullptr, nullptr, false).wait(ioContext.waitScope).sturdyRef;
    if(outputSturdyRefs && channelSR.size() > 0) std::cout << "channelSR=" << channelSR.cStr() << std::endl;

    for(const auto& srt : readerSrts){ 
      auto reader = channelClient.readerRequest().send().wait(ioContext.waitScope).getR();
      auto readerSR = restorer->saveStr(reader, srt, nullptr, false, nullptr, false).wait(ioContext.waitScope).sturdyRef;  
      if(outputSturdyRefs && channelSR.size() > 0) std::cout << "readerSR=" << readerSR.cStr() << std::endl;
    }
    for(const auto& srt : writerSrts){
      auto writer = channelClient.writerRequest().send().wait(ioContext.waitScope).getW();
      auto writerSR = restorer->saveStr(writer, srt, nullptr, false, nullptr, false).wait(ioContext.waitScope).sturdyRef;
      if(outputSturdyRefs && writerSR.size() > 0) std::cout << "writerSR=" << writerSR.cStr() << std::endl;
    }

    // Run forever, accepting connections and handling requests.
    kj::NEVER_DONE.wait(ioContext.waitScope);
  }

  kj::MainFunc getMain()
  {
    return addRestorableServiceOptions()
      .addOptionWithArg({'b', "buffer-size"}, KJ_BIND_METHOD(*this, setBufferSize),
                        "<buffer-size=1>", "Set buffer size of channel.")
      .addOptionWithArg({'r', "reader_srts"}, KJ_BIND_METHOD(*this, setReaderSrts),
                        "<Sturdy_ref_token_1,[Sturdy_ref_token_2],...>", "Create readers for given sturdy ref tokens.")
      .addOptionWithArg({'w', "writer_srts"}, KJ_BIND_METHOD(*this, setWriterSrts),
                        "<Sturdy_ref_token_1,[Sturdy_ref_token_2],...>", "Create writers for given sturdy ref tokens.")      
      .callAfterParsing(KJ_BIND_METHOD(*this, startChannel))
      .build();
  }

private:
  uint bufferSize{1};
  kj::Vector<kj::String> readerSrts;
  kj::Vector<kj::String> writerSrts;
};

} // namespace common
} // namespace infrastructure
} // namespace mas

KJ_MAIN(mas::infrastructure::common::ChannelMain)
