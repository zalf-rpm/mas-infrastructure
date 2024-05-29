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

# remote debugging via commandline
# -m ptvsd --host 0.0.0.0 --port 14000 --wait

import capnp
from collections import defaultdict
import csv
from datetime import date, timedelta
import numpy as np
import os
from pathlib import Path
from pyproj import CRS, Transformer
from scipy.interpolate import NearestNDInterpolator
import sys

PATH_TO_REPO = Path(os.path.realpath(__file__)).parent.parent.parent.parent.parent
if str(PATH_TO_REPO) not in sys.path:
    sys.path.insert(1, str(PATH_TO_REPO))

PATH_TO_PYTHON_CODE = PATH_TO_REPO / "src/python"
if str(PATH_TO_PYTHON_CODE) not in sys.path:
    sys.path.insert(1, str(PATH_TO_PYTHON_CODE))

from pkgs.common import common
from pkgs.common import geo

PATH_TO_CAPNP_SCHEMAS = PATH_TO_REPO / "capnproto_schemas"
abs_imports = [str(PATH_TO_CAPNP_SCHEMAS)]
common_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "common.capnp"), imports=abs_imports)
fbp_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "fbp.capnp"), imports=abs_imports)
geo_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "geo.capnp"), imports=abs_imports)
mgmt_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "management.capnp"), imports=abs_imports)


def create_seed_harvest_geoGrid_interpolator_and_read_data(path_to_csv_file, wgs84_crs, target_crs,
                                                           ilr_seed_harvest_data):
    "read seed/harvest dates and apoint climate stations"

    wintercrop = {
        "WW": True,
        "SW": False,
        "WR": True,
        "WRa": True,
        "WB": True,
        "SM": False,
        "GM": False,
        "SBee": False,
        "SU": False,
        "SB": False,
        "SWR": True,
        "CLALF": False,
        "PO": False
    }

    with open(path_to_csv_file) as _:
        reader = csv.reader(_)

        # print "reading:", path_to_csv_file

        # skip header line
        next(reader)

        points = []  # climate station position (lat, long transformed to a geoTargetGrid, e.g gk5)
        values = []  # climate station ids

        transformer = Transformer.from_crs(wgs84_crs, target_crs, always_xy=True)

        prev_cs = None
        prev_lat_lon = [None, None]
        # data_at_cs = defaultdict()
        for row in reader:

            # first column, climate station
            cs = int(row[0])

            # if new climate station, store the data of the old climate station
            if prev_cs is not None and cs != prev_cs:
                llat, llon = prev_lat_lon
                # r_geoTargetGrid, h_geoTargetGrid = transform(worldGeodeticSys84, geoTargetGrid, llon, llat)
                r_geoTargetGrid, h_geoTargetGrid = transformer.transform(llon, llat)

                points.append([r_geoTargetGrid, h_geoTargetGrid])
                values.append(prev_cs)

            crop_id = row[3]
            is_wintercrop = wintercrop[crop_id]
            ilr_seed_harvest_data[crop_id]["is-winter-crop"] = is_wintercrop

            base_date = date(2001, 1, 1)

            sdoy = int(float(row[4]))
            ilr_seed_harvest_data[crop_id]["data"][cs]["sowing-doy"] = sdoy
            sd = base_date + timedelta(days=sdoy - 1)
            ilr_seed_harvest_data[crop_id]["data"][cs]["sowing-date"] = {"year": 0, "month": sd.month,
                                                                         "day": sd.day}  # "0000-{:02d}-{:02d}".format(sd.month, sd.day)

            esdoy = int(float(row[8]))
            ilr_seed_harvest_data[crop_id]["data"][cs]["earliest-sowing-doy"] = esdoy
            esd = base_date + timedelta(days=esdoy - 1)
            ilr_seed_harvest_data[crop_id]["data"][cs]["earliest-sowing-date"] = {"year": 0, "month": esd.month,
                                                                                  "day": esd.day}  # "0000-{:02d}-{:02d}".format(esd.month, esd.day)

            lsdoy = int(float(row[9]))
            ilr_seed_harvest_data[crop_id]["data"][cs]["latest-sowing-doy"] = lsdoy
            lsd = base_date + timedelta(days=lsdoy - 1)
            ilr_seed_harvest_data[crop_id]["data"][cs]["latest-sowing-date"] = {"year": 0, "month": lsd.month,
                                                                                "day": lsd.day}  # "0000-{:02d}-{:02d}".format(lsd.month, lsd.day)

            digit = 1 if is_wintercrop else 0
            if crop_id == 'CLALF': digit = 2

            hdoy = int(float(row[6]))
            ilr_seed_harvest_data[crop_id]["data"][cs]["harvest-doy"] = hdoy
            hd = base_date + timedelta(days=hdoy - 1)
            ilr_seed_harvest_data[crop_id]["data"][cs]["harvest-date"] = {"year": 0, "month": hd.month,
                                                                          "day": hd.day}  # "000{}-{:02d}-{:02d}".format(digit, hd.month, hd.day)

            ehdoy = int(float(row[10]))
            ilr_seed_harvest_data[crop_id]["data"][cs]["earliest-harvest-doy"] = ehdoy
            ehd = base_date + timedelta(days=ehdoy - 1)
            ilr_seed_harvest_data[crop_id]["data"][cs]["earliest-harvest-date"] = {"year": digit, "month": ehd.month,
                                                                                   "day": ehd.day}  # "000{}-{:02d}-{:02d}".format(digit, ehd.month, ehd.day)

            lhdoy = int(float(row[11]))
            ilr_seed_harvest_data[crop_id]["data"][cs]["latest-harvest-doy"] = lhdoy
            lhd = base_date + timedelta(days=lhdoy - 1)
            ilr_seed_harvest_data[crop_id]["data"][cs]["latest-harvest-date"] = {"year": digit, "month": lhd.month,
                                                                                 "day": lhd.day}  # "000{}-{:02d}-{:02d}".format(digit, lhd.month, lhd.day)

            lat = float(row[1])
            lon = float(row[2])
            prev_lat_lon = (lat, lon)
            prev_cs = cs

        ilr_seed_harvest_data[crop_id]["interpolate"] = NearestNDInterpolator(np.array(points), np.array(values))


