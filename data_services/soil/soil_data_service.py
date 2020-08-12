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

import common_capnp
import sqlite3

import sys
import os
from datetime import date, timedelta
import numpy
import pandas as pd
import json
import time

#import argparse
import common
import capnp
capnp.add_import_hook(additional_paths=[
                      "../vcpkg/packages/capnproto_x64-windows-static/include/", "../capnproto_schemas/"])
import climate_data_capnp



print("local soil_io3.py")

def soil_parameters(con, profile_id):
    "return soil parameters from the database connection for given profile id"
    query = """
        select 
            id, 
            layer_depth, 
            soil_organic_carbon, 
            soil_organic_matter, 
            bulk_density, 
            raw_density,
            sand, 
            clay, 
            ph, 
            KA5_texture_class,
            permanent_wilting_point,
            field_capacity,
            saturation,
            soil_water_conductivity_coefficient,
            sceleton,
            soil_ammonium,
            soil_nitrate,
            c_n,
            initial_soil_moisture,
            layer_description,
            is_in_groundwater,
            is_impenetrable
        from soil_profile 
        where id = ? 
        order by id, layer_depth
    """

    layers = []
    prev_depth = 0

    con.row_factory = sqlite3.Row
    for row in con.cursor().execute(query, (profile_id,)):
        layer = {"type": "SoilParameters"}

        if row["layer_depth"] is not None:
            depth = float(row["layer_depth"])
            layer["Thickness"] = [depth - prev_depth, "m"]
            prev_depth = depth

        if row["KA5_texture_class"] is not None:
            layer["KA5TextureClass"] = row["KA5_texture_class"]

        if row["sand"] is not None:
            layer["Sand"] = [float(row["sand"]) / 100.0, "% [0-1]"]

        if row["clay"] is not None:
            layer["Clay"] = [float(row["clay"]) / 100.0, "% [0-1]"]

        if row["ph"] is not None:
            layer["pH"] = float(row["ph"])

        if row["sceleton"] is not None:
            layer["Sceleton"] = [float(row["sceleton"]) / 100.0, "vol% [0-1]"]

        if row["soil_organic_carbon"] is not None:
            layer["SoilOrganicCarbon"] = [float(row["soil_organic_carbon"]), "mass% [0-100]"]
        elif row["soil_organic_matter"] is not None:
            layer["SoilOrganicMatter"] = [float(row["soil_organic_matter"]) / 100.0, "mass% [0-1]"]


        if row["bulk_density"] is not None:
            layer["SoilBulkDensity"] = [float(row["bulk_density"]), "kg m-3"]
        elif row["raw_density"] is not None:
            layer["SoilRawDensity"] = [float(row["raw_density"]), "kg m-3"]

        if row["field_capacity"] is not None:
            layer["FieldCapacity"] = [float(row["field_capacity"]) / 100.0, "vol% [0-1]"]

        if row["permanent_wilting_point"] is not None:
            layer["PermanentWiltingPoint"] = [float(row["permanent_wilting_point"]) / 100.0, "vol% [0-1]"]

        if row["saturation"] is not None:
            layer["PoreVolume"] = [float(row["saturation"]) / 100.0, "vol% [0-1]"]

        if row["initial_soil_moisture"] is not None:
            layer["SoilMoisturePercentFC"] = [float(row["initial_soil_moisture"]), "% [0-100]"]

        if row["soil_water_conductivity_coefficient"] is not None:
            layer["Lambda"] = float(row["soil_water_conductivity_coefficient"])

        if row["soil_ammonium"] is not None:
            layer["SoilAmmonium"] = [float(row["soil_ammonium"]), "kg NH4-N m-3"]

        if row["soil_nitrate"] is not None:
            layer["SoilNitrate"] = [float(row["soil_nitrate"]), "kg NO3-N m-3"]

        if row["c_n"] is not None:
            layer["CN"] = float(row["c_n"])

        if row["layer_description"] is not None:
            layer["description"] = row["layer_description"]

        if row["is_in_groundwater"] is not None:
            layer["is_in_groundwater"] = int(row["is_in_groundwater"]) == 1

        if row["is_impenetrable"] is not None:
            layer["is_impenetrable"] = int(row["is_impenetrable"]) == 1

        found = lambda key: key in layer
        layer_is_ok = found("Thickness") \
            and (found("SoilOrganicCarbon") \
                    or found("SoilOrganicMatter")) \
            and (found("SoilBulkDensity") \
                    or found("SoilRawDensity")) \
            and (found("KA5TextureClass") \
                    or (found("Sand") and found("Clay")) \
                    or (found("PermanentWiltingPoint") \
                            and found("FieldCapacity") \
                            and found("PoreVolume") \
                            and found("Lambda")))

        if layer_is_ok:
            layers.append(layer)
        else:
            prev_depth -= depth
            print("Layer ", layer, " is incomplete. Skipping it!")

    return layers

