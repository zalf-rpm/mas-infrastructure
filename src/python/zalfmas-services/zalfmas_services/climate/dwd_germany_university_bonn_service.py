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

from zalfmas_common.climate import common_climate_data_capnp_impl as ccdi
from zalfmas_common import common
from zalfmas_common import service as serv
from zalfmas_common.climate import csv_file_based as csv_based
from zalfmas_common import fbp
import zalfmas_capnpschemas

sys.path.append(os.path.dirname(zalfmas_capnpschemas.__file__))
import climate_capnp
import registry_capnp as reg_capnp


def create_meta_plus_datasets(path_to_data_dir, interpolator, rowcol_to_latlon, restorer):
    datasets = []
    metadata = climate_capnp.Metadata.new_message(
        entries=[
            {"historical": None},
            {"start": {"year": 1901, "month": 1, "day": 1}},
            {"end": {"year": 2022, "month": 9, "day": 30}}
        ]
    )
    metadata.info = ccdi.MetadataInfo(metadata)
    transform_map = {
        "globrad": lambda gr: gr / 1000.0 if gr > 0 else gr
    }
    if "germany_ubn_1901-01-01_to_2022-09-30" in path_to_data_dir:
        transform_map["relhumid"] = lambda rh: rh * 100.0

    datasets.append(climate_capnp.MetaPlusData.new_message(
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
               reg_sturdy_ref=None, srt=None):

    config = {
        "path_to_data": path_to_data,
        "bonn_data_subdir": bonn_data_subdir,
        "host": host,
        "port": port,
        "id": id,
        "name": name,
        "description": description,
        "reg_sturdy_ref": reg_sturdy_ref,
        "serve_bootstrap": str(serve_bootstrap),
        "reg_category": "climate",
        "srt": srt
    }
    common.update_config(config, sys.argv, print_config=True, allow_new_keys=False)

    restorer = common.Restorer()
    interpolator, rowcol_to_latlon = ccdi.create_lat_lon_interpolator_from_json_coords_file(
        config["path_to_data"] + "/" + "latlon-to-rowcol.json")
    meta_plus_data = create_meta_plus_datasets(config["path_to_data"] + "/" + config["bonn_data_subdir"], interpolator,
                                               rowcol_to_latlon, restorer)
    service = ccdi.Service(meta_plus_data, id=config["id"], name=config["name"], description=config["description"],
                           restorer=restorer)
    await serv.init_and_run_service({"service": service}, config["host"], config["port"],
                                    serve_bootstrap=config["serve_bootstrap"],
                                    name_to_service_srs={"service": config["srt"]},
                                    restorer=restorer)


if __name__ == '__main__':
    asyncio.run(capnp.run(main(
        "/run/user/1000/gvfs/sftp:host=login01.cluster.zalf.de,user=rpm/beegfs/common/data/climate/dwd/csvs/",
        "germany_ubn_1901-01-01_to_2022-09-30",
        serve_bootstrap=True
        )))
