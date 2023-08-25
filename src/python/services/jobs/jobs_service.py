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

PATH_TO_REPO = Path(os.path.realpath(__file__)).parent.parent.parent.parent.parent
if str(PATH_TO_REPO) not in sys.path:
    sys.path.insert(1, str(PATH_TO_REPO))

PATH_TO_PYTHON_CODE = PATH_TO_REPO / "src/python"
if str(PATH_TO_PYTHON_CODE) not in sys.path:
    sys.path.insert(1, str(PATH_TO_PYTHON_CODE))

from pkgs.common import common
from pkgs.common import service as serv
from pkgs.common import csv

PATH_TO_CAPNP_SCHEMAS = PATH_TO_REPO / "capnproto_schemas"
abs_imports = [str(PATH_TO_CAPNP_SCHEMAS)]
jobs_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "jobs.capnp"), imports=abs_imports)


class Service(jobs_capnp.Service.Server, common.Identifiable, common.Persistable, serv.AdministrableService):

    def __init__(self, jobs=[], id=None, name=None, description=None, admin=None, restorer=None):
        common.Identifiable.__init__(self, id, name, description)
        common.Persistable.__init__(self, restorer)
        serv.AdministrableService.__init__(self, admin)

        self._jobs = [{"data": data} for data in jobs]

    def nextJob(self, **kwargs):  # nextJob @0 () -> (job :Job);
        print(len(self._jobs))
        try:
            return self._jobs.pop()
        except:
            return {"noFurtherJobs": True}


# ------------------------------------------------------------------------------

async def main(path_to_csv, serve_bootstrap=True, host=None, port=None,
               id=None, name="Jobs Service", description=None, use_async=False):
    config = {
        "path_to_csv": path_to_csv,
        "id_col_name": "dummy_id",
        "port": port,
        "host": host,
        "id": id,
        "name": name,
        "description": description,
        "serve_bootstrap": serve_bootstrap,
        "use_async": use_async
    }
    # read commandline args only if script is invoked directly from commandline
    common.update_config(config, sys.argv, print_config=True, allow_new_keys=False)

    restorer = common.Restorer()
    jobs = csv.read_csv(config["path_to_csv"], config["id_col_name"])
    jobs2 = [json.dumps(v) for k, v in jobs.items()]
    service = Service(jobs2, id=config["id"], name=config["name"], description=config["description"], restorer=restorer)
    if use_async:
        await serv.async_init_and_run_service({"service": service}, config["host"], config["port"],
                                              serve_bootstrap=config["serve_bootstrap"], restorer=restorer)
    else:

        serv.init_and_run_service({"service": service}, config["host"], config["port"],
                                  serve_bootstrap=config["serve_bootstrap"], restorer=restorer)


if __name__ == '__main__':
    asyncio.run(main("/home/berg/Desktop/Koordinaten_HE_dummy_ID.csv", use_async=True))
