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

import csv
import json
from datetime import date, timedelta
import numpy as np
import os
import pandas as pd
from pathlib import Path
from pyproj import CRS, Transformer
from scipy.interpolate import NearestNDInterpolator
import sys
import time

#import ptvsd
#ptvsd.enable_attach(("0.0.0.0", 14000))
#ptvsd.wait_for_attach()  # blocks execution until debugger is attached

#remote debugging via commandline
#-m ptvsd --host 0.0.0.0 --port 14000 --wait

PATH_TO_REPO = Path(os.path.realpath(__file__)).parent.parent.parent.parent.parent
if str(PATH_TO_REPO) not in sys.path:
    sys.path.insert(1, str(PATH_TO_REPO))

PATH_TO_PYTHON_CODE = PATH_TO_REPO / "src/python"
if str(PATH_TO_PYTHON_CODE) not in sys.path:
    sys.path.insert(1, str(PATH_TO_PYTHON_CODE))

import common.geo as geo

import capnp
from capnproto_schemas import common_capnp, geo_coord_capnp as geo_capnp, climate_data_old_capnp as climate_data_capnp
#import capnproto_schemas.model_capnp as model_capnp

ADAPT_TIMESERIES_TO_FUTURE_DATES = True

def read_header(path_to_ascii_grid_file):
    "read metadata from esri ascii grid file"
    metadata = {}
    header_str = ""
    with open(path_to_ascii_grid_file) as _:
        for i in range(0, 6):
            line = _.readline()
            header_str += line
            sline = [x for x in line.split() if len(x) > 0]
            if len(sline) > 1:
                metadata[sline[0].strip().lower()] = float(sline[1].strip())
    return metadata, header_str

def create_ascii_grid_interpolator(arr, meta, ignore_nodata=True):
    "read an ascii grid into a map, without the no-data values"

    rows, cols = arr.shape

    cellsize = int(meta["cellsize"])
    xll = int(meta["xllcorner"])
    yll = int(meta["yllcorner"])
    nodata_value = meta["nodata_value"]

    xll_center = xll + cellsize // 2
    yll_center = yll + cellsize // 2
    yul_center = yll_center + (rows - 1)*cellsize

    points = []
    values = []

    for row in range(rows):
        for col in range(cols):
            value = arr[row, col]
            if ignore_nodata and value == nodata_value:
                continue
            r = xll_center + col * cellsize
            h = yul_center - row * cellsize
            points.append([r, h])
            values.append(value)

    return NearestNDInterpolator(np.array(points), np.array(values))

def read_file_and_create_interpolator(path_to_grid, dtype=int, skiprows=6, confirm_creation=False):
    "read file and metadata and create interpolator"

    metadata, _ = read_header(path_to_grid)
    grid = np.loadtxt(path_to_grid, dtype=dtype, skiprows=skiprows)
    interpolate = create_ascii_grid_interpolator(grid, metadata)
    if confirm_creation: 
        print("created interpolator from:", path_to_grid)
    return (interpolate, grid, metadata)

wgs84 = Proj(init="epsg:4326")
gk3 = Proj(init="epsg:3396")
gk5 = Proj(init="epsg:31469")
utm21s = Proj(init="epsg:32721")
utm32n = Proj(init="epsg:25832")

cdict = {}
def create_lat_lon_interpolator_from_csv_coords_file(path_to_csv_coords_file):
    "create interpolator from json list of lat/lon to row/col mappings"
    with open(path_to_csv_coords_file) as _:
        reader = csv.reader(_)
        next(reader)

        points = []
        values = []
        
        for line in reader:
            rowcol = float(line[0])
            row = int(rowcol / 1000)
            col = int(rowcol - row*1000)
            lat = float(line[1])
            lon = float(line[2])
            alt = float(line[3])
            cdict[(row, col)] = {"lat": round(lat, 5), "lon": round(lon, 5), "alt": alt}
            points.append([lat, lon])
            values.append((row, col))
            #print("row:", row, "col:", col, "clat:", clat, "clon:", clon, "h:", h, "r:", r, "val:", values[i])

        return NearestNDInterpolator(np.array(points), np.array(values))


def lat_lon_interpolator():
    "create an interpolator for the macsur grid"
    if not hasattr(lat_lon_interpolator, "interpol"):
        lat_lon_interpolator.interpol = create_lat_lon_interpolator_from_csv_coords_file("macsur_european_climate_scenarios_geo_coords_and_altitude.csv")
    return lat_lon_interpolator.interpol


