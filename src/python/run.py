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
yieldstat_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "model" / "yieldstat" / "yieldstat.capnp"),
                             imports=abs_imports)
monica_state_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "model" / "monica" / "monica_state.capnp"),
                                imports=abs_imports)
monica_mgmt_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "model" / "monica" / "monica_management.capnp"),
                               imports=abs_imports)
climate_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "climate.capnp"), imports=abs_imports)
mgmt_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "management.capnp"), imports=abs_imports)
service_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "service.capnp"), imports=abs_imports)
common_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "common.capnp"), imports=abs_imports)
grid_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "grid.capnp"), imports=abs_imports)
storage_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "storage.capnp"), imports=abs_imports)
geo_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "geo.capnp"), imports=abs_imports)
crop_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "crop.capnp"), imports=abs_imports)
fbp_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "fbp.capnp"), imports=abs_imports)

capnp.remove_event_loop()
capnp.create_event_loop(threaded=True)

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
    registry = await conMan.connect("capnp://insecure@localhost:9999/a8b8ff83-0af4-42c9-95c8-b6ec19a35945",
                                    registry_capnp.Registry)
    print(await registry.info().a_wait())

    yieldstat = await conMan.connect("capnp://localhost:15000", model_capnp.EnvInstance)
    info = await yieldstat.info().a_wait()
    print(info)

    time_series = await conMan.connect("capnp://localhost:11002", climate_capnp.TimeSeries)
    ts_header = (await time_series.header().a_wait()).header
    print(ts_header)

    run_req = yieldstat.run_request()
    env = run_req.env
    env.timeSeries = time_series
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
        germanFederalStates=5,  # -1
        getDryYearWaterNeed=True  # false;
    )
    cr = env.init("cropRotation", 3)
    cr[0].type = "sowing"
    cr[0].params = mgmt_capnp.Params.Sowing.new_message(cultivar="wheatWinter")
    cr[1].type = "irrigation"
    cr[2].type = "harvest"

    ys_res = (await run_req.send().a_wait()).result.as_struct(yieldstat_capnp.Output)
    print(ys_res)


def x():
    s = capnp.TwoPartyServer("*:11002",
                             bootstrap=csv_based.TimeSeries.from_csv_file("data/climate/climate-iso.csv", header_map={},
                                                                          pandas_csv_config={}))
    s.run_forever()
    s._decref()

    # del s


def run_crop_service():
    conMan = common.ConnectionManager()
    sr = "capnp://YWF7ZnUsQb6O_4eT_65vrvpR__IvoesmxocB5W92HpM=@10.10.24.218:34359/NzdkOTFiYTctMzM5OC00MjllLWFmNzAtYTkxN2MyODllYjhi"
    x = conMan.connect("capnp://YWF7ZnUsQb6O_4eT_65vrvpR__IvoesmxocB5W92HpM=@10.10.24.218:34359")
    service = conMan.try_connect(sr, cast_as=registry_capnp.Registry)
    try:
        print(service.info().wait())
    except Exception as e:
        print(e)

    # mineral fertilizers
    mes = service.availableMineralFertilizers().wait().entries
    me = mes[0]
    mavh = me.ref
    mv = mavh.value().wait().val.as_struct(monica_mgmt_capnp.Params.MineralFertilization.Parameters)
    print("mv:", mv)
    msr = mavh.save().wait().sturdyRef
    mavh2 = conMan.try_connect(msr, cast_as=common_capnp.AnyValueHolder)
    mv2 = mavh2.value().wait().val.as_struct(monica_mgmt_capnp.Params.MineralFertilization.Parameters)
    print("mv2:", mv2)
    mv3 = service.mineralFertilizer(me.info.id).wait().fert
    print("mv3", mv3)

    # organic fertilizers
    oes = service.availableOrganicFertilizers().wait().entries
    oe = oes[0]
    oavh = oe.ref
    ov = oavh.value().wait().val.as_struct(monica_mgmt_capnp.Params.OrganicFertilization.OrganicMatterParameters)
    print("ov:", ov)
    osr = oavh.save().wait().sturdyRef
    oavh2 = conMan.try_connect(osr, cast_as=common_capnp.AnyValueHolder)
    ov2 = oavh2.value().wait().val.as_struct(monica_mgmt_capnp.Params.OrganicFertilization.OrganicMatterParameters)
    print("ov2:", ov2)
    ov3 = service.organicFertilizer(oe.info.id).wait().fert
    print("ov3", ov3)

    print()


