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

#from datetime import date, timedelta
import asyncio
import capnp
import json
import os
from pathlib import Path
import sys
#import time
import uuid

PATH_TO_REPO = Path(os.path.realpath(__file__)).parent.parent.parent.parent.parent
if str(PATH_TO_REPO) not in sys.path:
    sys.path.insert(1, str(PATH_TO_REPO))

PATH_TO_PYTHON_CODE = PATH_TO_REPO / "src/python"
if str(PATH_TO_PYTHON_CODE) not in sys.path:
    sys.path.insert(1, str(PATH_TO_PYTHON_CODE))

import common.capnp_async_helpers as async_helpers

abs_imports = ["capnproto_schemas"]
reg_capnp = capnp.load("capnproto_schemas/registry.capnp", imports=abs_imports)
common_capnp = capnp.load("capnproto_schemas/common.capnp", imports=abs_imports)
management_capnp = capnp.load("capnproto_schemas/management.capnp", imports=abs_imports)
monica_params_capnp = capnp.load("capnproto_schemas/monica/monica_params.capnp", imports=abs_imports)

#------------------------------------------------------------------------------

class Service(management_capnp.FertilizerService.Server): 

    def __init__(self, path_to_mineral_fertilizers_dir, path_organic_fertilizers_dir, id=None, name=None, description=None,):
        self._id = id if id else str(uuid.uuid4())
        self._name = name if name else id
        self._description = description if description else ""
        self._min_ferts_dir = path_to_mineral_fertilizers_dir
        self._org_ferts_dir = path_organic_fertilizers_dir
        self._min_ferts = {}
        self._org_ferts = {}
    

    def info(self):
        return common_capnp.Common.IdInformation.new_message(id=self._id, name=self._name, description=self._description) 


    def info_context(self, context): # -> (info :IdInformation);
        context.results.info = self.info()


    def get_value(self, val_or_array):
        return val_or_array[0] if type(val_or_array) is list and len(val_or_array) > 0 else val_or_array


    def mineralFertilizerPartitionFor_context(self, context): # mineralFertilizerPartitionFor @0 (minFert :MineralFertilizer) -> (partition ::MonicaParams.MineralFertilizerParameters);
        mf = context.params.minFert
        if mf in self._min_ferts:
            context.results.partition = self._min_ferts[mf]
        else:
            try:
                path = Path(self._min_ferts_dir) / (str(mf).upper() + ".json")
                with open(path) as _:
                    fertj = json.load(_)
                    part = {
                        "id": fertj.get("id", ""),
                        "name": fertj.get("name", ""),
                        "carbamid": self.get_value(fertj["Carbamid"]),
                        "nh4": self.get_value(fertj["NH4"]),
                        "no3": self.get_value(fertj["NO3"])
                    }
                    self._min_ferts[mf] = part
                    context.results.partition = part
            except Exception as e:
                print("Error: Mineral fertilizer data couldn't be loaded from", str(path), "! Exception:", e)


    def organicFertilizerParametersFor_context(self, context): # organicFertilizerParametersFor @1 (orgFert :OrganicFertilizer) -> (params :MonicaParams.OrganicFertilizerParameters);
        of = context.params.orgFert
        if of in self._org_ferts:
            context.results.params = self._org_ferts[of]
        else:
            try:
                path = Path(self._org_ferts_dir) / (str(of).upper() + ".json")
                with open(path) as _:
                    fertj = json.load(_)
                    params = {
                        "id": fertj.get("id", ""),
                        "name": fertj.get("name", ""),
                        "params": {
                            "aomDryMatterContent": self.get_value(fertj["AOM_DryMatterContent"]),
                            "aomFastDecCoeffStandard": self.get_value(fertj["AOM_FastDecCoeffStandard"]),
                            "aomNH4Content": self.get_value(fertj["AOM_NH4Content"]),
                            "aomNO3Content": self.get_value(fertj["AOM_NO3Content"]),
                            "aomSlowDecCoeffStandard": self.get_value(fertj["AOM_SlowDecCoeffStandard"]),
                            "cnRatioAOMFast": self.get_value(fertj["CN_Ratio_AOM_Fast"]),
                            "cnRatioAOMSlow": self.get_value(fertj["CN_Ratio_AOM_Slow"]),
                            "nConcentration": self.get_value(fertj["NConcentration"]),
                            "partAOMSlowToSMBFast": self.get_value(fertj["PartAOM_Slow_to_SMB_Fast"]),
                            "partAOMSlowToSMBSlow": self.get_value(fertj["PartAOM_Slow_to_SMB_Slow"]),
                            "partAOMToAOMFast": self.get_value(fertj["PartAOM_to_AOM_Fast"]),
                            "partAOMToAOMSlow": self.get_value(fertj["PartAOM_to_AOM_Slow"])
                        }
                    }
                    self._org_ferts[of] = params
                    context.results.params = params
            except Exception as e:
                print("Error: Organic fertilizer data couldn't be loaded from", str(path), "! Exception:", e)


#------------------------------------------------------------------------------

def main(server="*", port=13001, path_to_monica_parameters="../monica-parameters",
id=None, name="MONICA Parameters Fertilizer Service", description=None):

    config = {
        "port": str(port),
        "server": server,
        "id": id,
        "name": name,
        "description": description,
        "path_to_min_ferts_dir": path_to_monica_parameters + "/mineral-fertilisers",
        "path_to_org_ferts_dir": path_to_monica_parameters + "/organic-fertilisers"
    }
    # read commandline args only if script is invoked directly from commandline
    if len(sys.argv) > 1 and __name__ == "__main__":
        for arg in sys.argv[1:]:
            k, v = arg.split("=")
            if k in config:
                config[k] = v
    print(config)

    server = capnp.TwoPartyServer(config["server"] + ":" + config["port"],
                                  bootstrap=Service(config["path_to_min_ferts_dir"], config["path_to_org_ferts_dir"],
                                  id=config["id"], name=config["name"], description=config["description"]))
    server.run_forever()

#------------------------------------------------------------------------------

async def async_main(path_to_monica_parameters, serve_bootstrap=False,
host="0.0.0.0", port=None, reg_sturdy_ref=None, id=None, name="MONICA Parameters Fertilizer Service", description=None):

    config = {
        "path_to_min_ferts_dir": path_to_monica_parameters + "/mineral-fertilisers",
        "path_to_org_ferts_dir": path_to_monica_parameters + "/organic-fertilisers",
        "host": host,
        "port": str(port),
        "id": id,
        "name": name,
        "description": description,
        "reg_sturdy_ref": reg_sturdy_ref,
        "serve_bootstrap": str(serve_bootstrap),
        "reg_category": "fertilizer",
    }
    # read commandline args only if script is invoked directly from commandline
    if len(sys.argv) > 1 and __name__ == "__main__":
        for arg in sys.argv[1:]:
            k, v = arg.split("=")
            if k in config:
                config[k] = v
    print("config used:", config)

    conMan = async_helpers.ConnectionManager()

    service = Service(config["path_to_min_ferts_dir"], config["path_to_org_ferts_dir"],
    id=config["id"], name=config["name"], description=config["description"])    

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
    asyncio.run(async_main("../monica-parameters", serve_bootstrap=True, port=13001))

