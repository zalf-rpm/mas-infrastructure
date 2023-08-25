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
from pkgs.climate import csv_file_based as csv_based

PATH_TO_CAPNP_SCHEMAS = PATH_TO_REPO / "capnproto_schemas"
abs_imports = [str(PATH_TO_CAPNP_SCHEMAS)]


# reg_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "registry.capnp"), imports=abs_imports)


async def main(path_to_csv_file, serve_bootstrap=True, host=None, port=None,
               id=None, name="Single CSV Test Timeseries Service", description=None, use_async=False, srt=None):
    config = {
        "path_to_csv_file": path_to_csv_file,
        "id_col_name": "id",
        "port": port,
        "host": host,
        "id": id,
        "name": name,
        "description": description,
        "serve_bootstrap": serve_bootstrap,
        "use_async": use_async,
        "srt": srt
    }
    common.update_config(config, sys.argv, print_config=True, allow_new_keys=False)

    restorer = common.Restorer()
    service = csv_based.TimeSeries.from_csv_file(config["path_to_csv_file"],
                                                 header_map={},  # {"windspeed": "wind"},
                                                 pandas_csv_config={}  # {"sep": ";"}
                                                 )
    if use_async:
        await serv.async_init_and_run_service({"service": service}, config["host"], config["port"],
                                              serve_bootstrap=config["serve_bootstrap"], restorer=restorer,
                                              name_to_service_srs={"service": config["srt"]})
    else:
        serv.init_and_run_service({"service": service}, config["host"], config["port"],
                                  serve_bootstrap=config["serve_bootstrap"], restorer=restorer,
                                  name_to_service_srs={"service": config["srt"]})


# ------------------------------------------------------------------------------

if __name__ == '__main__':
    path = str(PATH_TO_REPO / "data/climate/climate-iso.csv")
    asyncio.run(main(path, use_async=True))
