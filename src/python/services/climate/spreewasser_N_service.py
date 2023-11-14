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
from netCDF4 import Dataset as NCDataset
import numpy as np
import os
from pathlib import Path
from pyproj import Transformer
from scipy.interpolate import NearestNDInterpolator
import sys

# remote debugging via commandline
# -m ptvsd --host 0.0.0.0 --port 14000 --wait

PATH_TO_REPO = Path(os.path.realpath(__file__)).parent.parent.parent.parent.parent
if str(PATH_TO_REPO) not in sys.path:
    sys.path.insert(1, str(PATH_TO_REPO))

PATH_TO_PYTHON_CODE = PATH_TO_REPO / "src/python"
if str(PATH_TO_PYTHON_CODE) not in sys.path:
    sys.path.insert(1, str(PATH_TO_PYTHON_CODE))

from pkgs.common import common
from pkgs.climate import common_climate_data_capnp_impl as ccdi
from pkgs.common import service as serv

PATH_TO_CAPNP_SCHEMAS = PATH_TO_REPO / "capnproto_schemas"
abs_imports = [str(PATH_TO_CAPNP_SCHEMAS)]
reg_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "registry.capnp"), imports=abs_imports)
climate_data_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "climate.capnp"), imports=abs_imports)


class MultiTimeSeries(climate_data_capnp.TimeSeries.Server):

    def __init__(self, data_t, header, start_date, metadata=None, location=None):
        self._data_t = data_t
        self._data = None
        self._header = header
        self._meta = metadata
        self._location = location
        no_of_days = len(data_t[0]) if len(data_t) > 0 else 0
        self._start_date = start_date
        self._end_date = start_date + timedelta(days=no_of_days - 1)

    def append_data(self, data_t, start_date):
        no_of_days = len(data_t[0]) if len(data_t) > 0 else 0
        end_date = start_date + timedelta(days=no_of_days - 1)
        new_data_t = []
        if start_date <= self._end_date:
            td = (self._end_date - start_date) + timedelta(days=1)
            for ds, ds_ in zip(self._data_t, data_t):
                nds = list(ds[:-td.days])
                nds.extend(ds_)
                new_data_t.append(nds)
        elif start_date == self._end_date + timedelta(days=1):
            for ds, ds_ in zip(self._data_t, data_t):
                nds = list(ds)
                nds.extend(ds_)
                new_data_t.append(nds)
        elif end_date >= self._start_date:
            td = (end_date - self.start_date) + timedelta(days=1)
            for ds, ds_ in zip(self._data_t, data_t):
                nds = list(ds_)
                nds.extend(ds[td.days:])
                new_data_t.append(nds)
        elif end_date == self._start_date - timedelta(days=1):
            for ds, ds_ in zip(self._data_t, data_t):
                nds = list(ds_)
                nds.extend(ds)
                new_data_t.append(nds)
        else:
            raise Exception("MultiTimeSeries.append_data would produce gaps in time-series")

        self._data_t = new_data_t
        no_of_days = len(self._data_t[0]) if len(self._data_t) > 0 else 0
        self._end_date = self._start_date + timedelta(days=no_of_days - 1)
        self._data = None

    def resolution_context(self, context):  # -> (resolution :TimeResolution);
        context.results.resolution = climate_data_capnp.TimeSeries.Resolution.daily

    def range_context(self, context):  # -> (startDate :Date, endDate :Date);
        context.results.startDate = ccdi.create_capnp_date(self._start_date)
        context.results.endDate = ccdi.create_capnp_date(self._end_date)

    def header(self, **kwargs):  # () -> (header :List(Element));
        return self._header

    def data(self, **kwargs):  # () -> (data :List(List(Float32)));
        if self._data is None:
            no_of_days = len(self._data_t[0]) if len(self._data_t) > 0 else 0
            self._data = list([list(map(lambda ds: ds[i], self._data_t)) for i in range(no_of_days)])
        return self._data

    def dataT(self, **kwargs):  # () -> (data :List(List(Float32)));
        return self._data_t

    def subrange_context(self, context):  # (from :Date, to :Date) -> (timeSeries :TimeSeries);
        from_date = ccdi.create_date(getattr(context.params, "from"))
        to_date = ccdi.create_date(context.params.to)
        start_i = (from_date - self._start_date).days
        end_i = (to_date - self._start_date).days
        sub_data_t = [ds[start_i: end_i + 1] for ds in self._data_t]
        context.results.timeSeries = MultiTimeSeries(sub_data_t, self._header, from_date, metadata=self._meta,
                                                     location=self._location)

    def subheader(self, elements, **kwargs):  # (elements :List(Element)) -> (timeSeries :TimeSeries);
        sub_header = [str(e) for e in elements]
        sub_data_t = []
        for i, elem in enumerate(self._header):
            if elem in sub_header:
                sub_data_t.append(self._data_t[i])
        return MultiTimeSeries(sub_data_t, sub_header, self._start_date, metadata=self._meta, location=self._location)

    def metadata(self, _context, **kwargs):  # metadata @7 () -> Metadata;
        """the metadata for this time series"""
        if self._meta:
            r = _context.results
            r.init("entries", len(self._meta.entries))
            for i, e in enumerate(self._meta.entries):
                r.entries[i] = e
            r.info = self._meta.info

    def location(self, _context, **kwargs):  # location @8 () -> Location;
        """location of this time series"""
        r = _context.results
        r.timeSeries = self
        if self._location:
            r.id = self._location.id
            r.heightNN = self._location.heightNN
            r.latlon = self._location.latlon


