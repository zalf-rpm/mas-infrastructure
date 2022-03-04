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
import json
import os
from pathlib import Path
import sys
import time
import uuid

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
import common.common as common
import common.service as serv
import csv_file_based as csv_based

PATH_TO_CAPNP_SCHEMAS = PATH_TO_REPO / "capnproto_schemas"
abs_imports = [str(PATH_TO_CAPNP_SCHEMAS)]
reg_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "registry.capnp"), imports=abs_imports)
climate_data_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "climate_data.capnp"), imports=abs_imports)


#------------------------------------------------------------------------------

def create_meta_plus_datasets(path_to_data_dir, interpolator, rowcol_to_latlon, restorer):
    datasets = []
    metadata = climate_data_capnp.Metadata.new_message(
        entries = [
            {"historical": None},
            {"start": {"year": 1990, "month": 1, "day": 1}},
            {"end": {"year": 2019, "month": 12, "day": 31}}
        ]
    )
    metadata.info = ccdi.Metadata_Info(metadata)
    datasets.append(climate_data_capnp.MetaPlusData.new_message(
        meta=metadata, 
        data=csv_based.Dataset(metadata, path_to_data_dir, interpolator, rowcol_to_latlon, 
            header_map={"windspeed": "wind"},
            row_col_pattern="row-{row}/col-{col}.csv",
            name="DWD Germany 1991-2019",
            description="ZALF DWD Germany data from 1991-2019 in MONICA CSV format.",
            restorer=restorer)
    ))
    return datasets

#------------------------------------------------------------------------------

def main(serve_bootstrap=True, host="*", port=None, reg_sturdy_ref=None,
    id=None, name="DWD - historical - 1991-2019", description=None):

    config = {
        "port": port,
        "host": host,
        "id": id,
        "name": name,
        "description": description,
        "serve_bootstrap": str(serve_bootstrap)
    }
    # read commandline args only if script is invoked directly from commandline
    if len(sys.argv) > 1 and __name__ == "__main__":
        for arg in sys.argv[1:]:
            k, v = arg.split("=")
            if k in config:
                config[k] = v
    print(config)

    # check for sturdy ref inputs
    if not sys.stdin.isatty():
        try:
            reg_config = json.loads(sys.stdin.read())
        except:
            pass
    else:
        reg_config = {}

    conMan = common.ConnectionManager()

    restorer = common.Restorer()

    interpolator, rowcol_to_latlon = ccdi.create_lat_lon_interpolator_from_json_coords_file(config["path_to_data"] + "/" + "latlon-to-rowcol.json")
    meta_plus_data = create_meta_plus_datasets(config["path_to_data"] + "/germany", interpolator, rowcol_to_latlon, restorer)
    service = ccdi.Service(meta_plus_data, id=config["id"], name=config["name"], description=config["description"])
    admin = serv.Admin(service)
    service.admin = admin
    
    for name, data in reg_config:
        try:
            reg_sr = data["reg_sr"]
            reg_name = data["reg_name"]
            reg_cat_id = data["cat_id"]
            registrar = conMan.try_connect(reg_sr, cast_as=reg_capnp.Registrar)
            if registrar:
                r = registrar.register(ref=service, regName=reg_name, categoryId=reg_cat_id).wait()
                unreg_action = r.unreg
                rereg_sr = r.reregSR
                admin.store_unreg_data(name, unreg_action, rereg_sr)
                print("Registered", name, "in category", reg_cat_id, "as", reg_name, ".")
            else:
                print("Couldn't connect to registrar at sturdy_ref:", reg_sr)
        except:
            pass

    addr = config["host"] + ((":" + str(config["port"])) if config["port"] else "")
    if config["serve_bootstrap"].lower() == "true":
        server = capnp.TwoPartyServer(addr, bootstrap=restorer)
        restorer.port = port if port else server.port
        admin_sr = restorer.save(admin)
        service_sr = restorer.save(service)
        print("admin_sr:", admin_sr)
        print("service_sr:", service_sr)
        print("restorer_sr:", restorer.sturdy_ref())
    else:
        capnp.wait_forever()
    server.run_forever()

#------------------------------------------------------------------------------

async def async_main(path_to_data, serve_bootstrap=True, host=None, port=10000, 
reg_sturdy_ref=None, id=None, name="DWD - historical - 1991-2019", description=None):

    config = {
        "path_to_data": path_to_data,
        "host": host,
        "port": port,
        "id": id,
        "name": name,
        "description": description,
        "serve_bootstrap": str(serve_bootstrap)
    }
    # read commandline args only if script is invoked directly from commandline
    if len(sys.argv) > 1 and __name__ == "__main__":
        for arg in sys.argv[1:]:
            k, v = arg.split("=")
            if k in config:
                config[k] = v
    print("config used:", config)

    # check for sturdy ref inputs
    if not sys.stdin.isatty():
        try:
            reg_config = json.loads(sys.stdin.read())
        except:
            pass
    else:
        reg_config = {}

    conMan = async_helpers.ConnectionManager()

    restorer = common.Restorer()
    interpolator, rowcol_to_latlon = ccdi.create_lat_lon_interpolator_from_json_coords_file(config["path_to_data"] + "/" + "latlon-to-rowcol.json")
    meta_plus_data = create_meta_plus_datasets(config["path_to_data"] + "/germany", interpolator, rowcol_to_latlon, restorer)
    service = ccdi.Service(meta_plus_data, id=config["id"], name=config["name"], description=config["description"])
    admin = serv.Admin(service)
    service.admin = admin

    for name, data in reg_config:
        try:
            reg_sr = data["reg_sr"]
            reg_name = data["reg_name"]
            reg_cat_id = data["cat_id"]
            registrar = await conMan.try_connect(reg_sr, cast_as=reg_capnp.Registrar)
            if registrar:
                r = await registrar.register(ref=service, regName=reg_name, categoryId=reg_cat_id).a_wait()
                unreg_action = r.unreg
                rereg_sr = r.reregSR
                admin.store_unreg_data(name, unreg_action, rereg_sr)
                print("Registered", name, "in category", reg_cat_id, "as", reg_name, ".")
            else:
                print("Couldn't connect to registrar at sturdy_ref:", reg_sr)
        except:
            pass

    if config["serve_bootstrap"].lower() == "true":
        server = await async_helpers.serve(config["host"], config["port"], restorer)

        admin_sr = restorer.save(admin)
        service_sr = restorer.save(service)
        print("admin_sr:", admin_sr[0])
        print("service_sr:", service_sr[0])
        print("restorer_sr:", restorer.sturdy_ref()[0])

        async with server:
            await server.serve_forever()
    else:
        await conMan.manage_forever()

#------------------------------------------------------------------------------

if __name__ == '__main__':
    #asyncio.run(async_main("/beegfs/common/data/climate/dwd/csvs"))
    asyncio.run(async_main("/run/user/1000/gvfs/sftp:host=localhost,port=2222,user=rpm/beegfs/common/data/climate/dwd/csvs"))