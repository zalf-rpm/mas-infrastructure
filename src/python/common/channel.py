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

PATH_TO_CAPNP_SCHEMAS = PATH_TO_REPO / "capnproto_schemas"
abs_imports = [str(PATH_TO_CAPNP_SCHEMAS)]
common_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "common.capnp"), imports=abs_imports) 
reg_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "registry.capnp"), imports=abs_imports)

#------------------------------------------------------------------------------

class Channel(common_capnp.Channel.Server, common.Identifiable, common.Persistable, serv.AdministrableService): 

    def __init__(self, channel_value_type="Text", buffer_size=1, id=None, name=None, description=None, admin=None, restorer=None):
        common.Identifiable.__init__(self, id, name, description)
        common.Persistable.__init__(self, restorer)
        serv.AdministrableService.__init__(self, admin)

        self._id = str(id if id else uuid.uuid4())
        self._name = name if name else self._id
        self._description = description if description else ""
        self._buffer = deque()
        self._buffer_size = buffer_size
        self._endpoints = []
        self._blocking_read_fulfillers = deque()
        self._blocking_write_fulfillers = deque()
        self._channel_value_type = channel_value_type


    def deserialize(self, v):
        if self._channel_value_type == "Text":
            return v
        elif isinstance(self._channel_value_type, capnp.lib.capnp._StructModule):
            return self._channel_value_type.from_bytes(v)


    def serialize(self, v):
        if self._channel_value_type == "Text":
            return v.as_text()
        elif isinstance(self._channel_value_type, capnp.lib.capnp._StructModule):
            return v.as_struct(self._channel_value_type).as_builder().to_bytes()


    def setBufferSize_context(self, context): # setBufferSize @0 (size :UInt64);
        self._buffer_size = max(1, context.params.size)


    def reader_context(self, context): # reader        @1 () -> (r :Reader(V));
        ep = {"r": Reader(self), "w": None}
        self._endpoints.append(ep)
        context.results.r = ep["r"]


    def writer_context(self, context): # writer        @2 () -> (w :Writer(V));
        if len(self._endpoints) > 0 and self._endpoints[-1]["w"] is None:
            ep = self._endpoints[-1]
            ep["w"] = Writer(self)
        else:
            ep = {"r": None, "w": Writer(self)}
            self._endpoints.append(ep)
        context.results.w = ep["w"]


    def create_reader_writer_pair(self):
        ep = {"r": Reader(self), "w": Writer(self)}
        self._endpoints.append(ep)
        return ep


    def endpoints_context(self, context): # endpoints     @1 () -> (r :Reader(V), w :Writer(V));
        ep = self.create_reader_writer_pair()
        context.results.r = ep["r"]
        context.results.w = ep["w"]

#------------------------------------------------------------------------------

class Reader(common_capnp.Reader.Server): 

    def __init__(self, channel):
        self._channel = channel

    def read_context(self, context): # read @0 () -> (value :V);
        c = self._channel
        b = c._buffer
        # read value non-blocking
        if len(b) > 0:
            #context.results.value = common_capnp.X.from_bytes(b.popleft())
            context.results.value = c.deserialize(b.popleft())
            print(c.name, "r ", sep="", end="")
            # unblock potentially waiting writers
            while len(b) < c._buffer_size and len(c._blocking_write_fulfillers) > 0:
                c._blocking_write_fulfillers.popleft().fulfill()
        else: # block because no value to read
            paf = capnp.PromiseFulfillerPair()
            c._blocking_read_fulfillers.append(paf) 
            print("[", c.name, "r"+str(len(c._blocking_read_fulfillers))+"] ", sep="", end="")
            #return paf.promise.then(lambda: setattr(context.results, "value", common_capnp.X.from_bytes(b.popleft())))
            return paf.promise.then(lambda: setattr(context.results, "value", c.deserialize(b.popleft())))


class Writer(common_capnp.Writer.Server): 

    def __init__(self, channel):
        self._channel = channel

    def write_context(self, context): # write @0 (value :V);
        v = context.params.value
        c = self._channel
        b = c._buffer
        # write value non-block
        if len(b) < c._buffer_size:
            #b.append(v.as_struct(common_capnp.X).as_builder().to_bytes())
            b.append(c.serialize(v))
            print(c.name, "w ", sep="", end="")
            # unblock potentially waiting readers
            while len(b) > 0 and len(c._blocking_read_fulfillers) > 0:
                c._blocking_read_fulfillers.popleft().fulfill()
        else: # block until buffer has space
            paf = capnp.PromiseFulfillerPair()
            c._blocking_write_fulfillers.append(paf)
            print("[", c.name, "w"+str(len(c._blocking_write_fulfillers))+"] ", sep="", end="")
            #return paf.promise.then(lambda: b.append(v.as_struct(common_capnp.X).as_builder().to_bytes()))
            return paf.promise.then(lambda: b.append(c.serialize(v)))

#------------------------------------------------------------------------------

async def main(no_of_channels = 1, buffer_size=1, serve_bootstrap=True, host=None, port=None, 
    id=None, name="Channel Service", description=None, use_async=False):

    config = {
        "no_of_channels": str(no_of_channels),
        "buffer_size": str(max(1, buffer_size)),
        "type_1": "/home/berg/GitHub/mas-infrastructure/capnproto_schemas/model/monica/monica_state.capnp:ICData",
        "type_2": "/home/berg/GitHub/mas-infrastructure/capnproto_schemas/model/monica/monica_state.capnp:ICData",
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
                config[k] = bool(v) if v.lower() in ["true", "false"] else v 
    print(config)

    channel_to_type = {i : "Text" for i in range(1, int(config["no_of_channels"])+1)}
    for k, v in config.items():
        if k.startswith("type_"):
            no = int(k.split("_")[1])
            capnp_module_path, type_name = v.split(":")
            capnp_module = capnp.load(capnp_module_path, imports=abs_imports)
            capnp_type = capnp_module.__dict__.get(type_name, "Text")
            channel_to_type[no] = capnp_type

    restorer = common.Restorer()
    services = {}
    name_to_service_srs = {}
    for i in range(1, int(no_of_channels)+1):
        channel_value_type = channel_to_type.get(i, "Text")
        c = Channel(channel_value_type=channel_value_type, buffer_size=int(config["buffer_size"]), 
            id=config["id"], name=str(i), description=config["description"], restorer=restorer)
        ep = c.create_reader_writer_pair()
        services["channel_" + str(i)] =  c
        services["reader_" + str(i)] = ep["r"]
        services["writer_" + str(i)] = ep["w"]

    store_srs_file_path = config["store_srs_file"]
    def write_srs():
        if store_srs_file_path:
            with open(store_srs_file_path, mode="wt") as _:
                _.write(json.dumps(name_to_service_srs))

    if config["use_async"]:
        await serv.async_init_and_run_service(services, config["host"], config["port"], 
            serve_bootstrap=config["serve_bootstrap"], restorer=restorer, name_to_service_srs=name_to_service_srs,
            run_before_enter_eventloop=write_srs)
    else:
        
        serv.init_and_run_service(services, config["host"], config["port"], 
            serve_bootstrap=config["serve_bootstrap"], restorer=restorer, name_to_service_srs=name_to_service_srs,
            run_before_enter_eventloop=write_srs)

#------------------------------------------------------------------------------

if __name__ == '__main__':
    asyncio.run(main(no_of_channels=2, buffer_size=2, serve_bootstrap=True, use_async=True)) 



