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
# This file is part of the util library used by models created at the Institute of
# Landscape Systems Analysis at the ZALF.
# Copyright (C: Leibniz Centre for Agricultural Landscape Research (ZALF)

import capnp
import io
import os
from pathlib import Path
from pyproj import CRS, Transformer
import sys
from datetime import timedelta, date

PATH_TO_SCRIPT_DIR = Path(os.path.realpath(__file__)).parent
PATH_TO_REPO = PATH_TO_SCRIPT_DIR.parent.parent.parent
if str(PATH_TO_REPO) not in sys.path:
    sys.path.insert(1, str(PATH_TO_REPO))

PATH_TO_PYTHON_CODE = PATH_TO_REPO / "src/python"
if str(PATH_TO_PYTHON_CODE) not in sys.path:
    sys.path.insert(1, str(PATH_TO_PYTHON_CODE))

import common.common as common
import common.geo as geo

PATH_TO_CAPNP_SCHEMAS = PATH_TO_REPO / "capnproto_schemas"
abs_imports = [str(PATH_TO_CAPNP_SCHEMAS)]
common_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "common.capnp"), imports=abs_imports) 
geo_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "geo.capnp"), imports=abs_imports)
climate_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "climate.capnp"), imports=abs_imports)

#------------------------------------------------------------------------------

config = {
    "in_sr": None, # string (sturdyref) | climate_capnp.TimeSeries (capability)
    "in_type": "sturdyref", # sturdyref | capability 

    "out_sr": None, # climate_capnp.TimeSeriesData (data)

    "to_attr": None, #"latlon",
    "from_attr": None,

    "subrange_from": None, # iso-date string
    "subrange_to": None, # iso-date string
    "subheader": None, # precip,globrad,tavg   ... etc
    "transposed": "false",
}
common.update_config(config, sys.argv, print_config=True, allow_new_keys=False)

conman = common.ConnectionManager()
inp = conman.try_connect(config["in_sr"], cast_as=common_capnp.Channel.Reader, retry_secs=1)
outp = conman.try_connect(config["out_sr"], cast_as=common_capnp.Channel.Writer, retry_secs=1)

in_type = config["in_type"]

def create_capnp_date(isodate):
    py_date = date.fromisoformat(isodate)
    return {"year": py_date.year, "month": py_date.month, "day": py_date.day}

try:
    if inp and outp:
        while True:
            msg = inp.read().wait()
            # check for end of data from in port
            if msg.which() == "done":
                break
            
            in_ip = msg.value.as_struct(common_capnp.IP)
            attr = common.get_fbp_attr(in_ip, config["from_attr"])
            obj = attr if attr else in_ip.content
            if in_type == "capability":
                timeseries = obj.as_interface(climate_capnp.TimeSeries)
            elif in_type == "sturdyref":
                try:
                    timeseries = conman.try_connect(obj.as_text(), cast_as=climate_capnp.TimeSeries, retry_secs=1)
                except Exception as e:
                    print("Error: Couldn't connect to sturdyref:", obj.as_text())
                    continue

            tsd = climate_capnp.TimeSeriesData.new_message()
            tsd.isTransposed = config["transposed"] == "true"
            
            if config["subheader"]:
                subheader = config["subheader"].split(",")
                timeseries = timeseries.subheader(subheader).timeSeries
            header = timeseries.header().wait().header
            
            if config["subrange_from"] or config["subrange_to"]:
                sr_req = timeseries.subrange_request()
                timeseries = timeseries.subrange(create_capnp_date(config["subrange_from"]), create_capnp_date(config["subrange_to"])).timeSeries
                #if config["subrange_from"]:
                #    setattr(sr_req, "from", create_capnp_date(config["subrange_from"]))
                #if config["subrange_to"]:
                #    setattr(sr_req, "to", create_capnp_date(config["subrange_to"]))
                #timeseries = sr_req.send().timeSeries

            resolution_prom = timeseries.resolution()
            se_date_prom = timeseries.range()
            header_size = len(header)
            ds = timeseries.dataT().wait().data if tsd.isTransposed else timeseries.data().wait().data
            tsd.init("data", len(ds))
            for i in range(len(ds)):
                l = tsd.data.init(i, header_size)
                for j in range(header_size):
                    l[j] = ds[i][j]
            se_date = se_date_prom.wait()
            tsd.startDate = se_date.startDate
            tsd.endDate = se_date.endDate
            tsd.resolution = resolution_prom.wait().resolution
            h = tsd.init("header", len(header))
            for i in range(len(header)):
                h[i] = header[i]

            out_ip = common_capnp.IP.new_message()
            if not config["to_attr"]:
                out_ip.content = tsd
            common.copy_fbp_attr(in_ip, out_ip, config["to_attr"], tsd)
            outp.write(value=out_ip).wait()

        # close out port
        outp.write(done=None).wait()

except Exception as e:
    print("timeseries_to_data.py ex:", e)

print("timeseries_to_data.py: exiting run")

