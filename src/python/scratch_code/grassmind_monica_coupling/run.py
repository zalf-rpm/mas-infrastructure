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
from datetime import date
import io
import subprocess
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
    "grassmind_current_working_dir": "/home/berg/Desktop/valeh/GRASSMIND",
    "path_to_formind_exe": "/home/berg/GitHub/grassmind_zalf/_cmake_debug/formind",
    "path_to_full_weather_file": "/home/berg/Desktop/valeh/weatherData/{row:03}/daily_mean_RES1_C{col:03}R{row:03}.csv",
    "path_to_grassmind_weather_file": "/home/berg/Desktop/valeh/GRASSMIND/4Zalf_10102024_rcp26/formind_parameters/Climate/daily_mean_RES1_C{col:03}R{row:03}.csv_Grassmind.txt",
    "path_to_grassmind_soil_file": "/home/berg/Desktop/valeh/GRASSMIND/4Zalf_10102024_rcp26/formind_parameters/Soil/soil_R{row:03}C{col:03}.txt",
    "path_to_grassmind_param_file": "/home/berg/Desktop/valeh/GRASSMIND/4Zalf_10102024_rcp26/formind_parameters/parameter_R{row:03}C{col:03}I41.par",
    "path_to_result_div": "/home/berg/Desktop/valeh/GRASSMIND/4Zalf_10102024_rcp26/results/parameter_R{row:03}C{col:03}I41.div"
}
async def main(config: dict):
    common.update_config(config, sys.argv, print_config=True, allow_new_keys=False)

    con_man = common.ConnectionManager()
    channels = []

    paths = {
        "cwd": config["grassmind_current_working_dir"],
        "full_weather": config["path_to_full_weather_file"].format(row=220, col=403),
        "weather": config["path_to_grassmind_weather_file"].format(row=220, col=403),
        "soil": config["path_to_grassmind_soil_file"].format(row=220, col=403),
        "params": config["path_to_grassmind_param_file"].format(row=220, col=403),
        "formind": config["path_to_formind_exe"].format(row=220, col=403),
        "div": config["path_to_result_div"].format(row=220, col=403),
    }

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

        with open("sim.json") as _:
            sim_json = json.load(_)
        with open("site.json") as _:
            site_json = json.load(_)
        with open("crop.json") as _:
            crop_json = json.load(_)
        #sim_json["include-file-base-path"] = "/home/berg/GitHub/monica-parameters"
        #sim_json["include-file-base-path"] = "/home/berg/GitHub/mas-infrastructure/src/python/scratch_code/grassmind_monica_coupling/params"
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

        iso_dates = []
        grassmind_climate = ["rain[mm]\tTemperature[degC]\tRadiation[mmolm-2s-1]\tDaylength[h]\tPET[mm]\tCO2[ppm]\n"]
        monica_climate = []
        with open(paths["full_weather"]) as _:
            header = _.readline()[:-1].split(",")
            h2i = {h.replace('"',''): i for i, h in enumerate(header)}
            #_.readline() # skip units
            for line in _.readlines():
                data = line.split(",")
                data[0] = data[0].replace('"','')
                #iso_date = data[h2i["iso-date"]]
                iso_date = data[h2i["date"]]

                if int(iso_date[:4]) < 2021:
                    continue

                iso_dates.append(iso_date)
                grassmind_climate.append(f'{float(data[h2i["precip"]])}\t{float(data[h2i["tavg"]])}\t{53.1 * float(data[h2i["globrad"]])}\t{float(data[h2i["DayLength"]])}\t{float(data[h2i["PET"]])}\t{400}')
                monica_climate.append(mgmt_capnp.Params.DailyWeather.new_message(data=[
                    {"key": "tavg", "value": float(data[h2i["tavg"]])},
                    {"key": "tmin", "value": float(data[h2i["tmin"]])},
                    {"key": "tmax", "value": float(data[h2i["tmax"]])},
                    {"key": "wind", "value": float(data[h2i["wind"]])},
                    {"key": "globrad", "value": float(data[h2i["globrad"]])},
                    {"key": "precip", "value": float(data[h2i["precip"]])},
                    {"key": "relhumid", "value": float(data[h2i["relhumid"]])}
                ]))

        ferts = {
            "AN": {
                "id": "AN",
                "name": "Ammonium Nitrate",
                "carbamid": 0,
                "nh4": 0.5,
                "no3": 0.5,
            }
        }

        abs_events = {
            "2021-03-01": create_sowing_event(cat_to_name_to_crop["Grass_Species4"]["Grass_CLV4"].cast_as(crop_capnp.Crop)),
        }
        rel_events = {
            "06-15": create_cutting_event([
                {"organ": "leaf", "value": 0.15, "unit": "lai", "cutOrLeft": "left", "exportPercentage": 100.0},
                {"organ": "shoot", "value": 100, "unit": "biomass", "cutOrLeft": "left", "exportPercentage": 100.0}]),
            "06-20": create_n_demand_fert_event(n_demand=20.0, depth=0.3, partition=ferts["AN"]),
            "08-15": create_cutting_event([
                {"organ": "leaf", "value": 0.4, "unit": "lai", "cutOrLeft": "left", "exportPercentage": 100.0},
                {"organ": "shoot", "value": 100, "unit": "biomass", "cutOrLeft": "left", "exportPercentage": 100.0}]),
            "08-20": create_n_demand_fert_event(n_demand=20.0, depth=0.3, partition=ferts["AN"]),
            "10-15": create_cutting_event(
                [{"organ": "leaf", "value": 0.4, "unit": "lai", "cutOrLeft": "left", "exportPercentage": 100.0},
                 {"organ": "shoot", "value": 100, "unit": "biomass", "cutOrLeft": "left",
                  "exportPercentage": 100.0}]),
            "10-20": create_n_demand_fert_event(n_demand=20.0, depth=0.3, partition=ferts["AN"]),
        }

        #print("wrote openBracket on event channel")
        for day_index, iso_date in enumerate(iso_dates[:-365]):
            current_date = date.fromisoformat(iso_date)

            await event_writer.write(value=fbp_capnp.IP.new_message(type="openBracket"))

            if iso_date in abs_events:
                current_events = abs_events[iso_date] if abs_events[iso_date] is list else [abs_events[iso_date]]
                for cev in current_events:
                    event = cev(current_date)
                    await event_writer.write(value=fbp_capnp.IP.new_message(content=event))

            rel_date = iso_date[5:]
            if rel_date in rel_events:
                current_events = rel_events[rel_date] if rel_events[rel_date] is list else [rel_events[rel_date]]
                for cev in current_events:
                    event = cev(current_date)
                    await event_writer.write(value=fbp_capnp.IP.new_message(content=event))

            # finally send weather to do daily step
            weather_event = create_weather_event(monica_climate[day_index])(current_date)
            await event_writer.write(value=fbp_capnp.IP.new_message(content=weather_event))
            #print("send weather event for day:", iso_date, "on event channel")

            # save state
            save_state_event = create_save_state_event(10, False)(current_date)
            #print("send saveState event for day:", iso_date, "on event channel")
            await event_writer.write(value=fbp_capnp.IP.new_message(content=save_state_event))

            # read state
            state_msg = await state_reader.read()
            if state_msg.which() == "value":
                #print("received state for day:", iso_date, "on state channel")
                state_ip = state_msg.value.as_struct(fbp_capnp.IP)
                old_state = state_ip.content.as_struct(monica_state_capnp.RuntimeState)
                new_state = run_grassmind_on_monica_state(old_state, day_index, grassmind_climate, paths)
                await state_writer.write(value=fbp_capnp.IP.new_message(content=new_state))
            else:
                print("received done on state channel")

            await event_writer.write(value=fbp_capnp.IP.new_message(type="closeBracket"))
            #print("send closeBracket on event channel")

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
        print(f"exception terminated {os.path.basename(__file__)} early. Exception:", e)

        #for process in process_id_to_process.values():
        #    process.terminate()
        for channel in channels:
            channel.terminate()

