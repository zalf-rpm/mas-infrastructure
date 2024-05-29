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
import psutil
from random import random
import sys
import time
from threading import Thread

PATH_TO_REPO = Path(os.path.realpath(__file__)).parent.parent.parent
if str(PATH_TO_REPO) not in sys.path:
    sys.path.insert(1, str(PATH_TO_REPO))

PATH_TO_PYTHON_CODE = PATH_TO_REPO / "src/python"
if str(PATH_TO_PYTHON_CODE) not in sys.path:
    sys.path.insert(1, str(PATH_TO_PYTHON_CODE))

from pkgs.common import common
from pkgs.climate import csv_file_based as csv_based
from pkgs.common import service as serv

PATH_TO_CAPNP_SCHEMAS = PATH_TO_REPO / "capnproto_schemas"
abs_imports = [str(PATH_TO_CAPNP_SCHEMAS)]
reg_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "registry.capnp"), imports=abs_imports)
soil_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "soil.capnp"), imports=abs_imports)
registry_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "registry.capnp"), imports=abs_imports)
persistence_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "persistence.capnp"), imports=abs_imports)
model_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "model.capnp"), imports=abs_imports)
yieldstat_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "model" / "yieldstat" / "yieldstat.capnp"),
                             imports=abs_imports)
monica_state_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "model" / "monica" / "monica_state.capnp"),
                                imports=abs_imports)
monica_mgmt_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "model" / "monica" / "monica_management.capnp"),
                               imports=abs_imports)
climate_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "climate.capnp"), imports=abs_imports)
mgmt_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "management.capnp"), imports=abs_imports)
service_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "service.capnp"), imports=abs_imports)
common_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "common.capnp"), imports=abs_imports)
grid_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "grid.capnp"), imports=abs_imports)
storage_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "storage.capnp"), imports=abs_imports)
geo_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "geo.capnp"), imports=abs_imports)
crop_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "crop.capnp"), imports=abs_imports)
fbp_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "fbp.capnp"), imports=abs_imports)
a_capnp = capnp.load("a.capnp", imports=abs_imports)


class A(a_capnp.A.Server):
    async def method(self, param, **kwargs):
        print("param:", param)

    async def m(self, id, **kwargs):
        print("id:", id)


async def new_connection(stream):
    await capnp.TwoPartyServer(stream, bootstrap=A()).on_disconnect()


async def main():
    await serv.init_and_run_service({"service": A()}, "localhost", 9999,
                                    serve_bootstrap=True,
                                    name_to_service_srs={"service": "aaaa"})

    server = await capnp.AsyncIoStream.create_server(new_connection, "localhost", 9999)
    async with server:
        await server.serve_forever()

if __name__ == '__main__':
    asyncio.run(capnp.run(main()))
