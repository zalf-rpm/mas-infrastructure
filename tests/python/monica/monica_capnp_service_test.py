#!/usr/bin/python
# -*- coding: UTF-8

# This Source Code Form is subject to the terms of the Mozilla Public
# License, v. 2.0. If a copy of the MPL was not distributed with this
# file, You can obtain one at http://mozilla.org/MPL/2.0/. */

# Authors:
# Michael Berg-Mohnicke <michael.berg@zalf.de>
#
# Maintainers:
# Currently maintained by the authors.
#
# This file has been created at the Institute of
# Landscape Systems Analysis at the ZALF.
# Copyright (C: Leibniz Centre for Agricultural Landscape Research (ZALF)

import os
import sys
import json
import pytest
from copy import deepcopy
import random

from multiprocessing import Process
from subprocess import Popen
import time
from datetime import date, timedelta

import capnp
#capnp.add_import_hook(additional_paths=[".."])
import model_capnp
import climate_data_capnp
import soil_data_capnp
import common_capnp

def test_monica(monica_cap, monica_env, time_series_cap):
    env = deepcopy(monica_env)
    env["customId"] = random.randint(0, 1000)

    env_struct = common_capnp.Common.StructuredText.new_message(value=json.dumps(env), structure={"json": None})
    res = monica_cap.run({"rest": env_struct, "timeSeries": time_series_cap}).wait().result.as_struct(common_capnp.Common.StructuredText)
    assert len(res.value) > 0
    res_j = json.loads(res.value)
    assert res_j["customId"] == env["customId"]

    # in the minimal hohenfinow2 examples there's a "run" section with the summed precipitation of the whole run = slightly more than 3500mm/run
    run = list(filter(lambda r: r["origSpec"] == '"run"', res_j["data"]))
    assert len(run) == 1
    assert run[0]["results"][0][0] > 3500


