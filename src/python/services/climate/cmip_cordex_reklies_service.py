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

#remote debugging via embedded code
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

from common import rect_ascii_grid_management as grid_man, common, geo, capnp_async_helpers as async_helpers

import common.geo as geo

import capnp
#import capnproto_schemas.common_capnp as common_capnp
from capnproto_schemas import geo_coord_capnp as geo_capnp, climate_data_capnp

#------------------------------------------------------------------------------

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


wgs84 = CRS.from_epsg(4326)
gk3 = CRS.from_epsg(3396)
gk5 = CRS.from_epsg(31469)
utm21s = CRS.from_epsg(32721)
utm32n = CRS.from_epsg(25832)


cdict = {}
def create_lat_lon_interpolator_from_json_coords_file(path_to_json_coords_file):
    "create interpolator from json list of lat/lon to row/col mappings"
    with open(path_to_json_coords_file) as _:
        points = []
        values = []
        
        for latlon, rowcol in json.load(_):
            row, col = rowcol
            lat, lon = latlon
            #alt = float(line[3])
            cdict[(row, col)] = {"lat": round(lat, 5), "lon": round(lon, 5), "alt": -9999}
            points.append([lat, lon])
            values.append((row, col))
            #print("row:", row, "col:", col, "clat:", clat, "clon:", clon, "h:", h, "r:", r, "val:", values[i])

        return NearestNDInterpolator(np.array(points), np.array(values))


def lat_lon_interpolator(path_to_latlon_to_rowcol_json_file):
    "create an interpolator for the macsur grid"
    if not hasattr(lat_lon_interpolator, "interpol"):
        lat_lon_interpolator.interpol = create_lat_lon_interpolator_from_json_coords_file(path_to_latlon_to_rowcol_json_file)
    return lat_lon_interpolator.interpol


def string_to_gcm(gcm_str):
    if not hasattr(string_to_gcm, "d"):
        string_to_gcm.d = {
            "CCCma-CanESM2": climate_data_capnp.Climate.GCM.cccmaCanEsm2,
            "ICHEC-EC-EARTH": climate_data_capnp.Climate.GCM.ichecEcEarth,
            "IPSL-IPSL-CM5A-MR": climate_data_capnp.Climate.GCM.ipslIpslCm5AMr,
            "MIROC-MIROC5": climate_data_capnp.Climate.GCM.mirocMiroc5,
            "MPI-M-MPI-ESM-LR": climate_data_capnp.Climate.GCM.mpiMMpiEsmLr
        }
    return string_to_gcm.d.get(gcm_str, None)

def gcm_to_info(gcm):
    if not hasattr(gcm_to_info, "d"):
        gcm_to_info.d = {
            climate_data_capnp.Climate.GCM.cccmaCanEsm2: {"id": "CCCma-CanESM2", "name": "CCCma-CanESM2", "description": ""},
            climate_data_capnp.Climate.GCM.ichecEcEarth: {"id": "ICHEC-EC-EARTH", "name": "ICHEC-EC-EARTH", "description": ""},
            climate_data_capnp.Climate.GCM.ipslIpslCm5AMr: {"id": "IPSL-IPSL-CM5A-MR", "name": "IPSL-IPSL-CM5A-MR", "description": ""},
            climate_data_capnp.Climate.GCM.mirocMiroc5: {"id": "MIROC-MIROC5", "name": "MIROC-MIROC5", "description": ""},
            climate_data_capnp.Climate.GCM.mpiMMpiEsmLr: {"id": "MPI-M-MPI-ESM-LR", "name": "MPI-M-MPI-ESM-LR", "description": ""}
        }
    return gcm_to_info.d.get(gcm.raw, None)