class Station(climate_data_capnpData.Station.Server):

    def __init__(self, sim, id, geo_coord, name=None, description=None):
        self._sim = sim
        self._id = id
        self._name = name if name else id
        self._description = description if description else ""
        self._time_series = []
        self._geo_coord = geo_coord

    def info(self, **kwargs): # () -> (info :IdInformation);
        return common_capnp.IdInformation.new_message(id=self._id, name=self._name, description=self._description) 

    def simulationInfo(self, **kwargs): # () -> (simInfo :IdInformation);
        return self._sim.info()

    def heightNN(self, **kwargs): # () -> (heightNN :Int32);
        return self._geo_coord["alt"]

    def geoCoord(self, **kwargs): # () -> (geoCoord :Geo.Coord);
        coord = geo_capnp.Coord.new_message()
        coord.init("latlon")
        coord.latlon.lat = self._geo_coord["lat"]
        coord.latlon.lon = self._geo_coord["lon"]
        return coord
        #return {"gk": {"meridianNo": 5, "r": 1, "h": 2}}

    def allTimeSeries(self, **kwargs): # () -> (allTimeSeries :List(TimeSeries));
        # get all time series available at this station 
        
        if len(self._time_series) == 0:
            for scen in self._sim.scenarios:
                for real in scen.realizations:
                    for ts in real.closest_time_series_at(self._geo_coord["lat"], self._geo_coord["lon"]):
                        self._time_series.append(ts)
        
        return self._time_series

    def timeSeriesFor(self, scenarioId, realizationId, **kwargs): # (scenarioId :Text, realizationId :Text) -> (timeSeries :TimeSeries);
        # get all time series for a given scenario and realization at this station
        return list(filter(lambda ts: ts.scenarioInfo().id == scenarioId and ts.realizationInfo().id == realizationId, self.allTimeSeries()))


def create_date(capnp_date):
    return date(capnp_date.year, capnp_date.month, capnp_date.day)

def create_capnp_date(py_date):
    return {
        "year": py_date.year if py_date else 0,
        "month": py_date.month if py_date else 0,
        "day": py_date.day if py_date else 0
    }
    
class TimeSeries(climate_data_capnpData.TimeSeries.Server): 

    def __init__(self, realization, path_to_csv=None, time_range_id=None, dataframe=None, adapt_timeseries_to_future_dates=None):
        "a supplied dataframe asumes the correct index is already set (when reading from csv then it will always be 1980 to 2010)"

        if not path_to_csv and not dataframe:
            raise Exception("Missing argument, either path_to_csv or dataframe have to be supplied!")
        if path_to_csv and not time_range_id:
            raise Exception("Missing argument, when supplying the path_to_csv, you also have to supply a time_range_id!")

        self._path_to_csv = path_to_csv
        self._time_range_id = time_range_id
        self._df = dataframe
        self._real = realization
        self._adapt_timeseries_to_future_dates = adapt_timeseries_to_future_dates

    @classmethod
    def from_csv_file(cls, realization, path_to_csv, time_range_id, adapt_timeseries_to_future_dates):
        return TimeSeries(realization, path_to_csv, time_range_id, adapt_timeseries_to_future_dates=adapt_timeseries_to_future_dates)

    @classmethod
    def from_dataframe(cls, realization, dataframe):
        return TimeSeries(dataframe)

    @property
    def dataframe(self):
        "init underlying dataframe lazily if initialized with path to csv file"
        if self._df is None and self._path_to_csv:
            # load csv file
            self._df = pd.read_csv(self._path_to_csv, skiprows=[1], index_col=0)

            # reduce headers to the supported ones
            all_supported_headers = ["tmin", "tavg", "tmax", "precip", "globrad", "wind", "relhumid"]
            self._df = self._df.loc[:, all_supported_headers]

            # update time ranges as the csv runs always from 1980 to 2010
            if self._time_range_id != "0" and self._adapt_timeseries_to_future_dates:
                time_range = {
                    "0": {"from": 1980, "to": 2010},
                    "2": {"from": 2040, "to": 2070},
                    "3": {"from": 2070, "to": 2100}
                }[self._time_range_id]
            
                if self._time_range_id == "3":
                    # drop the 1980 leap year day february 29th for the time range 2070 to 2100
                    self._df.drop("1980-02-29", axis=0, inplace=True)
                self._df.set_index(pd.date_range(date(time_range["from"], 1, 1), date(time_range["to"], 12, 31)), inplace=True)
            
        return self._df

    def resolution_context(self, context): # -> (resolution :TimeResolution);
        context.results.resolution = climate_data_capnp.TimeResolution.daily

    def range_context(self, context): # -> (startDate :Date, endDate :Date);
        context.results.startDate = create_capnp_date(date.fromisoformat(str(self.dataframe.index[0])[:10]))
        context.results.endDate = create_capnp_date(date.fromisoformat(str(self.dataframe.index[-1])[:10]))
        
    def header(self, **kwargs): # () -> (header :List(Element));
        return self.dataframe.columns.tolist()

    def data(self, **kwargs): # () -> (data :List(List(Float32)));
        return self.dataframe.to_numpy().tolist()

    def dataT(self, **kwargs): # () -> (data :List(List(Float32)));
        return self.dataframe.T.to_numpy().tolist()
                
    def subrange_context(self, context): # (from :Date, to :Date) -> (timeSeries :TimeSeries);
        from_date = create_date(getattr(context.params, "from"))
        to_date = create_date(context.params.to)

        sub_df = self._df.loc[str(from_date):str(to_date)]

        context.results.timeSeries = TimeSeries.from_dataframe(self._real, sub_df)

    def subheader_context(self, context): # (elements :List(Element)) -> (timeSeries :TimeSeries);
        sub_headers = [str(e) for e in context.params.elements]
        sub_df = self.dataframe.loc[:, sub_headers]

        context.results.timeSeries = TimeSeries.from_dataframew(self._real, sub_df)

    def simulationInfo(self, **kwargs): # simulationInfo @7 () -> (simulationInfo :Common.IdInformation);
        "which simulation does the time series belong to"
        return self._real.scenario.simulation.info()

    def scenarioInfo(self, **kwargs): # scenarioInfo @8 () -> (scenarioInfo :Common.IdInformation);
        "which scenario does the time series belong to"
        return self._real.scenario.info()

    def realizationInfo(self, **kwargs): # realizationInfo @9 () -> (realizationInfo :Common.IdInformation);
        "which realization does the time series belong to"
        return self._real.info()


