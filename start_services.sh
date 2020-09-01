#!/usr/bin/sh

cd src/python

# start registry service
~/.conda/envs/py38/bin/python services/registry_service.py async port=10001 &

# start BUEK1000 soil service
~/.conda/envs/py38/bin/python services/soil/sqlite_soil_data_service.py async_register \
reg_port=10001 \
path_to_sqlite_db=../../data/soil/buek1000.sqlite \
path_to_ascii_soil_grid=../../data/soil/buek1000_1000_gk5.asc \
grid_crs=gk5 \
id=buek1000 \
name=BUEK1000 &

# start BUEK200 soil service
~/.conda/envs/py38/bin/python services/soil/sqlite_soil_data_service.py async_register \
reg_port=10001 \
path_to_sqlite_db=../../data/soil/buek200.sqlite \
path_to_ascii_soil_grid=../../data/soil/buek200_1000_gk5.asc \
grid_crs=gk5 \
id=buek200 \
name=BUEK200 &

cd ../..



