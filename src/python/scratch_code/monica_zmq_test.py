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

import zmq
import asyncio
import json
import sys
#from zalfmas_common import common

standalone_config = {
    #"path_to_channel": "/home/berg/GitHub/monica/_cmake_debug/common/channel",
}
async def main(config: dict):
    #common.update_config(config, sys.argv, print_config=True, allow_new_keys=False)

    context = zmq.Context()
    prod_socket = context.socket(zmq.PUSH)  # pylint: disable=no-member
    prod_socket.connect("tcp://login01.cluster.zalf.de:6666")
    cons_socket = context.socket(zmq.PULL)
    cons_socket.connect("tcp://login01.cluster.zalf.de:7777")

    with open("/home/berg/GitHub/mas-infrastructure/src/python/scratch_code/env_1.json") as _:
        env_json = json.load(_)

    prod_socket.send_json(env_json)
    msg = cons_socket.recv_json()
    print(msg)

if __name__ == '__main__':
    asyncio.run(main(standalone_config))