class Simulation(climate_data_capnpData.Simulation.Server): 

    def __init__(self, id, name=None, description=None, scenario_ids=None):
        self._id = id
        self._name = name if name else self._id
        self._description = description if description else ""
        self._scens = None # []
        self._stations = None # []
        self._lat_lon_interpol = None

    @property
    def id(self):
        return self._id

    def info(self):
        return common_capnp.IdInformation.new_message(id=self._id, name=self._name, description=self._description) 

    def info_context(self, context): # () -> (info :IdInformation);
        context.results.info = self.info()

    @property
    def scenarios(self):
        if not self._scens:
            self._scens = Scenario.create_scenarios(self, self._path_to_sim_dir)
        return self._scens

    @scenarios.setter
    def scenarios(self, scens):
        self._scens = scens

    def scenarios_context(self, context): # () -> (scenarios :List(Scenario));
        context.results.init("scenarios", len(self.scenarios))
        for i, scen in enumerate(self.scenarios):
            context.results.scenarios[i] = scen

    def stations(self, **kwargs): # () -> (stations :List(Station));
        return list([Station(self, "[r:{}/c:{}]".format(row_col[0], row_col[1]), coord) for row_col, coord in cdict.items()])


class Scenario(climate_data_capnpData.Scenario.Server):

    def __init__(self, sim, id, reals=[], name=None, description=None):
        self._sim = sim
        self._id = id
        self._name = name if name else self._id
        self._description = description if description else ""
        self._reals = reals

    def info(self):
        return common_capnp.IdInformation.new_message(id=self._id, name=self._name, description=self._description) 

    def info_context(self, context): # -> (info :IdInformation);
        context.results.info = self.info()
        
    def simulationInfo(self, **kwargs): # () -> (simulationInfo :Common.IdInformation);
        return self._sim.info()

    @property
    def simulation(self):
        return self._sim

    @property
    def realizations(self):
        return self._reals

    @realizations.setter
    def realizations(self, reals):
        self._reals = reals

    def realizations_context(self, context): # -> (realizations :List(Realization));
        context.results.init("realizations", len(self.realizations))
        for i, real in enumerate(self.realizations):
            context.results.realizations[i] = real


