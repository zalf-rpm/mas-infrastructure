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
from netCDF4 import Dataset
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

from pkgs.common import geo
from pkgs.common import capnp_async_helpers as async_helpers
from pkgs.climate import common_climate_data_capnp_impl as ccdi

PATH_TO_CAPNP_SCHEMAS = PATH_TO_REPO / "capnproto_schemas"
abs_imports = [str(PATH_TO_CAPNP_SCHEMAS)]
reg_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "registry.capnp"), imports=abs_imports)
climate_data_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "climate.capnp"), imports=abs_imports)


class TimeSeries(climate_data_capnp.TimeSeries.Server):

    def __init__(self, data_t, header, metadata=None, location=None):
        self._data_t = data_t
        self._data = None
        self._header = header
        self._meta = metadata
        self._location = location
        no_of_days = len(data_t[0]) if len(data_t) > 0 else 0
        self._start_date = date(1961, 1, 1)
        self._end_date = date(1961, 1, 1) + timedelta(days=no_of_days - 1)

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
        context.results.timeSeries = TimeSeries(sub_data_t, self._header, metadata=self._meta, location=self._location)

    def subheader(self, elements, **kwargs):  # (elements :List(Element)) -> (timeSeries :TimeSeries);
        sub_header = [str(e) for e in elements]
        sub_data_t = []
        for i, elem in enumerate(self._header):
            if elem in sub_header:
                sub_data_t.append(self._data_t[i])
        return TimeSeries(sub_data_t, sub_header, metadata=self._meta, location=self._location)

    def metadata(self, _context, **kwargs):  # metadata @7 () -> Metadata;
        "the metadata for this time series"
        if self._meta:
            r = _context.results
            r.init("entries", len(self._meta.entries))
            for i, e in enumerate(self._meta.entries):
                r.entries[i] = e
            r.info = self._meta.info

    def location(self, _context, **kwargs):  # location @8 () -> Location;
        "location of this time series"
        r = _context.results
        r.timeSeries = self
        if self._location:
            r.id = self._location.id
            r.heightNN = self._location.heightNN
            r.latlon = self._location.latlon


