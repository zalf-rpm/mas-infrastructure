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

r = str(uuid.uuid4())
ws = [str(uuid.uuid4()) for _ in range(100)]
channel = sp.Popen([
    "python", 
    "/home/berg/GitHub/mas-infrastructure/src/python/common/channel.py", 
    "port=9990",
    "no_of_channels=1",
    "buffer_size=1",
    "reader_srts="+json.dumps([[r]]),
    "writer_srts="+json.dumps([ws]),
    "use_async=True"
])

time.sleep(5)

console = sp.Popen([
    "python", 
    "/home/berg/GitHub/mas-infrastructure/src/python/fbp/console.py", 
    "in_sr=capnp://localhost:9990/"+r
])

read_files = []
for i, w in enumerate(ws):
    read_files.append(sp.Popen([
        "python", 
        "/home/berg/GitHub/mas-infrastructure/src/python/fbp/read_file.py", 
        "out_sr=capnp://insecure@10.10.24.210:9990/"+w,
        "file=src/python/fbp/test"+str((i % 4)+1)+".txt"
    ]))

for read_file in read_files:
    read_file.wait()

print("test_2.py: read_files joined")
console.wait()
print("test_2.py: console joined")
channel.terminate()
print("test_2.py: after channel.join")

