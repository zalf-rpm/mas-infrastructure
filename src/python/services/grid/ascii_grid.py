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

#remote debugging via commandline
#-m ptvsd --host 0.0.0.0 --port 14000 --wait

import asyncio
import capnp
import json
import logging
import math
import os
from pathlib import Path
from pyproj import CRS, Transformer
import sys
#import time
import uuid

PATH_TO_REPO = Path(os.path.realpath(__file__)).parent.parent.parent.parent.parent
if str(PATH_TO_REPO) not in sys.path:
    sys.path.insert(1, str(PATH_TO_REPO))

PATH_TO_PYTHON_CODE = PATH_TO_REPO / "src/python"
if str(PATH_TO_PYTHON_CODE) not in sys.path:
    sys.path.insert(1, str(PATH_TO_PYTHON_CODE))

import common.common as common
import common.rect_ascii_grid_management as grid_man
import common.geo as geo
import common.service as serv
import common.capnp_async_helpers as async_helpers

PATH_TO_CAPNP_SCHEMAS = PATH_TO_REPO / "capnproto_schemas"
abs_imports = [str(PATH_TO_CAPNP_SCHEMAS)]
grid_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "grid.capnp"), imports=abs_imports) 
common_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "common.capnp"), imports=abs_imports) 
reg_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "registry.capnp"), imports=abs_imports)

#------------------------------------------------------------------------------

