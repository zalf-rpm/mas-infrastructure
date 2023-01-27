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

#include "channel.h"
#include "common.h"
#include "restorable-service-main.h"
#include "rpc-connection-manager.h"
#include "sole.hpp"

#include "common.capnp.h"

namespace mas { 
namespace infrastructure { 
namespace common {

class ChannelMain : public RestorableServiceMain
{
public:
  ChannelMain(kj::ProcessContext& context) 
  : RestorableServiceMain(context, "Channel v0.1", "Offers a channel service.")
  {}

  kj::MainBuilder::Validity setBufferSize(kj::StringPtr name) { bufferSize = std::max(1, std::stoi(name.cStr())); return true; }

  kj::MainBuilder::Validity setNoOfReaderWriterPairs(kj::StringPtr no) { 
    auto n = (uint8_t)std::min(std::stoul(no.cStr()), 255UL); 
    for(auto i : kj::zeroTo(n)){ 
      readerSrts.add(kj::str(sole::uuid4().str()));
      writerSrts.add(kj::str(sole::uuid4().str()));
    }
    return true; 
  }

  kj::MainBuilder::Validity setNoOfReaders(kj::StringPtr no) { 
    auto n = (uint8_t)std::min(std::stoul(no.cStr()), 255UL);
    for(auto i : kj::zeroTo(n)) readerSrts.add(kj::str(sole::uuid4().str()));
    return true; 
  }

  kj::MainBuilder::Validity setNoOfWriters(kj::StringPtr no) { 
    auto n = (uint8_t)std::min(std::stoul(no.cStr()), 255UL);
    for(auto i : kj::zeroTo(n)) writerSrts.add(kj::str(sole::uuid4().str()));
    return true;
  }

  kj::MainBuilder::Validity setReaderSrts(kj::StringPtr name) {
    for(auto& s : splitString(name, ",")) readerSrts.add(kj::str(s));
    return true;
  }

  kj::MainBuilder::Validity setWriterSrts(kj::StringPtr name) {
    for(auto& s : splitString(name, ",")) writerSrts.add(kj::str(s));
    return true;
  }

  kj::MainBuilder::Validity startChannel() {
    if(readerSrts.size() == 0) setNoOfReaders("1");
    if(writerSrts.size() == 0) setNoOfWriters("1");

    KJ_LOG(INFO, "starting channel");

    auto ownedChannel = kj::heap<Channel>(name, description, bufferSize);
    auto channel = ownedChannel.get();
    AnyPointerChannel::Client channelClient = kj::mv(ownedChannel);
    KJ_LOG(INFO, "created channel");

    startRestorerSetup(channelClient);
    channel->setRestorer(restorer);

    auto channelSR = restorer->saveStr(channelClient, nullptr, nullptr, false).wait(ioContext.waitScope).sturdyRef;
    if(outputSturdyRefs && channelSR.size() > 0) std::cout << "channelSR=" << channelSR.cStr() << std::endl;

    using SI = mas::schema::common::Channel<capnp::AnyPointer>::StartupInfo;
    using P = mas::schema::common::Pair<capnp::Text, SI>;
    using SIC = mas::schema::common::Channel<P>;

    kj::Maybe<SI::Builder> startupInfo;
    kj::Maybe<capnp::Request<SIC::Msg, SIC::ChanWriter::WriteResults>> infoReq;
    KJ_IF_MAYBE(anyOut, startupInfoWriterClient){
      auto out = anyOut->castAs<SIC::ChanWriter>();
      infoReq = out.writeRequest();
      KJ_IF_MAYBE(req, infoReq){
        auto p = req->initValue();
        p.setFst(startupInfoWriterSRId);
        auto info = p.initSnd();
        info.setBufferSize(bufferSize);
        info.setChannelSR(channelSR);
        info.initReaderSRs(readerSrts.size());
        info.initWriterSRs(writerSrts.size());
        startupInfo = info;
      }
    }

    for(auto i : kj::indices(readerSrts)){ 
      const auto& srt = readerSrts[i];
      auto reader = channelClient.readerRequest().send().wait(ioContext.waitScope).getR();
      auto readerSR = restorer->saveStr(reader, srt, nullptr, false, nullptr, false).wait(ioContext.waitScope).sturdyRef;  
      if(outputSturdyRefs && channelSR.size() > 0) std::cout << "readerSR=" << readerSR.cStr() << std::endl;
      KJ_IF_MAYBE(info, startupInfo){
        info->getReaderSRs().set(i, readerSR);
      }
    }
    for(auto i : kj::indices(writerSrts)){
      const auto& srt = writerSrts[i];
      auto writer = channelClient.writerRequest().send().wait(ioContext.waitScope).getW();
      auto writerSR = restorer->saveStr(writer, srt, nullptr, false, nullptr, false).wait(ioContext.waitScope).sturdyRef;
      if(outputSturdyRefs && writerSR.size() > 0) std::cout << "writerSR=" << writerSR.cStr() << std::endl;
      KJ_IF_MAYBE(info, startupInfo){
        info->getWriterSRs().set(i, writerSR);
      }
    }

    KJ_IF_MAYBE(req, infoReq){
      req->send().wait(ioContext.waitScope);
    }

    // Run forever, accepting connections and handling requests.
    kj::NEVER_DONE.wait(ioContext.waitScope);
    KJ_LOG(INFO, "stopped channel");
    return true;
  }

  kj::MainFunc getMain()
  {
    return addRestorableServiceOptions()
      .addOptionWithArg({'b', "buffer-size"}, KJ_BIND_METHOD(*this, setBufferSize),
                        "<buffer-size=1>", "Set buffer size of channel.")
      .addOptionWithArg({'c', "create"}, KJ_BIND_METHOD(*this, setNoOfReaderWriterPairs),
                        "<number_of_reader_writer_pairs (default: 1)>", "Create number of reader/writer pairs.")
      .addOptionWithArg({'R', "no_of_readers"}, KJ_BIND_METHOD(*this, setNoOfReaders),
                        "<number_of_readers (default: 1)>", "Create number of readers.")
      .addOptionWithArg({'W', "no_of_writers"}, KJ_BIND_METHOD(*this, setNoOfWriters),
                        "<number_of_writers (default: 1)>", "Create number of readers.")
      .addOptionWithArg({'r', "reader_srts"}, KJ_BIND_METHOD(*this, setReaderSrts),
                        "<Sturdy_ref_token_1,[Sturdy_ref_token_2],...>", "Create readers for given sturdy ref tokens.")
      .addOptionWithArg({'w', "writer_srts"}, KJ_BIND_METHOD(*this, setWriterSrts),
                        "<Sturdy_ref_token_1,[Sturdy_ref_token_2],...>", "Create writers for given sturdy ref tokens.")      
      .callAfterParsing(KJ_BIND_METHOD(*this, startChannel))
      .build();
  }

private:
  uint64_t bufferSize{1};
  kj::Vector<kj::String> readerSrts;
  kj::Vector<kj::String> writerSrts;
};

} // namespace common
} // namespace infrastructure
} // namespace mas

KJ_MAIN(mas::infrastructure::common::ChannelMain)
