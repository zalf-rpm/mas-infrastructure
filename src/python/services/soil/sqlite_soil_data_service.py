#!/usr/bin/python
# -*- coding: UTF-8

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

#remote debugging via commandline
#-m ptvsd --host 0.0.0.0 --port 14000 --wait

import asyncio
import capnp
import logging
import os
from pathlib import Path
from pyproj import CRS, Transformer
import socket
import sqlite3
import socket
import sys
#import time
import uuid

PATH_TO_REPO = Path(os.path.realpath(__file__)).parent.parent.parent.parent.parent
if str(PATH_TO_REPO) not in sys.path:
    sys.path.insert(1, str(PATH_TO_REPO))

PATH_TO_PYTHON_CODE = PATH_TO_REPO / "src/python"
if str(PATH_TO_PYTHON_CODE) not in sys.path:
    sys.path.insert(1, str(PATH_TO_PYTHON_CODE))

import common.common as common
import common.rect_ascii_grid_management as grid_man
import common.geo as geo
import common.capnp_async_helpers as async_helpers

PATH_TO_UTIL_SOIL = PATH_TO_REPO.parent / "util/soil"
if str(PATH_TO_UTIL_SOIL) not in sys.path:
    sys.path.insert(1, str(PATH_TO_UTIL_SOIL))
import soil_io3

abs_imports = ["capnproto_schemas"]
soil_data_capnp = capnp.load("capnproto_schemas/soil_data.capnp", imports=abs_imports) 
common_capnp = capnp.load("capnproto_schemas/common.capnp", imports=abs_imports) 
reg_capnp = capnp.load("capnproto_schemas/registry.capnp", imports=abs_imports)

#------------------------------------------------------------------------------

def set_capnp_prop_name_via_monica_name(param, name, value=None):
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

#------------------------------------------------------------------------------

CAPNP_PROP_to_MONICA_PARAM_NAME = {    
    "soilType": "KA5TextureClass",
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
    "soilMoisture": "SoilMoisturePercentFC", 
    "soilWaterConductivityCoefficient": "Lambda",
    "ammonium": "SoilAmmonium", 
    "nitrate": "SoilNitrate", 
    "cnRatio": "CN", 
    "inGroundwater": "is_in_groundwater", 
    "impenetrable": "is_impenetrable"
}

#------------------------------------------------------------------------------