def run_fertilizer_service():
    conMan = common.ConnectionManager()
    sr = "capnp://P1NRqmXFzLkkY3FYEW3tgF5dYdsdwGso3XRJG6wHg4w=@10.10.24.218:40263/YjUxMWRmNWMtNTMwNy00YzE5LThmNjItYWJlNGE2YjUwMzk3"
    registry = conMan.try_connect(sr, cast_as=reg_capnp.Registry)
    try:
        print(registry.info().wait())
    except Exception as e:
        print(e)

    # mineral fertilizers
    mes = registry.availableMineralFertilizers().wait().entries
    me = mes[0]
    mavh = me.ref
    mv = mavh.value().wait().val.as_struct(monica_mgmt_capnp.Params.MineralFertilization.Parameters)
    print("mv:", mv)
    msr = mavh.save().wait().sturdyRef
    mavh2 = conMan.try_connect(msr, cast_as=common_capnp.AnyValueHolder)
    mv2 = mavh2.value().wait().val.as_struct(monica_mgmt_capnp.Params.MineralFertilization.Parameters)
    print("mv2:", mv2)
    mv3 = registry.mineralFertilizer(me.info.id).wait().fert
    print("mv3", mv3)

    # organic fertilizers
    oes = registry.availableOrganicFertilizers().wait().entries
    oe = oes[0]
    oavh = oe.ref
    ov = oavh.value().wait().val.as_struct(monica_mgmt_capnp.Params.OrganicFertilization.OrganicMatterParameters)
    print("ov:", ov)
    osr = oavh.save().wait().sturdyRef
    oavh2 = conMan.try_connect(osr, cast_as=common_capnp.AnyValueHolder)
    ov2 = oavh2.value().wait().val.as_struct(monica_mgmt_capnp.Params.OrganicFertilization.OrganicMatterParameters)
    print("ov2:", ov2)
    ov3 = registry.organicFertilizer(oe.info.id).wait().fert
    print("ov3", ov3)

    print()


def run_climate_service():
    conMan = common.ConnectionManager()
    # restorer = conMan.try_connect("capnp://insecure@pc-berg-7920.fritz.box:10000", cast_as=persistence_capnp.Restorer)
    # service = conMan.try_connect("capnp://insecure@pc-berg-7920.fritz.box:10000/6feaf299-d620-430b-9189-36dfccf48b3a", cast_as=climate_data_capnp.CSVTimeSeriesFactory)
    service = conMan.try_connect(
        "capnp://SzOpxKvp2MfCwo5CKSKjvr5qBF2ZOLNJunmyZCpS-e4=@10.10.24.218:43029/MTRjNWViNzctMGJmNS00ZGEwLTkwY2QtYTE0ZmEyMTZhOTYz",
        cast_as=climate_capnp.Service)
    # timeseries = conMan.try_connect("capnp://insecure@pc-berg-7920.fritz.box:10000/8e7961c5-bd16-4c1d-86fd-8347dc46185e", cast_as=climate_data_capnp.TimeSeries)
    # unsave = conMan.try_connect("capnp://insecure@pc-berg-7920.fritz.box:10000/ac544d7b-1f82-4bf8-9adb-cf586ae46287", cast_as=common_capnp.Action)
    # 4e4fe3fb-791a-4a26-9ae1-1ce52093bda5'  row: 340/col: 288
    try:
        print(service.info().wait())
    except Exception as e:
        print(e)

    ds = service.getAvailableDatasets().wait().datasets[0].data
    print(ds.save().wait())

    cb = ds.streamLocations().wait().locationsCallback
    while True:
        ls = cb.nextLocations(10).wait().locations
        if len(ls) == 0:
            break
        for l in ls:
            rc = l.customData[0].value.as_struct(geo_capnp.RowCol)
            day0_data = l.timeSeries.data().wait().data[0]
            print("row:", rc.row, "col:", rc.col, "day0:", day0_data)

    p = psutil.Process()

    for ds in service.getAvailableDatasets().wait().datasets:
        ds_data = ds.data
        print(ds_data.info().wait())
        print(ds.meta)
        # tss = []
        for lat in range(45, 55):
            for lon in range(8, 15):
                ts = ds_data.closestTimeSeriesAt({"lat": lat, "lon": lon}).wait().timeSeries
                ts.data().wait()
                # tss.append(ts)
                print(ts.info().wait())
                # print(p.memory_percent())

    # unsave = conMan.try_connect("capnp://insecure@pc-berg-7920.fritz.box:10000/49ec71b8-a525-4c38-b137-58e1eafc0c1c", cast_as=common_capnp.Action)
    # unsave.do().wait()

    with open("../../data/climate/climate-iso.csv", "r") as _:
        csv_data = _.read()

    res = service.create(csvData=csv_data, config={}).wait()

    print()


