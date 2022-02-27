#!/usr/bin/python
# -*- coding: UTF-8

# This Source Code Form is subject to the terms of the Mozilla Public
# License, v. 2.0. If a copy of the MPL was not distributed with this
# file, You can obtain one at http://mozilla.org/MPL/2.0/. */

# Authors:
# Michael Berg-Mohnicke <michael.berg-mohnicke@zalf.de>
#
# Maintainers:
# Currently maintained by the authors.
#
# This file has been created at the Institute of
# Landscape Systems Analysis at the ZALF.
# Copyright (C: Leibniz Centre for Agricultural Landscape Research (ZALF)

#from datetime import date, timedelta
#import json
import asyncio
import capnp
import os
from pathlib import Path
import socket
import sys
import time
import uuid

PATH_TO_REPO = Path(os.path.realpath(__file__)).parent.parent.parent.parent.parent
if str(PATH_TO_REPO) not in sys.path:
    sys.path.insert(1, str(PATH_TO_REPO))

PATH_TO_PYTHON_CODE = PATH_TO_REPO / "src/python"
if str(PATH_TO_PYTHON_CODE) not in sys.path:
    sys.path.insert(1, str(PATH_TO_PYTHON_CODE))

import common.capnp_async_helpers as async_helpers
import common.common as common
import services.climate.csv_file_based as csv_based

PATH_TO_CAPNP_SCHEMAS = PATH_TO_REPO / "capnproto_schemas"
abs_imports = [str(PATH_TO_CAPNP_SCHEMAS)]
climate_data_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "climate_data.capnp"), imports=abs_imports)
service_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "service.capnp"), imports=abs_imports)
csv_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "csv.capnp"), imports=abs_imports)
reg_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "registry.capnp"), imports=abs_imports)
persistence_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "persistence.capnp"), imports=abs_imports)

#------------------------------------------------------------------------------

class Restorer(persistence_capnp.Restorer.Server):

    def __init__(self):
        self._issued_sr_tokens = {} # sr_token to timeseries obj
        self._actions = []
        self._host = socket.getfqdn() #gethostname()
        self._port = None


    @property
    def port(self):
        return self._port
    
    @port.setter
    def port(self, p):
        self._port = p


    @property
    def host(self):
        return self._host
    
    @host.setter
    def host(self, h):
        self._host = h


    def sturdy_ref(self, sr_token=None):
        if sr_token:
            return "capnp://insecure@{host}:{port}/{sr_token}".format(host=self.host, port=self.port, sr_token=sr_token)
        else:
            return "capnp://insecure@{host}:{port}".format(host=self.host, port=self.port)


    def save(self, cap):
        sr_token = str(uuid.uuid4())
        self._issued_sr_tokens[sr_token] = cap
        unsave_sr_token = str(uuid.uuid4())
        unsave_action = common.Action(lambda: [self.unsave(sr_token), self.unsave(unsave_sr_token)]) 
        self._issued_sr_tokens[unsave_sr_token] = unsave_action
        return (self.sturdy_ref(sr_token), self.sturdy_ref(unsave_sr_token))


    def unsave(self, sr_token): 
        if sr_token in self._issued_sr_tokens:
            del self._issued_sr_tokens[sr_token]


    def restore_context(self, context): # restore @0 (srToken :Text) -> (cap :Capability);
        srt = context.params.srToken
        if srt in self._issued_sr_tokens:
            context.results.cap = self._issued_sr_tokens[srt]

#------------------------------------------------------------------------------

class Admin(service_capnp.Admin.Server):

    def __init__(self, service, timeout = 0):
        self._service = service
        self._timeout = timeout
        self._timeout_prom = None
        self.make_timeout()


    def make_timeout(self):
        if self._timeout > 0:
            self._timeout_prom = capnp.getTimer().after_delay(self._timeout * 10**9).then(lambda: exit(0))


    def heartbeat_context(self, context): # heartbeat @0 ();
        if self._timeout_prom:
            self._timeout_prom.cancel()
        self.make_timeout()


    def setTimeout_context(self, context):  # setTimeout @1 (seconds :UInt64);
        self._timeout = max(0, context.params.seconds)
        self.make_timeout()


    def stop_context(self, context): # stop @2 ();
        exit(0)


    def identity_context(self, context): # identity @3 () -> Common.IdInformation;
        rs = context.results
        rs.id = self._service.id
        rs.name = self._service.name
        rs.description = self._service.description


    def updateIdentity_context(self, context): # updateIdentity @4 Common.IdInformation;
        ps = context.params
        self._service.id = ps.id if ps.id else ""
        self._service.name = ps.name if ps.name else ""
        self._service.description = ps.description if ps.description else ""

#------------------------------------------------------------------------------

