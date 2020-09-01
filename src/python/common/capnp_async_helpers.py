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

import asyncio
import logging
import socket

import capnp

logger = logging.getLogger(__name__)
logger.setLevel(logging.DEBUG)

ch = logging.StreamHandler()
ch.setLevel(logging.DEBUG)

#logger.addHandler(ch)

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
                #logger.debug("myreader timeout.")
                continue
            except Exception as err:
                logger.debug("Unknown myreader err: %s", err)
                self.retry = False
                return False
            #await self.server.write(data)
        logger.debug("myreader done.")
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
                #logger.debug("mywriter timeout.")
                continue
            except Exception as err:
                logger.debug("Unknown mywriter err: %s", err)
                self.retry = False
                return False
        logger.debug("mywriter done.")
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
    gather_results = asyncio.gather(*coroutines, return_exceptions=True)

    return (client, gather_results)