def run_monica():
    con_man = common.ConnectionManager()
    sr = "capnp://8TwMtyGcNgiSBLXps4xRi6ymeDinAINWSrzcWJyI0Uc@10.10.24.181:41075/NzVkNzc3OTMtZjA2My00YmRkLTlkNWYtNjM2NDg1MDdjODg5"
    monica = con_man.try_connect(sr, cast_as=model_capnp.EnvInstance)
    print(monica.info().wait())
    print(monica.info().wait())
    print(monica.info().wait())
    print(monica.info().wait())


def run_registry():
    con_man = common.ConnectionManager()
    registry_sr = "capnp://MCowBQYDK2VwAyEAYSMdzsTMWEQmsd3/BMyVzC25QwngGxF2KkV4zE3TsMg=@10.10.88.68:35877/ODM0YzUzY2MtZTI2Zi00M2M4LThiZTQtNDk5ZGQzOWNiMmM5"
    registry = con_man.try_connect(registry_sr, cast_as=registry_capnp.Registry)
    print(registry.info().wait())

    # registrar_sr = "capnp://MCowBQYDK2VwAyEAFJHQao0Dy26vAV2LmMT4lBXMtI+xebNohLBaYLvsqkc=@10.10.24.181:38101/ZTE2YWE4ZTQtN2FhMi00YWMyLWJlMzctZmZmMTIyMzg5N2Q5"
    # registrar = con_man.try_connect(registrar_sr, cast_as=registry_capnp.Registrar)
    # print(registrar.info().wait())

    # admin_sr = "capnp://MCowBQYDK2VwAyEAFJHQao0Dy26vAV2LmMT4lBXMtI+xebNohLBaYLvsqkc=@10.10.24.181:38101/MzZiZDkwODAtNjQxYS00ZjEwLThmMDktYTcwOGY0YThkNWI5"
    # admin = con_man.try_connect(admin_sr, cast_as=service_capnp.Admin)
    # print(admin.info().wait())

    # res = registrar.register(cap=admin, regName="admin", categoryId="climate").wait()
    # print(res)

    climate_entries = registry.entries("monica").wait().entries
    for ent in climate_entries:
        print(ent.name, " ref.info:", ent.ref.info().wait())

    print("bla")


def run_cross_domain_registry():
    con_man = common.ConnectionManager()
    registry_sr = "capnp://wmJ-RIgP0BNXM-R_y919CpLdogi9NA563OCtOGzlYRU@10.10.24.181:45007/ZjRmMDI3MmQtOWEyOS00MDZjLWE5ZjUtZGYxMjg0YzFlZDM5"
    registry = con_man.try_connect(registry_sr, cast_as=registry_capnp.Registry)
    print("registry info:", registry.info().wait())

    monica = registry.entries("monica").wait().entries[0].ref.cast_as(model_capnp.EnvInstance)
    print("monica info:", monica.info().wait())
    monica_sr = monica.save().wait().sturdyRef
    print("monica sr:", monica_sr)

    monica_2 = con_man.try_connect(monica_sr, cast_as=model_capnp.EnvInstance)
    print("monica2 info:", monica_2.info().wait())

    print("bla")


