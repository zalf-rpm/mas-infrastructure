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

from collections import defaultdict
from datetime import date, timedelta
import os
import pandas as pd
from pathlib import Path
import sys

PATH_TO_REPO = Path(os.path.realpath(__file__)).parent.parent.parent.parent
if str(PATH_TO_REPO) not in sys.path:
    sys.path.insert(1, str(PATH_TO_REPO))

PATH_TO_PYTHON_CODE = PATH_TO_REPO / "src/python"
if str(PATH_TO_PYTHON_CODE) not in sys.path:
    sys.path.insert(1, str(PATH_TO_PYTHON_CODE))

import common.common as common

config = {
    "path_to_rows_dir": "/beegfs/common/data/climate/dwd/csvs/germany",
    #"path_to_rows_dir": "/run/user/1000/gvfs/sftp:host=login01.cluster.zalf.de,user=rpm/beegfs/common/data/climate/dwd/csvs/germany/",
    "path_to_out_dir": "/scratch/aggregate_monthly_traditional/dwd_2000-2019_out/",
    #"path_to_out_dir": "/home/berg/GitHub/mas-infrastructure/src/python/fbp/out/",
    "subrange_start": "2000-01-01",
    "subrange_end": "2019-12-31",
    "subheader": "tavg,globrad,precip",
    "filepath_pattern": "csv_{id}.csv",
}
common.update_config(config, sys.argv, print_config=True, allow_new_keys=False)

def aggregate_monthly(header : list, data : list[list[float]], start_date : date):
    grouped_data = defaultdict(lambda: defaultdict(lambda: defaultdict(list))) # var -> year -> month -> values 
    for i, line in enumerate(data):
        current_date = start_date + timedelta(days=i)
        for j, v in enumerate(line):
            grouped_data[header[j]][current_date.year][current_date.month].append(v)

    def aggregate_values(var, values):
        if var == "precip": # sum precipition
            return sum(values)
        else:               # average all other values 
            return sum(values)/len(values)

    vars = {}
    for var, rest1 in grouped_data.items():
        agg_values = []
        for year, rest2 in rest1.items():
            for month, values in rest2.items():
                agg_values.append(int(round(aggregate_values(var, values), 1)*10))
        vars[var] = ",".join([str(d) for d in agg_values])
    return vars

sy, sm, sd = list(map(int, config["subrange_start"].split("-")))
ey, em, ed = list(map(int, config["subrange_end"].split("-")))
start_date = date(sy, sm, sd)
end_date = date(ey, em, ed)

if not os.path.exists(config["path_to_out_dir"]):
    os.makedirs(config["path_to_out_dir"], exist_ok=True)

rows = os.listdir(config["path_to_rows_dir"])
rows.sort(key=lambda r: int(r[4:]))
for row in rows:
    print("@", row)
    path_to_row = os.path.join(config["path_to_rows_dir"], row)
    if os.path.isdir(path_to_row):
        cols = os.listdir(path_to_row)
        cols.sort(key=lambda c: int(c[4:][:-4]))
        for col in cols:
            if col.endswith(".csv"):
                path_to_csv = os.path.join(path_to_row, col)
                
                df = pd.read_csv(path_to_csv, skiprows=[1], index_col=0, sep=",")

                df = df.loc[str(start_date):str(end_date)]

                sub_headers = list(config["subheader"].split(","))
                df = df.loc[:, sub_headers]

                data = df.to_numpy().tolist()

                vars = aggregate_monthly(sub_headers, data, start_date)

                for var, content in vars.items():
                    filepath = os.path.join(config["path_to_out_dir"], config["filepath_pattern"].format(id=var))
                    if not os.path.exists(filepath):
                        with open(filepath, "wt") as _:
                            header = ["cell_id"]
                            for year in range(start_date.year, end_date.year+1):
                                for month in range(1, 12+1):
                                    header.append(f"{var}_{year}{month:02d}")
                            header_str = ",".join(header) + "\n"
                            _.write(header_str)    
                        
                    with open(filepath, "at") as _:
                        _.write(row[4:].zfill(3) + str(col[4:][:-4]).zfill(3) + "," + content + "\n")
                    
                    #print("wrote row:", row, "col:", col, "var:", var, path_to_csv)
                

