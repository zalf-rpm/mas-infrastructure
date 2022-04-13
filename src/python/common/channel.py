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

    def __init__(self, channel_value_type="Text", buffer_size=1, fbp_close_semantics=True,
        id=None, name=None, description=None, admin=None, restorer=None):
        common.Identifiable.__init__(self, id, name, description)
        common.Persistable.__init__(self, restorer)
        serv.AdministrableService.__init__(self, admin)

        self._id = str(id if id else uuid.uuid4())
        self._name = name if name else self._id
        self._description = description if description else ""
        self._buffer = deque()
        self._buffer_size = buffer_size
        self._readers = []
        self._writers = []
        self._blocking_read_fulfillers = deque()
        self._blocking_write_fulfillers = deque()
        self._channel_value_type = channel_value_type
        self._fbp_close_semantics=fbp_close_semantics
        self._check_for_close = False


    def closed_reader(self, reader):
        self._readers.remove(reader)


    def closed_writer(self, writer):
        print("Channel::closed_writer")
        self._writers.remove(writer)
        self._check_for_close = True
        return self.check_for_close()


    def check_for_close(self):
        print("Channel::check_for_close")
        if self._check_for_close and len(self._writers) == 0 and len(self._buffer) == 0:
            print("Channel::check_for_close -> in")
            proms = []
            for r in self._readers.copy():
                self._readers.remove(r)
                proms.append(r.close())
            self._check_for_close = False
            return capnp.join_promises(proms)


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


    def reader_context(self, context): # reader @1 () -> (r :Reader);
        r = Reader(self)
        self._readers.append(r)
        context.results.r = r


    def writer_context(self, context): # writer @2 () -> (w :Writer);
        w = Writer(self)
        self._writers.append(w)
        context.results.w = w


    def create_reader_writer_pair(self):
        r = Reader(self)
        self._readers.append(r)
        w = Writer(self)
        self._writers.append(w)
        return {"r": r, "w": w}


    def endpoints_context(self, context): # endpoints @1 () -> (r :Reader, w :Writer);
        ep = self.create_reader_writer_pair()
        context.results.r = ep["r"]
        context.results.w = ep["w"]

#------------------------------------------------------------------------------

class Reader(common_capnp.Reader.Server): 

    def __init__(self, channel):
        self._channel = channel
        self._close_notification_actions = []

    def read_context(self, context): # read @0 () -> (value :V);
        c = self._channel
        b = c._buffer
        # read value non-blocking
        if len(b) > 0:
            context.results.value = c.deserialize(b.popleft())
            #print(c.name, "r ", sep="", end="")

            # run check_check_for close if we are supposed to shut down and buffer is empty
            if len(b) == 0 and c._check_for_close:
                print("Reader::read_context -> len(b)> 0 -> c.check_for_close")
                return c.check_for_close()

            # unblock potentially waiting writers
            while len(b) < c._buffer_size and len(c._blocking_write_fulfillers) > 0:
                c._blocking_write_fulfillers.popleft().fulfill()

        else: # block because no value to read
            # if we actually should close down if buffer is empty, no need to block 
            if c._check_for_close:
                print("Reader::read_context -> len(b) == 0 -> c.check_for_close")
                return c.check_for_close()

            paf = capnp.PromiseFulfillerPair()
            c._blocking_read_fulfillers.append(paf) 
            #print("[", c.name, "r"+str(len(c._blocking_read_fulfillers))+"] ", sep="", end="")
            return paf.promise.then(lambda: setattr(context.results, "value", c.deserialize(b.popleft())))


    def close(self):
        print("Reader::close")
        proms = []
        for a in self._close_notification_actions:
            proms.append(a.do())
        return capnp.join_promises(proms)


    def done_context(self, context): # done @1 ();
        return self._channel.closed_reader(self)


    def registerCloseNotification_context(self, context): # registerCloseNotification @2 (notification :Action);
        self._close_notification_actions.append(context.params.notification)

#------------------------------------------------------------------------------

class Writer(common_capnp.Writer.Server): 

    def __init__(self, channel):
        self._channel = channel
        self._close_notification_actions = []

    def write_context(self, context): # write @0 (value :V);
        v = context.params.value
        c = self._channel
        b = c._buffer
        # write value non-block
        if len(b) < c._buffer_size:
            b.append(c.serialize(v))
            #print(c.name, "w ", sep="", end="")
            # unblock potentially waiting readers
            while len(b) > 0 and len(c._blocking_read_fulfillers) > 0:
                c._blocking_read_fulfillers.popleft().fulfill()
        else: # block until buffer has space
            paf = capnp.PromiseFulfillerPair()
            c._blocking_write_fulfillers.append(paf)
            #print("[", c.name, "w"+str(len(c._blocking_write_fulfillers))+"] ", sep="", end="")
            return paf.promise.then(lambda: b.append(c.serialize(v)))


    def close(self):
        proms = []
        for a in self._close_notification_actions:
            proms.append(a.do())
        return capnp.join_promises(proms)


    def done_context(self, context): # done  @1 ();
        print("Writer::done_context")
        return self._channel.closed_writer(self)


    def registerCloseNotification_context(self, context): # registerCloseNotification @2 (notification :Action);
        self._close_notification_actions.append(context.params.notification)


#------------------------------------------------------------------------------

async def main(no_of_channels = 1, buffer_size=1, serve_bootstrap=True, host=None, port=None, 
    id=None, name="Channel Service", description=None, use_async=False):

    config = {
        "no_of_channels": str(no_of_channels),
        "buffer_size": str(max(1, buffer_size)),
        "type_1": "/home/berg/GitHub/mas-infrastructure/capnproto_schemas/model/monica/monica_state.capnp:ICData",
        "type_2": "/home/berg/GitHub/mas-infrastructure/capnproto_schemas/model/monica/monica_state.capnp:ICData",
        "rw_srts_1": "r1234|w1234", # predefined sturdy ref tokens for reader and writer
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

    channel_to_type = common.load_capnp_modules(
        {int(k.split("_")[1]) : v for k, v in config.items() if k.startswith("type_")})
    channel_to_sr_tokens = {int(k.split("_")[2]) : {"r": v.split("_")[0], "w": v.split("_")[1]} for k, v in config.items() if k.startswith("rw_srts_")}

    restorer = common.Restorer()
    services = {}
    name_to_service_srs = {}
    for i in range(1, int(config["no_of_channels"])+1):
        channel_value_type = channel_to_type.get(i, "Text")
        c = Channel(channel_value_type=channel_value_type, buffer_size=int(config["buffer_size"]), 
            id=config["id"], name=str(i), description=config["description"], restorer=restorer)
        ep = c.create_reader_writer_pair()
        services["channel_" + str(i)] =  c
        if i in channel_to_sr_tokens:
            name_to_service_srs["reader_" + str(i)] = channel_to_sr_tokens[i]["r"]
            name_to_service_srs["writer_" + str(i)] = channel_to_sr_tokens[i]["w"]
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
    asyncio.run(main(no_of_channels=1, buffer_size=1, serve_bootstrap=True, use_async=False)) 



