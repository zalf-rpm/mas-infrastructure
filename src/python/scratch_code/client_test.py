import asyncio
import capnp
import os
import sys
import time

import zalfmas_capnpschemas
sys.path.append(os.path.dirname(zalfmas_capnpschemas.__file__))
import a_capnp


async def main():
    con = await capnp.AsyncIoStream.create_connection(host="localhost", port="9999")
    client = capnp.TwoPartyClient(con)
    bs = client.bootstrap()
    a = bs.cast_as(a_capnp.A)

    time.sleep(5)
    #res = await a.method()
    res = await a.m()
    print(res.count, end=" ", flush=True)
    #print(res.res, flush=True)
    time.sleep(5)


if __name__ == '__main__':
    asyncio.run(capnp.run(main()))