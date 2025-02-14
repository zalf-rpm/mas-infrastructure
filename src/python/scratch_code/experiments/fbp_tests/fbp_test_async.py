import sys
import os
from datetime import date, timedelta
import json
import time
import logging
import argparse
import socket

import asyncio
import capnp
capnp.add_import_hook(additional_paths=["../capnproto_schemas/", "../capnproto_schemas/capnp_schemas/"])
import fbp_capnp

logger = logging.getLogger(__name__)
logger.setLevel(logging.DEBUG)

capnp.remove_event_loop()
capnp.create_event_loop(threaded=True)


class Server:
    async def myreader(self):
        while self.retry:
            try:
                # Must be a wait_for so we don't block on read()
                data = await asyncio.wait_for(
                    self.reader.read(4096),
                    timeout=0.1
                )
            except asyncio.TimeoutError:
                logger.debug("myreader timeout.")
                continue
            except Exception as err:
                logger.error("Unknown myreader err: %s", err)
                return False
            await self.server.write(data)
        logger.debug("myreader done.")
        return True


    async def mywriter(self):
        while self.retry:
            try:
                # Must be a wait_for so we don't block on read()
                data = await asyncio.wait_for(
                    self.server.read(4096),
                    timeout=0.1
                )
                self.writer.write(data.tobytes())
            except asyncio.TimeoutError:
                logger.debug("mywriter timeout.")
                continue
            except Exception as err:
                logger.error("Unknown mywriter err: %s", err)
                return False
        logger.debug("mywriter done.")
        return True


    async def myserver(self, reader, writer, bootstrap_object):
        # Start TwoPartyServer using TwoWayPipe (only requires bootstrap)
        self.server = capnp.TwoPartyServer(bootstrap=bootstrap_object)
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

async def new_connection(reader, writer, bootstrap_object):
    server = Server()
    await server.myserver(reader, writer, bootstrap_object)

async def myreader(client, reader):
    while True:
        data = await reader.read(4096)
        client.write(data)

async def mywriter(client, writer):
    while True:
        data = await client.read(4096)
        writer.write(data.tobytes())
        await writer.drain()



class Process(fbp_capnp.FBP.Input.Server):

    def __init__(self, out):
        self._out = out

    def input(self, data, _context, **kwargs): # (data :Text);
        #time.sleep(1)
        self._out.input(data + " -> output")
        print("outputed", data + " -> output")

class Consumer(fbp_capnp.FBP.Input.Server):
    def __init__(self):
        self.count = 0
        self.start = 0
        #pass

    def input(self, data, **kwargs): # (data :Text);
        if self.count == 0:
            self.start = time.time()

        self.count = self.count + 1
        #if self.count % 1000 == 0:
        #    print(data, "count:", self.count)
        print(data, "count:", self.count)

        if self.count == 100000:
            print("received", self.count, "messages in", time.time()-self.start, "s")

async def produce(config):
    reader, writer = await asyncio.open_connection("localhost", config["process_port"])

    # Start TwoPartyClient using TwoWayPipe (takes no arguments in this mode)
    client = capnp.TwoPartyClient()

    # Assemble reader and writer tasks, run in the background
    coroutines = [myreader(client, reader), mywriter(client, writer)]
    asyncio.gather(*coroutines, return_exceptions=True)

    process = client.bootstrap().cast_as(fbp_capnp.FBP.Input)

    start = time.time()

    outer = 100000
    sub = 50
    l = []
    for i in range(1, outer+1):
        l.append(process.input("data " + str(i)).a_wait())
        if i % sub == 0:
            await asyncio.gather(*l)
            l = []
        #await process.input("data " + str(i)).a_wait()
        print("sent","data " + str(i))

    end = time.time()
    print("sent", outer*sub, "messages in", end-start, "s")


async def bootstrap_cap_at(server, port, capnp_interface):
    reader, writer = await asyncio.open_connection(server, port)

    # Start TwoPartyClient using TwoWayPipe (takes no arguments in this mode)
    client = capnp.TwoPartyClient()

    # Assemble reader and writer tasks, run in the background
    coroutines = [myreader(client, reader), mywriter(client, writer)]
    asyncio.gather(*coroutines, return_exceptions=True)

    return client.bootstrap().cast_as(capnp_interface)


async def main():

    # start = [Process, Consumer, Producer]
    config = {
        "process_port": "10001",
        "consumer_port": "10002",
        "server": "*",
        "start": "Process"
    }
    # read commandline args only if script is invoked directly from commandline
    if len(sys.argv) > 1 and __name__ == "__main__":
        for arg in sys.argv[1:]:
            k, v = arg.split("=")
            if k in config:
                config[k] = v

    cs = config["start"]
    if cs == "Process":
        consumer_cap = await bootstrap_cap_at("localhost", config["consumer_port"], fbp_capnp.FBP.Input)

        server = await asyncio.start_server(lambda r,w: new_connection(r, w, Process(consumer_cap)), "127.0.0.1", config["process_port"])
        async with server:
            await server.serve_forever()

    elif cs == "Consumer":
        server = await asyncio.start_server(lambda r,w: new_connection(r, w, Consumer()), "127.0.0.1", config["consumer_port"])
        async with server:
            await server.serve_forever()

    elif cs == "Producer":
        await produce(config)

if __name__ == '__main__':
    asyncio.run(main())
