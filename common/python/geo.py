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
import json
import os
import sys
import time
import uuid

from pyproj import CRS, Transformer

import capnp
capnp.add_import_hook(additional_paths=["capnproto_schemas"])
import common_capnp
import geo_coord_capnp as geo

#------------------------------------------------------------------------------

def name_to_proj(name, default=None):
    return {
        "latlon": CRS.from_epsg(geo.Geo.EPSG.wgs84),
        "gk3":CRS.from_epsg(geo.Geo.EPSG.gk3),
        "gk5": CRS.from_epsg(geo.Geo.EPSG.gk5),
        "utm21S": CRS.from_epsg(geo.Geo.EPSG.utm21S),
        "utm32N": CRS.from_epsg(geo.Geo.EPSG.utm32N)
    }.get(name, default)

#------------------------------------------------------------------------------

def geo_coord_to_latlon(geo_coord):

    if not hasattr(geo_coord_to_latlon, "gk_cache"):
        geo_coord_to_latlon.gk_cache = {}
    if not hasattr(geo_coord_to_latlon, "utm_cache"):
        geo_coord_to_latlon.utm_cache = {}
    if not hasattr(geo_coord_to_latlon, "latlon_crs"):
        geo_coord_to_latlon.latlon_crs = name_to_proj("latlon")

    which = geo_coord.which()
    if which == "gk":
        meridian = geo_coord.gk.meridianNo
        if meridian not in geo_coord_to_latlon.gk_cache:
            gk_crs = CRS.from_epsg(geo.Geo.EPSG["gk" + str(meridian)])
            trans = geo_coord_to_latlon.gk_cache[meridian] = Transformer.from_crs(gk_crs, geo_coord_to_latlon.latlon_crs, always_xy=True)
        else:
            trans = geo_coord_to_latlon.gk_cache[meridian]
        lon, lat = trans.transform(geo_coord.gk.r, geo_coord.gk.h)
    elif which == "latlon":
        lat, lon = geo_coord.latlon.lat, geo_coord.latlon.lon
    elif which == "utm":
        utm_id = str(geo_coord.utm.zone) + geo_coord.utm.latitudeBand
        if meridian not in geo_coord_to_latlon.utm_cache:
            utm_crs = CRS.from_epsg(geo.Geo.EPSG["utm" + utm_id])
            trans = geo_coord_to_latlon.utm_cache[utm_id] = Transformer.from_crs(utm_crs, geo_coord_to_latlon.latlon_crs, always_xy=True)
        else:
            trans = geo_coord_to_latlon.utm_cache[utm_id]
        lon, lat = trans.transform(geo_coord.utm.r, geo_coord.utm.h)

    return lat, lon