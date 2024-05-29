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
from collections import defaultdict
import itertools
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
crop_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "crop.capnp"), imports=abs_imports)
common_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "common.capnp"), imports=abs_imports)
monica_params_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "model" / "monica" / "monica_params.capnp"),
                                 imports=abs_imports)
storage_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "storage.capnp"), imports=abs_imports)


LAST_SERVICE_SR_KEY_NAME = "last_service_sr"
LAST_ADMIN_SR_KEY_NAME = "last_admin_sr"
SERVICE_ITSELF_RESTORE_TOKEN = "monica_crop_service_itself"
ADMIN_RESTORE_TOKEN = "monica_crop_service_admin"


class Crop(crop_capnp.Crop.Server):

    def __init__(self, species_info, species_path, cult_info, cult_path, residue_path, entry_ref, id=None, name=None,
                 description=None):
        self._id = id if id else str(uuid.uuid4())
        self._name = name if name else id
        self._description = description if description else ""
        self._species_path = species_path
        self._cult_path = cult_path
        self._residue_path = residue_path
        self._params = None
        self._entry_ref = entry_ref
        self._species_info = species_info
        self._cultivar_info = cult_info

    def info_context(self, context):  # -> Common.IdInformation;
        r = context.results
        r.id = self._id
        cps = self.params.cropParams.cultivarParams
        r.name = self._name if not cps else cps.cultivarId
        r.description = self._description if not cps else cps.description

    def get_value(self, val_or_arr, expected_val_dim=0):

        def get_dim_of_first_value(arr):
            return 1 + (get_dim_of_first_value(arr[0]) if len(arr) > 0 else 0) if type(arr) is list else 0

        dim = get_dim_of_first_value(val_or_arr)
        if dim > expected_val_dim:
            return val_or_arr[0] if len(val_or_arr) > 0 else None
        else:
            return val_or_arr

    def create_species_params(self, j):
        sp = monica_params_capnp.SpeciesParameters.new_message()

        sp.speciesId = self.get_value(j.get("SpeciesName", ""))
        sp.carboxylationPathway = self.get_value(j.get("CarboxylationPathway", 0))
        sp.defaultRadiationUseEfficiency = self.get_value(j.get("DefaultRadiationUseEfficiency", 0))
        sp.partBiologicalNFixation = self.get_value(j.get("PartBiologicalNFixation", 0))
        sp.initialKcFactor = self.get_value(j.get("InitialKcFactor", 0))
        sp.luxuryNCoeff = self.get_value(j.get("LuxuryNCoeff", 0))
        sp.maxCropDiameter = self.get_value(j.get("MaxCropDiameter", 0))
        sp.stageAtMaxHeight = self.get_value(j.get("StageAtMaxHeight", 0))
        sp.stageAtMaxDiameter = self.get_value(j.get("StageAtMaxDiameter", 0))
        sp.minimumNConcentration = self.get_value(j.get("MinimumNConcentration", 0))
        sp.minimumTemperatureForAssimilation = self.get_value(j.get("MinimumTemperatureForAssimilation", 0))
        sp.optimumTemperatureForAssimilation = self.get_value(j.get("OptimumTemperatureForAssimilation", 0))
        sp.maximumTemperatureForAssimilation = self.get_value(j.get("MaximumTemperatureForAssimilation", 0))
        sp.nConcentrationAbovegroundBiomass = self.get_value(j.get("NConcentrationAbovegroundBiomass", 0))
        sp.nConcentrationB0 = self.get_value(j.get("NConcentrationB0", 0))
        sp.nConcentrationPN = self.get_value(j.get("NConcentrationPN", 0))
        sp.nConcentrationRoot = self.get_value(j.get("NConcentrationRoot", 0))
        sp.developmentAccelerationByNitrogenStress = self.get_value(j.get("DevelopmentAccelerationByNitrogenStress", 0))
        sp.fieldConditionModifier = self.get_value(j.get("FieldConditionModifier", 1))
        sp.assimilateReallocation = self.get_value(j.get("AssimilateReallocation", 0))

        sp.baseTemperature = self.get_value(j.get("BaseTemperature", []), expected_val_dim=1)
        sp.organMaintenanceRespiration = self.get_value(j.get("OrganMaintenanceRespiration", []), expected_val_dim=1)
        sp.organGrowthRespiration = self.get_value(j.get("OrganGrowthRespiration", []), expected_val_dim=1)
        sp.stageMaxRootNConcentration = self.get_value(j.get("StageMaxRootNConcentration", []), expected_val_dim=1)
        sp.initialOrganBiomass = self.get_value(j.get("InitialOrganBiomass", []), expected_val_dim=1)
        sp.criticalOxygenContent = self.get_value(j.get("CriticalOxygenContent", []), expected_val_dim=1)
        sp.stageMobilFromStorageCoeff = self.get_value(j.get("StageMobilFromStorageCoeff", []), expected_val_dim=1)

        sp.abovegroundOrgan = self.get_value(j.get("AbovegroundOrgan", []), expected_val_dim=1)
        sp.storageOrgan = self.get_value(j.get("StorageOrgan", []), expected_val_dim=1)

        sp.samplingDepth = self.get_value(j.get("SamplingDepth", 0))
        sp.targetNSamplingDepth = self.get_value(j.get("TargetNSamplingDepth", 0))
        sp.targetN30 = self.get_value(j.get("TargetN30", 0))
        sp.maxNUptakeParam = self.get_value(j.get("MaxNUptakeParam", 0))
        sp.rootDistributionParam = self.get_value(j.get("RootDistributionParam", 0))
        sp.plantDensity = self.get_value(j.get("PlantDensity", 0))
        sp.rootGrowthLag = self.get_value(j.get("RootGrowthLag", 0))
        sp.minimumTemperatureRootGrowth = self.get_value(j.get("MinimumTemperatureRootGrowth", 0))
        sp.initialRootingDepth = self.get_value(j.get("InitialRootingDepth", 0))
        sp.rootPenetrationRate = self.get_value(j.get("RootPenetrationRate", 0))
        sp.rootFormFactor = self.get_value(j.get("RootFormFactor", 0))
        sp.specificRootLength = self.get_value(j.get("SpecificRootLength", 0))
        sp.stageAfterCut = self.get_value(j.get("StageAfterCut", 0))
        sp.limitingTemperatureHeatStress = self.get_value(j.get("LimitingTemperatureHeatStress", 0))
        sp.cuttingDelayDays = self.get_value(j.get("CuttingDelayDays", 0))
        sp.droughtImpactOnFertilityFactor = self.get_value(j.get("DroughtImpactOnFertilityFactor", 0))

        sp.efMono = self.get_value(j.get("EF_MONO", 0.5))
        sp.efMonos = self.get_value(j.get("EF_MONOS", 0.5))
        sp.efIso = self.get_value(j.get("EF_ISO", 0))
        sp.vcMax25 = self.get_value(j.get("VCMAX25", 0))
        sp.aekc = self.get_value(j.get("AEKC", 65800))
        sp.aeko = self.get_value(j.get("AEKO", 1400))
        sp.aevc = self.get_value(j.get("AEVC", 68800))
        sp.kc25 = self.get_value(j.get("KC25", 460))
        sp.ko25 = self.get_value(j.get("KO25", 330))

        sp.transitionStageLeafExp = self.get_value(j.get("TransitionStageLeafExp", -1))

        return sp

    def create_cultivar_params(self, j):
        cp = monica_params_capnp.CultivarParameters.new_message()

        cp.cultivarId = self.get_value(j.get("CultivarName", ""))
        cp.description = self.get_value(j.get("Description", ""))
        cp.perennial = self.get_value(j.get("Perennial", False))

        cp.maxAssimilationRate = self.get_value(j.get("MaxAssimilationRate", 0))
        cp.maxCropHeight = self.get_value(j.get("MaxCropHeight", 0))
        cp.residueNRatio = self.get_value(j.get("ResidueNRatio", 0))
        cp.lt50cultivar = self.get_value(j.get("LT50cultivar", 0))

        cp.cropHeightP1 = self.get_value(j.get("CropHeightP1", 0))
        cp.cropHeightP2 = self.get_value(j.get("CropHeightP2", 0))
        cp.cropSpecificMaxRootingDepth = self.get_value(j.get("CropSpecificMaxRootingDepth", 0))

        cp.assimilatePartitioningCoeff = self.get_value(j.get("AssimilatePartitioningCoeff", []), expected_val_dim=2)
        cp.organSenescenceRate = self.get_value(j.get("OrganSenescenceRate", []), expected_val_dim=2)

        cp.baseDaylength = self.get_value(j.get("BaseDaylength", []), expected_val_dim=1)
        cp.optimumTemperature = self.get_value(j.get("OptimumTemperature", []), expected_val_dim=1)
        cp.daylengthRequirement = self.get_value(j.get("DaylengthRequirement", []), expected_val_dim=1)
        cp.droughtStressThreshold = self.get_value(j.get("DroughtStressThreshold", []), expected_val_dim=1)
        cp.specificLeafArea = self.get_value(j.get("SpecificLeafArea", []), expected_val_dim=1)
        cp.stageKcFactor = self.get_value(j.get("StageKcFactor", []), expected_val_dim=1)
        cp.stageTemperatureSum = self.get_value(j.get("StageTemperatureSum", []), expected_val_dim=1)
        cp.vernalisationRequirement = self.get_value(j.get("VernalisationRequirement", []), expected_val_dim=1)

        cp.heatSumIrrigationStart = self.get_value(j.get("HeatSumIrrigationStart", 0))
        cp.heatSumIrrigationEnd = self.get_value(j.get("HeatSumIrrigationEnd", 0))

        cp.criticalTemperatureHeatStress = self.get_value(j.get("CriticalTemperatureHeatStress", 0))
        cp.beginSensitivePhaseHeatStress = self.get_value(j.get("BeginSensitivePhaseHeatStress", 0))
        cp.endSensitivePhaseHeatStress = self.get_value(j.get("EndSensitivePhaseHeatStress", 0))

        cp.frostHardening = self.get_value(j.get("FrostHardening", 0))
        cp.frostDehardening = self.get_value(j.get("FrostDehardening", 0))
        cp.lowTemperatureExposure = self.get_value(j.get("LowTemperatureExposure", 0))
        cp.respiratoryStress = self.get_value(j.get("RespiratoryStress", 0))
        cp.latestHarvestDoy = self.get_value(j.get("LatestHarvestDoy", -1))

        cp.organIdsForPrimaryYield = [
            {
                "organId": yc.get("organId", -1),
                "yieldPercentage": yc.get("yieldPercentage", 0),
                "yieldDryMatter": yc.get("yieldDryMatter", 0)
            } for yc in self.get_value(j.get("OrganIdsForPrimaryYield", []), expected_val_dim=1)
        ]
        cp.organIdsForSecondaryYield = [
            {
                "organId": yc.get("organId", -1),
                "yieldPercentage": yc.get("yieldPercentage", 0),
                "yieldDryMatter": yc.get("yieldDryMatter", 0)
            } for yc in self.get_value(j.get("OrganIdsForSecondaryYield", []), expected_val_dim=1)
        ]
        cp.organIdsForCutting = [
            {
                "organId": yc.get("organId", -1),
                "yieldPercentage": yc.get("yieldPercentage", 0),
                "yieldDryMatter": yc.get("yieldDryMatter", 0)
            } for yc in self.get_value(j.get("OrganIdsForCutting", []), expected_val_dim=1)
        ]

        cp.earlyRefLeafExp = self.get_value(j.get("EarlyRefLeafExp", 12))
        cp.refLeafExp = self.get_value(j.get("RefLeafExp", 20))

        cp.minTempDevWE = self.get_value(j.get("MinTempDev_WE", 0))
        cp.optTempDevWE = self.get_value(j.get("OptTempDev_WE", 0))
        cp.maxTempDevWE = self.get_value(j.get("MaxTempDev_WE", 0))
        cp.winterCrop = self.get_value(j.get("WinterCrop", False))

        return cp

    def create_residue_params(self, j):
        rp = monica_params_capnp.CropResidueParameters.new_message()

        ps = rp.init("params")
        rp.species = self.get_value(j.get("species", ""))
        rp.residueType = self.get_value(j.get("residueType", ""))

        ps.aomDryMatterContent = self.get_value(j.get("AOM_DryMatterContent", 0))
        ps.aomFastDecCoeffStandard = self.get_value(j.get("AOM_FastDecCoeffStandard", 0))
        ps.aomNH4Content = self.get_value(j.get("AOM_NH4Content", 0))
        ps.aomNO3Content = self.get_value(j.get("AOM_NO3Content", 0))
        ps.aomSlowDecCoeffStandard = self.get_value(j.get("AOM_SlowDecCoeffStandard", 0))
        ps.cnRatioAOMFast = self.get_value(j.get("CN_Ratio_AOM_Fast", 0))
        ps.cnRatioAOMSlow = self.get_value(j.get("CN_Ratio_AOM_Slow", 0))
        ps.nConcentration = self.get_value(j.get("NConcentration", 0))
        ps.partAOMSlowToSMBFast = self.get_value(j.get("PartAOM_Slow_to_SMB_Fast", 0))
        ps.partAOMSlowToSMBSlow = self.get_value(j.get("PartAOM_Slow_to_SMB_Slow", 0))
        ps.partAOMToAOMFast = self.get_value(j.get("PartAOM_to_AOM_Fast", 0))
        ps.partAOMToAOMSlow = self.get_value(j.get("PartAOM_to_AOM_Slow", 0))

        return rp

    def species_context(self, context):  # species     @2 () -> (info :Common.IdInformation);
        context.results.info = self._species_info

    def cultivar_context(self, context):  # cultivar    @1 () -> (info :Common.IdInformation);
        context.results.info = self._cultivar_info

    @property
    def params(self):
        if not self._params:
            self._params = monica_params_capnp.CropSpec.new_message()
            cps = self._params.init("cropParams")

            with open(self._species_path) as _:
                j = json.load(_)
                cps.speciesParams = self.create_species_params(j)

            with open(self._cult_path) as _:
                j = json.load(_)
                cps.cultivarParams = self.create_cultivar_params(j)

            with open(self._residue_path) as _:
                j = json.load(_)
                self._params.residueParams = self.create_residue_params(j)

            if cps.cultivarParams and len(cps.cultivarParams.cultivarId) > 0:
                self._entry_ref.name = cps.cultivarParams.cultivarId

        return self._params

    def parameters(self, **kwargs):  # parameters @0 () -> (params :AnyPointer);
        return self.params


