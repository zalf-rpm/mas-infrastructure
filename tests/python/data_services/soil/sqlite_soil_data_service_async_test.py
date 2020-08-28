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

import asyncio
from datetime import date, timedelta
from multiprocessing import Process
from pathlib import Path
import pytest
import sys
import os
import time


PATH_TO_REPO = Path(os.path.realpath(__file__)).parent.parent.parent.parent.parent

import capnp

capnp.add_import_hook(additional_paths=["capnproto_schemas"])
import soil_data_capnp

if str(PATH_TO_REPO) not in sys.path:
    sys.path.insert(1, str(PATH_TO_REPO))

import data_services.soil.python.sqlite_soil_data_service as soil_service


@pytest.mark.asyncio
async def test_getAllAvailableParameters(soil_service_cap_async):
    res = await soil_service_cap_async.getAllAvailableParameters(onlyRawData=True).a_wait()
    assert len(res.mandatory) == 10
    assert len(res.optional) == 10
    assert list(map(str, res.mandatory))[:4] == ["organicCarbon", "bulkDensity", "rawDensity", "pH"]


def test_getAllAvailableParameters_derived(soil_service_cap_async):
    assert True
    #params = soil_service_cap_async.getAllAvailableParameters().wait()
    #assert len(params) == 16
    #assert list(map(lambda p: p.which(), params))[:4] == ["size", "ka5SoilType", "sand", "clay"]


@pytest.mark.asyncio
async def test_checkAvailableParameters(soil_service_cap_async):
    res = await soil_service_cap_async.checkAvailableParameters(
        mandatory=["soilType", "bulkDensity", "organicCarbon"],
        optional=["pH"]
    ).a_wait()
    assert res.failed is False
    assert len(res.mandatory) == 3
    assert list(map(str, res.mandatory)) == ["soilType", "bulkDensity", "organicCarbon"]
    assert len(res.optional) == 1
    assert list(map(str, res.optional)) == ["pH"]

    res = await soil_service_cap_async.checkAvailableParameters(
        mandatory=["organicMatter", "clay", "pH"],
        optional=["soilWaterConductivityCoefficient"]
    ).a_wait()
    assert res.failed is True
    assert len(res.mandatory) == 1
    assert list(map(str, res.mandatory)) == ["pH"]
    assert len(res.optional) == 1


@pytest.mark.asyncio
async def test_profilesAt(soil_service_cap_async):
    profiles = (await soil_service_cap_async.profilesAt(
        coord={"lat": 53.0, "lon": 12.5},
        query={
            "mandatory": ["sand", "clay", "bulkDensity", "organicCarbon"],
            "optional": ["pH"],
            "onlyRawData": False
        }
    ).a_wait()).profiles
    assert len(profiles) > 0, "The test should return at least one profile."
    p = profiles[0]  # at the moment there is always just one profile being returned

    assert len(p.layers) > 0, "There should be be at least one layer in a profile."
    assert int(p.percentageOfArea) == 100, "There is just one profile, so 100% of area should be covered by that one."

    l0 = p.layers[0]
    assert len(l0.properties) == 5, "There should be the 5 requested parameters."
    assert list(map(lambda p: str(p.name), l0.properties)) == ["sand", "clay", "bulkDensity", "organicCarbon", "pH"]

    # res = soil_service_cap.profilesAt(
    #    mandatory=[{"organicMatter": 0}, {"clay": 0}, {"pH": 0}],
    #    optional=[{"soilWaterConductivityCoefficient": 0}]
    # ).wait()
    # assert res.failed == True
    # assert len(res.mandatory) == 2
    # assert list(map(lambda p: p.which(), res.mandatory)) == ["clay", "pH"]
    # assert len(res.optional) == 0


@pytest.mark.asyncio
async def test_allLocations(soil_service_cap_async):
    profiles = (await soil_service_cap_async.allLocations(
        mandatory=[{"sand": 0}, {"clay": 0}, {"bulkDensity": 0}, {"organicCarbon": 0}],
        optional=[{"pH": 0}],
        onlyRawData=False
    ).a_wait()).profiles
    assert len(profiles) > 1, "For BÜK1000 there should be 71 profiles in the database."
    latlon_and_cap = profiles[0]  # at the moment there is always just one profile being returned
    p = latlon_and_cap.snd[0].cap().wait().object
    print(p)

    assert len(p.layers) > 0, "There should be be at least one layer in a profile."
    assert int(p.percentageOfArea) == 100, "There is just one profile, so 100% of area should be covered by that one."

    l0 = p.layers[0]
    assert len(l0.params) == 6, "There should be 6 parameters, including automatically added 'size' in every layer."
    assert list(map(lambda p: p.which(), l0.params)) == ["sand", "clay", "bulkDensity", "organicCarbon", "pH", "size"]

"""
def test_header(time_series_cap):
    header = time_series_cap.header().wait().header
    assert len(header) == 7
    assert list(header) == ["tavg", "tmin", "tmax", "wind", "globrad", "precip", "relhumid"]

def test_subheader(time_series_cap):
    ts = time_series_cap.subheader(["tavg", "precip", "wind"]).wait().timeSeries
    header = ts.header().wait().header
    assert len(header) == 3
    assert list(header) == ["tavg", "precip", "wind"]

def test_range(time_series_cap):
    r = time_series_cap.range().wait()
    assert ts_service.create_date(r.startDate) == date(1991, 1, 1)
    assert ts_service.create_date(r.endDate) == date(1997, 12, 31)

def test_subrange(time_series_cap):
    ts = time_series_cap.subrange({"year": 1992, "month": 2, "day": 3}, {"year": 1993, "month": 11, "day": 25}).wait().timeSeries
    r = ts.range().wait()
    assert ts_service.create_date(r.startDate) == date(1992, 2, 3)
    assert ts_service.create_date(r.endDate) == date(1993, 11, 25)

    ts2 = time_series_cap.subrange({"year": 1991, "month": 2, "day": 1}, {"year": 1991, "month": 2, "day": 20}).wait().timeSeries
    data2 = ts2.data().wait().data
    assert len(data2) == 20
    
    data0 = time_series_cap.data().wait().data
    assert list(data0[31]) == list(data2[0])
   
def test_resolution(time_series_cap):
    r = time_series_cap.resolution().wait().resolution
    assert str(r) == "daily"

#def test_location(time_series_cap):
#    l = time_series_cap.location().wait()
#    assert l.timeSeries == time_series_cap

def test_data(time_series_cap):
    r = time_series_cap.range().wait()
    timedelta = ts_service.create_date(r.endDate) - ts_service.create_date(r.startDate)
    d = time_series_cap.data().wait().data
    assert len(d) == timedelta.days + 1

def test_dataT(time_series_cap):
    d = time_series_cap.data().wait().data
    dt = time_series_cap.dataT().wait().data
    d0 = list(d[0])
    dt0 = list(dt[i][0] for i in range(7))
    assert d0 == dt0

    dx = list(d[-1])
    dtx = list(dt[i][-1] for i in range(7))
    assert dx == dtx
"""


