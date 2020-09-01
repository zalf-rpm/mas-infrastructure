#!/usr/bin/python
# -*- coding: UTF-8

# This Source Code Form is subject to the terms of the Mozilla Public
# License, v. 2.0. If a copy of the MPL was not distributed with this
# file, You can obtain one at http://mozilla.org/MPL/2.0/. */

# Authors:
# Michael Berg-Mohnicke <michael.berg-mohnicke@zalf.de>
#
# Maintainers:
# Currently maintained by the authors.
#
# This file has been created at the Institute of
# Landscape Systems Analysis at the ZALF.
# Copyright (C: Leibniz Centre for Agricultural Landscape Research (ZALF)


import asyncio
import os
from pathlib import Path
import socket
import sys
import time

PATH_TO_REPO = Path(os.path.realpath(__file__)).parent.parent.parent.parent.parent.parent
if str(PATH_TO_REPO) not in sys.path:
    sys.path.insert(1, str(PATH_TO_REPO))

import capnp
import a_capnp

class A_Impl(a_capnp.A.Server):

    def method(self, param, **kwargs):
        time.sleep(1.5)
        return "_______________method_RESULT___________________"


class Server:
    def __init__(self, service):
        self._service = service

    async def myreader(self):
        to_count = 0
        while self.retry:
            try:
                # Must be a wait_for so we don't block on read()
                data = await asyncio.wait_for(
                    self.reader.read(4096),
                    timeout=1#0.1
                )
                print("R<<<<<<<<", data)
            except asyncio.TimeoutError:
                #logger.debug("myreader timeout.")
                to_count += 1
                print("R" + str(to_count), end=" ", flush=True)
                continue
            except Exception as err:
                print("Unknown myreader err:", err)
                return False
            await self.server.write(data)
        #logger.debug("myreader done.")
        print("!!!!!! myreader done.")
        return True

    async def server_read(self):
        print("before", flush=True)
        data = await self.server.read(4096)
        print("after, data:", data, flush=True)
        return data

    async def mywriter(self):
        to_count = 0
        while self.retry:
            try:
                # Must be a wait_for so we don't block on read()
                print("?", end="", flush=True)
                #data = await asyncio.wait_for(
                #    self.server.read(4096),
                #    timeout=2#0.1
                #)
                data = await asyncio.wait_for(
                    self.server_read(),
                    timeout=1#0.1
                )
                print("W>>>>>>>>", data.tobytes())
                self.writer.write(data.tobytes())
                await self.writer.drain()
            except asyncio.TimeoutError:
                #logger.debug("mywriter timeout.")
                to_count += 1
                print("W" + str(to_count), end=" ", flush=True)
                continue
            except Exception as err:
                print("Unknown mywriter err:", err)
                return False
        #logger.debug("mywriter done.")
        print("!!!!!!!! mywriter done.")
        return True


    async def myserver(self, reader, writer):
        # Start TwoPartyServer using TwoWayPipe (only requires bootstrap)
        self.server = capnp.TwoPartyServer(bootstrap=self._service)
        self.reader = reader
        self.writer = writer
        self.retry = True

        # Assemble reader and writer tasks, run in the background
        coroutines = [self.myreader(), self.mywriter()]
        tasks = asyncio.gather(*coroutines, return_exceptions=True)

        while True:
            self.server.poll_once()
            # Check to see if reader has been sent an eof (disconnect)
            if self.reader.at_eof():
                print("R<<<<<<<<<<<<<< ------- END OF FILE --------", flush=True)
                self.retry = False
                break
            await asyncio.sleep(0.1)#0.01)

        # Make wait for reader/writer to finish (prevent possible resource leaks)
        await tasks


async def async_main():

    server = "0.0.0.0"
    port = 11111

    async def new_connection(reader, writer):
        server = Server(A_Impl())
        await server.myserver(reader, writer)

    # Handle both IPv4 and IPv6 cases
    try:
        print("Try IPv4")
        server = await asyncio.start_server(
            new_connection,
            server, port,
            family=socket.AF_INET
        )
    except Exception:
        print("Try IPv6")
        server = await asyncio.start_server(
            new_connection,
            server, port,
            family=socket.AF_INET6
        )

    async with server:
        await server.serve_forever()


def no_async_main():
    server = capnp.TwoPartyServer("*:11111", bootstrap=A_Impl())
    server.run_forever()


if __name__ == '__main__':

    mode = sys.argv[1] if len(sys.argv) > 1 else "client"
    if mode == "client":
        a_cap = capnp.TwoPartyClient("localhost:11111").bootstrap().cast_as(a_capnp.A)
        txt = a_cap.method("______________PARAM______________").wait().res
        print(txt)
    else:
        asyncio.run(async_main())