class Registry(reg_capnp.Registry.Server):

    def __init__(self, path_to_monica_parameters, id=None, name=None, description=None):
        self._id = id if id else str(uuid.uuid4())
        self._name = name if name else id
        self._description = description if description else ""
        self._path_to_monica_params = path_to_monica_parameters

        self._species_to_cultivars = defaultdict(
            list)  # list of cultivars for a given species { species: "", ref: Crop capability } which lazily load the actual parameters on first call

        crops_path = Path(self._path_to_monica_params) / "crops"
        for species_name in os.listdir(crops_path):
            species_path = crops_path / species_name
            residue_path = Path(self._path_to_monica_params) / "crop-residues" / (species_name + ".json")
            if os.path.isdir(species_path):
                for cult_fname in os.listdir(species_path):
                    cult_name = cult_fname[:-5]
                    if len(cult_name) == 0:
                        cult_name = species_name
                    cult_path = species_path / cult_fname
                    if not os.path.isdir(cult_path):
                        entry = reg_capnp.Registry.Entry(
                            categoryId=species_name,
                            name=cult_name
                        )
                        crop = Crop({"id": species_name, "name": species_name}, str(species_path) + ".json",
                                    {"id": cult_name, "name": cult_name}, str(cult_path), str(residue_path),
                                    entry, id=species_name + "_" + cult_name, name=species_name + "/" + cult_name)
                        entry.ref = crop
                        self._species_to_cultivars[species_name].append(entry)

    def info_context(self, context):  # -> Common.IdInformation;
        r = context.results
        r.id = self._id
        r.name = self._name
        r.description = self._description

    def supportedCategories(self, **kwargs):  # supportedCategories @0 () -> (cats :List(Common.IdInformation));
        species_names = self._species_to_cultivars.keys()
        species_names.sort()
        return list([{"id": name, "name": name} for name in species_names])

    def categoryInfo_context(self, context):  # categoryInfo @1 (categoryId :Text) -> Common.IdInformation;
        cat_id = context.params.categoryId
        if cat_id in self._species_to_cultivars:
            return {"id": cat_id, "name": cat_id}

    def entries(self, categoryId, **kwargs):  # entries @2 (categoryId :Text) -> (entries :List(Entry));
        if categoryId in self._species_to_cultivars:
            return self._species_to_cultivars[categoryId]
        elif categoryId is None or len(categoryId) == 0:
            return list(itertools.chain(*self._species_to_cultivars.values()))