def string_to_rcm(rcm_str):
    if not hasattr(string_to_rcm, "d"):
        string_to_rcm.d = {
            "CLMcom-CCLM4-8-17": climate_data_capnp.Climate.RCM.clmcomCclm4817,
            "GERICS-REMO2015": climate_data_capnp.Climate.RCM.gericsRemo2015,
            "KNMI-RACMO22E": climate_data_capnp.Climate.RCM.knmiRacmo22E,
            "SMHI-RCA4": climate_data_capnp.Climate.RCM.smhiRca4,
            "CLMcom-BTU-CCLM4-8-17": climate_data_capnp.Climate.RCM.clmcomBtuCclm4817,
            "MPI-CSC-REMO2009": climate_data_capnp.Climate.RCM.mpiCscRemo2009
        }
    return string_to_rcm.d.get(rcm_str, None)

def rcm_to_info(rcm):
    if not hasattr(rcm_to_info, "d"):
        rcm_to_info.d = {
            climate_data_capnp.Climate.RCM.clmcomCclm4817: {"id": "CLMcom-CCLM4-8-17", "name": "CLMcom-CCLM4-8-17", "description": ""},
            climate_data_capnp.Climate.RCM.gericsRemo2015: {"id": "GERICS-REMO2015", "name": "GERICS-REMO2015", "description": ""},
            climate_data_capnp.Climate.RCM.knmiRacmo22E: {"id": "KNMI-RACMO22E", "name": "KNMI-RACMO22E", "description": ""},
            climate_data_capnp.Climate.RCM.smhiRca4: {"id": "SMHI-RCA4", "name": "SMHI-RCA4", "description": ""},
            climate_data_capnp.Climate.RCM.clmcomBtuCclm4817: {"id": "CLMcom-BTU-CCLM4-8-17", "name": "CLMcom-BTU-CCLM4-8-17", "description": ""},
            climate_data_capnp.Climate.RCM.mpiCscRemo2009: {"id": "MPI-CSC-REMO2009", "name": "MPI-CSC-REMO2009", "description": ""}
        } 
    return rcm_to_info.d.get(rcm.raw, None)


def string_to_ensmem(ensmem_str):
    # split r<N>i<M>p<L>
    sr, sip = ensmem_str[1:].split("i")
    si, sp = sip.split("p")
    return {"r": int(sr), "i": int(si), "p": int(sp)}

def ensmem_to_info(ensmem):
    # r<N>i<M>p<L>
    id = "r{r}i{i}p{p}".format(r=ensmem.r, i=ensmem.i, p=ensmem.p)
    description = "Realization: #{r}, Initialization: #{i}, Pertubation: #{p}".format(r=ensmem.r, i=ensmem.i, p=ensmem.p)
    return {"id": id, "name": id, "description": description}


def date_to_info(d):
    iso = d.isoformat()[:10]
    return {"id": iso, "name": iso, "description": ""}


def rcp_or_ssp_to_info_factory(type):
    fwd_map = {
        "RCP": climate_data_capnp.Climate.RCP.schema.enumerants,
        "SSP": climate_data_capnp.Climate.SSP.schema.enumerants
    }.get(type.upper(), None)

    if not fwd_map:
        return {"id": "", "name": "", "description": ""}

    rev_map = {}
    for k, v in fwd_map.items():
        rev_map[v] = k
    
    def to_info(rev_map, xxp):
        id = rev_map[xxp]
        name = id[:3].upper() + id[3:]
        return {"id": id, "name": name, "description": ""}

    return lambda xxp: to_info(rev_map, xxp)

access_entries = {
    "gcm": lambda e: e.gcm,
    "rcm": lambda e: e.rcm,
    "historical": lambda e: True,
    "rcp": lambda e: e.rcp,
    "ssp": lambda e: e.ssp,
    "ensMem": lambda e: e.ensMem,
    "version": lambda e: e.version,
    "start": lambda e: create_date(e.start),
    "end": lambda e: create_date(e.end)
}
def create_entry_map(entries):
    entry_to_value = {}
    for e in entries:
        which = e.which()
        entry_to_value[which] = access_entries[which](e)
    
    return entry_to_value


def create_date(capnp_date):
    return date(capnp_date.year, capnp_date.month, capnp_date.day)

def create_capnp_date(py_date):
    return {
        "year": py_date.year if py_date else 0,
        "month": py_date.month if py_date else 0,
        "day": py_date.day if py_date else 0
    }
    

