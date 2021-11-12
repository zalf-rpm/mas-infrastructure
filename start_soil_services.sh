#!/usr/bin/sh

# start BUEK1000 soil service
~/.conda/envs/py38/bin/python src/python/services/soil/sqlite_soil_data_service.py \
path_to_sqlite_db=data/soil/buek1000.sqlite \
path_to_ascii_soil_grid=data/soil/buek1000_1000_gk5.asc \
grid_crs=gk5 \
id=buek1000 \
name=BUEK1000 \
port=10000 \
serve_bootstrap=true &

# start BUEK200 soil service
~/.conda/envs/py38/bin/python src/python/services/soil/sqlite_soil_data_service.py \
path_to_sqlite_db=data/soil/buek200.sqlite \
path_to_ascii_soil_grid=data/soil/buek200_1000_gk5.asc \
grid_crs=gk5 \
id=buek200 \
name=BUEK200 \
port=10001 \
serve_bootstrap=true &