def run_storage_container():
    con_man = common.ConnectionManager()
    container_sr = "capnp://jXS22bpAGSjfksa0JkDI_092-h-bdZi4lKNBBgD7kWk=@10.10.24.218:40305/ZGVjODYxYWQtZmVkOS00YjEzLWJmNjQtNWU0OGRmYzhhYmZh"
    container = con_man.try_connect(container_sr, cast_as=storage_capnp.Store.Container)
    print("container info:", container.info().wait())

    print("end")


def run_storage_service():
    run_storage_container()

    con_man = common.ConnectionManager()
    service_sr = "capnp://jXS22bpAGSjfksa0JkDI_092-h-bdZi4lKNBBgD7kWk@10.10.24.218:40305/ZWJhM2E2NWEtOGM2MS00OWQxLTk3NjAtODVlNDYyNWYyZmRj"
    service = con_man.try_connect(service_sr, cast_as=storage_capnp.Store)
    print("service info:", service.info().wait())
    # 'ab7b1472-5437-42ba-b65b-4285717ffe16'
    if (True):
        new_container = service.newContainer("test-container xxx", "test container descr").wait().container
        info = new_container.info().wait()
        id = info.id
        print("new_container:", info)

        try:
            print("add text:",
                  new_container.addEntry(key="some text", value={"textValue": "text value1"}).wait().success)
            print("add text list:", new_container.addEntry(key="a text list", value={
                "textListValue": ["eins", "zwei", "drei"]}).wait().success)
            print("add bool:", new_container.addEntry(key="a bool", value={"boolValue": True}).wait().success)
            print("add bool list:", new_container.addEntry(key="a bool list", value={
                "boolListValue": [True, False, False, True, False]}).wait().success)
            print("add int8:", new_container.addEntry(key="an int8", value={"int8Value": 42}).wait().success)
            print("add int8 list:", new_container.addEntry(key="an int8 list", value={
                "int8ListValue": [1, 2, 3, 4, 55, 66, 127]}).wait().success)
            print("add int16:", new_container.addEntry(key="an int16", value={"int16Value": 9999}).wait().success)
            print("add int16 list:", new_container.addEntry(key="an int16 list", value={
                "int16ListValue": [1, 22, 333, 4444, 32000]}).wait().success)
            print("add int32:", new_container.addEntry(key="an int32", value={"int32Value": 424242}).wait().success)
            print("add int32 list:", new_container.addEntry(key="an int32 list", value={
                "int32ListValue": [1111, 22222, 333333, 4444444, 55555555]}).wait().success)
            print("add int64:",
                  new_container.addEntry(key="an int64", value={"int64Value": 424242424242}).wait().success)
            print("add int64 list:", new_container.addEntry(key="an int64 list", value={
                "int64ListValue": [11111111111, 2, 3, 4, 55, 66, 192]}).wait().success)
            print("add float32", new_container.addEntry(key="a float32", value={"float32Value": 42.444}).wait().success)
            print("add float32 list:", new_container.addEntry(key="a float32 list", value={
                "float32ListValue": [0.1, 0.22, 3.333, 4.1234, 5]}).wait().success)
            print("add float64", new_container.addEntry(key="a float64", value={"float64Value": 42.444}).wait().success)
            print("add float64 list:", new_container.addEntry(key="a float64 list", value={
                "float64ListValue": [0.1, 0.22, 3.333, 4.1234, 5]}).wait().success)

            i = common_capnp.IdInformation.new_message(id="bla", name="blub", description="blob")
            succ = new_container.addEntry(key="id info struct", value={"anyValue": i}).wait().success
            print("add object:", succ)

            for entry in new_container.downloadEntries().wait().entries:
                print("entry:", entry)
                if entry.fst == "id info struct":
                    print("anyValue:", entry.snd.anyValue.as_struct(common_capnp.IdInformation))

            for entry in new_container.listEntries().wait().entries:
                key, value = capnp.join_promises([entry.getKey(), entry.getValue()]).wait()
                # keyp = entry.getKey()
                # value = entry.getValue().wait().value
                print("entry:", key, value)
                if entry.getKey().wait().key == "id info struct":
                    print("anyValue:", entry.getValue().wait().value.anyValue.as_struct(common_capnp.IdInformation))

            json_str = new_container.export().wait().json
            print("export:", json_str)

            print("removed container successfully:", service.removeContainer(id).wait().success)

            container = service.importContainer(json_str).wait().container
            print("container.info:", container.info().wait())
            for entry in container.downloadEntries().wait().entries:
                print("entry:", entry)
                if entry.fst == "id info struct":
                    print("anyValue:", entry.snd.anyValue.as_struct(common_capnp.IdInformation))

            print("end")

        except Exception as e:
            print("error:", e)
    if (False):
        all_containers = service.listContainers().wait().containers
        for cont in all_containers:
            info = cont.info().wait()
            print("container:", info)
            # all_objects = cont.listObjects().wait().objects
            # for obj in all_objects:
            #    print("object:", obj)

    # new_container_2 = service.containerWithId(id).wait().container
    # info_2 = new_container_2.info().wait()
    # print("new_container_2:", info_2)

    # time.sleep(5)

    # obj = new_container_2.getObject("test-key").wait().object
    # try:
    #    print("obj:", obj)
    # except Exception as e:
    #    print("error:", e)

    # containers = service.listContainers().wait().containers
    # for i, c in enumerate(containers):
    #    print("container", i, " info:", c.info().wait())

    print("end")


