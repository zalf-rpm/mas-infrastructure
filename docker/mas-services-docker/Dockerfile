FROM alpine:3.20 AS build-env

# install git
RUN apk add --no-cache git

# get mas-infrastructure
RUN git clone https://github.com/zalf-rpm/mas-infrastructure.git


# default docker file for capnp python services
# try alpine image python:3.9.20-alpine3.20
FROM python:3.10-alpine3.20

# install capnproto
RUN apk add --no-cache capnproto
# install supervisor
RUN apk add --no-cache supervisor

RUN python -m pip install virtualenv
# install poetry
RUN apk add --no-cache poetry

# install python dependencies
RUN pip install --no-cache-dir --upgrade pip

RUN apk add --no-cache build-base

RUN apk add --no-cache \
    gcc \
    python3-dev \
    musl-dev \
    linux-headers \
    proj \
    proj-dev \
    proj-util \
    geos \
    geos-dev \
    libsodium

# copy capnp schemas
COPY --from=build-env /mas-infrastructure/capnproto_schemas /mas-infrastructure/capnproto_schemas

# copy the supervisor configuration
COPY --from=build-env /mas-infrastructure/docker/mas-services-docker/start_supervisor.sh /start.sh
COPY --from=build-env /mas-infrastructure/docker/mas-services-docker/supervisord.conf /etc/supervisor/supervisord.conf

# create a virtual environment folder
RUN mkdir -p /envs
ENV POETRY_VIRTUALENVS_PATH=/envs
WORKDIR /envs
# copy the poetry.lock and pyproject.toml
COPY --from=build-env /mas-infrastructure/src/python/zalfmas-services/poetry.lock poetry.lock
COPY --from=build-env /mas-infrastructure/src/python/zalfmas-services/pyproject.toml pyproject.toml
# install the dependencies as defined in the pyproject.toml
RUN poetry install --no-root

# copy python service scripts from build-env
RUN mkdir -p /mas-infrastructure/src/python/zalfmas-services/zalfmas_services/climate
COPY --from=build-env /mas-infrastructure/src/python/zalfmas-services/zalfmas_services/climate/dwd_core_ensemble_service.py  /mas-infrastructure/src/python/zalfmas-services/zalfmas_services/climate/dwd_core_ensemble_service.py 
COPY --from=build-env /mas-infrastructure/src/python/zalfmas-services/zalfmas_services/climate/csv_time_series_service.py    /mas-infrastructure/src/python/zalfmas-services/zalfmas_services/climate/csv_time_series_service.py 
COPY --from=build-env /mas-infrastructure/src/python/zalfmas-services/zalfmas_services/climate/dwd_germany_university_bonn_service.py /mas-infrastructure/src/python/zalfmas-services/zalfmas_services/climate/dwd_germany_university_bonn_service.py
COPY --from=build-env /mas-infrastructure/src/python/zalfmas-services/zalfmas_services/grid/ascii_grid.py /mas-infrastructure/src/python/zalfmas-services/zalfmas_services/grid/ascii_grid.py
COPY --from=build-env /mas-infrastructure/src/python/zalfmas-services/zalfmas_services/soil/sqlite_soil_data_service.py /mas-infrastructure/src/python/zalfmas-services/zalfmas_services/soil/sqlite_soil_data_service.py


# supervisor configuration for the services
# restart the service if it crashes
ENV auto_restart=true
# start csv time series service
ENV autostart_csv_time_series=false
ENV data_csv_time_series=/data/csv_time_series
# start dwd core ensemble service
ENV autostart_dwd_core_ensemble=false
ENV config_dwd_core_ensemble=/data/dwd_core_ensemble
# start dwd germany university bonn service
ENV autostart_dwd_germany_university_bonn=false
ENV config_dwd_germany_university_bonn=/data/dwd_germany_university_bonn/config.toml
# start ascii grid service 
# TODO: move parameters to config file
ENV autostart_ascii_grid=false
ENV data_ascii_grid=/data/ascii_grid.asc
ENV grid_crs=gk5
ENV grid_value_type=float
# start sqlite soil data service
ENV autostart_sqlite_soil_data=false
ENV db_sqlite_soil_data=/data/sqlite_soil_data.db
ENV grid_sqlite_soil_data=/data/sqlite_soil_data.asc

# non root user
RUN adduser -D myuser
USER myuser

# run the service
CMD ["sh", "/start.sh"]