#from scipy.interpolate import NearestNDInterpolator
#import numpy as np
import sys
import os
from datetime import date, timedelta
#import pandas as pd
#from pyproj import Proj, transform
import json
import time
import uuid

import common
import capnp
capnp.add_import_hook(additional_paths=["../capnproto_schemas/", "../capnproto_schemas/capnp_schemas/"])
import cluster_admin_service_capnp

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

    def info_context(self, context): # info @0 () -> (info :IdInformation);
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

    #address = parse_args().address

    master_available = False
    while not master_available:
        try:
            admin_master = capnp.TwoPartyClient(config["admin_master_address"]).bootstrap().cast_as(cluster_admin_service_capnp.Cluster.AdminMaster)
            master_available = True
        except:
            # time.sleep(1)
            pass

    #runtime = SlurmRuntime(cores=4, admin_master=admin_master)
    #registered_ = False
    # while not registered_factory:
    #    try:
    #        runtime.registerModelInstanceFactory("monica_v2.1", monicaFactory).wait()
    #        registered_factory = True
    #    except:
    #        time.sleep(1)
    #        pass

    #server = capnp.TwoPartyServer("*:8000", bootstrap=DataServiceImpl("/home/berg/archive/data/"))
    server = capnp.TwoPartyServer("*:9000", bootstrap=SlurmRuntime(cores=4, admin_master=admin_master))
    server.run_forever()


if __name__ == '__main__':
    main()
