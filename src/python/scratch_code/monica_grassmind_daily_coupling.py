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
import time
from collections import defaultdict
import capnp
import json
import os
from pathlib import Path
import subprocess as sp
import sys
import uuid

from numpy.f2py.crackfortran import include_paths
from zalfmas_common import common
from zalfmas_common.climate import csv_file_based as csv_based
import zalfmas_capnp_schemas
from zalfmas_common import service as serv
from zalfmas_common.model import monica_io
from zalfmas_fbp.run import channels as chans, ports as fbp_ports
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
import monica_state_capnp

standalone_config = {
    "path_to_channel": "/home/berg/GitHub/monica/_cmake_debug/common/channel",
    "state_as_json": False,
}
async def main(config: dict):
    common.update_config(config, sys.argv, print_config=True, allow_new_keys=False)

    con_man = common.ConnectionManager()
    channels = []

    try:
        first_chan, first_reader_sr, first_writer_sr = chans.start_first_channel(config["path_to_channel"])
        channels.append(first_chan)
        first_reader = await con_man.try_connect(first_reader_sr, cast_as=fbp_capnp.Channel.Reader)

        # create the three channels for the three ports
        channels.append(chans.start_channel(config["path_to_channel"],
                                            "env_in|" + first_writer_sr, name="env"))
        channels.append(chans.start_channel(config["path_to_channel"],
                                            "events_in|" + first_writer_sr, name="events"))
        #channels.append(chans.start_channel(config["path_to_channel"],
        #                                    "result_out|" + first_writer_sr, name="result"))
        channels.append(chans.start_channel(config["path_to_channel"],
                                            "serialized_state_in|" + first_writer_sr, name="serialized_state"))
        channels.append(chans.start_channel(config["path_to_channel"],
                                            "serialized_state_out|" + first_writer_sr, name="serialized_state"))
        channels.append(chans.start_channel(config["path_to_channel"],
                                            "port_infos|" + first_writer_sr, name="port_infos",
                                            port=9991,
                                            reader_srts="r_in"))

        port_srs = {"in": {}, "out": {}}
        port_infos_reader_sr = None
        port_infos_writer = None
        port_infos_msg = fbp_capnp.PortInfos.new_message()
        in_ports = []
        out_ports = []
        for i in range(len(channels)-1):
            p = (await first_reader.read()).value.as_struct(common_capnp.Pair)
            c_id = p.fst.as_text()
            info = p.snd.as_struct(fbp_capnp.Channel.StartupInfo)
            print("channel:", c_id, "reader_sr:", info.readerSRs[0], "writer_sr:", info.writerSRs[0])
            if c_id[-3:] == "_in":
                port_name = c_id[:-3]
                in_ports.append({"name": port_name, "sr": info.readerSRs[0]})
                port_srs["in"][port_name] = info.writerSRs[0]
            elif c_id[-4:] == "_out":
                port_name = c_id[:-4]
                out_ports.append({"name": port_name, "sr": info.writerSRs[0]})
                port_srs["out"][port_name] = info.readerSRs[0]
            else:
                port_infos_writer = await con_man.try_connect(info.writerSRs[0], cast_as=fbp_capnp.Channel.Writer)
                port_infos_reader_sr = info.readerSRs[0]
        port_infos_msg.inPorts = in_ports
        port_infos_msg.outPorts = out_ports

        # write the config to the config channel
        await port_infos_writer.write(value=port_infos_msg)

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

        env_writer = await con_man.try_connect(port_srs["in"]["env"], cast_as=fbp_capnp.Channel.Writer)
        env = common_capnp.StructuredText.new_message(value=json.dumps(env_template),
                                                      structure={"json": None})
        await env_writer.write(value=fbp_capnp.IP.new_message(content=env))
        print("send env on env channel")

        crop_planted = False
        crop_service_sr = "capnp://10.10.24.222:9997/crop"
        crop_service = await con_man.try_connect(crop_service_sr, cast_as=crop_capnp.Service)
        cat_to_name_to_crop = defaultdict(dict)
        for e in (await crop_service.entries()).entries:
            cat_to_name_to_crop[e.categoryId][e.name] = e.ref
        print("got all crop service entries")

        state_reader = await con_man.try_connect(port_srs["out"]["serialized_state"], cast_as=fbp_capnp.Channel.Reader)
        state_writer = await con_man.try_connect(port_srs["in"]["serialized_state"], cast_as=fbp_capnp.Channel.Writer)

        event_writer = await con_man.try_connect(port_srs["in"]["events"], cast_as=fbp_capnp.Channel.Writer)
        print("wrote openBracket on event channel")
        #with open("/home/berg/GitHub/monica/installer/Hohenfinow2/climate-min.csv") as _:
        with open("/home/berg/Desktop/valeh/weatherData/220/daily_mean_RES1_C403R220.csv") as _:
            header = _.readline()[:-1].split(",")
            h2i = {h.replace('"',''): i for i, h in enumerate(header)}
            _.readline() # skip units
            for line in _.readlines():

                data = line.split(",")
                data[0] = data[0].replace('"','')
                #iso_date = data[h2i["iso-date"]]
                iso_date = data[h2i["date"]]

                await event_writer.write(value=fbp_capnp.IP.new_message(type="openBracket"))

                if iso_date[5:] == "09-22": # sowing
                    crop_planted = True
                    if "winter wheat" in cat_to_name_to_crop["wheat"]:
                        crop = cat_to_name_to_crop["wheat"]["winter wheat"].cast_as(crop_capnp.Crop)
                    else:
                        crop = cat_to_name_to_crop["wheat"]["winter-wheat"].cast_as(crop_capnp.Crop)
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
                current_grassmind_weather = f"rain[mm]\tTemperature[degC]\tRadiation[mmolm-2s-1]\tDaylength[h]\tPET[mm]\tCO2[ppm]\n"
                current_grassmind_weather += \
                f'{float(data[h2i["precip"]])}\t\
                {float(data[h2i["tavg"]])}\t\
                {53.1*float(data[h2i["globrad"]])}\t\
                {float(data[h2i["DayLength"]])}\t\
                {float(data[h2i["PET"]])}\t\
                {400}\n'

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

                # save state
                if True:#False:
                    event = mgmt_capnp.Event.new_message(type="saveState",
                                                         info={"id": iso_date},
                                                         at={"date": {"year": int(iso_date[:4]),
                                                                      "month": int(iso_date[5:7]),
                                                                      "day": int(iso_date[8:])}, },
                                                         params=mgmt_capnp.Params.SaveState.new_message(
                                                             noOfPreviousDaysSerializedClimateData=10,
                                                             asJson=config["state_as_json"]))
                    print("send saveState event for day:", iso_date, "on event channel")
                    await event_writer.write(value=fbp_capnp.IP.new_message(content=event))

                    # read state
                    state_msg = await state_reader.read()
                    if state_msg.which() == "value":
                        print("received state for day:", iso_date, "on state channel")
                        state_ip = state_msg.value.as_struct(fbp_capnp.IP)
                        if config["state_as_json"]:
                            state_json_txt = state_ip.content.as_text()
                            state_json = json.loads(state_json_txt)
                            do_something_on_json_state(state_json, current_grassmind_weather)
                            await state_writer.write(value=fbp_capnp.IP.new_message(content=json.dumps(state_json)))
                        else:
                            state = state_ip.content.as_struct(monica_state_capnp.RuntimeState)
                            do_something_on_state(state, current_grassmind_weather)
                            await state_writer.write(value=fbp_capnp.IP.new_message(content=state))
                    else:
                        print("received done on state channel")

                    await event_writer.write(value=fbp_capnp.IP.new_message(type="closeBracket"))
                    print("send closeBracket on event channel")

            #await event_writer.write(value=fbp_capnp.IP.new_message(type="closeBracket"))
            #print("send closeBracket on event channel")

            #await event_writer.write(done=None)
            #print("wrote done on event channel")

        await event_writer.close()
        await env_writer.close()
        await state_writer.close()
        time.sleep(3)

        #output_reader = await con_man.try_connect(port_srs["out"]["result"], cast_as=fbp_capnp.Channel.Reader)
        #out_msg = await output_reader.read()
        #if out_msg.which() == "value":
        #    out_ip = out_msg.value.as_struct(fbp_capnp.IP)
        #    c = out_ip.content.as_struct(common_capnp.StructuredText)
        #    print(c)
        #else:
        #    print("received done on output channel")

        for channel in channels:
            channel.terminate()
        print(f"{os.path.basename(__file__)}: all channels terminated")

    except Exception as e:
        #for process in process_id_to_process.values():
        #    process.terminate()

        for channel in channels:
            channel.terminate()

        print(f"exception terminated {os.path.basename(__file__)} early. Exception:", e)

