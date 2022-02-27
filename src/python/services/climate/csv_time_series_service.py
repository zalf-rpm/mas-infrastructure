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
#from datetime import date, timedelta
#import json
import os
from pathlib import Path
import sys
import time

PATH_TO_REPO = Path(os.path.realpath(__file__)).parent.parent.parent.parent.parent
if str(PATH_TO_REPO) not in sys.path:
    sys.path.insert(1, str(PATH_TO_REPO))

PATH_TO_PYTHON_CODE = PATH_TO_REPO / "src/python"
if str(PATH_TO_PYTHON_CODE) not in sys.path:
    sys.path.insert(1, str(PATH_TO_PYTHON_CODE))

import common.capnp_async_helpers as async_helpers
import common.common as common
import services.climate.csv_file_based as csv_based

PATH_TO_CAPNP_SCHEMAS = PATH_TO_REPO / "capnproto_schemas"
abs_imports = [str(PATH_TO_CAPNP_SCHEMAS)]
reg_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "registry.capnp"), imports=abs_imports)
#climate_data_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "climate_data.capnp"), imports=abs_imports)

#------------------------------------------------------------------------------

def main(path_to_csv_file, serve_bootstrap=False, host="*", port=None, reg_sturdy_ref=None):

    config = {
        "port": port,
        "host": host,
        "path_to_csv_file": path_to_csv_file,
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
    print(config)

    conMan = common.ConnectionManager()

    service = csv_based.TimeSeries.from_csv_file(config["path_to_csv_file"], \
        header_map={}, #{"windspeed": "wind"}, 
        pandas_csv_config={} #{"sep": ";"}
    )

    if config["reg_sturdy_ref"]:
        registrator = conMan.try_connect(config["reg_sturdy_ref"], cast_as=reg_capnp.Registrator)
        if registrator:
            unreg = registrator.register(ref=service, categoryId=config["reg_category"]).wait()
            print("Registered ", config["name"], "climate service.")
            #await unreg.unregister.unregister().a_wait()
        else:
            print("Couldn't connect to registrator at sturdy_ref:", config["reg_sturdy_ref"])

    addr = config["host"] + (":" + str(config["port"])) if config["port"] else ""
    if config["serve_bootstrap"].lower() == "true":
        server = capnp.TwoPartyServer(addr, bootstrap=service)
    else:
        capnp.wait_forever()
    server.run_forever()

#------------------------------------------------------------------------------

async def async_main(path_to_csv_file, serve_bootstrap=False, host="0.0.0.0", port=None, reg_sturdy_ref=None):
    config = {
        "path_to_csv_file": path_to_csv_file,
        "host": host,
        "port": port,
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

    service = csv_based.TimeSeries.from_csv_file(config["path_to_csv_file"], 
                                                header_map={},#{"windspeed": "wind"},
                                                pandas_csv_config={})#{"sep": ";"}))

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
    main("data/climate/climate-iso.csv", serve_bootstrap=False, port=11002)
    #asyncio.run(async_main("data/climate/climate-iso.csv", serve_bootstrap=True, port=11002))

