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
from random import random
import capnp
from pathlib import Path
import os
import sys
import time
from threading import Thread
import psutil

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
soil_data_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "soil.capnp"), imports=abs_imports)
registry_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "registry.capnp"), imports=abs_imports)
persistence_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "persistence.capnp"), imports=abs_imports)
model_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "model.capnp"), imports=abs_imports)
yieldstat_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "model" / "yieldstat" / "yieldstat.capnp"), imports=abs_imports)
monica_state_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "model" / "monica" / "monica_state.capnp"), imports=abs_imports)
monica_mgmt_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "model" / "monica" / "monica_management.capnp"), imports=abs_imports)
climate_data_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "climate.capnp"), imports=abs_imports)
mgmt_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "management.capnp"), imports=abs_imports)
service_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "service.capnp"), imports=abs_imports)
common_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "common.capnp"), imports=abs_imports)
grid_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "grid.capnp"), imports=abs_imports)

capnp.remove_event_loop()
capnp.create_event_loop(threaded=True)

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
    registry = await conMan.connect("capnp://insecure@localhost:9999/a8b8ff83-0af4-42c9-95c8-b6ec19a35945", registry_capnp.Registry)
    print(await registry.info().a_wait())

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


def test_fertilizer_managment_service():
    conMan = common.ConnectionManager()
    service = conMan.try_connect("capnp://insecure@10.10.24.169:33887/78e9d85f-a7f3-4063-bb15-0429ac79f6bf", cast_as=monica_mgmt_capnp.FertilizerService)
    try:
        print(service.info().wait())
    except Exception as e:
        print(e)

    es = service.availableMineralFertilizers().wait().entries
    avh = es[0].ref
    v = avh.value().wait().val.as_struct(monica_mgmt_capnp.Params.MineralFertilization.Parameters)
    print("v:", v)
    sr = avh.save().wait().sturdyRef
    avh2 = conMan.try_connect(sr, cast_as=common_capnp.AnyValueHolder)
    v2 = avh2.value().wait().val.as_struct(monica_mgmt_capnp.Params.MineralFertilization.Parameters)
    print("v2:", v2)

    print()


def test_climate_service():
    conMan = common.ConnectionManager()
    #restorer = conMan.try_connect("capnp://insecure@pc-berg-7920.fritz.box:10000", cast_as=persistence_capnp.Restorer)
    #service = conMan.try_connect("capnp://insecure@pc-berg-7920.fritz.box:10000/6feaf299-d620-430b-9189-36dfccf48b3a", cast_as=climate_data_capnp.CSVTimeSeriesFactory)
    service = conMan.try_connect("capnp://insecure@10.10.24.218:36541/7555fd1a-e413-4ec7-be5d-8f3a94825b3c", cast_as=climate_data_capnp.Service)
    #timeseries = conMan.try_connect("capnp://insecure@pc-berg-7920.fritz.box:10000/8e7961c5-bd16-4c1d-86fd-8347dc46185e", cast_as=climate_data_capnp.TimeSeries)
    #unsave = conMan.try_connect("capnp://insecure@pc-berg-7920.fritz.box:10000/ac544d7b-1f82-4bf8-9adb-cf586ae46287", cast_as=common_capnp.Action)
    #4e4fe3fb-791a-4a26-9ae1-1ce52093bda5'  row: 340/col: 288
    try:
        print(service.info().wait())
    except Exception as e:
        print(e)

    p = psutil.Process()

    for ds in service.getAvailableDatasets().wait().datasets:
        ds_data = ds.data
        print(ds_data.info().wait())
        print(ds.meta)
        #tss = []
        for lat in range(45, 55):
            for lon in range(8, 15):
                ts = ds_data.closestTimeSeriesAt({"lat": lat, "lon": lon}).wait().timeSeries
                ts.data().wait()
                #tss.append(ts)
                print(ts.info().wait())
                #print(p.memory_percent())


    #unsave = conMan.try_connect("capnp://insecure@pc-berg-7920.fritz.box:10000/49ec71b8-a525-4c38-b137-58e1eafc0c1c", cast_as=common_capnp.Action)
    #unsave.do().wait()

    with open("../../data/climate/climate-iso.csv", "r") as _:
        csv_data = _.read()

    res = service.create(csvData=csv_data, config={}).wait()

    print()


