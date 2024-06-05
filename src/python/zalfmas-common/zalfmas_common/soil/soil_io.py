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

import sqlite3


def soil_parameters(con, profile_id):
    """compatibility function to get soil parameters for older monica python scripts"""

    layers = []
    skipped_depths = 0
    for layer in get_soil_profile(con, profile_id)[0][1]:
        def found(key):
            return key in layer

        layer_is_ok = found("Thickness") \
                      and (found("SoilOrganicCarbon")
                           or found("SoilOrganicMatter")) \
                      and (found("SoilBulkDensity")
                           or found("SoilRawDensity")) \
                      and (found("KA5TextureClass")
                           or (found("Sand") and found("Clay"))
                           or (found("PermanentWiltingPoint")
                               and found("FieldCapacity")
                               and found("PoreVolume")
                               and found("Lambda")))

        if layer_is_ok:
            layer["Thickness"][0] += skipped_depths
            skipped_depths = 0
            layers.append(layer)
        else:
            if len(layers) == 0:
                skipped_depths += layer["Thickness"][0]
            else:
                layers[-1]["Thickness"][0] += layer["Thickness"][0]
            print("Layer ", layer, " is incomplete. Skipping it!")

    return layers


def create_layer(row, prev_depth, only_raw_data, no_units=False):
    layer = {"type": "SoilParameters"}

    def add_units(value, unit):
        return value if no_units else [value, unit]

    if row["layer_depth"] is not None:
        depth = float(row["layer_depth"])
        layer["Thickness"] = add_units(depth - prev_depth, "m")
        prev_depth = depth

    if row["KA5_texture_class"] is not None:
        layer["KA5TextureClass"] = row["KA5_texture_class"]
    elif not only_raw_data and row["sand"] is not None and row["clay"] is not None:
        layer["KA5TextureClass"] = sand_and_clay_to_ka5_texture(float(row["sand"]) / 100.0, float(row["clay"]) / 100.0)

    if row["sand"] is not None:
        layer["Sand"] = add_units(float(row["sand"]) / 100.0, "% [0-1]")
    elif not only_raw_data and row["KA5_texture_class"] is not None:
        layer["Sand"] = add_units(ka5_texture_to_sand(row["KA5_texture_class"]), "% [0-1]")

    if row["clay"] is not None:
        layer["Clay"] = add_units(float(row["clay"]) / 100.0, "% [0-1]")
    elif not only_raw_data and row["KA5_texture_class"] is not None:
        layer["Clay"] = add_units(ka5_texture_to_clay(row["KA5_texture_class"]), "% [0-1]")

    if row["silt"] is not None:
        layer["Silt"] = add_units(float(row["silt"]) / 100.0, "% [0-1]")
    elif not only_raw_data and row["KA5_texture_class"] is not None:
        layer["Silt"] = add_units(ka5_texture_to_silt(row["KA5_texture_class"]), "% [0-1]")

    if row["ph"] is not None:
        layer["pH"] = float(row["ph"])

    if row["sceleton"] is not None:
        layer["Sceleton"] = add_units(float(row["sceleton"]) / 100.0, "vol% [0-1]")

    if row["soil_organic_carbon"] is not None:
        layer["SoilOrganicCarbon"] = add_units(float(row["soil_organic_carbon"]), "mass% [0-100]")
    elif not only_raw_data and row["soil_organic_matter"] is not None:
        layer["SoilOrganicCarbon"] = add_units(organic_matter_to_organic_carbon(float(row["soil_organic_matter"])),
                                               "mass% [0-100]")

    if row["soil_organic_matter"] is not None:
        layer["SoilOrganicMatter"] = add_units(float(row["soil_organic_matter"]) / 100.0, "mass% [0-1]")
    elif not only_raw_data and row["soil_organic_carbon"] is not None:
        layer["SoilOrganicMatter"] = add_units(
            organic_carbon_to_organic_matter(float(row["soil_organic_carbon"]) / 100.0), "mass% [0-1]")

    if row["bulk_density"] is not None:
        layer["SoilBulkDensity"] = add_units(float(row["bulk_density"]), "kg m-3")
    elif not only_raw_data and row["raw_density"] is not None and "Clay" in layer:
        layer["SoilBulkDensity"] = add_units(raw_density_to_bulk_density(float(row["raw_density"]), layer["Clay"][0]),
                                             "kg m-3")

    if row["raw_density"] is not None:
        layer["SoilRawDensity"] = add_units(float(row["raw_density"]), "kg m-3")
    elif not only_raw_data and row["bulk_density"] is not None and "Clay" in layer:
        layer["SoilRawDensity"] = add_units(bulk_density_to_raw_density(float(row["bulk_density"]), layer["Clay"][0]),
                                            "kg m-3")

    if row["field_capacity"] is not None:
        layer["FieldCapacity"] = add_units(float(row["field_capacity"]) / 100.0, "vol% [0-1]")

    if row["permanent_wilting_point"] is not None:
        layer["PermanentWiltingPoint"] = add_units(float(row["permanent_wilting_point"]) / 100.0, "vol% [0-1]")

    if row["saturation"] is not None:
        layer["PoreVolume"] = add_units(float(row["saturation"]) / 100.0, "vol% [0-1]")

    if row["initial_soil_moisture"] is not None:
        layer["SoilMoisturePercentFC"] = add_units(float(row["initial_soil_moisture"]), "% [0-100]")

    if row["soil_water_conductivity_coefficient"] is not None:
        layer["Lambda"] = float(row["soil_water_conductivity_coefficient"])

    if row["soil_ammonium"] is not None:
        layer["SoilAmmonium"] = add_units(float(row["soil_ammonium"]), "kg NH4-N m-3")

    if row["soil_nitrate"] is not None:
        layer["SoilNitrate"] = add_units(float(row["soil_nitrate"]), "kg NO3-N m-3")

    if row["c_n"] is not None:
        layer["CN"] = float(row["c_n"])

    if row["layer_description"] is not None:
        layer["description"] = row["layer_description"]

    if row["is_in_groundwater"] is not None:
        layer["is_in_groundwater"] = int(row["is_in_groundwater"]) == 1

    if row["is_impenetrable"] is not None:
        layer["is_impenetrable"] = int(row["is_impenetrable"]) == 1

    return layer, prev_depth


