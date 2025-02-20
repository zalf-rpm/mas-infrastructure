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

import capnp
from copy import deepcopy
from datetime import date, timedelta
import json
from multiprocessing import Process
from pathlib import Path
import os
import pytest
import random
from subprocess import Popen
import sys
import time

PATH_TO_REPO = Path(os.path.realpath(__file__)).parent.parent.parent.parent.parent
if str(PATH_TO_REPO) not in sys.path:
    sys.path.insert(1, str(PATH_TO_REPO))

PATH_TO_PYTHON_CODE = PATH_TO_REPO / "src/python"
if str(PATH_TO_PYTHON_CODE) not in sys.path:
    sys.path.insert(1, str(PATH_TO_PYTHON_CODE))

PATH_TO_CAPNP_SCHEMAS = PATH_TO_REPO / "capnproto_schemas"
abs_imports = [str(PATH_TO_CAPNP_SCHEMAS)]
common_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "common.capnp"), imports=abs_imports)
climate_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "climate.capnp"), imports=abs_imports)
soil_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "soil.capnp"), imports=abs_imports)
model_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "model.capnp"), imports=abs_imports)


# ------------------------------------------------------------------------------

def test_monica(monica_cap, monica_env, time_series_cap):
    env = deepcopy(monica_env)
    env["customId"] = random.randint(0, 1000)

    env_struct = common_capnp.StructuredText.new_message(value=json.dumps(env), structure={"json": None})
    res = monica_cap.run({"rest": env_struct, "timeSeries": time_series_cap}).wait().result.as_struct(
        common_capnp.StructuredText)
    assert len(res.value) > 0
    res_j = json.loads(res.value)
    assert res_j["customId"] == env["customId"]

    # in the minimal hohenfinow2 examples there's a "run" section with the summed precipitation of the whole run = slightly more than 3500mm/run
    run = list(filter(lambda r: r["origSpec"] == '"run"', res_j["data"]))
    assert len(run) == 1
    assert run[0]["results"][0][0] > 3500
