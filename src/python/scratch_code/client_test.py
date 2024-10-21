import asyncio
import capnp
import os
import sys
from zalfmas_common import common
import zalfmas_capnp_schemas
sys.path.append(os.path.dirname(zalfmas_capnp_schemas.__file__))
import a_capnp

async def main():
    con_man = common.ConnectionManager()
    #a = await con_man.connect("capnp://localhost:9999/aaaa", a_capnp.A)
    a = await con_man.connect("capnp://localhost:9999/aaaa", a_capnp.A)
    res = await a.method()
    print(res.res, end=" ", flush=True)


if __name__ == '__main__':
    asyncio.run(capnp.run(main()))