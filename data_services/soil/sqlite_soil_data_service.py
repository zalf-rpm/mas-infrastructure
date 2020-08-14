# This Source Code Form is subject to the terms of the Mozilla Public
# License, v. 2.0. If a copy of the MPL was not distributed with this
# file, You can obtain one at http://mozilla.org/MPL/2.0/. */

# Authors:
# Michael Berg-Mohnicke <michael.berg@zalf.de>
#
# Maintainers:
# Currently maintained by the authors.
#
# This file is part of the util library used by models created at the Institute of
# Landscape Systems Analysis at the ZALF.
# Copyright (C: Leibniz Centre for Agricultural Landscape Research (ZALF)

import os
from pathlib import Path
import sqlite3
import sys

from pyproj import Proj, Transformer

top_level_path = Path(os.path.realpath(__file__)).parent.parent.parent

this_repo_path = str(top_level_path)
if this_repo_path not in sys.path:
    sys.path.append(this_repo_path)
from common.python import rect_ascii_grid_management as grid_man, common

soil_path = str(top_level_path.parent / "util/soil")
if soil_path not in sys.path:
    sys.path.append(soil_path)
import soil_io3

import capnp
capnp.add_import_hook(additional_paths=["capnproto_schemas"])
import soil_data_capnp

#------------------------------------------------------------------------------

def set_param(param, name, value=None):
    "set the correct union parameter in capnp Parameters struct object given the parameter name and optionally value"

    if name == "KA5TextureClass":
        param.ka5SoilType = value if value else ""
    elif name == "Sand":
        param.sand = value if value else 0.0
    elif name == "Clay":
        param.clay = value if value else 0.0
    elif name == "Silt":
        param.silt = value if value else 0.0
    elif name == "pH":
        param.pH = value if value else 0.0
    elif name == "Sceleton":
        param.sceleton = value if value else 0.0
    elif name == "SoilOrganicCarbon":
        param.organicCarbon = value if value else 0.0
    elif name == "SoilOrganicMatter":
        param.organicMatter = value if value else 0.0
    elif name == "SoilBulkDensity":
        param.bulkDensity = value if value else 0.0
    elif name == "SoilRawDensity":
        param.rawDensity = value if value else 0.0
    elif name == "FieldCapacity":
        param.fieldCapacity = value if value else 0.0 
    elif name == "PermanentWiltingPoint":
        param.permanentWiltingPoint = value if value else 0.0
    elif name == "PoreVolume":
        param.saturation = value if value else 0.0
    elif name == "SoilMoisturePercentFC":
        param.initialSoilMoisture = value if value else 0.0
    elif name == "Lambda":
        param.soilWaterConductivityCoefficient = value if value else 0.0
    elif name == "SoilAmmonium":
        param.ammonium = value if value else 0.0
    elif name == "SoilNitrate":
        param.nitrate = value if value else 0.0
    elif name == "CN":
        param.cnRatio = value if value else 0.0
    elif name == "is_in_groundwater":
        param.isInGroundwater = value if value else False
    elif name == "is_impenetrable":
        param.isImpenetrable = value if value else False
    elif name == "Thickness":
        param.size = value if value else 0.0

def get_param(param):
    "get the soil_io3 parameter name from the given capnproto Parameter union struct object"

    return {    
        "ka5SoilType": "KA5TextureClass",
        "sand": "Sand", 
        "clay": "Clay",
        "silt": "Silt", 
        "pH": "pH", 
        "sceleton": "Sceleton", 
        "organicCarbon": "SoilOrganicCarbon", 
        "organicMatter": "SoilOrganicMatter", 
        "bulkDensity": "SoilBulkDensity", 
        "rawDensity": "SoilRawDensity", 
        "fieldCapacity": "FieldCapacity", 
        "permanentWiltingPoint": "PermanentWiltingPoint", 
        "saturation": "PoreVolume", 
        "initialSoilMoisture": "SoilMoisturePercentFC", 
        "soilWaterConductivityCoefficient": "Lambda",
        "ammonium": "SoilAmmonium", 
        "nitrate": "SoilNitrate", 
        "cnRatio": "CN", 
        "isInGroundwater": "is_in_groundwater", 
        "isImpenetrable": "is_impenetrable", 
        "size": "Thickness"
    }.get(param.which(), None)


