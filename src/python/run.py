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
import json
import os
from pathlib import Path
import psutil
from random import random
import sys
import time
from threading import Thread

from zalfmas_common import common
from zalfmas_common.climate import csv_file_based as csv_based
import zalfmas_capnpschemas
from zalfmas_common.model import monica_io

capnp_path = Path(os.path.dirname(zalfmas_capnpschemas.__file__))
sys.path.append(str(capnp_path))
import a_capnp
import climate_capnp
import common_capnp
import crop_capnp
import fbp_capnp
import geo_capnp
import grid_capnp
import management_capnp as mgmt_capnp
import model_capnp
import persistence_capnp
import registry_capnp as reg_capnp
import service_capnp
import soil_capnp
import storage_capnp

sys.path.append(str(capnp_path / "model" / "monica"))
import management_capnp as monica_mgmt_capnp
import monica_state_capnp

sys.path.append(str(capnp_path / "model" / "yieldstat"))
import yieldstat_capnp

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

    conMan = common.ConnectionManager()
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

async def run_monica():
    with open("sim.json") as _:
        sim_json = json.load(_)
    with open("site.json") as _:
        site_json = json.load(_)
    with open("crop.json") as _:
        crop_json = json.load(_)
    env_template = monica_io.create_env_json_from_json_config({
        "crop": crop_json,
        "site": site_json,
        "sim": sim_json,
        "climate": ""
    })
    env_template["csvViaHeaderOptions"] = sim_json["climate.csv-options"]
    env_template["pathToClimateCSV"] = "/home/berg/GitHub/klimertrag_2/data/germany/col-181.csv"

    conman = common.ConnectionManager()
    soil_service = await conman.try_connect("capnp://localhost:9901/soil", cast_as=soil_capnp.Service, retry_secs=1)
    monica_in = await conman.try_connect("capnp://localhost:9921/w_in", cast_as=fbp_capnp.Channel.Writer, retry_secs=1)
    monica_out = await conman.try_connect("capnp://localhost:9922/r_out", cast_as=fbp_capnp.Channel.Reader, retry_secs=1)

    soil_profiles = (await soil_service.closestProfilesAt(coord={"lat": 54, "lon": 12}, query={
        "mandatory": ["soilType", "sand", "clay", "organicCarbon",
                      "bulkDensity"],
        "optional": ["pH"]})).profiles

    capnp_env = model_capnp.Env.new_message()
    # capnp_env.timeSeries = timeseries
    capnp_env.soilProfile = soil_profiles[0]
    capnp_env.rest = common_capnp.StructuredText.new_message(value=json.dumps(env_template),
                                                             structure={"json": None})
    out_ip = fbp_capnp.IP.new_message(content=capnp_env,
                                      attributes=[{"key": "id", "value": common_capnp.Value(ui8=1)}])
    await monica_in.write(value=out_ip)

    in_ip = await monica_out.read()
    st = in_ip.value.as_struct(fbp_capnp.IP).content.as_text()  # struct(common_capnp.StructuredText)
    msg = json.loads(st)
    print(msg)
    #await monica_in.write(done=None)


def x():
    s = capnp.TwoPartyServer("*:11002",
                             bootstrap=csv_based.TimeSeries.from_csv_file("data/climate/climate-iso.csv", header_map={},
                                                                          pandas_csv_config={}))
    s.run_forever()
    s._decref()

    # del s


async def run_soil_service():
    con_man = common.ConnectionManager()
    sr = "capnp://B8NKD4qssSlDBlpc77kYjxxsIGx9-Fqj6tLxE5Z-qrs=@10.10.24.176:33907/1ee32f4f-f4f3-4207-994d-a5b079cb337a"
    service = await con_man.try_connect(sr, cast_as=soil_capnp.Service)
    try:
        print(await service.info())
    except Exception as e:
        print(e)

    ps = await service.closestProfilesAt(coord={"lat": 52.52, "lon": 14.11},
                                   query={"mandatory": ["soilType", "organicCarbon", "rawDensity"]})
    print(ps)
    p0 = ps.profiles[0]
    print(await p0.geoLocation())
    d = await p0.data()
    print(d)
    sr = (await p0.save()).sturdyRef
    sr_str = common.sturdy_ref_str_from_sr(sr)
    print("first profile sr:", sr_str)

    # mineral fertilizers
    print("bla")

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


