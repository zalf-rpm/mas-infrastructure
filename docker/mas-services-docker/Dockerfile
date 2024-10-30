FROM alpine:3.20 AS build-env

# install git
RUN apk add --no-cache git

# get mas-infrastructure
RUN git clone https://github.com/zalf-rpm/mas-infrastructure.git


# default docker file for capnp python services
# try alpine image python:3.9.20-alpine3.20
FROM python:3.12-alpine3.20

# install capnproto
RUN apk add --no-cache capnproto
# install supervisor
RUN apk add --no-cache supervisor

RUN apk add --no-cache python3.9 python3-dev python3.9-venv 

# install python dependencies
RUN pip install --no-cache-dir --upgrade pip

COPY /mas-infrastructure/docker/mas-services-docker/requirements.txt /requirements.txt

RUN python3.9 -m venv /envs/python3.9
RUN source /envs/python3.9/bin/activate
RUN pip install -r requirements.txt

ENV auto_restart=true

ENV autostart_climate=true
ENV climate_sturdy_ref=none


# copy python scripts from build-env
RUN mkdir -p /mas-infrastructure/zalfmas_services/climate
COPY --from=build-env /mas-infrastructure/src/python/zalfmas-services/zalfmas_services/climate/dwd_core_ensemble_service.py  /mas-infrastructure/src/python/zalfmas_services/zalfmas_services/climate/dwd_core_ensemble_service.py 
COPY --from=build-env /mas-infrastructure/src/python/zalfmas-services/zalfmas_services/climate/csv_time_series_service.py  /mas-infrastructure/src/python/zalfmas-services/zalfmas_services/climate/csv_time_series_service.py 

# copy the supervisor configuration
COPY --from=build-env /mas-infrastructure/docker/mas-services-docker/start_supervisor.sh /start.sh
COPY --from=build-env /mas-infrastructure/docker/mas-services-docker/supervisord.conf /etc/supervisor/supervisord.conf

# copy capnp schemas
COPY --from=build-env /mas-infrastructure/capnproto_schemas /mas-infrastructure/capnproto_schemas

# non root user
RUN adduser -D myuser
USER myuser

# run the service
CMD ["sh", "/start.sh"]