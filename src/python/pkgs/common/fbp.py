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

# remote debugging via commandline
# -m ptvsd --host 0.0.0.0 --port 14000 --wait

import asyncio
import capnp
from collections import deque, defaultdict
import json
import os
from pathlib import Path
import sys
import uuid

PATH_TO_REPO = Path(os.path.realpath(__file__)).parent.parent.parent.parent.parent
if str(PATH_TO_REPO) not in sys.path:
    sys.path.insert(1, str(PATH_TO_REPO))

PATH_TO_PYTHON_CODE = PATH_TO_REPO / "src/python"
if str(PATH_TO_PYTHON_CODE) not in sys.path:
    sys.path.insert(1, str(PATH_TO_PYTHON_CODE))

from pkgs.common import common
from pkgs.common import service as serv

PATH_TO_CAPNP_SCHEMAS = PATH_TO_REPO / "capnproto_schemas"
abs_imports = [str(PATH_TO_CAPNP_SCHEMAS)]
common_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "common.capnp"), imports=abs_imports)
fbp_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "fbp.capnp"), imports=abs_imports)
reg_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "registry.capnp"), imports=abs_imports)


def connect_ports(config: dict, connection_manager=None):
    if not connection_manager:
        connection_manager = common.ConnectionManager()
    ports = {}
    for k, v in config.items():
        if k.endswith("in_sr"):
            port_name = k[:-6]
            if len(port_name) == 0:
                port_name = "in"
            elif port_name[:-1] == "_":
                port_name = port_name[:-1]
            if v is None:
                ports[port_name] = None
            else:
                ports[port_name] = connection_manager.try_connect(v, cast_as=fbp_capnp.Channel.Reader, retry_secs=1)
        elif k.endswith("out_sr"):
            port_name = k[:-7]
            if len(port_name) == 0:
                port_name = "out"
            elif port_name[:-1] == "_":
                port_name = port_name[:-1]
            if v is None:
                ports[port_name] = None
            else:
                ports[port_name] = connection_manager.try_connect(v, cast_as=fbp_capnp.Channel.Writer, retry_secs=1)
        elif k.endswith("out_srs"):
            port_name = k[:-8]
            if len(port_name) == 0:
                port_name = "out"
            elif port_name[:-1] == "_":
                port_name = port_name[:-1]
            if v is None:
                ports[port_name] = None
            else:
                ports[port_name] = []
                for out_sr in v.split("|"):
                    ports[port_name].append(connection_manager.try_connect(out_sr, cast_as=fbp_capnp.Channel.Writer,
                                                                       retry_secs=1))
    return ports