def get_soil_profile(con, profile_id=None, only_raw_data=True, no_units=False):
    """return soil parameters from the database connection for given profile id"""
    query = f"""select 
    id, 
    layer_depth, 
    soil_organic_carbon, 
    soil_organic_matter, 
    bulk_density, 
    raw_density,
    sand, 
    clay,
    silt, 
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
{" where id = ? " if profile_id else " "} 
order by id, layer_depth"""

    con.row_factory = sqlite3.Row
    rows = con.cursor().execute(query, (profile_id,)) if profile_id else con.cursor().execute(query)
    last_profile_id = None
    profiles = []
    layers = []
    prev_depth = 0
    for row in rows:
        id = int(row["id"])
        if not last_profile_id:
            last_profile_id = id
        if last_profile_id != id:
            profiles.append((last_profile_id, layers))
            last_profile_id = id
            layers = []
            prev_depth = 0

        layer, prev_depth = create_layer(row, prev_depth, only_raw_data, no_units=no_units)
        layers.append(layer)

    # store also last profile
    profiles.append((last_profile_id, layers))
    return profiles


def get_soil_profile_group(con, profile_group_id=None, only_raw_data=True, no_units=False):
    """return soil profile groups from the database connection for given profile group id"""
    query = f"""select 
polygon_id,
profile_id_in_polygon,
range_percentage_of_area,
avg_range_percentage_of_area,
layer_depth, 
soil_organic_carbon, 
soil_organic_matter, 
bulk_density, 
raw_density,
sand, 
clay, 
silt,
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
from soil_profile_all
{" where polygon_id = ? " if profile_group_id else " "} 
order by polygon_id, profile_id_in_polygon, layer_depth"""

    con.row_factory = sqlite3.Row
    rows = con.cursor().execute(query, (profile_group_id,)) if profile_group_id else con.cursor().execute(query)
    last_profile_group_id = None
    last_profile_id = None
    profile_groups = []
    profiles = []
    layers = []
    prev_depth = 0
    range_percentage = ""
    avg_percentage = 0
    for row in rows:
        group_id = int(row["polygon_id"])
        profile_id = int(row["profile_id_in_polygon"])
        range_percentage = row["range_percentage_of_area"]
        avg_percentage = float(row["avg_range_percentage_of_area"])
        if not last_profile_id:
            last_profile_id = profile_id
        if not last_profile_group_id:
            last_profile_group_id = group_id

        if profile_id != last_profile_id or group_id != last_profile_group_id:
            profiles.append({
                "id": last_profile_id,
                "layers": layers,
                "range_percentage_in_group": range_percentage,
                "avg_range_percentage_in_group": avg_percentage
            })
            last_profile_id = profile_id
            layers = []
            prev_depth = 0

        if group_id != last_profile_group_id:
            profile_groups.append((last_profile_group_id, profiles))
            last_profile_group_id = group_id
            profiles = []

        layer, prev_depth = create_layer(row, prev_depth, only_raw_data, no_units=no_units)
        layers.append(layer)

    # store also last profile and profile group 
    profiles.append({
        "id": last_profile_id,
        "layers": layers,
        "range_percentage_in_group": range_percentage,
        "avg_range_percentage_in_group": avg_percentage
    })
    profile_groups.append((last_profile_group_id, profiles))
    return profile_groups


