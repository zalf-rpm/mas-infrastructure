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
from multiprocessing import connection
import capnp
from collections import deque, defaultdict
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

#if str(PATH_TO_SCRIPT_DIR) in sys.path:
    
#else:

import common.common as common
import common.service as serv
import common.capnp_async_helpers as async_helpers

PATH_TO_CAPNP_SCHEMAS = PATH_TO_REPO / "capnproto_schemas"
abs_imports = [str(PATH_TO_CAPNP_SCHEMAS)]
fbp_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "fbp.capnp"), imports=abs_imports)


#------------------------------------------------------------------------------

class Input(fbp_capnp.Input.Server, common.Identifiable, common.Persistable): 

    def __init__(self, component, data_type="Text", buffer_size=1, id=None, name=None, description=None, restorer=None):
        common.Identifiable.__init__(self, id, name, description)
        common.Persistable.__init__(self, restorer)

        self._component = component
        self._id = str(id if id else uuid.uuid4())
        self._name = name if name else self._id
        self._description = description if description else ""
        self._buffer = deque()
        self._buffer_size = buffer_size
        self._blocking_read_fulfiller = None
        self._blocking_write_fulfillers = deque()
        self._data_type = data_type

    def read_value(self):
        b = self._buffer
        # read value non-blocking
        if len(b) > 0:
            val = self.deserialize(b.popleft())
            #print(c.name, "r ", sep="", end="")
            # unblock potentially waiting writers
            while len(b) < self._buffer_size and len(self._blocking_write_fulfillers) > 0:
                self._blocking_write_fulfillers.popleft().fulfill()
            return capnp.Promise().then(lambda: val)
        else: # block because no value to read
            paf = capnp.PromiseFulfillerPair()
            self._blocking_read_fulfiller = paf 
            #print("[", c.name, "r"+str(len(c._blocking_read_fulfillers))+"] ", sep="", end="")
            return paf.promise.then(lambda: self.deserialize(b.popleft()))

    def deserialize(self, v):
        if self._data_type == "Text":
            return v
        elif isinstance(self._data_type, capnp.lib.capnp._StructModule):
            return self._data_type.from_bytes(v)


    def serialize(self, v):
        if self._data_type == "Text":
            return v.as_text()
        elif isinstance(self._data_type, capnp.lib.capnp._StructModule):
            return v.as_struct(self._data_type).as_builder().to_bytes()


    def setBufferSize_context(self, context): # setBufferSize @0 (size :UInt64);
        self._buffer_size = max(1, context.params.size)


    def _send_context(self, context): # send 	@0 (data :AnyPointer); 
        v = context.params.data
        b = self._buffer
        # write value non-block
        if len(b) < self._buffer_size:
            b.append(self.serialize(v))
            #print(c.name, "w ", sep="", end="")
            # unblock potentially waiting readers
            while len(b) > 0 and self._next_value_fulfiller is not None:
                ff = self._blocking_read_fulfiller
                self._blocking_read_fulfiller = None
                ff.fulfill()
        else: # block until buffer has space
            paf = capnp.PromiseFulfillerPair()
            self._blocking_write_fulfillers.append(paf)
            #print("[", c.name, "w"+str(len(c._blocking_write_fulfillers))+"] ", sep="", end="")
            return paf.promise.then(lambda: b.append(self.serialize(v)))

    def send_context(self, context): # send 	@0 (data :AnyPointer); 
        v = context.params.data
        b = self._buffer
        # write value non-block
        if len(b) < self._buffer_size:
            b.append(self.serialize(v))
            self._component.run()
            #print(c.name, "w ", sep="", end="")
            # unblock potentially waiting readers
            #while len(b) > 0 and self._next_value_fulfiller is not None:
            #    ff = self._blocking_read_fulfiller
            #    self._blocking_read_fulfiller = None
            #    ff.fulfill()
        else: # block until buffer has space
            paf = capnp.PromiseFulfillerPair()
            self._blocking_write_fulfillers.append(paf)
            #print("[", c.name, "w"+str(len(c._blocking_write_fulfillers))+"] ", sep="", end="")
            return paf.promise.then(lambda: b.append(self.serialize(v)))



    def close_context(self, context): #close 	@1 ();
        self._component._stop = True

#--------------------------------------------------------------------------------------

