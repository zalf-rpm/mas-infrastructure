import asyncio
import socket
import time

import common.python.capnp_async_helpers as async_helpers

import capnp
import capnproto_schemas.a_capnp as a_capnp

class A_Impl(a_capnp.A.Server):

    def method_context(self, context):
        print("---------- method start", flush=True)
        time.sleep(1.5)
        context.results.res = "_______________method_RESULT___________________"
        print("---------- method finished", flush=True)


async def async_main():

    server = "0.0.0.0"
    port = 11111

    async def new_connection(reader, writer):
        server = async_helpers.Server(A_Impl())
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