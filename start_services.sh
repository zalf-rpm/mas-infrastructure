#!/usr/bin/sh

#cd src/python

#export DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=1

REG_PORT=$1
if [ -z $1 ]; then REG_PORT=10001; fi

# start registry service
~/.conda/envs/py38/bin/python  src/python/services/registry_service.py async port=$REG_PORT &

# start BUEK1000 soil service
~/.conda/envs/py38/bin/python src/python/services/soil/sqlite_soil_data_service.py async_register \
reg_port=$REG_PORT \
path_to_sqlite_db=data/soil/buek1000.sqlite \
path_to_ascii_soil_grid=data/soil/buek1000_1000_gk5.asc \
grid_crs=gk5 \
id=buek1000 \
name=BUEK1000 &

# start BUEK200 soil service
~/.conda/envs/py38/bin/python src/python/services/soil/sqlite_soil_data_service.py async_register \
reg_port=$REG_PORT \
path_to_sqlite_db=data/soil/buek200.sqlite \
path_to_ascii_soil_grid=data/soil/buek200_1000_gk5.asc \
grid_crs=gk5 \
id=buek200 \
name=BUEK200 &

#cd ../..

# start some climate services
# start DWD climate service
~/.conda/envs/py38/bin/python src/python/services/climate/dwd_germany_service.py async_register \
reg_port=$REG_PORT &

# start DWD Bonn climate service
~/.conda/envs/py38/bin/python src/python/services/climate/dwd_germany_university_bonn_service.py async_register \
reg_port=$REG_PORT &

# start DWD cmip cordex reklies climate service
~/.conda/envs/py38/bin/python src/python/services/climate/dwd_cmip_cordex_reklies_service.py async_register \
reg_port=$REG_PORT &

# start Isimip global climate service
~/.conda/envs/py38/bin/python src/python/services/climate/isimip_service.py async_register \
reg_port=$REG_PORT &

