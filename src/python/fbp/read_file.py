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
from collections import deque, defaultdict
import json
import logging
import os
from pathlib import Path
import socket
import sys
import time
import threading
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
fbp_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "fbp.capnp"), imports=abs_imports)

capnp.remove_event_loop()
capnp.create_event_loop(threaded=True)


#------------------------------------------------------------------------------

class FileReader(fbp.Component):#, common.Identifiable): 

    #def __init__(self, id=None, name=None, description=None, restorer=None):
    #    out_ports={"out": "Text"}

    #    fbp.Component.__init__(self, out_ports=out_ports, restorer=restorer)
    #    common.Identifiable.__init__(self, id, name, description)

    #    self._id = str(id if id else uuid.uuid4())
    #    self._name = name if name else self._id
    #    self._description = description if description else ""


    def run(self):
        while not self.stop:
            outp = self.out_ports["out"]["port"]
            if outp:
                print("out-port available")
                break
            else:
                print("sleeping a bit")
                time.sleep(1)
                capnp.poll_once()

        if outp and not self.stop:
            with open("src/python/fbp/test.txt") as _:
                while not self.stop:
                    line = _.readline()
                    outp.write(value=line).wait();#then(lambda v: print(v))

        print("exiting run")

#------------------------------------------------------------------------------

async def main(serve_bootstrap=True, host=None, port=None, 
    id=None, name="Read file component", description=None, use_async=False):

    config = {
        "out_sr": "capnp://insecure@10.10.24.210:9999/1234",#None,
        "port": port, 
        "host": host,
        "id": id,
        "name": name,
        "description": description,
        "serve_bootstrap": serve_bootstrap,
        "use_async": use_async
    }
    
    # read commandline args only if script is invoked directly from commandline
    if len(sys.argv) > 1 and __name__ == "__main__":
        for arg in sys.argv[1:]:
            k, v = arg.split("=")
            if k in config:
                config[k] = v.lower() == "true" if v.lower() in ["true", "false"] else v 
    print(config)

    #name_to_out_type = common.load_capnp_modules({"in": config["out_type"]})

    s1, s2 = socket.socketpair()
    t = threading.Thread(target=fbp.start_component_thread, args=(s2, FileReader()))
    t.daemon = True
    t.start()

    component_client = capnp.TwoPartyClient(s1)
    component_cap = component_client.bootstrap().cast_as(fbp_capnp.Component)
    
    restorer = common.Restorer()
    out_ports = {
        "out": {
            "sr": config["out_sr"],
            "port": None    
        }
    }

    def set_ports():
        component_cap.setOutputPorts([{"name": "out", "port": out_ports["out"]["port"]}]).wait()

    if config["use_async"]:
        await fbp.async_init_and_run_fbp_component(name_to_out_ports=out_ports, 
            host=config["host"], port=config["port"], 
            serve_bootstrap=config["serve_bootstrap"], restorer=restorer, eventloop_wait_forever=True,
            run_before_enter_eventloop=set_ports)
    else:
        fbp.init_and_run_fbp_component(name_to_out_ports=out_ports, 
            host=config["host"], port=config["port"], 
            serve_bootstrap=config["serve_bootstrap"], restorer=restorer, eventloop_wait_forever=True,
            run_before_enter_eventloop=set_ports)

#------------------------------------------------------------------------------

if __name__ == '__main__':
    asyncio.run(main(serve_bootstrap=True, use_async=False)) 


