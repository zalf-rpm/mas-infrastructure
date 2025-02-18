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


async def main():
    config = {
        "_": "write_file.py",
        "in_sr": None,
        "id_attr": "id",
        "from_attr": None,
        "filepath_pattern": "csv_{id}.csv",
        "path_to_out_dir": "/home/berg/GitHub/mas-infrastructure/src/python/fbp/out/",
        "append": False,
        "debug": False,
    }
    common.update_config(config, sys.argv, print_config=True, allow_new_keys=False)
    ports, close_out_ports = await fbp.connect_ports(config)

    count = 0
    while ports["inp"]:
        try:
            in_msg = await ports["inp"].read()
            if in_msg.which() == "done":
                ports["inp"] = None
                continue

            in_ip = in_msg.value.as_struct(fbp_capnp.IP)
            id_attr = common.get_fbp_attr(in_ip, config["id_attr"])
            id = id_attr.as_text() if id_attr else str(count)
            content_attr = common.get_fbp_attr(in_ip, config["from_attr"])
            content = content_attr.as_text() if content_attr else in_ip.content.as_text()

            filepath = config["path_to_out_dir"] + config["filepath_pattern"].format(id=id)
            with open(filepath, "at" if config["append"] else "wt") as _:
                _.write(content)
                count += 1

            if config["debug"]:
                print("wrote", filepath)

        except Exception as e:
            print("write_file.py ex:", e)

        print("write_file.py: exiting run")


if __name__ == '__main__':
    asyncio.run(capnp.run(main()))