class TimeSeries(climate_data_capnp.Climate.TimeSeries.Server): 

    def __init__(self, metadata, location, path_to_csv=None, dataframe=None):
        "a supplied dataframe asumes the correct index is already set (when reading from csv then it will always be 1980 to 2010)"

        if not path_to_csv and not dataframe:
            raise Exception("Missing argument, either path_to_csv or dataframe have to be supplied!")

        self._path_to_csv = path_to_csv
        self._df = dataframe
        self._meta = metadata
        self._location = location

    @classmethod
    def from_csv_file(cls, metadata, location, path_to_csv):
        return TimeSeries(metadata, location, path_to_csv=path_to_csv)

    @classmethod
    def from_dataframe(cls, metadata, location, dataframe):
        return TimeSeries(metadata, location, dataframe=dataframe)

    @property
    def dataframe(self):
        "init underlying dataframe lazily if initialized with path to csv file"
        if self._df is None and self._path_to_csv:
            # load csv file
            self._df = pd.read_csv(self._path_to_csv, skiprows=[1], index_col=0)
            self._df.rename(columns={"windspeed": "wind"}, inplace=True)

            # reduce headers to the supported ones
            #all_supported_headers = ["tmin", "tavg", "tmax", "precip", "globrad", "wind", "relhumid"]
            #self._df = self._df.loc[:, all_supported_headers]

        return self._df

    def resolution_context(self, context): # -> (resolution :TimeResolution);
        context.results.resolution = climate_data_capnp.Climate.TimeSeries.Resolution.daily

    def range_context(self, context): # -> (startDate :Date, endDate :Date);
        context.results.startDate = create_capnp_date(date.fromisoformat(str(self.dataframe.index[0])[:10]))
        context.results.endDate = create_capnp_date(date.fromisoformat(str(self.dataframe.index[-1])[:10]))
        
    def header(self, **kwargs): # () -> (header :List(Element));
        return self.dataframe.columns.tolist()

    def data(self, **kwargs): # () -> (data :List(List(Float32)));
        return self.dataframe.to_numpy().tolist()

    def dataT(self, **kwargs): # () -> (data :List(List(Float32)));
        return self.dataframe.T.to_numpy().tolist()
                
    def subrange(self, from_, to, **kwargs): # (from :Date, to :Date) -> (timeSeries :TimeSeries);
        from_date = create_date(from_)
        to_date = create_date(to)

        sub_df = self._df.loc[str(from_date):str(to_date)]

        return TimeSeries.from_dataframe(self._real, self._location, sub_df)

    def subheader(self, elements, **kwargs): # (elements :List(Element)) -> (timeSeries :TimeSeries);
        sub_headers = [str(e) for e in elements]
        sub_df = self.dataframe.loc[:, sub_headers]

        return TimeSeries.from_dataframew(self._real, self._location, sub_df)

    def metadata(self, _context, **kwargs): # metadata @7 () -> Metadata;
        "the metadata for this time series"
        r = _context.results
        r.init("entries", len(self._meta.entries))
        for i, e in enumerate(self._meta.entries):
            r.entries[i] = e
        r.info = self._meta.info

    def location(self, _context, **kwargs): # location @8 () -> Location;
        "location of this time series"
        r = _context.results
        r.id = self._location.id
        r.heightNN = self._location.heightNN
        r.geoCoord = self._location.geoCoord
        r.timeSeries = self


class Dataset(climate_data_capnp.Climate.Dataset.Server):

    def __init__(self, metadata, path_to_rows, interpolator):
        self._meta = metadata
        self._path_to_rows = path_to_rows
        self._interpolator = interpolator
        self._time_series = {}
        self._locations = {}
        self._all_locations_created = False

    def metadata(self, _context, **kwargs): # metadata @0 () -> Metadata;
        # get metadata for these data 
        r = _context.results
        r.init("entries", len(self._meta.entries))
        for i, e in enumerate(self._meta.entries):
            r.entries[i] = e
        r.info = self._meta.info
        
    def time_series_at(self, row, col, location=None):
        if (row, col) not in self._time_series:
            path_to_csv = self._path_to_rows + "/row-" + str(row) + "/col-" + str(col) + ".csv"
            if not location:
                location = self.location_at(row, col)
            time_series = TimeSeries.from_csv_file(self._meta, location, path_to_csv)    
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
                coord = cdict[(row, col)]
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
            for row_col, coord in cdict.items():
                row, col = row_col
                loc = self.location_at(row, col, coord)
                ts = self.time_series_at(row, col, loc)
                loc.timeSeries = ts
                locs.append(loc)
            self._all_locations_created = True
        else:
            locs.extend(self._locations.values())
        return locs


