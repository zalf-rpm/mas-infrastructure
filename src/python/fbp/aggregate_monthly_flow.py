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

import socket
import subprocess as sp
import sys
import uuid

def get_free_port():
    with socket.socket() as s:
        s.bind(('',0))
        return s.getsockname()[1]

def start_channel(path_to_channel, host, chan_port, reader_srt, writer_srt):
    return sp.Popen([
        path_to_channel, 
        "--host={}".format(host),
        "--name=chan_{}".format(chan_port),
        "--port={}".format(chan_port),
        "--reader_srts={}".format(reader_srt),
        "--writer_srts={}".format(writer_srt),
        "--verbose"
    ])

config = {
    "hpc": False,
    "shared_in_sr": "",
    "use_infiniband": False,
    "in_dataset_sr": "capnp://SzOpxKvp2MfCwo5CKSKjvr5qBF2ZOLNJunmyZCpS-e4=@10.10.24.218:43029/MTRjNWViNzctMGJmNS00ZGEwLTkwY2QtYTE0ZmEyMTZhOTYz",
    "path_to_channel": "/home/berg/GitHub/mas-infrastructure/src/cpp/common/_cmake_debug/channel",
    "path_to_mas": "/home/berg/GitHub/mas-infrastructure",
    "path_to_out_dir": "/home/berg/Desktop/aggregate_monthly_out/",
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

use_infiniband = config["use_infiniband"]
node_hostname = socket.gethostname()
if config["use_infiniband"]:
    node_hostname.replace(".service", ".opa")
node_ip = socket.gethostbyname(node_hostname)

components = []
channels = []
rs = [] # reader sturdy ref tokens
ws = [] # writer sturdy ref tokens
ps = [] # ports

rs.append(str(uuid.uuid4()))
ws.append(str(uuid.uuid4()))
ps.append(get_free_port())
channels.append(start_channel(config["path_to_channel"], node_ip, ps[-1], rs[-1], ws[-1]))

_ = sp.Popen([
    "python", 
    "{}/src/python/fbp/get_climate_locations.py".format(config["path_to_mas"]), 
    "dataset_sr={}".format(config["in_dataset_sr"]),
    "out_sr=capnp://insecure@{host}:{port}/{srt}".format(host=node_ip, port=ps[-1], srt=ws[-1]),
])
components.append(_)

rs.append(str(uuid.uuid4()))
ws.append(str(uuid.uuid4()))
ps.append(get_free_port())
channels.append(start_channel(config["path_to_channel"], node_ip, ps[-1], rs[-1], ws[-1]))

_ = sp.Popen([
    "python", 
    "{}/src/python/fbp/timeseries_to_data.py".format(config["path_to_mas"]), 
    "in_sr=capnp://insecure@{host}:{port}/{srt}".format(host=node_ip, port=ps[-2], srt=rs[-2]),
    "out_sr=capnp://insecure@{host}:{port}/{srt}".format(host=node_ip, port=ps[-1], srt=ws[-1]),
    "in_type=capability",
    "subrange_from=2000-01-01",
    "subrange_to=2019-12-31",
    "subheader=tavg,globrad,precip",
])
components.append(_)

rs.append(str(uuid.uuid4()))
ws.append(str(uuid.uuid4()))
ps.append(get_free_port())
channels.append(start_channel(config["path_to_channel"], node_ip, ps[-1], rs[-1], ws[-1]))

_ = sp.Popen([
    "python", 
    "{}/src/python/fbp/aggregate_timeseries_data_monthly.py".format(config["path_to_mas"]), 
    "in_sr=capnp://insecure@{host}:{port}/{srt}".format(host=node_ip, port=ps[-2], srt=rs[-2]),
    "out_sr=capnp://insecure@{host}:{port}/{srt}".format(host=node_ip, port=ps[-1], srt=ws[-1]),
])
components.append(_)


rs.append(str(uuid.uuid4()))
ws.append(str(uuid.uuid4()))
ps.append(get_free_port())
channels.append(start_channel(config["path_to_channel"], node_ip, ps[-1], rs[-1], ws[-1]))

_ = sp.Popen([
    "python", 
    "{}/src/python/fbp/write_file.py".format(config["path_to_mas"]), 
    "in_sr=capnp://insecure@{host}:{port}/{srt}".format(host=node_ip, port=ps[-1], srt=rs[-1]),
])
components.append(_)


for component in components:
    component.wait()
print("aggregate_monthly_flow.py: all components finished")

for channel in channels:
    channel.terminate()
print("aggregate_monthly_flow.py: all channels terminated")

