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
climate_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "climate_data.capnp"), imports=abs_imports)
common_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "common.capnp"), imports=abs_imports)
geo_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "geo_coord.capnp"), imports=abs_imports)

#------------------------------------------------------------------------------

def fbp(config : dict, service : ccdi.Service):
    conman = common.ConnectionManager()
    inp = conman.try_connect(config["in_sr"], cast_as=common_capnp.Channel.Reader, retry_secs=1)
    outp = conman.try_connect(config["out_sr"], cast_as=common_capnp.Channel.Writer, retry_secs=1)
    mode = config["mode"]

    def iso_to_cdate(iso_date_str):
        ds = iso_date_str.split("-")
        return {"year": int(ds[0]), "month": int(ds[1]), "day": int(ds[2])} 

    try:
        if inp and outp and service:
            dataset : csv_based.Dataset = service.getAvailableDatasets().wait().datasets[0].data
            while True:
                in_msg = inp.read().wait()
                if in_msg.which() == "done":
                    break

                in_ip = in_msg.value.as_struct(common_capnp.IP)
                attr = common.get_fbp_attr(in_ip, config["latlon_attr"])
                if attr:
                    coord = attr.as_struct(geo_capnp.LatLonCoord)
                else:
                    coord = in_ip.content.as_struct(geo_capnp.LatLonCoord)
                start_date = common.get_fbp_attr(in_ip, config["start_date_attr"]).as_text()
                end_date = common.get_fbp_attr(in_ip, config["end_date_attr"]).as_text()

                timeseries_p : csv_based.TimeSeries = dataset.closestTimeSeriesAt(coord).timeSeries
                timeseries = timeseries_p.subrange(iso_to_cdate(start_date), iso_to_cdate(end_date)).wait().timeSeries

                res = timeseries
                if mode == "sturdyref":
                    res = timeseries.save().wait()
                elif mode == "capability":
                    res = timeseries
                elif mode == "data":
                    res = climate_capnp.TimeSeriesData.new_message()
                    res.isTransposed = False
                    header = timeseries.header().header
                    se_date = timeseries.range()
                    resolution = timeseries.resolution().resolution
                    res.data = timeseries.data().wait().data
                    res.header = header
                    se_date = se_date
                    res.startDate = se_date.startDate
                    res.endDate = se_date.endDate
                    res.resolution = resolution

                #print(res.data().wait())
                out_ip = common_capnp.IP.new_message()
                if not config["to_attr"]:
                    out_ip.content = res
                common.copy_fbp_attr(in_ip, out_ip, config["to_attr"], res)
                outp.write(value=out_ip).wait()


            outp.write(done=None).wait()

    except Exception as e:
        print("dwd_germany_service.py ex:", e)

    print("dwd_germany_service.py: exiting FBP component")

#------------------------------------------------------------------------------

def create_meta_plus_datasets(path_to_data_dir, interpolator, rowcol_to_latlon, restorer):
    datasets = []
    metadata = climate_capnp.Metadata.new_message(
        entries = [
            {"historical": None},
            {"start": {"year": 1990, "month": 1, "day": 1}},
            {"end": {"year": 2019, "month": 12, "day": 31}}
        ]
    )
    metadata.info = ccdi.Metadata_Info(metadata)
    datasets.append(climate_capnp.MetaPlusData.new_message(
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
        "in_sr": None,
        "out_sr": None,
        "fbp": False,
        "no_fbp": False,
        "use_async": use_async,
        "to_attr": None, #"climate",
        "latlon_attr": "latlon",
        "start_date_attr": "startDate",
        "end_date_attr": "endDate",
        "mode": "sturdyref", # sturdyref | capability | data
    }
    common.update_config(config, sys.argv, print_config=True, allow_new_keys=False)

    restorer = common.Restorer()
    interpolator, rowcol_to_latlon = ccdi.create_lat_lon_interpolator_from_json_coords_file(config["path_to_data"] + "/" + "latlon-to-rowcol.json")
    meta_plus_data = create_meta_plus_datasets(config["path_to_data"] + "/germany", interpolator, rowcol_to_latlon, restorer)
    service = ccdi.Service(meta_plus_data, id=config["id"], name=config["name"], description=config["description"], restorer=restorer)
    if config["fbp"]:
        fbp(config, climate_capnp.Service._new_client(service))
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
