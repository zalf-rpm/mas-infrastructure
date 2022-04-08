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

#remote debugging via commandline
#-m ptvsd --host 0.0.0.0 --port 14000 --wait

import asyncio
import capnp
from collections import deque
import json
import logging
import os
from pathlib import Path
import sys
import uuid

PATH_TO_SCRIPT_DIR = Path(os.path.realpath(__file__)).parent
PATH_TO_REPO = PATH_TO_SCRIPT_DIR.parent.parent.parent
if str(PATH_TO_REPO) not in sys.path:
    sys.path.insert(1, str(PATH_TO_REPO))

PATH_TO_PYTHON_CODE = PATH_TO_REPO / "src/python"
if str(PATH_TO_PYTHON_CODE) not in sys.path:
    sys.path.insert(1, str(PATH_TO_PYTHON_CODE))

if str(PATH_TO_SCRIPT_DIR) in sys.path:
    import fbp as fbp    
else:
    import fbp.fbp as fbp

import common.common as common
import common.service as serv
import common.capnp_async_helpers as async_helpers

PATH_TO_CAPNP_SCHEMAS = PATH_TO_REPO / "capnproto_schemas"
abs_imports = [str(PATH_TO_CAPNP_SCHEMAS)]
common_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "common.capnp"), imports=abs_imports) 
reg_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "registry.capnp"), imports=abs_imports)
fbp_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "fbp.capnp"), imports=abs_imports)

is_async = False

#------------------------------------------------------------------------------

class Console(fbp.Component, common.Identifiable): 

    def __init__(self, in_buffer_size=1, id=None, name=None, description=None, restorer=None):
        in_ports={"in": ("Text", in_buffer_size)}

        fbp.Component.__init__(self, in_ports=in_ports, restorer=restorer)
        common.Identifiable.__init__(self, id, name, description)

        self._id = str(id if id else uuid.uuid4())
        self._name = name if name else self._id
        self._description = description if description else ""
        self._stop = False


    def run(self):
        inp = self.in_ports["in"]["port"]
        if inp:
            if is_async:
                val = inp.read_value().then(lambda v: print(v))    
            else:
                val = inp.read_value().then(lambda v: print(v))
            print(val)

#------------------------------------------------------------------------------

async def main(buffer_size=1, serve_bootstrap=True, host=None, port=None, 
    id=None, name="Console component", description=None, use_async=False):

    config = {
        "in_buffer_size": str(buffer_size),
        "in_sr_token": "1234",#None, 
        "port": "9999",#port, 
        "host": host,
        "id": id,
        "name": name,
        "description": description,
        "serve_bootstrap": serve_bootstrap,
        "use_async": use_async
    }

    global is_async
    is_async = config["use_async"]

    # read commandline args only if script is invoked directly from commandline
    if len(sys.argv) > 1 and __name__ == "__main__":
        for arg in sys.argv[1:]:
            k, v = arg.split("=")
            if k in config:
                config[k] = v.lower() == "true" if v.lower() in ["true", "false"] else v 
    print(config)

    #name_to_out_type = common.load_capnp_modules({"in": config["out_type"]})

    restorer = common.Restorer()
    component = Console(in_buffer_size=int(config["in_buffer_size"]), 
        id=config["id"], name=config["name"], description=config["description"], restorer=restorer)
    component.in_ports["in"]["sr_token"] = config["in_sr_token"]

    if config["use_async"]:
        await fbp.async_init_and_run_fbp_component(name_to_in_ports=component.in_ports, 
            host=config["host"], port=config["port"], 
            serve_bootstrap=config["serve_bootstrap"], restorer=restorer, eventloop_wait_forever=False)
    else:
        fbp.init_and_run_fbp_component(name_to_in_ports=component.in_ports,
            host=config["host"], port=config["port"], 
            serve_bootstrap=config["serve_bootstrap"], restorer=restorer, eventloop_wait_forever=False)

    if is_async:
        component.run()
    else:
        component.run()

#------------------------------------------------------------------------------

if __name__ == '__main__':
    asyncio.run(main(buffer_size=1, serve_bootstrap=True, use_async=True)) 



