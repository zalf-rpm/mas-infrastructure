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

import capnp
import json
import sys
import os
from datetime import date, timedelta
# import numpy as np
# import pandas as pd
from pathlib import Path
# from pyproj import Proj, transform
# from scipy.interpolate import NearestNDInterpolator
import time
import uuid

PATH_TO_REPO = Path(os.path.realpath(__file__)).parent.parent.parent.parent
if str(PATH_TO_REPO) not in sys.path:
    sys.path.insert(1, str(PATH_TO_REPO))

PATH_TO_PYTHON_CODE = PATH_TO_REPO / "src/python"
if str(PATH_TO_PYTHON_CODE) not in sys.path:
    sys.path.insert(1, str(PATH_TO_PYTHON_CODE))

import common

PATH_TO_CAPNP_SCHEMAS = PATH_TO_REPO / "capnproto_schemas"
abs_imports = [str(PATH_TO_CAPNP_SCHEMAS)]
cluster_admin_service_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "cluster_admin_service.capnp"), imports=abs_imports)

# ------------------------------------------------------------------------------

def printBla(prom):
    print("bla, ", prom)


class SlurmRuntime(cluster_admin_service_capnp.Cluster.Runtime.Server):

    def __init__(self, cores, admin_master):
        self._admin_master = admin_master
        self._cores = cores
        self._used_cores = 0
        self._uuid4 = uuid.uuid4()
        self._factories = {}
        self._unregs = {}

    def info_context(self, context):  # info @0 () -> (info :IdInformation);
        # interface to retrieve id information from an object
        return {"id": str(self._uuid4), "name": "SlurmRuntime(" + str(self._uuid4) + ")", "description": ""}

    def delFactory(self, aModelId):
        # remove factor for given model id
        del self._factories[aModelId]

        # unregister factory at admin_master
        # works because deleting object will just do the same as unregistering
        del self._unregs[aModelId]
        # self._unregs.pop(aModelId).unregister()

    # registerModelInstanceFactory @0 (aModelId :Text, aFactory :ModelInstanceFactory) -> (unreg :Common.Unregister);
    def registerModelInstanceFactory(self, aModelId, aFactory, _context, **kwargs):
        "register a model instance factory for the given model id"
        self._factories[aModelId] = aFactory
        self._unregs[aModelId] = self._admin_master.registerModelInstanceFactory(aModelId, aFactory).unregister
        return common.CallbackImpl(self.delFactory, aModelId, exec_callback_on_del=True)

    # availableModels @1 () -> (factories :List(ModelInstanceFactory));
    def availableModels_context(self, context):
        "# the model instance factories this runtime has access to"
        pass

    # numberOfCores @2 () -> (cores :Int16);
    def numberOfCores_context(self, context):
        "# how many cores does the runtime offer"
        return self._cores

    # freeNumberOfCores @3 () -> (cores :Int16);
    def freeNumberOfCores_context(self, context):
        "# how many cores are still unused"
        return self._cores - self._used_cores

    # reserveNumberOfCores @4 (reserveCores :Int16, aModelId :Text) -> (reservedCores :Int16);
    def reserveNumberOfCores_context(self, context):
        "# try to reserve number of reserveCores for model aModelId and return the number of actually reservedCores"
        pass


def main():
    config = {
        "admin_master_address": "localhost:8000"
    }
    # read commandline args only if script is invoked directly from commandline
    if len(sys.argv) > 1 and __name__ == "__main__":
        for arg in sys.argv[1:]:
            k, v = arg.split("=")
            if k in config:
                config[k] = v

    print("config:", config)

    # address = parse_args().address

    master_available = False
    while not master_available:
        try:
            admin_master = capnp.TwoPartyClient(config["admin_master_address"]).bootstrap().cast_as(
                cluster_admin_service_capnp.Cluster.AdminMaster)
            master_available = True
        except:
            # time.sleep(1)
            pass

    # runtime = SlurmRuntime(cores=4, admin_master=admin_master)
    # registered_ = False
    # while not registered_factory:
    #    try:
    #        runtime.registerModelInstanceFactory("monica_v2.1", monicaFactory).wait()
    #        registered_factory = True
    #    except:
    #        time.sleep(1)
    #        pass

    # server = capnp.TwoPartyServer("*:8000", bootstrap=DataServiceImpl("/home/berg/archive/data/"))
    server = capnp.TwoPartyServer("*:9000", bootstrap=SlurmRuntime(cores=4, admin_master=admin_master))
    server.run_forever()


if __name__ == '__main__':
    main()
