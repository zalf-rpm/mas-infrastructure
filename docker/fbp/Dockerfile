
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
COPY ./capnproto_schemas /mas-infrastructure/capnproto_schemas

COPY ./docker/fbp/start_fbp.sh /start.sh
RUN mkdir -p /envs
ENV POETRY_VIRTUALENVS_PATH=/envs
WORKDIR /envs

COPY ./src/python/zalfmas-fbp/poetry.lock poetry.lock
COPY ./src/python/zalfmas-fbp/pyproject.toml pyproject.toml

RUN poetry install --no-root


# copy python scripts from build-env
RUN mkdir -p /mas-infrastructure/src/python/zalfmas-fbp/zalfmas_fbp/components
COPY ./src/python/zalfmas-fbp/zalfmas_fbp/components/console_output.py /mas-infrastructure/src/python/zalfmas-fbp/zalfmas_fbp/components/console_output.py
COPY ./src/python/zalfmas-fbp/zalfmas_fbp/components/read_file.py /mas-infrastructure/src/python/zalfmas-fbp/zalfmas_fbp/components/read_file.py


# non root user
RUN adduser -D myuser
USER myuser

# run the service
ENTRYPOINT ["sh", "/start.sh"]