def run_grid():
    con_man = common.ConnectionManager()
    grid = con_man.try_connect("capnp://insecure@10.10.24.210:39875/2fa47702-f112-4628-b72c-7754e457d3a2",
                               cast_as=grid_capnp.Grid)
    print(grid.info().wait())
    value = grid.valueAt(coord={'lat': 50.02045903295569, 'lon': 8.449222632820296}).wait()


def run_restorer():
    con_man = common.ConnectionManager()
    restorer = con_man.try_connect("capnp://insecure@pc-berg-7920.fritz.box:10000", cast_as=persistence_capnp.Restorer)
    print(restorer.info().wait())


def run_climate():
    con_man = common.ConnectionManager()
    climate = con_man.try_connect("capnp://insecure@10.10.24.210:37203/6b57e75b-dee3-4882-90ae-731a679a3653",
                                  cast_as=climate_capnp.Service)
    print(climate.info().wait())


def run_channel():
    con_man = common.ConnectionManager()

    writer = con_man.try_connect("capnp://2djJAQhpUZuiQxCllmwVBF86XNvrnNVw8JQnFomcBUM@10.10.24.218:33893/b3V0",
                                 cast_as=fbp_capnp.Channel.Writer)
    writer.write(value=geo_capnp.RowCol.new_message(row=5, col=12)).wait()
    print("bla")

    # test channel
    # channel_sr = "capnp://insecure@10.10.24.210:37505/6c25454e-4ef9-4659-9c94-341bdd827df5"
    def writer():
        conMan = common.ConnectionManager()
        writer = conMan.try_connect("capnp://insecure@10.10.24.210:43513/668ce2c1-f256-466d-99ce-30b01fd2b21b",
                                    cast_as=fbp_capnp.Channel.Writer)
        # channel = conMan.try_connect(channel_sr, cast_as=fbp_capnp.Channel)
        # writer = channel.writer().wait().w.as_interface(common_capnp.Writer)
        for i in range(1000):
            time.sleep(random())
            writer.write(value=common_capnp.X.new_message(t="hello_" + str(i))).wait()
            # writer.write(value="hello_" + str(i)).wait()
            print("wrote: hello_" + str(i))
            # writer.write(value=common_capnp.X.new_message(t="world")).wait()
        # print("wrote value:", "hello", "world")

    Thread(target=writer).start()
    reader = con_man.try_connect("capnp://2djJAQhpUZuiQxCllmwVBF86XNvrnNVw8JQnFomcBUM@10.10.24.218:33893/aW4",
                                 cast_as=fbp_capnp.Channel.Reader)
    # channel = conMan.try_connect(channel_sr, cast_as=fbp_capnp.Channel)
    # reader = channel.reader().wait().r.as_interface(common_capnp.Reader)
    for i in range(1000):
        time.sleep(random())
        print("read:", reader.read().wait().value.as_struct(common_capnp.X).t)
        # print("read:", reader.read().wait().value.as_text())
    # print(reader.read().wait().value.as_struct(common_capnp.X).t)
    # print("read value:", value)


