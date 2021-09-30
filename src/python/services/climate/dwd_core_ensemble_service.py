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

PATH_TO_CAPNP_SCHEMAS = PATH_TO_REPO / "capnproto_schemas"
abs_imports = [str(PATH_TO_CAPNP_SCHEMAS)]
reg_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "registry.capnp"), imports=abs_imports)
climate_data_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "climate_data.capnp"), imports=abs_imports)

#------------------------------------------------------------------------------

def create_meta_plus_datasets(path_to_data_dir, interpolator, rowcol_to_latlon):
    datasets = []
    for folder in os.listdir(path_to_data_dir):
        print(folder)
        gcm, rcm, scen, ensmem, version = folder.split("_")
        metadata = climate_data_capnp.Metadata.new_message(
            entries = [
                {"gcm": ccdi.string_to_gcm(gcm)},
                {"rcm": ccdi.string_to_rcm(rcm)},
                {"historical": None} if scen == "historical" else {"rcp": scen},
                {"ensMem": ccdi.string_to_ensmem(ensmem)},
                {"version": version}
            ]
        )
        metadata.info = ccdi.Metadata_Info(metadata)
        datasets.append(climate_data_capnp.MetaPlusData.new_message(
            meta=metadata, 
            data=csv_based.Dataset(metadata, folder, interpolator, rowcol_to_latlon, 
            row_col_pattern="row-{row}/col-{col}.csv")
        ))
    return datasets

#------------------------------------------------------------------------------

async def async_main(path_to_data, serve_bootstrap=False,
host="0.0.0.0", port=None, reg_sturdy_ref=None, id=None, name="DWD Core Ensemble", description=None):

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
    meta_plus_data = create_meta_plus_datasets(config["path_to_data"] + "/csvs_all", interpolator, rowcol_to_latlon)
    service = ccdi.Service(meta_plus_data, id=config["id"], name=config["name"], description=config["description"])

    if config["reg_sturdy_ref"]:
        registrator = await conMan.try_connect(config["reg_sturdy_ref"], cast_as=reg_capnp.Registrator)
        if registrator:
            unreg = await registrator.register(ref=service, categoryId=config["reg_category"]).a_wait()
            print("Registered ", config["name"], "climate service.")
            #await unreg.unregister.unregister().a_wait()
        else:
            print("Couldn't connect to registrator at sturdy_ref:", config["reg_sturdy_ref"])

    if config["serve_bootstrap"].lower() == "true":
        await async_helpers.serve_forever(config["host"], config["port"], service)
    else:
        await conMan.manage_forever()

#------------------------------------------------------------------------------

if __name__ == '__main__':
    asyncio.run(async_main("/beegfs/common/data/climate/dwd_core_ensemble"))
