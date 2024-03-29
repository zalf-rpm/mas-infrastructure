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

import os
from pathlib import Path
import subprocess as sp
import time
from threading import Thread
import uuid

r = str(uuid.uuid4())
w = str(uuid.uuid4())
w2 = str(uuid.uuid4())
w3 = str(uuid.uuid4())
w4 = str(uuid.uuid4())
channel = Thread(
    name="channel",
    target=sp.run, 
    args=(["python", "/home/berg/GitHub/mas-infrastructure/src/python/common/channel.py", 
        "port=9990",
        "no_of_channels=1",
        "buffer_size=1",
        "reader_srts=[[\""+r+"\"]]",
        "writer_srts=[[\""+w+"\",\""+w2+"\",\""+w3+"\",\""+w4+"\"]]",
        "use_async=True"
    ],),
    daemon=True
)
channel.start()

console = Thread(
    name="console",
    target=sp.run, 
    args=(["python", "/home/berg/GitHub/mas-infrastructure/src/python/fbp/console.py", 
        "in_sr=capnp://localhost:9990/"+r
    ],),
    daemon=False
)
console.start()

read_file = Thread(
    name="read_file_1",
    target=sp.run, 
    args=(["python", "/home/berg/GitHub/mas-infrastructure/src/python/fbp/read_file.py", 
        "out_sr=capnp://insecure@10.10.24.210:9990/"+w,
        "file=src/python/fbp/test1.txt"
    ],),
    daemon=False
)
read_file.start()

read_file_2 = Thread(
    name="read_file_2",
    target=sp.run, 
    args=(["python", "/home/berg/GitHub/mas-infrastructure/src/python/fbp/read_file.py", 
        "out_sr=capnp://insecure@10.10.24.210:9990/"+w2,
        "file=src/python/fbp/test2.txt"
    ],),
    daemon=False
)
read_file_2.start()

read_file_3 = Thread(
    name="read_file_3",
    target=sp.run, 
    args=(["python", "/home/berg/GitHub/mas-infrastructure/src/python/fbp/read_file.py", 
        "out_sr=capnp://insecure@10.10.24.210:9990/"+w3,
        "file=src/python/fbp/test3.txt"
    ],),
    daemon=False
)
read_file_3.start()

read_file_4 = Thread(
    name="read_file_4",
    target=sp.run, 
    args=(["python", "/home/berg/GitHub/mas-infrastructure/src/python/fbp/read_file.py", 
        "out_sr=capnp://insecure@10.10.24.210:9990/"+w4,
        "file=src/python/fbp/test4.txt"
    ],),
    daemon=False
)
read_file_4.start()

read_file.join()
read_file_2.join()
read_file_3.join()
read_file_4.join()
print("test_1.py: read_files joined")
console.join()
print("test_1.py: console joined")
channel.join()
#print("test_1.py: after channel.join")



"""
# wait for file to be written
time.sleep(2)

# load the sturdy refs from the file written by the channel service
srs = {}
with os.fdopen(fd) as _:
    srs = json.load(_)
print("read sturdy refs:", srs)

# start MONICA 1
mout_1 = open("out_1", "wt")
monica_1 = Thread(
    target=sp.run, 
    args=(["/home/berg/GitHub/monica/_cmake_linux_debug/monica-run", 
        "-icrsr", srs["reader_1"][0],
        "-icwsr", srs["writer_2"][0],
        "-c", "/home/berg/GitHub/monica/installer/Hohenfinow2/crop-inter_w.json",
        "/home/berg/GitHub/monica/installer/Hohenfinow2/sim-min.json"],),
    kwargs={
        "stdout": mout_1, 
        "text": True,
        "env": {"MONICA_PARAMETERS": "/home/berg/GitHub/monica-parameters"}
    }
)
monica_1.start()

# start MONICA 2
mout_2 = open("out_2", "wt")
monica_2 = Thread(
    target=sp.run,
    args=(["/home/berg/GitHub/monica/_cmake_linux_debug/monica-run", 
        "-icrsr", srs["reader_2"][0],
        "-icwsr", srs["writer_1"][0],
        "-c", "/home/berg/GitHub/monica/installer/Hohenfinow2/crop-inter_s.json",
        "/home/berg/GitHub/monica/installer/Hohenfinow2/sim-min.json"],),
    kwargs={
        "stdout": mout_2, 
        "text": True,
        "env": {"MONICA_PARAMETERS": "/home/berg/GitHub/monica-parameters"}
    },
)
monica_2.start()

# wait for MONICAs to finish
monica_1.join()
mout_1.close()
monica_2.join()
mout_2.close()

"""


