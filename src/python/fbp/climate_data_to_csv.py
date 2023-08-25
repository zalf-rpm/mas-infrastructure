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

from pkgs.common import common

PATH_TO_CAPNP_SCHEMAS = PATH_TO_REPO / "capnproto_schemas"
abs_imports = [str(PATH_TO_CAPNP_SCHEMAS)]
common_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "common.capnp"), imports=abs_imports) 
geo_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "geo.capnp"), imports=abs_imports)
climate_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "climate.capnp"), imports=abs_imports)
fbp_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "fbp.capnp"), imports=abs_imports)


config = {
    "to_attr": None, #"latlon",
    "from_attr": None, 
    "in_sr": None, # string (sturdyref) | climate_capnp.TimeSeries (capability) | climate_capnp.TimeSeriesData (data)
    "out_sr": None # string (csv)
}
common.update_config(config, sys.argv, print_config=True, allow_new_keys=False)

conman = common.ConnectionManager()
inp = conman.try_connect(config["in_sr"], cast_as=fbp_capnp.Channel.Reader, retry_secs=1)
outp = conman.try_connect(config["out_sr"], cast_as=fbp_capnp.Channel.Writer, retry_secs=1)

def data_to_csv(header : list, data : list[list[float]], start_date : date):
    csv = io.StringIO()
    h_str = ",".join([str(h) for h in header])
    csv.write(h_str + "\n")
    for i, line in enumerate(data):
        current_date = start_date + timedelta(days=i)
        d_str = ",".join([str(d) for d in line])
        csv.write(current_date.strftime("%Y-%m-%d") + "," + d_str + "\n")
    return csv

try:
    if inp and outp:
        while True:
            msg = inp.read().wait()
            # check for end of data from in port
            if msg.which() == "done":
                break
            
            in_ip = msg.value.as_struct(fbp_capnp.IP)
            attr = common.get_fbp_attr(in_ip, config["from_attr"])
            if attr:
                data = attr.as_struct(climate_capnp.TimeSeriesData)
            else:
                data = in_ip.content.as_struct(climate_capnp.TimeSeriesData)

            csv = data_to_csv(data.header, data.data, data.startDate)
            
            out_ip = fbp_capnp.IP.new_message()
            if not config["to_attr"]:
                out_ip.content = csv
            common.copy_and_set_fbp_attrs(in_ip, out_ip, **({config["to_attr"]: csv} if config["to_attr"] else {}))
            outp.write(value=out_ip).wait()

        # close out port
        outp.write(done=None).wait()

except Exception as e:
    print("climate_data_to_csv.py ex:", e)

print("climate_data_to_csv.py: exiting run")

