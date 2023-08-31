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
import string
import sys

PATH_TO_SCRIPT_DIR = Path(os.path.realpath(__file__)).parent
PATH_TO_REPO = PATH_TO_SCRIPT_DIR.parent.parent.parent
if str(PATH_TO_REPO) not in sys.path:
    sys.path.insert(1, str(PATH_TO_REPO))

PATH_TO_PYTHON_CODE = PATH_TO_REPO / "src/python"
if str(PATH_TO_PYTHON_CODE) not in sys.path:
    sys.path.insert(1, str(PATH_TO_PYTHON_CODE))

from pkgs.common import common

PATH_TO_CAPNP_SCHEMAS = PATH_TO_REPO / "capnproto_schemas"
abs_imports = [str(PATH_TO_CAPNP_SCHEMAS)]
common_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "common.capnp"), imports=abs_imports)
fbp_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "fbp.capnp"), imports=abs_imports)


config = {
    "split_at": ",",
    "cast_to": "text",  # text | float | int
    "in_sr": None,  # string
    "out_sr": None  # list[text | float | int]
}
common.update_config(config, sys.argv, print_config=True, allow_new_keys=False)

conman = common.ConnectionManager()
inp = conman.try_connect(config["in_sr"], cast_as=fbp_capnp.Channel.Reader, retry_secs=1)
outp = conman.try_connect(config["out_sr"], cast_as=fbp_capnp.Channel.Writer, retry_secs=1)

cast_to = None
if config["cast_to"] == "float":
    cast_to = lambda v: float(v)
elif config["cast_to"] == "int":
    cast_to = lambda v: int(v)

init_list = lambda anyp, len: anyp.init_as_list(capnp._ListSchema(capnp.types.Text), len) 
if config["cast_to"] == "float":
    init_list = lambda anyp, len: anyp.init_as_list(capnp._ListSchema(capnp.types.Float64), len) 
elif config["cast_to"] == "int":
    init_list = lambda anyp, len: anyp.init_as_list(capnp._ListSchema(capnp.types.Int64), len) 

try:
    if inp and outp:
        while True:
            msg = inp.read().wait()
            # check for end of data from in port
            if msg.which() == "done":
                break
            
            s : str = msg.value.as_struct(fbp_capnp.IP).content.as_text()
            s = s.rstrip()
            vals = s.split(config["split_at"])
            if cast_to:
                vals = list(map(cast_to, vals))
            #print("split_string vals:", vals)

            req = outp.write_request()
            l = init_list(req.value.as_struct(fbp_capnp.IP).content, len(vals))
            for i, val in enumerate(vals):
                l[i] = val
            req.send().wait()
            #outp.write(value=vals).wait()

        # close out port
        outp.write(done=None).wait()

except Exception as e:
    print("split_string.py ex:", e)

print("split_string.py: exiting run")

