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

import asyncio
import logging
import os
from pathlib import Path
import socket
import sqlite3
import sys
import time
import uuid

from pyproj import CRS, Transformer

PATH_TO_REPO = Path(os.path.realpath(__file__)).parent.parent.parent.parent.parent
if str(PATH_TO_REPO) not in sys.path:
    sys.path.insert(1, str(PATH_TO_REPO))

PATH_TO_PYTHON_CODE = PATH_TO_REPO / "src/python"
if str(PATH_TO_PYTHON_CODE) not in sys.path:
    sys.path.insert(1, str(PATH_TO_PYTHON_CODE))

from common import rect_ascii_grid_management as grid_man, common, geo, capnp_async_helpers as async_helpers

PATH_TO_UTIL_SOIL = PATH_TO_REPO.parent / "util/soil"
if str(PATH_TO_UTIL_SOIL) not in sys.path:
    sys.path.insert(1, str(PATH_TO_UTIL_SOIL))
import soil_io3

import capnp
from capnproto_schemas import soil_data_capnp, common_capnp, service_registry_capnp as reg_capnp

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

    def __init__(self, path_to_sqlite_db, path_to_ascii_grid, grid_crs, id=None, name=None, description=None):
        self._path_to_sqlite_db = path_to_sqlite_db
        self._path_to_ascii_grid = path_to_ascii_grid
        self._con = sqlite3.connect(self._path_to_sqlite_db)
        self._grid_crs = grid_crs

        self._all_available_params_raw = None
        self._all_available_params_derived = None

        self._interpol_and_latlon_coords = None
        self._all_latlon_coords = None
        self._latlon_to_capholders = {}

        self._id = id if id else uuid.uuid4()
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

#------------------------------------------------------------------------------

"""
# interface CapHolder(Object)
class ProfileCapHolder(soil_data_capnp.Soil.Service.CommonCapHolderProfile.Server): #(common_capnp.Common.CapHolder.Server):

    def __init__(self, service, latlon, queried_names, only_raw_data, latlon_to_capholders):
        self._service = service
        self._latlon = latlon
        self._queried_names = queried_names
        self._only_raw_data = only_raw_data
        self._latlon_to_capholders = latlon_to_capholders


    def __del__(self):
        print("ProfileCapHolder.__del__")
        self._latlon_to_capholders.pop(self._latlon)


    def cap_context(self, context): # cap @0 () -> (object :Object);
        #profile = context.results.object.as_struct(soil_data_capnp.Soil.Profile)
        profile = context.results.object
        self._service.profiles_at(self._latlon[0], self._latlon[1], profile, self._queried_names, self._only_raw_data)
        print(profile)
        print(dir(context))


    def release_context(self, context): # release @1 ();
        print("ProfileCapHolder.release_context")
        pass
"""

#------------------------------------------------------------------------------

def new_connection_factory(service):
    
    async def new_connection(reader, writer):
        server = async_helpers.Server(service)
        await server.myserver(reader, writer)

    return new_connection

#------------------------------------------------------------------------------

async def async_main_register(path_to_sqlite_db, path_to_ascii_soil_grid, grid_crs, reg_server="127.0.0.1", reg_port=10001, id=None, name=None, description=None):
    config = {
        "path_to_sqlite_db": path_to_sqlite_db,
        "path_to_ascii_soil_grid": path_to_ascii_soil_grid,
        "grid_crs": grid_crs,
        "id": id,
        "name": name,
        "description": description,
        "reg_port": str(reg_port),
        "reg_server": reg_server
    }
    # read commandline args only if script is invoked directly from commandline
    if len(sys.argv) > 1 and __name__ == "__main__":
        for arg in sys.argv[1:]:
            k, v = arg.split("=")
            if k in config:
                config[k] = v
    print("config used:", config)

    service = Service(
        path_to_sqlite_db=config["path_to_sqlite_db"],
        path_to_ascii_grid=config["path_to_ascii_soil_grid"],
        grid_crs=geo.name_to_proj(config["grid_crs"]),
        id=config["id"], name=config["name"], description=config["description"])

    registry_available = False
    connect_to_registry_retry_count = 10
    retry_secs = 5
    while not registry_available:
        try:
            client, gather_results = await async_helpers.connect_to_server(port=config["reg_port"], address=config["reg_server"])
            registry_available = True
        except:
            if connect_to_registry_retry_count == 0:
                print("Couldn't connect to registry server at {}:{}!".format(config["reg_server"], config["reg_port"]))
                exit(0)
            connect_to_registry_retry_count -= 1
            print("Trying to connect to {}:{} again in {} secs!".format(config["reg_server"], config["reg_port"], retry_secs))
            time.sleep(retry_secs)
            retry_secs += 1

    registry = client.bootstrap().cast_as(reg_capnp.Service.Registry)
    unreg = await registry.registerService(type="soil", service=service).a_wait()
    #await unreg.unregister.unregister().a_wait()

    print("registered soil service")

    #await unreg.unregister.unregister().a_wait()

    await gather_results

    print("after gather_results")

