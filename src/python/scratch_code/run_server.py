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
# This file has been created at the Institute of
# Landscape Systems Analysis at the ZALF.
# Copyright (C: Leibniz Centre for Agricultural Landscape Research (ZALF)

import asyncio
import capnp
import os
from pathlib import Path
import psutil
from random import random
import sys
import time
from threading import Thread
from zalfmas_common import service as serv
import zalfmas_capnpschemas
sys.path.append(os.path.dirname(zalfmas_capnpschemas.__file__))
import a_capnp


class A(a_capnp.A.Server):
    async def method(self, param, **kwargs):
        print("param:", param)

    async def m(self, id, **kwargs):
        print("id:", id)


async def main():
    await serv.init_and_run_service({"service": A()}, "localhost", 9999,
                                    serve_bootstrap=True,
                                    name_to_service_srs={"service": "aaaa"})

if __name__ == '__main__':
    asyncio.run(capnp.run(main()))