async def run_time_series():
    #"capnp://bepiW8i4L69bme20_6URQav0Vy67h_mRNfeRaMcJpPg=@10.10.25.161:9999/aaaa"
    #connection = await capnp.AsyncIoStream.create_connection(host="localhost", port=9999)
    #client = capnp.TwoPartyClient(connection)
    #restorer = client.bootstrap().cast_as(persistence_capnp.Restorer)
    #cap = await restorer.restore(localRef={"text": "aaaa"})
    #await cap.m(id=123)
    #await cap.m(id=123)

    con_man = common.ConnectionManager()
    sr = "capnp://o7Hm91vJAmyg4eBdrnCOZJinc9CjRm6uVwOwo0Ni0PE=@localhost:39529/6dd2a1a7-2536-41b2-8a84-ec4cd845ad1e"
    #time_series = await con_man.try_connect(sr, cast_as=climate_capnp.TimeSeries)
    service = await con_man.try_connect(sr, cast_as=climate_capnp.Service)
    try:
        print(await service.info())

    except Exception as e:
        print(e)


async def run_climate_service():
    con_man = common.ConnectionManager()
    # restorer = conMan.try_connect("capnp://insecure@pc-berg-7920.fritz.box:10000", cast_as=persistence_capnp.Restorer)
    # service = conMan.try_connect("capnp://insecure@pc-berg-7920.fritz.box:10000/6feaf299-d620-430b-9189-36dfccf48b3a", cast_as=climate_data_capnp.CSVTimeSeriesFactory)
    sr = "capnp://psPzzv6IceWif1_WKy3M15VN1H41nlqetNb4pgmMhKA=@10.10.25.161:40617/b42d4993-3617-4e6a-a222-578ca670391a"

    service = con_man.try_connect(sr, cast_as=climate_capnp.Service)
    service = con_man.try_connect(sr, cast_as=climate_capnp.Service)
    # timeseries = conMan.try_connect("capnp://insecure@pc-berg-7920.fritz.box:10000/8e7961c5-bd16-4c1d-86fd-8347dc46185e", cast_as=climate_data_capnp.TimeSeries)
    # unsave = conMan.try_connect("capnp://insecure@pc-berg-7920.fritz.box:10000/ac544d7b-1f82-4bf8-9adb-cf586ae46287", cast_as=common_capnp.Action)
    # 4e4fe3fb-791a-4a26-9ae1-1ce52093bda5'  row: 340/col: 288
    try:
        print(service.info().wait())
    except Exception as e:
        print(e)

    #lat, lon = 51.295972, 9.829111  # Fahrenbach
    #lat, lon = 51.383333, 9.900000  # Neu-Eichenberg
    lat, lon = 52.516598, 14.121966  # Müncheberg
    ds = service.getAvailableDatasets().wait().datasets[0].data
    ds.closestTimeSeriesAt({"lat": lat, "lon": lon}).wait()

    print(ds.save().wait())

    cb = ds.streamLocations().wait().locationsCallback
    while True:
        ls = cb.nextLocations(10).wait().locations
        if len(ls) == 0:
            break
        for l in ls:
            rc = l.customData[0].value
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


def check_climate_dataset_service():
    conMan = common.ConnectionManager()
    sr = "capnp://RVIAERC9hxIviWwuPIcsy5BgngjI7B3AMOtFroI3pww=@10.10.25.31:43377/66a945ed-9fa2-4643-9c1e-5de6c440b559"
    service = conMan.try_connect(sr, cast_as=climate_capnp.Dataset)
    try:
        print(service.info().wait())
    except Exception as e:
        print(e)

    lat, lon = 52.516598, 14.121966  # Müncheberg
    ds = service.closestTimeSeriesAt({"lat": lat, "lon": lon}).wait()




    cb = ds.streamLocations().wait().locationsCallback
    while True:
        ls = cb.nextLocations(10).wait().locations
        if len(ls) == 0:
            break
        for l in ls:
            rc = l.customData[0].value
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