def kelvin_to_degree_celcius(degree_kelvin):
    return degree_kelvin - 273.15


def mm_per_sec_to_mm_per_day(precip):
    return precip * 60 * 60 * 24


def j_per_m2_sec_to_mj_per_day(gr):
    return gr * 60 * 60 * 24 / 1000000


def identity(v):
    return v


class Dataset(climate_data_capnp.Dataset.Server):

    def __init__(self, historic_dataset_sr, path_to_historic_nc_files, path_to_6month_forecast_nc_files, metadata=None):
        self.year_to_historic_elem_to_data = {}
        for year in range(2022, 2023 + 1):
            self.year_to_historic_elem_to_data[year] = {
                "tmax": {"var": "tasmax", "convf": identity,
                         "ds": NCDataset(path_to_historic_nc_files + f"/zalf_tasmax_amber_{year}_v1-0.nc")},  # -> °C
                "tavg": {"var": "tas", "convf": identity,
                         "ds": NCDataset(path_to_historic_nc_files + f"/zalf_tas_amber_{year}_v1-0.nc")},  # -> °C
                "tmin": {"var": "tasmin", "convf": identity,
                         "ds": NCDataset(path_to_historic_nc_files + f"/zalf_tasmin_amber_{year}_v1-0.nc")},  # -> °C
                "precip": {"var": "pr", "convf": identity,  #mm_per_sec_to_mm_per_day,
                           "ds": NCDataset(path_to_historic_nc_files + f"/zalf_pr_amber_{year}_v1-0.nc")},  # -> mm
                "globrad": {"var": "rsds", "convf": j_per_m2_sec_to_mj_per_day,
                            "ds": NCDataset(path_to_historic_nc_files + f"/zalf_rsds_amber_{year}_v1-0.nc")},
                # -> MJ/m2/d
                "wind": {"var": "sfcWind", "convf": identity,
                         "ds": NCDataset(path_to_historic_nc_files + f"/zalf_sfcwind_amber_{year}_v1-0.nc")},  # -> m/s
                "relhumid": {"var": "hurs", "convf": identity,
                             "ds": NCDataset(path_to_historic_nc_files + f"/zalf_hurs_amber_{year}_v1-0.nc")}  # -> %
            }

        fc_ensmem = "r1i1p1"
        fc_start_date = "2022-11-01".replace("-", "")
        self.fc_start_date = date.fromisoformat("2022-11-01")
        fc_end_date = "2023-04-30".replace("-", "")
        self.forecast_elem_to_data = {
            "tmax": {"var": "tasmax", "convf": kelvin_to_degree_celcius,
                     "ds": NCDataset(
                         path_to_6month_forecast_nc_files + f"/tasmax_day_GCFS21--DWD-EPISODES2022--DE-0075x005_sfc20221101_{fc_ensmem}_{fc_start_date}-{fc_end_date}.nc")},
            # -> °C
            "tavg": {"var": "tas", "convf": kelvin_to_degree_celcius,
                     "ds": NCDataset(
                         path_to_6month_forecast_nc_files + f"/tas_day_GCFS21--DWD-EPISODES2022--DE-0075x005_sfc20221101_{fc_ensmem}_{fc_start_date}-{fc_end_date}.nc")},
            # -> °C
            "tmin": {"var": "tasmin", "convf": kelvin_to_degree_celcius,
                     "ds": NCDataset(
                         path_to_6month_forecast_nc_files + f"/tasmin_day_GCFS21--DWD-EPISODES2022--DE-0075x005_sfc20221101_{fc_ensmem}_{fc_start_date}-{fc_end_date}.nc")},
            # -> °C
            "precip": {"var": "pr", "convf": mm_per_sec_to_mm_per_day,
                       "ds": NCDataset(
                           path_to_6month_forecast_nc_files + f"/pr_day_GCFS21--DWD-EPISODES2022--DE-0075x005_sfc20221101_{fc_ensmem}_{fc_start_date}-{fc_end_date}.nc")},
            # -> mm
            "globrad": {"var": "rsds", "convf": j_per_m2_sec_to_mj_per_day,
                        "ds": NCDataset(
                            path_to_6month_forecast_nc_files + f"/rsds_day_GCFS21--DWD-EPISODES2022--DE-0075x005_sfc20221101_{fc_ensmem}_{fc_start_date}-{fc_end_date}.nc")},
            # -> MJ/m2/d
            "wind": {"var": "sfcWind", "convf": identity,
                     "ds": NCDataset(
                         path_to_6month_forecast_nc_files + f"/sfcWind_day_GCFS21--DWD-EPISODES2022--DE-0075x005_sfc20221101_{fc_ensmem}_{fc_start_date}-{fc_end_date}.nc")},
            # -> m/s
            "relhumid": {"var": "hurs", "convf": identity,
                         "ds": NCDataset(
                             path_to_6month_forecast_nc_files + f"/hurs_day_GCFS21--DWD-EPISODES2022--DE-0075x005_sfc20221101_{fc_ensmem}_{fc_start_date}-{fc_end_date}.nc")}
            # -> %
        }

        no_of_days = 0
        years = list(sorted(self.year_to_historic_elem_to_data.keys()))
        hist_dss = {}
        for year in years:
            tavg = self.year_to_historic_elem_to_data[year].get("tavg", None)
            if tavg:
                hist_dss[year] = tavg["ds"]
                no_of_days += np.ma.count(tavg["ds"]["tas"][:, 300, 300])

        self.hist_ll0rs = {}
        for year, ds in hist_dss.items():
            if ds:
                self.hist_ll0rs[year] = {
                    "lat_0": ds["lat"][0],
                    "lat_res": ds["lat"][0] - ds["lat"][1],
                    "no_rows": len(ds["lat"]),
                    "lon_0": ds["lon"][0],
                    "lon_res": ds["lon"][1] - ds["lon"][0],
                    "no_cols": len(ds["lon"]),
                }

        fc_tavg_ds = self.forecast_elem_to_data["tavg"]["ds"] if "tavg" in self.forecast_elem_to_data else None
        no_of_days += fc_tavg_ds["time"].shape[0] if fc_tavg_ds else 0
        self.fc_ll0r = {
            "lat_0": fc_tavg_ds["lat"][0],
            "lat_res": fc_tavg_ds["lat"][1] - fc_tavg_ds["lat"][0],
            "no_rows": len(fc_tavg_ds["lat"]),
            "lon_0": fc_tavg_ds["lon"][0],
            "lon_res": fc_tavg_ds["lon"][1] - fc_tavg_ds["lon"][0],
            "no_cols": len(fc_tavg_ds["lon"]),
        } if fc_tavg_ds else None

        self._meta = climate_data_capnp.Metadata.new_message(
            entries=[
                {"historical": None},
                {"start": ccdi.create_capnp_date(date(years[0], 1, 1))},
                {"end": ccdi.create_capnp_date(date(years[0], 1, 1) + timedelta(days=int(no_of_days) - 1))}
            ])
        self._time_series = {}
        self._locations = {}
        self._all_locations_created = False

    def metadata(self, _context, **kwargs):  # metadata @0 () -> Metadata;
        # get metadata for these data 
        r = _context.results
        r.init("entries", len(self._meta.entries))
        for i, e in enumerate(self._meta.entries):
            r.entries[i] = e
        r.info = self._meta.info

    def time_series_at(self, lat, lon, location=None):
        ilat = int(round(lat, 2)*100)
        ilon = int(round(lon, 2)*100)

        if (ilat, ilon) not in self._time_series:
            if not location:
                location = self.location_at(lat, lon)

            def col(ll0r):
                return int(abs((lon - ll0r["lon_0"]) / ll0r["lon_res"]))

            def row(ll0r):
                return int(abs((ll0r["lat_0"] - lat) / ll0r["lat_res"]))

            def create_data_t(elem_to_data, row, col):
                return list(
                    [list(map(lambda v: float(data["convf"](v)), data["ds"][data["var"]][:, row, col].compressed())) for data in
                     elem_to_data.values()])

            time_series = None
            for year in sorted(self.hist_ll0rs.keys()):
                ll0r = self.hist_ll0rs[year]
                c = col(ll0r)
                r = row(ll0r)
                if 0 <= r < ll0r["no_rows"] and 0 <= c < ll0r["no_cols"]:
                    elem_to_data = self.year_to_historic_elem_to_data[year]
                    data_t = create_data_t(elem_to_data, r, c)
                    start_date = date(year, 1, 1)
                    if time_series:
                        time_series.append_data(data_t, start_date)
                    else:
                        time_series = MultiTimeSeries(data_t, list(elem_to_data.keys()), start_date,
                                                      metadata=self._meta, location=location)

            c = col(self.fc_ll0r)
            r = row(self.fc_ll0r)
            if 0 <= r < self.fc_ll0r["no_rows"] and 0 <= c < self.fc_ll0r["no_cols"]:
                elem_to_data = self.forecast_elem_to_data
                data_t = create_data_t(elem_to_data, r, c)
                start_date = self.fc_start_date
                if time_series:
                    time_series.append_data(data_t, start_date)
                else:
                    time_series = MultiTimeSeries(data_t, list(elem_to_data.keys()), start_date,
                                                  metadata=self._meta, location=location)

            self._time_series[(ilat, ilon)] = time_series

        return self._time_series[(ilat, ilon)]

    def closestTimeSeriesAt(self, latlon, **kwargs):  # (latlon :Geo.LatLonCoord) -> (timeSeries :TimeSeries);
        # closest TimeSeries object which represents the whole time series 
        # of the climate realization at the give climate coordinate
        lat, lon = (latlon.lat, latlon.lon)
        return self.time_series_at(lat, lon)

    def timeSeriesAt(self, locationId, **kwargs):  # (locationId :Text) -> (timeSeries :TimeSeries);
        lat_s, lon_s = locationId.split("/")
        lat = float(lat_s[4:])
        lon = float(lon_s[4:])
        return self.time_series_at(lat, lon)

    def location_at(self, lat, lon, alt=0, time_series=None):
        id = f"lat:{lat}/lon:{lon}"
        name = f"LatLon:{lat}/{lon}"
        loc = climate_data_capnp.Location.new_message(
            id={"id": id, "name": name, "description": ""},
            heightNN=alt,
            latlon={"lat": lat, "lon": lon}
        )
        if time_series:
            loc.timeSeries = time_series
        return loc

    def locations(self, **kwargs):  # locations @2 () -> (locations :List(Location));
        # all the climate locations this dataset has
        locs = []
        #if not self._all_locations_created:
        #    for (row, col), (gk4_r, gk4_h) in self._rowcol_to_gk4_rh.items():
        #        lon, lat = self._gk4_to_latlon_transformer(gk4_r, gk4_h)
        #        ll_coord = {"lat": lat, "lon": lon, "alt": -9999}
        #        # row, col = row_col
        #        loc = self.location_at(row, col, ll_coord)
        #        ts = self.time_series_at(row, col, loc)
        #        loc.timeSeries = ts
        #        locs.append(loc)
        #    self._all_locations_created = True
        #else:
        #    locs.extend(self._locations.values())
        return locs