config = {
    "crop_ids": "WW,SW,WB",  # ALF,CLALF,GM,PO,SB,SBee,SM,SU,SW,SWR,WB,WG_test,WR,WRa,WW
    "crop_id_attr": "cropId",
    "in_sr": None,
    "out_sr": None,
    "latlon_attr": "latlon",
    "sowing_time_attr": "sowingTime",  # "fixed", #fixed | auto
    "harvest_time_attr": "harvestTime",  # "fixed", #fixed | auto
    "to_attr": "ilr",
}
# read commandline args only if script is invoked directly from commandline
common.update_config(config, sys.argv, print_config=True, allow_new_keys=False)

conman = common.ConnectionManager()
inp = conman.try_connect(config["in_sr"], cast_as=fbp_capnp.Channel.Reader, retry_secs=1)
outp = conman.try_connect(config["out_sr"], cast_as=fbp_capnp.Channel.Writer, retry_secs=1)

wgs84_crs = CRS.from_epsg(4326)
utm32n_crs = CRS.from_epsg(25832)

# load all crops to support by this component instance
ilr_path_template = str(
    PATH_TO_REPO) + "/data/management/ilr_seed_harvest_doys_germany/ILR_SEED_HARVEST_doys_{crop_id}.csv"
ilr_seed_harvest_data = defaultdict(lambda: {"interpolate": None, "data": defaultdict(dict), "is-winter-crop": None})
try:
    for crop_id in config["crop_ids"].split(","):
        # read seed/harvest dates for each crop_id
        path_harvest = ilr_path_template.format(crop_id=crop_id)
        print("created seed harvest gk5 interpolator and read data:", path_harvest)
        create_seed_harvest_geoGrid_interpolator_and_read_data(path_harvest, wgs84_crs, utm32n_crs,
                                                               ilr_seed_harvest_data)
