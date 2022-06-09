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
import csv
import os
from pathlib import Path
import sys

PATH_TO_SCRIPT_DIR = Path(os.path.realpath(__file__)).parent
PATH_TO_REPO = PATH_TO_SCRIPT_DIR.parent.parent.parent
if str(PATH_TO_REPO) not in sys.path:
    sys.path.insert(1, str(PATH_TO_REPO))

PATH_TO_PYTHON_CODE = PATH_TO_REPO / "src/python"
if str(PATH_TO_PYTHON_CODE) not in sys.path:
    sys.path.insert(1, str(PATH_TO_PYTHON_CODE))

import common.common as common

PATH_TO_CAPNP_SCHEMAS = PATH_TO_REPO / "capnproto_schemas"
abs_imports = [str(PATH_TO_CAPNP_SCHEMAS)]
common_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "common.capnp"), imports=abs_imports) 

#------------------------------------------------------------------------------

config = {
    "out_sr": None,
    "id_col": "id",
    "send_ids": None, #None, #1,2,3 -> None = all
    "file": "sim_setups_bgr_flow.csv",
    "path_to_capnp_struct": "bgr.capnp:Setup", #"bla.capnp:MyType",
    "to_attr": None,
}
common.update_config(config, sys.argv, print_config=True, allow_new_keys=False)

conman = common.ConnectionManager()
outp = conman.try_connect(config["out_sr"], cast_as=common_capnp.Channel.Writer, retry_secs=1)

struct_type = common.load_capnp_module(config["path_to_capnp_struct"])
struct_fieldnames = struct_type.schema.fieldnames
struct_fields = struct_type.schema.fields
id_col = config["id_col"]
send_ids = config["send_ids"].split(",") if config["send_ids"] is not None else None

try:
    if outp:
        with open(config["file"]) as _:
            key_to_data = {}
            # determine seperator char
            dialect = csv.Sniffer().sniff(_.read(), delimiters=';,\t')
            _.seek(0)
            # read csv with seperator char
            reader = csv.reader(_, dialect)
            header_cols = next(reader)
            for row in reader:
                val = struct_type.new_message()
                for i, header_col in enumerate(header_cols):
                    if header_col not in struct_fieldnames:
                        continue
                    value = row[i]
                    fld_type = struct_fields[header_col].proto.slot.type.which()
                    if fld_type == "bool":
                        val.__setattr__(header_col, value.lower() == "true")
                    elif fld_type == "text":
                        val.__setattr__(header_col, value)
                    elif fld_type in ["float32", "float64"]:
                        val.__setattr__(header_col, float(value))
                    elif fld_type in ["int8", "int16", "int32", "int64", "uint8", "uint16", "uint32", "uint64"]:
                        val.__setattr__(header_col, int(value))
                    elif fld_type == "enum":
                        if value in struct_fields[header_col].schema.enumerants:
                            val.__setattr__(header_col, value)

                if send_ids is None or (id_col in struct_fieldnames and str(val.__getattr__(id_col)) in send_ids):
                    out_ip = common_capnp.IP.new_message()
                    if config["to_attr"]:
                        out_ip.attributes=[{"key": config["to_attr"], "value": val}]
                    else:
                        out_ip.content = val
                    outp.write(value=out_ip).wait()

    outp.write(done=None).wait()

except Exception as e:
    print("read_csv.py ex:", e)

print("read_csv.py: exiting run")