class Component(common.Persistable): 

    def __init__(self, in_ports={}, out_ports={}, restorer=None):
        common.Persistable.__init__(self, restorer)

        self._in_ports = defaultdict(lambda: {"port": None, "data_type": None, "sr_token": None})
        for name, data in in_ports.items():
            data_type, buffer_size = data if isinstance(data, type(())) else (data, 1)
            self._in_ports[name]["port"] = Input(self, data_type=data_type, buffer_size=buffer_size, name=name, restorer=restorer)
            self._in_ports[name]["data_type"] = data_type
        
        self._out_ports = defaultdict(lambda: {"port": None, "data_type": None, "sr": None})
        for name, data_type in out_ports.items():
            self._out_ports[name]["data_type"] = data_type
        
        self._stop = False


    @property
    def in_ports(self):
        return self._in_ports


    @property
    def out_ports(self):
        return self._out_ports


    @property
    def stop(self):
        return self._stop

    @stop.setter
    def stop(self, s):
        self._stop = s


#--------------------------------------------------------------------------------------

class Config(fbp_capnp.Config.Server, common.Persistable): 

    def __init__(self, connection_manager, in_port_config={}, out_port_config={}, restorer=None):
        common.Persistable.__init__(self, restorer)
        self._conman = connection_manager
        self._component = self
        self._in_ports = {}
        for name, (data_type, buffer_size) in in_port_config.items():
            self._in_ports[name] = {"port": Input(self, data_type, buffer_size, restorer=restorer)}
        self._out_ports = {}
        for name, data_type in out_port_config.items():
            self._out_ports[name] = {"port": None, "data_type": data_type}


    def setup_context(self, context): # setup @0 (config :List(NameToSR));
        c = context.params.config

        for name, sr in c.items():
            if name in self._out_ports:
                self._out_ports[name]["port"] = self._conman.try_connect(sr, cast_as=fbp_capnp.Input)

#------------------------------------------------------------------------------

async def async_init_and_run_fbp_component(name_to_in_ports={}, name_to_out_ports={}, 
    host=None, port=0, serve_bootstrap=True, restorer=None, 
    conman=None, run_before_enter_eventloop=None, eventloop_wait_forever=True):

    port = port if port else 0

    if not conman:
        conman = async_helpers.ConnectionManager()
    if not restorer:
        restorer = common.Restorer()

    async def connect_out_ports():
        for name, data in name_to_out_ports.items():
            try:
                print("trying to connect out port:", name, " via sr:", data["sr"])
                data["port"] = await conman.try_connect(data["sr"], cast_as=fbp_capnp.Input, retry_secs=1)
            except Exception as e:
                print("Error connecting to out port:", name, ". Exception:", e)

    if serve_bootstrap:
        server = await async_helpers.serve(host, port, restorer)
         
         # make input ports available via restorer
        for name, data in name_to_in_ports.items():
            if data["port"] and data["sr_token"]:
                port_sr, _ = restorer.save(data["port"], data["sr_token"])
            print("in_port:", name, "sr:", port_sr)
        print("restorer_sr:", restorer.sturdy_ref())

        await connect_out_ports()

        if run_before_enter_eventloop:
            run_before_enter_eventloop()
        if eventloop_wait_forever:
            async with server:
                await server.serve_forever()
    else:
        await connect_out_ports()
        if run_before_enter_eventloop:
            run_before_enter_eventloop()
        if eventloop_wait_forever:
            await conman.manage_forever()

#--------------------------------------------------------------------------------------------

def init_and_run_fbp_component(name_to_in_ports={}, name_to_out_ports={}, 
    host="*", port=None, serve_bootstrap=True, restorer=None, 
    conman=None, run_before_enter_eventloop=None, eventloop_wait_forever=True):

    host = host if host else "*"

    if not conman:
        conman = common.ConnectionManager()
    if not restorer:
        restorer = common.Restorer()

    def connect_out_ports():
        for name, data in name_to_out_ports.items():
            try:
                print("trying to connect out port:", name, " via sr:", data["sr"])
                data["port"] = conman.try_connect(data["sr"], cast_as=fbp_capnp.Input, retry_secs=1)
            except Exception as e:
                print("Error connecting to out port:", name, ". Exception:", e)

    addr = host + ((":" + str(port)) if port else "")
    if serve_bootstrap:
        server = capnp.TwoPartyServer(addr, bootstrap=restorer)
        restorer.port = port if port else server.port

        # make input ports available via restorer
        for name, data in name_to_in_ports.items():
            if data["port"] and data["sr_token"]:
                port_sr, _ = restorer.save(data["port"], data["sr_token"])
                print("in_port:", name, "sr:", port_sr)
        print("restorer_sr:", restorer.sturdy_ref())

        connect_out_ports()
    else:
        connect_out_ports()
        if run_before_enter_eventloop:
            run_before_enter_eventloop()
        if eventloop_wait_forever:
            capnp.wait_forever()
    
    if run_before_enter_eventloop:
        run_before_enter_eventloop()
    if eventloop_wait_forever:
        server.run_forever()