except IOError:
    print("Couldn't read file:", path_harvest)
    exit(1)

try:
    if inp and outp:
        while True:
            in_msg = inp.read().wait()
            if in_msg.which() == "done":
                break

            in_ip = in_msg.value.as_struct(fbp_capnp.IP)
            latlon = common.get_fbp_attr(in_ip, config["latlon_attr"]).as_struct(geo_capnp.LatLonCoord)
            sowing_time = common.get_fbp_attr(in_ip, config["sowing_time_attr"]).as_text()
            harvest_time = common.get_fbp_attr(in_ip, config["harvest_time_attr"]).as_text()
            crop_id = common.get_fbp_attr(in_ip, config["crop_id_attr"]).as_text()

            utm = geo.transform_from_to_geo_coord(latlon, "utm32n")
            ilr_interpolate = ilr_seed_harvest_data[crop_id]["interpolate"]
            seed_harvest_cs = ilr_interpolate(utm.r, utm.h) if ilr_interpolate else None

            out_ip = fbp_capnp.IP.new_message()
            if ilr_interpolate is None or seed_harvest_cs is None:
                common.copy_and_set_fbp_attrs(in_ip, out_ip)
                outp.write(value=out_ip).wait()
            else:
                ilr_dates = mgmt_capnp.ILRDates.new_message()

                seed_harvest_data = ilr_seed_harvest_data[crop_id]["data"][seed_harvest_cs]
                if seed_harvest_data:
                    is_winter_crop = ilr_seed_harvest_data[crop_id]["is-winter-crop"]

                    if sowing_time == "fixed":  # fixed indicates that regionally fixed sowing dates will be used
                        sowing_date = seed_harvest_data["sowing-date"]
                    elif sowing_time == "auto":  # auto indicates that automatic sowng dates will be used that vary between regions
                        sowing_date = seed_harvest_data["latest-sowing-date"]
                    else:
                        sowing_date = None

                    if sowing_date:
                        sds = sowing_date
                        sd = date(2001, sds["month"], sds["day"])
                        sdoy = sd.timetuple().tm_yday

                    if harvest_time == "fixed":  # fixed indicates that regionally fixed harvest dates will be used
                        harvest_date = seed_harvest_data["harvest-date"]
                    elif harvest_time == "auto":  # auto indicates that automatic harvest dates will be used that vary between regions
                        harvest_date = seed_harvest_data["latest-harvest-date"]
                    else:
                        harvest_date = None

                    # print("sowing_date:", ilr_dates["sowing"], "harvest_date:", ilr_dates["harvest"])

                    if harvest_date:
                        hds = harvest_date
                        hd = date(2001, hds["month"], hds["day"])
                        hdoy = hd.timetuple().tm_yday

                    esds = seed_harvest_data["earliest-sowing-date"]
                    esd = date(2001, esds["month"], esds["day"])

                    # sowing after harvest should probably never occur in both fixed setup!
                    if sowing_time == "fixed" and harvest_time == "fixed":
                        # calc_harvest_date = date(2000, 12, 31) + timedelta(days=min(hdoy, sdoy-1))
                        if is_winter_crop:
                            calc_harvest_date = date(2000, 12, 31) + timedelta(days=min(hdoy, sdoy - 1))
                        else:
                            calc_harvest_date = date(2000, 12, 31) + timedelta(days=hdoy)
                        ilr_dates.sowing = seed_harvest_data["sowing-date"]
                        ilr_dates.harvest = {"year": hds["year"], "month": calc_harvest_date.month,
                                             "day": calc_harvest_date.day}  # "{:04d}-{:02d}-{:02d}".format(hds[0], calc_harvest_date.month, calc_harvest_date.day)
                        # print("dates: ", int(seed_harvest_cs), ":", ilr_dates["sowing"])
                        # print("dates: ", int(seed_harvest_cs), ":", ilr_dates["harvest"])

                    elif sowing_time == "fixed" and harvest_time == "auto":
                        if is_winter_crop:
                            calc_harvest_date = date(2000, 12, 31) + timedelta(days=min(hdoy, sdoy - 1))
                        else:
                            calc_harvest_date = date(2000, 12, 31) + timedelta(days=hdoy)
                        ilr_dates.sowing = seed_harvest_data["sowing-date"]
                        ilr_dates.latestHarvest = {"year": hds["year"], "month": calc_harvest_date.month,
                                                   "day": calc_harvest_date.day}  # "{:04d}-{:02d}-{:02d}".format(hds[0], calc_harvest_date.month, calc_harvest_date.day)
                        # print("dates: ", int(seed_harvest_cs), ":", ilr_dates["sowing"])
                        # print("dates: ", int(seed_harvest_cs), ":", latest_harvest_date)

                    elif sowing_time == "auto" and harvest_time == "fixed":
                        ilr_dates.earliestSowing = seed_harvest_data["earliest-sowing-date"] if esd > date(esd.year, 6,
                                                                                                           20) else {
                            "year": sds["year"], "month": 6, "day": 20}  # "{:04d}-{:02d}-{:02d}".format(sds[0], 6, 20)
                        calc_sowing_date = date(2000, 12, 31) + timedelta(days=max(hdoy + 1, sdoy))
                        ilr_dates.latestSowing = {"year": sds["year"], "month": calc_sowing_date.month,
                                                  "day": calc_sowing_date.day}  # "{:04d}-{:02d}-{:02d}".format(sds[0], calc_sowing_date.month, calc_sowing_date.day)
                        ilr_dates.harvest = seed_harvest_data["harvest-date"]
                        # print("dates: ", int(seed_harvest_cs), ":", ilr_dates["earliestSowing"], "<", ilr_dates["latestSowing"])
                        # print("dates: ", int(seed_harvest_cs), ":", ilr_dates["harvest"])

                    elif sowing_time == "auto" and harvest_time == "auto":
                        ilr_dates.earliestSowing = seed_harvest_data["earliest-sowing-date"] if esd > date(esd.year, 6,
                                                                                                           20) else {
                            "year": sds["year"], "month": 6, "day": 20}  # "{:04d}-{:02d}-{:02d}".format(sds[0], 6, 20)
                        if is_winter_crop:
                            calc_harvest_date = date(2000, 12, 31) + timedelta(days=min(hdoy, sdoy - 1))
                        else:
                            calc_harvest_date = date(2000, 12, 31) + timedelta(days=hdoy)
                        ilr_dates.latestSowing = seed_harvest_data["latest-sowing-date"]
                        ilr_dates.latestHarvest = {"year": hds["year"], "month": calc_harvest_date.month,
                                                   "day": calc_harvest_date.day}  # "{:04d}-{:02d}-{:02d}".format(hds[0], calc_harvest_date.month, calc_harvest_date.day)
                        # print("dates: ", int(seed_harvest_cs), ":", ilr_dates["earliestSowing"], "<", ilr_dates["latestSowing"])
                        # print("dates: ", int(seed_harvest_cs), ":", ilr_dates["latestHarvest"])

                common.copy_and_set_fbp_attrs(in_ip, out_ip,
                                              **({config["to_attr"]: ilr_dates} if config["to_attr"] else {}))
                outp.write(value=out_ip).wait()

        outp.write(done=None).wait()

except Exception as e:
    print("ilr_sowing_harvest_dates_fbp_component.py ex:", e)

print("ilr_sowing_harvest_dates_fbp_component.py: exiting FBP component")