class Service(soil_data_capnp.Soil.Service.Server): 

    def __init__(self, path_to_sqlite_db, path_to_ascii_grid, grid_crs, id=None, name=None, description=None, port=None):
        self._path_to_sqlite_db = path_to_sqlite_db
        self._path_to_ascii_grid = path_to_ascii_grid
        self._con = sqlite3.connect(self._path_to_sqlite_db)
        self._grid_crs = grid_crs

        self._all_available_params_raw = None
        self._all_available_params_derived = None

        self._interpol_and_latlon_coords = None
        self._all_latlon_coords = None
        self._latlon_to_capholders = {}

        self._id = str(id if id else uuid.uuid4())
        self._name = name if name else self._path_to_sqlite_db
        self._description = description if description else ""
        self._cache_raw = {}
        self._cache_derived = {}

        self._capnp_prop_to_monica_param_name = CAPNP_PROP_to_MONICA_PARAM_NAME
        self._monica_param_to_capnp_prop_name = {value : key for key, value in CAPNP_PROP_to_MONICA_PARAM_NAME.items()}

        self._issued_sr_tokens = []
        self._host = socket.getfqdn() #gethostname()
        self._port = port


    @property
    def port(self):
        return self._port
    
    @port.setter
    def port(self, p):
        self._port = p


    @property
    def interpol_and_latlon_coords(self):
        # create interpolator
        if not self._interpol_and_latlon_coords:
            rect_interpol, points_to_value = grid_man.create_interpolator_from_ascii_grid(self._path_to_ascii_grid)
            latlon_interpol = grid_man.interpolate_from_latlon(rect_interpol, self._grid_crs)
            all_latlon_coords = grid_man.rect_coordinates_to_latlon(self._grid_crs, points_to_value.keys())
            self._interpol_and_latlon_coords = (latlon_interpol, all_latlon_coords)
        return self._interpol_and_latlon_coords


    @property
    def interpolator(self):
        return self.interpol_and_latlon_coords[0]


    @property
    def all_latlon_coords(self):
        return self.interpol_and_latlon_coords[1]


    @property
    def all_available_params_derived(self):
        if not self._all_available_params_derived:
            params = soil_io3.available_soil_parameters(self._con, only_raw_data=False)
            self._all_available_params_derived = {
                "mandatory": list(filter(None, map(lambda p: self._monica_param_to_capnp_prop_name.get(p, None), params["mandatory"]))),
                "optional": list(filter(None, map(lambda p: self._monica_param_to_capnp_prop_name.get(p, None), params["optional"])))
            }
        return self._all_available_params_derived


    @property
    def all_available_params_raw(self):
        if not self._all_available_params_raw:
            params = soil_io3.available_soil_parameters(self._con, only_raw_data=True)
            self._all_available_params_raw = {
                "mandatory": list(filter(None, map(lambda p: self._monica_param_to_capnp_prop_name.get(p, None), params["mandatory"]))),
                "optional": list(filter(None, map(lambda p: self._monica_param_to_capnp_prop_name.get(p, None), params["optional"])))
            }
        return self._all_available_params_raw


    def info_context(self, context): # info @0 () -> IdInformation;
        r = context.results
        r.id = self._id
        r.name = self._name
        r.description = self._description


    def check_params_are_available(self, mandatory, optional, only_raw_data): 
        aps = self.all_available_params_raw if only_raw_data else self.all_available_params_derived

        avail_mandatory = list(filter(lambda p: p in aps["mandatory"], mandatory))
        avail_optional = list(filter(lambda p: p in aps["mandatory"] or p in aps["optional"], optional))
        failed = len(avail_mandatory) < len(mandatory)

        return {
            "failed": failed, 
            "mandatory": avail_mandatory,
            "optional": avail_optional
        }


    def checkAvailableParameters_context(self, context): # checkAvailableParameters @2 Query -> Query.Result;
        p = context.params
        r = context.results

        avail = self.check_params_are_available(p.mandatory, p.optional, p.onlyRawData)
        r.mandatory = avail["mandatory"]
        r.optional = avail["optional"]
        r.failed = avail["failed"]


    def getAllAvailableParameters_context(self, context): # getAllAvailableParameters @3 () -> (mandatory :List(PropertyName), optional :List(PropertyName));
        r = context.results
        aps = self.all_available_params_raw if context.params.onlyRawData else self.all_available_params_derived
        
        r.mandatory = aps["mandatory"]
        r.optional = aps["optional"]


    def profiles_at(self, lat, lon, profile, avail_props, only_raw_data): 
        if len(avail_props) > 0:
            soil_id = self.interpolator(lat, lon)
            cache = self._cache_raw if only_raw_data else self._cache_derived
            if soil_id in cache:
                sp = cache[soil_id]
            else:
                profiles = soil_io3.get_soil_profile(self._con, int(soil_id), only_raw_data=only_raw_data, no_units=True)
                sp = profiles[0][1]
                cache[soil_id] = sp
        else:
            sp = []

        profile.init("layers", len(sp))

        for k, layer in enumerate(sp):
            l = profile.layers[k]
            l.size = layer["Thickness"]
            if "description" in sp:
                l.description = layer["description"]
            props = l.init("properties", len(avail_props))
            for i, prop in enumerate(avail_props):
                monica_param = self._capnp_prop_to_monica_param_name.get(prop, None)
                if monica_param:
                    props[i].name = prop
                    value = layer[monica_param]
                    if prop == "impenetrable" or prop == "inGroundwater":
                        props[i].bValue = value
                    elif prop == "soilType":
                        props[i].type = value
                    else:
                        props[i].f32Value = value


    def available_properties(self, query):
        """
        Get all the names of the parameters requested in the query.
        If a mandatory param is not available return no names, to indicate failure. 
        """

        res = self.check_params_are_available(query.mandatory, query.optional, query.onlyRawData)
        if res["failed"]:
            return []
        names = res["mandatory"].copy()
        names.extend(res["optional"])
        return names


    def profilesAt_context(self, context): # profilesAt @0 (coord :Geo.LatLonCoord, query :Query) -> (profiles :List(Profile));
        r = context.results
        ps = context.params
        c = ps.coord
        q = ps.query

        avail_props = self.available_properties(q)

        # at the moment just support one soil profile per coordinate
        r.init("profiles", 1)

        # fill profile with data
        self.profiles_at(c.lat, c.lon, r.profiles[0], avail_props, q.onlyRawData)

    """
    def allLocations_context(self, context): # allLocations @1 Query -> (profiles :List(Common.Pair(Geo.LatLonCoord, List(Common.CapHolder(Profile)))));    
        r = context.results
        q = context.params

        names = self.queried_names(q)

        r.init("profiles", len(self.all_latlon_coords))
        for i, latlon in enumerate(self.all_latlon_coords):
            p = r.profiles[i]
            p.fst.lat = latlon[0]
            p.fst.lon = latlon[1]
            pchs = p.init("snd", 1)
            pchs[0] = ProfileCapHolder(self, latlon, names, q.onlyRawData, self._latlon_to_capholders)
            self._latlon_to_capholders[latlon] = p.snd[0] #store reference, so the object won't be garbage collected immediately
    """

    def save_context(self, context): # save @0 SaveParams -> SaveResults;
        if self.port:
            id = uuid.uuid4()
            self._issued_sr_tokens.append(str(id))
            context.results.sturdyRef = "capnp://insecure@{host}:{port}/{sr_token}".format(host=self._host, port=self.port, sr_token=id)
        

    def restore_context(self, context): # restore @0 (srToken :Token, owner :SturdyRef.Owner) -> (cap :Capability);
        if context.params.srToken in self._issued_sr_tokens:
            context.results.cap = self


    def drop_context(self, context): # drop @1 (srToken :Token, owner :SturdyRef.Owner);
        self._issued_sr_tokens.remove(context.params.srToken)

