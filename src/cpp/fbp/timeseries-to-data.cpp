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
#include <fstream>
#include <string>
#include <tuple>
#include <vector>
#include <algorithm>

#include <kj/debug.h>
#include <kj/common.h>
#include <kj/exception.h>
#define KJ_MVCAP(var) var = kj::mv(var)
#include <kj/main.h>

#include <capnp/any.h>

#include "tools/date.h"
#include "tools/helper.h"
#include "common/rpc-connection-manager.h"
#include "common/common.h"

#include "climate.capnp.h"
#include "common.capnp.h"

using namespace std;
using namespace Tools;

namespace mas {
namespace infrastructure {
namespace fbp {

class TimeSeriesToTimeSeriesDataMain {
public:
  TimeSeriesToTimeSeriesDataMain(kj::ProcessContext &context) : context(context) {}

  kj::MainBuilder::Validity setName(kj::StringPtr n) { name = str(n); return true; }

  kj::MainBuilder::Validity setFromAttr(kj::StringPtr name) { fromAttr = kj::str(name); return true; }

  kj::MainBuilder::Validity setToAttr(kj::StringPtr name) { toAttr = kj::str(name); return true; }

  kj::MainBuilder::Validity setInSr(kj::StringPtr name) { inSr = kj::str(name); return true; }

  kj::MainBuilder::Validity setInType(kj::StringPtr type) { inType = kj::str(type); return true; }

  kj::MainBuilder::Validity setSubrangeStart(kj::StringPtr isoDateStr) { 
    subrangeStart = Tools::Date::fromIsoDateString(isoDateStr.cStr());
    return true; 
  }
  
  kj::MainBuilder::Validity setSubrangeEnd(kj::StringPtr isoDateStr) { 
    subrangeEnd = Tools::Date::fromIsoDateString(isoDateStr.cStr()); 
    return true; 
  }
  
  kj::MainBuilder::Validity setSubheader(kj::StringPtr headers) { 
    for(auto& s : mas::infrastructure::common::splitString(headers, ",")) subheaders.add(kj::str(s)); 
    return true; 
  }
  
  kj::MainBuilder::Validity setTransposed(kj::StringPtr isTransposed) { 
    transposed = Tools::toLower(isTransposed.cStr()) == "true"; 
    return true; 
  }

  kj::MainBuilder::Validity setOutSr(kj::StringPtr name) { outSr = kj::str(name); return true; }

  kj::MainBuilder::Validity startComponent() {
    auto ioContext = kj::setupAsyncIo();

    KJ_LOG(INFO, "starting timeseries-to-data Cap'n Proto FBP component");

    typedef schema::common::IP IP;
    typedef schema::common::Channel<IP> Channel;

    auto inp = conMan.tryConnectB(ioContext, inSr.cStr()).castAs<Channel::ChanReader>();
    auto outp = conMan.tryConnectB(ioContext, outSr.cStr()).castAs<Channel::ChanWriter>();

    using TS = mas::schema::climate::TimeSeries;
    using TSD = mas::schema::climate::TimeSeriesData;

    try {
      while(true) {
        auto msg = inp.readRequest().send().wait(ioContext.waitScope);
        // check for end of data from in port
        if (msg.isDone()) break;
        else {
          auto inIp = msg.getValue();
          auto attr = common::getIPAttr(inIp, fromAttr);
          TS::Client timeseries(nullptr);
          if (inType == "capability") {
            timeseries = attr.orDefault(inIp.getContent()).getAs<TS>();
          } else if(inType == "sturdyref") {
            auto timeseriesSR = attr.orDefault(inIp.getContent()).getAs<capnp::Text>();
            timeseries = conMan.tryConnectB(ioContext, timeseriesSR).castAs<TS>();
          }

          auto wreq = outp.writeRequest();
          IP::Builder outIp = wreq.initValue();

          TSD::Builder tsdb(nullptr);
          // set content if not to be set as attribute
          if (kj::size(toAttr) == 0) tsdb = outIp.initContent().getAs<TSD>();
          // copy attributes, if any and set result as attribute, if requested
          KJ_IF_MAYBE(builder, common::copyAndSetIPAttrs(inIp, outIp, toAttr)) { 
            tsdb = builder->getAs<TSD>();
          }

          tsdb.setIsTransposed(transposed);
          if(subheaders.size() > 0) {
            auto shReq = timeseries.subheaderRequest();
            auto shb = shReq.initElements(subheaders.size());
            using E = mas::schema::climate::Element;
            for(size_t i = 0; i < subheaders.size(); ++i) {
              if(subheaders[i] == "tmin") shb.set(i, E::TMIN); 
              else if(subheaders[i] == "tavg") shb.set(i, E::TAVG); 
              else if(subheaders[i] == "tmax") shb.set(i, E::TMAX); 
              else if(subheaders[i] == "precip") shb.set(i, E::PRECIP); 
              else if(subheaders[i] == "relhumid") shb.set(i, E::RELHUMID); 
              else if(subheaders[i] == "wind") shb.set(i, E::WIND); 
              else if(subheaders[i] == "globrad") shb.set(i, E::GLOBRAD); 
            }
            timeseries = shReq.send().getTimeSeries();   
          }

          if(subrangeStart.isValid() || subrangeEnd.isValid()) {
            auto srReq = timeseries.subrangeRequest();
            if(subrangeStart.isValid()){
              auto start = srReq.initStart();
              start.setYear(subrangeStart.year());
              start.setMonth(subrangeStart.month());
              start.setDay(subrangeStart.day());
            }
            if(subrangeEnd.isValid()){
              auto end = srReq.initEnd();
              end.setYear(subrangeEnd.year());
              end.setMonth(subrangeEnd.month());
              end.setDay(subrangeEnd.day());
            }
            timeseries = srReq.send().getTimeSeries();   
          }

          auto resolutionPromise = timeseries.resolutionRequest().send();
          auto headerPromise = timeseries.headerRequest().send();
          auto rangePromise = timeseries.rangeRequest().send();
          if(transposed) tsdb.setData(timeseries.dataTRequest().send().wait(ioContext.waitScope).getData());
          else tsdb.setData(timeseries.dataRequest().send().wait(ioContext.waitScope).getData());

          tsdb.setResolution(resolutionPromise.wait(ioContext.waitScope).getResolution());
          tsdb.setHeader(headerPromise.wait(ioContext.waitScope).getHeader());
          auto range = rangePromise.wait(ioContext.waitScope);
          tsdb.setStartDate(range.getStartDate());
          tsdb.setEndDate(range.getEndDate());

          wreq.send().wait(ioContext.waitScope);
        }
      }

      auto wreq = outp.writeRequest();
      wreq.setDone();
      wreq.send().wait(ioContext.waitScope);
    } catch(kj::Exception e) {
      std::cerr << "Exception: " << e.getDescription().cStr() << endl;
    }

    return true;
  }

