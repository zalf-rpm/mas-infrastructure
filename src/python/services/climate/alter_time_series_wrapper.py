#!/usr/bin/python
# -*- coding: UTF-8

# This Source Code Form is subject to the terms of the Mozilla Public
# License, v. 2.0. If a copy of the MPL was not distributed with this
# file, You can obtain one at http://mozilla.org/MPL/2.0/. */

# Authors:
# Michael Berg-Mohnicke <michael.berg-mohnicke@zalf.de>
#
# Maintainers:
# Currently maintained by the authors.
#
# This file has been created at the Institute of
# Landscape Systems Analysis at the ZALF.
# Copyright (C: Leibniz Centre for Agricultural Landscape Research (ZALF)

import asyncio
import capnp
import csv
import json
from datetime import date, timedelta
import gzip
import io
import numpy as np
import os
import pandas as pd
from pathlib import Path
import sys
import time

PATH_TO_REPO = Path(os.path.realpath(__file__)).parent.parent.parent.parent.parent
if str(PATH_TO_REPO) not in sys.path:
    sys.path.insert(1, str(PATH_TO_REPO))

PATH_TO_PYTHON_CODE = PATH_TO_REPO / "src/python"
if str(PATH_TO_PYTHON_CODE) not in sys.path:
    sys.path.insert(1, str(PATH_TO_PYTHON_CODE))

import common.capnp_async_helpers as async_helpers

abs_imports = ["capnproto_schemas"]
climate_data_capnp = capnp.load("capnproto_schemas/climate_data.capnp", imports=abs_imports)

#------------------------------------------------------------------------------

class AlterTimeSeriesWrapper(climate_data_capnp.Climate.AlterTimeSeriesWrapper.Server): 

    def __init__(self, time_series, header, altered = {}):

        self._timeSeries = time_series
        self._altered = altered #dict of elem to pair (Altered, func)
        self._cloned_time_series = []

        self._available_headers = header
        self._element_defaults = {
            "tmin": "absValue", "tavg": "absValue", "tmax": "absValue", 
            "precip": "percentage", "globrad": "percentage", "relhumid": "percentage", 
            "co2": "absValue", "wind": "percentage",
            "sunhours": "absValue", "cloudamount": "percentage", "airpress": "percentage",
            "vaporpress": "percentage", "o3": "absValue", "et0": "absValue", "dewpointTemp": "absValue"
        }


    def wrappedTimeSeries_context(self, context): # wrappedTimeSeries @0 () -> (timeSeries :TimeSeries);
        context.results = self._timeSeries


    def listAlteredElements_context(self, context): # listAlteredElements @7 () -> (list :List(Common.Pair(Element, Float32)));
        context.results = [altered for altered, _ in self._altered.values()]


    def alter(self, context): # alter @1 (element :Element, by :Float32, type :AlterType = elementDefault, asNewTimeSeries :Bool = false)  -> (timeSeries :TimeSeries);
        elem = context.params.element
        if elem in self._available_headers:
            altered = self._altered if context.params.asNewTimeSeries else {}

            by = context.params.by
            type_ = context.params.type

            if int(by * 100000) != 0:
                type__ = self._element_defaults[elem] if type__ == "elementDefault" else type_
                f = {
                    "absValue": lambda v: v + by,
                    "fraction": lambda v: v * by,
                }.get(type__, lambda v: v + by)
                
                altered[elem] = ({"element": elem, "value": by, "type": type_}, f)

                if context.params.asNewTimeSeries:
                    cts = AlterTimeSeriesWrapper(self, self._available_headers, altered)
                    self._cloned_time_series.append(cts)
                    context.results.timeSeries = cts
                else:
                    context.results.timeSeries = self


    def resolution_context(self, context): # -> (resolution :TimeResolution);
        return self._timeSeries.resolution().then(lambda res: setattr(context.results, "resolution", res.resolution))


    def range_context(self, context): # -> (startDate :Date, endDate :Date);
        return self._timeSeries.range().then(lambda res: [
            setattr(context.results, "startDate", res.startDate), 
            setattr(context.results, "endDate", res.endDate)
        ])
        

    def header_context(self, context): # () -> (header :List(Element));
        #return self._timeSeries.header().then(lambda h: setattr(context.results, "header", list(h.header)))
        return self._timeSeries.header().then(lambda h: setattr(context.results, "header", list(h.header)))


    def data_context(self, context): # () -> (data :List(List(Float32)));
        return self._timeSeries.data().then(lambda res: setattr(context.results, "data", [list(d) for d in res.data]))


    def dataT_context(self, context): # () -> (data :List(List(Float32)));
        return self._timeSeries.dataT().then(lambda res: setattr(context.results, "data", [list(d) for d in res.data]))


    def subrange_context(self, context): # (from :Date, to :Date) -> (timeSeries :TimeSeries);
        from_date = ccdi.create_date(getattr(context.params, "from"))
        to_date = ccdi.create_date(context.params.to)
        return self._timeSeries.subrange(from_date, to_date).then(lambda res: setattr(context.results, "timeSeries", res.timeSeries))


    def subheader_context(self, context): # (elements :List(Element)) -> (timeSeries :TimeSeries);
        return self._timeSeries.subheader(context.params.elements).then(lambda res: setattr(context.results, "timeSeries", res.timeSeries))


    def metadata_context(self, context): # metadata @7 () -> Metadata;
        return self._timeSeries.metadata().then(lambda res: [
            setattr(context.results, "entries", list(res.entries)),
            setattr(context.results, "info", res.info)
        ])


    def location_context(self, context): # location @8 () -> Location;
        return self._timeSeries.location().then(lambda res: [
            setattr(context.results, "id", res.id),
            setattr(context.results, "heightNN", res.heightNN),
            setattr(context.results, "geoCoord", res.geoCoord),
            setattr(context.results, "timeSeries", res.timeSeries)
        ])


