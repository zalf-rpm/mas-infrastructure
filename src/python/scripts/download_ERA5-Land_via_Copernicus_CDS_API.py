import cdsapi
import os.path
import sys

config = {
    "path_to_out_dir": "/beegfs/common/data/climate/ERA5-Land/download/",
}

argv = sys.argv
allow_new_keys = False
print_config = True
if len(argv) > 1:
    for arg in argv[1:]:
        k, v = arg.split("=", maxsplit=1)
        if allow_new_keys or k in config:
            config[k] = v.lower() == "true" if v.lower() in ["true", "false"] else v
    if print_config:
        print(config)

c = cdsapi.Client()

inst_vars = [
    "10m_u_component_of_wind",
    "10m_v_component_of_wind",
    "2m_temperature",
    "2m_dewpoint_temperature",
    # "surface_pressure"
]

accu_vars = [
    "surface_solar_radiation_downwards",
    "total_precipitation"
]

for year in range(1950, 2023):
    for inst_var in inst_vars:
        download_filepath = f"{config['path_to_out_dir']}/{inst_var}_{year}.netcdf.zip"
        if os.path.exists(download_filepath):
            continue
        c.retrieve(
            "reanalysis-era5-land",
            {
                "variable": inst_var,
                "year": "2022",
                "month": list([f"{month:02}" for month in range(1, 13)]),
                "day": list([f"{day:02}" for day in range(1, 32)]),
                "time": [
                    "00:00", "01:00", "02:00",
                    "03:00", "04:00", "05:00",
                    "06:00", "07:00", "08:00",
                    "09:00", "10:00", "11:00",
                    "12:00", "13:00", "14:00",
                    "15:00", "16:00", "17:00",
                    "18:00", "19:00", "20:00",
                    "21:00", "22:00", "23:00",
                ],
                "format": "netcdf.zip",
            },
            download_filepath)
        print("downloaded:", download_filepath)

    for accu_var in accu_vars:
        download_filepath = f"{config['path_to_out_dir']}/{accu_var}_{year}.netcdf.zip"
        if os.path.exists(download_filepath):
            continue
        c.retrieve(
            "reanalysis-era5-land",
            {
                "variable": accu_var,
                "year": "2022",
                "month": list([f"{month:02}" for month in range(1, 13)]),
                "day": list([f"{day:02}" for day in range(1, 32)]),
                "time": [
                    "00:00", "23:00",
                ],
                "format": "netcdf.zip",
            },
            download_filepath)
        print("downloaded:", download_filepath)
