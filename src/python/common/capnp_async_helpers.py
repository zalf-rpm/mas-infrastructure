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
import capnp
import logging
import os
from pathlib import Path
import socket
import sys
import time

import common.common as common

PATH_TO_REPO = Path(os.path.realpath(__file__)).parent.parent.parent.parent
if str(PATH_TO_REPO) not in sys.path:
    sys.path.insert(1, str(PATH_TO_REPO))

PATH_TO_CAPNP_SCHEMAS = (PATH_TO_REPO / "capnproto_schemas").resolve()
abs_imports = [str(PATH_TO_CAPNP_SCHEMAS)]
persistence_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "persistence.capnp"), imports=abs_imports) 

logger = logging.getLogger(__name__)
logger.setLevel(logging.DEBUG)

ch = logging.StreamHandler(sys.stdout)
ch.setLevel(logging.DEBUG)

#logger.addHandler(ch)

def pr():
    print("disconnected")

class Server:
    def __init__(self, service):
        self._service = service

        self.server = None
        self.reader = None
        self.writer = None
        self.retry = False

    async def socket_reader(self):
        while self.retry:
            try:
                task = asyncio.create_task(self.reader.read(4096))
                left = [task]
                while self.retry:
                    # Must be a wait_for so we don't block on read()
                    done, left = await asyncio.wait(left, timeout=0.1)
                    if task in done:
                        data = task.result()
                    else:
                        #print("<r", flush=True, end="")
                        continue

                    #print("<" + str(len(data)), flush=True, end="<")
                    await self.server.write(data)
                    break
            except Exception as err:
                logger.debug("Unknown socket_reader err: %s", err)
                self.retry = False
                return False
        logger.debug("socket_reader done.")
        return True


    async def socket_writer(self):
        while self.retry or self.writer.at_eof():
            try:
                task = asyncio.create_task(self.server.read(4096))
                left = [task]
                while self.retry:
                    # Must be a wait_for so we don't block on read()
                    done, left = await asyncio.wait(left, timeout=0.1)
                    
                    if task in done:
                        data = task.result()
                    else:
                        #print("w>", flush=True, end="")
                        continue

                    #print(">" + str(len(data)), flush=True, end=">")
                    self.writer.write(data.tobytes())
                    await self.writer.drain()
                    break
            except Exception as err:
                logger.debug("Unknown socket_writer err: %s", err)
                self.retry = False
                return False
        logger.debug("socket_writer done.")
        return True

    async def handle_connection(self, reader, writer):
        # Start TwoPartyServer using TwoWayPipe (only requires bootstrap)
        
        #with capnp.TwoPartyServer(bootstrap=self._service) as ss:
        #    self.server = ss
        self.server = capnp.TwoPartyServer(bootstrap=self._service)
        #self._disconnect_prom = self.server.on_disconnect().then(lambda: print("disconnected"))
        self.reader = reader
        self.writer = writer
        self.retry = True

        # Assemble reader and writer tasks, run in the background
        coroutines = [self.socket_reader(), self.socket_writer()]
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
        
        #del self.server
        #self.server._decref()
        #del self.server
        #print("finished")

    def __del__(self):
        pass
        #print("dying")
        #self._disconnect_prom.wait()
        #print("died")

servers = []

async def serve(host, port, bootstrap):

    def new_connection_factory(bootstrap):
        async def new_connection(reader, writer):
            server = Server(bootstrap)
            #servers.append(server)
            await server.handle_connection(reader, writer)
            #print("handled connection")
        return new_connection

    # Handle both IPv4 and IPv6 cases
    try:
        server = await asyncio.start_server(
            new_connection_factory(bootstrap),
            host, port,
            family=socket.AF_INET
        )
    except Exception:
        print("Tried start server via IPv4, now trying IPv6.")
        server = await asyncio.start_server(
            new_connection_factory(bootstrap),
            host, port,
            family=socket.AF_INET6
        )
    h, p = server.sockets[0].getsockname()
    try:
        bootstrap.port = p
    except:
        pass
    #print("serving bootstrap on interface:", h, "port:", p)
    return server


async def serve_forever(host, port, bootstrap):
    server = await serve(host, port, bootstrap)
    async with server:
        await server.serve_forever()

#------------------------------------------------------------------------------

