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
import threading
from collections import defaultdict
import capnp
import json
import os
from pathlib import Path
import subprocess as sp
import sys
import uuid
from zalfmas_common import common
from zalfmas_common.climate import csv_file_based as csv_based
import zalfmas_capnp_schemas
from zalfmas_common import service as serv
from zalfmas_common.model import monica_io
from zalfmas_fbp.run import channels as chans
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

def start_component(path_to_component, writer_sr, name=None, verbose=False):
    return sp.Popen([path_to_component,
                    f"--name=comp_{name if name else str(uuid.uuid4())}",
                    f"--startup_info_writer_sr={writer_sr}",
                    ] + (["--verbose"] if verbose else []))

standalone_config = {
    "path_to_channel": "/home/berg/GitHub/monica/_cmake_debug/common/channel",
}
async def main(config: dict):
    common.update_config(config, sys.argv, print_config=True, allow_new_keys=False)

    con_man = common.ConnectionManager()
    channels = []

    try:
        first_chan, first_reader_sr, first_writer_sr = chans.start_first_channel(config["path_to_channel"])
        channels.append(first_chan)
        first_reader = await con_man.try_connect(first_reader_sr, cast_as=fbp_capnp.Channel.Reader)

        chan_id = f"env"
        env_chan = chans.start_channel(config["path_to_channel"], chan_id + "|" + first_writer_sr, name=chan_id)
        channels.append(env_chan)

        p = (await first_reader.read()).value.as_struct(common_capnp.Pair)
        c_id = p.fst.as_text()
        info = p.snd.as_struct(fbp_capnp.Channel.StartupInfo)
        print(info.readerSRs[0])
        print(info.writerSRs[0])

        for channel in channels:
            channel.terminate()
        print(f"{os.path.basename(__file__)}: all channels terminated")

    except Exception as e:
        #for process in process_id_to_process.values():
        #    process.terminate()

        for channel in channels:
            channel.terminate()

        print(f"exception terminated {os.path.basename(__file__)} early. Exception:", e)




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

    env_writer = await con_man.try_connect("capnp://10.10.25.25:9921/w_in", cast_as=fbp_capnp.Channel.Writer)
    env = common_capnp.StructuredText.new_message(value=json.dumps(env_template),
                                                  structure={"json": None})
    await env_writer.write(value=fbp_capnp.IP.new_message(content=env))
    print("send env on env channel")

    event_writer = await con_man.try_connect("capnp://10.10.25.25:9923/w_ev", cast_as=fbp_capnp.Channel.Writer)
    crop_planted = False
    crop_service_sr = "capnp://A3yYVWcdedLjLf4iyJ-NqQ4dykmq8ojseB4ghNOL0-w=@10.10.25.25:33845/cccd6b5b-9e0f-4294-a159-fd630458480c"
    crop_service = await con_man.try_connect(crop_service_sr, cast_as=crop_capnp.Service)
    cat_to_name_to_crop = defaultdict(dict)
    for e in (await crop_service.entries()).entries:
        cat_to_name_to_crop[e.categoryId][e.name] = e.ref
    print("got all crop service entries")

    await event_writer.write(value=fbp_capnp.IP.new_message(type="openBracket"))
    print("wrote openBracket on event channel")
    with open("/home/berg/GitHub/monica/installer/Hohenfinow2/climate-min.csv") as _:
        header = _.readline().split(",")
        h2i = {h: i for i, h in enumerate(header)}
        _.readline() # skip units
        for line in _.readlines():

            data = line.split(",")
            iso_date = data[h2i["iso-date"]]

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
                await event_writer.write(value=fbp_capnp.IP.new_message(content=event))
                print("sent sowing event on event channel")

            if crop_planted and iso_date[5:] == "09-05": # harvest
                harvest = mgmt_capnp.Params.Harvest.new_message()
                event = mgmt_capnp.Event.new_message(type="harvest",
                                                     at={"date": {"year": int(iso_date[:4]), "month": 9, "day": 5}},
                                                     info={"id": iso_date},
                                                     params=harvest)
                await event_writer.write(value=fbp_capnp.IP.new_message(content=event))
                print("sent harvest event on event channel")
                crop_planted = False

            # finally send weather to initiate daily step
            weather = mgmt_capnp.Params.DailyWeather.new_message(data=[
                {"key": "tavg", "value": float(data[h2i["tavg"]])},
                {"key": "tmin", "value": float(data[h2i["tmin"]])},
                {"key": "tmax", "value": float(data[h2i["tmax"]])},
                {"key": "wind", "value": float(data[h2i["wind"]])},
                {"key": "globrad", "value": float(data[h2i["globrad"]])},
                {"key": "precip", "value": float(data[h2i["precip"]])},
                {"key": "relhumid", "value": float(data[h2i["relhumid"]])}
            ])
            event = mgmt_capnp.Event.new_message(type="weather",
                                                 info={"id": iso_date},
                                                 at={"date": {"year": int(iso_date[:4]),
                                                              "month": int(iso_date[5:7]),
                                                              "day": int(iso_date[8:])},},
                                                 params=weather)
            await event_writer.write(value=fbp_capnp.IP.new_message(content=event))
            print("send weather event for day:", iso_date, "on event channel")

        await event_writer.write(value=fbp_capnp.IP.new_message(type="closeBracket"))
        print("send closeBracket on event channel")

        #await event_writer.write(done=None)
        #print("wrote done on event channel")

    output_reader = await con_man.try_connect("capnp://10.10.25.25:9922/r_out", cast_as=fbp_capnp.Channel.Reader)
    out_msg = await output_reader.read()
    if out_msg.which() == "value":
        out_ip = out_msg.value.as_struct(fbp_capnp.IP)
        c = out_ip.content.as_struct(common_capnp.StructuredText)
        print(c)
    else:
        print("received done on output channel")

if __name__ == '__main__':
    asyncio.run(capnp.run(main(standalone_config)))
