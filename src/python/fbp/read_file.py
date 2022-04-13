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
    "out_sr": "capnp://insecure@10.10.24.210:9999/w1234",#None,
}
common.update_config(config, sys.argv, True)

conman = common.ConnectionManager()
outp = conman.try_connect(config["out_sr"], cast_as=common_capnp.Writer, retry_secs=1)

if outp:
    with open("src/python/fbp/test.txt") as _:
        for line in _.readlines():
            #print(line, end="", flush=True)
            outp.write(value=line).wait()

outp.done().wait()
print("read_file.py: exiting run")


