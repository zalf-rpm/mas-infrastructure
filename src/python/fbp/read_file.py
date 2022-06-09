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
import sys

PATH_TO_SCRIPT_DIR = Path(os.path.realpath(__file__)).parent
PATH_TO_REPO = PATH_TO_SCRIPT_DIR.parent.parent.parent
if str(PATH_TO_REPO) not in sys.path:
    sys.path.insert(1, str(PATH_TO_REPO))

PATH_TO_PYTHON_CODE = PATH_TO_REPO / "src/python"
if str(PATH_TO_PYTHON_CODE) not in sys.path:
    sys.path.insert(1, str(PATH_TO_PYTHON_CODE))

import common.common as common

PATH_TO_CAPNP_SCHEMAS = PATH_TO_REPO / "capnproto_schemas"
abs_imports = [str(PATH_TO_CAPNP_SCHEMAS)]
common_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "common.capnp"), imports=abs_imports) 

#------------------------------------------------------------------------------

config = {
    "attr_sr": None,
    "to_attr": "setup",
    "out_sr": None,
    "file": "src/python/fbp/test.txt",
    "skip_lines": str(0)
}
common.update_config(config, sys.argv, print_config=True, allow_new_keys=False)

conman = common.ConnectionManager()
attrp = conman.try_connect(config["attr_sr"], cast_as=common_capnp.Channel.Reader, retry_secs=1)
outp = conman.try_connect(config["out_sr"], cast_as=common_capnp.Channel.Writer, retry_secs=1)
skip_lines = int(config["skip_lines"])

try:
    attr = None
    if attrp:
        msg = attrp.read().wait()
        # check for end of data from in port
        if msg.which() != "done":
            attr_ip = msg.value.as_struct(common_capnp.IP)
            attr = attr_ip.content

    if outp:
        with open(config["file"]) as _:
            for line in _.readlines():
                if skip_lines > 0:
                    skip_lines -= 1
                    continue
                
                out_ip = common_capnp.IP.new_message(content=line)
                if attr and config["to_attr"]:
                    out_ip.attributes = [{"key": config["to_attr"], "value": attr}]
                outp.write(value=out_ip).wait()
    
    outp.write(done=None).wait()

except Exception as e:
    print("read_file.py ex:", e)

print("read_file.py: exiting run")