class Metadata_Info(climate_data_capnp.Climate.Metadata.Info.Server):

    def __init__(self, metadata):
        self._meta = metadata
        self._entry_map = create_entry_map(metadata.entries)
        rcp_to_info = rcp_or_ssp_to_info_factory("rcp")
        ssp_to_info = rcp_or_ssp_to_info_factory("ssp")
        self._entry_to_info = {
            "gcm": lambda v: gcm_to_info(v),
            "rcm": lambda v: rcm_to_info(v),
            "historical": lambda v: {"id": "historical", "name": "Historical", "description": ""},
            "rcp": lambda v: rcp_to_info(v),
            "ssp": lambda v: ssp_to_info(v),
            "ensMem": lambda v: ensmem_to_info(v),
            "version": lambda v: {"id": v, "name": v, "description": ""}, 
            "start": lambda v: date_to_info(create_date(v)),
            "end": lambda v: date_to_info(create_date(v))    
        }

    def forOne(self, entry, _context, **kwargs): # forOne @0 (entry :Entry) -> Common.IdInformation;
        which = entry.which()
        value = self._entry_map[which]
        id_info = self._entry_to_info[which](value)
        r = _context.results
        r.id = id_info["id"]
        r.name = id_info["name"]
        r.description = id_info["description"]

    def forAll(self, **kwargs): # forAll @0 () -> (all :List(Common.IdInformation));
        id_infos = []
        for e in self._meta.entries:
            which = e.which()
            value = self._entry_map[which]
            id_infos.append({"fst": e, "snd": self._entry_to_info[which](value)})
        return id_infos


def create_meta_plus_datasets(path_to_data_dir, interpolator):
    datasets = []
    for gcm in os.listdir(path_to_data_dir):
        gcm_dir = path_to_data_dir + gcm
        if os.path.isdir(gcm_dir):
            for rcm in os.listdir(gcm_dir):
                rcm_dir = gcm_dir + "/" + rcm
                if os.path.isdir(rcm_dir):
                    for scen in os.listdir(rcm_dir):
                        scen_dir = rcm_dir + "/" + scen
                        if os.path.isdir(scen_dir):
                            for ensmem in os.listdir(scen_dir):
                                ensmem_dir = scen_dir + "/" + ensmem
                                if os.path.isdir(ensmem_dir):
                                    for version in os.listdir(ensmem_dir):
                                        version_dir = ensmem_dir + "/" + version
                                        if os.path.isdir(version_dir):
                                            metadata = climate_data_capnp.Climate.Metadata.new_message(
                                                entries = [
                                                    {"gcm": string_to_gcm(gcm)},
                                                    {"rcm": string_to_rcm(rcm)},
                                                    {"historical": None} if scen == "historical" else {"rcp": scen},
                                                    {"ensMem": string_to_ensmem(ensmem)},
                                                    {"version": version}
                                                ]
                                            )
                                            metadata.info = Metadata_Info(metadata)
                                            datasets.append(climate_data_capnp.Climate.MetaPlusData.new_message(
                                                meta=metadata, 
                                                data=Dataset(metadata, version_dir, interpolator)
                                            ))
    return datasets


