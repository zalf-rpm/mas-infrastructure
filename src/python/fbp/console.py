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
    import common as common
    import service as serv
    import capnp_async_helpers as async_helpers
else:
    import common.common as common
    import common.service as serv
    import common.capnp_async_helpers as async_helpers
import fbp.fbp as fbp

PATH_TO_CAPNP_SCHEMAS = PATH_TO_REPO / "capnproto_schemas"
abs_imports = [str(PATH_TO_CAPNP_SCHEMAS)]
common_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "common.capnp"), imports=abs_imports) 
reg_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "registry.capnp"), imports=abs_imports)
fbp_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "fbp.capnp"), imports=abs_imports)


#------------------------------------------------------------------------------

class Console(fbp.Config, common.Identifiable): 

    def __init__(self, connection_manager, data_type="Text", buffer_size=1, 
        id=None, name=None, description=None, restorer=None):
        fbp.Config.__init__(self, connection_manager, in_port_config={"in": (data_type, buffer_size)}, restorer=restorer)
        common.Identifiable.__init__(self, id, name, description)

        self._id = str(id if id else uuid.uuid4())
        self._name = name if name else self._id
        self._description = description if description else ""
        self._stop = False


    def run(self):
        inp = self._in_ports["in"]["port"]
        if inp:
            while not self._stop:
                inp.read_value().then(lambda v: print(v))

#------------------------------------------------------------------------------

async def main(buffer_size=1, serve_bootstrap=True, host=None, port=None, 
    id=None, name="Console component", description=None, use_async=False):

    config = {
        "in_buffer_size": str(buffer_size),
        "in_type": None, #"/home/berg/GitHub/mas-infrastructure/capnproto_schemas/model/monica/monica_state.capnp:ICData",
        "port": port, 
        "host": host,
        "id": id,
        "name": name,
        "description": description,
        "serve_bootstrap": serve_bootstrap,
        "use_async": use_async,
        "store_srs_file": None
    }
    
    # read commandline args only if script is invoked directly from commandline
    if len(sys.argv) > 1 and __name__ == "__main__":
        for arg in sys.argv[1:]:
            k, v = arg.split("=")
            if k in config:
                config[k] = v.lower() == "true" if v.lower() in ["true", "false"] else v 
    print(config)

    in_type = "Text"
    if config["in_type"] is not None:
        capnp_module_path, type_name = config["in_type"].split(":")
        capnp_module = capnp.load(capnp_module_path, imports=abs_imports)
        capnp_type = capnp_module.__dict__.get(type_name, "Text")
        in_type = capnp_type

    restorer = common.Restorer()
    conman = async_helpers.ConnectionManager() if config["use_async"] else common.ConnectionManager()
    services = {}
    name_to_service_srs = {}
    component = Console(conman, in_value_type=in_type, in_buffer_size=int(config["buffer_size"]), 
        id=config["id"], name=config["name"], description=config["description"], restorer=restorer)
    in_port = component._in
    services["in"] =  in_port

    store_srs_file_path = config["store_srs_file"]
    def write_srs():
        if store_srs_file_path:
            with open(store_srs_file_path, mode="wt") as _:
                _.write(json.dumps(name_to_service_srs))

    if config["use_async"]:
        await serv.async_init_and_run_service(services, config["host"], config["port"], 
            serve_bootstrap=config["serve_bootstrap"], conman=conman, restorer=restorer, 
            name_to_service_srs=name_to_service_srs,
            run_before_enter_eventloop=write_srs)
    else:
        
        serv.init_and_run_service(services, config["host"], config["port"], 
            serve_bootstrap=config["serve_bootstrap"], conman=conman, restorer=restorer, 
            name_to_service_srs=name_to_service_srs,
            run_before_enter_eventloop=write_srs)

#------------------------------------------------------------------------------

if __name__ == '__main__':
    asyncio.run(main(buffer_size=1, serve_bootstrap=True, use_async=False)) 



