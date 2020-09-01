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

from datetime import date, timedelta
import json
import numpy
import os
import pandas as pd
from pathlib import Path
import sys
import time

PATH_TO_REPO = Path(os.path.realpath(__file__)).parent.parent.parent.parent.parent

import capnp
from capnproto_schemas import climate_data_capnp

#------------------------------------------------------------------------------

def create_date(capnp_date):
    return date(capnp_date.year, capnp_date.month, capnp_date.day)


def create_capnp_date(py_date):
    return {
        "year": py_date.year if py_date else 0,
        "month": py_date.month if py_date else 0,
        "day": py_date.day if py_date else 0
    }


class CSV_TimeSeries(climate_data_capnp.Climate.TimeSeries.Server):

    def __init__(self, dataframe=None, path_to_csv_file=None, headers=None, start_date=None, end_date=None):
        if path_to_csv_file:
            dataframe = pd.read_csv(path_to_csv_file, skiprows=[1], index_col=0, delimiter=";")
        elif dataframe is None:
            return
        self._df = dataframe.rename(columns={"windspeed": "wind"})
        self._data = None
        self._headers = headers
        self._start_date = start_date if start_date else (date.fromisoformat(dataframe.index[0]) if len(dataframe) > 0 else None)
        self._end_date = end_date if end_date else (date.fromisoformat(dataframe.index[-1]) if len(dataframe) > 0 else None)

    def resolution_context(self, context): # () -> (resolution :TimeResolution);
        context.results.resolution = climate_data_capnp.Climate.TimeSeries.Resolution.daily

    def range_context(self, context): # () -> (startDate :Date, endDate :Date);
        context.results.startDate = create_capnp_date(self._start_date)
        context.results.endDate = create_capnp_date(self._end_date)

    def header(self, **kwargs): # () -> (header :List(Element));
        return self._df.columns.tolist()

    def data(self, **kwargs): # () -> (data :List(List(Float32)));
        print("data requested")
        return self._df.to_numpy().tolist()

    def dataT(self, **kwargs): # () -> (data :List(List(Float32)));
        print("dataT requested")
        return self._df.T.to_numpy().tolist()

    def subrange_context(self, context): # (from :Date, to :Date) -> (timeSeries :TimeSeries);
        from_date = create_date(getattr(context.params, "from"))
        to_date = create_date(context.params.to)

        sub_df = self._df.loc[str(from_date):str(to_date)]

        context.results.timeSeries = CSV_TimeSeries(dataframe=sub_df, headers=self._headers,
                                                    start_date=from_date, end_date=to_date)
    
    def subheader_context(self, context): # (elements :List(Element)) -> (timeSeries :TimeSeries);
        sub_headers = [str(e) for e in context.params.elements]
        sub_df = self._df.loc[:, sub_headers]

        context.results.timeSeries = CSV_TimeSeries(dataframe=sub_df, headers=sub_headers,
                                                    start_date=self._start_date, end_date=self._end_date)

    def metadata(self, _context, **kwargs): # () -> Metadata;
        pass

    def location(self, _context, **kwargs): # () -> Location;
        _context.results.timeSeries = self


def main(server="*", port=11002, path_to_csv_file="data/climate/climate-iso.csv"):

    config = {
        "port": str(port),
        "server": server,
        "path_to_csv_file": path_to_csv_file
    }
    # read commandline args only if script is invoked directly from commandline
    if len(sys.argv) > 1 and __name__ == "__main__":
        for arg in sys.argv[1:]:
            k, v = arg.split("=")
            if k in config:
                config[k] = v

    server = capnp.TwoPartyServer(config["server"] + ":" + config["port"],
                                  bootstrap=CSV_TimeSeries(path_to_csv_file=config["path_to_csv_file"]))
    server.run_forever()


if __name__ == '__main__':
    main()