class Factory(climate_data_capnp.CSVTimeSeriesFactory.Server):

    def __init__(self, id=None, name=None, description=None):
        self._id = id if id else str(uuid.uuid4()) 
        self._name = name if name else "Unnamed Factory " + self._id 
        self._description = description if description else ""

        self._admin = None
        self._restorer = None


    @property
    def id(self):
        return self._id

    @id.setter
    def id(self, i):
        self._if = i


    @property
    def name(self):
        return self._name

    @name.setter
    def name(self, n):
        self._name = n


    @property
    def description(self):
        return self._description

    @description.setter
    def description(self, d):
        self._description = d


    @property
    def admin(self):
        return self._admin

    @admin.setter
    def admin(self, a):
        self._admin = a


    @property
    def restorer(self):
        return self._restorer

    @restorer.setter
    def restorer(self, r):
        self._restorer = r


    def refesh_timeout(self):
        if self.admin:
            self.admin.heartbeat_context(None)


    def info_context(self, context): # () -> IdInformation;
        r = context.results
        r.id = self.id
        r.name = self.name
        r.description = self.description
        self.refesh_timeout()


    def create_context(self, context): # create @0 (csvData :Text, config :CSVConfig) -> (timeseries :TimeSeries, error :Text);
        c = context.params.config
        if c is None:
            context.results.error = "No or wrong payload in message. Expected CSV payload."
        else:
            csv = context.params.csvData

            if csv is None:
                context.results.error = "no CSV data in message"
            else:    
                try:
                    header_map = {}
                    if c.headerMap:
                        for p in c.headerMap:
                            header_map[p.fst] = p.snd
                    
                    pandas_csv_config = {}
                    if c.sep:
                        pandas_csv_config["sep"] = c.sep

                    skip = []
                    for i in range(0, c.skipLinesToHeader):
                        skip.append(i)
                    header_line = 0 if len(skip) == 0 else skip[-1] + 1
                    for i in range(header_line + 1, header_line + 1 + c.skipLinesFromHeaderToData):
                        skip.append(i)
                    pandas_csv_config["skip_rows"] = skip

                    ts = csv_based.TimeSeries.from_csv_string(csv, header_map=header_map, pandas_csv_config=pandas_csv_config)
                    ts.persistence_service = self.restorer
                    #self._time_series_caps.append(ts)
                    context.results.timeseries = ts
                except Exception as e:
                    context.results.error = str(e)

                self.refesh_timeout()

#------------------------------------------------------------------------------

def main(serve_bootstrap=True, host="*", port=10000, reg_sturdy_ref=None):

    config = {
        "port": port,
        "host": host,
        "reg_sturdy_ref": reg_sturdy_ref,
        "serve_bootstrap": str(serve_bootstrap),
        "reg_category": "climate",
    }
    # read commandline args only if script is invoked directly from commandline
    if len(sys.argv) > 1 and __name__ == "__main__":
        for arg in sys.argv[1:]:
            k, v = arg.split("=")
            if k in config:
                config[k] = v
    print(config)

    conMan = common.ConnectionManager()

    service = Factory()
    admin = Admin(service)
    service.admin = admin
    restorer = Restorer()

    if config["reg_sturdy_ref"]:
        registrator = conMan.try_connect(config["reg_sturdy_ref"], cast_as=reg_capnp.Registrator)
        if registrator:
            unreg = registrator.register(ref=service, categoryId=config["reg_category"]).wait()
            print("Registered ", config["name"], "climate service.")
            #await unreg.unregister.unregister().a_wait()
        else:
            print("Couldn't connect to registrator at sturdy_ref:", config["reg_sturdy_ref"])

    addr = config["host"] + ((":" + str(config["port"])) if config["port"] else "")
    if config["serve_bootstrap"].lower() == "true":
        server = capnp.TwoPartyServer(addr, bootstrap=restorer)
        restorer.port = port if port else server.port
        admin_sr = restorer.save(admin)
        service_sr = restorer.save(service)
        print("admin_sr:", admin_sr)
        print("service_sr:", service_sr)
        print("restorer_sr:", restorer.sturdy_ref())
    else:
        capnp.wait_forever()
    server.run_forever()

#------------------------------------------------------------------------------
#"0.0.0.0"
async def async_main(serve_bootstrap=True, host=None, port=10000, reg_sturdy_ref=None):
    config = {
        "host": host,
        "port": port,
        "reg_sturdy_ref": reg_sturdy_ref,
        "serve_bootstrap": str(serve_bootstrap),
        "reg_category": "climate",
    }
    # read commandline args only if script is invoked directly from commandline
    if len(sys.argv) > 1 and __name__ == "__main__":
        for arg in sys.argv[1:]:
            k, v = arg.split("=")
            if k in config:
                config[k] = v
    print("config used:", config)

    conMan = async_helpers.ConnectionManager()

    service = Factory()
    admin = Admin(service)
    service.admin = admin
    restorer = Restorer()

    if config["reg_sturdy_ref"]:
        registrator = await conMan.try_connect(config["reg_sturdy_ref"], cast_as=reg_capnp.Registrator)
        if registrator:
            unreg = await registrator.register(ref=service, categoryId=config["reg_category"]).a_wait()
            print("Registered ", config["name"], "climate service.")
            #await unreg.unregister.unregister().a_wait()
        else:
            print("Couldn't connect to registrator at sturdy_ref:", config["reg_sturdy_ref"])

    if config["serve_bootstrap"].lower() == "true":
        server = await async_helpers.serve(config["host"], config["port"], restorer)

        admin_sr = restorer.save(admin)
        service_sr = restorer.save(service)
        print("admin_sr:", admin_sr)
        print("service_sr:", service_sr)
        print("restorer_sr:", restorer.sturdy_ref())

        async with server:
            await server.serve_forever()
    else:
        await conMan.manage_forever()

#------------------------------------------------------------------------------

if __name__ == '__main__':
    #main()
    asyncio.run(async_main())
