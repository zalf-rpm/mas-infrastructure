﻿#!/usr/bin/python
# -*- coding: UTF-8

# This Source Code Form is subject to the terms of the Mozilla Public
# License, v. 2.0. If a copy of the MPL was not distributed with this
# file, You can obtain one at http://mozilla.org/MPL/2.0/. */

# Authors:
# Michael Berg-Mohnicke <michael.berg@zalf.de>
#
# Maintainers:
# Currently maintained by the authors.
#
# This file is part of the util library used by models created at the Institute of
# Landscape Systems Analysis at the ZALF.
# Copyright (C: Leibniz Centre for Agricultural Landscape Research (ZALF)

import capnp
import os
from pathlib import Path
import sys
from zalfmas_common import common
from zalfmas_common import fbp
import zalfmas_capnpschemas
sys.path.append(os.path.dirname(zalfmas_capnpschemas.__file__))
import common_capnp
import fbp_capnp

config = {
    "out_sr": None,  # string
}

async def main(config: dict):
    common.update_config(config, sys.argv, print_config=True,
                         allow_new_keys=False)
    ports, close_out_ports = await fbp.connect_ports(config)

    count = 0
    while ports["out"]:
        try:
            count += 1
            out_ip = fbp_capnp.IP.new_message()
            out_ip = str(count)
            await ports["out"].write(value=out_ip)
        except Exception as e:
            print(f"{os.path.basename(__file__)} Exception:", e)

    await close_out_ports()
    print(f"{os.path.basename(__file__)}: process finished")

if __name__ == '__main__':
    asyncio.run(capnp.run(main(config)))




