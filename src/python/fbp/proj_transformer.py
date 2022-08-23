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

import common.common as common
import common.geo as geo

PATH_TO_CAPNP_SCHEMAS = PATH_TO_REPO / "capnproto_schemas"
abs_imports = [str(PATH_TO_CAPNP_SCHEMAS)]
common_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "common.capnp"), imports=abs_imports) 
geo_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "geo.capnp"), imports=abs_imports)

#------------------------------------------------------------------------------

config = {
    "from_name": "utm32n",
    "to_name": "latlon",
    "to_attr": None, #"latlon",
    "from_attr": None, 
    "in_sr": None, # geo.LatLonCoord | geo.UTMCoord | geo.GKCoord
    "out_sr": None # geo.LatLonCoord | geo.UTMCoord | geo.GKCoord
}
common.update_config(config, sys.argv, print_config=True, allow_new_keys=False)

conman = common.ConnectionManager()
inp = conman.try_connect(config["in_sr"], cast_as=common_capnp.Channel.Reader, retry_secs=1)
outp = conman.try_connect(config["out_sr"], cast_as=common_capnp.Channel.Writer, retry_secs=1)

from_type = geo.name_to_struct_type(config["from_name"])

try:
    if inp and outp:
        while True:
            msg = inp.read().wait()
            # check for end of data from in port
            if msg.which() == "done":
                break
            
            in_ip = msg.value.as_struct(common_capnp.IP)
            attr = common.get_fbp_attr(in_ip, config["from_attr"])
            if attr:
                from_coord = attr.as_struct(from_type)
            else:
                from_coord = in_ip.content.as_struct(from_type)
            to_coord = geo.transform_from_to_geo_coord(from_coord, config["to_name"])
            out_ip = common_capnp.IP.new_message()
            if not config["to_attr"]:
                out_ip.content = to_coord
            common.copy_fbp_attr(in_ip, out_ip, config["to_attr"], to_coord)
            outp.write(value=out_ip).wait()

        # close out port
        outp.write(done=None).wait()

except Exception as e:
    print("proj_transformer.py ex:", e)

print("proj_transformer.py: exiting run")

