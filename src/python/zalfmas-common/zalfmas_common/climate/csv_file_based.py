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

from collections import deque, OrderedDict
import capnp
from datetime import date
import gzip
import io
import itertools
import os
import pandas as pd
from pathlib import Path
import psutil
import sys

from zalfmas_common import common
from zalfmas_common.climate import common_climate_data_capnp_impl as ccdi
import zalfmas_capnpschemas

sys.path.append(os.path.dirname(zalfmas_capnpschemas.__file__))
import common_capnp
import geo_capnp
import persistence_capnp
import climate_capnp


class TimeSeries(climate_capnp.TimeSeries.Server, common.Identifiable, common.Persistable):

    def __init__(self, metadata=None, location=None, path_to_csv=None, dataframe=None, csv_string=None,
                 header_map=None, supported_headers=None, pandas_csv_config={}, transform_map=None,
                 id=None, name=None, description=None, restorer=None):
        common.Persistable.__init__(self, restorer)
        common.Identifiable.__init__(self, id, name, description)

        # global ID
        # self.__ID = ID
        # ID += 1
        # print("creating Timeseries:", name, "ID:", self.__ID)

        if path_to_csv is None and dataframe is None and csv_string is None:
            raise Exception("Missing argument, either path_to_csv or dataframe have to be supplied!")

        # print(csv_string)

        self._path_to_csv = path_to_csv
        self._csv_string = csv_string
        self._df = dataframe
        self._meta = metadata
        self._location = location

        self._header_map = header_map
        self._supported_headers = list(
            climate_capnp.Element.schema.enumerants.keys()) if supported_headers is None else supported_headers
        self._pandas_csv_config_defaults = {"skip_rows": [1], "index_col": 0, "sep": ","}
        self._pandas_csv_config = {**self._pandas_csv_config_defaults, **pandas_csv_config}
        self._transform_map = transform_map

        self._persistence_service = None

    @classmethod
    def from_csv_file(cls, path_to_csv, metadata=None, location=None, header_map=None,
                      supported_headers=None, pandas_csv_config=None, transform_map=None,
                      id=None, name=None, description=None, restorer=None):
        return TimeSeries(metadata=metadata, location=location, path_to_csv=path_to_csv, header_map=header_map,
                          supported_headers=supported_headers,
                          pandas_csv_config=pandas_csv_config, transform_map=transform_map, id=id, name=name,
                          description=description, restorer=restorer)

    @classmethod
    def from_csv_string(cls, csv_string, metadata=None, location=None, header_map=None,
                        supported_headers=None, pandas_csv_config=None, transform_map=None,
                        id=None, name=None, description=None, restorer=None):
        return TimeSeries(metadata=metadata, location=location, csv_string=csv_string, header_map=header_map,
                          supported_headers=supported_headers,
                          pandas_csv_config=pandas_csv_config, transform_map=transform_map, id=id, name=name,
                          description=description, restorer=restorer)

    @classmethod
    def from_dataframe(cls, dataframe, metadata=None, location=None, id=None, name=None, description=None,
                       restorer=None):
        return TimeSeries(metadata=metadata, location=location, dataframe=dataframe, id=id, name=name,
                          description=description, restorer=restorer)

    @property
    def dataframe(self):
        """init underlying dataframe lazily if initialized with path to csv file"""
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

    async def resolution(self, **kwargs):  # -> (resolution :TimeResolution);
        return climate_capnp.TimeSeries.Resolution.daily

    async def range(self, _context, **kwargs):  # -> (startDate :Date, endDate :Date);
        _context.results.startDate = ccdi.create_capnp_date(date.fromisoformat(str(self.dataframe.index[0])[:10]))
        _context.results.endDate = ccdi.create_capnp_date(date.fromisoformat(str(self.dataframe.index[-1])[:10]))

    async def header(self, **kwargs):  # () -> (header :List(Element));
        return self.dataframe.columns.tolist()

    async def data(self, **kwargs):  # () -> (data :List(List(Float32)));
        return self.dataframe.to_numpy().tolist()

    async def dataT(self, **kwargs):  # () -> (data :List(List(Float32)));
        return self.dataframe.T.to_numpy().tolist()

    async def subrange(self, _context, **kwargs):  # (from :Date, to :Date) -> (timeSeries :TimeSeries);
        ps = _context.params
        start_date = ccdi.create_date(ps.start) if ps._has("start") else self.dataframe.index[0]
        end_date = ccdi.create_date(ps.end) if ps._has("end") else self.dataframe.index[-1]

        sub_df = self.dataframe.loc[str(start_date):str(end_date)]

        _context.results.timeSeries = TimeSeries.from_dataframe(sub_df, metadata=self._meta, location=self._location,
                                                               name=self.name, description=self.description,
                                                               restorer=self._restorer)

    async def subheader(self, elements, **kwargs):  # (elements :List(Element)) -> (timeSeries :TimeSeries);
        sub_headers = [str(e) for e in elements]
        sub_df = self.dataframe.loc[:, sub_headers]

        return TimeSeries.from_dataframe(sub_df, metadata=self._meta, location=self._location,
                                         name=self.name, description=self.description, restorer=self._restorer)

    async def metadata(self, _context, **kwargs):  # metadata @7 () -> Metadata;
        """the metadata for this time series"""
        if self._meta:
            r = _context.results
            r.init("entries", len(self._meta.entries))
            for i, e in enumerate(self._meta.entries):
                r.entries[i] = e
            r.info = self._meta.info

    async def location(self, _context, **kwargs):  # location @8 () -> Location;
        """location of this time series"""
        r = _context.results
        r.timeSeries = self
        if self._location:
            r.id = self._location.id
            r.heightNN = self._location.heightNN
            r.latlon = self._location.latlon

    def __del__(self):
        pass
        # print("deleting timeseries:", self.name, "id:", self.__ID)