#------------------------------------------------------------------------------

async def async_main_server(path_to_sqlite_db, path_to_ascii_soil_grid, grid_crs, server="0.0.0.0", port=6003, id=None, name=None, description=None):
    config = {
        "port": str(port),
        "server": server,
        "path_to_sqlite_db": path_to_sqlite_db,
        "path_to_ascii_soil_grid": path_to_ascii_soil_grid,
        "grid_crs": grid_crs,
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
    print("config used:", config)

    service = Service(
        path_to_sqlite_db=config["path_to_sqlite_db"],
        path_to_ascii_grid=config["path_to_ascii_soil_grid"],
        grid_crs=geo.name_to_proj(config["grid_crs"]),
        id=config["id"], name=config["name"], description=config["description"])

    # Handle both IPv4 and IPv6 cases
    #try:
    #print("Try IPv4")
    server = await asyncio.start_server(
        new_connection_factory(service),
        server, port,
        family=socket.AF_INET
    )
    #except Exception:
    #    print("Try IPv6")
    #    server = await asyncio.start_server(
    #        new_connection_factory(service),
    #        server, port,
    #        family=socket.AF_INET6
    #    )

    async with server:
        await server.serve_forever()

#------------------------------------------------------------------------------

def no_async_main(path_to_sqlite_db, path_to_ascii_soil_grid, grid_crs, server="*", port=6003, id=None, name=None, description=None):
    config = {
        "port": str(port),
        "server": server,
        "path_to_sqlite_db": path_to_sqlite_db,
        "path_to_ascii_soil_grid": path_to_ascii_soil_grid,
        "grid_crs": grid_crs,
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
                                  path_to_ascii_grid=config["path_to_ascii_soil_grid"],
                                  grid_crs=geo.name_to_proj(config["grid_crs"]),
                                  id=config["id"], name=config["name"], description=config["description"]))
    server.run_forever()

#------------------------------------------------------------------------------

def main(path_to_sqlite_db, path_to_ascii_soil_grid, grid_crs, server="*", port=6003, id=None, name=None, description=None, use_asyncio=True):

    if use_asyncio:
        asyncio.run(async_main_server(path_to_sqlite_db, path_to_ascii_soil_grid, grid_crs, server=server, port=port, id=id, name=name, description=description))
    else:
        no_async_main(path_to_sqlite_db, path_to_ascii_soil_grid, grid_crs, server=server, port=port, id=id, name=name, description=description)

#------------------------------------------------------------------------------

if __name__ == '__main__':
    db = "data/soil/buek1000.sqlite"
    grid = "data/soil/buek1000_1000_gk5.asc"
    crs = "gk5"

    #asyncio.run(async_main_register(db, grid, crs))
    #exit()

    if len(sys.argv) > 1:
        command = sys.argv[1]
        if command == "async_server":
            sys.argv.pop(1)
            asyncio.run(async_main_server(db, grid, crs))
        elif command == "async_register":
            sys.argv.pop(1)
            asyncio.run(async_main_register(db, grid, crs))
    else:
        no_async_main(db, grid, crs)

#------------------------------------------------------------------------------

