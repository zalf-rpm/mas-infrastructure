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

RUN mkdir -p /envs

ENV POETRY_VIRTUALENVS_PATH=/envs
WORKDIR /envs
# COPY --from=build-env /mas-infrastructure/docker/mas-services-docker/requirements.txt requirements.txt

COPY --from=build-env /mas-infrastructure/src/python/zalfmas-services/poetry.lock poetry.lock
COPY --from=build-env /mas-infrastructure/src/python/zalfmas-services/pyproject.toml pyproject.toml

# RUN virtualenv pythonservice
# RUN source pythonservice/bin/activate
# #RUN pip install --no-cache-dir pyproj
# RUN pip wheel --no-cache-dir --use-pep517 "pyproj (==3.7.0)"
# # RUN pip wheel --no-cache-dir --use-pep517 "psutil (==6.1.0)"

RUN poetry install --no-root

ENV auto_restart=true

ENV autostart_climate=true
ENV climate_sturdy_ref=none


# copy python scripts from build-env
RUN mkdir -p /mas-infrastructure/src/python/zalfmas-services/zalfmas_services/climate
COPY --from=build-env /mas-infrastructure/src/python/zalfmas-services/zalfmas_services/climate/dwd_core_ensemble_service.py  /mas-infrastructure/src/python/zalfmas-services/zalfmas_services/climate/dwd_core_ensemble_service.py 
COPY --from=build-env /mas-infrastructure/src/python/zalfmas-services/zalfmas_services/climate/csv_time_series_service.py    /mas-infrastructure/src/python/zalfmas-services/zalfmas_services/climate/csv_time_series_service.py 

# copy the supervisor configuration
COPY --from=build-env /mas-infrastructure/docker/mas-services-docker/start_supervisor.sh /start.sh
COPY --from=build-env /mas-infrastructure/docker/mas-services-docker/supervisord.conf /etc/supervisor/supervisord.conf

# copy capnp schemas
COPY --from=build-env /mas-infrastructure/capnproto_schemas /mas-infrastructure/capnproto_schemas

# flag to start each service
ENV autostart_csv_time_series=false
ENV autostart_dwd_core_ensemble=false

# restart the service if it crashes
ENV auto_restart=true

# non root user
RUN adduser -D myuser
USER myuser

# run the service
CMD ["sh", "/start.sh"]