class DatasetImpl(climate_data_capnp.Dataset.Server):

    def __init__(self, path_to_nc_files, region="sn", metadata=None):

        self._elem_to_data = {}
        if region == "sn":
            self._elem_to_data = {
                "tmax": {"var": "tx", "convf": 1.0, "ds": Dataset(path_to_nc_files + "/Temperatur_max.nc")},  # -> °C
                "tavg": {"var": "tmean", "convf": 1.0, "ds": Dataset(path_to_nc_files + "/Temperatur_mean.nc")},
                # -> °C
                "tmin": {"var": "tn", "convf": 1.0, "ds": Dataset(path_to_nc_files + "/Temperatur_min.nc")},  # -> °C
                "precip": {"var": "p", "convf": 1.0, "ds": Dataset(path_to_nc_files + "/Niederschlag.nc")},  # -> mm
                "relhumid": {"var": "rh", "convf": 1.0, "ds": Dataset(path_to_nc_files + "/Relative_Feuchte.nc")},
                # -> %
                "globrad": {"var": "gr", "convf": 3.6, "ds": Dataset(path_to_nc_files + "/Globalstrahlung.nc")},
                # -> MJ/m2/d
                "wind": {"var": "wind", "convf": 1.0, "ds": Dataset(path_to_nc_files + "/Windgeschwindigkeit.nc")}
                # -> m/s
            }
        elif region == "sa":
            self._elem_to_data = {
                "tmax": {"var": "Tagesmaximum_Temperatur", "convf": 1.0,
                         "ds": Dataset(path_to_nc_files + "/Temperatur_max.nc")},  # -> °C
                "tavg": {"var": "Tagesmittel_Temperatur", "convf": 1.0,
                         "ds": Dataset(path_to_nc_files + "/Temperatur_mean.nc")},  # -> °C
                "tmin": {"var": "TagesminimumTemperatur", "convf": 1.0,
                         "ds": Dataset(path_to_nc_files + "/Temperatur_min.nc")},  # -> °C
                "precip": {"var": "Korrigierter_Niederschlag", "convf": 1.0,
                           "ds": Dataset(path_to_nc_files + "/Niederschlag.nc")},  # -> mm
                "globrad": {"var": "Globalstrahlung", "convf": 3.6,
                            "ds": Dataset(path_to_nc_files + "/Globalstrahlung.nc")},  # -> MJ/m2/d
                "wind": {"var": "Tagesmittel_Windgeschwindigkeit", "convf": 1.0,
                         "ds": Dataset(path_to_nc_files + "/Windgeschwindigkeit.nc")},  # -> m/s
                "airpress": {"var": "Tagesmittel_Luftdruck", "convf": 1.0,
                             "ds": Dataset(path_to_nc_files + "/Luftdruck.nc")}  # -> hPa
            }
        elif region == "tn":
            self._elem_to_data = {
                "tmax": {"var": "Tagesmaximum_Temperatur", "convf": 1.0,
                         "ds": Dataset(path_to_nc_files + "/Temperatur_max.nc")},  # -> °C
                "tavg": {"var": "Tagesmittel_Temperatur", "convf": 1.0,
                         "ds": Dataset(path_to_nc_files + "/Temperatur_mean.nc")},  # -> °C
                "tmin": {"var": "TagesminimumTemperatur", "convf": 1.0,
                         "ds": Dataset(path_to_nc_files + "/Temperatur_min.nc")},  # -> °C
                "precip": {"var": "Korrigierter_Niederschlag", "convf": 1.0,
                           "ds": Dataset(path_to_nc_files + "/Niederschlag.nc")},  # -> mm
                "relhumid": {"var": "Relative_Feuchte", "convf": 1.0,
                             "ds": Dataset(path_to_nc_files + "/Relative_Feuchte.nc")},  # -> mm
                "globrad": {"var": "Globalstrahlung", "convf": 3.6,
                            "ds": Dataset(path_to_nc_files + "/Globalstrahlung.nc")},  # -> MJ/m2/d
                "wind": {"var": "Tagesmittel_Windgeschwindigkeit", "convf": 1.0,
                         "ds": Dataset(path_to_nc_files + "/Windgeschwindigkeit.nc")},  # -> m/s
                "airpress": {"var": "Tagesmittel_Luftdruck", "convf": 1.0,
                             "ds": Dataset(path_to_nc_files + "/Luftdruck.nc")}  # -> hPa
            }

        no_of_days = self._elem_to_data["tavg"]["ds"]["time"].shape[0] if "tavg" in self._elem_to_data else 0
        self._meta = climate_data_capnp.Metadata.new_message(
            entries=[
                {"historical": None},
                {"start": ccdi.create_capnp_date(date(1961, 1, 1))},
                {"end": ccdi.create_capnp_date(date(1961, 1, 1) + timedelta(days=no_of_days - 1))}
            ])
        self._time_series = {}
        self._locations = {}
        self._all_locations_created = False
        self._rowcol_to_gk4_rh = {}

        latlon_crs = geo.name_to_crs("latlon")
        gk4_crs = geo.name_to_crs("gk4")
        self._latlon_to_gk4_transformer = Transformer.from_crs(latlon_crs, gk4_crs, always_xy=True)
        self._gk4_to_latlon_transformer = Transformer.from_crs(gk4_crs, latlon_crs, always_xy=True)
        self._interpolator = self.create_interpolator()

    def metadata(self, _context, **kwargs):  # metadata @0 () -> Metadata;
        # get metadata for these data 
        r = _context.results
        r.init("entries", len(self._meta.entries))
        for i, e in enumerate(self._meta.entries):
            r.entries[i] = e
        r.info = self._meta.info

    def create_interpolator(self):
        "read an ascii grid into a map, without the no-data values"

        tavg = self._elem_to_data["tavg"]
        ds = tavg["ds"]

        gk4_coords = []
        row_cols = []

        # first day
        arr = ds[tavg["var"]][0]
        xs = ds["x"]
        ys = ds["y"]

        nrows = ys.shape[0]
        ncols = xs.shape[0]

        for row in range(nrows):
            for col in range(ncols):
                if arr.mask[row, col]:
                    continue
                r_gk4 = int(xs[col])
                h_gk4 = int(ys[row])
                gk4_coords.append([r_gk4, h_gk4])
                row_cols.append((row, col))
                self._rowcol_to_gk4_rh[(row, col)] = (r_gk4, h_gk4)
                # print "row:", row, "col:", col, "lat:", lat, "lon:", lon, "val:", values[i]
            # print row,

        return NearestNDInterpolator(gk4_coords, row_cols)

    def time_series_at(self, row, col, location=None):
        if (row, col) not in self._time_series:
            data_t = list([list(map(float, data["ds"][data["var"]][:, row, col] * data["convf"])) for data in
                           self._elem_to_data.values()])

            if not location:
                location = self.location_at(row, col)

            timeSeries = TimeSeries(data_t, list(self._elem_to_data.keys()), metadata=self._meta, location=location)

            self._time_series[(row, col)] = timeSeries

        return self._time_series[(row, col)]

    def closestTimeSeriesAt(self, latlon, **kwargs):  # (latlon :Geo.LatLonCoord) -> (timeSeries :TimeSeries);
        # closest TimeSeries object which represents the whole time series 
        # of the climate realization at the give climate coordinate
        lat, lon = (latlon.lat, latlon.lon)
        gk4_r, gk4_h = self._latlon_to_gk4_transformer.transform(lon, lat)
        row, col = self._interpolator(gk4_r, gk4_h)
        return self.time_series_at(row, col)

    def timeSeriesAt(self, locationId, **kwargs):  # (locationId :Text) -> (timeSeries :TimeSeries);
        rs, cs = locationId.split("/")
        row = int(rs[2:])
        col = int(cs[2:])
        return self.time_series_at(row, col)

    def location_at(self, row, col, ll_coord=None, time_series=None):
        if (row, col) not in self._locations:
            if not ll_coord:
                gk4_r, gk4_h = self._rowcol_to_gk4_rh[(row, col)]
                lonlat = self._gk4_to_latlon_transformer.transform(gk4_r, gk4_h)
                ll_coord = {"lat": lonlat[1], "lon": lonlat[0], "alt": -9999}
            id = "r:{}/c:{}".format(row, col)
            name = "Row/Col:{}/{}|LatLon:{}/{}".format(row, col, ll_coord["lat"], ll_coord["lon"])
            loc = climate_data_capnp.Location.new_message(
                id={"id": id, "name": name, "description": ""},
                heightNN=ll_coord["alt"],
                latlon={"lat": ll_coord["lat"], "lon": ll_coord["lon"]}
            )
            if time_series:
                loc.timeSeries = time_series
            self._locations[(row, col)] = loc
        return self._locations[(row, col)]

    def locations(self, **kwargs):  # locations @2 () -> (locations :List(Location));
        # all the climate locations this dataset has
        locs = []
        if not self._all_locations_created:
            for (row, col), (gk4_r, gk4_h) in self._rowcol_to_gk4_rh.items():
                lon, lat = self._gk4_to_latlon_transformer(gk4_r, gk4_h)
                ll_coord = {"lat": lat, "lon": lon, "alt": -9999}
                # row, col = row_col
                loc = self.location_at(row, col, ll_coord)
                ts = self.time_series_at(row, col, loc)
                loc.timeSeries = ts
                locs.append(loc)
            self._all_locations_created = True
        else:
            locs.extend(self._locations.values())
        return locs