#------------------------------------------------------------------------------

class AlterTimeSeriesWrapperFactory(climate_data_capnp.Climate.AlterTimeSeriesWrapperFactory.Server): 

    def __init__(self):
        self._timeseries = []


    def wrap_context(self, context): # wrap @0 (timeSeries :TimeSeries) -> (wrapper :AlterTimeSeriesWrapper);
        def create_wrapper(header):
            atsw = AlterTimeSeriesWrapper(ts, header)
            self._timeseries.append(atsw)
            context.results.wrapper = atsw

        ts = context.params.timeSeries
        if ts:
            return ts.header().then(lambda h: create_wrapper(list(h)))


#------------------------------------------------------------------------------

async def async_main(serve_bootstrap=False,
host="0.0.0.0", port=None, reg_sturdy_ref=None, id=None, name="AlterTimeSeriesWrapperFactory", description=None):

    config = {
        "host": host,
        "port": port,
        "id": id,
        "name": name,
        "description": description,
        "reg_sturdy_ref": reg_sturdy_ref,
        "serve_bootstrap": str(serve_bootstrap),
        "reg_category": "climate",
    }
    # read commandline args only if script is invoked directly from commandline
    if len(sys.argv) > 1 and __name__ == "__main__":
        for arg in sys.argv[1:]:
            k, v = arg.split("=")
            if k in config:
                config[k] = v
    print("config used:", config)

    conMan = async_helpers.ConnectionManager()

    service = AlterTimeSeriesWrapperFactory()

    if config["reg_sturdy_ref"]:
        registrator = await conMan.try_connect(config["reg_sturdy_ref"], cast_as=reg_capnp.Registrator)
        if registrator:
            unreg = await registrator.register(ref=service, categoryId=config["reg_category"]).a_wait()
            print("Registered ", config["name"], "climate service.")
            #await unreg.unregister.unregister().a_wait()
        else:
            print("Couldn't connect to registrator at sturdy_ref:", config["reg_sturdy_ref"])

    if config["serve_bootstrap"].upper() == "TRUE":
        await async_helpers.serve_forever(config["host"], config["port"], service)
    else:
        await conMan.manage_forever()

#------------------------------------------------------------------------------

def main(server="*", port=11006):

    config = {
        "port": str(port),
        "server": server
    }
    # read commandline args only if script is invoked directly from commandline
    if len(sys.argv) > 1 and __name__ == "__main__":
        for arg in sys.argv[1:]:
            k, v = arg.split("=")
            if k in config:
                config[k] = v
    print(config)

    server = capnp.TwoPartyServer(config["server"] + ":" + config["port"],
                                  bootstrap=AlterTimeSeriesWrapperFactory())
    server.run_forever()

#------------------------------------------------------------------------------

if __name__ == '__main__':
    asyncio.run(async_main(port=11006, serve_bootstrap=True))
    #main()
