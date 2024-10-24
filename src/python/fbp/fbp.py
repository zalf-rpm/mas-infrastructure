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

import capnp
from collections import deque
import os
from pathlib import Path
import sys
import time
import uuid

PATH_TO_SCRIPT_DIR = Path(os.path.realpath(__file__)).parent
PATH_TO_REPO = PATH_TO_SCRIPT_DIR.parent.parent.parent
if str(PATH_TO_REPO) not in sys.path:
    sys.path.insert(1, str(PATH_TO_REPO))

PATH_TO_PYTHON_CODE = PATH_TO_REPO / "src/python"
if str(PATH_TO_PYTHON_CODE) not in sys.path:
    sys.path.insert(1, str(PATH_TO_PYTHON_CODE))

from pkgs.common import common
from pkgs.common import capnp_async_helpers as async_helpers

PATH_TO_CAPNP_SCHEMAS = (PATH_TO_REPO / "capnproto_schemas").resolve()
abs_imports = [str(PATH_TO_CAPNP_SCHEMAS)]
fbp_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "fbp.capnp"), imports=abs_imports)
common_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "common.capnp"), imports=abs_imports)


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


    def get_reader(self):
        return Reader(self, self._restorer)


    def get_writer(self):
        return Writer(self, self._restorer)


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


    def close_context(self, context): #close 	@1 ();
        return self._component.stop()


class Reader(fbp_capnp.Input.Reader.Server, common.Persistable): 

    def __init__(self, input, restorer=None):
        common.Persistable.__init__(self, restorer)

        self._input = input


    def read_context(self, context): # read @0 () -> (value :AnyPointer);
        i = self._input
        b = i._buffer
        # read value non-blocking
        if len(b) > 0:
            context.results.value = i.deserialize(b.popleft())
            #print(c.name, "r ", sep="", end="")
            # unblock potentially waiting writers
            while len(b) < i._buffer_size and len(i._blocking_write_fulfillers) > 0:
                i._blocking_write_fulfillers.popleft().fulfill()
        else: # block because no value to read
            paf = capnp.PromiseFulfillerPair()
            i._blocking_read_fulfiller = paf 
            #print("[", c.name, "r"+str(len(c._blocking_read_fulfillers))+"] ", sep="", end="")
            return paf.promise.then(lambda: setattr(context.results, "value", i.deserialize(b.popleft())))


class Writer(fbp_capnp.Input.Writer.Server, common.Persistable):

    def __init__(self, input, restorer=None):
        common.Persistable.__init__(self, restorer)

        self._input = input

    def write_context(self, context): # write @0 (value :AnyPointer);
        v = context.params.value
        i = self._input
        b = i._buffer
        # write value non-block
        if len(b) < i._buffer_size:
            b.append(i.serialize(v))
            #print(c.name, "w ", sep="", end="")
            # unblock potentially waiting readers
            while len(b) > 0 and i._blocking_read_fulfiller is not None:
                ff = i._blocking_read_fulfiller
                i._blocking_read_fulfiller = None
                ff.fulfill()
        else: # block until buffer has space
            paf = capnp.PromiseFulfillerPair()
            i._blocking_write_fulfillers.append(paf)
            #print("[", c.name, "w"+str(len(c._blocking_write_fulfillers))+"] ", sep="", end="")
            return paf.promise.then(lambda: b.append(i.serialize(v)))


def start_component_thread(sock, bootstrap):
    server = capnp.TwoPartyServer(sock, bootstrap=bootstrap)
    bootstrap.run()
    #server.run_forever()


class Component(fbp_capnp.Component.Server):#, common.Persistable): 

    def __init__(self, in_ports={}, out_ports=[]):
        self._in_ports = {name : {"port": None, "optional": opt} for name, opt in in_ports.items()} 
        self._out_ports = {name : {"port": None, "connected": False} for name in out_ports}
        self._stop = False
        self._ports_setup_complete = False


    def setupPorts_context(self, context): # setupPorts @0 (inPorts :List(NameToPort), outPorts :List(NameToPort));
        mandatory_in_ports = []
        for name, port_data in self._in_ports.items():
            if not port_data["optional"]:
                mandatory_in_ports.append(name)
        for n2p in context.params.inPorts:
            self._in_ports[n2p.name]["port"] = n2p.port.as_interface(fbp_capnp.Input.Reader)
            mandatory_in_ports.remove(n2p.name)
        
        for n2p in context.params.outPorts:
            if n2p.name in self._out_ports:
                op = self._out_ports[n2p.name]
                op["port"] = n2p.port.as_interface(fbp_capnp.Input.Writer)
                op["connected"] = True

        if len(mandatory_in_ports) > 0:
            raise Exception("Mandatory inport ports not connected: " + str(mandatory_in_ports))
        
        self._ports_setup_complete = True


    def stop_context(self, context): # stop @2 ();
        self._stop = True


    # execute the actual components behavior
    def execute():
        pass


    def run(self):
        try:
            # wait for setup to complete
            while not self._ports_setup_complete and not self.stop:
                time.sleep(0.01)
                capnp.poll_once()

            if not self.stop:
                self.execute()
        except Exception as e:
            print(e)


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


def init_and_run_fbp_component(component_cap, name_to_in_ports={}, name_to_out_ports={}, 
    host="*", port=None, serve_bootstrap=True, restorer=None, 
    conman=None, run_before_enter_eventloop=None, eventloop_wait_forever=True):

    host = host if host else "*"

    if not conman:
        conman = common.ConnectionManager()
    if not restorer:
        restorer = common.Restorer()

    def connect_ports(side, name_to_ports, port_type):
        for name, data in name_to_ports.items():
            # only connect via sr if port is not already containing a capability
            if "sr" in data and data["port"] is None:
                try:
                    print("trying to connect", side, "port:", name, " via sr:", data["sr"])
                    data["port"] = conman.try_connect(data["sr"], cast_as=port_type, retry_secs=1)
                except Exception as e:
                    print("Error connecting to", side, "port:", name, ". Exception:", e)

    addr = host + ((":" + str(port)) if port else "")
    if serve_bootstrap:
        server = capnp.TwoPartyServer(addr, bootstrap=restorer)
        restorer.port = port if port else server.port

        connect_ports("in", name_to_in_ports, common_capnp.Reader)
        connect_ports("out", name_to_out_ports, common_capnp.Writer)
    else:
        connect_ports("in", name_to_in_ports, common_capnp.Reader)
        connect_ports("out", name_to_out_ports, common_capnp.Writer)

        if run_before_enter_eventloop:
            run_before_enter_eventloop()
        if eventloop_wait_forever:
            capnp.wait_forever()
    
    ips = []
    for name, pd in name_to_in_ports.items():
        ips.append({"name": name, "port": pd["port"]})
    ops = []
    for name, pd in name_to_out_ports.items():
        ops.append({"name": name, "port": pd["port"]})

    component_cap.setupPorts(inPorts=ips, outPorts=ops).wait()

    if run_before_enter_eventloop:
        run_before_enter_eventloop()
    if eventloop_wait_forever:
        server.run_forever()
