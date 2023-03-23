#!/usr/bin/python
# -*- coding: UTF-8

# This Source Code Form is subject to the terms of the Mozilla Public
# License, v. 2.0. If a copy of the MPL was not distributed with this
# file, You can obtain one at http://mozilla.org/MPL/2.0/. */

# Authors:
# Michael Berg-Mohnicke <michael.berg@zalf.de>
#
# Maintainers:
# Currently maintained by the authors.
#
# This file is part of the util library used by models created at the Institute of
# Landscape Systems Analysis at the ZALF.
# Copyright (C: Leibniz Centre for Agricultural Landscape Research (ZALF)

import capnp
import io
import os
from pathlib import Path
from pyproj import CRS, Transformer
import sys
from datetime import timedelta, date

PATH_TO_SCRIPT_DIR = Path(os.path.realpath(__file__)).parent
PATH_TO_REPO = PATH_TO_SCRIPT_DIR.parent.parent.parent
if str(PATH_TO_REPO) not in sys.path:
    sys.path.insert(1, str(PATH_TO_REPO))

PATH_TO_PYTHON_CODE = PATH_TO_REPO / "src/python"
if str(PATH_TO_PYTHON_CODE) not in sys.path:
    sys.path.insert(1, str(PATH_TO_PYTHON_CODE))

import common.common as common
import common.geo as geo

PATH_TO_CAPNP_SCHEMAS = PATH_TO_REPO / "capnproto_schemas"
abs_imports = [str(PATH_TO_CAPNP_SCHEMAS)]
common_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "common.capnp"), imports=abs_imports) 
geo_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "geo.capnp"), imports=abs_imports)
climate_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "climate.capnp"), imports=abs_imports)
fbp_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "fbp.capnp"), imports=abs_imports)

#------------------------------------------------------------------------------

config = {
    "_": "get_climate_locations.py",
    "dataset_sr": None, # sturdy ref to climate dataset
    "out_sr": None, # climate_capnp.TimeSeries (capability)
    "no_of_locations_at_once": "10",
    "to_attr": None,
    "continue_after_location_id": None
}
common.update_config(config, sys.argv, print_config=True, allow_new_keys=False)

conman = common.ConnectionManager()
service = conman.try_connect(config["dataset_sr"], cast_as=climate_capnp.Service, retry_secs=1)
dataset = service.getAvailableDatasets().wait().datasets[0].data
outp = conman.try_connect(config["out_sr"], cast_as=fbp_capnp.Channel.Writer, retry_secs=1)

try:
    if dataset and outp:
        if config["continue_after_location_id"]:
            callback = dataset.streamLocations(config["continue_after_location_id"]).wait().locationsCallback
        else:
            callback = dataset.streamLocations().wait().locationsCallback
        while True:
            ls = callback.nextLocations(int(config["no_of_locations_at_once"])).wait().locations
            if len(ls) == 0:
                break
            for l in ls:
                rc = l.customData[0].value.as_struct(geo_capnp.RowCol)
                attrs = [{"key": "id", "value": "row-{}_col-{}".format(rc.row, rc.col)}]
                if config["to_attr"]:
                    attrs.append({"key": config["to_attr"], "value": l.timeSeries})

                out_ip = fbp_capnp.IP.new_message(attributes=attrs)
                if not config["to_attr"]:
                    out_ip.content = l.timeSeries
                outp.write(value=out_ip).wait()

        # close out port
        outp.write(done=None).wait()

except Exception as e:
    print("get_climate_locations.py ex:", e)

print("get_climate_locations.py: exiting run")

