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
        self._altered = altered #dict of elem to pair (Altered, header_index, func)
        self._cloned_time_series = []

        self._available_headers = header


    def wrappedTimeSeries_context(self, context): # wrappedTimeSeries @0 () -> (timeSeries :TimeSeries);
        context.results = self._timeSeries


    def alteredElements_context(self, context): # alteredElements @7 () -> (list :List(Common.Pair(Element, Float32)));
        context.results.list = [altered for altered, _, _ in self._altered.values()]


    def alter_context(self, context): # alter @1 (desc :Altered, asNewTimeSeries :Bool = false)  -> (timeSeries :TimeSeries);
        desc = context.params.desc
        elem = desc.element
        if elem in self._available_headers:
            altered = {} if context.params.asNewTimeSeries else self._altered

            val = desc.value
            f = {
                "add": lambda v: v + val,
                "mul": lambda v: v * val,
            }.get(desc.type, None)
            
            if f is None:
                return
            
            altered[elem] = ({"element": elem, "value": val, "type": desc.type}, self._available_headers.index(elem), f)

            if context.params.asNewTimeSeries:
                cts = AlterTimeSeriesWrapper(self, self._available_headers, altered)
                self._cloned_time_series.append(cts)
                context.results.timeSeries = cts
            else:
                context.results.timeSeries = self


    def remove_context(self, context): # remove @2 (alteredElement :Element);
        ae = context.params.alteredElement
        if ae:
            self._altered.pop(ae, None)


    def resolution_context(self, context): # -> (resolution :TimeResolution);
        return self._timeSeries.resolution().then(lambda res: setattr(context.results, "resolution", res.resolution))


    def range_context(self, context): # -> (startDate :Date, endDate :Date);
        return self._timeSeries.range().then(lambda res: [
            setattr(context.results, "startDate", res.startDate), 
            setattr(context.results, "endDate", res.endDate)
        ])
        

    def header_context(self, context): # () -> (header :List(Element));
        return self._timeSeries.header().then(lambda h: setattr(context.results, "header", list(h.header)))


    def data_context(self, context): # () -> (data :List(List(Float32)));
        index_to_func = {index : func for _, (_, index, func) in self._altered.items()}
        def alter(values):
            vs = list(values)
            for i, f in index_to_func.items():
                vs[i] = f(vs[i])
            return vs
        return self._timeSeries.data().then(lambda res: setattr(context.results, "data", [alter(d) for d in res.data]))


    def dataT_context(self, context): # () -> (data :List(List(Float32)));
        i_to_f = {i : f for _, (_, i, f) in self._altered.items()}
        def alter(index, vals):
            if index in i_to_f:
                func = i_to_f[index]
                return [func(val) for val in vals]
            else:
                return list(vals)
        return self._timeSeries.dataT().then(lambda res: setattr(context.results, "data", [alter(i, d) for i, d in enumerate(res.data)]))#alter(res.data)))


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

    def __init__(self, id=None, name=None, description=None):
        self._id = id if id else str(uuid.uuid4())
        self._name = name if name else id
        self._description = description if description else ""
        self._timeseries = []


    def info_context(self, context): # -> Common.IdInformation;
        r = context.results
        r.id = self._id
        r.name = self._name
        r.description = self._description


    def wrap_context(self, context): # wrap @0 (timeSeries :TimeSeries) -> (wrapper :AlterTimeSeriesWrapper);
        def create_wrapper(header):
            atsw = AlterTimeSeriesWrapper(ts, header)
            self._timeseries.append(atsw)
            context.results.wrapper = atsw

        ts = context.params.timeSeries
        if ts:
            return ts.header().then(lambda h: create_wrapper(list(h.header)))


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

    service = AlterTimeSeriesWrapperFactory(id=config["id"], name=config["name"], description=config["description"])

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
