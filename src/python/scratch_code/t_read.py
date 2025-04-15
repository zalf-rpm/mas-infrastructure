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
import capnp
import os
import sys
from zalfmas_common import common
import zalfmas_capnp_schemas
sys.path.append(str(os.path.dirname(zalfmas_capnp_schemas.__file__)))
import fbp_capnp

async def main():
    con_man = common.ConnectionManager()

    r = await con_man.try_connect("capnp://10.10.24.222:9922/r_in", cast_as=fbp_capnp.Channel.Reader)

    async def read():
        print((await r.read()).value.as_text(), flush=True)

    prom = r.read()
    print((await prom).value.as_text(), flush=True)
    #await read()

if __name__ == '__main__':
    asyncio.run(capnp.run(main()))
