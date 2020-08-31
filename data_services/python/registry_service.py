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

import asyncio
import logging
import os
from pathlib import Path
import socket
import sqlite3
import sys
import uuid

PATH_TO_REPO = Path(os.path.realpath(__file__)).parent.parent.parent

if str(PATH_TO_REPO) not in sys.path:
    sys.path.insert(1, str(PATH_TO_REPO))
import common.python.capnp_async_helpers as async_helpers

import capnp
import capnproto_schemas.service_registry_capnp as reg_capnp

#------------------------------------------------------------------------------

class Registry(reg_capnp.Service.Registry.Server):

    def __init__(self, id=None, name=None, description=None):
        self._id = id if id else uuid.uuid4()
        self._name = name if name else self._uuid
        self._description = description if description else ""
        self._services = {} # id -> {type, service}


    def info(self): # info @0 () -> IdInformation;
        return {"id": self._id, "name": self._name, "description": self._description}


    def type_to_string(self, type_):
        which = type_.which()
        if which == "fixed":
            type_str = str(type_.fixed)    
            return {"str": type_str, "dict": {"fixed": type_str}}
        elif which == "other":
            type_str = type_.other
            return {"str": type_str, "dict": {"other": type_str}}
        else:
            type_str = ""
            return {"str": "", "dict": {"none": None}}


    def getAvailableServices_context(self, context): # getAvailableServices @0 QueryType -> (services :List(Entry));
        all_services = []
        if context.params.which() == "all":
            for _, service_entry in self._services.items():
                all_services.append(service_entry["entry"])
        else:
            tts = self.type_to_string(context.params.type)
            for service_entry in filter(lambda key, value: value["entry"]["type"] == tts["dict"], self._services):
                all_services.append(service_entry["entry"])

        context.results.services = all_services

    
    def getService(self, id, _context, **kwargs): # getService @1 (id :Text) -> Entry;
        if id in self._services:
            return self._service[id]["entry"]


    #async def check_id_and_store_service(self, pfp, context_results, type_str, service):
    #    try:
    #        info = await service.info()
    #        self._services[info.id] = {"type": type_str, "service": service}
    #        context_results.type = type_str
    #        context_results.service = service
    #    except Exception as e:
    #        print(e)
    #    pfp.fulfill()


    def registerService_context(self, context): # registerService @2 (type :Type, service :Capability) -> Entry; 
        ps = context.params
        tts = self.type_to_string(ps.type)
        regId = str(uuid.uuid4())
        unreg = Unregister(regId, self._services)
        self._services[regId] = {"entry": {"registerId": regId, "type": tts["dict"], "service": ps.service}, "unreg": unreg}
        context.results.unregister = unreg
        print("registered service regId:", regId, "type:", tts["str"])

#------------------------------------------------------------------------------

class Unregister(reg_capnp.Service.Registry.Unregister.Server):

    def __init__(self, id, services):
        self._id = id
        self._services = services


    def __del__(self):
        self._services.pop(self._id, None)
        print("__del__ id:", self._id)


    def unregister(self, **kwargs): # unregister @0 ();
        self._services.pop(self._id, None)
        print("unregister id:", self._id)

#------------------------------------------------------------------------------


def new_connection_factory(service):
    
    async def new_connection(reader, writer):
        server = async_helpers.Server(service)
        await server.myserver(reader, writer)

    return new_connection

#------------------------------------------------------------------------------

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
    try:
        print("Try IPv4")
        server = await asyncio.start_server(
            new_connection_factory(registry),
            server, port,
            family=socket.AF_INET
        )
    except Exception:
        print("Try IPv6")
        server = await asyncio.start_server(
            new_connection_factory(registry),
            server, port,
            family=socket.AF_INET6
        )

    async with server:
        await server.serve_forever()

#------------------------------------------------------------------------------

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
                                  bootstrap=Registry(id=config["id"], name=config["name"], description=config["description"]))
    server.run_forever()

#------------------------------------------------------------------------------

def main(server="*", port=10001, id=None, name=None, description=None, use_asyncio=True):
    if use_asyncio:
        asyncio.run(async_main(server=server, port=port, id=id, name=name, description=description))
    else:
        no_async_main(server=server, port=port, id=id, name=name, description=description)

#------------------------------------------------------------------------------

if __name__ == '__main__':

    asyncio.run(async_main())
    exit()

    if len(sys.argv) > 1:
        async_no_async = sys.argv[1]
        if async_no_async == "async":
            sys.argv.pop(1)
            asyncio.run(async_main())
    else:
        no_async_main()

#------------------------------------------------------------------------------
