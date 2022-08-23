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

from collections import deque
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
import psutil
import sys
import time
import uuid

PATH_TO_REPO = Path(os.path.realpath(__file__)).parent.parent.parent.parent.parent
if str(PATH_TO_REPO) not in sys.path:
    sys.path.insert(1, str(PATH_TO_REPO))

PATH_TO_PYTHON_CODE = PATH_TO_REPO / "src/python"
if str(PATH_TO_PYTHON_CODE) not in sys.path:
    sys.path.insert(1, str(PATH_TO_PYTHON_CODE))

#import common.common as cc
import common.geo as geo
import common.common as common
import services.climate.common_climate_data_capnp_impl as ccdi

PATH_TO_CAPNP_SCHEMAS = PATH_TO_REPO / "capnproto_schemas"
abs_imports = [str(PATH_TO_CAPNP_SCHEMAS)]
climate_data_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "climate.capnp"), imports=abs_imports)
persistence_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "persistence.capnp"), imports=abs_imports)

#------------------------------------------------------------------------------

class TimeSeries(climate_data_capnp.TimeSeries.Server, common.Identifiable, common.Persistable): 

    def __init__(self, metadata=None, location=None, path_to_csv=None, dataframe=None, csv_string=None, 
    header_map=None, supported_headers=None, pandas_csv_config={}, transform_map=None,
        id=None, name=None, description=None, restorer=None):
        common.Persistable.__init__(self, restorer)
        common.Identifiable.__init__(self, id, name, description)

        if path_to_csv is None and dataframe is None and csv_string is None:
            raise Exception("Missing argument, either path_to_csv or dataframe have to be supplied!")

        #print(csv_string)

        self._path_to_csv = path_to_csv
        self._csv_string = csv_string
        self._df = dataframe
        self._meta = metadata
        self._location = location

        self._header_map = header_map
        self._supported_headers = list(climate_data_capnp.Element.schema.enumerants.keys()) if supported_headers is None else supported_headers
        self._pandas_csv_config_defaults = {"skip_rows": [1], "index_col": 0, "sep": ","}
        self._pandas_csv_config = {**self._pandas_csv_config_defaults, **pandas_csv_config}
        self._transform_map = transform_map

        self._persistence_service = None


    @classmethod
    def from_csv_file(cls, path_to_csv, metadata=None, location=None, header_map=None, supported_headers=None, pandas_csv_config=None,
        transform_map=None):
        return TimeSeries(metadata=metadata, location=location, path_to_csv=path_to_csv, header_map=header_map, supported_headers=supported_headers,
            pandas_csv_config=pandas_csv_config, transform_map=transform_map)


    @classmethod
    def from_csv_string(cls, csv_string, metadata=None, location=None, header_map=None, supported_headers=None, pandas_csv_config=None,
        transform_map=None):
        return TimeSeries(metadata=metadata, location=location, csv_string=csv_string, header_map=header_map, supported_headers=supported_headers,
            pandas_csv_config=pandas_csv_config, transform_map=transform_map)


    @classmethod
    def from_dataframe(cls, dataframe, metadata=None, location=None):
        return TimeSeries(metadata=metadata, location=location, dataframe=dataframe)


    @property
    def dataframe(self):
        "init underlying dataframe lazily if initialized with path to csv file"
        if self._df is None and (self._path_to_csv or self._csv_string):
            # load csv file
            if self._path_to_csv and self._path_to_csv[-2:] == "gz":
                with gzip.open(self._path_to_csv) as _:
                    self._df = pd.read_csv(_, 
                        skiprows=self._pandas_csv_config["skip_rows"], 
                        index_col=self._pandas_csv_config["index_col"],
                        sep=self._pandas_csv_config["sep"])
            elif self._path_to_csv:
                self._df = pd.read_csv(self._path_to_csv, 
                    skiprows=self._pandas_csv_config["skip_rows"], 
                    index_col=self._pandas_csv_config["index_col"],
                    sep=self._pandas_csv_config["sep"])
            else:
                self._df = pd.read_csv(io.StringIO(self._csv_string), 
                    skiprows=self._pandas_csv_config["skip_rows"], 
                    index_col=self._pandas_csv_config["index_col"],
                    sep=self._pandas_csv_config["sep"])

            if self._header_map:
                self._df.rename(columns=self._header_map, inplace=True)

            # reduce headers to the supported ones
            if self._supported_headers:
                self._df = self._df.loc[:, self._df.columns.intersection(self._supported_headers)]

            if self._transform_map:
                for col_name, trans_func in self._transform_map.items():
                    self._df[col_name] = self._df[col_name].map(trans_func)

        return self._df


    def resolution_context(self, context): # -> (resolution :TimeResolution);
        context.results.resolution = climate_data_capnp.TimeSeries.Resolution.daily


    def range_context(self, context): # -> (startDate :Date, endDate :Date);
        context.results.startDate = ccdi.create_capnp_date(date.fromisoformat(str(self.dataframe.index[0])[:10]))
        context.results.endDate = ccdi.create_capnp_date(date.fromisoformat(str(self.dataframe.index[-1])[:10]))
        

    def header(self, **kwargs): # () -> (header :List(Element));
        return self.dataframe.columns.tolist()


    def data(self, **kwargs): # () -> (data :List(List(Float32)));
        return self.dataframe.to_numpy().tolist()


    def dataT(self, **kwargs): # () -> (data :List(List(Float32)));
        return self.dataframe.T.to_numpy().tolist()


    def subrange_context(self, context): # (from :Date, to :Date) -> (timeSeries :TimeSeries);
        from_date = ccdi.create_date(getattr(context.params, "from"))
        to_date = ccdi.create_date(context.params.to)

        sub_df = self.dataframe.loc[str(from_date):str(to_date)]

        context.results.timeSeries = TimeSeries.from_dataframe(sub_df, metadata=self._meta, location=self._location)


    def subheader(self, elements, **kwargs): # (elements :List(Element)) -> (timeSeries :TimeSeries);
        sub_headers = [str(e) for e in elements]
        sub_df = self.dataframe.loc[:, sub_headers]

        return TimeSeries.from_dataframe(sub_df, metadata=self._meta, location=self._location)


    def metadata(self, _context, **kwargs): # metadata @7 () -> Metadata;
        "the metadata for this time series"
        if self._meta:
            r = _context.results
            r.init("entries", len(self._meta.entries))
            for i, e in enumerate(self._meta.entries):
                r.entries[i] = e
            r.info = self._meta.info


    def location(self, _context, **kwargs): # location @8 () -> Location;
        "location of this time series"
        r = _context.results
        r.timeSeries = self
        if self._location:
            r.id = self._location.id
            r.heightNN = self._location.heightNN
            r.latlon = self._location.latlon


    def __del__(self):
        pass
        #print("deleting timeseries")

