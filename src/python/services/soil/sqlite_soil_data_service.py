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
import json
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
import common.service as serv
import common.capnp_async_helpers as async_helpers
import soil_io3

PATH_TO_CAPNP_SCHEMAS = PATH_TO_REPO / "capnproto_schemas"
abs_imports = [str(PATH_TO_CAPNP_SCHEMAS)]
soil_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "soil.capnp"), imports=abs_imports) 
common_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "common.capnp"), imports=abs_imports)
geo_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "geo.capnp"), imports=abs_imports)

#------------------------------------------------------------------------------

def fbp(config, service : soil_capnp.Service):
    conman = common.ConnectionManager()
    inp = conman.try_connect(config["in_sr"], cast_as=common_capnp.Channel.Reader, retry_secs=1)
    outp = conman.try_connect(config["out_sr"], cast_as=common_capnp.Channel.Writer, retry_secs=1)
    mandatory = json.loads(config["mandatory"])

    try:
        if inp and outp and service:
            while True:
                in_msg = inp.read().wait()
                if in_msg.which() == "done":
                    break

                in_ip = in_msg.value.as_struct(common_capnp.IP)
                attr = common.get_fbp_attr(in_ip, config["from_attr"])
                if attr:
                    coord = attr.as_struct(geo_capnp.LatLonCoord)
                else:
                    coord = in_ip.content.as_struct(geo_capnp.LatLonCoord)

                profiles = service.profilesAt(coord, {"mandatory": mandatory, "onlyRawData": False}).wait().profiles
                if len(profiles) > 0:
                    profile = profiles[0]

                    out_ip = common_capnp.IP.new_message()
                    if not config["to_attr"]:
                        out_ip.content = profile
                    common.copy_fbp_attr(in_ip, out_ip, config["to_attr"], profile)
                    outp.write(value=out_ip).wait()
            
            outp.write(done=None).wait()

    except Exception as e:
        print("sqlite_soil_data_service.py ex:", e)

    print("dwd_germany_service.py: exiting FBP component")


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

class Service(soil_capnp.Service.Server, common.Identifiable, common.Persistable, serv.AdministrableService): 

    def __init__(self, path_to_sqlite_db, path_to_ascii_grid, grid_crs, id=None, name=None, description=None, admin=None, restorer=None):
        common.Identifiable.__init__(self, id, name, description)
        common.Persistable.__init__(self, restorer)
        serv.AdministrableService.__init__(self, admin)

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
            params = soil_io3.available_soil_parameters_group(self._con, only_raw_data=False)
            self._all_available_params_derived = {
                "mandatory": list(filter(None, map(lambda p: self._monica_param_to_capnp_prop_name.get(p, None), params["mandatory"]))),
                "optional": list(filter(None, map(lambda p: self._monica_param_to_capnp_prop_name.get(p, None), params["optional"])))
            }
        return self._all_available_params_derived


    @property
    def all_available_params_raw(self):
        if self._all_available_params_raw is None:
            params = soil_io3.available_soil_parameters_group(self._con, only_raw_data=True)
            #print("params:", params)
            self._all_available_params_raw = {
                "mandatory": list(filter(None, map(lambda p: self._monica_param_to_capnp_prop_name.get(p, None), params["mandatory"]))),
                "optional": list(filter(None, map(lambda p: self._monica_param_to_capnp_prop_name.get(p, None), params["optional"])))
            }
        return self._all_available_params_raw


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


    def profiles_at(self, lat, lon, avail_props, only_raw_data):
        if len(avail_props) > 0:
            try:
                soil_id = self.interpolator(lat, lon)
            except:
                return
            cache = self._cache_raw if only_raw_data else self._cache_derived
            if soil_id in cache:
                sps = cache[soil_id]
            else:
                sp_groups = soil_io3.get_soil_profile_group(self._con, int(soil_id), only_raw_data=only_raw_data, no_units=True)
                # because of given soil_id we expect only one profile group (with potentially many profiles)
                sps = sp_groups[0]
                cache[soil_id] = sps
        else:
            return

        profiles = []#results.init("profiles", len(sps[1]))
        profile_group_id = sps[0]
        for j, sp in enumerate(sps[1]):
            profile = soil_capnp.Profile.new_message()#profiles[j] 
            profiles.append(profile)
            profile.id = str(profile_group_id) + "_" + str(sp["id"])
            profile.percentageOfArea = sp["avg_range_percentage_in_group"]
            
            layers = sp["layers"]
            profile.init("layers", len(layers))
            for k, layer in enumerate(layers):
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
                        elif prop == "sand" or prop == "clay" or prop == "silt":
                            props[i].f32Value = value * 100.0
                        elif prop == "sceleton" or prop == "fieldCapacity" or prop == "permanentWiltingPoint" or prop == "saturation":
                            props[i].f32Value = value * 100.0
                        elif prop == "soilmoisture":
                            props[i].f32Value = value * 100.0
                        else:
                            props[i].f32Value = value

        profiles.sort(key=lambda p: p.percentageOfArea, reverse=True)
        return profiles


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


    def profilesAt(self, coord, query, **kwargs): # profilesAt @0 (coord :Geo.LatLonCoord, query :Query) -> (profiles :List(Profile));
        avail_props = self.available_properties(query)
        profiles = self.profiles_at(coord.lat, coord.lon, avail_props, query.onlyRawData)
        return profiles

        

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