class ConnectionManager:

    def __init__(self, restorer=None):
        self._connections = {}
        self._alltasks = []
        self._restorer = restorer if restorer else common.Restorer()


    async def manage_forever(self):
        await asyncio.gather(*self._alltasks, return_exceptions=True)


    async def connect(self, sturdy_ref, cast_as = None):
        # we assume that a sturdy ref url looks always like 
        # capnp://vat-id_base64-curve25519-public-key@host:port/sturdy-ref-token_base64_vat-public-key-signed
        try:
            if sturdy_ref[:8] == "capnp://":
                rest = sturdy_ref[8:]
                vat_id_b64, rest = rest.split("@") if "@" in rest else (None, rest)
                host, rest = rest.split(":")
                port, sr_token_b64 = rest.split("/") if "/" in rest else (rest, None)

                host_port = "{}:{}".format(host, port)
                if host_port in self._connections:
                    bootstrap_cap = self._connections[host_port]
                else:
                    # Handle both IPv4 and IPv6 cases
                    try:
                        reader, writer = await asyncio.open_connection(
                            host, port,
                            family=socket.AF_INET
                        )
                    except Exception:
                        print("Tried open connection via IPv4, now trying IPv6.")
                        reader, writer = await asyncio.open_connection(
                            host, port,
                            family=socket.AF_INET6
                        )

                    # Start TwoPartyClient using TwoWayPipe (takes no arguments in this mode)
                    client = capnp.TwoPartyClient()

                    # Assemble reader and writer tasks, run in the background
                    coroutines = [self.socket_reader(client, reader), self.socket_writer(client, writer)]
                    self._alltasks.append(asyncio.gather(*coroutines, return_exceptions=True))

                    bootstrap_cap = client.bootstrap()
                    self._connections[host_port] = bootstrap_cap

                if sr_token_b64:
                    ok, _ = self._restorer.verify_sr_token(sr_token_b64, vat_id_b64)
                    if ok:
                        restorer = bootstrap_cap.cast_as(persistence_capnp.Restorer)
                        dyn_obj_reader = (await restorer.restore(sr_token_b64).a_wait()).cap
                        return dyn_obj_reader.as_interface(cast_as) if cast_as else dyn_obj_reader
                else:
                    return bootstrap_cap.cast_as(cast_as) if cast_as else bootstrap_cap

            return None

        except Exception as e:
            print("Exception in capnp_async_helpers.py::ConnectionManager.connect: {}".format(e))
            return None


    async def try_connect(self, sturdy_ref, cast_as = None, retry_count=10, retry_secs=5, print_retry_msgs=True):
        while True:
            try:
                return await self.connect(sturdy_ref, cast_as=cast_as)
            except Exception as e:
                if retry_count == 0:
                    if print_retry_msgs:
                        print("Couldn't connect to sturdy_ref at {}!".format(sturdy_ref))
                    return None
                retry_count -= 1
                if print_retry_msgs:
                    print("Trying to connect to {} again in {} secs!".format(sturdy_ref, retry_secs))
                time.sleep(retry_secs)
                retry_secs += 1


    async def socket_reader(self, client, reader, retry_task=True):
        '''
        Reads from asyncio socket and writes to pycapnp client interface
        '''

        while True:
            data = await reader.read(4096)
            client.write(data)
        return

        while retry_task:
            try:
                # Must be a wait_for in order to give watch_connection a slot
                # to try again
                data = await asyncio.wait_for(
                    reader.read(4096),
                    timeout=5.0
                )
            except asyncio.TimeoutError:
                logger.debug("socketreader timeout.")
                continue
            except Exception as err:
                logger.error("Unknown socket_reader err: %s", err)
                return False
            client.write(data)
        logger.debug("socketreader done.")
        return True


    async def socket_writer(self, client, writer, retry_task=True):
        '''
        Reads from pycapnp client interface and writes to asyncio socket
        '''

        while True:
            data = await client.read(4096)
            writer.write(data.tobytes())
            await writer.drain()
        return

        while retry_task:
            try:
                # Must be a wait_for in order to give watch_connection a slot
                # to try again
                data = await asyncio.wait_for(
                    client.read(4096),
                    timeout=5.0
                )
                writer.write(data.tobytes())
                await writer.drain()
            except asyncio.TimeoutError:
                logger.debug("socketwriter timeout.")
                continue
            except Exception as err:
                logger.error("Unknown socket_writer err: %s", err)
                return False
        logger.debug("socketwriter done.")
        return True

#------------------------------------------------------------------------------

"""
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
    #try:
    #print("Try IPv4")
    reader, writer = await asyncio.open_connection(
        address, port,
        family=socket.AF_INET
    )
    #except Exception:
    #    print("Try IPv6")
    #    reader, writer = await asyncio.open_connection(
    #        address, port,
    #        family=socket.AF_INET6
    #    )

    # Start TwoPartyClient using TwoWayPipe (takes no arguments in this mode)
    client = capnp.TwoPartyClient()

    # Assemble reader and writer tasks, run in the background
    coroutines = [myreader(client, reader), mywriter(client, writer)]
    gather_results = asyncio.gather(*coroutines, return_exceptions=True)

    return (client, gather_results)
"""