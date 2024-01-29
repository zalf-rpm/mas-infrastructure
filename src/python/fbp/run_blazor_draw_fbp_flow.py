#!/usr/bin/python
# -*- coding: UTF-8
from collections import defaultdict
import json

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

from pkgs.common import common

common_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "common.capnp"), imports=abs_imports)
fbp_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "fbp.capnp"), imports=abs_imports)

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
        #"--verbose",
        "--startup_info_writer_sr={}".format(writer_sr),
    ])


config = {
    "hpc": False,
    "use_infiniband": False,
    "path_to_flow": "/home/berg/Downloads/test2.json",
    "path_to_channel": "/home/berg/GitHub/mas-infrastructure/src/cpp/common/_cmake_debug/channel",
    "path_to_out_dir": "/home/berg/GitHub/mas-infrastructure/src/python/fbp/out/",
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

# read flow file
#flow_json = None
with open(config["path_to_flow"], "r") as _:
    flow_json = json.load(_)

if not flow_json:
    print(f"{os.path.basename(__file__)} error: could not read flow file")
    sys.exit(1)

# create dicts for easy access to nodes and links
node_id_to_node = {node["node_id"]: node for node in flow_json["nodes"]}
links = [{"out": link["source"], "in": link["target"]} for link in flow_json["links"]]

# read components file
path_to_components = flow_json["components"]
#component_id_to_component = None
# create dicts for easy access to components
with open(path_to_components, "r") as _:
    component_id_to_component = {
        component["id"]: component
        for cat, comps in json.load(_).items()
        for component in comps
    }

# a mapping of node_id to lambdas for process creation
process_id_to_Popen_args = {}
iip_process_ids = set()
for node_id, node in node_id_to_node.items():
    component = component_id_to_component[node["component_id"]]
    if not component:
        component = node["inline_component"]
    if not component:
        continue
    # we will send IIPs directly on the channel to the receiving process
    if component.get("type", "") == "CapnpFbpIIP":
        iip_process_ids.add(node_id)
        continue
    # args
    args = ([component["interpreter"]] if "interpreter" in component else []) + [component["path"]]
    for k, v in node["data"]["cmd_params"].items():
        args.append(f"{k}={v}")
    process_id_to_Popen_args[node_id] = args

process_id_to_process = {}
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

    process_id_to_process_srs = defaultdict(dict)
    chan_id_to_in_out_sr_names = {} 

    # start all channels for the flow
    for link in links:
        out_port = link["out"]
        in_port = link["in"]
        chan_id = f"{out_port['node_id']}.{out_port['port']}->{in_port['node_id']}.{in_port['port']}"
        # start channel
        chan = start_channel(config["path_to_channel"], chan_id + "|" + first_writer_sr, name=chan_id)
        channels.append(chan)

        chan_id_to_in_out_sr_names[chan_id] = {"out": out_port['port'], "in": in_port['port']}
        process_id_to_process_srs[out_port["node_id"]][out_port["port"]] = None
        process_id_to_process_srs[in_port["node_id"]][in_port["port"]] = None

    # collect channel sturdy refs in order to start components
    while True:
        p = first_reader.read().wait().value.as_struct(common_capnp.Pair)
        out_process_and_port, in_process_and_port = p.fst.as_text().split("->")
        out_process_id, out_port_name = out_process_and_port.split(".")
        in_process_id, in_port_name = in_process_and_port.split(".")

        # there should be code to start the components
        if ((out_process_id in process_id_to_Popen_args or out_process_id in iip_process_ids)
                and in_process_id in process_id_to_Popen_args):
            info = p.snd.as_struct(fbp_capnp.Channel.StartupInfo)

            if out_process_id in iip_process_ids:
                out_writer = con_man.try_connect(info.writerSRs[0], cast_as=fbp_capnp.Channel.Writer)
                content = node_id_to_node[out_process_id]["data"]["content"]
                out_ip = fbp_capnp.IP.new_message(content=content)
                out_writer.write(value=out_ip).wait()
                out_writer.write(done=None).wait()
                out_writer.close().wait()
                del out_writer
                # not needed anymore since we sent the IIP
                del process_id_to_process_srs[out_process_id]
            else:
                process_id_to_process_srs[out_process_id][out_port_name] = \
                    f"{out_port_name}_out_sr={info.writerSRs[0]}".replace('out_out_', 'out_')
            process_id_to_process_srs[in_process_id][in_port_name] = \
                f"{in_port_name}_in_sr={info.readerSRs[0]}".replace('in_in_', 'in_')

            # sturdy refs for all ports of start component are available
            # check for a non IIP process_id if the process can be started
            if out_process_id not in iip_process_ids:
                srs = process_id_to_process_srs[out_process_id].values()
                if all([sr is not None for sr in srs]):
                    process_id_to_process[out_process_id] = sp.Popen(process_id_to_Popen_args[out_process_id] + list(srs))

            # sturdy refs for all ports of end component are available
            srs = process_id_to_process_srs[in_process_id].values()
            if all([sr is not None for sr in srs]):
                process_id_to_process[in_process_id] = sp.Popen(process_id_to_Popen_args[in_process_id] + list(srs))

            # exit loop if we started all components in the flow
            if len(process_id_to_process) == len(process_id_to_process_srs):
                break

    for process in process_id_to_process.values():
        process.wait()

    print(f"{os.path.basename(__file__)}: all components finished")

    for channel in channels:
        channel.terminate()
    print(f"{os.path.basename(__file__)}: all channels terminated")

except Exception as e:
    for process in process_id_to_process.values():
        process.terminate()

    for channel in channels:
        channel.terminate()

    print(f"exception terminated {os.path.basename(__file__)} early. Exception:", e)
