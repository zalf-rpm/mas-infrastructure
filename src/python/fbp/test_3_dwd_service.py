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

import json
import os
from pathlib import Path
import subprocess as sp
import time
from threading import Thread
import uuid

components = []
channels = []

r1 = str(uuid.uuid4())
w1 = str(uuid.uuid4())
channel1 = sp.Popen([
    "python", 
    "/home/berg/GitHub/mas-infrastructure/src/python/common/channel.py", 
    "port=9991",
    "no_of_channels=1",
    "buffer_size=1",
    "reader_srts="+json.dumps([[r1]]),
    "writer_srts="+json.dumps([[w1]]),
    "use_async=True"
])
channels.append(channel1)

read_file = sp.Popen([
    "python", 
    "/home/berg/GitHub/mas-infrastructure/src/python/fbp/read_file.py", 
    "out_sr=capnp://insecure@10.10.24.210:9991/"+w1,
    "skip_lines=1",
    "file=/home/berg/Desktop/Koordinaten_HE_dummy_ID.csv"
])
components.append(read_file)

r2 = str(uuid.uuid4())
w2 = str(uuid.uuid4())
channel2 = sp.Popen([
    "python", 
    "/home/berg/GitHub/mas-infrastructure/src/python/common/channel.py", 
    "port=9992",
    "no_of_channels=1",
    "buffer_size=1",
    "reader_srts="+json.dumps([[r2]]),
    "writer_srts="+json.dumps([[w2]]),
    "use_async=True"
])
channels.append(channel2)

split_string = sp.Popen([
    "python", 
    "/home/berg/GitHub/mas-infrastructure/src/python/fbp/split_string.py", 
    "in_sr=capnp://insecure@10.10.24.210:9991/"+r1, 
    "out_list_sr=capnp://insecure@10.10.24.210:9992/"+w2,
    "cast_to=float"
])
components.append(split_string)

r3 = str(uuid.uuid4())
w3 = str(uuid.uuid4())
channel3 = sp.Popen([
    "python", 
    "/home/berg/GitHub/mas-infrastructure/src/python/common/channel.py", 
    "port=9993",
    "no_of_channels=1",
    "buffer_size=1",
    "reader_srts="+json.dumps([[r3]]),
    "writer_srts="+json.dumps([[w3]]),
    "use_async=True"
])
channels.append(channel3)

to_geo_coord = sp.Popen([
    "python", 
    "/home/berg/GitHub/mas-infrastructure/src/python/fbp/to_geo_coord.py", 
    "in_vals_sr=capnp://insecure@10.10.24.210:9992/"+r2, 
    "out_coord_sr=capnp://insecure@10.10.24.210:9993/"+w3,
    "to_name=utm32n"
])
components.append(to_geo_coord)

r4 = str(uuid.uuid4())
w4 = str(uuid.uuid4())
channel4 = sp.Popen([
    "python", 
    "/home/berg/GitHub/mas-infrastructure/src/python/common/channel.py", 
    "port=9994",
    "no_of_channels=1",
    "buffer_size=1",
    "reader_srts="+json.dumps([[r4]]),
    "writer_srts="+json.dumps([[w4]]),
    "use_async=True"
])
channels.append(channel4)

proj_transformer = sp.Popen([
    "python", 
    "/home/berg/GitHub/mas-infrastructure/src/python/fbp/proj_transformer.py", 
    "in_coord_sr=capnp://insecure@10.10.24.210:9993/"+r3, 
    "out_coord_sr=capnp://insecure@10.10.24.210:9994/"+w4,
    "from_name=utm32n",
    "to_name=latlon"
])
components.append(proj_transformer)

r5 = str(uuid.uuid4())
w5 = str(uuid.uuid4())
channel5 = sp.Popen([
    "python", 
    "/home/berg/GitHub/mas-infrastructure/src/python/common/channel.py", 
    "port=9995",
    "no_of_channels=1",
    "buffer_size=1",
    "reader_srts="+json.dumps([[r5]]),
    "writer_srts="+json.dumps([[w5]]),
    "use_async=True"
])
channels.append(channel5)

dwd = sp.Popen([
    "python", 
    "/home/berg/GitHub/mas-infrastructure/src/python/services/climate/dwd_germany_service.py", 
    "in_coord_sr=capnp://insecure@10.10.24.210:9994/"+r4,
    "out_csv_sr=capnp://insecure@10.10.24.210:9995/"+w5,
    "fbp=true"
])
components.append(dwd)

write_file = sp.Popen([
    "python", 
    "/home/berg/GitHub/mas-infrastructure/src/python/fbp/write_file.py", 
    "in_sr=capnp://insecure@10.10.24.210:9995/"+r5,
    "filepath_pattern=out_fbp/csv_{id}.csv"
])
components.append(write_file)

for component in components:
    component.wait()
print("test_3.py: all components finished")

for channel in channels:
    channel.terminate()
print("test_3.py: all channels terminated")

