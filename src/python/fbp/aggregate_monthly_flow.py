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

import capnp
from collections import defaultdict
import os
from pathlib import Path
import subprocess as sp
import sys
import uuid

PATH_TO_REPO = Path(os.path.realpath(__file__)).parent.parent.parent.parent
if str(PATH_TO_REPO) not in sys.path:
    sys.path.insert(1, str(PATH_TO_REPO))

PATH_TO_PYTHON_CODE = PATH_TO_REPO / "src/python"
if str(PATH_TO_PYTHON_CODE) not in sys.path:
    sys.path.insert(1, str(PATH_TO_PYTHON_CODE))

PATH_TO_CAPNP_SCHEMAS = PATH_TO_REPO / "capnproto_schemas"
abs_imports = [str(PATH_TO_CAPNP_SCHEMAS)]

import common.common as common

common_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "common.capnp"), imports=abs_imports)

#def get_free_port():
#    with socket.socket() as s:
#        s.bind(('',0))
#        return s.getsockname()[1]

def start_first_channel(path_to_channel, name=None):
    return sp.Popen([
        path_to_channel, 
        "--name=chan_{}".format(name if name else str(uuid.uuid4())),
        "--output_srs",
    ], stdout=sp.PIPE, text=True)


def start_channel(path_to_channel, writer_sr, name=None):
    return sp.Popen([
        path_to_channel, 
        "--name=chan_{}".format(name if name else str(uuid.uuid4())),
        "--startup_info_writer_sr={}".format(writer_sr),
    ])

config = {
    "hpc": False,
    "shared_in_sr": "",
    "use_infiniband": False,
    "in_dataset_sr": "capnp://cLjtk2UwqMkycxsGh-oskFOEvuwgAEEl6i5wEKcRQMc=@10.10.24.218:40733/NjdiNWU5N2QtZWIyMS00ZDc0LWExZjAtMjFkOWJiM2YxOGZl",
    "path_to_channel": "/home/berg/GitHub/mas-infrastructure/src/cpp/common/_cmake_debug/channel",
    "path_to_mas": "/home/berg/GitHub/mas-infrastructure",
    "path_to_out_dir": "/home/berg/GitHub/mas-infrastructure/src/python/fbp/out/",
    #"path_to_dwd_csvs": "/run/user/1000/gvfs/sftp:host=login01.cluster.zalf.de,user=rpm/beegfs/common/data/climate/dwd/csvs",
}
if len(sys.argv) > 1 and __name__ == "__main__":
    for arg in sys.argv[1:]:
        k,v = arg.split("=", maxsplit=1)
        if k in config:
            if v.lower() in ["true", "false"]:
                config[k] = v.lower() == "true"
            else:
                config[k] = v
print(config)

#use_infiniband = config["use_infiniband"]
#node_hostname = socket.gethostname()
#if config["use_infiniband"]:
#    node_hostname.replace(".service", ".opa")
#node_ip = socket.gethostbyname(node_hostname)

components = {}
channels = []

