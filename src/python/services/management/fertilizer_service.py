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
import common.common as common
import common.service as serv

PATH_TO_CAPNP_SCHEMAS = PATH_TO_REPO / "capnproto_schemas"
abs_imports = [str(PATH_TO_CAPNP_SCHEMAS)]
reg_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "registry.capnp"), imports=abs_imports)
common_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "common.capnp"), imports=abs_imports)
management_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "management.capnp"), imports=abs_imports)
monica_mgmt_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "model" / "monica" / "monica_management.capnp"), imports=abs_imports)
monica_params_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "model" / "monica" / "monica_params.capnp"), imports=abs_imports)

#------------------------------------------------------------------------------

class Service(monica_mgmt_capnp.FertilizerService.Server, common.Identifiable, common.Persistable, serv.AdministrableService): 

    def __init__(self, path_to_mineral_fertilizers_dir, path_organic_fertilizers_dir, 
        id=None, name=None, description=None, 
        admin=None, restorer=None):
        common.Identifiable.__init__(self, id, name, description)
        common.Persistable.__init__(self, restorer)
        serv.AdministrableService.__init__(self, admin)

        self._id = id if id else str(uuid.uuid4())
        self._name = name if name else id
        self._description = description if description else ""
        self._min_ferts_dir = path_to_mineral_fertilizers_dir
        self._org_ferts_dir = path_organic_fertilizers_dir
        self._min_ferts = {}
        self._all_min_ferts = {}
        self._org_ferts = {}
        self._all_org_ferts = {}
    

    def info_context(self, context): # -> Common.IdInformation;
        r = context.results
        r.id = self._id
        r.name = self._name
        r.description = self._description


    def get_value(self, val_or_array):
        return val_or_array[0] if type(val_or_array) is list and len(val_or_array) > 0 else val_or_array

    @property
    def mineral_fertilizers(self):
        if len(self._all_min_ferts) == 0:
            for fert in os.listdir(self._min_ferts_dir):
                path = Path(self._min_ferts_dir) / fert
                with open(path) as _:
                    fertj = json.load(_)
                    id = fertj.get("id", "")
                    name = fertj.get("name", "")
                    urea = self.get_value(fertj["Carbamid"])
                    ammonia = self.get_value(fertj["NH4"])
                    nitrate = self.get_value(fertj["NO3"])
                    ps = monica_mgmt_capnp.Params.MineralFertilization.Parameters(
                            carbamid=urea,
                            nh4=ammonia,
                            no3=nitrate)
                    vh = common.AnyValueHolder(ps, self.restorer)
                    e = monica_mgmt_capnp.FertilizerService.Entry.new_message(
                        info={"id": id, "name": name},
                        ref=vh)
                    self._all_min_ferts[id] = (e, vh)
        return self._all_min_ferts


    def availableMineralFertilizers_context(self, context): # availableMineralFertilizers @2 () -> (entries :List(Entry)); #(Params.MineralFertilization.Parameters)));
        context.results.entries = list([e for (e, _) in self.mineral_fertilizers.values()])


    def mineralFertilizer_context(self, context): # mineralFertilizer @4 (id :Text) -> (fert :Params.MineralFertilization.Parameters);
        id = context.params.id
        if id in self._all_min_ferts:
            context.results.fert = self._all_min_ferts[id][1].val

    @property
    def organic_fertilizers(self):
        if len(self._all_org_ferts) == 0:
            for fert in os.listdir(self._org_ferts_dir):
                path = Path(self._org_ferts_dir) / fert
                with open(path) as _:
                    fertj = json.load(_)
                    id = fertj.get("id", "")
                    name = fertj.get("name", "")
                    ps = monica_mgmt_capnp.Params.OrganicFertilization.OrganicMatterParameters(
                        aomDryMatterContent=self.get_value(fertj["AOM_DryMatterContent"]),
                        aomFastDecCoeffStandard=self.get_value(fertj["AOM_FastDecCoeffStandard"]),
                        aomNH4Content=self.get_value(fertj["AOM_NH4Content"]),
                        aomNO3Content=self.get_value(fertj["AOM_NO3Content"]),
                        aomSlowDecCoeffStandard=self.get_value(fertj["AOM_SlowDecCoeffStandard"]),
                        cnRatioAOMFast=self.get_value(fertj["CN_Ratio_AOM_Fast"]),
                        cnRatioAOMSlow=self.get_value(fertj["CN_Ratio_AOM_Slow"]),
                        nConcentration=self.get_value(fertj["NConcentration"]),
                        partAOMSlowToSMBFast=self.get_value(fertj["PartAOM_Slow_to_SMB_Fast"]),
                        partAOMSlowToSMBSlow=self.get_value(fertj["PartAOM_Slow_to_SMB_Slow"]),
                        partAOMToAOMFast=self.get_value(fertj["PartAOM_to_AOM_Fast"]),
                        partAOMToAOMSlow=self.get_value(fertj["PartAOM_to_AOM_Slow"])
                    )
                    vh = common.AnyValueHolder(ps, self.restorer)
                    e = monica_mgmt_capnp.FertilizerService.Entry.new_message(
                        info={"id": id, "name": name},
                        ref=vh)
                    self._all_org_ferts[id] = (e, vh)
        return self._all_org_ferts


    def availableOrganicFertilizers_context(self, context): # availableOrganicFertilizers @2 () -> (entries :List(Entry)); #(Params.MineralFertilization.Parameters)));
        context.results.entries = list([e for (e, _) in self.organic_fertilizers.values()])


    def organicFertilizer_context(self, context): # organicFertilizer @4 (id :Text) -> (fert :Params.OrganicFertilization.OrganicMatterParameters);
        id = context.params.id
        if id in self._all_org_ferts:
            context.results.fert = self._all_org_ferts[id][1].val


    def mineralFertilizerPartitionFor_context(self, context): # mineralFertilizerPartitionFor @0 (minFert :MineralFertilizer) -> (partition :Params.MineralFertilization.Parameters);
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


    def organicFertilizerParametersFor_context(self, context): # organicFertilizerParametersFor @1 (orgFert :OrganicFertilizer) -> (params :Params.OrganicFertilization.Parameters);
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