class Channel(fbp_capnp.Channel.Server, common.Identifiable, common.Persistable, serv.AdministrableService):

    def __init__(self, buffer_size=1, auto_close_semantics="fbp",
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
        self._auto_close_semantics = auto_close_semantics

    def shutdown(self):
        print("Channel::shutdown")
        # capnp.reset_event_loop()
        exit(0)

    def closed_reader(self, reader):
        print("Channel::closed_reader")
        self._readers.remove(reader)
        # if self._auto_close_semantics == "fbp" and len(self._readers) == 0:
        # capnp.getTimer().after_delay(1*10**9).then(lambda: self.shutdown())
        # Timer(1, self.shutdown).start()

    def closed_writer(self, writer):
        print("Channel::closed_writer")
        self._writers.remove(writer)
        if self._auto_close_semantics == "fbp" and len(self._writers) == 0:
            for r in self._readers:
                r.send_close_on_empty_buffer = True
            # as we just received a done message which should be distributed and would
            # fill the buffer, unblock all readers, so they send the done message
            while len(self._blocking_read_fulfillers) > 0:
                self._blocking_read_fulfillers.popleft().fulfill()

    def setBufferSize_context(self, context):  # setBufferSize @0 (size :UInt64);
        self._buffer_size = max(1, context.params.size)

    def reader_context(self, context):  # reader @1 () -> (r :Reader);
        r = self.create_reader()
        context.results.r = r

    def writer_context(self, context):  # writer @2 () -> (w :Writer);
        w = self.create_writer()
        context.results.w = w

    def create_reader(self):
        r = Reader(self)
        self._readers.append(r)
        return r

    def create_writer(self):
        w = Writer(self)
        self._writers.append(w)
        return w

    def create_reader_writer_pair(self):
        r = self.create_reader()
        w = self.create_writer()
        return {"r": r, "w": w}

    def endpoints_context(self, context):  # endpoints @1 () -> (r :Reader, w :Writer);
        ep = self.create_reader_writer_pair()
        context.results.r = ep["r"]
        context.results.w = ep["w"]

    def setAutoCloseSemantics_context(self, context):  # setAutoCloseSemantics @4 (cs :CloseSemantics);
        self._auto_close_semantics = context.params.cs

    def close_context(self, context):  # close @5 (waitForEmptyBuffer :Bool = true);
        # make new writes refused
        for w in self._writers:
            w.closed = True
        # forget writers
        self._writers.clear()

        if context.params.waitForEmptyBuffer:
            # cancel blocking writers
            for paf in self._blocking_write_fulfillers:
                paf.promise.cancel()
            for r in self._readers:
                r.send_close_on_empty_buffer = True
            while len(self._blocking_read_fulfillers) > 0:
                self._blocking_read_fulfillers.popleft().fulfill()
        else:
            # close all readers
            for r in self._readers:
                r.closed = True
            # and forget them
            self._readers.clear()
            # return capnp.getTimer().after_delay(1*10**9).then(lambda: self.shutdown())
            self.shutdown()


class Reader(fbp_capnp.Channel.Reader.Server):

    def __init__(self, channel):
        self._channel = channel
        self._send_close_on_empty_buffer = False
        self._closed = False

    @property
    def closed(self):
        return self._closed

    @closed.setter
    def closed(self, v):
        self._closed = v

    @property
    def send_close_on_empty_buffer(self):
        return self._send_close_on_empty_buffer

    @send_close_on_empty_buffer.setter
    def send_close_on_empty_buffer(self, v):
        self._send_close_on_empty_buffer = v

    def read_context(self, context):  # read @0 () -> (value :V);
        if self.closed:
            raise Exception("Reader closed")

        c = self._channel
        b = c._buffer

        def set_results_from_buffer():
            if self.closed:
                raise Exception("Reader closed")

            if self.send_close_on_empty_buffer and len(b) == 0:
                context.results.done = None
                c.closed_reader(self)
            elif len(b) > 0:
                context.results.value = fbp_capnp.Channel.Msg.from_bytes(b.popleft()).value

        # read value non-blocking
        if len(b) > 0:
            set_results_from_buffer()
            # print(c.name, "r ", sep="", end="")

            # unblock writers unless we're about to close down
            if len(b) == 0 and not self.send_close_on_empty_buffer:
                # unblock potentially waiting writers
                while len(b) < c._buffer_size and len(c._blocking_write_fulfillers) > 0:
                    c._blocking_write_fulfillers.popleft().fulfill()

        else:  # block because no value to read
            # if the channel is supposed to close down, just generate a close message
            if self.send_close_on_empty_buffer:
                print("Reader::read_context -> len(b) == 0 -> send done")
                set_results_from_buffer()

                # as the buffer is empty and we're supposed to shut down as any other reader
                # fulfill waiting readers with done messages
                while len(c._blocking_read_fulfillers) > 0:
                    c._blocking_read_fulfillers.popleft().fulfill()
            else:
                paf = capnp.PromiseFulfillerPair()
                c._blocking_read_fulfillers.append(paf)

                # print("[", c.name, "r"+str(len(c._blocking_read_fulfillers))+"] ", sep="", end="")
                return paf.promise.then(set_results_from_buffer)

    def close_context(self, context):  # close @1 ();
        if self.closed:
            raise Exception("Reader closed")

        self._channel.closed_reader(self)
        self.closed = True


class Writer(fbp_capnp.Channel.Writer.Server):

    def __init__(self, channel):
        self._channel = channel
        self._closed = False

    @property
    def closed(self):
        return self._closed

    @closed.setter
    def closed(self, v):
        self._closed = v

    def write_context(self, context):  # write @0 (value :V);
        if self.closed:
            raise Exception("Writer closed")

        v = context.params
        c = self._channel
        b = c._buffer

        def append_from_buffer():
            if self.closed:
                raise Exception("Writer closed")
            b.append(v.as_builder().to_bytes())

        # if we received a done, this writer can be removed
        if v.which() == "done":
            c.closed_writer(self)
            return

        # write value non-block
        if len(b) < c._buffer_size:
            append_from_buffer()
            # print(c.name, "w ", sep="", end="")
            # unblock potentially waiting readers
            while len(b) > 0 and len(c._blocking_read_fulfillers) > 0:
                c._blocking_read_fulfillers.popleft().fulfill()
        else:  # block until buffer has space
            paf = capnp.PromiseFulfillerPair()
            c._blocking_write_fulfillers.append(paf)
            # print("[", c.name, "w"+str(len(c._blocking_write_fulfillers))+"] ", sep="", end="")
            return paf.promise.then(append_from_buffer)

    def close_context(self, context):  # close @1 ();
        if self.closed:
            raise Exception("Writer closed")

        self._channel.closed_writer(self)
        self.closed = True


async def main(no_of_channels=1, buffer_size=1, serve_bootstrap=True, host=None, port=None,
               id=None, name="Channel Service", description=None, use_async=False):
    config = {
        "no_of_channels": str(no_of_channels),
        "buffer_size": str(max(1, buffer_size)),
        "reader_srts": "[[r1_1234,r2_1234]]",  # [[c1_r1,c1_r2,c1_r3],[c1_r1],...]
        "writer_srts": "[[w1_1234]]",  # [[c1_w1,c1_w2],[c2_w1],...]
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
            config[k] = v.lower() == "true" if v.lower() in ["true", "false"] else v
    print(config)

    channel_to_reader_srts = defaultdict(list)
    channel_to_writer_srts = defaultdict(list)
    if "reader_srts" in config:
        for i, srts in enumerate(json.loads(config["reader_srts"])):
            channel_to_reader_srts[i + 1] = srts
    if "writer_srts" in config:
        for i, srts in enumerate(json.loads(config["writer_srts"])):
            channel_to_writer_srts[i + 1] = srts

    restorer = common.Restorer()
    services = {}
    name_to_service_srs = {}
    for i in range(1, int(config["no_of_channels"]) + 1):
        c = Channel(buffer_size=int(config["buffer_size"]),
                    id=config["id"], name=str(i), description=config["description"], restorer=restorer)
        services["channel_" + str(i)] = c
        for k, srt in enumerate(channel_to_reader_srts.get(i, [None])):
            reader_name = "channel_" + str(i) + "_reader_" + str(k + 1)
            name_to_service_srs[reader_name] = srt
            services[reader_name] = c.create_reader()
        for k, srt in enumerate(channel_to_writer_srts.get(i, [None])):
            writer_name = "channel_" + str(i) + "_writer_" + str(k + 1)
            name_to_service_srs[writer_name] = srt
            services[writer_name] = c.create_writer()

    store_srs_file_path = config["store_srs_file"]

    def write_srs():
        if store_srs_file_path:
            with open(store_srs_file_path, mode="wt") as _:
                _.write(json.dumps(name_to_service_srs))

    if config["use_async"]:
        await serv.async_init_and_run_service(services, config["host"], config["port"],
                                              serve_bootstrap=config["serve_bootstrap"], restorer=restorer,
                                              name_to_service_srs=name_to_service_srs,
                                              run_before_enter_eventloop=write_srs)
    else:

        serv.init_and_run_service(services, config["host"], config["port"],
                                  serve_bootstrap=config["serve_bootstrap"], restorer=restorer,
                                  name_to_service_srs=name_to_service_srs,
                                  run_before_enter_eventloop=write_srs)


if __name__ == '__main__':
    asyncio.run(main(no_of_channels=1, buffer_size=1, serve_bootstrap=True, use_async=False))
