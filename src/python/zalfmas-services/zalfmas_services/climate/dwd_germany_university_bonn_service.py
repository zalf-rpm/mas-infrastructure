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
import click
import os
from pathlib import Path
import sys
import tomlkit as tk
import uuid

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


async def main(config_toml: str = None, **kwargs):
    config = tk.loads(config_toml)

    path_to_data = Path(kwargs.get("path_to_data", config["service"]["path_to_data"]))
    path_to_latlon_to_rowcol = Path(kwargs.get("path_to_latlon_to_rowcol", config["service"]["path_to_latlon_to_rowcol"]))

    restorer = common.Restorer()
    interpolator, rowcol_to_latlon = ccdi.create_lat_lon_interpolator_from_json_coords_file(
        path_to_data / path_to_latlon_to_rowcol)
    meta_plus_data = create_meta_plus_datasets(str(path_to_data), interpolator, rowcol_to_latlon, restorer)
    service = ccdi.Service(meta_plus_data,
                           id=kwargs.get("id", config["service"]["id"]),
                           name=kwargs.get("name", config["service"]["name"]),
                           description=kwargs.get("description", config["service"]["description"]),
                           restorer=restorer)
    await serv.init_and_run_service({"service": service},
                                    kwargs.get("host", config["network"]["host"]),
                                    kwargs.get("port", config["network"]["port"]),
                                    serve_bootstrap=kwargs.get("serve_bootstrap", config["network"]["serve_bootstrap"]),
                                    name_to_service_srs={"service": kwargs.get("fixed_sturdy_ref_token", config["capabilities"]["fixed_sturdy_ref_token"])},
                                    restorer=restorer)


CONTEXT_SETTINGS = dict(help_option_names=['-h', '--help'])

@click.command(context_settings=CONTEXT_SETTINGS)
@click.version_option(version='0.1.0')
@click.argument("config_toml", type=click.Path(), required=False)
@click.option("--id", help="ID of the service")
@click.option("--name", default="DWD/UBN - historical - 1901 - 2023", help="Name of the service")
@click.option("--description", help="Description of the service")
@click.option("--path_to_data", help="Path to the directory containing the data (rows)")
@click.option("--path_to_latlon_to_rowcol", help="Path to the JSON file containing the lat/lon to row/coll mapping")
@click.option("--host", type=str, default=None, help="Use this host (e.g. localhost)")
@click.option("--port", type=int, default=None, help="Use this port (default = choose random free port)")
@click.option("--serve_bootstrap", is_flag=True, default=True, help="Is the service reachable directly via its restorer interface")
@click.option("--fixed_sturdy_ref_token", help="Use this token as the sturdy ref token of this service")
@click.option("--reg_sturdy_ref", help="Connect to registry using this sturdy ref")
@click.option("--reg_category", default="climate", help="Connect to registry using this category")
@click.option("--create_config_template", help="Create a config.toml template file in the current directory")
def cmd(config_toml, **kwargs):

    if config_toml:
        config_toml_str = config_toml.read()
    elif kwargs.get("create_config_template"):
        config = common.create_service_toml_config(**kwargs)
        config["service"]["path_to_data"] = "/path/to/data"
        config["service"]["path_to_latlon_to_rowcol"] = "/path/to/latlon-to-rowcol.json"
        with open(kwargs["create_config_template"], "w") as f:
            tk.dump(config, f)
        return
    else:
        config_toml_str = ""

    asyncio.run(capnp.run(main(config_toml_str, **kwargs)))
    #    "/run/user/1000/gvfs/sftp:host=login01.cluster.zalf.de,user=rpm/beegfs/common/data/climate/dwd/csvs/",
    #    "germany_ubn_1901-01-01_to_2022-09-30",
    #    serve_bootstrap=True
    #)))

if __name__ == '__main__':
    cmd()