def test_registry():

    conMan = common.ConnectionManager()
    #monica = conMan.try_connect("capnp://insecure@10.10.24.210:40867/16e65116-76d5-44ae-b3e6-65738bcb3b31", cast_as=model_capnp.EnvInstance)
    #print(monica.info().wait())
    
    #restorer = conMan.try_connect("capnp://insecure@pc-berg-7920.fritz.box:10000", cast_as=persistence_capnp.Restorer)
    
    #climate = conMan.try_connect("capnp://insecure@10.10.24.210:37203/6b57e75b-dee3-4882-90ae-731a679a3653", cast_as=climate_data_capnp.Service)
    
    #registry = conMan.try_connect("capnp://insecure@10.10.24.71:38955/5681fdaa-4d7f-4ea9-876e-e0566255486c", cast_as=registry_capnp.Registry)
    
    #registrar = conMan.try_connect("capnp://insecure@10.10.24.71:36501/2d997d95-95ac-400e-aeb7-c439b58b77a5", cast_as=registry_capnp.Registrar)
    
    #admin = conMan.try_connect("capnp://insecure@10.10.24.210:37087/6a9dea44-d00e-4a48-adf3-6c83b24cc5fa", cast_as=service_capnp.Admin)
    #print(service_admin.info().wait())

    #grid = conMan.try_connect("capnp://insecure@10.10.24.210:39875/2fa47702-f112-4628-b72c-7754e457d3a2", cast_as=grid_capnp.Grid)
    #value = grid.valueAt(coord={'lat': 50.02045903295569, 'lon': 8.449222632820296}).wait()
    
    writer = conMan.try_connect("capnp://insecure@10.10.24.210:44735/2c7554bc-5112-461d-a478-e95cbc905d37", cast_as=common_capnp.Writer)
    writer.write(value=monica_state_capnp.ICData.new_message(init={"syncDate": {"day": 1, "month": 2, "year": 2022}})).wait()
    print("bla")

    # test channel
    #channel_sr = "capnp://insecure@10.10.24.210:37505/6c25454e-4ef9-4659-9c94-341bdd827df5"
    def writer():
        conMan = common.ConnectionManager()
        writer = conMan.try_connect("capnp://insecure@10.10.24.210:43513/668ce2c1-f256-466d-99ce-30b01fd2b21b", cast_as=common_capnp.Writer)
        #channel = conMan.try_connect(channel_sr, cast_as=common_capnp.Channel)
        #writer = channel.writer().wait().w.as_interface(common_capnp.Writer)
        for i in range(1000):
            time.sleep(random())
            writer.write(value=common_capnp.X.new_message(t="hello_" + str(i))).wait()
            #writer.write(value="hello_" + str(i)).wait()
            print("wrote: hello_"+str(i))
            #writer.write(value=common_capnp.X.new_message(t="world")).wait()
        #print("wrote value:", "hello", "world")
    Thread(target=writer).start()
    reader = conMan.try_connect("capnp://insecure@10.10.24.210:34307/40daafc3-490b-4b27-a84f-6ee4c111b352", cast_as=common_capnp.Reader)
    #channel = conMan.try_connect(channel_sr, cast_as=common_capnp.Channel)
    #reader = channel.reader().wait().r.as_interface(common_capnp.Reader)
    for i in range(1000):
        time.sleep(random())
        print("read:", reader.read().wait().value.as_struct(common_capnp.X).t)
        #print("read:", reader.read().wait().value.as_text())
    #print(reader.read().wait().value.as_struct(common_capnp.X).t)
    #print("read value:", value)

    soil = conMan.try_connect("capnp://insecure@10.10.24.210:39341/9c15ad6f-0778-4bea-b91e-b015453188b9", cast_as=soil_data_capnp.Service)
    ps = soil.profilesAt(coord={'lat': 50.02045903295569, 'lon': 8.449222632820296}, query={"mandatory": ["soilType", "organicCarbon", "rawDensity"]}).wait()
    print(ps)
    
    #cap_holder = common.IdentifiableHolder(service, lambda: print("deleted capholder"))
    #res = registrar.register(cap=cap_holder, regName="dwd-klima", categoryId="climate").wait()

    res = registrar.register(cap=service, regName="dwd-klima", categoryId="climate").wait()
    
    climate_entries = registry.entries("climate").wait().entries
    for ent in climate_entries:
        print(ent.name, " ref.info:")#, ent.ref.info().wait())

    service_again = ent.ref.cast_as(climate_data_capnp.Service)


    return
    
    
    try:
        print(registrar.info().wait())
    except Exception as e:
        print(e)

    #unsave = conMan.try_connect("capnp://insecure@pc-berg-7920.fritz.box:10000/49ec71b8-a525-4c38-b137-58e1eafc0c1c", cast_as=common_capnp.Action)
    #unsave.do().wait()

    with open("../../data/climate/climate-iso.csv", "r") as _:
        csv_data = _.read()

    res = service.create(csvData=csv_data, config={}).wait()

    print()


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

    test_fertilizer_managment_service()

    #test_climate_service()

    #test_registry()

    return

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