#------------------------------------------------------------------------------

class Dataset(climate_data_capnp.Dataset.Server, common.Identifiable, common.Persistable):

    def __init__(self, metadata, path_to_rows, interpolator, rowcol_to_latlon, 
        gzipped=False, header_map=None, supported_headers=None, row_col_pattern="row-{row}/col-{col}.csv",
        pandas_csv_config={}, transform_map=None, id=None, name=None, description=None, restorer=None,
        percentage_of_main_memory_use=20):
        common.Persistable.__init__(self, restorer)
        common.Identifiable.__init__(self, id, name, description)

        self._meta = metadata
        self._path_to_rows = path_to_rows
        self._interpolator = interpolator
        self._time_series = {}
        self._creation_order = deque()
        self._locations = {}
        self._all_locations_created = False
        self._header_map = header_map
        self._supported_headers = supported_headers
        self._rowcol_to_latlon = rowcol_to_latlon
        self._row_col_pattern = row_col_pattern
        self._pandas_csv_config = pandas_csv_config
        self._transform_map = transform_map
        self._process = psutil.Process()
        self._percentage_of_main_memory_use = percentage_of_main_memory_use


    def metadata(self, _context, **kwargs): # metadata @0 () -> Metadata;
        # get metadata for these data 
        r = _context.results
        r.init("entries", len(self._meta.entries))
        for i, e in enumerate(self._meta.entries):
            r.entries[i] = e
        r.info = self._meta.info
        

    def time_series_at(self, row, col, location=None):
        if (row, col) not in self._time_series:
            path_to_csv = self._path_to_rows + "/" + self._row_col_pattern.format(row=row, col=col)
            if not location:
                location = self.location_at(row, col)
            time_series = TimeSeries.from_csv_file(path_to_csv, 
                metadata=self._meta, 
                location=location, 
                supported_headers=self._supported_headers,
                header_map=self._header_map,
                pandas_csv_config=self._pandas_csv_config,
                transform_map=self._transform_map) 
            self._time_series[(row, col)] = time_series
            self._creation_order.append((row, col))
            time_series.name = "row: {}/col: {}".format(row, col)
            time_series.restorer = self._restorer

            #print(self._process.memory_percent(memtype="rss"))
            while len(self._creation_order) > 1 and self._process.memory_percent(memtype="rss") > self._percentage_of_main_memory_use:
                rc = self._creation_order.popleft()
                if rc != (row, col):
                    self._time_series.pop(rc)
                    #print("after pop:", self._process.memory_percent(memtype="rss"))

        return self._time_series[(row, col)]


    def closestTimeSeriesAt(self, latlon, **kwargs): # (latlon :Geo.LatLonCoord) -> (timeSeries :TimeSeries);
        # closest TimeSeries object which represents the whole time series 
        # of the climate realization at the give climate coordinate
        row, col = self._interpolator(latlon.lat, latlon.lon)
        return self.time_series_at(row, col)


    def timeSeriesAt(self, locationId, **kwargs): # (locationId :Text) -> (timeSeries :TimeSeries);
        rs, cs = locationId.split("/")
        row = int(rs[2:])
        col = int(cs[2:])
        return self.time_series_at(row, col)


    def location_at(self, row, col, coord=None, time_series=None):
        if (row, col) not in self._locations:
            if not coord:
                coord = self._rowcol_to_latlon[(row, col)]
            id = "r:{}/c:{}".format(row, col)
            name = "Row/Col:{}/{}|LatLon:{}/{}".format(row, col, coord["lat"], coord["lon"])
            loc = climate_data_capnp.Location.new_message(
                id={"id": id, "name": name, "description": ""},
                heightNN=coord["alt"],
                latlon={"lat": coord["lat"], "lon": coord["lon"]},
            )
            if time_series:
                loc.timeSeries = time_series
            self._locations[(row, col)] = loc
        return self._locations[(row, col)]


    def locations(self, **kwargs): # locations @2 () -> (locations :List(Location));
        # all the climate locations this dataset has
        locs = []
        if not self._all_locations_created:
            for row_col, coord in self._rowcol_to_latlon.items():
                row, col = row_col
                loc = self.location_at(row, col, coord)
                ts = self.time_series_at(row, col, loc)
                loc.timeSeries = ts
                locs.append(loc)
            self._all_locations_created = True
        else:
            locs.extend(self._locations.values())
        return locs

#------------------------------------------------------------------------------
