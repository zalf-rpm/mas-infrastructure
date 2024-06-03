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

import capnp
from collections import defaultdict
import os
from pathlib import Path
from pyproj import CRS, Transformer
import sys

import zalfmas_capnpschemas

sys.path.append(os.path.dirname(zalfmas_capnpschemas.__file__))
import common_capnp
import geo_capnp


def name_to_struct_instance(name, x=None, y=None, default=None):
    lname = name.lower()
    c = default
    if lname == "2d" or lname == "xy":
        c = geo_capnp.Point2D.new_message()
        if x:
            c.x = x
        if y: 
            c.y = y
    elif lname == "latlon" or name == "wgs84":
        c = geo_capnp.LatLonCoord.new_message()
        if x:
            c.lon = x
        if y:
            c.lat = y
    elif len(name) == 3 and name[:2] == "gk":
        c = geo_capnp.GKCoord.new_message(meridianNo=int(name[2:3]))
        if x:
            c.r = x
        if y:
            c.h = y
    elif len(name) == 6 and name[:3] == "utm":
        zone = int(name[3:5])
        lb = name[-1]
        c = geo_capnp.UTMCoord.new_message(zone=zone, latitudeBand=lb)
        if x:
            c.r = x
        if y:
            c.h = y

    return c


def name_to_struct_type(name, default=None):
    lname = name.lower()
    c = default
    if lname == "2d" or lname == "xy":
        c = geo_capnp.Point2D
    elif lname == "latlon" or name == "wgs84":
        c = geo_capnp.LatLonCoord
    elif len(name) == 3 and name[:2] == "gk":
        c = geo_capnp.GKCoord
    elif len(name) == 6 and name[:3] == "utm":
        c = geo_capnp.UTMCoord
    return c


def get_xy(obj, default=None):
    c = default
    objs = obj.schema
    if objs == geo_capnp.Point2D.schema:
        c = (obj.x, obj.y)
    elif objs == geo_capnp.LatLonCoord.schema:
        c = (obj.lon, obj.lat)
    elif objs == geo_capnp.GKCoord.schema or objs == geo_capnp.UTMCoord.schema:
        c = (obj.r, obj.h)
    return c


def set_xy(obj, x, y):
    objs = obj.schema
    if objs == geo_capnp.Point2D.schema:
        obj.x = x
        obj.y = y
    elif objs == geo_capnp.LatLonCoord.schema:
        obj.lon = x
        obj.lat = y
    elif objs == geo_capnp.GKCoord.schema or objs == geo_capnp.UTMCoord.schema:
        obj.r = x
        obj.h = y


def name_to_crs(name, default=None):
    return {
        "latlon": CRS.from_epsg(geo_capnp.EPSG.wgs84),
        "wgs84": CRS.from_epsg(geo_capnp.EPSG.wgs84),
        "gk3": CRS.from_epsg(geo_capnp.EPSG.gk3),
        "gk4": CRS.from_epsg(geo_capnp.EPSG.gk4),
        "gk5": CRS.from_epsg(geo_capnp.EPSG.gk5),
        "utm21s": CRS.from_epsg(geo_capnp.EPSG.utm21S),
        "utm32n": CRS.from_epsg(geo_capnp.EPSG.utm32N)
    }.get(name.lower(), default)


def geo_coord_to_latlon(geo_coord):

    if not hasattr(geo_coord_to_latlon, "gk_cache"):
        geo_coord_to_latlon.gk_cache = {}
    if not hasattr(geo_coord_to_latlon, "utm_cache"):
        geo_coord_to_latlon.utm_cache = {}
    if not hasattr(geo_coord_to_latlon, "latlon_crs"):
        geo_coord_to_latlon.latlon_crs = name_to_crs("latlon")

    which = geo_coord.which()
    if which == "gk":
        meridian = geo_coord.gk.meridianNo
        if meridian not in geo_coord_to_latlon.gk_cache:
            gk_crs = CRS.from_epsg(geo_capnp.EPSG["gk" + str(meridian)])
            trans = geo_coord_to_latlon.gk_cache[meridian] = Transformer.from_crs(gk_crs, geo_coord_to_latlon.latlon_crs, always_xy=True)
        else:
            trans = geo_coord_to_latlon.gk_cache[meridian]
        lon, lat = trans.transform(geo_coord.gk.r, geo_coord.gk.h)
    elif which == "latlon":
        lat, lon = geo_coord.latlon.lat, geo_coord.latlon.lon
    elif which == "utm":
        utm_id = str(geo_coord.utm.zone) + geo_coord.utm.latitudeBand
        if utm_id not in geo_coord_to_latlon.utm_cache:
            utm_crs = CRS.from_epsg(geo_capnp.EPSG["utm" + utm_id])
            trans = geo_coord_to_latlon.utm_cache[utm_id] = Transformer.from_crs(utm_crs, geo_coord_to_latlon.latlon_crs, always_xy=True)
        else:
            trans = geo_coord_to_latlon.utm_cache[utm_id]
        lon, lat = trans.transform(geo_coord.utm.r, geo_coord.utm.h)

    return lat, lon



def transform_from_to_geo_coord(from_coord, to_name, default=None):
    if not hasattr(transform_from_to_geo_coord, "cache"):
        transform_from_to_geo_coord.cache = defaultdict(dict)

    to_coord = default
    from_coord_schema = from_coord.schema
    if from_coord_schema == geo_capnp.LatLonCoord.schema:
        trans = transform_from_to_geo_coord.cache["latlon"].setdefault(to_name, Transformer.from_crs(name_to_crs("latlon"), name_to_crs(to_name), always_xy=True))
        res = trans.transform(*get_xy(from_coord))
        to_coord = name_to_struct_instance(to_name, x=res[0], y=res[1])
    elif from_coord_schema == geo_capnp.UTMCoord.schema:
        utm_id = "utm" + str(from_coord.zone) + from_coord.latitudeBand
        trans = transform_from_to_geo_coord.cache[utm_id].setdefault(to_name, Transformer.from_crs(name_to_crs(utm_id), name_to_crs(to_name), always_xy=True))
        res = trans.transform(*get_xy(from_coord))
        to_coord = name_to_struct_instance(to_name, x=res[0], y=res[1])
    elif from_coord_schema == geo_capnp.GKCoord.schema:
        gk_id = "gk" + str(from_coord.meridian)
        trans = transform_from_to_geo_coord.cache[gk_id].setdefault(to_name, Transformer.from_crs(name_to_crs(gk_id), name_to_crs(to_name), always_xy=True))
        res = trans.transform(*get_xy(from_coord))
        to_coord = name_to_struct_instance(to_name, x=res[0], y=res[1])
    return to_coord