def available_soil_parameters_group(con, table="soil_profile_all", id_col="polygon_id", only_raw_data=True):
    return available_soil_parameters(con, table=table, id_col=id_col, only_raw_data=only_raw_data)


def available_soil_parameters(con, table="soil_profile", id_col="id", only_raw_data=True):
    "return which soil parameters in the database are always there (mandatory) and which are sometimes there (optional) "

    query = "select count({}) as count from {} where {} is null"
    params = {
        "layer_depth": "Thickness",
        "soil_organic_carbon": "SoilOrganicCarbon",
        "soil_organic_matter": "SoilOrganicMatter",
        "bulk_density": "SoilBulkDensity",
        "raw_density": "SoilRawDensity",
        "sand": "Sand",
        "clay": "Clay",
        "silt": "Silt",
        "ph": "pH",
        "KA5_texture_class": "KA5TextureClass",
        "permanent_wilting_point": "PermanentWiltingPoint",
        "field_capacity": "FieldCapacity",
        "saturation": "PoreVolume",
        "soil_water_conductivity_coefficient": "Lambda",
        "sceleton": "Sceleton",
        "soil_ammonium": "SoilAmmonium",
        "soil_nitrate": "SoilNitrate",
        "c_n": "CN",
        "initial_soil_moisture": "SoilMoisturePercentFC",
        "layer_description": "description",
        "is_in_groundwater": "is_in_groundwater",
        "is_impenetrable": "is_impenetrable"
    }

    mandatory = []
    optional = []

    no_of_rows = 0
    con.row_factory = sqlite3.Row
    for row in con.cursor().execute(f"select count({id_col}) from {table}"):
        no_of_rows = int(row[0])

    for param in params.keys():
        for row in con.cursor().execute(f"select count({id_col}) as count from {table} where {param} is null"):
            row_count = int(row["count"])
            if row_count == 0:
                mandatory.append(params[param])
            elif row_count < no_of_rows:
                optional.append(params[param])

    # update mandatory list if we can derive some data
    if not only_raw_data:
        def move_from_optional(param, if_=True):
            if param in optional and if_:
                optional.remove(param)
                mandatory.append(param)

        move_from_optional("Sand", if_="KA5TextureClass" in mandatory)
        move_from_optional("Clay", if_="KA5TextureClass" in mandatory)
        move_from_optional("Silt", if_="KA5TextureClass" in mandatory)
        move_from_optional("KA5TextureClass", if_="Sand" in mandatory and "Clay" in mandatory)
        move_from_optional("SoilOrganicCarbon", if_="SoilOrganicMatter" in mandatory)
        move_from_optional("SoilOrganicMatter", if_="SoilOrganicCarbon" in mandatory)
        move_from_optional("SoilRawDensity", if_="SoilBulkDensity" in mandatory and "Clay" in mandatory)
        move_from_optional("SoilBulkDensity", if_="SoilRawDensity" in mandatory and "Clay" in mandatory)

    return {"mandatory": mandatory, "optional": optional}


