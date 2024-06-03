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
import tomli

from zalfmas_common.climate import common_climate_data_capnp_impl as ccdi
from zalfmas_common import common
from zalfmas_common import service as serv
from zalfmas_common.climate import csv_file_based as csv_based
from zalfmas_common import fbp
from zalfmas_common import geo
from zalfmas_common import rect_ascii_grid_management as grid_man
import zalfmas_capnpschemas

sys.path.append(os.path.dirname(zalfmas_capnpschemas.__file__))
import climate_capnp
import registry_capnp as reg_capnp


def create_meta_plus_datasets(path_to_config, config, interpolator, rowcol_to_latlon, restorer):
    general = config["general"]
    conf_datasets = config["datasets"]

    datasets = []
    for ds in conf_datasets:
        path_to_rowcols = str(
            path_to_config / general["path_to_rowcols"].format(gcm=ds["gcm"], rcm=ds["rcm"], scen=ds["scen"],
                                                               ensmem=ds["ensmem"], version=ds["version"]))
        metadata = climate_capnp.Metadata.new_message(
            entries=[
                {"gcm": ccdi.string_to_gcm(ds["gcm"])},
                {"rcm": ccdi.string_to_rcm(ds["rcm"])},
                {"historical": None} if ds["scen"] == "historical" else {"rcp": ds["scen"]},
                {"ensMem": ccdi.string_to_ensmem(ds["ensmem"])},
                {"version": ds["version"]},
                {"start": ccdi.create_capnp_date(ds["start"])},
                {"end": ccdi.create_capnp_date(ds["end"])},
            ]
        )
        name = "{}_{}_{}_{}_{}".format(ds["gcm"], ds["rcm"], ds["scen"], ds["ensmem"], ds["version"])
        metadata.info = ccdi.MetadataInfo(metadata)
        datasets.append(climate_capnp.MetaPlusData.new_message(
            meta=metadata,
            data=csv_based.Dataset(metadata, path_to_rowcols, interpolator, rowcol_to_latlon,
                                   row_col_pattern=general["row_col_pattern"],
                                   restorer=restorer,
                                   name=name
                                   )
        ))
    return datasets


async def main(path_to_config, serve_bootstrap=True, host=None, port=None,
               id=None, name="DWD Core Ensemble", description=None, reg_sturdy_ref=None, srt=None):
    config = {
        "path_to_config": path_to_config,
        "config_toml_file": "metadata.toml",
        "port": port,
        "host": host,
        "id": id,
        "name": name,
        "description": description,
        "serve_bootstrap": serve_bootstrap,
        "reg_sturdy_ref": reg_sturdy_ref,
        "reg_category": "climate",
        "srt": srt
    }
    common.update_config(config, sys.argv, print_config=True, allow_new_keys=False)

    path_to_config = Path(config["path_to_config"])
    with open(path_to_config / config["config_toml_file"], "rb") as f:
        datasets_config = tomli.load(f)

    if not datasets_config:
        print("Couldn't load datasets configuration from:", str(path_to_config / config["config_toml_file"]))
        exit(1)

    general = datasets_config["general"]

    restorer = common.Restorer()
    interpolator, rowcol_to_latlon = ccdi.create_lat_lon_interpolator_from_json_coords_file(
        path_to_config / general["latlon_to_rowcol_mapping"])
    meta_plus_data = create_meta_plus_datasets(path_to_config, datasets_config, interpolator, rowcol_to_latlon,
                                               restorer)
    service = ccdi.Service(meta_plus_data, id=config["id"], name=config["name"], description=config["description"],
                           restorer=restorer)
    await serv.init_and_run_service({"service": service}, config["host"], config["port"],
                                    serve_bootstrap=config["serve_bootstrap"],
                                    name_to_service_srs={"service": config["srt"]},
                                    restorer=restorer)


if __name__ == '__main__':
    asyncio.run(capnp.run(main(
        "/run/user/1000/gvfs/sftp:host=login01.cluster.zalf.de,user=rpm/beegfs/common/data/climate/dwd_core_ensemble",
        serve_bootstrap=True)))