class Grid(grid_capnp.Grid.Server, common.Identifiable, common.Persistable, serv.AdministrableService): 

    def __init__(self, path_to_ascii_grid, grid_crs, val_type, id=None, name=None, description=None, admin=None, restorer=None):
        common.Identifiable.__init__(self, id, name, description)
        common.Persistable.__init__(self, restorer)
        serv.AdministrableService.__init__(self, admin)

        self._id = str(id if id else uuid.uuid4())
        self._name = name if name else self._id
        self._description = description if description else ""

        self._path = path_to_ascii_grid
        self._grid_crs = grid_crs
        self._wgs84 = geo.name_to_proj("latlon")
        self._grid_crs_to_latlon = Transformer.from_crs(grid_crs, self._wgs84, always_xy=True)
        self._latlon_to_grid_crs = Transformer.from_crs(self._wgs84, grid_crs, always_xy=True)
        self._val_type = val_type

        self._grid, self._metadata = grid_man.load_grid_and_metadata_from_ascii_grid(path_to_ascii_grid, datatype=val_type)
        self._include_nodata_rowcol_interpol, _ = grid_man.create_interpolator_from_rect_grid(self._grid, self._metadata, ignore_nodata=False, row_col_value=True, no_points_to_values=True)
        self._ignore_nodata_rowcol_interpol, _ = grid_man.create_interpolator_from_rect_grid(self._grid, self._metadata, ignore_nodata=True, row_col_value=True, no_points_to_values=True)
        self._cellsize = int(self._metadata["cellsize"])
        self._nrows = int(self._metadata["nrows"])
        self._ncols = int(self._metadata["ncols"])
        self._nodata = int(self._metadata["nodata_value"]) if self._val_type == int else float(self._metadata["nodata_value"])
        self._xll = int(self._metadata["xllcorner"])
        self._yll = int(self._metadata["yllcorner"])


    def to_union(self, value):
        if value == self._nodata:
            val = {"no": True}
        elif self._val_type == int:
            val = {"i": int(value)}
        else:
            val = {"f": float(value)}
        return val            

    def closestValueAt(self, latlonCoord, ignoreNoData, resolution, agg, returnRowCols, includeAggParts, _context, **kwargs): # closestValueAt @0 (latlonCoord :Geo.LatLonCoord, ignoreNoData :Bool, resolution :UInt64, agg :Aggregation = none, includeAggParts :Bool = false) -> (val :Value, tl :RowCol, br :RowCol, aggParts :List(AggregationPart));
        lat = latlonCoord.lat
        lon = latlonCoord.lon

        r, h = self._latlon_to_grid_crs.transform(lon, lat)
        if ignoreNoData:
            row, col, value = self._ignore_nodata_rowcol_interpol(r, h)
        else:
            row, col, value = self._include_nodata_rowcol_interpol(r, h)

        if resolution <= self._cellsize:
            val = self.to_union(value)
            rc = {"row": int(row), "col": int(col)}
            return (val, rc, rc) if returnRowCols else val
        
        elif resolution % self._cellsize == 0:

            union_value, aggValues, tl, br = self.valueAtRowCol(int(row), int(col), resolution, agg, includeAggParts)

            if agg == "none":
                _context.results.aggParts = aggValues
                if returnRowCols:
                    _context.results.tl = tl
                    _context.results.br = br
                return
            elif includeAggParts:
                _context.results.aggParts = aggValues
                _context.results.val = union_value
                if returnRowCols:
                    _context.results.tl = tl
                    _context.results.br = br
                return
            else: 
                return (union_value, tl, br) if returnRowCols else union_value


    def valueAt(self, row, col, resolution, agg, includeAggParts, _context, **kwargs): # valueAt @4 (row :UInt64, col :UInt64, resolution :UInt64, agg :Aggregation = none, includeAggParts :Bool = false) -> (val :Value, aggParts :List(AggregationPart));

        if resolution <= self._cellsize and row >= 0 and row < self._nrows and col >= 0 and col < self._ncols:
            value = self._grid[row, col]
            return self.to_union(value)

        elif resolution % self._cellsize == 0 and row >= 0 and row < self._nrows and col >= 0 and col < self._ncols:

            union_value, aggValues, _, _ = self.valueAtRowCol(row, col, resolution, agg, includeAggParts)

            if agg == "none":
                _context.results.aggParts = aggValues
                return
            elif includeAggParts:
                _context.results.aggParts = aggValues
                _context.results.val = union_value
                return
            else: 
                return union_value


    def valueAtRowCol(self, row, col, resolution, agg, includeAggParts):

        cellsize = self._cellsize

        # what is outside of main cell
        boundary_size = resolution - cellsize
        # divide amongst sides (left/right, top/bottom)
        boundary_size_per_side = boundary_size // 2
        full_cell_count, rest_outer_boundary_size = divmod(boundary_size_per_side, cellsize)

        cells = [(row, col, 1.0, None)]
        tl = {"row": row, "col": col}
        br = {"row": row, "col": col}

        # first create the fraction ring, if necessary
        if rest_outer_boundary_size > 0:
            # corners
            fraction = rest_outer_boundary_size / cellsize

            outer_tl = None
            top_row = row - full_cell_count - 1
            if top_row >= 0:
                # top left corner
                left_col = col - full_cell_count - 1
                if left_col >= 0:
                    # store top left row/col
                    outer_tl = {"row": top_row, "col": left_col}
                    cells.append((top_row, left_col, fraction*fraction, (top_row + 1, left_col + 1))) 

                # top side
                for i in range(1, 1 + (2*full_cell_count) + 1):
                    left_i = left_col + i
                    if 0 <= left_i and left_i <= self._ncols - 1:
                        # if not done yet, store top left row/col
                        if outer_tl is None:
                            outer_tl = {"row": top_row, "col": left_i}
                        cells.append((top_row, left_i, fraction, (top_row + 1, left_i)))

                # top right corner
                right_col = col + full_cell_count + 1
                if right_col <= self._ncols - 1:
                    cells.append((top_row, right_col, fraction*fraction, (top_row + 1, right_col - 1))) 

            # left side
            left_col = col - full_cell_count - 1
            if left_col >= 0:
                for i in range(1, 1 + (2*full_cell_count) + 1):
                    top_i = top_row + i
                    if top_i >= 0:
                        if outer_tl is None:
                            outer_tl = {"row": top_i, "col": left_col}
                        cells.append((top_i, left_col, fraction, (top_i, left_col + 1)))

            if outer_tl is not None:
                tl = outer_tl

            outer_br = None
            # right side
            right_col = col + full_cell_count + 1
            if right_col <= self._ncols - 1:
                for i in range(1, 1 + (2*full_cell_count) + 1):
                    top_i = top_row + i
                    if top_i >= 0:
                        # store bottom right row/col
                        outer_br = {"row": top_i, "col": right_col}
                        cells.append((top_i, right_col, fraction, (top_i, right_col - 1)))


            bottom_row = row + full_cell_count + 1
            if bottom_row <= self._nrows - 1:
                # bottom left corner
                left_col = col - full_cell_count - 1
                if left_col >= 0:
                    cells.append((bottom_row, left_col, fraction*fraction, (bottom_row - 1, left_col + 1))) 

                # bottom side
                for i in range(1, 1 + (2*full_cell_count) + 1):
                    left_i = left_col + i
                    if 0 <= left_i and left_i <= self._ncols - 1:
                        # store bottom right row/col
                        outer_br = {"row": bottom_row, "col": left_i}
                        cells.append((bottom_row, left_i, fraction, (bottom_row - 1, left_i)))

                # bottom right corner
                right_col = col + full_cell_count + 1
                if right_col <= self._ncols - 1:
                    # store bottom right row/col
                    outer_br = {"row": bottom_row, "col": right_col}
                    cells.append((bottom_row, right_col, fraction*fraction, (bottom_row - 1, right_col - 1))) 

            if outer_br is not None:
                br = outer_br

        inner_tl = None
        inner_br = None
        # create the inner full columns
        for row_i in range(-full_cell_count, full_cell_count + 1):
            r = row + row_i
            if r >= 0 and r <= self._nrows - 1:
                for col_i in range(-full_cell_count, full_cell_count + 1):
                    c = col + col_i
                    if c >= 0 and c <= self._ncols - 1 and not (r == row and c == col):
                        # if not set previously, store top left row/col
                        if inner_tl is None:
                            inner_tl = {"row": r, "col": c}
                        # store bottom right row col
                        inner_br = {"row": r, "col": c}
                        cells.append((r, c, 1.0, None))
        
        if outer_tl is None and inner_tl is not None:
            tl = inner_tl
        if outer_br is None and inner_br is not None:
            br = inner_br
        

        value = self._nodata
        values = []
        weighted_values = []
        rc_to_val = {}
        rc_to_agg_val = {}
        # calc weighted values
        for r, c, frac, _ in cells:
            val = self._grid[r, c]
            if agg == "none" or includeAggParts:
                rc_to_agg_val[(r,c)] = {
                    "value": self.to_union(val), 
                    "rowCol": {"row": r, "col": c},
                    "areaFrac": frac,
                    "iValue": 0
                }
            if val == self._nodata:
                continue
            rc_to_val[(r,c)] = val
            values.append(val)
            weighted_values.append((val * frac, frac))

        # if the aggregation demands it, calc actually interpolated values for the outer cells
        interpolated_values = []
        if str(agg)[0] == "i":
            for r, c, frac, closest_full_cell in cells:
                
                if int(frac) == 1 or closest_full_cell is None:
                    interpolated_values.append(rc_to_val[(r,c)])
                    continue

                (fr,fc) = closest_full_cell

                dr = abs(r - fr)
                dc = abs(c - fc)

                full_dist = math.sqrt(dr * cellsize**2 + dc * cellsize**2)
                half_full_dist = math.sqrt(dr * (cellsize/2)**2 + dc * (cellsize/2)**2)
                half_short_dist = math.sqrt(dr * frac * (cellsize/2)**2 + dc * frac * (cellsize/2)**2)

                fval = rc_to_val[(fr,fc)]
                interpol_value = fval * (half_full_dist + half_short_dist) / full_dist

                interpolated_values.append(interpol_value)
                if includeAggParts:
                    rc_to_agg_val[(r,c)]["iValue"] = float(interpol_value)


        if len(values) > 0:
            if agg == "avg":
                # calc average
                value = sum(values) / len(values) if len(values) > 0 else 0
            elif agg == "wAvg":
                # calc weighted average https://www.indeed.com/career-advice/career-development/how-to-calculate-weighted-average
                value = sum(map(lambda t: t[0], weighted_values)) / sum(map(lambda t: t[1], weighted_values))
            elif agg == "iAvg":
                # calc interpolated average
                value = sum(interpolated_values) / len(interpolated_values) if len(interpolated_values) > 0 else 0
            elif agg == "median":
                # calc median
                values.sort()
                d, m = divmod(len(values), 2)
                if m == 0:
                    value = (values[d - 1] + values[d]) / 2
                else:
                    value = values[d]
            elif agg == "wMedian":
                # calc weighted median https://www.datablick.com/blog/2017/7/3/weighted-medians-for-weighted-data-in-tableau
                weighted_values.sort(key=lambda t: t[0])
                sum_fractions = sum(map(lambda t: t[1], weighted_values))
                running_fraction = 0
                for i, (weighted_value, frac) in enumerate(weighted_values):
                    running_fraction += frac
                    if running_fraction / sum_fractions > 0.5:
                        value = weighted_value
                        break
                    elif running_fraction / sum_fractions == 0.5:
                        value = (weighted_value + weighted_values[i+1][0]) / 2.0  # it should be impossible to have no i+1 if == 0.5
                        break
            elif agg == "iMedian":
                # calc interpolated median
                interpolated_values.sort()
                d, m = divmod(len(interpolated_values), 2)
                if m == 0:
                    value = (interpolated_values[d - 1] + interpolated_values[d]) / 2
                else:
                    value = interpolated_values[d]
            elif agg == "min":
                value = min(values)
            elif agg == "wMin":
                value = min(map(lambda t: t[0], weighted_values))
            elif agg == "iMin":
                value = min(interpolated_values)
            elif agg == "max":
                value = max(values)
            elif agg == "wMax":
                value = max(map(lambda t: t[0], weighted_values))
            elif agg == "iMax":
                value = max(interpolated_values)
            elif agg == "sum":
                value = sum(values)
            elif agg == "wSum":
                value = sum(map(lambda t: t[0], weighted_values))
            elif agg == "iSum":
                value = sum(interpolated_values)

        return (self.to_union(value), list(rc_to_agg_val.values()), tl, br)


    def resolution(self, **kwargs): # resolution @1 () -> (res :UInt64);
        return self._cellsize


    def dimension(self, **kwargs): # dimension @2 () -> (rows :UInt64, cols :UInt64);
        return (self._nrows, self._ncols)


    def noDataValue(self, **kwargs): # noDataValue @3 () -> (nodata :Value);
        return {"i": int(self._metadata["nodata_value"])} if self._val_type == int else {"f": float(self._metadata["nodata_value"])}


    def latLonBounds(self, useCellCenter, **kwargs): # latLonBounds @5 (useCellCenter :Bool = false) -> (tl :Geo.LatLonCoord, tr :Geo.LatLonCoord, br :Geo.LatLonCoord, bl :Geo.LatLonCoord);

        bl_x = self._xll + self._cellsize // 2 if useCellCenter else self._xll
        bl_y = self._yll + self._cellsize // 2 if useCellCenter else self._yll
        bl_lon, bl_lat = self._grid_crs_to_latlon.transform(bl_x, bl_y)
        
        tl_x = bl_x
        tl_y = bl_y + (self._nrows - 1)*self._cellsize
        tl_lon, tl_lat = self._grid_crs_to_latlon.transform(tl_x, tl_y)
        
        tr_x = tl_x + (self._ncols - 1)*self._cellsize
        tr_y = tl_y
        tr_lon, tr_lat = self._grid_crs_to_latlon.transform(tr_x, tr_y)

        br_x = tr_x    
        br_y = bl_y
        br_lon, br_lat = self._grid_crs_to_latlon.transform(br_x, br_y)

        return (
            {"lat": tl_lat, "lon": tl_lon},
            {"lat": tr_lat, "lon": tr_lon},
            {"lat": br_lat, "lon": br_lon},
            {"lat": bl_lat, "lon": bl_lon}
        )