class Dataset(climate_capnp.Dataset.Server, common.Identifiable, common.Persistable):

    def __init__(self, metadata, path_to_rows, interpolator, rowcol_to_latlon,
                 gzipped=False, header_map=None, supported_headers=None, row_col_pattern="row-{row}/col-{col}.csv",
                 pandas_csv_config={}, transform_map=None, id=None, name=None, description=None, restorer=None,
                 percentage_of_main_memory_use=20, cache_data=True):
        common.Persistable.__init__(self, restorer)
        common.Identifiable.__init__(self, id, name, description)

        self._meta = metadata
        self._path_to_rows = path_to_rows
        self._interpolator = interpolator
        self._timeseries = {}
        self._creation_order = deque()
        self._locations = {}
        self._all_locations_created = False
        self._header_map = header_map
        self._supported_headers = supported_headers
        self._rowcol_to_latlon = OrderedDict(rowcol_to_latlon)
        self._row_col_pattern = row_col_pattern
        self._pandas_csv_config = pandas_csv_config
        self._transform_map = transform_map
        self._process = psutil.Process()
        self._percentage_of_main_memory_use = percentage_of_main_memory_use
        self._cache_data = cache_data

    async def metadata(self, _context, **kwargs):  # metadata @0 () -> Metadata;
        # get metadata for these data 
        r = _context.results
        r.init("entries", len(self._meta.entries))
        for i, e in enumerate(self._meta.entries):
            r.entries[i] = e
        r.info = self._meta.info

    def timeseries_at(self, row: int, col: int, location=None):
        if not self._cache_data or \
                (row, col) not in self._timeseries:
            path_to_csv = self._path_to_rows + "/" + self._row_col_pattern.format(row=row, col=col)
            if not location:
                location = self.location_at(row, col)
            timeseries = TimeSeries.from_csv_file(path_to_csv,
                                                  metadata=self._meta,
                                                  location=location,
                                                  supported_headers=self._supported_headers,
                                                  header_map=self._header_map,
                                                  pandas_csv_config=self._pandas_csv_config,
                                                  transform_map=self._transform_map,
                                                  name="row: {}/col: {}".format(row, col),
                                                  restorer=self._restorer)
            if location:
                location.timeSeries = timeseries
            if self._cache_data:
                self._timeseries[(row, col)] = timeseries
                self._creation_order.append((row, col))

        # print(self._process.memory_percent(memtype="rss"))
        while self._cache_data and \
                len(self._creation_order) > 1 and \
                self._process.memory_percent(memtype="rss") > self._percentage_of_main_memory_use:
            rc = self._creation_order[0] if len(self._creation_order) > 0 else None
            if rc != (row, col):
                rc = self._creation_order.popleft()
                ts: TimeSeries = self._timeseries.pop(rc)
                ts._df = None
                # print("after pop:", self._process.memory_percent(memtype="rss"))

        return self._timeseries[(row, col)] if self._cache_data else timeseries

    async def closestTimeSeriesAt(self, latlon, **kwargs):  # (latlon :Geo.LatLonCoord) -> (timeSeries :TimeSeries);
        # closest TimeSeries object which represents the whole time series 
        # of the climate realization at the give climate coordinate
        row, col = map(int, self._interpolator(latlon.lat, latlon.lon))
        return self.timeseries_at(row, col)

    async def timeSeriesAt(self, locationId, **kwargs):  # (locationId :Text) -> (timeSeries :TimeSeries);
        rs, cs = locationId.split("/")
        row = int(rs[2:])
        col = int(cs[2:])
        return self.timeseries_at(row, col)

    def create_location_id(self, row: int, col: int):
        return "r:{}/c:{}".format(row, col)

    def location_at(self, row: int, col: int, coord=None, timeseries=None):
        if not self._cache_data or \
                (row, col) not in self._locations:
            if not coord:
                coord = self._rowcol_to_latlon[(row, col)]
            id = self.create_location_id(row, col)
            name = "Row/Col:{}/{}|LatLon:{}/{}".format(row, col, coord["lat"], coord["lon"])
            loc = climate_capnp.Location.new_message(
                id={"id": id, "name": name, "description": ""},
                heightNN=coord["alt"],
                latlon={"lat": coord["lat"], "lon": coord["lon"]},
                customData=[climate_capnp.Location.KV.new_message(
                    key="row/col", value=geo_capnp.RowCol.new_message(row=row, col=col)
                )]
            )
            if timeseries:
                loc.timeSeries = timeseries
            if self._cache_data:
                self._locations[(row, col)] = loc
        return self._locations[(row, col)] if self._cache_data else loc

    async def locations(self, **kwargs):  # locations @3 () -> (locations :List(Location));
        # all the climate locations this dataset has
        locs = []
        if not self._all_locations_created:
            for row_col, coord in self._rowcol_to_latlon.items():
                row, col = row_col
                loc = self.location_at(row, col, coord)
                ts = self.timeseries_at(row, col, loc)
                loc.timeSeries = ts
                locs.append(loc)
            self._all_locations_created = True
        else:
            locs.extend(self._locations.values())
        return locs

    async def streamLocations(self, _context):  # streamLocations @4 (startAfterLocationId :Text) -> (locationsCallback :GetLocationsCallback);
        # all the climate locations this dataset has

        def create_loc(row_col, coord):
            row, col = row_col
            loc = self.location_at(row, col, coord)
            ts = self.timeseries_at(row, col, loc)
            loc.timeSeries = ts
            return loc

        loc_id = _context.params.startAfterLocationId
        if (loc_id and len(loc_id) > 0):
            it = itertools.dropwhile(lambda rcll: self.create_location_id(*rcll[0]) != loc_id,
                                     self._rowcol_to_latlon.items())
            next(it)
        else:
            it = self._rowcol_to_latlon.items()

        locs_gen = (create_loc(row_col, coord) for row_col, coord in it)
        _context.results.locationsCallback = GetLocationsCallback(locs_gen)


class GetLocationsCallback(climate_capnp.Dataset.GetLocationsCallback.Server):
    def __init__(self, locations_gen):
        self._locations_gen = locations_gen

    async def nextLocations(self, maxCount, **kwargs):  # nextLocations @1 (maxCount :Int64) -> (locations :List(Location));
        l = []
        for _ in range(maxCount):
            try:
                l.append(next(self._locations_gen))
            except StopIteration:
                break
        return l
