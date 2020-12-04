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

import asyncio
from pathlib import Path
import os
import sys

PATH_TO_REPO = Path(os.path.realpath(__file__)).parent.parent.parent
if str(PATH_TO_REPO) not in sys.path:
    sys.path.insert(1, str(PATH_TO_REPO))

PATH_TO_PYTHON_CODE = PATH_TO_REPO / "src/python"
if str(PATH_TO_PYTHON_CODE) not in sys.path:
    sys.path.insert(1, str(PATH_TO_PYTHON_CODE))

#import common.python.capnp_async_helpers as async_helpers

import capnp
import capnproto_schemas.soil_data_capnp as soil_data_capnp
import capnproto_schemas.registry_capnp as registry_capnp
import capnproto_schemas.persistence_capnp as persistence_capnp
import capnproto_schemas.climate_data_capnp as climate_data_capnp


#------------------------------------------------------------------------------

bootstrap_caps = {}

def connect(sturdy_ref, cast_as = None):

    # we assume that a sturdy ref url looks always like capnp://hash-digest-or-insecure@host:port/sturdy-ref-token
    try:
        if sturdy_ref[:8] == "capnp://":
            rest = sturdy_ref[8:]
            hash_digest, rest = sturdy_ref.split("@")
            host, rest = rest.split(":")
            port_sr_token = rest.split("/")
            port = port_sr_token[0]
            sr_token = port_sr_token[1] if len(port_sr_token) > 1 else ""

            host_port = "{}:{}".format(host, port)
            if host_port in bootstrap_caps:
                bootstrap_cap = bootstrap_caps[host_port]
            else:
                bootstrap_cap = capnp.TwoPartyClient(host_port).bootstrap()
                bootstrap_caps[host_port] = bootstrap_cap

            if len(sr_token) == 0:
                return bootstrap_cap.cast_as(cast_as) if cast_as else bootstrap_cap
            else:
                restorer = bootstrap_cap.cast_as(persistence_capnp.Restorer)
                dyn_obj_reader = restorer.restore(sr_token).wait().cap
                return dyn_obj_reader.as_interface(cast_as) if cast_as else dyn_obj_reader

    except Exception as e:
        print(e)
        return None

    

def main():
    config = {
        "port": "6003",
        "server": "localhost"
    }
    # read commandline args only if script is invoked directly from commandline
    if len(sys.argv) > 1 and __name__ == "__main__":
        for arg in sys.argv[1:]:
            k, v = arg.split("=")
            if k in config:
                config[k] = v

    csv_timeseries_cap = capnp.TwoPartyClient("localhost:11002").bootstrap().cast_as(climate_data_capnp.Climate.TimeSeries)
    header = csv_timeseries_cap.header().wait().header
    print(header)

    #async def main():

    #    client = await async_helpers.connect_to_server(6003)
    #    soil_service = client.bootstrap().cast_as(soil_data_capnp.Soil.Service)
    #    params = soil_service.getAllAvailableParameters().wait().params

    #    print(soil_service)

    #registry = capnp.TwoPartyClient("localhost:10001").bootstrap().cast_as(registry_capnp.Registry)
    #print(registry.info().wait())

    admin = connect("capnp://insecure@nb-berg-9550:10001/320a351d-c6cb-400a-92e0-4647d33cfedb", registry_capnp.Admin)
    success = admin.addCategory({"id": "models", "name": "models"}).wait().success



    soil_service = capnp.TwoPartyClient(config["server"] + ":" + config["port"]).bootstrap().cast_as(soil_data_capnp.Soil.Service)
    props = soil_service.getAllAvailableParameters().wait()
    print(props)

    profiles = soil_service.profilesAt(
        coord={"lat": 53.0, "lon": 12.5},
        query={
            "mandatory": ["sand", "clay", "bulkDensity", "organicCarbon"],
            "optional": ["pH"],
            "onlyRawData": False
        }
    ).wait().profiles
    print(profiles)

    #profiles = soil_service.allLocations(
    #    mandatory=[{"sand": 0}, {"clay": 0}, {"bulkDensity": 0}, {"organicCarbon": 0}],
    #    optional=[{"pH": 0}],
    #    onlyRawData=False
    #).wait().profiles
    #latlon_and_cap = profiles[0]  # at the moment there is always just one profile being returned
    #cap_list = latlon_and_cap.snd
    #cap = cap_list[0]
    #p = latlon_and_cap.snd[0].cap().wait().object
    #print(p)


    #soil_service = capnp.TwoPartyClient("localhost:6003").bootstrap().cast_as(soil_data_capnp.Soil.Service)
    #profiles = soil_service.profilesAt(
    #    coord={"lat": 53.0, "lon": 12.5},
    #    query={
    #        "mandatory": [{"sand": 0}, {"clay": 0}, {"bulkDensity": 0}, {"organicCarbon": 0}],
    #        "optional": [{"pH": 0}]
    #    }
    #).wait().profiles

    #print(profiles)



if __name__ == '__main__':
    main()