async def main(historic_dataset_sr, path_to_historic_nc_files, path_to_6month_forecast_nc_files,
               serve_bootstrap=True, host=None, port=None,
               id=None, name="Spreewasser N", description=None, use_async=False, srt=None):
    config = {
        "historic_dataset_sr": historic_dataset_sr,
        "path_to_historic_nc_files": path_to_historic_nc_files,
        "path_to_6month_forecast_nc_files": path_to_6month_forecast_nc_files,
        "port": port,
        "host": host,
        "id": id,
        "name": name,
        "description": description,
        "serve_bootstrap": serve_bootstrap,
        "use_async": use_async,
        "srt": srt
    }
    common.update_config(config, sys.argv, print_config=True, allow_new_keys=False)

    restorer = common.Restorer()
    service = Dataset(historic_dataset_sr, path_to_historic_nc_files, path_to_6month_forecast_nc_files)

    if use_async:
        await serv.async_init_and_run_service({"service": service}, config["host"], config["port"],
                                              serve_bootstrap=config["serve_bootstrap"], restorer=restorer,
                                              name_to_service_srs={"service": config["srt"]})
    else:
        serv.init_and_run_service({"service": service}, config["host"], config["port"],
                                  serve_bootstrap=config["serve_bootstrap"], restorer=restorer,
                                  name_to_service_srs={"service": config["srt"]})


if __name__ == '__main__':
    asyncio.run(main("????",
                     "/home/berg/Desktop/roland/DWD_SpreeWasser_N/",
                     "/home/berg/Desktop/roland/DWD_6_month_forecast/",
                     use_async=True))
