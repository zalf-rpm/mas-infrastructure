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

sys.path.append(str(capnp_path / "model" / "monica"))
import monica_management_capnp as mgmt_capnp

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
    with open("/home/berg/GitHub/monica/installer/Hohenfinow2/climate-min.csv") as _:
        climate_csv = _.read()
    sim_json["include-file-base-path"] = "/home/berg/GitHub/monica-parameters"
    env_template = monica_io.create_env_json_from_json_config({
        "crop": crop_json,
        "site": site_json,
        "sim": sim_json,
        "climate": ""
    })
    env_template["csvViaHeaderOptions"] = sim_json["climate.csv-options"]
    env_template["climateCSV"] = climate_csv

    env_writer = await con_man.try_connect("capnp://10.10.25.25:9921/w_in", cast_as=fbp_capnp.Channel.Writer)
    env = common_capnp.StructuredText.new_message(value=json.dumps(env_template),
                                                  structure={"json": None})
    env_ip = fbp_capnp.IP.new_message(content=env)
    await env_writer.write(value=env_ip)
    print("wrote env")

    event_writer = await con_man.try_connect("capnp://10.10.25.25:9923/w_ev", cast_as=fbp_capnp.Channel.Writer)
    weather = mgmt_capnp.Params.DailyWeather.new_message(data=[
        {"key": "tavg", "value": -0.6},
        {"key": "tmin", "value": -1.5},
        {"key": "tmax", "value": 1},
        {"key": "wind", "value": 6.7},
        {"key": "globrad", "value": 0.52},
        {"key": "precip", "value": 0},
        {"key": "relhumid", "value": 90}
    ])
    event = mgmt_capnp.Event.new_message(type="weather",
                                    info={"id": "1", "name": "day one"},
                                    params=weather)
    event_ip = fbp_capnp.IP.new_message(content=event)
    await event_writer.write(value=event_ip)
    print("wrote weather event")

    #output_reader = await con_man.try_connect("capnp://10.10.25.25:9922/r_out", cast_as=fbp_capnp.Channel.Reader)



if __name__ == '__main__':
    asyncio.run(capnp.run(main()))