#------------------------------------------------------------------------------

async def main(path_to_sqlite_db, path_to_ascii_soil_grid, grid_crs=None, grid_epsg=None, serve_bootstrap=True, host=None, port=None, 
    id=None, name="Soil service", description=None, use_async=False):

    config = {
        "path_to_sqlite_db": path_to_sqlite_db,
        "path_to_ascii_soil_grid": path_to_ascii_soil_grid,
        "grid_crs": grid_crs,
        "grid_epsg": grid_epsg,
        "port": port, 
        "host": host,
        "id": id,
        "name": name,
        "description": description,
        "serve_bootstrap": serve_bootstrap,
        "use_async": use_async,
        "fbp": False,
        "in_sr": None,
        "out_sr": None,
        "mandatory": """["soilType","organicCarbon","rawDensity"]""",
        "to_attr": None, #"soil",
        "from_attr": None, #"latlon"
    }
    # read commandline args only if script is invoked directly from commandline
    if len(sys.argv) > 1 and __name__ == "__main__":
        for arg in sys.argv[1:]:
            k, v = arg.split("=")
            if k in config:
                config[k] = bool(v) if v.lower() in ["true", "false"] else v 
    print(config)

    if config["grid_epsg"]:
        grid_crs = CRS.from_epsg(int(config["grid_epsg"]))
    elif config["grid_crs"]:
        grid_crs = geo.name_to_crs(config["grid_crs"])
    else:
        try:
            epsg = int(Path(config["path_to_ascii_soil_grid"]).name.split("_")[2])
            grid_crs = CRS.from_epsg(epsg)
        except:
            print("Couldn't create CRS from soil grid name:", config["path_to_ascii_soil_grid"])
            exit(0)

    restorer = common.Restorer()
    service = Service(
        path_to_sqlite_db=config["path_to_sqlite_db"],
        path_to_ascii_grid=config["path_to_ascii_soil_grid"],
        grid_crs=grid_crs,
        id=config["id"], name=config["name"], description=config["description"], restorer=restorer)
    if config["fbp"]:
        fbp(config, soil_capnp.Service._new_client(service))
    else:
        if config["use_async"]:
            await serv.async_init_and_run_service({"service": service}, config["host"], config["port"], 
            serve_bootstrap=config["serve_bootstrap"], restorer=restorer)
        else:
            serv.init_and_run_service({"service": service}, config["host"], config["port"], 
                serve_bootstrap=config["serve_bootstrap"], restorer=restorer)

#------------------------------------------------------------------------------

if __name__ == '__main__':
    #db = str(PATH_TO_REPO / "data/soil/buek1000.sqlite")
    #grid = str(PATH_TO_REPO / "data/soil/buek1000_1000_31469_gk5.asc")
    db = str(PATH_TO_REPO / "data/soil/buek200.sqlite")
    grid = str(PATH_TO_REPO / "data/soil/buek200_1000_25832_etrs89-utm32n.asc")
    asyncio.run(main(db, grid, use_async=True)) 
