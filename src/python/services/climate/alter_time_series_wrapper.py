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
import os
from pathlib import Path
import sys
import uuid

PATH_TO_REPO = Path(os.path.realpath(__file__)).parent.parent.parent.parent.parent
if str(PATH_TO_REPO) not in sys.path:
    sys.path.insert(1, str(PATH_TO_REPO))

PATH_TO_PYTHON_CODE = PATH_TO_REPO / "src/python"
if str(PATH_TO_PYTHON_CODE) not in sys.path:
    sys.path.insert(1, str(PATH_TO_PYTHON_CODE))

from pkgs.common import capnp_async_helpers as async_helpers
from pkgs.climate import common_climate_data_capnp_impl as ccdi
from pkgs.common import common
from pkgs.common import service as serv

PATH_TO_CAPNP_SCHEMAS = PATH_TO_REPO / "capnproto_schemas"
abs_imports = [str(PATH_TO_CAPNP_SCHEMAS)]
climate_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "climate.capnp"), imports=abs_imports)
reg_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "registry.capnp"), imports=abs_imports)


class AlterTimeSeriesWrapper(climate_capnp.AlterTimeSeriesWrapper.Server):

    def __init__(self, timeseries, header, altered = {}):

        self._timeseries = timeseries
        self._altered = altered #dict of elem to pair (Altered, header_index, func)

        self._available_headers = header


    def replaceWrappedTimeSeries_context(self, context): # replaceWrappedTimeSeries @4 (timeSeries :TimeSeries); 
        self._timeseries = context.params.timeSeries


    def wrappedTimeSeries_context(self, context): # wrappedTimeSeries @0 () -> (timeSeries :TimeSeries);
        context.results = self._timeseries


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
                context.results.timeSeries = cts
            else:
                context.results.timeSeries = self


    def remove_context(self, context): # remove @2 (alteredElement :Element);
        ae = context.params.alteredElement
        if ae:
            self._altered.pop(ae, None)


    def resolution_context(self, context): # -> (resolution :TimeResolution);
        return self._timeseries.resolution().then(lambda res: setattr(context.results, "resolution", res.resolution))


    def range_context(self, context): # -> (startDate :Date, endDate :Date);
        return self._timeseries.range().then(lambda res: [
            setattr(context.results, "startDate", res.startDate), 
            setattr(context.results, "endDate", res.endDate)
        ])
        

    def header_context(self, context): # () -> (header :List(Element));
        return self._timeseries.header().then(lambda h: setattr(context.results, "header", list(h.header)))


    def data_context(self, context): # () -> (data :List(List(Float32)));
        index_to_func = {index : func for _, (_, index, func) in self._altered.items()}
        def alter(values):
            vs = list(values)
            for i, f in index_to_func.items():
                vs[i] = f(vs[i])
            return vs
        return self._timeseries.data().then(lambda res: setattr(context.results, "data", [alter(d) for d in res.data]))


    def dataT_context(self, context): # () -> (data :List(List(Float32)));
        i_to_f = {i : f for _, (_, i, f) in self._altered.items()}
        def alter(index, vals):
            if index in i_to_f:
                func = i_to_f[index]
                return [func(val) for val in vals]
            else:
                return list(vals)
        return self._timeseries.dataT().then(lambda res: setattr(context.results, "data", [alter(i, d) for i, d in enumerate(res.data)]))#alter(res.data)))


    def subrange_context(self, context): # (from :Date, to :Date) -> (timeSeries :TimeSeries);
        from_date = ccdi.create_date(getattr(context.params, "from"))
        to_date = ccdi.create_date(context.params.to)
        return self._timeseries.subrange(from_date, to_date).then(lambda res: setattr(context.results, "timeSeries", res.timeSeries))


    def subheader_context(self, context): # (elements :List(Element)) -> (timeSeries :TimeSeries);
        return self._timeseries.subheader(context.params.elements).then(lambda res: setattr(context.results, "timeSeries", res.timeSeries))


    def metadata_context(self, context): # metadata @7 () -> Metadata;
        return self._timeseries.metadata().then(lambda res: [
            setattr(context.results, "entries", list(res.entries)),
            setattr(context.results, "info", res.info)
        ])


    def location_context(self, context): # location @8 () -> Location;
        return self._timeseries.location().then(lambda res: [
            setattr(context.results, "id", res.id),
            setattr(context.results, "heightNN", res.heightNN),
            setattr(context.results, "geoCoord", res.geoCoord),
            setattr(context.results, "timeSeries", res.timeSeries)
        ])


class AlterTimeSeriesWrapperFactory(climate_capnp.AlterTimeSeriesWrapperFactory.Server, common.Identifiable, common.Persistable):

    def __init__(self, id=None, name=None, description=None, restorer=None):
        common.Persistable.__init__(self, restorer)
        common.Identifiable.__init__(self, id, name, description)
        self._id = id if id else str(uuid.uuid4())
        self._name = name if name else id
        self._description = description if description else ""
        self._wrapped_timeseries = None


    def info_context(self, context): # -> Common.IdInformation;
        r = context.results
        r.id = self._id
        r.name = self._name
        r.description = self._description


    def wrap_context(self, context): # wrap @0 (timeSeries :TimeSeries) -> (wrapper :AlterTimeSeriesWrapper);
        def create_wrapper(header):
            atsw = AlterTimeSeriesWrapper(ts, header)
            self._wrapped_timeseries = ts
            context.results.wrapper = atsw

        ts = context.params.timeSeries
        if ts:
            return ts.header().then(lambda h: create_wrapper(list(h.header)))


async def main(serve_bootstrap=True, host=None, port=None, 
    id=None, name="AlterTimeSeriesWrapperFactory", description=None, reg_sturdy_ref=None, use_async=False):

    config = {
        "port": port, 
        "host": host,
        "id": id,
        "name": name,
        "description": description,
        "serve_bootstrap": serve_bootstrap,
        "reg_sturdy_ref": reg_sturdy_ref,
        "reg_category": "climate",
        "use_async": use_async,
    }
    common.update_config(config, sys.argv, print_config=True, allow_new_keys=False)

    conman = async_helpers.ConnectionManager()
    restorer = common.Restorer()
    service = AlterTimeSeriesWrapperFactory(id=config["id"], name=config["name"], description=config["description"], restorer=restorer)

    if config["reg_sturdy_ref"]:
        registrator = await conman.try_connect(config["reg_sturdy_ref"], cast_as=reg_capnp.Registrator)
        if registrator:
            unreg = await registrator.register(ref=service, categoryId=config["reg_category"]).a_wait()
            print("Registered ", config["name"], "climate service.")
            #await unreg.unregister.unregister().a_wait()
        else:
            print("Couldn't connect to registrator at sturdy_ref:", config["reg_sturdy_ref"])
    
    if config["use_async"]:
        await serv.async_init_and_run_service({"service": service}, config["host"], config["port"], 
        serve_bootstrap=config["serve_bootstrap"], restorer=restorer, conn_man=conman)
    else:
        
        serv.init_and_run_service({"service": service}, config["host"], config["port"], 
            serve_bootstrap=config["serve_bootstrap"], restorer=restorer, conn_man=conman)


if __name__ == '__main__':
    #asyncio.run(main(serve_bootstrap=True, use_async=True))
    asyncio.run(main(serve_bootstrap=True, use_async=True))