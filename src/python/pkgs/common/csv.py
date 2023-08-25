#!/usr/bin/python
# -*- coding: UTF-8

# This Source Code Form is subject to the terms of the Mozilla Public
# License, v. 2.0. If a copy of the MPL was not distributed with this
# file, You can obtain one at http://mozilla.org/MPL/2.0/. */

# Authors:
# Susanne Schulz <susanne.schulz@zalf.de>
#
# Maintainers:
# Currently maintained by the authors.
#
# This file has been created at the Institute of
# Landscape Systems Analysis at the ZALF.
# Copyright (C: Leibniz Centre for Agricultural Landscape Research (ZALF)

import csv


def read_csv(path_to_setups_csv, key="id"):
    """read sim setup from csv file"""
    with open(path_to_setups_csv) as _:
        key_to_data = {}
        # determine seperator char
        dialect = csv.Sniffer().sniff(_.read(), delimiters=';,\t')
        _.seek(0)
        # read csv with seperator char
        reader = csv.reader(_, dialect)
        header_cols = next(reader)
        for row in reader:
            data = {}
            for i, header_col in enumerate(header_cols):
                value = row[i]
                if value.lower() in ["true", "false"]:
                    value = value.lower() == "true"
                if header_col == key:
                    value = int(value)
                data[header_col] = value
            key_to_data[int(data[key])] = data
        return key_to_data

