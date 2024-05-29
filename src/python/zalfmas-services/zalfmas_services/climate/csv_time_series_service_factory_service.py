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

# from datetime import date, timedelta
# import json
import asyncio
import capnp
import os
from pathlib import Path
import sys

PATH_TO_REPO = Path(os.path.realpath(__file__)).parent.parent.parent.parent.parent
if str(PATH_TO_REPO) not in sys.path:
    sys.path.insert(1, str(PATH_TO_REPO))

PATH_TO_PYTHON_CODE = PATH_TO_REPO / "src/python"
if str(PATH_TO_PYTHON_CODE) not in sys.path:
    sys.path.insert(1, str(PATH_TO_PYTHON_CODE))

from pkgs.common import capnp_async_helpers as async_helpers
from pkgs.common import common
from pkgs.common import service as serv
from pkgs.climate import csv_file_based as csv_based

PATH_TO_CAPNP_SCHEMAS = PATH_TO_REPO / "capnproto_schemas"
abs_imports = [str(PATH_TO_CAPNP_SCHEMAS)]
climate_data_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "climate.capnp"), imports=abs_imports)
service_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "service.capnp"), imports=abs_imports)
reg_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "registry.capnp"), imports=abs_imports)
persistence_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "persistence.capnp"), imports=abs_imports)


class Factory(climate_data_capnp.CSVTimeSeriesFactory.Server, common.Factory):

    def __init__(self, id=None, name=None, description=None):
        common.Factory.__init__(self, id, name, description)

    def create_context(self,
                       context):  # create @0 (csvData :Text, config :CSVConfig) -> (timeseries :TimeSeries, error :Text);
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

                    ts = csv_based.TimeSeries.from_csv_string(csv, header_map=header_map,
                                                              pandas_csv_config=pandas_csv_config)
                    ts.persistence_service = self.restorer
                    # self._time_series_caps.append(ts)
                    context.results.timeseries = ts
                except Exception as e:
                    context.results.error = str(e)

                self.refesh_timeout()


def main(serve_bootstrap=True, host="*", port=10000, reg_sturdy_ref=None):
    config = {
        "port": port,
        "host": host,
        "reg_sturdy_ref": reg_sturdy_ref,
        "serve_bootstrap": str(serve_bootstrap),
        "reg_category": "climate",
    }
    # read commandline args only if script is invoked directly from commandline
    common.update_config(config, sys.argv, print_config=True, allow_new_keys=False)

    conMan = common.ConnectionManager()

    service = Factory()
    admin = serv.Admin(service)
    service.admin = admin
    restorer = common.Restorer()

    if config["reg_sturdy_ref"]:
        registrator = conMan.try_connect(config["reg_sturdy_ref"], cast_as=reg_capnp.Registrator)
        if registrator:
            unreg = registrator.register(ref=service, categoryId=config["reg_category"]).wait()
            print("Registered ", config["name"], "climate service.")
            # await unreg.unregister.unregister().a_wait()
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
    admin = serv.Admin(service)
    service.admin = admin
    restorer = common.Restorer()

    if config["reg_sturdy_ref"]:
        registrator = await conMan.try_connect(config["reg_sturdy_ref"], cast_as=reg_capnp.Registrator)
        if registrator:
            unreg = await registrator.register(ref=service, categoryId=config["reg_category"]).a_wait()
            print("Registered ", config["name"], "climate service.")
            # await unreg.unregister.unregister().a_wait()
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


if __name__ == '__main__':
    # main()
    asyncio.run(async_main())
