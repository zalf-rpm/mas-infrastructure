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
geo_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "geo_coord.capnp"), imports=abs_imports)
climate_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "climate_data.capnp"), imports=abs_imports)

#------------------------------------------------------------------------------

config = {
    "to_attr": None, #"latlon",
    "from_attr": None,
    "in_type": "sturdyref", # sturdyref | capability 
    "in_sr": None, # string (sturdyref) | climate_capnp.TimeSeries (capability)
    "out_sr": None # climate_capnp.TimeSeriesData (data)
}
common.update_config(config, sys.argv, print_config=True, allow_new_keys=False)

conman = common.ConnectionManager()
inp = conman.try_connect(config["in_sr"], cast_as=common_capnp.Channel.Reader, retry_secs=1)
outp = conman.try_connect(config["out_sr"], cast_as=common_capnp.Channel.Writer, retry_secs=1)

in_type = config["in_type"]

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
            tsd.isTransposed = False
            tsd.header = timeseries.header().wait().header
            se_date = timeseries.range().wait()
            tsd.startDate = se_date.startDate
            tsd.endDate = se_date.endDate
            tsd.resolution = timeseries.resolution().wait().resolution
            tsd.data = timeseries.data().wait().data

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
