#!/usr/bin/python
# -*- coding: UTF-8

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

# remote debugging via commandline
# -m ptvsd --host 0.0.0.0 --port 14000 --wait

import asyncio
import capnp
import logging
import os
from pathlib import Path
import socket
import sqlite3
import sys
import uuid

PATH_TO_REPO = Path(os.path.realpath(__file__)).parent.parent.parent.parent
if str(PATH_TO_REPO) not in sys.path:
    sys.path.insert(1, str(PATH_TO_REPO))
PATH_TO_PYTHON_CODE = PATH_TO_REPO / "src/python"
if str(PATH_TO_PYTHON_CODE) not in sys.path:
    sys.path.insert(1, str(PATH_TO_PYTHON_CODE))

from pkgs.common import capnp_async_helpers as async_helpers

PATH_TO_CAPNP_SCHEMAS = PATH_TO_REPO / "capnproto_schemas"
abs_imports = [str(PATH_TO_CAPNP_SCHEMAS)]
common_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "common.capnp"), imports=abs_imports)
reg_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "service_registry.capnp"), imports=abs_imports)


class Registry(reg_capnp.Service.Registry.Server):

    def __init__(self, id=None, name=None, description=None):
        self._id = id if id else uuid.uuid4()
        self._name = name if name else self._uuid
        self._description = description if description else ""
        self._services = {}  # regToken -> {"entry": {regToken, type, service}, "unreg": unreg_cap}

    def info(self):  # info @0 () -> IdInformation;
        return {"id": self._id, "name": self._name, "description": self._description}

    def getAvailableServices_context(self, context):  # getAvailableServices @0 QueryType -> (services :List(Entry));
        all_services = []
        if context.params.which() == "all":
            for _, service_entry in self._services.items():
                all_services.append(service_entry["entry"])
        else:
            serviceType = str(context.params.type)
            for service_entry in filter(lambda v: v["entry"]["type"] == serviceType, self._services.values()):
                all_services.append(service_entry["entry"])

        context.results.services = all_services

    def getService_context(self, context):  # getService [Service] @1 (regToken :Text) -> (service :Service);
        regToken = context.params.regToken
        if regToken in self._services:
            return self._service[regToken]["entry"]["service"]

    def registerService_context(self,
                                context):  # registerService @2 (type :Type, service :Common.Identifiable) -> (regToken :Text, unreg :Common.Registry.Unregister);
        ps = context.params
        serviceType = ps.type
        regToken = str(uuid.uuid4())
        unreg = Unregister(regToken, lambda: self._services.pop(regToken, None))
        self._services[regToken] = {"entry": {"regToken": regToken, "type": serviceType, "service": ps.service},
                                    "unreg": unreg}
        context.results.unreg = unreg
        context.results.regToken = regToken
        print("registered service regToken:", regToken, "type:", serviceType)


class Unregister(common_capnp.Registry.Unregister.Server):

    def __init__(self, regToken, removeServiceFunc):
        self._regToken = regToken
        self._removeServiceFunc = removeServiceFunc

    def __del__(self):
        self._removeServiceFunc()
        print("__del__ id:", self._regToken)

    def unregister(self, **kwargs):  # unregister @0 ();
        self._removeServiceFunc()
        print("unregister id:", self._regToken)


def new_connection_factory(service):
    async def new_connection(reader, writer):
        server = async_helpers.Server(service)
        await server.myserver(reader, writer)

    return new_connection


async def async_main(server="0.0.0.0", port=10001, id=None, name="registry name", description=None):
    config = {
        "port": str(port),
        "server": server,
        "id": id,
        "name": name,
        "description": description
    }
    # read commandline args only if script is invoked directly from commandline
    if len(sys.argv) > 1 and __name__ == "__main__":
        for arg in sys.argv[1:]:
            k, v = arg.split("=")
            if k in config:
                config[k] = v
    print("config used:", config)

    registry = Registry(id=config["id"], name=config["name"], description=config["description"])

    # Handle both IPv4 and IPv6 cases
    # try:
    # print("Try IPv4")
    server = await asyncio.start_server(
        new_connection_factory(registry),
        server, port,
        family=socket.AF_INET
    )
    # except Exception:
    #    print("Try IPv6")
    #    server = await asyncio.start_server(
    #        new_connection_factory(registry),
    #        server, port,
    #        family=socket.AF_INET6
    #    )

    async with server:
        await server.serve_forever()


def no_async_main(server="*", port=6003, id=None, name="registry name", description=None):
    config = {
        "port": str(port),
        "server": server,
        "id": id,
        "name": name,
        "description": description
    }
    # read commandline args only if script is invoked directly from commandline
    if len(sys.argv) > 1 and __name__ == "__main__":
        for arg in sys.argv[1:]:
            k, v = arg.split("=")
            if k in config:
                config[k] = v

    server = capnp.TwoPartyServer(config["server"] + ":" + config["port"],
                                  bootstrap=Registry(id=config["id"], name=config["name"],
                                                     description=config["description"]))
    server.run_forever()


def main(server="*", port=10001, id=None, name=None, description=None, use_asyncio=True):
    if use_asyncio:
        asyncio.run(async_main(server=server, port=port, id=id, name=name, description=description))
    else:
        no_async_main(server=server, port=port, id=id, name=name, description=description)


if __name__ == '__main__':

    # asyncio.run(async_main())
    # exit()

    if len(sys.argv) > 1:
        async_no_async = sys.argv[1]
        if async_no_async == "async":
            sys.argv.pop(1)
            asyncio.run(async_main())
    else:
        no_async_main()