SOM_to_C = 0.57  # [] converts soil organic matter to carbon


def organic_matter_to_organic_carbon(organic_matter):
    return organic_matter * SOM_to_C


def organic_carbon_to_organic_matter(organic_carbon):
    return organic_carbon / SOM_to_C


def raw_density_to_bulk_density(raw_density, clay):
    return ((raw_density / 1000.0) + (0.009 * 100.0 * clay)) * 1000.0


def bulk_density_to_raw_density(bulk_density, clay):
    return ((bulk_density / 1000.0) - (0.009 * 100.0 * clay)) * 1000.0


def humus_class_to_corg(humus_class):
    """convert humus class to soil organic carbon content"""
    hc_to_corg = {
        0: 0.0,
        1: 0.5 / 1.72,
        2: 1.5 / 1.72,
        3: 3.0 / 1.72,
        4: 6.0 / 1.72,
        5: 11.5 / 2.0,
        6: 17.5 / 2.0,
        7: 30.0 / 2.0
    }
    return hc_to_corg.get(humus_class, 0.0)


def bulk_density_class_to_raw_density(bulk_density_class, clay):
    """convert a bulk density class to an approximated raw density"""
    bdc_to_rd = {
        1: 1.3,
        2: 1.5,
        3: 1.7,
        4: 1.9,
        5: 2.1
    }
    xxx = bdc_to_rd.get(bulk_density_class, 0.0)
    return (xxx - (0.9 * clay)) * 1000.0  # *1000 = conversion from g cm-3 -> kg m-3


def sand_and_clay_to_lambda(sand, clay):
    """roughly calculate lambda value from sand and clay content"""
    return (2.0 * (sand * sand * 0.575)) + (clay * 0.1) + ((1.0 - sand - clay) * 0.35)