  kj::MainFunc getMain() {
    return kj::MainBuilder(context, "timeseries-to-data FBP Component v0.1", "Transform a timeseries cap into a pure data.")
      .addOptionWithArg({'n', "name"}, KJ_BIND_METHOD(*this, setName),
                        "<component-name>", "Give this component a name.")
      .addOptionWithArg({'f', "from_attr"}, KJ_BIND_METHOD(*this, setFromAttr),
                        "<attr>", "Which attribute to read the MONICA env from.")
      .addOptionWithArg({'t', "to_attr"}, KJ_BIND_METHOD(*this, setToAttr),
                        "<attr>", "Which attribute to write the MONICA result to.")
      .addOptionWithArg({'i', "in_sr"}, KJ_BIND_METHOD(*this, setInSr),
                        "<sturdy_ref>", "Sturdy ref to input channel.")
      .addOptionWithArg({"in_type"}, KJ_BIND_METHOD(*this, setInType),
                         "<sturdyref | capability (default: sturdyref)>", "Receive on in port a TimeSeries cap or a sturdy ref to one.")      
      .addOptionWithArg({'o', "out_sr"}, KJ_BIND_METHOD(*this, setOutSr),
                        "<sturdy_ref>", "Sturdy ref to output channel.")
      .addOptionWithArg({"subrange_start"}, KJ_BIND_METHOD(*this, setSubrangeStart),
                        "<iso-date-string>", "Start of subrange.")
      .addOptionWithArg({"subrange_end"}, KJ_BIND_METHOD(*this, setSubrangeEnd),
                        "<iso-date-string>", "End of subrange.")
      .addOptionWithArg({"subheader"}, KJ_BIND_METHOD(*this, setSubheader),
                        "<[precip,globrad,tavg,....] (default: all available)>", "List of climate elements.")
      .addOptionWithArg({"transposed"}, KJ_BIND_METHOD(*this, setTransposed),
                        "<true | false (default: false>", "Return data transposed.")
      .callAfterParsing(KJ_BIND_METHOD(*this, startComponent))
      .build();
  }

private:
  mas::infrastructure::common::ConnectionManager conMan;
  kj::String name;
  kj::ProcessContext &context;
  kj::String inSr;
  kj::String inType{kj::str("sturdyref")};
  kj::String outSr;
  kj::String fromAttr;
  kj::String toAttr;
  Tools::Date subrangeStart;
  Tools::Date subrangeEnd;
  kj::Vector<kj::String> subheaders;
  bool transposed{false};
};

} // namespace fbp
} // namespace infrastructure
} // namespace mas

KJ_MAIN(mas::infrastructure::fbp::TimeSeriesToTimeSeriesDataMain)

