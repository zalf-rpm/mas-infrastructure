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
import gzip
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

import common.common as cc
import common.geo as geo
import common.capnp_async_helpers as async_helpers
import common_climate_data_capnp_impl as ccdi
import csv_file_based as csv_based

import capnp
#import capnproto_schemas.common_capnp as common_capnp
import capnproto_schemas.geo_coord_capnp as geo_capnp
import capnproto_schemas.climate_data_capnp as climate_data_capnp
import capnproto_schemas.service_registry_capnp as reg_capnp

#------------------------------------------------------------------------------

def create_meta_plus_datasets(path_to_data_dir, interpolator, rowcol_to_latlon):
    gcms = ["GFDL-ESM4", "IPSL-CM6A-LR", "MPI-ESM1-2-HR", "MRI-ESM2-0", "UKESM1-0-LL"]
    ssps = ["ssp126", "ssp585"]

    datasets = []
    for gcm in os.listdir(path_to_data_dir):
        gcm_dir = path_to_data_dir + gcm
        if os.path.isdir(gcm_dir) and gcm in gcms:
            for scen in os.listdir(gcm_dir):
                scen_dir = gcm_dir + "/" + scen

                entries = [{"gcm": ccdi.string_to_gcm(gcm)}]
                if scen in ssps: 
                    entries.append({"rcp": "rcp" + scen[-2:]})
                    entries.append({"ssp": scen[:-2]})
                else:
                    entries.append({scen: None})

                metadata = climate_data_capnp.Climate.Metadata.new_message(entries=entries)
                metadata.info = Metadata_Info(metadata)
                datasets.append(climate_data_capnp.Climate.MetaPlusData.new_message(
                    meta=metadata, 
                    data=csv_based.Dataset(metadata, scen_dir, interpolator, rowcol_to_latlon)
                ))
    return datasets

#------------------------------------------------------------------------------

class Service(climate_data_capnp.Climate.Service.Server):

    def __init__(self, path_to_data_dir, interpolator, rowcol_to_latlon, id=None, name=None, description=None):
        self._id = id if id else "isimip_agmip_phase3"
        self._name = name if name else "ISIMIP AgMIP Phase3"
        self._description = description if description else ""
        self._meta_plus_datasets = create_meta_plus_datasets(path_to_data_dir, interpolator, rowcol_to_latlon)

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
        search_entry_to_value = ccdi.create_entry_map(template.entries)

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

    interpolator, rowcol_to_latlon = ccdi.create_lat_lon_interpolator_from_json_coords_file(config["path_to_data"] + "/" + "latlon-to-rowcol.json")
    service = Service(config["path_to_data"] + "/csv/", interpolator, rowcol_to_latlon)

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
    unreg = await registry.registerService(type="climate", service=service).a_wait()
    #await unreg.unregister.unregister().a_wait()

    print("Registered a ISIMIP climate service.")

    #await unreg.unregister.unregister().a_wait()

    await gather_results

    print("after gather_results")

#------------------------------------------------------------------------------

def sync_main_server(path_to_data, port):
    config = {
        "path_to_data": path_to_data,
        "port": str(port),
        "server": "*"
    }
    # read commandline args only if script is invoked directly from commandline
    if len(sys.argv) > 1 and __name__ == "__main__":
        for arg in sys.argv[1:]:
            k, v = arg.split("=")
            if k in config:
                config[k] = v
    print("config used:", config)

    interpolator, rowcol_to_latlon = ccdi.create_lat_lon_interpolator_from_json_coords_file(config["path_to_data"] + "/" + "latlon-to-rowcol.json")
    server = capnp.TwoPartyServer(config["server"] + ":" + config["port"], bootstrap=Service(config["path_to_data"] + "/csv/", interpolator, rowcol_to_latlon))
    server.run_forever()

#------------------------------------------------------------------------------

if __name__ == '__main__':
    path_to_data = "/beegfs/common/data/climate/isimip/AgMIP.input_csvs"

    if len(sys.argv) > 1:
        command = sys.argv[1]
        if command == "sync_server":
            sys.argv.pop(1)
            sync_main_server(path_to_data, 12000)
        elif command == "async_register":
            sys.argv.pop(1)
            asyncio.run(async_main_register(path_to_data, reg_server="login01.cluster.zalf.de", reg_port=11001))

    print("sync_server | async_register")