async def main(path_to_monica_parameters, serve_bootstrap=True, host=None, port=None, 
    id=None, name="MONICA Parameters Fertilizer Service", description=None, use_async=False):

    config = {
        "port": port, 
        "host": host,
        "id": id,
        "name": name,
        "description": description,
        "serve_bootstrap": serve_bootstrap,
        "path_to_min_ferts_dir": path_to_monica_parameters + "/mineral-fertilisers",
        "path_to_org_ferts_dir": path_to_monica_parameters + "/organic-fertilisers",
        "in_sr": None,
        "out_sr": None,
        "fbp": False,
        "no_fbp": False,
        "use_async": use_async,
        "to_attr": None, #"climate",
        "latlon_attr": "latlon",
        "start_date_attr": "startDate",
        "end_date_attr": "endDate",
        "mode": "sturdyref", # sturdyref | capability | data
    }
    common.update_config(config, sys.argv, print_config=True, allow_new_keys=False)

    restorer = common.Restorer()
    service = Service(config["path_to_min_ferts_dir"], config["path_to_org_ferts_dir"], 
        id=config["id"], name=config["name"], description=config["description"], restorer=restorer)
    if config["fbp"]:
        pass
        #fbp(config, climate_capnp.Service._new_client(service))
    else:
        if config["use_async"]:
            await serv.async_init_and_run_service({"service": service}, config["host"], config["port"], 
            serve_bootstrap=config["serve_bootstrap"], restorer=restorer)
        else:
            
            serv.init_and_run_service({"service": service}, config["host"], config["port"], 
                serve_bootstrap=config["serve_bootstrap"], restorer=restorer)

#------------------------------------------------------------------------------

if __name__ == '__main__':
    asyncio.run(main("../monica-parameters", serve_bootstrap=True, use_async=True)) 
