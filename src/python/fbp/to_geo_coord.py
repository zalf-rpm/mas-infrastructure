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
import os
from pathlib import Path
from pyproj import CRS, Transformer
import sys

PATH_TO_SCRIPT_DIR = Path(os.path.realpath(__file__)).parent
PATH_TO_REPO = PATH_TO_SCRIPT_DIR.parent.parent.parent
if str(PATH_TO_REPO) not in sys.path:
    sys.path.insert(1, str(PATH_TO_REPO))

PATH_TO_PYTHON_CODE = PATH_TO_REPO / "src/python"
if str(PATH_TO_PYTHON_CODE) not in sys.path:
    sys.path.insert(1, str(PATH_TO_PYTHON_CODE))

from pkgs.common import common
from pkgs.common import geo

PATH_TO_CAPNP_SCHEMAS = PATH_TO_REPO / "capnproto_schemas"
abs_imports = [str(PATH_TO_CAPNP_SCHEMAS)]
common_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "common.capnp"), imports=abs_imports)
fbp_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "fbp.capnp"), imports=abs_imports)
geo_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "geo.capnp"), imports=abs_imports)


config = {
    "to_name": "wgs84",
    "list_type": "float", # float | int
    "in_vals_sr": None, # list[float | int]
    "out_coord_sr": None # geo.LatLonCoord | geo.UTMCoord | geo.GKCoord
}
common.update_config(config, sys.argv, print_config=True, allow_new_keys=False)

conman = common.ConnectionManager()
inp = conman.try_connect(config["in_vals_sr"], cast_as=fbp_capnp.Channel.Reader, retry_secs=1)
outp = conman.try_connect(config["out_coord_sr"], cast_as=fbp_capnp.Channel.Writer, retry_secs=1)

to_instance = geo.name_to_struct_instance(config["to_name"])
list_schema_type = capnp._ListSchema(capnp.types.Float64) if config["list_type"] == "float" else capnp._ListSchema(capnp.types.Int64)

try:
    if inp and outp:
        while True:
            msg = inp.read().wait()
            # check for end of data from in port
            if msg.which() == "done":
                break
            
            vals = msg.value.as_struct(fbp_capnp.IP).content.as_list(list_schema_type)
            if len(vals) > 1:
                to_coord = to_instance.copy()
                geo.set_xy(to_coord, vals[0], vals[1])
                outp.write(value=fbp_capnp.IP.new_message(content=to_coord)).wait()
            else:
                raise Exception("Not enough values in list. Need at least two for a coordinate.")

        # close out port
        outp.write(done=None).wait()

except Exception as e:
    print("to_geo_coord.py ex:", e)

print("to_geo_coord.py: exiting run")