def do_something_on_json_state(state, grassmind_weather):
    # This function is called when the state is read
    # You can do whatever you want with the state here
    print(state["modelState"]["currentStepDate"])
    p = sp.Popen([
        "/home/berg/GitHub/grassmind_zalf/_cmake_debug/formind",
        "/home/berg/Desktop/valeh/GRASSMIND/4Zalf_10102024_rcp26/formind_parameters/parameter_R220C403I41.par"],
        cwd="/home/berg/Desktop/valeh/GRASSMIND")
    p.wait()
    pass




def do_something_on_state(state, grassmind_weather):
    # This function is called when the state is read
    # You can do whatever you want with the state here
    print(state.modelState.currentStepDate)
    with open("/home/berg/Desktop/valeh/GRASSMIND/4Zalf_10102024_rcp26/formind_parameters/Climate/daily_mean_RES1_C507R216.csv_Grassmind.txt", "wt") as f:
        f.write(grassmind_weather)

    p = sp.Popen([
        "/home/berg/GitHub/grassmind_zalf/_cmake_debug/formind",
        "/home/berg/Desktop/valeh/GRASSMIND/4Zalf_10102024_rcp26/formind_parameters/parameter_R216C507I41.par"],
        cwd="/home/berg/Desktop/valeh/GRASSMIND")
    p.wait()
    pass

if __name__ == '__main__':
    asyncio.run(capnp.run(main(standalone_config)))
