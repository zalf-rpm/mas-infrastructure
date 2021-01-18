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

class TimeSeries(climate_data_capnp.Climate.TimeSeries.Server): 

    def __init__(self, metadata=None, location=None, path_to_csv=None, dataframe=None, header_map=None, supported_headers=None,
        pandas_csv_config={}, transform_map=None):
        "a supplied dataframe asumes the correct index is already set (when reading from csv then it will always be 1980 to 2010)"

        if not path_to_csv and not dataframe:
            raise Exception("Missing argument, either path_to_csv or dataframe have to be supplied!")

        self._path_to_csv = path_to_csv
        self._df = dataframe
        self._meta = metadata
        self._location = location
        self._header_map = header_map
        self._supported_headers = supported_headers
        self._pandas_csv_config_defaults = {"skip_rows": [1], "index_col": 0, "sep": ","}
        self._pandas_csv_config = {**self._pandas_csv_config_defaults, **pandas_csv_config}
        self._transform_map = transform_map


    @classmethod
    def from_csv_file(cls, path_to_csv, metadata=None, location=None, header_map=None, supported_headers=None, pandas_csv_config=None,
        transform_map=None):
        return TimeSeries(metadata=metadata, location=location, path_to_csv=path_to_csv, header_map=header_map, supported_headers=supported_headers,
            pandas_csv_config=pandas_csv_config, transform_map=transform_map)


    @classmethod
    def from_dataframe(cls, dataframe, metadata=None, location=None):
        return TimeSeries(metadata=metadata, location=location, dataframe=dataframe)


    @property
    def dataframe(self):
        "init underlying dataframe lazily if initialized with path to csv file"
        if self._df is None and self._path_to_csv:
            # load csv file
            if self._path_to_csv[-2:] == "gz":
                with gzip.open(self._path_to_csv) as _:
                    self._df = pd.read_csv(_, 
                        skiprows=self._pandas_csv_config["skip_rows"], 
                        index_col=self._pandas_csv_config["index_col"],
                        sep=self._pandas_csv_config["sep"])
            else:
                self._df = pd.read_csv(self._path_to_csv, 
                    skiprows=self._pandas_csv_config["skip_rows"], 
                    index_col=self._pandas_csv_config["index_col"],
                    sep=self._pandas_csv_config["sep"])
            
            if self._header_map:
                self._df.rename(columns=self._header_map, inplace=True)

            # reduce headers to the supported ones
            #all_supported_headers = ["tmin", "tavg", "tmax", "precip", "globrad", "wind", "relhumid"]
            if self._supported_headers:
                self._df = self._df.loc[:, self._supported_headers]

            if self._transform_map:
                for col_name, trans_func in self._transform_map.items():
                    self._df[col_name] = self._df[col_name].map(trans_func)

        return self._df


    def resolution_context(self, context): # -> (resolution :TimeResolution);
        context.results.resolution = climate_data_capnp.Climate.TimeSeries.Resolution.daily


    def range_context(self, context): # -> (startDate :Date, endDate :Date);
        context.results.startDate = ccdi.create_capnp_date(date.fromisoformat(str(self.dataframe.index[0])[:10]))
        context.results.endDate = ccdi.create_capnp_date(date.fromisoformat(str(self.dataframe.index[-1])[:10]))
        

    def header(self, **kwargs): # () -> (header :List(Element));
        return self.dataframe.columns.tolist()


    def data(self, **kwargs): # () -> (data :List(List(Float32)));
        return self.dataframe.to_numpy().tolist()


    def dataT(self, **kwargs): # () -> (data :List(List(Float32)));
        return self.dataframe.T.to_numpy().tolist()


    def subrange(self, from_, to, **kwargs): # (from :Date, to :Date) -> (timeSeries :TimeSeries);
        from_date = ccdi.create_date(from_)
        to_date = ccdi.create_date(to)

        sub_df = self._df.loc[str(from_date):str(to_date)]

        return TimeSeries.from_dataframe(sub_df, metadata=self._meta, location=self._location)


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
            r.geoCoord = self._location.geoCoord

#------------------------------------------------------------------------------

class Dataset(climate_data_capnp.Climate.Dataset.Server):

    def __init__(self, metadata, path_to_rows, interpolator, rowcol_to_latlon, 
        gzipped=False, header_map=None, supported_headers=None, row_col_pattern="row-{row}/col-{col}.csv",
        pandas_csv_config={}, transform_map=None):
        self._meta = metadata
        self._path_to_rows = path_to_rows
        self._interpolator = interpolator
        self._time_series = {}
        self._locations = {}
        self._all_locations_created = False
        self._header_map = header_map
        self._supported_headers = supported_headers
        self._rowcol_to_latlon = rowcol_to_latlon
        self._row_col_pattern = row_col_pattern
        self._pandas_csv_config = pandas_csv_config
        self._transform_map = transform_map


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
        return self._time_series[(row, col)]


    def closestTimeSeriesAt(self, geoCoord, **kwargs): # (geoCoord :Geo.Coord) -> (timeSeries :TimeSeries);
        # closest TimeSeries object which represents the whole time series 
        # of the climate realization at the give climate coordinate
        lat, lon = geo.geo_coord_to_latlon(geoCoord)
        row, col = self._interpolator(lat, lon)
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
            loc = climate_data_capnp.Climate.Location.new_message(
                id={"id": id, "name": name, "description": ""},
                heightNN=coord["alt"],
                geoCoord={"latlon": {"lat": coord["lat"], "lon": coord["lon"]}},
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
