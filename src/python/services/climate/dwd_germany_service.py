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

import asyncio
import capnp
from datetime import date, timedelta
import io
import json
import os
from pathlib import Path
import string
import sys
import time
import uuid

#remote debugging via commandline
#-m ptvsd --host 0.0.0.0 --port 14000 --wait

PATH_TO_REPO = Path(os.path.realpath(__file__)).parent.parent.parent.parent.parent
if str(PATH_TO_REPO) not in sys.path:
    sys.path.insert(1, str(PATH_TO_REPO))

PATH_TO_PYTHON_CODE = PATH_TO_REPO / "src/python"
if str(PATH_TO_PYTHON_CODE) not in sys.path:
    sys.path.insert(1, str(PATH_TO_PYTHON_CODE))

import common.capnp_async_helpers as async_helpers
import common_climate_data_capnp_impl as ccdi
import common.common as common
import common.service as serv
import csv_file_based as csv_based
import common.geo as geo

PATH_TO_CAPNP_SCHEMAS = PATH_TO_REPO / "capnproto_schemas"
abs_imports = [str(PATH_TO_CAPNP_SCHEMAS)]
reg_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "registry.capnp"), imports=abs_imports)
climate_data_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "climate_data.capnp"), imports=abs_imports)
common_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "common.capnp"), imports=abs_imports)
geo_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "geo_coord.capnp"), imports=abs_imports)

#------------------------------------------------------------------------------

def data_to_csv(header : list, data : list[list[float]], start_date : date):
    csv = io.StringIO()
    h_str = ",".join([str(h) for h in header])
    csv.write(h_str + "\n")
    for i, line in enumerate(data):
        current_date = start_date + timedelta(days=i)
        d_str = ",".join([str(d) for d in line])
        csv.write(current_date.strftime("%Y-%m-%d") + "," + d_str + "\n")
    return csv


def fbp(in_coord_sr, out_csv_sr, service : ccdi.Service):
    conman = common.ConnectionManager()
    coordp = conman.try_connect(in_coord_sr, cast_as=common_capnp.Channel.Reader, retry_secs=1)
    csvp = conman.try_connect(out_csv_sr, cast_as=common_capnp.Channel.Writer, retry_secs=1)

    try:
        if coordp and csvp and service:
            dataset : csv_based.Dataset = service.getAvailableDatasets()[0].data
            while True:
                in_msg = coordp.read().wait()
                if in_msg.which() == "done":
                    break
                coord = in_msg.value.as_struct(geo_capnp.LatLonCoord)
                timeseries : csv_based.TimeSeries = dataset.closestTimeSeriesAt(coord).timeSeries
                header : list = timeseries.header().wait().header
                sd = timeseries.range().wait().startDate
                data = timeseries.data().wait().data
                csv = data_to_csv(header, data, date(sd.year, sd.month, sd.day))
                csvp.write(value=csv.getvalue()).wait()
            
            csvp.write(done=None).wait()

    except Exception as e:
        print("dwd_germany_service.py ex:", e)

    print("dwd_germany_service.py: exiting FBP component")


def no_fbp(service : ccdi.Service):

    dataset : csv_based.Dataset = service.getAvailableDatasets()[0].data
    count = 0

    # read file
    with open("/home/berg/Desktop/Koordinaten_HE_dummy_ID.csv") as _:
        skip_lines = 1
        for line in _.readlines():
            if skip_lines > 0:
                skip_lines -= 1
                continue

            line = line.rstrip()
            vals = line.split(",")
            vals = list(map(lambda v: float(v), vals))

            to_instance = geo.name_to_struct_instance("utm32n")
            utm_coord = to_instance.copy()
            geo.set_xy(utm_coord, vals[0], vals[1])
            ll_coord = geo.transform_from_to_geo_coord(utm_coord, "latlon")

            timeseries : csv_based.TimeSeries = dataset.closestTimeSeriesAt(ll_coord).timeSeries
            header : list = timeseries.header().wait().header
            sd = timeseries.range().wait().startDate
            data = timeseries.data().wait().data
            csv = data_to_csv(header, data, date(sd.year, sd.month, sd.day))
            filepath = "out_no_fbp/csv_{id}.csv".format(id=count)
            with open(filepath, "wt") as _:
                _.write(csv.getvalue())
            count += 1
            print("wrote", filepath)

        print("no_fbp done")


#------------------------------------------------------------------------------

def create_meta_plus_datasets(path_to_data_dir, interpolator, rowcol_to_latlon, restorer):
    datasets = []
    metadata = climate_data_capnp.Metadata.new_message(
        entries = [
            {"historical": None},
            {"start": {"year": 1990, "month": 1, "day": 1}},
            {"end": {"year": 2019, "month": 12, "day": 31}}
        ]
    )
    metadata.info = ccdi.Metadata_Info(metadata)
    datasets.append(climate_data_capnp.MetaPlusData.new_message(
        meta=metadata, 
        data=csv_based.Dataset(metadata, path_to_data_dir, interpolator, rowcol_to_latlon, 
            header_map={"windspeed": "wind"},
            row_col_pattern="row-{row}/col-{col}.csv",
            name="DWD Germany 1991-2019",
            description="ZALF DWD Germany data from 1991-2019 in MONICA CSV format.",
            restorer=restorer)
    ))
    return datasets

#------------------------------------------------------------------------------

async def main(path_to_data, serve_bootstrap=True, host=None, port=None, 
    id=None, name="DWD - historical - 1991-2019", description=None, use_async=False):

    config = {
        "path_to_data": path_to_data,
        "port": port, 
        "host": host,
        "id": id,
        "name": name,
        "description": description,
        "serve_bootstrap": serve_bootstrap,
        "in_coord_sr": None,
        "out_csv_sr": None,
        "fbp": False,
        "no_fbp": False,
        "use_async": use_async
    }
    common.update_config(config, sys.argv, print_config=True, allow_new_keys=False)

    restorer = common.Restorer()
    interpolator, rowcol_to_latlon = ccdi.create_lat_lon_interpolator_from_json_coords_file(config["path_to_data"] + "/" + "latlon-to-rowcol.json")
    meta_plus_data = create_meta_plus_datasets(config["path_to_data"] + "/germany", interpolator, rowcol_to_latlon, restorer)
    service = ccdi.Service(meta_plus_data, id=config["id"], name=config["name"], description=config["description"], restorer=restorer)
    if config["fbp"]:
        fbp(config["in_coord_sr"], config["out_csv_sr"], service)
    if config["no_fbp"]:
        no_fbp(service)
    else:
        if config["use_async"]:
            await serv.async_init_and_run_service({"service": service}, config["host"], config["port"], 
            serve_bootstrap=config["serve_bootstrap"], restorer=restorer)
        else:
            
            serv.init_and_run_service({"service": service}, config["host"], config["port"], 
                serve_bootstrap=config["serve_bootstrap"], restorer=restorer)

#------------------------------------------------------------------------------

if __name__ == '__main__':
    asyncio.run(main("/run/user/1000/gvfs/sftp:host=login01.cluster.zalf.de,user=rpm/beegfs/common/data/climate/dwd/csvs", 
        serve_bootstrap=True, use_async=True)) 
