import asyncio
import capnp
import os
from pathlib import Path
import sys
import time

PATH_TO_REPO = Path(os.path.realpath(__file__)).parent.parent.parent
if str(PATH_TO_REPO) not in sys.path:
    sys.path.insert(1, str(PATH_TO_REPO))

PATH_TO_PYTHON_CODE = PATH_TO_REPO / "src/python"
if str(PATH_TO_PYTHON_CODE) not in sys.path:
    sys.path.insert(1, str(PATH_TO_PYTHON_CODE))

# import common.capnp_async_helpers as async_helpers
# import common.common as common

PATH_TO_CAPNP_SCHEMAS = PATH_TO_REPO / "capnproto_schemas"
abs_imports = [str(PATH_TO_CAPNP_SCHEMAS)]
a_capnp = capnp.load(str(PATH_TO_REPO / "src" / "python" / "a.capnp"), imports=abs_imports)
#common_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "common.capnp"), imports=abs_imports)


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