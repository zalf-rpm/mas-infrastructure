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
import json
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

from pkgs.common import common
from pkgs.model import monica_io3
from pkgs.common import fbp

PATH_TO_CAPNP_SCHEMAS = PATH_TO_REPO / "capnproto_schemas"
abs_imports = [str(PATH_TO_CAPNP_SCHEMAS)]
common_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "common.capnp"), imports=abs_imports)
fbp_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "fbp.capnp"), imports=abs_imports)

config = {
    "in_sr": None,  # string (json)
    "path_to_out_dir": "out/",
    "file_pattern": "csv_{id}.csv",
    "from_attr": None,
    "id_attr": "id",
    "out_path_attr": "out_path",
}
common.update_config(config, sys.argv, print_config=True, allow_new_keys=False)
ports, close_out_ports = fbp.connect_ports(config)

count = 0
while ports["in"]:
    try:
        msg = ports["in"].read().wait()
        if msg.which() == "done":
            ports["in"] = None
            continue

        in_ip = msg.value.as_struct(fbp_capnp.IP)
        id_attr = common.get_fbp_attr(in_ip, config["id_attr"])
        id = id_attr.as_text() if id_attr else str(count)
        out_path_attr = common.get_fbp_attr(in_ip, config["out_path_attr"])
        out_path = out_path_attr.as_text() if out_path_attr else config["path_to_out_dir"]

        dir = out_path
        if os.path.isdir(dir) and os.path.exists(dir):
            pass
        else:
            try:
                os.makedirs(dir)
            except OSError:
                print("c: Couldn't create dir:", dir, "! Exiting.")
                exit(1)

        filepath = dir + "/" + config["file_pattern"].format(id=id)
        with open(filepath, "wt") as _:
            writer = csv.writer(_, delimiter=",")

            content_attr = common.get_fbp_attr(in_ip, config["from_attr"])
            jstr = content_attr.as_text() if content_attr else in_ip.content.as_text()
            j = json.loads(jstr)

            for data_ in j.get("data", []):
                results = data_.get("results", [])
                orig_spec = data_.get("origSpec", "")
                output_ids = data_.get("outputIds", [])

                if len(results) > 0:
                    writer.writerow([orig_spec.replace("\"", "")])
                    for row in monica_io3.write_output_header_rows(output_ids,
                                                                   include_header_row=True,
                                                                   include_units_row=True,
                                                                   include_time_agg=False):
                        writer.writerow(row)

                    for row in monica_io3.write_output(output_ids, results):
                        writer.writerow(row)

                writer.writerow([])

            count += 1
        print(f"{os.path.basename(__file__)} wrote:", filepath)

    except Exception as e:
        print(f"{os.path.basename(__file__)} Exception:", e)

close_out_ports()
print(f"{os.path.basename(__file__)}: process finished")