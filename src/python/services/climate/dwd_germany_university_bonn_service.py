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

# remote debugging via commandline
# -m ptvsd --host 0.0.0.0 --port 14000 --wait

PATH_TO_REPO = Path(os.path.realpath(__file__)).parent.parent.parent.parent.parent
if str(PATH_TO_REPO) not in sys.path:
    sys.path.insert(1, str(PATH_TO_REPO))

PATH_TO_PYTHON_CODE = PATH_TO_REPO / "src/python"
if str(PATH_TO_PYTHON_CODE) not in sys.path:
    sys.path.insert(1, str(PATH_TO_PYTHON_CODE))

import common.capnp_async_helpers as async_helpers
import lib.climate.common_climate_data_capnp_impl as ccdi
import lib.climate.csv_file_based as csv_based
import common.common as common
import lib.common.service as serv

PATH_TO_CAPNP_SCHEMAS = PATH_TO_REPO / "capnproto_schemas"
abs_imports = [str(PATH_TO_CAPNP_SCHEMAS)]
reg_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "registry.capnp"), imports=abs_imports)
climate_data_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "climate.capnp"), imports=abs_imports)


def create_meta_plus_datasets(path_to_data_dir, interpolator, rowcol_to_latlon, restorer):
    datasets = []
    metadata = climate_data_capnp.Metadata.new_message(
        entries=[
            {"historical": None},
            {"start": {"year": 1901, "month": 1, "day": 1}},
            {"end": {"year": 2019, "month": 12, "day": 31}}
        ]
    )
    metadata.info = ccdi.Metadata_Info(metadata)
    transform_map = {
        "globrad": lambda gr: gr / 1000.0 if gr > 0 else gr
    }
    if "germany_ubn_1901-2018" in path_to_data_dir:
        transform_map["relhumid"] = lambda rh: rh * 100.0

    datasets.append(climate_data_capnp.MetaPlusData.new_message(
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
                               transform_map=transform_map,
                               restorer=restorer)
    ))
    return datasets


async def main(path_to_data, bonn_data_subdir, serve_bootstrap=True, host=None, port=None,
               id=None, name="DWD/UBN - historical - 1901 - ...", description=None,
               reg_sturdy_ref=None, use_async=False):

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
        "use_async": use_async,
    }
    common.update_config(config, sys.argv, print_config=True, allow_new_keys=False)

    con_man = async_helpers.ConnectionManager()
    restorer = common.Restorer()
    interpolator, rowcol_to_latlon = ccdi.create_lat_lon_interpolator_from_json_coords_file(
        config["path_to_data"] + "latlon-to-rowcol.json")
    meta_plus_data = create_meta_plus_datasets(config["path_to_data"] + bonn_data_subdir, interpolator,
                                               rowcol_to_latlon, restorer)
    service = ccdi.Service(meta_plus_data, id=config["id"], name=config["name"], description=config["description"],
                           restorer=restorer)

    if config["reg_sturdy_ref"]:
        registrator = await con_man.try_connect(config["reg_sturdy_ref"], cast_as=reg_capnp.Registrator)
        if registrator:
            unreg = await registrator.register(ref=service, categoryId=config["reg_category"]).a_wait()
            print("Registered ", config["name"], "climate service.")
            # await unreg.unregister.unregister().a_wait()
        else:
            print("Couldn't connect to registrator at sturdy_ref:", config["reg_sturdy_ref"])

    if config["use_async"]:
        await serv.async_init_and_run_service({"service": service}, config["host"], config["port"],
                                              serve_bootstrap=config["serve_bootstrap"], restorer=restorer,
                                              conn_man=con_man)
    else:

        serv.init_and_run_service({"service": service}, config["host"], config["port"],
                                  serve_bootstrap=config["serve_bootstrap"], restorer=restorer, conn_man=con_man)


if __name__ == '__main__':
    #asyncio.run(main("/beegfs/common/data/climate/dwd/csvs/", "germany_ubn_1901-2018",
    #                 serve_bootstrap=True, use_async=True))
    asyncio.run(
        main("/run/user/1000/gvfs/sftp:host=login01.cluster.zalf.de,user=rpm/beegfs/common/data/climate/dwd/csvs/",
             "germany_ubn_1991-2022",
             serve_bootstrap=True, use_async=True))