def percent_sand_and_clay_to_ka5_texture(sand: int, clay: int) -> str:
    """get a rough KA5 soil texture class from given sand and soil content"""

    soil_texture = ""
    silt: int = 100 - sand - clay

    if 0 <= silt < 10 and 0.0 <= clay < 5:
        soil_texture = "SS"  # SS silt 0-10% clay 0-5%
    elif 0 <= silt < 10 and 5 <= clay < 17:
        soil_texture = "ST2"  # ST2 silt 0-10% clay 5-17%
    elif 0 <= silt < 15 and 17 <= clay < 25:
        soil_texture = "ST3"  # ST3 silt 0-15% clay 17-25%
    elif 10 <= silt < 25 and 0 <= clay < 5:
        soil_texture = "SU2"  # SU2 silt 10-25% clay 0-5%
    elif 25 <= silt < 40 and 0 <= clay < 8:
        soil_texture = "SU3"  # SU3 silt 25-40% clay 0-8%
    elif 40 <= silt < 50 and 0 <= clay < 8:
        soil_texture = "SU4"  # SU4 silt 40-50% clay 0-8%
    elif 10 <= silt < 25 and 5 <= clay < 8:
        soil_texture = "SL2"  # SL2 silt 10-25% clay 5-8%
    elif 10 <= silt < 40 and 8 <= clay < 12:
        soil_texture = "SL3"  # SL3 silt 10-40% clay 8-12%
    elif 10 <= silt < 40 and 12 <= clay < 17:
        soil_texture = "SL4"  # SL4 silt 10-40% clay 12-17%
    elif 40 <= silt < 50 and 8 <= clay < 17:
        soil_texture = "SLU"  # SLU silt 40-50% clay 8-17%
    elif 40 <= silt < 50 and 17 <= clay < 25:
        soil_texture = "LS2"  # LS2 silt 40-50% clay 17-25%
    elif 30 <= silt < 40 and 17 <= clay < 25:
        soil_texture = "LS3"  # LS3 silt 30-40% clay 17-25%
    elif 15 <= silt < 30 and 17 <= clay < 25:
        soil_texture = "LS4"  # LS4 silt 15-30% clay 17-25%
    elif 30 <= silt < 50 and 25 <= clay < 35:
        soil_texture = "LT2"  # LT2 silt 30-50% clay 25-35%
    elif 30 <= silt < 50 and 35 <= clay < 45:
        soil_texture = "LT3"  # LT3 silt 30-50% clay 35-45%
    elif 15 <= silt < 30 and 25 <= clay < 45:
        soil_texture = "LTS"  # LTS silt 15-30% clay 25-45%
    elif 50 <= silt < 65 and 17 <= clay < 30:
        soil_texture = "LU"  # LU silt 50-65% clay 17-30%
    elif 50 <= silt < 65 and 8 <= clay < 17:
        soil_texture = "ULS"  # ULS silt 50-65% clay 8-17%
    elif 50 <= silt < 80 and 0 <= clay < 8:
        soil_texture = "US"  # US silt 50-80% clay 0-8%
    elif silt >= 80 and 0 <= clay < 8:
        soil_texture = "UU"  # UU silt >80% clay 0-8%
    elif silt >= 65 and 8 <= clay < 12:
        soil_texture = "UT2"  # UT2 silt >65% clay 8-12%
    elif silt >= 65 and 12 <= clay < 17:
        soil_texture = "UT3"  # UT3 silt >65% clay 12-17%
    elif silt >= 65 and 17 <= clay < 25:
        soil_texture = "UT4"  # UT4 silt >65% clay 17-25%
    elif 0 <= silt < 15 and 45 <= clay < 65:
        soil_texture = "TS2"  # TS2 silt 0-15% clay 45-65%
    elif 0 <= silt < 15 and 35 <= clay < 45:
        soil_texture = "TS3"  # TS3 silt 0-15% clay 35-45%
    elif 0 <= silt < 15 and 25 <= clay < 35:
        soil_texture = "TS4"  # TS4 silt 0-15% clay 25-35%
    elif 15 <= silt < 30 and 45 <= clay < 65:
        soil_texture = "TL"  # TL silt 15-30% clay 45-65%
    elif 50 <= silt < 65 and 30 <= clay < 45:
        soil_texture = "TU3"  # TU3 silt 50-65% clay 30-45%
    elif silt >= 30 and 45 <= clay < 65:
        soil_texture = "TU2"  # TU2 silt > 30% clay 45-65%
    elif silt >= 65 and clay >= 25:
        soil_texture = "TU4"  # TU4 silt > 65% clay >25%
    elif clay >= 65:
        soil_texture = "TT"  # TT clay > 65
    return soil_texture


def sand_and_clay_to_ka5_texture(sand: float, clay: float) -> str:
    return percent_sand_and_clay_to_ka5_texture(int(sand * 100.0), int(clay * 100.0))


def ka5_texture_to_sand(soil_type):
    "return sand content given the KA5 soil texture"
    return ka5_texture_to_sand_clay_silt(soil_type)["silt"]


def ka5_texture_to_clay(soil_type):
    "return clay content given the KA5 soil texture"
    return ka5_texture_to_sand_clay_silt(soil_type)["clay"]