async def main(path_to_monica_parameters, serve_bootstrap=True, host=None, port=None,
               id=None, name="MONICA Crop Parameters Service", description=None, use_async=False):
    config = {
        "path_to_monica_parameters": path_to_monica_parameters,
        "port": port,
        "host": host,
        "id": id,
        "name": name,
        "description": description,
        "serve_bootstrap": serve_bootstrap,
        "use_async": use_async,
        "restorer_container_sr": None,
        # "capnp://jXS22bpAGSjfksa0JkDI_092-h-bdZi4lKNBBgD7kWk=@10.10.24.218:40305/ZGVjODYxYWQtZmVkOS00YjEzLWJmNjQtNWU0OGRmYzhhYmZh",
        "service_container_sr": None,
        # "capnp://jXS22bpAGSjfksa0JkDI_092-h-bdZi4lKNBBgD7kWk=@10.10.24.218:40305/ZGVjODYxYWQtZmVkOS00YjEzLWJmNjQtNWU0OGRmYzhhYmZh"
    }
    common.update_config(config, sys.argv, print_config=True, allow_new_keys=False)

    restorer = common.Restorer()
    service = Registry(config["path_to_monica_parameters"],
                       id=config["id"], name=config["name"], description=config["description"])

    name_to_service = {"service": service}

    def restore_callback(restore_token):
        if restore_token == SERVICE_ITSELF_RESTORE_TOKEN:
            return name_to_service["service"]
        elif restore_token == ADMIN_RESTORE_TOKEN:
            return name_to_service.get("admin", None)
        else:
            return None

    restorer.restore_callback = restore_callback

    def load_last_or_store_services(service_container, name, service):
        (storage_key, restore_token) = {
            "service": (LAST_SERVICE_SR_KEY_NAME, SERVICE_ITSELF_RESTORE_TOKEN),
            "admin": (LAST_ADMIN_SR_KEY_NAME, ADMIN_RESTORE_TOKEN)
        }[name]
        entry_prom = service_container.getEntry(key=storage_key).entry
        entry_val = entry_prom.getValue().wait()
        if entry_val.isUnset:  # there was no set last token, so we need to create a new one
            save_res = restorer.save_str(service, create_unsave=False, restore_token=restore_token,
                                         store_sturdy_refs=True).wait()
            service_sr = save_res["sturdy_ref"]
            # keep the sturdy ref around for output to the user
            # and save the actual token as the one we used
            if entry_prom.setValue(value={"textValue": save_res["sr_token"]}).wait().success:
                return service_sr
        else:  # there was a previously stored token, so just create a sturdy ref for output from it
            service_sr_token = entry_val.value.textValue
            save_res = restorer.save_str(service, fixed_sr_token=service_sr_token, create_unsave=False,
                                         store_sturdy_refs=False).wait()
            return save_res["sturdy_ref"]

    if config["use_async"]:
        await serv.async_init_and_run_service(name_to_service, config["host"], config["port"],
                                              serve_bootstrap=config["serve_bootstrap"], restorer=restorer,
                                              restorer_container_sr=config["restorer_container_sr"],
                                              service_container_sr=config["service_container_sr"])
    else:
        serv.init_and_run_service(name_to_service, config["host"], config["port"],
                                  serve_bootstrap=config["serve_bootstrap"], restorer=restorer,
                                  restorer_container_sr=config["restorer_container_sr"],
                                  service_container_sr=config["service_container_sr"],
                                  load_last_or_store_services_callback=load_last_or_store_services)


if __name__ == '__main__':
    asyncio.run(main(str(PATH_TO_REPO.parent / "monica-parameters"), serve_bootstrap=True, use_async=True))