#------------------------------------------------------------------------------

async def main(path_to_ascii_grid, grid_crs, val_type, serve_bootstrap=True, host=None, port=None, 
    id=None, name="Grid Service", description=None, use_async=False):

    config = {
        "path_to_ascii_grid": path_to_ascii_grid,
        "grid_crs": grid_crs,
        "val_type": val_type,
        "port": port, 
        "host": host,
        "id": id,
        "name": name,
        "description": description,
        "serve_bootstrap": serve_bootstrap,
        "use_async": use_async
    }
    # read commandline args only if script is invoked directly from commandline
    if len(sys.argv) > 1 and __name__ == "__main__":
        for arg in sys.argv[1:]:
            k, v = arg.split("=")
            if k in config:
                config[k] = bool(v) if v.lower() in ["true", "false"] else v 
    print(config)

    restorer = common.Restorer()
    service = Grid(path_to_ascii_grid=config["path_to_ascii_grid"], 
        grid_crs=geo.name_to_proj(config["grid_crs"]), val_type=int if config["val_type"] == "int" else float,
        id=config["id"], name=config["name"], description=config["description"], restorer=restorer)
    if config["use_async"]:
        await serv.async_init_and_run_service({"service": service}, config["host"], config["port"], 
        serve_bootstrap=config["serve_bootstrap"], restorer=restorer)
    else:
        
        serv.init_and_run_service({"service": service}, config["host"], config["port"], 
            serve_bootstrap=config["serve_bootstrap"], restorer=restorer)

#------------------------------------------------------------------------------

if __name__ == '__main__':
    #grid = "data/geo/dem_1000_31469_gk5.asc"
    grid = str(PATH_TO_REPO / "data/geo/slope_1000_31469_gk5.asc")
    crs = "gk5"
    asyncio.run(main(grid, crs, "float", serve_bootstrap=True, use_async=True)) 



