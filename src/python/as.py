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
import time

PATH_TO_REPO = Path(os.path.realpath(__file__)).parent.parent.parent
if str(PATH_TO_REPO) not in sys.path:
    sys.path.insert(1, str(PATH_TO_REPO))

#import capnp_async_helpers as async_helpers
#import common.python.capnp_async_helpers as async_helpers

import capnp
import capnproto_schemas.a_capnp as a_capnp

class A_Impl(a_capnp.A.Server):

    async def sleep(self, secs, pfp):
        time.sleep(secs)
        pfp.fulfill()

    def method_context(self, context, **kwargs):
        pfp = capnp.PromiseFulfillerPair()
        pfp.fulfill()
        asyncio.create_task(self.sleep(3, pfp))
        #task.add_done_callback(lambda res: context.results.res = "___RESULT____")
        #time.sleep(3)
        context.results.res = "_______________method_RESULT___________________"
        return pfp.promise

    def method2(self, param, **kwargs):
        time.sleep(3)
        return "_______________method_RESULT___________________"


""
class Server:
    def __init__(self, service):
        self._service = service

    async def myreader(self):
        while self.retry:
            try:
                # Must be a wait_for so we don't block on read()
                data = await asyncio.wait_for(
                    self.reader.read(4096),
                    timeout=0.1
                )
                await self.server.write(data)
            except asyncio.TimeoutError:
                print("myreader timeout.")
                continue
            except Exception as err:
                print("Unknown myreader err:", err)
                self.retry = False
                return False
            #await self.server.write(data)
        print("myreader done.")
        return True


    async def mywriter(self):
        while self.retry or self.writer.at_eof():
            try:
                # Must be a wait_for so we don't block on read()
                data = await asyncio.wait_for(
                    self.server.read(4096),
                    timeout=0.1
                )
                self.writer.write(data.tobytes())
                await self.writer.drain()
            except asyncio.TimeoutError:
                print("mywriter timeout.")
                continue
            except Exception as err:
                print("Unknown mywriter err:", err)
                self.retry = False
                return False
        print("mywriter done.")
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
                self.retry = False
                break
            await asyncio.sleep(0.01)

        # Make wait for reader/writer to finish (prevent possible resource leaks)
        await tasks
""

async def async_main():

    server = "0.0.0.0"
    port = 11111

    async def new_connection(reader, writer):
        #server = async_helpers.Server()#A_Impl())
        #await server.myserver(capnp.TwoPartyServer(bootstrap=A_Impl()), reader, writer)
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
    #no_async_main()
    asyncio.run(async_main())