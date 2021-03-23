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

#import common.common as cc
import common.geo as geo
import common_climate_data_capnp_impl as ccdi

abs_imports = ["capnproto_schemas"]
climate_data_capnp = capnp.load("capnproto_schemas/climate_data.capnp", imports=abs_imports)

#------------------------------------------------------------------------------

class AlterTimeSeriesWrapper(climate_data_capnp.Climate.AlterTimeSeriesWrapper.Server): 

    def __init__(self, timeSeries):

        self._timeSeries = timeSeries


    def wrappedTimeSeries_context(self, context): # wrappedTimeSeries @0 () -> (timeSeries :TimeSeries);
        pass


    def alterTemperaturesByDegrees_context(self, context): # alterTemperaturesByDegrees(minTemp :Int16 = 0, avgTemp :Int16 = 0, maxTemp :Int16 = 0, bool asNewTimeSeries = false) -> (timeSeries :TimeSeries);
        pass


    def alterPrecipitationByPercentage_context(self, context): # alterPrecipitationByPercentage(percentage :Float32, bool asNewTimeSeries = false) -> (timeSeries :TimeSeries);
        pass


    def alterCO2ContentByPpm_context(self, context): # alterCO2ContentByPpm(ppm :Int16, bool asNewTimeSeries = false) -> (timeSeries :TimeSeries);
        pass


    def alterGlobalRadiationByPercentage_context(self, context): # alterGlobalRadiationByPercentage(percentage :Float32, bool asNewTimeSeries = false) -> (timeSeries :TimeSeries);
        pass


    def alterRelativeHumidityByPercentage_context(self, context): # alterRelativeHumidityByPercentage(percentage :Float32, bool asNewTimeSeries = false) -> (timeSeries :TimeSeries);
        pass


    def alterWindspeedByPercentage_context(self, context): # alterWindspeedByPercentage(percentage :Float32, bool asNewTimeSeries = false) -> (timeSeries :TimeSeries);
        pass


    def resolution_context(self, context): # -> (resolution :TimeResolution);
        return self._timeSeries.resolution()


    def range_context(self, context): # -> (startDate :Date, endDate :Date);
        return self._timeSeries.range()
        

    def header(self, **kwargs): # () -> (header :List(Element));
        return self._timeSeries.header()


    def data(self, **kwargs): # () -> (data :List(List(Float32)));
        return self.dataframe.to_numpy().tolist()


    def dataT(self, **kwargs): # () -> (data :List(List(Float32)));
        return self.dataframe.T.to_numpy().tolist()


    def subrange_context(self, context): # (from :Date, to :Date) -> (timeSeries :TimeSeries);
        from_date = ccdi.create_date(getattr(context.params, "from"))
        to_date = ccdi.create_date(context.params.to)
        return self._timeSeries.subrange(from_date, to_date)


    def subheader_context(self, context): # (elements :List(Element)) -> (timeSeries :TimeSeries);
        return self._timeSeries.subheader(context.params.elements)


    def metadata_context(self, context): # metadata @7 () -> Metadata;
        return self._timeSeries.metadata()


    def location_context(self, context): # location @8 () -> Location;
        return self._timeSeries.location()


#------------------------------------------------------------------------------

class AlterTimeSeriesWrapperFactory(climate_data_capnp.Climate.AlterTimeSeriesWrapperFactory.Server): 

    def __init__(self):

        self._timeseries = []


    def wrap_context(self, context): # wrap @0 (timeSeries :TimeSeries) -> (wrapper :AlterTimeSeriesWrapper);
        ts = context.params.timeSeries
        if ts:
            atsw = AlterTimeSeriesWrapper(ts)
            self._timeseries.append(atsw)
            context.results.wrapper = atsw


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


if __name__ == '__main__':
    main()
