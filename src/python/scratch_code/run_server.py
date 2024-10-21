#!/usr/bin/python
# -*- coding: UTF-8

# This Source Code Form is subject to the terms of the Mozilla Public
# License, v. 2.0. If a copy of the MPL was not distributed with this
# file, You can obtain one at http://mozilla.org/MPL/2.0/. */

# Authors:
# Michael Berg-Mohnicke <michael.berg-mohnicke@zalf.de>
#
# Maintainers:
# Currently maintained by the authors.
#
# Copyright (C: Leibniz Centre for Agricultural Landscape Research (ZALF)

import asyncio
from io import StringIO
import capnp
import os
import sys
from zalfmas_common import service as serv
import zalfmas_capnp_schemas
sys.path.append(os.path.dirname(zalfmas_capnp_schemas.__file__))
import a_capnp

class A(a_capnp.A.Server):
    async def method(self, param, **kwargs):
        sio = StringIO()
        for i in range(20200):
            sio.write(".")
        return sio.getvalue()

    async def m(self, id, **kwargs):
        print("id:", id)


async def main():
    await serv.init_and_run_service({"service": A()}, "localhost", 9999,
                                    serve_bootstrap=True,
                                    name_to_service_srs={"service": "aaaa"})

if __name__ == '__main__':
    asyncio.run(capnp.run(main()))
