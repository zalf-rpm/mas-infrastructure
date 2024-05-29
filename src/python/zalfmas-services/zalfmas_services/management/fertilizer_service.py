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

# from datetime import date, timedelta
import asyncio
import capnp
import json
import os
from pathlib import Path
import sys
# import time
import uuid

PATH_TO_REPO = Path(os.path.realpath(__file__)).parent.parent.parent.parent.parent
if str(PATH_TO_REPO) not in sys.path:
    sys.path.insert(1, str(PATH_TO_REPO))

PATH_TO_PYTHON_CODE = PATH_TO_REPO / "src/python"
if str(PATH_TO_PYTHON_CODE) not in sys.path:
    sys.path.insert(1, str(PATH_TO_PYTHON_CODE))

from pkgs.common import common
from pkgs.common import service as serv

PATH_TO_CAPNP_SCHEMAS = PATH_TO_REPO / "capnproto_schemas"
abs_imports = [str(PATH_TO_CAPNP_SCHEMAS)]
reg_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "registry.capnp"), imports=abs_imports)
common_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "common.capnp"), imports=abs_imports)
management_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "management.capnp"), imports=abs_imports)
monica_mgmt_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "model" / "monica" / "monica_management.capnp"),
                               imports=abs_imports)


class MineralFertilizer(management_capnp.Fertilizer.Server, common.Identifiable, common.Persistable):

    def __init__(self, path_to_mineral_fertilizer_file,
                 id=None, name=None, description=None, restorer=None):
        common.Identifiable.__init__(self, id, name, description)
        common.Persistable.__init__(self, restorer)
        self._path_to_file = path_to_mineral_fertilizer_file
        self._parameters = None
        self.init_info_func = self.load_data

    def get_value(self, val_or_array):
        return val_or_array[0] if type(val_or_array) is list and len(val_or_array) > 0 else val_or_array

    def load_data(self):
        if self._parameters is None:
            with open(self._path_to_file) as _:
                fertj = json.load(_)
                self.id = fertj.get("id", "")
                self.name = fertj.get("name", "")
                self._parameters = monica_mgmt_capnp.Params.MineralFertilization.Parameters(
                    id=self.id,
                    name=self.name,
                    carbamid=self.get_value(fertj["Carbamid"]),
                    nh4=self.get_value(fertj["NH4"]),
                    no3=self.get_value(fertj["NO3"])
                )

    def nutrients_context(self, context):  # nutrients @0 () -> (nutrients :List(Nutrient));
        self.load_data()
        context.results.nutrients = [
            {"nutrient": "urea", "value": self._parameters.carbamid, "unit": "fraction"},
            {"nutrient": "ammonia", "value": self._parameters.nh4, "unit": "fraction"},
            {"nutrient": "nitrate", "value": self._parameters.no3, "unit": "fraction"},
        ]

    def parameters_context(self, context):  # parameters @1 () -> (params :AnyPointer);
        self.load_data()
        context.results.params = self._parameters


class OrganicFertilizer(management_capnp.Fertilizer.Server, common.Identifiable, common.Persistable):

    def __init__(self, path_to_organic_fertilizer_file,
                 id=None, name=None, description=None, restorer=None):
        common.Identifiable.__init__(self, id, name, description)
        common.Persistable.__init__(self, restorer)
        self._path_to_file = path_to_organic_fertilizer_file
        self._parameters = None
        self.init_info_func = self.load_data

    def get_value(self, val_or_array):
        return val_or_array[0] if type(val_or_array) is list and len(val_or_array) > 0 else val_or_array

    def load_data(self):
        if self._parameters is None:
            with open(self._path_to_file) as _:
                fertj = json.load(_)
                self.id = fertj.get("id", "")
                self.name = fertj.get("name", "")
                self._parameters = monica_mgmt_capnp.Params.OrganicFertilization.OrganicMatterParameters(
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

    def nutrients_context(self, context):  # nutrients @0 () -> (nutrients :List(Nutrient));
        self.load_data()
        context.results.nutrients = [
            {"nutrient": "ammonia", "value": self._parameters.aomNH4Content, "unit": "fraction"},
            {"nutrient": "nitrate", "value": self._parameters.aomNO3Content, "unit": "fraction"},
        ]

    def parameters_context(self, context):  # parameters @1 () -> (params :AnyPointer);
        self.load_data()
        context.results.params = self._parameters


class Service(reg_capnp.Registry.Server, common.Identifiable, common.Persistable, serv.AdministrableService):

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
        self._categories = {
            "mineral": "Mineral fertilizers",
            "organic": "Organic fertilizers",
        }
        self._cat_to_ferts = {}

    def supportedCategories_context(self, context):  # supportedCategories @0 () -> (cats :List(Common.IdInformation));
        context.results.cats = list([{"id": id, "name": name} for id, name in self._categories.items()])

    def categoryInfo_context(self, context):  # categoryInfo @1 (categoryId :Text) -> Common.IdInformation;
        id = context.params.categoryId
        if id in self._categories:
            context.results.id = id
            context.results.name = self._categories[id]

    def entries_context(self, context):  # entries @2 (categoryId :Text) -> (entries :List(Entry));
        id = context.params.categoryId
        ferts = self._cat_to_ferts.setdefault(id, self.create_fertilizers())
        context.results.entries = list(map(lambda f: {"categoryId": id, "name": f.name, "ref": f}, ferts))

    def create_fertilizers(self, catId):
        ferts = []
        if catId == "mineral":
            for fert in os.listdir(self._min_ferts_dir):
                path = Path(self._min_ferts_dir) / fert
                ferts.append(MineralFertilizer(path, restorer=self.restorer))
        elif catId == "organic":
            for fert in os.listdir(self._org_ferts_dir):
                path = Path(self._org_ferts_dir) / fert
                ferts.append(OrganicFertilizer(path, restorer=self.restorer))
        return ferts


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
        "use_async": use_async,
    }
    common.update_config(config, sys.argv, print_config=True, allow_new_keys=False)

    restorer = common.Restorer()
    service = Service(config["path_to_min_ferts_dir"], config["path_to_org_ferts_dir"],
                      id=config["id"], name=config["name"], description=config["description"], restorer=restorer)
    if config["use_async"]:
        await serv.async_init_and_run_service({"service": service}, config["host"], config["port"],
                                              serve_bootstrap=config["serve_bootstrap"], restorer=restorer)
    else:

        serv.init_and_run_service({"service": service}, config["host"], config["port"],
                                  serve_bootstrap=config["serve_bootstrap"], restorer=restorer)


if __name__ == '__main__':
    asyncio.run(main("../monica-parameters", serve_bootstrap=True, use_async=True))
