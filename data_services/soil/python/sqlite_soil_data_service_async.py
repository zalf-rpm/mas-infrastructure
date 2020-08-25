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

from pyproj import CRS, Transformer

PATH_TO_REPO = Path(os.path.realpath(__file__)).parent.parent.parent.parent

if str(PATH_TO_REPO) not in sys.path:
    sys.path.insert(1, str(PATH_TO_REPO))
from common.python import rect_ascii_grid_management as grid_man, common, geo, capnp_async_helpers as async_helpers

PATH_TO_UTIL_SOIL = PATH_TO_REPO.parent / "util/soil"
if str(PATH_TO_UTIL_SOIL) not in sys.path:
    sys.path.insert(1, str(PATH_TO_UTIL_SOIL))
import soil_io3

import capnp
capnp.add_import_hook(additional_paths=["capnproto_schemas"])
import soil_data_capnp
import common_capnp

#------------------------------------------------------------------------------

def set_capnp_param_via_monica_name(param, name, value=None):
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

def capnp_to_monica_param_name(capnp_param):
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
        "size": "Thickness",
        "description": "description"
    }.get(capnp_param.which(), None)

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

        self._id = id if id else path_to_sqlite_db
        self._name = name if name else self._id
        self._description = description if description else ""
        self._cache_raw = {}
        self._cache_derived = {}


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
            self._all_available_params_derived = soil_io3.available_soil_parameters(self._con, only_raw_data=False)
        return self._all_available_params_derived


    @property
    def all_available_params_raw(self):
        if not self._all_available_params_raw:
            self._all_available_params_raw = soil_io3.available_soil_parameters(self._con, only_raw_data=True)
        return self._all_available_params_raw


    def info_context(self, context): # info @0 () -> IdInformation;
        r = context.results
        r.id = self._id
        r.name = self._name
        r.description = self._description


    def check_params_are_available(self, mandatory_params, optional_params, only_raw_data): 
        aps = self.all_available_params_raw if only_raw_data else self.all_available_params_derived

        mandatory_names = []
        for param in mandatory_params:
            name = capnp_to_monica_param_name(param)
            if name in aps["mandatory"]:
                mandatory_names.append(name)

        optional_names = []
        for param in optional_params:
            name = capnp_to_monica_param_name(param)
            if name in aps["optional"] or name in aps["mandatory"]:
                optional_names.append(name)

        return {
            "failed": len(mandatory_names) < len(mandatory_params), 
            "mandatory_names": mandatory_names,
            "optional_names": optional_names
        }


    def checkAvailableParameters_context(self, context): # checkAvailableParameters @2 Query -> Query.Result;
        p = context.params
        r = context.results

        res = self.check_params_are_available(p.mandatory, p.optional, p.onlyRawData)
        r.failed = res["failed"]

        r.init("mandatory", len(res["mandatory_names"]))
        for i, m_name in enumerate(res["mandatory_names"]):
            set_capnp_param_via_monica_name(r.mandatory[i], m_name)

        r.init("optional", len(res["optional_names"]))
        for i, o_name in enumerate(res["optional_names"]):
            set_capnp_param_via_monica_name(r.optional[i], o_name)


    def getAllAvailableParameters_context(self, context): # getAllAvailableParameters @3 () -> (params :List(Parameter));
        r = context.results
        aps = self.all_available_params_raw if context.params.onlyRawData else self.all_available_params_derived
        
        r.init("mandatory", len(aps["mandatory"]))
        for i, param_name in enumerate(aps["mandatory"]):
            set_capnp_param_via_monica_name(r.mandatory[i], param_name)

        r.init("optional", len(aps["optional"]))
        for i, param_name in enumerate(aps["optional"]):
            set_capnp_param_via_monica_name(r.optional[i], param_name)


    def profiles_at(self, lat, lon, profile, queried_names, only_raw_data): 
        if len(queried_names) > 0:
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
            l.init("params", len(queried_names))
            for i, name in enumerate(queried_names):
                set_capnp_param_via_monica_name(l.params[i], name, layer[name])


    def queried_names(self, query):
        """
        Get all the names of the parameters requested in the query.
        If a mandatory param is not available return no names, to indicate failure. 
        """

        res = self.check_params_are_available(query.mandatory, query.optional, query.onlyRawData)
        if res["failed"]:
            return []
        names = res["mandatory_names"].copy()
        names.extend(res["optional_names"])

        # always add Thickness, even if the user didn't ask for it
        if "Thickness" not in names: 
            names.append("Thickness")

        return names


    def profilesAt_context(self, context): # profilesAt @0 (coord :Geo.LatLonCoord, query :Query) -> (profiles :List(Profile));
        r = context.results
        ps = context.params
        c = ps.coord
        q = ps.query

        names = self.queried_names(q)

        # at the moment just support one soil profile per coordinate
        r.init("profiles", 1)

        # fill profile with data
        self.profiles_at(c.lat, c.lon, r.profiles[0], names, q.onlyRawData)


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

#------------------------------------------------------------------------------

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

#------------------------------------------------------------------------------

def new_connection_factory(service):
    
    async def new_connection(reader, writer):
        server = async_helpers.Server(service)
        await server.myserver(reader, writer)

    return new_connection

#------------------------------------------------------------------------------

async def main(path_to_sqlite_db, path_to_ascii_soil_grid, grid_crs, server="0.0.0.0", port=6003, id=None, name=None, description=None):
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

    service = Service(
        path_to_sqlite_db=config["path_to_sqlite_db"],
        path_to_ascii_grid=config["path_to_ascii_soil_grid"],
        grid_crs=geo.name_to_proj(config["grid_crs"]),
        id=config["id"], name=config["name"], description=config["description"])

    # Handle both IPv4 and IPv6 cases
    try:
        print("Try IPv4")
        server = await asyncio.start_server(
            new_connection_factory(service),
            server, port,
            family=socket.AF_INET
        )
    except Exception:
        print("Try IPv6")
        server = await asyncio.start_server(
            new_connection_factory(service),
            server, port,
            family=socket.AF_INET6
        )

    async with server:
        await server.serve_forever()

#------------------------------------------------------------------------------

def no_async_main(path_to_sqlite_db, path_to_ascii_soil_grid, grid_crs, server="127.0.0.1", port=6003, id=None, name=None, description=None):
    asyncio.run(main(path_to_sqlite_db, path_to_ascii_soil_grid, grid_crs, server=server, port=port, id=id, name=name, description=description))

#------------------------------------------------------------------------------

if __name__ == '__main__':
    asyncio.run(main("data/soil/buek1000.sqlite", "data/soil/buek1000_1000_gk5.asc", "gk5"))

#------------------------------------------------------------------------------