class Service(climate_data_capnp.Climate.Service.Server):

    def __init__(self, path_to_data_dir, interpolator, id=None, name=None, description=None):
        self._id = id if id else "cmip_cordex_reklies"
        self._name = name if name else "CMIP Cordex Reklies"
        self._description = description if description else ""
        self._meta_plus_datasets = create_meta_plus_datasets(path_to_data_dir, interpolator)

    def info(self, _context, **kwargs): # () -> IdInformation;
        r = _context.results
        r.id = self._id
        r.name = self._name
        r.description = self._description

    def getAvailableDatasets(self, **kwargs): # getAvailableDatasets @0 () -> (datasets :List(MetaPlusData));
        "get a list of all available datasets"
        return self._meta_plus_datasets

    def getDatasetsFor(self, template, **kwargs): # getDatasets @1 (template :Metadata) -> (datasets :List(Dataset));
        "get a reference to the simulation with given id"
        search_entry_to_value = create_entry_map(template.entries)

        def contains_search_entries(mds):
            for e in mds.meta.entries:
                which = e.which()
                if which in search_entry_to_value and search_entry_to_value[which] != access_entries[which](e):
                    return False
            return True

        meta_plus_datasets = filter(contains_search_entries, self._meta_plus_datasets)
        datasets = map(lambda mds: mds.data, meta_plus_datasets)
        return list(datasets)

#------------------------------------------------------------------------------

async def async_main_register(path_to_data, reg_server=None, reg_port=None, id=None, name=None, description=None):
    config = {
        "path_to_data": path_to_data,
        "id": id,
        "name": name,
        "description": description,
        "reg_port": str(reg_port),
        "reg_server": reg_server
    }
    # read commandline args only if script is invoked directly from commandline
    if len(sys.argv) > 1 and __name__ == "__main__":
        for arg in sys.argv[1:]:
            k, v = arg.split("=")
            if k in config:
                config[k] = v
    print("config used:", config)

    interpolator = lat_lon_interpolator(config["path_to_data"] + "/" + "latlon-to-rowcol.json")
    service = Service(config["path_to_data"] + "/csv/", interpolator)

    registry_available = False
    connect_to_registry_retry_count = 10
    retry_secs = 5
    while not registry_available:
        try:
            client, gather_results = await async_helpers.connect_to_server(port=config["reg_port"], address=config["reg_server"])
            registry_available = True
        except:
            if connect_to_registry_retry_count == 0:
                print("Couldn't connect to registry server at {}:{}!".format(config["reg_server"], config["reg_port"]))
                exit(0)
            connect_to_registry_retry_count -= 1
            print("Trying to connect to {}:{} again in {} secs!".format(config["reg_server"], config["reg_port"], retry_secs))
            time.sleep(retry_secs)
            retry_secs += 1

    registry = client.bootstrap().cast_as(reg_capnp.Service.Registry)
    unreg = await registry.registerService(type="soil", service=service).a_wait()
    #await unreg.unregister.unregister().a_wait()

    print("registered soil service")

    #await unreg.unregister.unregister().a_wait()

    await gather_results

    print("after gather_results")

#------------------------------------------------------------------------------

def sync_main_server(path_to_data, port):
    config = {
        "path_to_data": path_to_data,
        "port": str(reg_port),
        "server": "*"
    }
    # read commandline args only if script is invoked directly from commandline
    if len(sys.argv) > 1 and __name__ == "__main__":
        for arg in sys.argv[1:]:
            k, v = arg.split("=")
            if k in config:
                config[k] = v
    print("config used:", config)

    interpolator = lat_lon_interpolator(config["path_to_data"] + "/" + "latlon-to-rowcol.json")
    server = capnp.TwoPartyServer(config["server"] + ":" + config["port"], bootstrap=Service(config["path_to_data"] + "/csv/", interpolator))
    server.run_forever()

#------------------------------------------------------------------------------

if __name__ == '__main__':
    path_to_data = "/beegfs/common/data/climate/dwd/cmip_cordex_reklies"

    if len(sys.argv) > 1:
        command = sys.argv[1]
        if command == "sync_server":
            sys.argv.pop(1)
            sync_main_server(path_to_data, 12000)
        elif command == "async_register":
            sys.argv.pop(1)
            asyncio.run(async_main_register(path_to_data, reg_server="login01.cluster.zalf.de", reg_port=11001))

    print("sync_server | async_register")