class Service(soil_data_capnp.Soil.Service.Server):

    def __init__(self, path_to_sqlite_db, path_to_ascii_grid, grid_proj, id=None, name=None, description=None):
        self._path_to_sqlite_db = path_to_sqlite_db
        self._con = sqlite3.connect(self._path_to_sqlite_db)
        self._all_available_params = soil_io3.available_soil_parameters(self._con)
        if "Sand" in self._all_available_params and "Clay" in self._all_available_params:
            self._all_available_params.append("Silt")

        interpol = grid_man.create_interpolator_from_ascii_grid(path_to_ascii_grid)
        self._interpol = grid_man.interpolate_from_latlon(interpol, grid_proj)

        self._id = id if id else path_to_sqlite_db
        self._name = name if name else self._id
        self._description = description if description else ""
        self._cache = {}


    def info_context(self, context): # info @0 () -> IdInformation;
        r = context.results
        r.id = self._id
        r.name = self._name
        r.description = self._description


    def checkParameters(self, params): 
        aps = self._all_available_params

        names = []
        for param in params:
            name = get_param(param)
            if name in aps:
                names.append(name)
        return {"failed": len(names) < len(params), "names": names}


    def checkAvailableParameters_context(self, context): # checkAvailableParameters @2 Query -> Query.Result;
        p = context.params
        r = context.results
        aps = self._all_available_params

        mandatory = self.checkParameters(p.mandatory)
        r.failed = mandatory["failed"]
        r.mandatory.init(len(mandatory["names"]))
        for i, m_name in enumerate(mandatory["names"]):
            set_param(r.mandatory[i], m_name)

        optional = self.checkParameters(p.optional)
        r.optional.init(len(optional["names"]))
        for i, o_name in enumerate(optional["names"]):
            set_param(r.optional[i], o_name)


    def getAllAvailableParameters_context(self, context): # getAllAvailableParameters @3 () -> (params :List(Parameter));
        r = context.results
        aps = self._all_available_params
        
        r.params.init(len(aps))
        for i, param_name in enumerate(aps):
            set_param(r.params[i], param_name)


    def profilesAt_context(self, context): # profilesAt @0 (coord :Geo.LatLonCoord, query :Query) -> (profiles :List(Profile));
        ps = context.params
        r = context.results
        q = ps.query

        mandatory = self.checkParameters(q.mandatory)
        names = mandatory["names"]
        if mandatory["failed"]:
            return
        optional = self.checkParameters(q.optional)
        names.extend(optional["names"])
        if "Thickness" not in names:
            names.append("Thickness")

        soil_id = self._interpol(ps.coord.lat, ps.coord.lon)
        if soil_id in self._cache:
            sp = self._cache[soil_id]
        else:
            sp = soil_io3.soil_parameters(self._con, soil_id)
            self._cache[soil_id] = sp

        r.profiles.init(1)
        p = r.profiles[0]
        p.profile.init(len(names))

        prev_depth = 0
        for layer in sp:
            for i, name in enumerate(names):
                if name == "Thickness":
                    set_param(p.profile[i], name, layer["Thickness"] - prev_depth)
                    prev_depth = layer["Thickness"]
                elif name == "Silt":
                    set_param(p.profile[i], "Silt", 1 - layer["Sand"] - layer["Clay"])
                else:
                    set_param(p.profile[i], name, layer[name])


    def allProfiles_context(self, context): # allProfiles @1 Query -> (profiles :List(Common.Pair(Geo.LatLonCoord, List(Common.CapHolder(Profile)))));    
        pass


def main(path_to_sqlite_db, path_to_ascii_soil_grid, grid_proj, server="*", port=11002, id=None, name=None, description=None):

    config = {
        "port": str(port),
        "server": server,
        "path_to_sqlite_db": path_to_sqlite_db,
        "path_to_ascii_soil_grid": path_to_ascii_soil_grid,
        "grid_proj": grid_proj,
        "id": id,
        "name": name,
        "description": description
    }
    # read commandline args only if script is invoked directly from commandline
    if len(sys.argv) > 1 and __name__ == "__main__":
        for arg in sys.argv[1:]:
            k, v = arg.split("=")
            if k in config:
                config[k] = v

    server = capnp.TwoPartyServer(config["server"] + ":" + config["port"],
                                  bootstrap=Service(path_to_sqlite_db=config["path_to_sqlite_db"],
                                  path_to_ascii_soil_grid=config["path_to_ascii_soil_grid"],
                                  grid_proj=grid_man.name_to_proj(config["grid_proj"]),
                                  id=config["id"], name=config["name"], description=config["description"]))
    server.run_forever()


if __name__ == '__main__':
    main()