class Realization(climate_data_capnpData.Realization.Server):

    def __init__(self, scen, paths_to_csv_config, id=None, name=None, description=None, adapt_timeseries_to_future_dates=True):
        self._scen = scen
        self._paths_to_csv_config = paths_to_csv_config
        self._id = id if id else "1"
        self._name = name if name else self._id
        self._description = description if description else ""
        self._adapt_timeseries_to_future_dates = adapt_timeseries_to_future_dates

    def info(self):
        return common_capnp.IdInformation.new_message(id=self._id, name=self._name, description=self._description) 

    def info_context(self, context): # -> (info :IdInformation);
        context.results.info = self.info()

    def scenarioInfo(self, **kwargs): # () -> (scenarioInfo :Common.IdInformation);
        return self._scen.info()
        
    @property
    def scenario(self):
        return self._scen

    def closest_time_series_at(self, lat, lon):

        row, col = lat_lon_interpolator()(lat, lon)

        c = self._paths_to_csv_config
        closest_time_series = []
        for time_range in c["time_ranges"]:
            formated_path = c["path_template"].format(sim_id=c["sim_id"], scen_id=c["scen_id"], version=c["version_id"], period_id=time_range, row=row, col=col)
            closest_time_series.append(TimeSeries.from_csv_file(self, formated_path, time_range, self._adapt_timeseries_to_future_dates))
            
        return closest_time_series


    def closestTimeSeriesAt(self, latlon, **kwargs): # (latlon :Geo.LatLonCoord) -> (timeSeries :List(TimeSeries));
        # closest TimeSeries object which represents the whole time series 
        # of the climate realization at the give climate coordinate
        return self.closest_time_series_at(latlon.lat, latlon.lon)


class Service(climate_data_capnpData.Service.Server):

    def __init__(self, id=None, name=None, description=None, adapt_timeseries_to_future_dates=True):
        self._id = id if id else "macsur_european_climate_scenarios_v2"
        self._name = name if name else "MACSUR European Climate Scenarios V2"
        self._description = description if description else ""
        self._sims = create_simulations(adapt_timeseries_to_future_dates)

    def info(self):
        return common_capnp.IdInformation.new_message(id=self._id, name=self._name, description=self._description) 

    def info_context(self, context): # -> (info :IdInformation);
        context.results.info = self.info()

    def getAvailableSimulations_context(self, context): # getAvailableSimulations @0 () -> (availableSimulations :List(Simulation));
        context.results.availableSimulations = self._sims
        #context.results.init("realizations", len(self._realizations))
        #for i, real in enumerate(self.realizations):
        #    context.results.realizations[i] = real

    def getSimulation(self, id, **kwargs): # getSimulation @1 (id :UInt64) -> (simulation :Simulation);
        for sim in self._sims:
            if sim.id == id:
                return sim


def create_simulations(adapt_timeseries_to_future_dates):
    sims_and_scens = {
        "0": {"scens": ["0"], "tranges": ["0"]},
        "GFDL-CM3": {"scens": ["45", "85"], "tranges": ["2", "3"]},
        "GISS-E2-R": {"scens": ["45", "85"], "tranges": ["2", "3"]},
        "HadGEM2-ES": {"scens": ["26", "45", "85"], "tranges": ["2", "3"]},
        "MIROC5": {"scens": ["45", "85"], "tranges": ["2", "3"]},
        "MPI-ESM-MR": {"scens": ["26", "45", "85"], "tranges": ["2", "3"]}
    }

    path_template = "/beegfs/common/data/climate/macsur_european_climate_scenarios_v2/transformed/{period_id}/{sim_id}_{scen_id}/{row}_{col:03d}_{version}.csv"

    sims = []
    for sim_id, scens_and_tranges in sims_and_scens.items():
        sim = Simulation(sim_id, name=sim_id)
        scens = []
        tranges = scens_and_tranges["tranges"]
        for scen_id in scens_and_tranges["scens"]:
            scen = Scenario(sim, scen_id, name=scen_id)
            real = Realization(scen, {
                "path_template": path_template, 
                "time_ranges": tranges, 
                "sim_id": sim_id, 
                "scen_id": scen_id,
                "version_id": "v2"
            }, adapt_timeseries_to_future_dates=adapt_timeseries_to_future_dates)
            scen.realizations = [real]
            scens.append(scen)
        sim.scenarios = scens
        sims.append(sim)
    return sims


def main():
    #address = parse_args().address

    #server = capnp.TwoPartyServer("*:8000", bootstrap=DataServiceImpl("/home/berg/archive/data/"))
    server = capnp.TwoPartyServer("*:11001", bootstrap=Service())
    server.run_forever()

if __name__ == '__main__':
    main()