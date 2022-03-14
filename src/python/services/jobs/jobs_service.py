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

PATH_TO_REPO = Path(os.path.realpath(__file__)).parent.parent.parent.parent.parent
if str(PATH_TO_REPO) not in sys.path:
    sys.path.insert(1, str(PATH_TO_REPO))

PATH_TO_PYTHON_CODE = PATH_TO_REPO / "src/python"
if str(PATH_TO_PYTHON_CODE) not in sys.path:
    sys.path.insert(1, str(PATH_TO_PYTHON_CODE))

import common.capnp_async_helpers as async_helpers
import common.common as common
import common.service as serv
import common.csv as csv

PATH_TO_CAPNP_SCHEMAS = PATH_TO_REPO / "capnproto_schemas"
abs_imports = [str(PATH_TO_CAPNP_SCHEMAS)]
reg_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "registry.capnp"), imports=abs_imports)
jobs_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "jobs.capnp"), imports=abs_imports)

class Service(jobs_capnp.Service.Server, common.Identifiable, serv.AdministrableService): 

    def __init__(self, jobs = []):
        self._jobs = [{"data": data} for data in jobs]


    def nextJob(self, **kwargs): # nextJob @0 () -> (job :Job);
        print(len(self._jobs))
        try:
            return self._jobs.pop()
        except:
            return {"noFurtherJobs": True}

#------------------------------------------------------------------------------

async def main(use_async, path_to_csv, serve_bootstrap=True, host=None, port=None, id=None, name="Jobs Service", description=None):

    config = {
        "path_to_csv": path_to_csv,
        "id_col_name": "id",
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

    jobs = csv.read_csv(config["path_to_csv"], config["id_col_name"])
    jobs2 = [json.dumps(v) for k, v in jobs.items()]
    service = Service(jobs2)
    if use_async:
        await serv.async_init_and_run_service({"service": service}, config["host"], config["port"], 
        serve_bootstrap=config["serve_bootstrap"])
    else:
        
        serv.init_and_run_service({"service": service}, config["host"], config["port"], 
            serve_bootstrap=config["serve_bootstrap"])

#------------------------------------------------------------------------------

if __name__ == '__main__':
    asyncio.run(main(False, "/home/berg/Desktop/Koordinaten_HE_dummy_ID.csv")) 
    asyncio.run(main(True, "/home/berg/Desktop/Koordinaten_HE_dummy_ID.csv")) #asyncio