def create_save_state_event(no_of_prev_days_to_serialize=10, serialize_as_json=False):
    return lambda at: mgmt_capnp.Event.new_message(type="saveState",
                                                   at={"date": {"year": at.year, "month": at.month, "day": at.day}},
                                                   info={"id": at.isoformat()},
                                                   params=mgmt_capnp.Params.SaveState.new_message(
                                                       noOfPreviousDaysSerializedClimateData=no_of_prev_days_to_serialize,
                                                       asJson=serialize_as_json))

def create_weather_event(daily_weather):
    return lambda at: mgmt_capnp.Event.new_message(type="weather",
                                                   at={"date": {"year": at.year, "month": at.month, "day": at.day}},
                                                   info={"id": at.isoformat()},
                                                   params=daily_weather)

def create_sowing_event(crop):
    return lambda at: mgmt_capnp.Event.new_message(type="sowing",
                                                   at={"date": {"year": at.year, "month": at.month, "day": at.day}},
                                                   info={"id": at.isoformat()},
                                                   params=mgmt_capnp.Params.Sowing.new_message(
                                                       cultivar="Grass_CLV4",
                                                       crop=crop))

def create_harvest_event():
    return lambda at: mgmt_capnp.Event.new_message(type="harvest",
                                                   at={"date": {"year": at.year, "month": at.month, "day": at.day}},
                                                   info={"id": at.isoformat()},
                                                   params=mgmt_capnp.Params.Harvest.new_message())

