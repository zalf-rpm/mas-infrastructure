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

PATH_TO_REPO = Path(os.path.realpath(__file__)).parent.parent.parent.parent.parent
if str(PATH_TO_REPO) not in sys.path:
    sys.path.insert(1, str(PATH_TO_REPO))

PATH_TO_PYTHON_CODE = PATH_TO_REPO / "src/python"
if str(PATH_TO_PYTHON_CODE) not in sys.path:
    sys.path.insert(1, str(PATH_TO_PYTHON_CODE))

#import common.common as cc
#import common.geo as geo

import capnp
import capnproto_schemas.geo_coord_capnp as geo_capnp
import capnproto_schemas.climate_data_capnp as climate_data_capnp

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

#------------------------------------------------------------------------------

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

#------------------------------------------------------------------------------

def read_file_and_create_interpolator(path_to_grid, dtype=int, skiprows=6, confirm_creation=False):
    "read file and metadata and create interpolator"

    metadata, _ = read_header(path_to_grid)
    grid = np.loadtxt(path_to_grid, dtype=dtype, skiprows=skiprows)
    interpolate = create_ascii_grid_interpolator(grid, metadata)
    if confirm_creation: 
        print("created interpolator from:", path_to_grid)
    return (interpolate, grid, metadata)

#------------------------------------------------------------------------------

def name_to_crs(name):
    if not hasattr(name_to_crs, "d"):
        name_to_crs.d = {
            "latlon": CRS.from_epsg(4326), 
            "wgs84": CRS.from_epsg(4326),
            "gk3": CRS.from_epsg(3396),
            "gk5": CRS.from_epsg(31469),
            "utm21s": CRS.from_epsg(32721),
            "utm32n": CRS.from_epsg(25832)
        }
    return name_to_crs.d.get(name, None)

#------------------------------------------------------------------------------

def create_lat_lon_interpolator_from_json_coords_file(path_to_json_coords_file):
    """
    create interpolator from json list of lat/lon to row/col mappings and return rowcol to latlon dict
    """

    cdict = {}
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

        return (NearestNDInterpolator(np.array(points), np.array(values)), cdict)

#------------------------------------------------------------------------------

def lat_lon_interpolator(path_to_latlon_to_rowcol_json_file):
    "create an interpolator for the macsur grid"
    if not hasattr(lat_lon_interpolator, "interpol"):
        lat_lon_interpolator.interpol = create_lat_lon_interpolator_from_json_coords_file(path_to_latlon_to_rowcol_json_file)
    return lat_lon_interpolator.interpol

#------------------------------------------------------------------------------

def string_to_gcm(gcm_str):
    if not hasattr(string_to_gcm, "d"):
        string_to_gcm.d = {
            "CCCma-CanESM2": climate_data_capnp.Climate.GCM.cccmaCanEsm2,
            "ICHEC-EC-EARTH": climate_data_capnp.Climate.GCM.ichecEcEarth,
            "IPSL-IPSL-CM5A-MR": climate_data_capnp.Climate.GCM.ipslIpslCm5AMr,
            "MIROC-MIROC5": climate_data_capnp.Climate.GCM.mirocMiroc5,
            "MPI-M-MPI-ESM-LR": climate_data_capnp.Climate.GCM.mpiMMpiEsmLr,
            "GFDL-ESM4": climate_data_capnp.Climate.GCM.gfdlEsm4,
            "IPSL-CM6A-LR": climate_data_capnp.Climate.GCM.ipslCm6aLr,
            "MPI-ESM1-2-HR": climate_data_capnp.Climate.GCM.mpiEsm12Hr,
            "MRI-ESM2-0": climate_data_capnp.Climate.GCM.mriEsm20,
            "UKESM1-0-LL": climate_data_capnp.Climate.GCM.ukesm10Ll
        }
    return string_to_gcm.d.get(gcm_str, None)

#------------------------------------------------------------------------------

