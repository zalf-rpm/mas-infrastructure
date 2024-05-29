# This Source Code Form is subject to the terms of the Mozilla Public
# License, v. 2.0. If a copy of the MPL was not distributed with this
# file, You can obtain one at http://mozilla.org/MPL/2.0/. */

# Authors:
# Michael Berg-Mohnicke <michael.berg-mohnicke@zalf.de>
# Susanne Schulz <susanne.schulz@zalf.de>
#
# Maintainers:
# Currently maintained by the authors.
#
# This file has been created at the Institute of
# Landscape Systems Analysis at the ZALF.
# Copyright (C: Leibniz Centre for Agricultural Landscape Research (ZALF)

import json
import numpy as np
from scipy.interpolate import NearestNDInterpolator
from pyproj import CRS, Transformer


def read_header(path_to_ascii_grid_file):
    """read metadata from esri ascii grid file"""
    metadata = {}
    header_str = ""
    with open(path_to_ascii_grid_file) as _:
        for i in range(0, 6):
            line = _.readline()
            header_str += line
            sline = [x for x in line.split() if len(x) > 0]
            if len(sline) > 1:
                metadata[sline[0].strip().lower()] = float(sline[1].strip())
    return metadata, header_str


def create_interpolator_from_rect_grid(grid, metadata, ignore_nodata=True, transform_func=None, row_col_value=False, no_points_to_values=False):
    """Create an interpolator from the given grid.
    It is assumed that the values in the grid have a rectangular projection
    so the interpolators underlying distance calculations make sense.
    grid - 2D (numpy) array of values
    metadata - data describing the grid
    transform_func - a function f(r, h) -> (r, h) to transform the r, h values before storing them"""

    rows, cols = grid.shape

    cellsize = int(metadata["cellsize"])
    xll = int(metadata["xllcorner"])
    yll = int(metadata["yllcorner"])
    nodata_value = metadata["nodata_value"]

    xll_center = xll + cellsize // 2
    yll_center = yll + cellsize // 2
    yul_center = yll_center + (rows - 1)*cellsize

    points = []
    values = []
    points_to_values = {}

    for row in range(rows):
        for col in range(cols):
            value = grid[row, col]
            if ignore_nodata and value == nodata_value:
                continue
            r = xll_center + col * cellsize
            h = yul_center - row * cellsize

            if transform_func:
                r, h = transform_func(r, h)

            points.append([r, h])
            values.append((row, col, value) if row_col_value else value)
            
            if not no_points_to_values:
                points_to_values[(r, h)] = value

    return NearestNDInterpolator(np.array(points), np.array(values)), None if no_points_to_values else points_to_values
    

def interpolate_from_latlon(interpolator, interpolator_crs):
    input_crs = CRS.from_epsg(4326)
    transformer = Transformer.from_crs(input_crs, interpolator_crs, always_xy=True)
    
    def interpol(lat, lon):
        r, h = transformer.transform(lon, lat)
        return interpolator(r, h)

    return interpol


def rect_coordinates_to_latlon(rect_crs, coords):
    latlon_crs = CRS.from_epsg(4326)
    transformer = Transformer.from_crs(rect_crs, latlon_crs, always_xy=True)
    
    rs, hs = zip(*coords)
    lons, lats = transformer.transform(list(rs), list(hs))
    latlons = list(zip(lats, lons))

    return latlons


def create_interpolator_from_ascii_grid(path_to_ascii_grid, datatype=int, no_of_header_rows=6, ignore_nodata=True):
    grid, metadata = load_grid_and_metadata_from_ascii_grid(path_to_ascii_grid, datatype, no_of_header_rows)
    return create_interpolator_from_rect_grid(grid, metadata, ignore_nodata)


def load_grid_and_metadata_from_ascii_grid(path_to_ascii_grid, datatype=int, no_of_header_rows=6):
    metadata, _ = read_header(path_to_ascii_grid)
    grid = np.loadtxt(path_to_ascii_grid, dtype=datatype, skiprows=no_of_header_rows)
    return (grid, metadata)


def create_climate_geoGrid_interpolator_from_json_file(path_to_latlon_to_rowcol_file, worldGeodeticSys84, geoTargetGrid, cdict):
    "create interpolator from json list of lat/lon to row/col mappings"
    with open(path_to_latlon_to_rowcol_file) as _:
        points = []
        values = []

        transformer = Transformer.from_crs(worldGeodeticSys84, geoTargetGrid, always_xy=True) 

        for latlon, rowcol in json.load(_):
            row, col = rowcol
            clat, clon = latlon
            try:
                cr_geoTargetGrid, ch_geoTargetGrid = transformer.transform(clon, clat)
                cdict[(row, col)] = (round(clat, 4), round(clon, 4))
                points.append([cr_geoTargetGrid, ch_geoTargetGrid])
                values.append((row, col))
                #print "row:", row, "col:", col, "clat:", clat, "clon:", clon, "h:", h, "r:", r, "val:", values[i]
            except:
                continue

        return NearestNDInterpolator(np.array(points), np.array(values))

