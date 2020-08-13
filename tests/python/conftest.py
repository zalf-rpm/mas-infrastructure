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

import sys
import os
import time
import json
from multiprocessing import Process
from subprocess import Popen
from pathlib import Path
import pytest

top_level_path = Path(os.path.realpath(__file__)).parent.parent.parent
sys.path.append(str(top_level_path))

TIME_SERIES_SERVICE_PORT = 6001
MONICA_SERVICE_PROXY_PORT = 6002
PATH_TO_MONICA_REPO = top_level_path.parent / "monica" 
PATH_TO_MONICA_CAPNP_PROXY_EXECUTABLE = None #PATH_TO_MONICA_REPO + "_cmake_win64/Release/"

sys.path.append(str(PATH_TO_MONICA_REPO / "src/python"))
import monica_io3

from data_services.climate.python import csv_time_series_service as ts_service

import capnp
capnp.add_import_hook(additional_paths=["capnproto_schemas"])
import model_capnp
import climate_data_capnp
import soil_data_capnp


@pytest.fixture(scope="session")
def start_monica_capnp_proxy():
    p = Popen(["monica-capnp-proxy", "-p", str(MONICA_SERVICE_PROXY_PORT), "-t", "3"], cwd=PATH_TO_MONICA_CAPNP_PROXY_EXECUTABLE)
    time.sleep(0.1)
    yield
    p.terminate()


@pytest.fixture(scope="session")
def monica_cap(start_monica_capnp_proxy):
    monica = capnp.TwoPartyClient("localhost:" + str(MONICA_SERVICE_PROXY_PORT)).bootstrap().cast_as(model_capnp.Model.EnvInstance)
    return monica


@pytest.fixture(scope="session")
def start_time_series_service():
    p = Process(target = ts_service.main, kwargs={
        "port": TIME_SERIES_SERVICE_PORT,
        "path_to_csv_file": "data/climate/climate-iso.csv"
    })
    p.start()
    time.sleep(0.1)
    yield
    p.terminate()


@pytest.fixture(scope="session")
def time_series_cap(start_time_series_service):
    csv_time_series = capnp.TwoPartyClient("localhost:" + str(TIME_SERIES_SERVICE_PORT)).bootstrap().cast_as(climate_data_capnp.Climate.TimeSeries)
    return csv_time_series


@pytest.fixture(scope="session")
def monica_env():
    with open("data/monica/sim-min.json") as _:
        sim_json = json.load(_)

    with open("data/monica/site-min.json") as _:
        site_json = json.load(_)

    with open("data/monica/crop-min.json") as _:
        crop_json = json.load(_)

    env = monica_io3.create_env_json_from_json_config({
        "crop": crop_json,
        "site": site_json,
        "sim": sim_json,
        "climate": "" #climate_csv
    })

    return env



