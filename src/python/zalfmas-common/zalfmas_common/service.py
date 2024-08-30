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

import capnp
import json
import os
from pathlib import Path
import sys
import threading

from zalfmas_common import common
import zalfmas_capnpschemas

sys.path.append(os.path.dirname(zalfmas_capnpschemas.__file__))
import common_capnp
import fbp_capnp
import persistence_capnp
import registry_capnp as reg_capnp
import service_capnp
import storage_capnp


class AdministrableService:

    def __init__(self, admin=None):
        self._admin = admin

    @property
    def admin(self):
        return self._admin

    @admin.setter
    def admin(self, a):
        self._admin = a

    def refesh_timeout(self):
        if self.admin:
            self.admin.heartbeat_context(None)


class Admin(service_capnp.Admin.Server, common.Identifiable):

    def __init__(self, services, id=None, name=None, description=None, timeout=0):
        common.Identifiable.__init__(self, id, name, description)

        self._services = services
        self._timeout = timeout
        self._timeout_prom = None
        self.make_timeout()
        self._unreg_sturdy_refs = {}  # name to (unreg action, rereg sturdy ref)

        self._stop_action = None

    @property
    def stop_action(self):
        return self._stop_action

    @stop_action.setter
    def stop_action(self, a):
        self._stop_action = a

    def store_unreg_data(self, name, unreg_action, rereg_sr):
        self._unreg_sturdy_refs[name] = (unreg_action, rereg_sr)

    def make_timeout(self):
        if self._timeout > 0:
            self._timeout_prom = capnp.getTimer().after_delay(self._timeout * 10 ** 9).then(lambda: exit(0))

    async def heartbeat(self, **kwargs):  # heartbeat @0 ();
        if self._timeout_prom:
            self._timeout_prom.cancel()
        self.make_timeout()

    async def setTimeout(self, seconds, **kwargs):  # setTimeout @1 (seconds :UInt64);
        self._timeout = max(0, seconds)
        self.make_timeout()

    async def stop(self, **kwargs):  # stop @2 ();
        def stop():
            capnp.remove_event_loop(ignore_errors=True)
            exit(0)

        if self._stop_action:
            print("Admin::stop message with stop_action")
            return self._stop_action().then(lambda proms: [proms, threading.Timer(5, stop).start()][0])
        else:
            print("Admin::stop message without")
            threading.Timer(5, stop).start()

    async def identities(self, **kwargs):  # identities @3 () -> (infos :List(Common.IdInformation));
        infos = []
        for s in self._services:
            infos.append({"id": s.id, "name": s.name, "description": s.description})
        return infos

    async def updateIdentity(self, oldId, newInfo, **kwargs):  # updateIdentity @4 (oldId :Text, newInfo :Common.IdInformation);
        for s in self._services:
            if s.id == oldId:
                s.id = newInfo.id
                s.name = newInfo.name
                s.description = newInfo.description


async def init_and_run_service(name_to_service, host=None, port=0, serve_bootstrap=True, restorer=None,
                               conn_man=None, name_to_service_srs=None, run_before_enter_eventloop=None,
                               restorer_container_sr=None, read_from_stdin=False, **kwargs):
    port = port if port else 0

    # check for sturdy ref inputs
    reg_config = {}
    if read_from_stdin and not sys.stdin.isatty():
        try:
            reg_config = json.loads(sys.stdin.read())
            # print("read from stdin:", reg_config)
        except Exception as e:
            print("service.py: Error reading from sys.stdin. Exception:", e)

    if not restorer:
        restorer = common.Restorer()
    if not conn_man:
        conn_man = common.ConnectionManager(restorer)
    if not name_to_service_srs:
        name_to_service_srs = {}

    if restorer and restorer_container_sr:
        restorer_container = await conn_man.try_connect(restorer_container_sr, cast_as=storage_capnp.Store.Container)
        if restorer_container:
            restorer.storage_container = restorer_container
            await restorer.init_vat_id_from_container()
            if not port:
                await restorer.init_port_from_container()
                port = restorer.port

    # create and register admin with services
    admin = Admin(list(name_to_service.values()))
    for s in name_to_service.values():
        if isinstance(s, AdministrableService):
            s.admin = admin
    if "admin" not in name_to_service and admin not in name_to_service.values():
        name_to_service["admin"] = admin

    async def register_services(conn_man, admin, reg_config):
        for name, data in reg_config.items():
            try:
                if isinstance(data, dict):
                    reg_sr = data["reg_sr"]
                    reg_name = data.get("reg_name", "")
                    reg_cat_id = data.get("cat_id", "")
                elif isinstance(data, str):
                    reg_sr = data
                    reg_name = ""
                    reg_cat_id = ""
                else:
                    continue
                print("trying to register name:", name, "data:", data)
                registrar = await conn_man.try_connect(reg_sr, cast_as=reg_capnp.Registrar)
                if registrar and name in name_to_service:
                    r = await registrar.register(cap=name_to_service[name], regName=reg_name,
                                                 categoryId=reg_cat_id)
                    unreg_action = r.unreg
                    rereg_sr = r.reregSR
                    admin.store_unreg_data(name, unreg_action, rereg_sr)
                    print("Registered", name, "in category '", reg_cat_id, "' as '", reg_name, "'.")
                else:
                    print("Couldn't connect to registrar at sturdy_ref:", reg_sr)
            except Exception as e:
                print("Error registering service name:", name, ". Exception:", e)

    async def new_connection(stream):
        await capnp.TwoPartyServer(stream, bootstrap=restorer).on_disconnect()

    if serve_bootstrap:
        server = await capnp.AsyncIoStream.create_server(new_connection, host, port)
        restorer.port = server.sockets[0].getsockname()[1]

        for name, s in name_to_service.items():
            res = await restorer.save_str(s, name_to_service_srs.get(name, None))
            name_to_service_srs[name] = res["sturdy_ref"]
            print("service:", name, "sr:", res["sturdy_ref"])
        print("restorer_sr:", restorer.sturdy_ref_str())

        await register_services(conn_man, admin, reg_config)
        if run_before_enter_eventloop:
            run_before_enter_eventloop()
        async with server:
            await server.serve_forever()
    #else:
    #    await register_services(conn_man, admin, reg_config)
    #    if run_before_enter_eventloop:
    #        run_before_enter_eventloop()
    #    await conn_man.manage_forever()


