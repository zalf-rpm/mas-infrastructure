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
import logging
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
import common.capnp_async_helpers as async_helpers

PATH_TO_CAPNP_SCHEMAS = PATH_TO_REPO / "capnproto_schemas"
abs_imports = [str(PATH_TO_CAPNP_SCHEMAS)]
grid_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "grid.capnp"), imports=abs_imports) 
common_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "common.capnp"), imports=abs_imports) 
reg_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "registry.capnp"), imports=abs_imports)

#------------------------------------------------------------------------------

class Grid(grid_capnp.Grid.Server): 

    def __init__(self, path_to_ascii_grid, grid_crs, val_type, id=None, name=None, description=None):

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


    def closestValueAt(self, latlonCoord, ignoreNoData, resolution, agg, returnRowCols, **kwargs): # closestValueAt @0 (latlonCoord :Geo.LatLonCoord, ignoreNoData :Bool, resolution :UInt64, agg :Aggregation = none) -> (val :Value, tl :RowCol, br :RowCol);
        lat = latlonCoord.lat
        lon = latlonCoord.lon

        r, h = self._latlon_to_grid_crs.transform(lon, lat)
        if resolution < self._cellsize:
            if ignoreNoData:
                row, col, value = self._ignore_nodata_rowcol_interpol(r, h)
            else:
                row, col, value = self._include_nodata_rowcol_interpol(r, h)
            val = {"i": int(value)} if self._val_type == int else {"f": float(value)}
            rc = {"row": int(row), "col": int(col)}
            return (val, rc, rc) if returnRowCols else val


    def valueAt(self, row, col, resolution, agg, **kwargs): # valueAt @4 (row :UInt64, col :UInt64, resolution :UInt64, agg :Aggregation = none) -> (val :Value);
        
        if resolution <= self._cellsize and row >= 0 and row < self._nrows and col >= 0 and col < self._ncols:
            value = self._grid[row, col]
            if value == self._nodata:
                val = {"no": True}
            elif self._val_type == int:
                val = {"i": int(value)}
            else:
                val = {"f": float(value)}
            return val
        elif resolution % self._cellsize == 0 and row >= 0 and row < self._nrows and col >= 0 and col < self._ncols:

            cs = self._cellsize

            # what is outside of main cell
            rest_cellsize = resolution - cs
            # divide amongst sides (left/right, top/bottom)
            sidecell = rest_cellsize // 2
            d, mod_sidecell = divmod(sidecell, cs)

            cells = []

            # first create the fraction ring, if necessary
            if mod_sidecell > 0:
                # corners
                fraction = mod_sidecell / cs

                top_row = row - d - 1
                if top_row >= 0:

                    left_col = col - d - 1
                    if left_col >= 0:
                        # top left corner
                        cells.append((top_row, left_col, fraction*fraction)) 

                        # left side
                        for i in range(1, 1 + d + 1):
                            cells.append((top_row + i, left_col, fraction))

                    # top side
                    for i in range(1, 1 + d + 1):
                        cells.append((top_row, left_col + i, fraction))

                    right_col = col + d + 1
                    if right_col <= self._ncols - 1:
                        # top right corner
                        cells.append((top_row, right_col, fraction*fraction)) 

                        # right side
                        for i in range(1, 1 + d + 1):
                            cells.append((top_row + i, right_col, fraction))

                


                bottom_row = row + d + 1
                if bottom_row <= self._nrows - 1:

                    left_col = col - d - 1
                    if left_col >= 0:
                        # bottom left corner
                        cells.append((bottom_row, left_col, fraction*fraction)) 

                    # bottom side
                    for i in range(1, 1 + d + 1):
                        cells.append((bottom_row, left_col + i, fraction))

                    right_col = col + d + 1
                    if right_col <= self._ncols - 1:
                        # bottom right corner
                        cells.append((bottom_row, right_col, fraction*fraction)) 
    
            # create the inner full columns
            for row_i in range(-d, d + 1):
                r = row + row_i
                if r >= 0 and r <= self._nrows - 1:
                    for col_i in range(-d, d + 1):
                        c = col + col_i
                        if c >= 0 and c <= self._ncols - 1 and not (r == row and c == col):
                            cells.append((r, c, 1.0))

            
            value = self._nodata
            values = []
            for r, c, frac in cells:
                val = self._grid[r, c]
                if val == self._nodata:
                    continue
                values.append(val * frac)

            if len(values) > 0:
                if agg == "avg":
                    value = sum(values) / len(values)
                elif agg == "median":
                    sort(values)
                    value = values[len(values) // 2]
                elif agg == "min":
                    value = min(values)
                elif agg == "max":
                    value = max(values)
                elif agg == "sum":
                    value = sum(values)

            if value == self._nodata:
                val = {"no": True}
            elif self._val_type == int:
                val = {"i": int(value)}
            else:
                val = {"f": float(value)}
            return val            
                

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

async def async_main(path_to_ascii_grid, grid_crs, val_type, serve_bootstrap=False,
host="0.0.0.0", port=None, reg_sturdy_ref=None, id=None, name=None, description=None):

    config = {
        "host": host,
        "port": port,
        "path_to_ascii_grid": path_to_ascii_grid,
        "grid_crs": grid_crs,
        "val_type": val_type,
        "id": id,
        "name": name,
        "description": description,
        "reg_sturdy_ref": reg_sturdy_ref,
        "serve_bootstrap": str(serve_bootstrap),
        "reg_category": "soil",
    }
    # read commandline args only if script is invoked directly from commandline
    if len(sys.argv) > 1 and __name__ == "__main__":
        for arg in sys.argv[1:]:
            k, v = arg.split("=")
            if k in config:
                config[k] = v
    print("config used:", config)

    conMan = async_helpers.ConnectionManager()

    grid = Grid(path_to_ascii_grid=config["path_to_ascii_grid"], 
        grid_crs=geo.name_to_proj(config["grid_crs"]), val_type=int if config["val_type"] == "int" else float,
        id=config["id"], name=config["name"], description=config["description"])

    if config["reg_sturdy_ref"]:
        registrator = await conMan.try_connect(config["reg_sturdy_ref"], cast_as=reg_capnp.Registrator)
        if registrator:
            unreg = await registrator.register(ref=grid, categoryId=config["reg_category"]).a_wait()
            print("Registered ", config["name"], "soil service")
            #await unreg.unregister.unregister().a_wait()
        else:
            print("Couldn't connect to registrator at sturdy_ref:", config["reg_sturdy_ref"])

    if config["serve_bootstrap"].upper() == "TRUE":
        await async_helpers.serve_forever(config["host"], config["port"], grid)
    else:
        await conMan.manage_forever()

#------------------------------------------------------------------------------

def main(path_to_ascii_grid, grid_crs, val_type,serve_bootstrap=False,
host="0.0.0.0", port=None, reg_sturdy_ref=None, id=None, name=None, description=None):

    config = {
        "host": host,
        "port": port,
        "path_to_ascii_grid": path_to_ascii_grid,
        "grid_crs": grid_crs,
        "val_type": val_type,
        "id": id,
        "name": name,
        "description": description,
        "reg_sturdy_ref": reg_sturdy_ref,
        "serve_bootstrap": str(serve_bootstrap),
        "reg_category": "soil",
    }
    # read commandline args only if script is invoked directly from commandline
    if len(sys.argv) > 1 and __name__ == "__main__":
        for arg in sys.argv[1:]:
            k, v = arg.split("=")
            if k in config:
                config[k] = v
    print("config used:", config)

    grid = Grid(path_to_ascii_grid=config["path_to_ascii_grid"],
        grid_crs=geo.name_to_proj(config["grid_crs"]), val_type=int if config["val_type"] == "int" else float,
        id=config["id"], name=config["name"], description=config["description"])

    if config["serve_bootstrap"].upper() == "TRUE":
        server = capnp.TwoPartyServer(config["host"] + ":" + str(config["port"]), bootstrap=grid)
        server.run_forever()

#------------------------------------------------------------------------------

if __name__ == '__main__':
    grid = "data/geo/dem_1000_gk5.asc"
    crs = "gk5"

    #main(db, grid, crs, serve_bootstrap=True, port=10000)
    asyncio.run(async_main(grid, crs, "int", serve_bootstrap=True, port=16000))


