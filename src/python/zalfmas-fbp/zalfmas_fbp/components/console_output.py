#!/usr/bin/python
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
# Copyright (C: Leibniz Centre for Agricultural Landscape Research (ZALF)

import asyncio
import capnp
import os
import sys
from zalfmas_common import common
from zalfmas_common import fbp
import zalfmas_capnp_schemas
sys.path.append(os.path.dirname(zalfmas_capnp_schemas.__file__))
import common_capnp
import fbp_capnp

config = {
    "in_sr": "capnp://_VcNoyHw5kwHS62qx_uIHqrnhYXUPEmfegbtIY9rf4I@10.10.25.19:9922/r_out",  # string
}

async def main(config: dict):
    common.update_config(config, sys.argv, print_config=True,
                         allow_new_keys=False)
    cm = common.ConnectionManager()
    ports, close_out_ports = await fbp.connect_ports(config, cm)

    while ports["in"]:
        try:
            in_msg = await ports["in"].read()
            if in_msg.which() == "done":
                ports["in"] = None
                continue
            in_ip = in_msg.value.as_struct(fbp_capnp.IP)
            print(in_ip.content.as_text())
        except Exception as e:
            print(f"{os.path.basename(__file__)} Exception:", e)

    await close_out_ports()
    print(f"{os.path.basename(__file__)}: process finished")

if __name__ == '__main__':
    asyncio.run(capnp.run(main(config)))