def ka5_texture_to_silt(soil_type):
    "return clay content given the KA5 soil texture"
    return ka5_texture_to_sand_clay_silt(soil_type)["clay"]


def ka5_texture_to_sand_clay_silt(soil_type: str) -> dict:
    "return {sand, clay, silt} content given KA5 soil texture"

    soil_type = soil_type.upper()
    x = (0.66, 0.0)
    err = None

    if soil_type == "FS":
        x = (0.84, 0.02)
    elif soil_type == "FSMS":
        x = (0.86, 0.02)
    elif soil_type == "FSGS":
        x = (0.88, 0.02)
    elif soil_type == "GS":
        x = (0.93, 0.02)
    elif soil_type == "MSGS":
        x = (0.96, 0.02)
    elif soil_type == "MSFS":
        x = (0.93, 0.02)
    elif soil_type == "MS":
        x = (0.96, 0.02)
    elif soil_type == "SS":
        x = (0.93, 0.02)
    elif soil_type == "SL2":
        x = (0.76, 0.06)
    elif soil_type == "SL3":
        x = (0.65, 0.10)
    elif soil_type == "SL4":
        x = (0.60, 0.14)
    elif soil_type == "SLU":
        x = (0.43, 0.12)
    elif soil_type == "ST2":
        x = (0.84, 0.11)
    elif soil_type == "ST3":
        x = (0.71, 0.21)
    elif soil_type == "SU2":
        x = (0.80, 0.02)
    elif soil_type == "SU3":
        x = (0.63, 0.04)
    elif soil_type == "SU4":
        x = (0.56, 0.04)
    elif soil_type == "LS2":
        x = (0.34, 0.21)
    elif soil_type == "LS3":
        x = (0.44, 0.21)
    elif soil_type == "LS4":
        x = (0.56, 0.21)
    elif soil_type == "LT2":
        x = (0.30, 0.30)
    elif soil_type == "LT3":
        x = (0.20, 0.40)
    elif soil_type == "LTS":
        x = (0.42, 0.35)
    elif soil_type == "LU":
        x = (0.19, 0.23)
    elif soil_type == "UU":
        x = (0.10, 0.04)
    elif soil_type == "ULS":
        x = (0.30, 0.12)
    elif soil_type == "US":
        x = (0.31, 0.04)
    elif soil_type == "UT2":
        x = (0.13, 0.10)
    elif soil_type == "UT3":
        x = (0.11, 0.14)
    elif soil_type == "UT4":
        x = (0.09, 0.21)
    elif soil_type == "UTL":
        x = (0.19, 0.23)
    elif soil_type == "TT":
        x = (0.17, 0.82)
    elif soil_type == "TL":
        x = (0.17, 0.55)
    elif soil_type == "TU2":
        x = (0.12, 0.55)
    elif soil_type == "TU3":
        x = (0.10, 0.37)
    elif soil_type == "TS3":
        x = (0.52, 0.40)
    elif soil_type == "TS2":
        x = (0.37, 0.55)
    elif soil_type == "TS4":
        x = (0.62, 0.30)
    elif soil_type == "TU4":
        x = (0.05, 0.30)
    elif soil_type == "L":
        x = (0.35, 0.31)
    elif soil_type == "S":
        x = (0.93, 0.02)
    elif soil_type == "U":
        x = (0.10, 0.04)
    elif soil_type == "T":
        x = (0.17, 0.82)
    elif soil_type == "HZ1":
        x = (0.30, 0.15)
    elif soil_type == "HZ2":
        x = (0.30, 0.15)
    elif soil_type == "HZ3":
        x = (0.30, 0.15)
    elif soil_type == "HH":
        x = (0.15, 0.1)
    elif soil_type == "HN":
        x = (0.15, 0.1)
    else:
        err = "Unknown soil type: " + soil_type + "!"

    return {"sand": x[0], "clay": x[1], "silt": 1 - x[0] - x[1], "err": err}