#------------------------------------------------------------------------------

async def async_main(path_to_sqlite_db, path_to_ascii_soil_grid, grid_crs, serve_bootstrap=False,
host="0.0.0.0", port=None, reg_sturdy_ref=None, id=None, name=None, description=None):

    config = {
        "host": host,
        "port": port,
        "path_to_sqlite_db": path_to_sqlite_db,
        "path_to_ascii_soil_grid": path_to_ascii_soil_grid,
        "grid_crs": grid_crs,
        "id": id,
        "name": name,
        "description": description,
        "reg_sturdy_ref": reg_sturdy_ref,
        "serve_bootstrap": str(serve_bootstrap),
        "reg_category": "soil",
    }
    # read commandline args only if script is invoked directly from commandline
    if len(sys.argv) > 1 and __name__ == "__main__":
        for arg in sys.argv[1:]:
            k, v = arg.split("=")
            if k in config:
                config[k] = v
    print("config used:", config)

    conMan = async_helpers.ConnectionManager()

    service = Service(
        path_to_sqlite_db=config["path_to_sqlite_db"],
        path_to_ascii_grid=config["path_to_ascii_soil_grid"],
        grid_crs=geo.name_to_proj(config["grid_crs"]),
        id=config["id"], name=config["name"], description=config["description"])

    if config["reg_sturdy_ref"]:
        registrator = await conMan.try_connect(config["reg_sturdy_ref"], cast_as=reg_capnp.Registrator)
        if registrator:
            unreg = await registrator.register(ref=service, categoryId=config["reg_category"]).a_wait()
            print("Registered ", config["name"], "soil service")
            #await unreg.unregister.unregister().a_wait()
        else:
            print("Couldn't connect to registrator at sturdy_ref:", config["reg_sturdy_ref"])

    if config["serve_bootstrap"].upper() == "TRUE":
        await async_helpers.serve_forever(config["host"], config["port"], service)
    else:
        await conMan.manage_forever()

#------------------------------------------------------------------------------

if __name__ == '__main__':
    db = "data/soil/buek1000.sqlite"
    grid = "data/soil/buek1000_1000_gk5.asc"
    crs = "gk5"

    asyncio.run(async_main(db, grid, crs, serve_bootstrap=True))