def create_n_fert_event(amount: float, partition: dict):
    return lambda at: mgmt_capnp.Event.new_message(type="mineralFertilization",
                                                   at={"date": {"year": at.year, "month": at.month, "day": at.day}},
                                                   info={"id": at.isoformat()},
                                                   params=mgmt_capnp.Params.MineralFertilization.new_message(
                                                       amount = amount,
                                                       partition = partition))

def create_n_demand_fert_event(n_demand: float, depth: float, partition: dict, stage: int = 1):
    return lambda at: mgmt_capnp.Event.new_message(type="nDemandFertilization",
                                                   at={"date": {"year": at.year, "month": at.month, "day": at.day}},
                                                   info={"id": at.isoformat()},
                                                   params=mgmt_capnp.Params.NDemandFertilization.new_message(
                                                       nDemand = n_demand,
                                                       depth = depth,
                                                       stage = stage,
                                                       partition = partition))

def create_cutting_event(cutting_spec: list[dict]):
    return lambda at: mgmt_capnp.Event.new_message(type="cutting",
                                                   at={"date": {"year": at.year, "month": at.month, "day": at.day}},
                                                   info={"id": at.isoformat()},
                                                   params=mgmt_capnp.Params.Cutting.new_message(cuttingSpec = cutting_spec))


def run_grassmind_on_monica_state(old_state, day_index, grassmind_climate, paths):
    if not old_state.modelState._has("currentCropModule"):
        return old_state

    new_state = old_state
    if not hasattr(run_grassmind_on_monica_state, "initial_param_values"):
        run_grassmind_on_monica_state.initial_param_values = {
            "SpecificLeafArea": old_state.modelState.currentCropModule.cultivarParams.specificLeafArea,
            "StageKcFactor": old_state.modelState.currentCropModule.cultivarParams.stageKcFactor,
            "DroughtStressThreshold": old_state.modelState.currentCropModule.cultivarParams.droughtStressThreshold,
            "CropSpecificMaxRootingDepth": old_state.modelState.currentCropModule.cultivarParams.cropSpecificMaxRootingDepth,
        }

    with open(paths["weather"], "wt") as f:
        f.write(grassmind_climate[0])
        for line in grassmind_climate[day_index:day_index + 365]:
            f.write(line)
            f.write("\n")

    with open(paths["soil"], "wt") as f:
        f.write(create_grassmind_soil_from_state(old_state))

    p = sp.Popen([paths["formind"], paths["params"]], cwd=paths["cwd"], stdout=subprocess.DEVNULL)
    p.wait()

    # read .div file to get the current fractions
    rel_species_abundance = None
    with open(paths["div"]) as f:
        lines = f.readlines()
        rel_species_abundance = list(map(float, lines[4].split("\t")[2:6]))

    if rel_species_abundance:
        params = calc_community_level_params(rel_species_abundance)
        print(old_state.modelState.currentStepDate, "community params:", params)
        new_state = old_state.as_builder()
        cps = new_state.modelState.currentCropModule.cultivarParams
        cps.specificLeafArea = list(map(lambda v: v * params["SpecificLeafArea"], list(cps.specificLeafArea)))
        cps.stageKcFactor = list(map(lambda v: v * params["StageKcFactor"], list(cps.stageKcFactor)))
        cps.droughtStressThreshold = list(map(lambda v: v * params["DroughtStressThreshold"],
                                              list(cps.droughtStressThreshold)))
        cps.cropSpecificMaxRootingDepth = params["CropSpecificMaxRootingDepth"]
    return new_state

