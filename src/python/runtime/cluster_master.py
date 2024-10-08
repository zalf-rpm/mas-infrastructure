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
from collections import defaultdict
import json
from datetime import date, timedelta
# import numpy as np
import os
# import pandas as pd
from pathlib import Path
# from pyproj import Proj, transform
# from scipy.interpolate import NearestNDInterpolator
import sys
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
common_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "common.capnp"), imports=abs_imports)
cluster_admin_service_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "cluster_admin_service.capnp"),
                                         imports=abs_imports)


# ------------------------------------------------------------------------------

class AdminMasterImpl(cluster_admin_service_capnp.Cluster.AdminMaster.Server):
    "Implementation of the Cluster.AdminMaster Cap'n Proto server interface."

    def __init__(self):
        self._uuid4 = uuid.uuid4()
        self._factories = defaultdict(list)

    def info_context(self, context):  # info @0 () -> (info :IdInformation);
        "# interface to retrieve id information from an object"
        return {"id": str(self._uuid4), "name": "AdminMaster(" + str(self._uuid4) + ")", "description": ""}

    # def unregisterFactory(self, aModelId, aFactory):
    #   self._factories[aModelId].remove(aFactory)

    # registerModelInstanceFactory @0 (aModelId :Text, aFactory :ModelInstanceFactory) -> (unreg :Common.Unregister);
    def registerModelInstanceFactory(self, aModelId, aFactory, _context, **kwargs):
        "# register a model instance factory for the given model id"
        self._factories[aModelId].append(aFactory)
        return common.CallbackImpl(lambda: self._factories[aModelId].remove(aFactory), exec_callback_on_del=True)
        # return common.UnregisterImpl(self.unregisterFactory, aModelId, aFactory)

    # availableModels @1 () -> (factories :List(ModelInstanceFactory));
    def availableModels_context(self, context):
        "# get instance factories to all the available models on registered runtimes"
        context.results.init("factories", len(self._factories))
        fs = []
        for model_id, factories in self._factories.items():
            if len(factories) == 1:
                fs.append(factories[0])
            elif len(factories) > 1:
                fs.append(MultiRuntimeModelInstanceFactory(model_id, factories))

        context.results.factories = fs


class UserMasterImpl(cluster_admin_service_capnp.Cluster.UserMaster.Server):

    def __init__(self, admin_master):
        self._uuid4 = uuid.uuid4()
        self._admin_master = admin_master

    def info_context(self, context):  # info @0 () -> (info :IdInformation);
        # interface to retrieve id information from an object
        return {"id": str(self._uuid4), "name": "UserMaster(" + str(self._uuid4) + ")", "description": ""}

    # availableModels @0 () -> (factories :List(ModelInstanceFactory));
    def availableModels(self, context):
        "# get instance factories to all the available models to the user"
        return self._admin_master.availableModels()


class MultiRuntimeModelInstanceFactory(cluster_admin_service_capnp.Cluster.ModelInstanceFactory.Server):

    def __init__(self, model_id, factories):
        self._model_id = model_id
        self._factories = factories
        self._uuid4 = uuid.uuid4()

    def modelId(self, _context, **kwargs):  # modelId @4 () -> (id :Text);
        "# return the id of the model this factory creates instances of"
        return self._model_id

    def info_context(self, context):  # info @0 () -> (info :IdInformation);
        # interface to retrieve id information from an object
        return {"id": str(self._uuid4),
                "name": "MultiRuntimeModelInstanceFactory(" + self._model_id + ")(" + str(self._uuid4) + ")",
                "description": ""}

    # newInstance @0 () -> (instance :AnyPointer);
    def newInstance_context(self, context):
        "# return a new instance of the model"
        pass

    # newInstances @1 (numberOfInstances :Int16) -> (instances :AnyList);
    def newInstances_context(self, context):
        "# return the requested number of model instances"
        pass

    # newCloudViaZmqPipelineProxies @2 (numberOfInstances :Int16) -> (zmqInputAddress :Text, zmqOutputAddress :Text);
    def newCloudViaZmqPipelineProxies_context(self, context):
        "# return the TCP addresses of two ZeroMQ proxies to access the given number of instances of the model"
        pass

    # newCloudViaProxy @3 (numberOfInstances :Int16) -> (proxy :AnyPointer);
    def newCloudViaProxy_context(self, context):
        "# return a model proxy acting as a gateway to the requested number of model instances"
        pass


def main():
    config = {
        "port": "6666"
    }
    # read commandline args only if script is invoked directly from commandline
    if len(sys.argv) > 1 and __name__ == "__main__":
        for arg in sys.argv[1:]:
            k, v = arg.split("=")
            if k in config:
                config[k] = v

    print("config:", config)

    # address = parse_args().address

    # server = capnp.TwoPartyServer("*:8000", bootstrap=DataServiceImpl("/home/berg/archive/data/"))
    # UserMasterImpl(AdminMasterImpl()))
    server = capnp.TwoPartyServer("*:" + config["port"], bootstrap=AdminMasterImpl())
    server.run_forever()


if __name__ == '__main__':
    main()