#con = sqlite3.connect("soil.sqlite")
#x = soil_parameters(con, 197595)
#print(x)





def create_date(capnp_date):
    return date(capnp_date.year, capnp_date.month, capnp_date.day)


def create_capnp_date(py_date):
    return {
        "year": py_date.year if py_date else 0,
        "month": py_date.month if py_date else 0,
        "day": py_date.day if py_date else 0
    }


class CSV_TimeSeries(climate_data_capnp.Climate.TimeSeries.Server):

    def __init__(self, dataframe=None, path_to_csv_file=None, headers=None, start_date=None, end_date=None):
        if path_to_csv_file:
            dataframe = pd.read_csv(path_to_csv_file, skiprows=[
                                    1], index_col=0, delimiter=";")
        elif not dataframe:
            return
        self._df = dataframe.rename(columns={"windspeed": "wind"})
        self._data = None
        self._headers = headers
        self._start_date = start_date if start_date else \
            (date.fromisoformat(dataframe.index[0]) if len(
                dataframe) > 0 else None)
        self._end_date = end_date if end_date else \
            (date.fromisoformat(
                dataframe.index[-1]) if len(dataframe) > 0 else None)
        #self._real = realization

    def resolution_context(self, context):  # -> (resolution :TimeResolution);
        context.results.resolution = climate_data_capnp.Climate.TimeResolution.daily

    def range_context(self, context):  # -> (startDate :Date, endDate :Date);
        context.results.startDate = create_capnp_date(self._start_date)
        context.results.endDate = create_capnp_date(self._end_date)

    def header(self, **kwargs):  # () -> (header :List(Element));
        return self._df.columns.tolist()

    def data(self, **kwargs):  # () -> (data :List(List(Float32)));
        print("data requested")
        return self._df.to_numpy().tolist()

    def dataT(self, **kwargs):  # () -> (data :List(List(Float32)));
        print("dataT requested")
        return self._df.T.to_numpy().tolist()

    # (from :Date, to :Date) -> (timeSeries :TimeSeries);
    def subrange_context(self, context):
        from_date = create_date(getattr(context.params, "from"))
        to_date = create_date(context.params.to)

        sub_df = self._df.loc[str(from_date):str(to_date)]

        context.results.timeSeries = CSV_TimeSeries(
            dataframe=sub_df, headers=self._headers,
            start_date=from_date, end_date=to_date)

    # (elements :List(Element)) -> (timeSeries :TimeSeries);
    def subheader_context(self, context):
        sub_headers = [str(e) for e in context.params.elements]
        sub_df = self._df.loc[:, sub_headers]

        context.results.timeSeries = CSV_TimeSeries(
            dataframe=sub_df, headers=sub_headers,
            start_date=self._start_date, end_date=self._end_date)


class CSV_TimeSeries_CH(climate_data_capnp.Climate.Test.Server):

    def __init__(self, dataframe=None, path_to_csv_file=None, headers=None, start_date=None, end_date=None):
        self._ts = CSV_TimeSeries(dataframe=dataframe, path_to_csv_file=path_to_csv_file,
                                  headers=headers, start_date=start_date, end_date=end_date)

    def timeSeries(self, **kwargs):
        print("dataT requested")
        return common.CapHolderImpl(self._ts, "sturdy ref", lambda: print("cleanup called"))


def main():

    config = {
        "port": "11000",
        "server": "*",
        "path_to_csv_file": "climate-iso.csv"
    }
    # read commandline args only if script is invoked directly from commandline
    if len(sys.argv) > 1 and __name__ == "__main__":
        for arg in sys.argv[1:]:
            k, v = arg.split("=")
            if k in config:
                config[k] = v

    server = capnp.TwoPartyServer(config["server"] + ":" + config["port"],
                                  bootstrap=CSV_TimeSeries_CH(path_to_csv_file=config["path_to_csv_file"]))
                                  #bootstrap=CSV_TimeSeries(path_to_csv_file=config["path_to_csv_file"]))
    server.run_forever()


if __name__ == '__main__':
    main()
