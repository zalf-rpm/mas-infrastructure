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
# Copyright (C: Leibniz Centre for Agricultural Landscape Research (ZALF)

import asyncio
import capnp
import os
from pathlib import Path
import sys
from zalfmas_common import common
from zalfmas_common import service as serv
from zalfmas_common.climate import csv_file_based as csv_based
import zalfmas_capnp_schemas
sys.path.append(os.path.dirname(zalfmas_capnp_schemas.__file__))


async def main(path_to_csv_file, serve_bootstrap=True, host=None, port=None,
               id=None, name="Single CSV Test Timeseries Service", description=None, srt=None):
    config = {
        "path_to_csv_file": path_to_csv_file,
        "id_col_name": "id",
        "port": port,
        "host": host,
        "id": id,
        "name": name,
        "description": description,
        "serve_bootstrap": serve_bootstrap,
        "srt": srt,
        "csv_separator": ",",
        "csv_skip_rows": 0,
        "csv_decimal": ".",
        "csv_thousands": None,
    }
    common.update_config(config, sys.argv, print_config=True, allow_new_keys=True)

    if config["path_to_csv_file"]:
        pandas_csv_config = {
            "sep": config["csv_separator"],
            "skiprows": config["csv_skip_rows"],
            "decimal": config["csv_decimal"],
        }
        if config["csv_thousands"]:
            pandas_csv_config["thousands"] = config["csv_thousands"]

        header_map = {}
        for k, v in config.items():
            if k[:7] == "header_":
                header_map[k[7:]] = v

        service = csv_based.TimeSeries.from_csv_file(config["path_to_csv_file"],
                                                     header_map=header_map,
                                                     pandas_csv_config=pandas_csv_config)
        await serv.init_and_run_service({"service": service}, config["host"], config["port"],
                                        serve_bootstrap=config["serve_bootstrap"],
                                        name_to_service_srs={"service": config["srt"]})
    else:
        print("No path to csv file given.")


if __name__ == '__main__':
    asyncio.run(capnp.run(main(None)))
