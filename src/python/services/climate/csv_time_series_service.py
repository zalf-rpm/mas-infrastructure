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

import csv_file_based as csv_based

import capnp
#import capnproto_schemas.climate_data_capnp as climate_data_capnp

#------------------------------------------------------------------------------

def main(server="*", port=11002, path_to_csv_file="data/climate/climate-iso.csv"):

    config = {
        "port": str(port),
        "server": server,
        "path_to_csv_file": path_to_csv_file
    }
    # read commandline args only if script is invoked directly from commandline
    if len(sys.argv) > 1 and __name__ == "__main__":
        for arg in sys.argv[1:]:
            k, v = arg.split("=")
            if k in config:
                config[k] = v
    print(config)

    server = capnp.TwoPartyServer(config["server"] + ":" + config["port"],
                                  bootstrap=csv_based.TimeSeries.from_csv_file(config["path_to_csv_file"],
                                    header_map={},#{"windspeed": "wind"},
                                    pandas_csv_config={}))#{"sep": ";"}))
    server.run_forever()


if __name__ == '__main__':
    main()
