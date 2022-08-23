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
    "lift_from_attr": "name",
    "lift_from_type": "schema.capnp:Type",
    "lifted_attrs": "attr1,attr2,attr3",
    "in_sr": None, 
    "out_sr": None 
}
common.update_config(config, sys.argv, print_config=True, allow_new_keys=False)

conman = common.ConnectionManager()
inp = conman.try_connect(config["in_sr"], cast_as=common_capnp.Channel.Reader, retry_secs=1)
outp = conman.try_connect(config["out_sr"], cast_as=common_capnp.Channel.Writer, retry_secs=1)

lift_from_type = common.load_capnp_module(config["lift_from_type"])
lft_fieldnames = lift_from_type.schema.fieldnames
lift_from_attr_name = config["lift_from_attr"]

try:
    if inp and outp:
        while True:
            msg = inp.read().wait()
            # check for end of data from in port
            if msg.which() == "done":
                break
            
            in_ip = msg.value.as_struct(common_capnp.IP)
            lift_from_attr = common.get_fbp_attr(in_ip, config["lift_from_attr"]).as_struct(lift_from_type)
            
            out_ip = common_capnp.IP.new_message(content=in_ip.content)
            lifted_attrs = config["lifted_attrs"].split(",")
            attrs = []
            for attr in in_ip.attributes:
                attrs.append(attr)
            if lift_from_attr:
                for lattr_name in lifted_attrs:
                    if lattr_name in lft_fieldnames:
                        attrs.append({"key": lattr_name, "value": lift_from_attr.__getattr__(lattr_name)})
            out_ip.attributes = attrs    
            outp.write(value=out_ip).wait()

        # close out port
        outp.write(done=None).wait()

except Exception as e:
    print("lift_attributes.py ex:", e)

print("lift_attributes.py: exiting run")

