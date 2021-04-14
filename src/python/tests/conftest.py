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

import asyncio
import json
from multiprocessing import Process
import os
from pathlib import Path
import pytest
from subprocess import Popen
import sys
import time

PATH_TO_REPO = Path(os.path.realpath(__file__)).parent.parent.parent.parent
if str(PATH_TO_REPO) not in sys.path:
    sys.path.insert(1, str(PATH_TO_REPO))

PATH_TO_PYTHON_CODE = PATH_TO_REPO / "src/python"
if str(PATH_TO_PYTHON_CODE) not in sys.path:
    sys.path.insert(1, str(PATH_TO_PYTHON_CODE))

TIME_SERIES_SERVICE_PORT = 6001
MONICA_SERVICE_PROXY_PORT = 6002
SOIL_SERVICE_PORT = 6003
PATH_TO_MONICA_REPO = PATH_TO_REPO.parent / "monica" 
PATH_TO_MONICA_CAPNP_PROXY_EXECUTABLE = None #PATH_TO_MONICA_REPO + "_cmake_win64/Release/"
PATH_TO_MONICA_IO3 = PATH_TO_MONICA_REPO / "src/python"

if str(PATH_TO_MONICA_IO3) not in sys.path:
    sys.path.insert(1, str(PATH_TO_MONICA_IO3))
import monica_io3

import services.climate.csv_time_series_service as ts_service
import services.soil.sqlite_soil_data_service as soil_service
import common.capnp_async_helpers as async_helpers

import capnp
from capnproto_schemas import model_capnp, climate_data_capnp, soil_data_capnp

#------------------------------------------------------------------------------

@pytest.fixture(scope="session")
def start_monica_capnp_proxy():
    p = Popen(["monica-capnp-proxy", "-p", str(MONICA_SERVICE_PROXY_PORT), "-t", "3"], cwd=PATH_TO_MONICA_CAPNP_PROXY_EXECUTABLE)
    time.sleep(0.1)
    yield
    p.terminate()


@pytest.fixture(scope="session")
def monica_cap(start_monica_capnp_proxy):
    monica = capnp.TwoPartyClient("localhost:" + str(MONICA_SERVICE_PROXY_PORT)).bootstrap().cast_as(model_capnp.EnvInstance)
    return monica

#------------------------------------------------------------------------------

@pytest.fixture(scope="session")
def start_time_series_service():
    p = Process(target = ts_service.main, kwargs={
        "port": TIME_SERIES_SERVICE_PORT,
        "path_to_csv_file": str(PATH_TO_REPO / "data/climate/climate-iso.csv")
    })
    p.start()
    time.sleep(0.1)
    yield
    p.terminate()


@pytest.fixture(scope="session")
def time_series_cap(start_time_series_service):
    csv_time_series = capnp.TwoPartyClient("localhost:" + str(TIME_SERIES_SERVICE_PORT)).bootstrap().cast_as(climate_data_capnp.TimeSeries)
    return csv_time_series

#------------------------------------------------------------------------------

@pytest.fixture(scope="session")
def monica_env():
    with open(PATH_TO_REPO / "data/monica/sim-min.json") as _:
        sim_json = json.load(_)

    with open(PATH_TO_REPO / "data/monica/site-min.json") as _:
        site_json = json.load(_)

    with open(PATH_TO_REPO / "data/monica/crop-min.json") as _:
        crop_json = json.load(_)

    env = monica_io3.create_env_json_from_json_config({
        "crop": crop_json,
        "site": site_json,
        "sim": sim_json,
        "climate": "" #climate_csv
    })

    return env

#------------------------------------------------------------------------------

@pytest.fixture(scope="session")
def start_soil_service():
    p = Process(target = soil_service.main, kwargs={
        "port": SOIL_SERVICE_PORT,
        "path_to_sqlite_db": str(PATH_TO_REPO / "data/soil/buek1000.sqlite"),
        "path_to_ascii_soil_grid": str(PATH_TO_REPO / "data/soil/buek1000_1000_gk5.asc"),
        "grid_crs": "gk5",
        "id": "buek1000_germany",
        "name": "BÜK1000 - Germany"
    })
    p.start()
    time.sleep(10)
    yield
    p.terminate()


@pytest.fixture(scope="session")
def soil_service_cap(start_soil_service):
    soil_service = capnp.TwoPartyClient("localhost:" + str(SOIL_SERVICE_PORT)).bootstrap().cast_as(soil_data_capnp.Service)
    return soil_service

#------------------------------------------------------------------------------

@pytest.fixture(scope="session")
def start_soil_service_async():
    p = Process(target = soil_service.no_async_main, kwargs={
        "port": SOIL_SERVICE_PORT,
        "path_to_sqlite_db": str(PATH_TO_REPO / "data/soil/buek1000.sqlite"),
        "path_to_ascii_soil_grid": str(PATH_TO_REPO / "data/soil/buek1000_1000_gk5.asc"),
        "grid_crs": "gk5",
        "id": "buek1000_germany",
        "name": "BÜK1000 - Germany"
    })
    p.start()
    time.sleep(0.1)
    yield
    p.terminate()


@pytest.fixture(scope="session")
def soil_service_cap_async(start_soil_service_async):
    soil_service = capnp.TwoPartyClient("localhost:" + str(SOIL_SERVICE_PORT)).bootstrap().cast_as(soil_data_capnp.Service)
    #client = await async_helpers.connect_to_server(SOIL_SERVICE_PORT)
    #soil_service = client.bootstrap().cast_as(soil_data_capnp.Service)
    return soil_service
