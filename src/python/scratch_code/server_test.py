import asyncio
import capnp
import os
from pathlib import Path
import sys
import time
import random

from zalfmas_common import service as serv
import zalfmas_capnpschemas
sys.path.append(os.path.dirname(zalfmas_capnpschemas.__file__))
import a_capnp

count = 0

async def bla():
    global count
    count += 1
    print(count, end=" ", flush=True)
    sleep_secs = random.random()
    time.sleep(sleep_secs)
    return f"slept for {sleep_secs} seconds"


async def bli():
    global count
    count += 1
    return count


class A(a_capnp.A.Server):

    def __init__(self):
        self.msg_count = 0

    async def method(self, param, **kwargs):
        return await bla()
        # self.msg_count += 1
        # print(self.msg_count, end=" ", flush=True)
        # sleep_secs = random.random()
        # time.sleep(sleep_secs)
        # return f"slept for {sleep_secs} seconds"
        # time.sleep(1)
        # return f"method called"

    async def m(self, id, **kwargs):
        return await bli()


class AA(a_capnp.A.Server):

    def __init__(self):
        self.msg_count = 0

    async def method(self, param, **kwargs):
        self.msg_count += 1
        print(self.msg_count, end=" ", flush=True)
        sleep_secs = random.random()
        time.sleep(sleep_secs)
        return f"slept for {sleep_secs} seconds"
        #time.sleep(1)
        #return f"method called"

    async def m(self, id, **kwargs):
        self.msg_count += 1
        print(self.msg_count, end=" ", flush=True)
        return self.msg_count


class S(a_capnp.S.Server):
    def getCB_context(self, context):
        context.results.cb = CB()


class CB(a_capnp.CB.Server):
    def __init__(self):
        self.ds = map(lambda i: D(i), range(1000000))

    def getD_context(self, context):
        try:
            d = next(self.ds)
            context.results.d = d
        except StopIteration:
            pass


class D(a_capnp.D.Server):
    def __init__(self, i):
        self.i = i
        self.megabytes = 10
        self.sum = 0

    def getData_context(self, context):
        data = bytes(1024 * 1024 * self.megabytes)
        self.sum += round(len(data) / 1024 / 1024)
        context.results.data = data  # = list(itertools.repeat(self.i, 1000000))
        context.results.i = self.i
        print("i:", self.i, self.i * self.sum, "MB sent")


#class Server(test_capability_capnp.TestInterface.Server):
#    def __init__(self, val=1):
#        self.val = val

#    async def foo(self, i, j, **kwargs):
#        return str(i * 5 + self.val)

def new_connection(bs):
    async def new_con(stream):
        await capnp.TwoPartyServer(stream, bootstrap=bs).on_disconnect()
    return new_con

async def main():
    await serv.init_and_run_service({"service": AA()}, "localhost", 9999,
                                    serve_bootstrap=True,
                                    name_to_service_srs={"service": "aaaa"})

if __name__ == '__main__':
    asyncio.run(capnp.run(main()))