async def run_channel():
    con_man = common.ConnectionManager()

    wsr = "capnp://vtGotI6_o12o-aY5QoSIHuiVK66QIw0bTU1QAzTIbCc@10.10.25.161:41207/377d8e50-c809-4fef-b4c5-d6de22cd337c"
    rsr = "capnp://vtGotI6_o12o-aY5QoSIHuiVK66QIw0bTU1QAzTIbCc@10.10.25.161:41207/29a53860-2016-49f2-9d71-e6869b495f35"

    if False:
        writer = await con_man.try_connect(wsr, cast_as=fbp_capnp.Channel.Writer)
        out_ip = fbp_capnp.IP.new_message(content="hello1")
        await writer.write(value=out_ip)
        print("wrote hello")
        #writer.write(value="hello2").wait()
        #writer.write(value="hello3").wait()
        #writer.write(value="hello4").wait()
        #return

        reader = await con_man.try_connect(rsr, cast_as=fbp_capnp.Channel.Reader)
        msg = (await reader.read()).value.as_struct(fbp_capnp.IP)
        print("read", msg.content.as_text())
        #return

        for _ in range(100):
            await writer.write(value="hello")
            print("wrote hello")
            msg = await reader.read()
            print("read", msg.value.as_text())
        return

    # test channel
    # channel_sr = "capnp://insecure@10.10.24.210:37505/6c25454e-4ef9-4659-9c94-341bdd827df5"
    def writer():
        async def writer2():
            async with capnp.kj_loop():
                conMan = common.ConnectionManager()
                writer = await conMan.try_connect(wsr, cast_as=fbp_capnp.Channel.Writer)
                # channel = conMan.try_connect(channel_sr, cast_as=fbp_capnp.Channel)
                # writer = channel.writer().wait().w.as_interface(common_capnp.Writer)
                for i in range(1000):
                    #time.sleep(random())
                    await writer.write(value=a_capnp.X.new_message(t="hello_" + str(i)))
                    # writer.write(value="hello_" + str(i)).wait()
                    print("wrote: hello_" + str(i))
                    # writer.write(value=common_capnp.X.new_message(t="world")).wait()
                # print("wrote value:", "hello", "world")

        loop = asyncio.new_event_loop()
        asyncio.set_event_loop(loop)
        asyncio.run(writer2())

    Thread(target=writer).start()
    #asyncio.run(writer())
    reader = await con_man.try_connect(rsr, cast_as=fbp_capnp.Channel.Reader)
    # channel = conMan.try_connect(channel_sr, cast_as=fbp_capnp.Channel)
    # reader = channel.reader().wait().r.as_interface(common_capnp.Reader)
    for i in range(1000):
        #time.sleep(random())
        print("read:", (await reader.read()).value.as_struct(a_capnp.X).t)
        # print("read:", reader.read().wait().value.as_text())
    # print(reader.read().wait().value.as_struct(common_capnp.X).t)
    # print("read value:", value)


def run_some():
    con_man = common.ConnectionManager()

    soil = con_man.try_connect("capnp://insecure@10.10.24.210:39341/9c15ad6f-0778-4bea-b91e-b015453188b9",
                               cast_as=soil_capnp.Service)
    ps = soil.profilesAt(coord={'lat': 50.02045903295569, 'lon': 8.449222632820296},
                         query={"mandatory": ["soilType", "organicCarbon", "rawDensity"]}).wait()
    print(ps)


def run_resolver():
    con_man = common.ConnectionManager()

    sr = "capnp://lpfbBZeidvkWtZXsnOO6PgNXXXHrNZi-f8yBluWkIaQ@10.10.24.250:39345"
    resolver = con_man.try_connect(sr, cast_as=persistence_capnp.HostPortResolver)
    hp = resolver.resolve("resolver").wait()
    print(hp)

def run_resolver_registrar():
    con_man = common.ConnectionManager()

    sr = "capnp://lpfbBZeidvkWtZXsnOO6PgNXXXHrNZi-f8yBluWkIaQ@10.10.24.250:39345/OGE1NjlmYWEtMDE5ZS00OTVhLWJhMGMtODNiNDJiZjZmZWZk"
    registrar = con_man.try_connect(sr, cast_as=persistence_capnp.HostPortResolver.Registrar)
    hb = registrar.register(base64VatId="lpfbBZeidvkWtZXsnOO6PgNXXXHrNZi-f8yBluWkIaQ", host="10.10.24.250", port=39345,
                            alias="resolver").wait()
    print("let heart beat every", hb.secsHeartbeatInterval, "seconds")
    def beat():
        while True:
            print("beat")
            hb.heartbeat.beat().wait()
            time.sleep(5)

    return Thread(target=beat).start()


async def main():
    config = {
        "port": "6003",
        "server": "localhost"
    }
    common.update_config(config, sys.argv, print_config=True, allow_new_keys=True)

    #await run_monica()

    #run_resolver()
    #hb_thread = run_resolver_registrar()

    #while True:
    #    print(".", end="")
    #    time.sleep(1)

    #check_climate_dataset_service()

    #run_climate_service()

    await run_soil_service()

    #await run_time_series()

    #await run_channel()

    # run_crop_service()

    # run_fertilizer_service()

    # run_monica()

    #await run_climate_service()

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
    asyncio.run(capnp.run(main()))