def run_some():
    con_man = common.ConnectionManager()

    soil = con_man.try_connect("capnp://insecure@10.10.24.210:39341/9c15ad6f-0778-4bea-b91e-b015453188b9",
                               cast_as=soil_data_capnp.Service)
    ps = soil.profilesAt(coord={'lat': 50.02045903295569, 'lon': 8.449222632820296},
                         query={"mandatory": ["soilType", "organicCarbon", "rawDensity"]}).wait()
    print(ps)


def run_resolver():
    con_man = common.ConnectionManager()

    sr = "capnp://2nQ6wFu_2FkJZYn6peo0TL9Ki9JH_Z71LDFcKNjqFM0@10.10.24.250:46643"
    resolver = con_man.try_connect(sr, cast_as=persistence_capnp.HostPortResolver)
    
    print(resolver.info().wait())


def run_resolver_registrar():
    con_man = common.ConnectionManager()

    sr = "capnp://2nQ6wFu_2FkJZYn6peo0TL9Ki9JH_Z71LDFcKNjqFM0@10.10.24.250:46643/MjQ0NmUzODgtMGQ1Zi00NGRjLTlhZjUtNGZlNDI5YTMzZmEz"
    registrar = con_man.try_connect(sr, cast_as=persistence_capnp.HostPortResolver.Registrar)
    hb = registrar.register(base64VatId="2nQ6wFu_2FkJZYn6peo0TL9Ki9JH_Z71LDFcKNjqFM0", host="10.10.24.250", port=46643,
                            alias="resolver").wait()
    print("let heart beat every", hb.secsHeartbeatInterval)
    hb.heartbeat.beat().wait()

    print(registrar.info().wait())


def main():
    config = {
        "port": "6003",
        "server": "localhost"
    }
    common.update_config(config, sys.argv, print_config=True, allow_new_keys=True)

    run_resolver()
    #run_resolver_registrar()

    # run_channel()

    # run_crop_service()

    #run_fertilizer_service()

    # run_monica()

    # run_climate_service()

    # run_registry()

    # run_cross_domain_registry()

    # run_storage_service()

    # run_storage_container()

    return

    # s = capnp.TwoPartyServer("*:11002", bootstrap=csv_based.TimeSeries.from_csv_file("data/climate/climate-iso.csv", header_map={}, pandas_csv_config={}))
    # s.run_forever()
    # del s
    # x()

    for i in range(1):
        csv_timeseries_cap = capnp.TwoPartyClient("localhost:11002").bootstrap().cast_as(climate_capnp.TimeSeries)
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

    # profiles = soil_service.allLocations(
    #    mandatory=[{"sand": 0}, {"clay": 0}, {"bulkDensity": 0}, {"organicCarbon": 0}],
    #    optional=[{"pH": 0}],
    #    onlyRawData=False
    # ).wait().profiles
    # latlon_and_cap = profiles[0]  # at the moment there is always just one profile being returned
    # cap_list = latlon_and_cap.snd
    # cap = cap_list[0]
    # p = latlon_and_cap.snd[0].cap().wait().object
    # print(p)

    # soil_service = capnp.TwoPartyClient("localhost:6003").bootstrap().cast_as(soil_data_capnp.Service)
    # profiles = soil_service.profilesAt(
    #    coord={"lat": 53.0, "lon": 12.5},
    #    query={
    #        "mandatory": [{"sand": 0}, {"clay": 0}, {"bulkDensity": 0}, {"organicCarbon": 0}],
    #        "optional": [{"pH": 0}]
    #    }
    # ).wait().profiles

    # print(profiles)


if __name__ == '__main__':
    main()
    # asyncio.get_event_loop().run_until_complete(async_main()) # gets rid of some eventloop cleanup problems using te usual call below
    # asyncio.run(async_main())