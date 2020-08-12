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
import subprocess
from collections import defaultdict

import common
import capnp
capnp.add_import_hook(additional_paths=["../capnproto_schemas/", "../capnproto_schemas/capnp_schemas/"])

import model_capnp
import cluster_admin_service_capnp
import common_capnp


class SlurmMonicaInstanceFactory(cluster_admin_service_capnp.Cluster.ModelInstanceFactory.Server):

    def __init__(self, config):
        self._uuid4 = uuid.uuid4()
        self._registry = defaultdict(lambda: {
            "procs": {},  # map instance to processes
            "instance_caps": {},  # maps instance_id to instance
            "unregister_caps": [],
            "prom_fulfiller": capnp.PromiseFulfillerPair(),
            "fulfill_count": 0
        })
        self._port = config["port"]
        self._path_to_monica_binaries = config["path_to_monica_binaries"]

    def __del__(self):
        for reg_token in self._registry.keys():
            self.terminate_all_procs(reg_token)

    def terminate_all_procs(self, registration_token):
        if registration_token in self._registry:
            for _, p in self._registry[registration_token]["procs"].items():
                p.terminate()

    # registerModelInstance @5 [ModelInstance] (instance :ModelInstance, registrationToken :Text = "") -> (unregister :Common.Callback);

    def registerModelInstance_context(self, context):
        """# register the given instance of some model optionally given the registration token and receive a
        # callback to unregister this instance again"""

        instance_reg_token = context.params.registrationToken
        # a token without id = single instance, will get 0 as id
        registration_token, proc_id = (instance_reg_token + ":0").split(":")[:2]

        if registration_token in self._registry:
            reg = self._registry[registration_token]

            # store new cap holder
            reg["instance_caps"][proc_id] = common.CapHolderImpl(context.params.instance, instance_reg_token, lambda: reg["procs"][proc_id].terminate())

            # create unregister cap for instance, to get notified if instance dies
            unreg_cap = common.CallbackImpl(lambda: self._registry.pop(registration_token, None), exec_callback_on_del=True)
            reg["unregister_caps"].append(unreg_cap)
            reg["fulfill_count"] -= 1
            if reg["fulfill_count"] == 0:
                reg["prom_fulfiller"].fulfill()
            context.results.unregister = unreg_cap

    # restoreSturdyRef @6 (sturdyRef :Text) -> (cap :Common.CapHolder);

    def restoreSturdyRef_context(self, context):
        "# return the capability holder for the given sturdyRef if available"

        registration_token, proc_id = (context.params.sturdyRef + ":-1").split(":")[:2]
        if registration_token in self._registry:
            reg = self._registry[registration_token]

            if int(proc_id) < 0:
                context.results.cap = common.CapHolderImpl(reg["instance_caps"].values(), 
                registration_token, lambda: self.terminate_all_procs(registration_token))
            else:
                if proc_id in reg["instance_caps"]:
                    context.results.cap = reg["instance_caps"][proc_id]

    def modelId(self, _context, **kwargs):  # modelId @4 () -> (id :Text);
        "# return the id of the model this factory creates instances of"
        return "monica_v2.1"

    def info(self, _context, **kwargs):  # info @0 () -> (info :IdInformation);
        # interface to retrieve id information from an object
        return {"id": str(self._uuid4), "name": "SlurmMonicaInstanceFactory(" + str(self._uuid4) + ")", "description": ""}

    # newInstance @0 () -> (instance :Capability);
    def newInstance_context(self, context):
        "# return a new instance of the model"

        registration_token = str(uuid.uuid4())
        monica = subprocess.Popen([self._path_to_monica_binaries + "monica-capnp-server.exe", "-i",
                                   "-cf", "-fa", "localhost", "-fp", str(self._port), "-rt", registration_token + ":0"])

        reg = self._registry[registration_token]
        reg["procs"]["0"] = monica
        reg["fulfill_count"] = 1
        return reg["prom_fulfiller"].promise.then(lambda: setattr(context.results, "instance", self._registry[registration_token]["instance_caps"]["0"]))

    # newInstances @1 (numberOfInstances :Int16) -> (instances :AnyList);
    def newInstances_context(self, context):
        "# return the requested number of model instances"
        registration_token = str(uuid.uuid4())
        instance_count = context.params.numberOfInstances

        # start monica processes
        procs = {}
        for i in range(instance_count):
            monica = subprocess.Popen([self._path_to_monica_binaries + "monica-capnp-server.exe", "-i",
                                       "-cf", "-fa", "localhost", "-fp", str(self._port), "-rt", registration_token + ":" + str(i)])
            procs[str(i)] = monica

        reg = self._registry[registration_token]
        reg["procs"] = procs
        reg["fulfill_count"] = instance_count

        # return promise for the list of capabilities once the monicas started up and registered themselfs with the factory
        return reg["prom_fulfiller"].promise.then(lambda: setattr(context.results, "instances", \
                                                                  # common.CapHolderImpl(self._registry[registration_token]["instance_caps"]["0"], registration_token, lambda: self.terminate_all_procs(registration_token))))
                                                                  common.CapHolderImpl([1, 2, 3], registration_token, lambda: self.terminate_all_procs(registration_token))))

    # newCloudViaZmqPipelineProxies @2 (numberOfInstances :Int16) -> (zmqInputAddress :Text, zmqOutputAddress :Text);
    def newCloudViaZmqPipelineProxies_context(self, context):
        "# return the TCP addresses of two ZeroMQ proxies to access the given number of instances of the model"
        pass

    # newCloudViaProxy @3 (numberOfInstances :Int16) -> (proxy :AnyPointer);
    def newCloudViaProxy_context(self, context):
        "# return a model proxy acting as a gateway to the requested number of model instances"
        pass


def main():
    #address = parse_args().address

    runtime_available = False
    while not runtime_available:
        try:
            # runtime = capnp.TwoPartyClient("localhost:9000").bootstrap().cast_as(
            runtime = capnp.TwoPartyClient("10.10.24.186:9000").bootstrap().cast_as(cluster_admin_service_capnp.Cluster.Runtime)
            runtime_available = True
        except:
            # time.sleep(1)
            pass

    monicaFactory = SlurmMonicaInstanceFactory({
        "port": 10000,
        "path_to_monica_binaries": "C:/Users/berg.ZALF-AD/GitHub/monica/_cmake_vs2019_win64/Debug/"
    })
    registered_factory = False
    while not registered_factory:
        try:
            unreg = runtime.registerModelInstanceFactory("monica_v2.1", monicaFactory).wait().unregister
            registered_factory = True
        except capnp.KjException as e:
            print(e)
            time.sleep(1)

    #server = capnp.TwoPartyServer("*:8000", bootstrap=DataServiceImpl("/home/berg/archive/data/"))
    server = capnp.TwoPartyServer("*:10000", bootstrap=monicaFactory)
    server.run_forever()


if __name__ == '__main__':
    main()