try:

    first_chan = start_first_channel(config["path_to_channel"])
    channels.append(first_chan)
    first_reader_sr = None
    first_writer_sr = None
    while True:
        s = first_chan.stdout.readline().split("=", maxsplit=1)
        id, sr = s if len(s) == 2 else (None, None)
        if id and id == "readerSR":
            first_reader_sr = sr.strip()
        elif id and id == "writerSR":
            first_writer_sr = sr.strip()
        if first_reader_sr and first_writer_sr:
            break

    con_man = common.ConnectionManager()
    first_reader = con_man.try_connect(first_reader_sr, cast_as=fbp_capnp.Channel.Reader)
    
    create_components = {
        "get_climate_locations": lambda srs: sp.Popen([
            "python", 
            "{}/src/python/fbp/get_climate_locations.py".format(config["path_to_mas"]), 
            "dataset_sr={}".format(config["in_dataset_sr"]),
            "continue_after_location_id=r:361/c:142",
            "no_of_locations_at_once=10000",
        ] + srs),
        # "timeseries_to_data": lambda srs: print("""
        #     ------------------------------
        #     timeseries_to_data srs:
        #     """, srs, """
        #     ------------------------------
        #     """),
        #"timeseries_to_data": lambda srs: sp.Popen([
        #    "python", 
        #    "{}/src/python/fbp/timeseries_to_data.py".format(config["path_to_mas"]), 
        #    "in_type=capability",
        #    "subrange_start=2000-01-01",
        #    "subrange_end=2019-12-31",
        #    "subheader=tavg,globrad,precip",
        #] + srs),
        "timeseries_to_data": lambda srs: sp.Popen([
        "{}/src/cpp/fbp/_cmake_debug/timeseries-to-data".format(config["path_to_mas"]), 
        "--in_type=capability",
        "--subrange_start=2000-01-01",
        "--subrange_end=2019-12-31",
        "--subheader=tavg,globrad,precip",
        ] + list(map(lambda sr: "--"+sr, srs))),
        "aggregate_timeseries_data_monthly": lambda srs: sp.Popen([
            "python", 
            "{}/src/python/fbp/aggregate_timeseries_data_monthly.py".format(config["path_to_mas"]), 
        ] + srs),
        "write_file_1": lambda srs: sp.Popen([
            "python", 
            "{}/src/python/fbp/write_file.py".format(config["path_to_mas"]), 
            "append=true",
            "path_to_out_dir={}".format(config["path_to_out_dir"]),
        ] + srs),
        "write_file_2": lambda srs: sp.Popen([
            "python", 
            "{}/src/python/fbp/write_file.py".format(config["path_to_mas"]), 
            "append=true",
            "path_to_out_dir={}".format(config["path_to_out_dir"]),
        ] + srs),
        "write_file_3": lambda srs: sp.Popen([
            "python", 
            "{}/src/python/fbp/write_file.py".format(config["path_to_mas"]),
            "append=true",
            "path_to_out_dir={}".format(config["path_to_out_dir"]),
        ] + srs),
    }

    flow = [
        (("get_climate_locations", "out_sr"), ("timeseries_to_data", "in_sr")),
        (("timeseries_to_data", "out_sr"), ("aggregate_timeseries_data_monthly", "in_sr")),
        (("aggregate_timeseries_data_monthly", "out_sr_precip"), ("write_file_1", "in_sr")),
        (("aggregate_timeseries_data_monthly", "out_sr_tavg"), ("write_file_2", "in_sr")),
        (("aggregate_timeseries_data_monthly", "out_sr_globrad"), ("write_file_3", "in_sr")),
    ]

    comp_name_to_component = defaultdict(dict)
    chan_id_to_in_out_sr_names = {} 

    # start all channels for the flow
    for i, (start_node, end_node) in enumerate(flow):
        chan_id = start_node[0] + "->" + end_node[0]
        # start channel
        chan = start_channel(config["path_to_channel"], chan_id+"|"+first_writer_sr, name=chan_id)
        channels.append(chan)

        chan_id_to_in_out_sr_names[chan_id] = {"out": start_node[1], "in": end_node[1]}
        comp_name_to_component[start_node[0]][start_node[1]] = None
        comp_name_to_component[end_node[0]][end_node[1]] = None

    # collect channel sturdy refs in order to start components
    while True:
        p = first_reader.read().wait().value.as_struct(common_capnp.Pair)
        id = p.fst.as_text()
        start_comp_name, end_comp_name = id.split("->")

        # there should be code to start the components
        if start_comp_name in create_components and end_comp_name in create_components:
            info = p.snd.as_struct(fbp_capnp.Channel.StartupInfo)

            start_sr_name = chan_id_to_in_out_sr_names[id]["out"]
            end_sr_name = chan_id_to_in_out_sr_names[id]["in"]
            comp_name_to_component[start_comp_name][start_sr_name] = "{}={}".format(start_sr_name, info.writerSRs[0])
            comp_name_to_component[end_comp_name][end_sr_name] = "{}={}".format(end_sr_name, info.readerSRs[0])

            # sturdy refs for all ports of start component are available
            srs = comp_name_to_component[start_comp_name].values()
            if all([sr is not None for sr in srs]):
                components[start_comp_name] = create_components[start_comp_name](list(srs))

            # sturdy refs for all ports of end component are available
            srs = comp_name_to_component[end_comp_name].values()
            if all([sr is not None for sr in srs]):
                components[end_comp_name] = create_components[end_comp_name](list(srs))

            # exit loop if we started all components in the flow
            if len(components) == len(comp_name_to_component):
                break

    for component in components.values():
        component.wait()
    print("aggregate_monthly_flow.py: all components finished")

    for channel in channels:
        channel.terminate()
    print("aggregate_monthly_flow.py: all channels terminated")

except Exception as e:
    for component in components.values():
        component.terminate()

    for channel in channels:
        channel.terminate()

    print("exception terminated aggregate_monthly_flow.py early. e:", e)    
