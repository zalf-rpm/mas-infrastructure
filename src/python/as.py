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
#from datetime import date, timedelta
#import json
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

import common.capnp_async_helpers as async_helpers

abs_imports = ["capnproto_schemas"]
a_capnp = capnp.load("src/python/a.capnp", imports=abs_imports)

#------------------------------------------------------------------------------

class A_Impl(a_capnp.A.Server):

    async def sleep(self, secs, pfp):
        time.sleep(secs)
        pfp.fulfill()

    def _method_context(self, context, **kwargs):
        pfp = capnp.PromiseFulfillerPair()
        pfp.fulfill()
        asyncio.create_task(self.sleep(3, pfp))
        #task.add_done_callback(lambda res: context.results.res = "___RESULT____")
        #time.sleep(3)
        context.results.res = "_______________method_RESULT___________________"
        return pfp.promise

    def method2(self, param, **kwargs):
        time.sleep(3)
        return "_______________method_RESULT___________________"

    def method(self, param, **kwargs):
        return "_______________method_RESULT___________________"


#------------------------------------------------------------------------------

def main(server="*", port=11002):

    config = {
        "port": str(port),
        "server": server
    }
    # read commandline args only if script is invoked directly from commandline
    if len(sys.argv) > 1 and __name__ == "__main__":
        for arg in sys.argv[1:]:
            k, v = arg.split("=")
            if k in config:
                config[k] = v
    print(config)

    server = capnp.TwoPartyServer(config["server"] + ":" + config["port"],
                                  bootstrap=A_Impl())
    server.run_forever()

#------------------------------------------------------------------------------

async def async_main(serve_bootstrap=False, host="0.0.0.0", port=None, reg_sturdy_ref=None):
    config = {
        "host": host,
        "port": port,
        "reg_sturdy_ref": reg_sturdy_ref,
        "serve_bootstrap": str(serve_bootstrap),
        "reg_category": "climate",
    }
    # read commandline args only if script is invoked directly from commandline
    if len(sys.argv) > 1 and __name__ == "__main__":
        for arg in sys.argv[1:]:
            k, v = arg.split("=")
            if k in config:
                config[k] = v
    print("config used:", config)

    conMan = async_helpers.ConnectionManager()

    service = A_Impl()

    if config["reg_sturdy_ref"]:
        registrator = await conMan.try_connect(config["reg_sturdy_ref"], cast_as=reg_capnp.Registrator)
        if registrator:
            unreg = await registrator.register(ref=service, categoryId=config["reg_category"]).a_wait()
            print("Registered ", config["name"], "climate service.")
            #await unreg.unregister.unregister().a_wait()
        else:
            print("Couldn't connect to registrator at sturdy_ref:", config["reg_sturdy_ref"])

    if config["serve_bootstrap"].upper() == "TRUE":
        await async_helpers.serve_forever(config["host"], config["port"], service)
    else:
        await conMan.manage_forever()

#------------------------------------------------------------------------------

if __name__ == '__main__':
    #main()
    asyncio.run(async_main(serve_bootstrap=True, port=11002))

