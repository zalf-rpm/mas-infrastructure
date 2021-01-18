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
import time

#remote debugging via commandline
#-m ptvsd --host 0.0.0.0 --port 14000 --wait

PATH_TO_REPO = Path(os.path.realpath(__file__)).parent.parent.parent.parent.parent
if str(PATH_TO_REPO) not in sys.path:
    sys.path.insert(1, str(PATH_TO_REPO))

PATH_TO_PYTHON_CODE = PATH_TO_REPO / "src/python"
if str(PATH_TO_PYTHON_CODE) not in sys.path:
    sys.path.insert(1, str(PATH_TO_PYTHON_CODE))

import common.capnp_async_helpers as async_helpers
import common_climate_data_capnp_impl as ccdi
import csv_file_based as csv_based

abs_imports = ["capnproto_schemas"]
reg_capnp = capnp.load("capnproto_schemas/registry.capnp", imports=abs_imports)
climate_data_capnp = capnp.load("capnproto_schemas/climate_data.capnp", imports=abs_imports)

#------------------------------------------------------------------------------

def create_meta_plus_datasets(path_to_data_dir, interpolator, rowcol_to_latlon):
    datasets = []
    metadata = climate_data_capnp.Climate.Metadata.new_message(
        entries = [
            {"historical": None},
            {"start": {"year": 1901, "month": 1, "day": 1}},
            {"end": {"year": 2019, "month": 12, "day": 31}}
        ]
    )
    metadata.info = ccdi.Metadata_Info(metadata)
    datasets.append(climate_data_capnp.Climate.MetaPlusData.new_message(
        meta=metadata, 
        data=csv_based.Dataset(metadata, path_to_data_dir, interpolator, rowcol_to_latlon, 
            header_map={
                "Date": "iso-date",
                "Precipitation": "precip",
                "TempMin": "tmin",
                "TempMean": "tavg",
                "TempMax": "tmax",
                "Radiation": "globrad",
                "Windspeed": "wind",
                "RelHumCalc": "relhumid"
            },
            supported_headers=["tmin", "tavg", "tmax", "precip", "globrad", "wind", "relhumid"],
            row_col_pattern="{row}/daily_mean_RES1_C{col}R{row}.csv.gz",
            pandas_csv_config={"skip_rows": 0, "sep": "\t"},
            transform_map={
                "relhumid": lambda rh: rh * 100.0,
                "globrad": lambda gr: gr / 1000.0 if gr > 0 else gr
            })
    ))
    return datasets

#------------------------------------------------------------------------------

"""
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
    meta_plus_data = create_meta_plus_datasets(config["path_to_data"] + "/germany_ubn_1901-2018", interpolator, rowcol_to_latlon)
    service = ccdi.Service(meta_plus_data, name="DWD/UBN - historical - 1901 - ...")

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

    registry = client.bootstrap().cast_as(service_registry_capnp.Registry)
    unreg = await registry.registerService(type="climate", service=service).a_wait()
    #await unreg.unregister.unregister().a_wait()

    print("Registered a DWD/UBN - historical climate service.")

    #await unreg.unregister.unregister().a_wait()

    await gather_results

    print("after gather_results")
"""

#------------------------------------------------------------------------------

async def async_main(path_to_data, serve_bootstrap=False,
host="0.0.0.0", port=None, reg_sturdy_ref=None, id=None, name="DWD/UBN - historical - 1901 - ...", description=None):

    config = {
        "path_to_data": path_to_data,
        "host": host,
        "port": port,
        "id": id,
        "name": name,
        "description": description,
        "reg_sturdy_ref": reg_sturdy_ref,
        "serve_bootstrap": str(serve_bootstrap),
        "reg_category": "climate",
    }
    # read commandline args only if script is invoked directly from commandline
    if len(sys.argv) > 1 and __name__ == "__main__":
        for arg in sys.argv[1:]:
            k, v = arg.split("=")
            if k in config:
                config[k] = v
    print("config used:", config)

    conMan = async_helpers.ConnectionManager()

    interpolator, rowcol_to_latlon = ccdi.create_lat_lon_interpolator_from_json_coords_file(config["path_to_data"] + "/" + "latlon-to-rowcol.json")
    meta_plus_data = create_meta_plus_datasets(config["path_to_data"] + "/germany_ubn_1901-2018", interpolator, rowcol_to_latlon)
    service = ccdi.Service(meta_plus_data, id=config["id"], name=config["name"], description=config["description"])

    if config["reg_sturdy_ref"]:
        registrator = await conMan.try_connect(config["reg_sturdy_ref"], cast_as=reg_capnp.Registrator)
        if registrator:
            unreg = await registrator.register(ref=service, categoryId=config["reg_category"]).a_wait()
            print("Registered ", config["name"], "climate service.")
            #await unreg.unregister.unregister().a_wait()
        else:
            print("Couldn't connect to registrator at sturdy_ref:", config["reg_sturdy_ref"])

    if config["serve_bootstrap"].upper() == "TRUE":
        await async_helpers.serve_forever(config["host"], config["port"], service)
    else:
        await conMan.manage_forever()

#------------------------------------------------------------------------------

if __name__ == '__main__':
    asyncio.run(async_main("/beegfs/common/data/climate/dwd/csvs"))