def create_grassmind_soil_from_state(state):
    sc = state.modelState.soilColumn
    sb = io.StringIO()
    sb.write("Silt\t\Clay\tSand\n")
    silt = 1 - sc.layers[0].sps.soilSandContent - sc.layers[0].sps.soilClayContent
    sb.write(f"{silt}\t\{sc.layers[0].sps.soilClayContent}\t{sc.layers[0].sps.soilSandContent}\n")
    sb.write("\n")
    sb.write("Layer\tRWC[-]\tFC[V%]\tPWP[V%]\tMinN[gm-2]\tPOR[V%]\tKS[mm/d]\n")
    for i, l in enumerate(sc.layers):
        n_kg_per_m3 = l.soilNO3 + l.soilNO2 + l.soilNH4
        n_kg_per_m2 = n_kg_per_m3 * 0.01 # m3 -> m2 (0.1m layer thickness)
        n_g_per_m2 = n_kg_per_m2 * 1000.0 # kg -> g
        ks = l.sps._get("lambda") * 1000.0 # m/d -> mm/d
        sb.write(f"{i}\t{round(l.soilMoistureM3, 5)}\t{round(l.sps.fieldCapacity*100.0, 2)}\t{round(l.sps.permanentWiltingPoint*100.0,2)}\t{round(n_g_per_m2, 5)}\t{round(l.sps.saturation*100.0, 2)}\t{round(ks,2)}\n")
    return sb.getvalue()

def calc_community_level_params(s_i_s):
    p_i_s = {
        "SpecificLeafArea": {"S_PFTs": [0.36, 0.47, 0.03, 0.36], "min": 0.6, "max": 1.4, "values": [0.8, 0.75, 0.7, 0.6]},
        "StageKcFactor": {"S_PFTs": [0.36, 0.16, 0.03, 0.4], "min": 0.6, "max": 1.4, "values": [1.3, 1.1, 0.91, 0.83]},
        "DroughtStressThreshold": {"S_PFTs": [0.0, 0.16, 0.78, 0.58], "min": 0.2, "max": 1.0, "values": [0.35, 0.5, 0.75, 0.9]},
        "CropSpecificMaxRootingDepth": {"S_PFTs": [0.36, 0.31, 0.03, 0.98], "min": 0.1, "max": 0.3, "values": [0.15, 0.2, 0.25, 0.3]},
    }
    res = {}
    for param_name, ps in p_i_s.items():
        sum_sip_x_si_x_pi = 0
        sum_sip_x_si = 0
        for i, s_i_p in enumerate(ps["S_PFTs"]):
            sum_sip_x_si_x_pi += s_i_p * s_i_s[i] * ps["values"][i]
            sum_sip_x_si += s_i_p * s_i_s[i]
        res[param_name] = ps["min"] + (sum_sip_x_si_x_pi/sum_sip_x_si)*(ps["max"] - ps["min"])
    return res

if __name__ == '__main__':
    asyncio.run(capnp.run(main(standalone_config)))
