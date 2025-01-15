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
# Copyright (C: Leibniz Centre for Agricultural Landscape Research (ZALF)

import asyncio
from collections import defaultdict

import capnp
import json
import os
from pathlib import Path
import sys

from zalfmas_common import common
from zalfmas_common.climate import csv_file_based as csv_based
import zalfmas_capnp_schemas
from zalfmas_common.model import monica_io

capnp_path = Path(os.path.dirname(zalfmas_capnp_schemas.__file__))
sys.path.append(str(capnp_path))
import climate_capnp
import common_capnp
import fbp_capnp
import model_capnp
import crop_capnp

sys.path.append(str(capnp_path / "model" / "monica"))
import monica_management_capnp as mgmt_capnp
import monica_params_capnp

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


async def main():

    con_man = common.ConnectionManager()

    with open("/home/berg/GitHub/monica/installer/Hohenfinow2/sim-min.json") as _:
        sim_json = json.load(_)
    with open("/home/berg/GitHub/monica/installer/Hohenfinow2/site-min.json") as _:
        site_json = json.load(_)
    with open("/home/berg/GitHub/monica/installer/Hohenfinow2/crop-min.json") as _:
        crop_json = json.load(_)
    sim_json["include-file-base-path"] = "/home/berg/GitHub/monica-parameters"
    env_template = monica_io.create_env_json_from_json_config({
        "crop": crop_json,
        "site": site_json,
        "sim": sim_json,
        "climate": ""
    })
    #env_template["csvViaHeaderOptions"] = sim_json["climate.csv-options"]
    #env_template["climateCSV"] = climate_csv
    json_crop_params = env_template["cropRotation"][0]["worksteps"][0]["crop"]

    env_writer = await con_man.try_connect("capnp://10.10.25.25:9921/w_in", cast_as=fbp_capnp.Channel.Writer)
    env = common_capnp.StructuredText.new_message(value=json.dumps(env_template),
                                                  structure={"json": None})
    env_ip = fbp_capnp.IP.new_message(content=env)
    await env_writer.write(value=env_ip)
    print("wrote env")

    event_writer = await con_man.try_connect("capnp://10.10.25.25:9923/w_ev", cast_as=fbp_capnp.Channel.Writer)
    crop_planted = False
    crop_service_sr = "capnp://A3yYVWcdedLjLf4iyJ-NqQ4dykmq8ojseB4ghNOL0-w=@10.10.25.25:33845/cccd6b5b-9e0f-4294-a159-fd630458480c"
    crop_service = await con_man.try_connect(crop_service_sr, cast_as=crop_capnp.Service)
    cat_to_name_to_crop = defaultdict(dict)
    for e in (await crop_service.entries()).entries:
        cat_to_name_to_crop[e.categoryId][e.name] = e.ref

    with open("/home/berg/GitHub/monica/installer/Hohenfinow2/climate-min.csv") as _:
        header = _.readline().split(",")
        h2i = {h: i for i, h in enumerate(header)}
        _.readline() # skip units
        for line in _.readlines():

            events = []

            data = line.split(",")
            weather = mgmt_capnp.Params.DailyWeather.new_message(data=[
                {"key": "tavg", "value": float(data[h2i["tavg"]])},
                {"key": "tmin", "value": float(data[h2i["tmin"]])},
                {"key": "tmax", "value": float(data[h2i["tmax"]])},
                {"key": "wind", "value": float(data[h2i["wind"]])},
                {"key": "globrad", "value": float(data[h2i["globrad"]])},
                {"key": "precip", "value": float(data[h2i["precip"]])},
                {"key": "relhumid", "value": float(data[h2i["relhumid"]])}
            ])
            iso_date = data[h2i["iso-date"]]
            event = mgmt_capnp.Event.new_message(type="weather",
                                                 info={"id": iso_date},
                                                 at={"date": {"year": int(iso_date[:4]),
                                                              "month": int(iso_date[5:7]),
                                                              "day": int(iso_date[8:])},},
                                                 params=weather)
            events.append(event)
            print("created weather event for day", iso_date)

            if iso_date[5:] == "09-22": # sowing
                crop_planted = True
                crop = cat_to_name_to_crop["wheat"]["winter wheat"].cast_as(crop_capnp.Crop)
                print(await crop.info())
                sowing = mgmt_capnp.Params.Sowing.new_message(
                    cultivar="winter-wheat",
                    crop=crop
                )
                print(sowing)
                event = mgmt_capnp.Event.new_message(type="sowing",
                                                     at={"date": {"year": int(iso_date[:4]), "month": 9, "day": 22}},
                                                     info={"id": iso_date},
                                                     params=sowing)
                events.append(event)
                print("created sowing event")

            if crop_planted and iso_date[5:] == "09-05": # harvest
                harvest = mgmt_capnp.Params.Harvest.new_message()
                event = mgmt_capnp.Event.new_message(type="harvest",
                                                     at={"date": {"year": int(iso_date[:4]), "month": 9, "day": 5}},
                                                     info={"id": iso_date},
                                                     params=harvest)
                events.append(event)
                print("created harvest event")
                crop_planted = False

            # sending events
            wrq = event_writer.write_request()
            v = wrq.init("value").as_struct(fbp_capnp.IP)
            c = v.init("content").init_as_list(capnp._ListSchema(mgmt_capnp.Event), len(events))
            for i, e in enumerate(events):
                c[i] = e

            await wrq.send()
            print("wrote event(s)")

        await event_writer.write(done=None)
        print("wrote done on event channel")

    output_reader = await con_man.try_connect("capnp://10.10.25.25:9922/r_out", cast_as=fbp_capnp.Channel.Reader)
    out_msg = await output_reader.read()
    if out_msg.which() == "value":
        val = out_msg.value.as_struct(common_capnp.StructuredText)
        print(val)
        print(val.value)
        #print(out_msg.value.as_struct(common_capnp.StructuredText).value)


if __name__ == '__main__':
    asyncio.run(capnp.run(main()))
