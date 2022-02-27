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
import capnp
from pathlib import Path
import os
import sys
import time

PATH_TO_REPO = Path(os.path.realpath(__file__)).parent.parent.parent
if str(PATH_TO_REPO) not in sys.path:
    sys.path.insert(1, str(PATH_TO_REPO))

PATH_TO_PYTHON_CODE = PATH_TO_REPO / "src/python"
if str(PATH_TO_PYTHON_CODE) not in sys.path:
    sys.path.insert(1, str(PATH_TO_PYTHON_CODE))

PATH_TO_CAPNP_SCHEMAS = PATH_TO_REPO / "capnproto_schemas"
abs_imports = [str(PATH_TO_CAPNP_SCHEMAS)]

import common.capnp_async_helpers as async_helpers
import common.common as common
import services.climate.csv_file_based as csv_based

reg_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "registry.capnp"), imports=abs_imports)
soil_data_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "soil_data.capnp"), imports=abs_imports)
registry_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "registry.capnp"), imports=abs_imports)
persistence_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "persistence.capnp"), imports=abs_imports)
model_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "model.capnp"), imports=abs_imports)
yieldstat_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "models" / "yieldstat" / "yieldstat.capnp"), imports=abs_imports)
climate_data_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "climate_data.capnp"), imports=abs_imports)
mgmt_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "management.capnp"), imports=abs_imports)
service_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "service.capnp"), imports=abs_imports)
common_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "common.capnp"), imports=abs_imports)
csv_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "csv.capnp"), imports=abs_imports)


#------------------------------------------------------------------------------

async def async_main():
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
    
    conMan = async_helpers.ConnectionManager()
    yieldstat = await conMan.connect("capnp://localhost:15000", model_capnp.EnvInstance)
    info = await yieldstat.info().a_wait()
    print(info)

    time_series = await conMan.connect("capnp://localhost:11002", climate_data_capnp.TimeSeries)
    ts_header = (await time_series.header().a_wait()).header
    print(ts_header)

    run_req = yieldstat.run_request()
    env = run_req.env
    env.timeSeries=time_series
    env.rest = yieldstat_capnp.RestInput.new_message(
        useDevTrend=True,
        useCO2Increase=True,
        dgm=100.5,
        hft=53,
        nft=1,
        sft=36,
        slope=0,
        steino=1,
        az=14,
        klz=8,
        stt=152,
        germanFederalStates=5, #-1
        getDryYearWaterNeed=True #false;
    )
    cr = env.init("cropRotation", 3)
    cr[0].type = "sowing"
    cr[0].params = mgmt_capnp.Params.Sowing.new_message(cultivar="wheatWinter")
    cr[1].type = "irrigation"
    cr[2].type = "harvest"

    ys_res = (await run_req.send().a_wait()).result.as_struct(yieldstat_capnp.Output)
    print(ys_res)



def x():
    s = capnp.TwoPartyServer("*:11002", bootstrap=csv_based.TimeSeries.from_csv_file("data/climate/climate-iso.csv", header_map={}, pandas_csv_config={}))
    s.run_forever()
    s._decref()
    
    #del s

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

    conMan = common.ConnectionManager()
    #restorer = conMan.try_connect("capnp://insecure@pc-berg-7920.fritz.box:10000", cast_as=persistence_capnp.Restorer)
    service = conMan.try_connect("capnp://insecure@pc-berg-7920.fritz.box:10000/cf478c42-9a5c-403e-8377-03445494bfac", cast_as=climate_data_capnp.CSVTimeSeriesFactory)

    try:
        print(service.info().wait())
    except Exception as e:
        print(e)

    #unsave = conMan.try_connect("capnp://insecure@pc-berg-7920.fritz.box:10000/49ec71b8-a525-4c38-b137-58e1eafc0c1c", cast_as=common_capnp.Action)
    #unsave.do().wait()

    with open("../../data/climate/climate-iso.csv", "r") as _:
        csv_data = _.read()

    res = service.create(csvData=csv_data, config={}).wait()


    #s = capnp.TwoPartyServer("*:11002", bootstrap=csv_based.TimeSeries.from_csv_file("data/climate/climate-iso.csv", header_map={}, pandas_csv_config={}))
    #s.run_forever()
    #del s
    #x()


    for i in range(1):
        csv_timeseries_cap = capnp.TwoPartyClient("localhost:11002").bootstrap().cast_as(climate_data_capnp.TimeSeries)
        header = csv_timeseries_cap.header().wait().header
        data = csv_timeseries_cap.data().wait().data
        print("i:", i, "header:", header)


    """
    admin = connect("capnp://insecure@nb-berg-9550:10001/320a351d-c6cb-400a-92e0-4647d33cfedb", registry_capnp.Admin)
    success = admin.addCategory({"id": "models", "name": "models"}).wait().success

    soil_service = capnp.TwoPartyClient(config["server"] + ":" + config["port"]).bootstrap().cast_as(soil_data_capnp.Service)
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

    """

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


    #soil_service = capnp.TwoPartyClient("localhost:6003").bootstrap().cast_as(soil_data_capnp.Service)
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
    #asyncio.get_event_loop().run_until_complete(async_main()) # gets rid of some eventloop cleanup problems using te usual call below
    #asyncio.run(async_main())