async def async_main(path_to_nc_files, region="sn", serve_bootstrap=False,
                     host="0.0.0.0", port=None, reg_sturdy_ref=None, id=None, name="Klima Konform", description=None):
    config = {
        "path_to_nc_files": path_to_nc_files,
        "region": region,
        "host": host,
        "port": port,
        "id": id,
        "name": name,
        "description": description,
        "reg_sturdy_ref": reg_sturdy_ref,
        "serve_bootstrap": str(serve_bootstrap),
        "reg_category": "climate",
    }
    # read commandline args only if script is invoked directly from commandline
    if len(sys.argv) > 1 and __name__ == "__main__":
        for arg in sys.argv[1:]:
            k, v = arg.split("=")
            if k in config:
                config[k] = v
    print("config used:", config)

    conMan = async_helpers.ConnectionManager()

    # interpolator, rowcol_to_latlon = ccdi.create_lat_lon_interpolator_from_json_coords_file(config["path_to_data"] + "/" + "latlon-to-rowcol.json")
    # meta_plus_data = create_meta_plus_datasets(config["path_to_data"], interpolator, rowcol_to_latlon)
    service = DatasetImpl(path_to_nc_files, config["region"])

    if config["reg_sturdy_ref"]:
        registrator = await conMan.try_connect(config["reg_sturdy_ref"], cast_as=reg_capnp.Registrator)
        if registrator:
            unreg = await registrator.register(ref=service, categoryId=config["reg_category"]).a_wait()
            print("Registered ", config["name"], "climate service.")
            # await unreg.unregister.unregister().a_wait()
        else:
            print("Couldn't connect to registrator at sturdy_ref:", config["reg_sturdy_ref"])

    if config["serve_bootstrap"].upper() == "TRUE":
        await async_helpers.serve_forever(config["host"], config["port"], service)
    else:
        await conMan.manage_forever()


if __name__ == '__main__':
    asyncio.run(async_main("/home/berg/Schreibtisch/klima_konform/sn", region="sn", serve_bootstrap=True, port=8888))
