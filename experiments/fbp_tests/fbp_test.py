import sys
import os
from datetime import date, timedelta
import json
import time

import capnp
capnp.add_import_hook(additional_paths=["capnproto_schemas"])
import fbp_capnp

class Process(fbp_capnp.FBP.Input.Server):

    def __init__(self, out):
        self._out = out

    def input_context(self, context): # (data :Text);
        data = context.params.data
        time.sleep(1)
        self._out.input(data + " -> output")
        print("outputed", data + " -> output")

class Consumer(fbp_capnp.FBP.Input.Server):
    def __init__(self):
        self.count = 0
        self.start = 0

    def input(self, data, _context, **kwargs): # (data :Text);
        if self.count == 0:
            self.start = time.time()

        self.count = self.count + 1
        #if self.count % 1000 == 0:
        #    print(data, "count:", self.count)
        print(data, "count:", self.count)

        if self.count == 100000:
            print("received", self.count, "messages in", time.time()-self.start, "s")

def produce(config):
    process = capnp.TwoPartyClient("localhost:" + config["process_port"]).bootstrap().cast_as(fbp_capnp.FBP.Input)

    start = time.time()

    outer = 100000
    sub = 50
    l = []
    for i in range(1, outer+1):
        #l.append(process.input("data " + str(i)).a_wait())
        #if i % sub == 0:
        #    await asyncio.gather(*l)
        #    l = []
        process.input("data " + str(i)).wait()
        #if i % 1000 == 0:
        #    print("sent","data " + str(i))
        print("sent","data " + str(i))

    end = time.time()
    print("sent", outer*sub, "messages in", end-start, "s")


def main():

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
        consumer = capnp.TwoPartyClient("localhost:" + config["consumer_port"]).bootstrap().cast_as(fbp_capnp.FBP.Input)
        server = capnp.TwoPartyServer(config["server"] + ":" + config["process_port"], bootstrap=Process(consumer))
        server.run_forever()
    elif cs == "Consumer":
        server = capnp.TwoPartyServer(config["server"] + ":" + config["consumer_port"], bootstrap=Consumer())
        server.run_forever()
    elif cs == "Producer":
        produce(config)

if __name__ == '__main__':
    main()
