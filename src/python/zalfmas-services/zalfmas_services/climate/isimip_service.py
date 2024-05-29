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

from pkgs.common import common
from pkgs.common import service as serv
from pkgs.climate import common_climate_data_capnp_impl as ccdi
from pkgs.climate import csv_file_based as csv_based

PATH_TO_CAPNP_SCHEMAS = PATH_TO_REPO / "capnproto_schemas"
abs_imports = [str(PATH_TO_CAPNP_SCHEMAS)]
reg_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "registry.capnp"), imports=abs_imports)
climate_data_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "climate.capnp"), imports=abs_imports)


def create_meta_plus_datasets(path_to_data_dir, interpolator, rowcol_to_latlon):
    gcms = ["GFDL-ESM4", "IPSL-CM6A-LR", "MPI-ESM1-2-HR", "MRI-ESM2-0", "UKESM1-0-LL"]
    ssps = ["ssp126", "ssp585"]

    datasets = []
    for gcm in os.listdir(path_to_data_dir):
        gcm_dir = path_to_data_dir + "/" + gcm
        if os.path.isdir(gcm_dir) and gcm in gcms:
            for scen in os.listdir(gcm_dir):
                scen_dir = gcm_dir + "/" + scen

                entries = [{"gcm": ccdi.string_to_gcm(gcm)}]
                if scen in ssps:
                    entries.append({"rcp": "rcp" + scen[-2:]})
                    entries.append({"ssp": scen[:-2]})
                else:  # either historical or picontrol
                    entries.append({scen: None})

                metadata = climate_data_capnp.Metadata.new_message(entries=entries)
                metadata.info = ccdi.MetadataInfo(metadata)
                datasets.append(climate_data_capnp.MetaPlusData.new_message(
                    meta=metadata,
                    data=csv_based.Dataset(metadata, scen_dir, interpolator, rowcol_to_latlon,
                                           row_col_pattern="row-{row}/col-{col}.csv.gz")
                ))
    return datasets


async def main(path_to_data, serve_bootstrap=False, host=None, port=None,
               reg_sturdy_ref=None, id=None, name="ISIMIP AgMIP Phase3", description=None, srt=None):
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
        "srt": srt
    }
    common.update_config(config, sys.argv, print_config=True, allow_new_keys=False)

    interpolator, rowcol_to_latlon = ccdi.create_lat_lon_interpolator_from_json_coords_file(
        config["path_to_data"] + "/" + "latlon-to-rowcol.json")
    meta_plus_data = create_meta_plus_datasets(config["path_to_data"], interpolator, rowcol_to_latlon)
    service = ccdi.Service(meta_plus_data, id=config["id"], name=config["name"], description=config["description"])

    await serv.init_and_run_service({"service": service}, config["host"], config["port"],
                                    serve_bootstrap=config["serve_bootstrap"],
                                    name_to_service_srs={"service": config["srt"]})


if __name__ == '__main__':
    asyncio.run(capnp.run(main("/beegfs/common/data/climate/isimip/AgMIP.input_csvs")))