def gcm_to_info(gcm):
    if not hasattr(gcm_to_info, "d"):
        gcm_to_info.d = {
            climate_data_capnp.Climate.GCM.cccmaCanEsm2: {"id": "CCCma-CanESM2", "name": "CCCma-CanESM2", "description": ""},
            climate_data_capnp.Climate.GCM.ichecEcEarth: {"id": "ICHEC-EC-EARTH", "name": "ICHEC-EC-EARTH", "description": ""},
            climate_data_capnp.Climate.GCM.ipslIpslCm5AMr: {"id": "IPSL-IPSL-CM5A-MR", "name": "IPSL-IPSL-CM5A-MR", "description": ""},
            climate_data_capnp.Climate.GCM.mirocMiroc5: {"id": "MIROC-MIROC5", "name": "MIROC-MIROC5", "description": ""},
            climate_data_capnp.Climate.GCM.mpiMMpiEsmLr: {"id": "MPI-M-MPI-ESM-LR", "name": "MPI-M-MPI-ESM-LR", "description": ""},
            climate_data_capnp.Climate.GCM.gfdlEsm4: {"id": "GFDL-ESM4", "name": "GFDL-ESM4", "description": ""},
            climate_data_capnp.Climate.GCM.ipslCm6aLr: {"id": "IPSL-CM6A-LR", "name": "IPSL-CM6A-LR", "description": ""},
            climate_data_capnp.Climate.GCM.mpiEsm12Hr: {"id": "MPI-ESM1-2-HR", "name": "MPI-ESM1-2-HR", "description": ""},
            climate_data_capnp.Climate.GCM.mriEsm20: {"id": "MRI-ESM2-0", "name": "MRI-ESM2-0", "description": ""},
            climate_data_capnp.Climate.GCM.ukesm10Ll: {"id": "UKESM1-0-LL", "name": "UKESM1-0-LL", "description": ""}
        }
    return gcm_to_info.d.get(gcm.raw, None)

#------------------------------------------------------------------------------

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

#------------------------------------------------------------------------------

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

#------------------------------------------------------------------------------

def string_to_ensmem(ensmem_str):
    # split r<N>i<M>p<L>
    sr, sip = ensmem_str[1:].split("i")
    si, sp = sip.split("p")
    return {"r": int(sr), "i": int(si), "p": int(sp)}

#------------------------------------------------------------------------------

def ensmem_to_info(ensmem):
    # r<N>i<M>p<L>
    id = "r{r}i{i}p{p}".format(r=ensmem.r, i=ensmem.i, p=ensmem.p)
    description = "Realization: #{r}, Initialization: #{i}, Pertubation: #{p}".format(r=ensmem.r, i=ensmem.i, p=ensmem.p)
    return {"id": id, "name": id, "description": description}

#------------------------------------------------------------------------------

def date_to_info(d):
    iso = d.isoformat()[:10]
    return {"id": iso, "name": iso, "description": ""}

#------------------------------------------------------------------------------

def rcp_or_ssp_to_info_factory(type):
    fwd_map = {
        "RCP": climate_data_capnp.Climate.RCP.schema.enumerants,
        "SSP": climate_data_capnp.Climate.SSP.schema.enumerants
    }.get(type.upper(), None)

    if not fwd_map:
        return {"id": "", "name": "", "description": ""}

    rev_map = {v : k for k, v in fwd_map.items()}
    
    def to_info(rev_map, xxp):
        id = rev_map[int(xxp)]
        name = id[:3].upper() + id[3:]
        return {"id": id, "name": name, "description": ""}

    return lambda xxp: to_info(rev_map, xxp)

#------------------------------------------------------------------------------

def access_entries(which):
    if not hasattr(access_entries, "d"):
        access_entries.d = {
            "gcm": lambda e: e.gcm,
            "rcm": lambda e: e.rcm,
            "historical": lambda e: True,
            "rcp": lambda e: e.rcp,
            "ssp": lambda e: e.ssp,
            "ensMem": lambda e: e.ensMem,
            "version": lambda e: e.version,
            "start": lambda e: create_date(e.start),
            "end": lambda e: create_date(e.end),
            "co2": lambda e: e.co2,
            "picontrol": lambda e: e.picontrol
        }
    return access_entries.d.get(which, None)

#------------------------------------------------------------------------------

def create_entry_map(entries):
    entry_to_value = {}
    for e in entries:
        which = e.which()
        entry_to_value[which] = access_entries(which)(e)
    
    return entry_to_value

#------------------------------------------------------------------------------

def create_date(capnp_date):
    return date(capnp_date.year, capnp_date.month, capnp_date.day)

#------------------------------------------------------------------------------

def create_capnp_date(py_date):
    return {
        "year": py_date.year if py_date else 0,
        "month": py_date.month if py_date else 0,
        "day": py_date.day if py_date else 0
    }
    
#------------------------------------------------------------------------------

class Metadata_Info(climate_data_capnp.Climate.Metadata.Information.Server):

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
            "version": lambda v: {"id": "version", "name": "Version", "description": v}, 
            "start": lambda v: {"id": "start", "name": "Start", "description": create_date(v).isoformat()[:10]},
            "end": lambda v: {"id": "end", "name": "End", "description": create_date(v).isoformat()[:10]},
            "co2": lambda v: {"id": "co2", "name": "CO2", "description": str(v) + "ppm"},
            "picontrol": lambda v: {"id": "picontrol", "name": "piControl", "description": ""},
            "description": lambda v: {"id": "description", "name": "Description", "description": v}
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

#------------------------------------------------------------------------------
