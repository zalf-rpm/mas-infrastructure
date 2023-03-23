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
fbp_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "fbp.capnp"), imports=abs_imports)

#------------------------------------------------------------------------------

config = {
    "_": "write_file.py",
    "in_sr": None,
    "id_attr": "id",
    "from_attr": None,
    "filepath_pattern": "csv_{id}.csv",
    "path_to_out_dir": "/home/berg/GitHub/mas-infrastructure/src/python/fbp/out/",
    "append": False,
    "debug": False,
}
common.update_config(config, sys.argv, print_config=True, allow_new_keys=False)

conman = common.ConnectionManager()
inp = conman.try_connect(config["in_sr"], cast_as=fbp_capnp.Channel.Reader, retry_secs=1)
count = 0

try:
    if inp:
        while True:
            msg = inp.read().wait()
            if msg.which() == "done":
                break

            in_ip = msg.value.as_struct(fbp_capnp.IP)
            
            id_attr = common.get_fbp_attr(in_ip, config["id_attr"])
            id = id_attr.as_text() if id_attr else str(count)
            content_attr = common.get_fbp_attr(in_ip, config["from_attr"])
            content = content_attr.as_text() if content_attr else in_ip.content.as_text()

            filepath = config["path_to_out_dir"] + config["filepath_pattern"].format(id=id)
            with open(filepath, "at" if config["append"] else "wt") as _:
                _.write(content)
                count += 1
                
            if config["debug"]:
                print("wrote", filepath)

except Exception as e:
    print("write_file.py ex:", e)

print("write_file.py: exiting run")





