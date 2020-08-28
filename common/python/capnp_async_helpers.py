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

# code adapted from: https://github.com/capnproto/pycapnp/blob/master/examples/async_calculator_server.py

import argparse
import asyncio
import logging
import socket


import capnp


logger = logging.getLogger(__name__)
logger.setLevel(logging.DEBUG)


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
                logger.debug("myreader timeout.")
                to_count += 1
                print("R" + str(to_count), end=" ", flush=True)
                continue
            except Exception as err:
                logger.error("Unknown myreader err: %s", err)
                return False
            await self.server.write(data)
        logger.debug("myreader done.")
        print("[[[[myreader done.]]]]")
        return True

    async def server_read(self):
        print("before")
        data = await self.server.read(4096)
        print("after, data:", data)
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
                    timeout=0.1#0.1
                )
                print("W>>>>>>>>", data.tobytes())
                self.writer.write(data.tobytes())
                await self.writer.drain()
            except asyncio.TimeoutError:
                logger.debug("mywriter timeout.")
                to_count += 1
                print("W" + str(to_count), end=" ", flush=True)
                continue
            except Exception as err:
                logger.error("Unknown mywriter err: %s", err)
                return False
        logger.debug("mywriter done.")
        print("[[[[mywriter done.]]]]")
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

#------------------------------------------------------------------------------

async def myreader(client, reader):
    while True:
        data = await reader.read(4096)
        client.write(data)


async def mywriter(client, writer):
    while True:
        data = await client.read(4096)
        writer.write(data.tobytes())
        await writer.drain()


async def connect_to_server(port, address="127.0.0.1"):
    # Handle both IPv4 and IPv6 cases
    try:
        print("Try IPv4")
        reader, writer = await asyncio.open_connection(
            address, port,
            family=socket.AF_INET
        )
    except Exception:
        print("Try IPv6")
        reader, writer = await asyncio.open_connection(
            address, port,
            family=socket.AF_INET6
        )

    # Start TwoPartyClient using TwoWayPipe (takes no arguments in this mode)
    client = capnp.TwoPartyClient()

    # Assemble reader and writer tasks, run in the background
    coroutines = [myreader(client, reader), mywriter(client, writer)]
    asyncio.gather(*coroutines, return_exceptions=True)

    return client