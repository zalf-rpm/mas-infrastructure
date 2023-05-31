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
from collections import defaultdict
from datetime import date, timedelta
import json
import os
from pathlib import Path
import socket
import sys
import threading
import time
import uuid

PATH_TO_SCRIPT_DIR = Path(os.path.realpath(__file__)).parent
PATH_TO_REPO = PATH_TO_SCRIPT_DIR.parent.parent.parent
if str(PATH_TO_REPO) not in sys.path:
    sys.path.insert(1, str(PATH_TO_REPO))

PATH_TO_PYTHON_CODE = PATH_TO_REPO / "src/python"
if str(PATH_TO_PYTHON_CODE) not in sys.path:
    sys.path.insert(1, str(PATH_TO_PYTHON_CODE))

if str(PATH_TO_SCRIPT_DIR) in sys.path:
    import common as common
    import capnp_async_helpers as async_helpers
else:
    import common.common as common
    import common.capnp_async_helpers as async_helpers

PATH_TO_CAPNP_SCHEMAS = (PATH_TO_REPO / "capnproto_schemas").resolve()
abs_imports = [str(PATH_TO_CAPNP_SCHEMAS)]
common_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "common.capnp"), imports=abs_imports)
persistence_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "persistence.capnp"), imports=abs_imports)
service_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "service.capnp"), imports=abs_imports)
reg_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "registry.capnp"), imports=abs_imports)
storage_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "storage.capnp"), imports=abs_imports)


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

    def heartbeat_context(self, context):  # heartbeat @0 ();
        if self._timeout_prom:
            self._timeout_prom.cancel()
        self.make_timeout()

    def setTimeout_context(self, context):  # setTimeout @1 (seconds :UInt64);
        self._timeout = max(0, context.params.seconds)
        self.make_timeout()

    def stop_context(self, context):  # stop @2 ();
        def stop():
            capnp.remove_event_loop(ignore_errors=True)
            exit(0)

        if self._stop_action:
            print("Admin::stop message with stop_action")
            return self._stop_action().then(lambda proms: [proms, threading.Timer(5, stop).start()][0])
        else:
            print("Admin::stop message without")
            threading.Timer(5, stop).start()

    def identities_context(self, context):  # identities @3 () -> (infos :List(Common.IdInformation));
        infos = []
        for s in self._services:
            infos.append({"id": s.id, "name": s.name, "description": s.description})
        return infos

    def updateIdentity_context(self, context):  # updateIdentity @4 (oldId :Text, newInfo :Common.IdInformation);
        oid = context.params.oldId
        ni = context.params.newInfo
        for s in self._services:
            if s.id == oid:
                s.id = ni.id
                s.name = ni.name
                s.description = ni.description


async def async_init_and_run_service(name_to_service, host=None, port=0, serve_bootstrap=True, restorer=None,
                                     conn_man=None, name_to_service_srs={}, run_before_enter_eventloop=None,
                                     restorer_container_sr=None, **kwargs):
    port = port if port else 0

    # check for sturdy ref inputs
    if sys.stdin.isatty():
        try:
            reg_config = json.loads(sys.stdin.read())
            # print("read from stdin:", reg_config)
        except Exception as e:
            print("service.py: Error reading from sys.stdin. Exception:", e)
    else:
        reg_config = {}

    if not restorer:
        restorer = common.Restorer()
    if not conn_man:
        conn_man = async_helpers.ConnectionManager(restorer)

    if restorer and restorer_container_sr:
        restorer_container = await conn_man.try_connect(restorer_container_sr, cast_as=storage_capnp.Store.Container)
        if restorer_container:
            restorer.storage_container = restorer_container
            await restorer.init_vat_id_from_container().a_wait()
            if not port:
                await restorer.init_port_from_container().a_wait()
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
                                                 categoryId=reg_cat_id).a_wait()
                    unreg_action = r.unreg
                    rereg_sr = r.reregSR
                    admin.store_unreg_data(name, unreg_action, rereg_sr)
                    print("Registered", name, "in category '", reg_cat_id, "' as '", reg_name, "'.")
                else:
                    print("Couldn't connect to registrar at sturdy_ref:", reg_sr)
            except Exception as e:
                print("Error registering service name:", name, ". Exception:", e)

    if serve_bootstrap:
        server = await async_helpers.serve(host, port, restorer)

        for name, s in name_to_service.items():
            res = restorer.save_str(s, name_to_service_srs.get(name, None)).wait()
            name_to_service_srs[name] = res["sturdy_ref"]
            print("service:", name, "sr:", res["sturdy_ref"])
        print("restorer_sr:", restorer.sturdy_ref_str())

        await register_services(conn_man, admin, reg_config)
        if run_before_enter_eventloop:
            run_before_enter_eventloop()
        async with server:
            await server.serve_forever()
    else:
        await register_services(conn_man, admin, reg_config)
        if run_before_enter_eventloop:
            run_before_enter_eventloop()
        await conn_man.manage_forever()


def init_and_run_service(name_to_service, host="*", port=None, serve_bootstrap=True, restorer=None,
                         conn_man=None, name_to_service_srs={}, run_before_enter_eventloop=None,
                         restorer_container_sr=None, service_container_sr=None,
                         load_last_or_store_services_callback=None, **kwargs):
    host = host if host else "*"

    # check for sturdy ref inputs
    if not sys.stdin.isatty():
        try:
            reg_config = json.loads(sys.stdin.read())
            # print("read from stdin:", reg_config)
        except Exception as e:
            print("service.py: Error reading from sys.stdin. Exception:", e)
            pass
    else:
        reg_config = {}

    if not restorer:
        restorer = common.Restorer()
    if not conn_man:
        conn_man = common.ConnectionManager(restorer)

    if restorer and restorer_container_sr:
        restorer_container = conn_man.try_connect(restorer_container_sr, cast_as=storage_capnp.Store.Container)
        if restorer_container:
            restorer.storage_container = restorer_container
            restorer.init_vat_id_from_container().wait()
            if not port:
                restorer.init_port_from_container().wait()
                port = restorer.port

    # create and register admin with services
    admin = Admin(list(name_to_service.values()))
    for s in name_to_service.values():
        if isinstance(s, AdministrableService):
            s.admin = admin
    if "admin" not in name_to_service and admin not in name_to_service.values():
        name_to_service["admin"] = admin

    def register_services(conn_man, admin, reg_config):
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
                registrar = conn_man.try_connect(reg_sr, cast_as=reg_capnp.Registrar)
                if registrar and name in name_to_service:
                    r = registrar.register(cap=name_to_service[name], regName=reg_name, categoryId=reg_cat_id).wait()
                    unreg_action = r.unreg
                    rereg_sr = r.reregSR
                    admin.store_unreg_data(name, unreg_action, rereg_sr)
                    print("Registered", name, "in category '", reg_cat_id, "' as '", reg_name, "'.")
                else:
                    print("Couldn't connect to registrar at sturdy_ref:", reg_sr)
            except Exception as e:
                print("Error registering service name:", name, ". Exception:", e)

    addr = host + ((":" + str(port)) if port else "")
    if serve_bootstrap:
        server = capnp.TwoPartyServer(addr, bootstrap=restorer)
        restorer.port = port if port else server.port
        store_port_prom = restorer.store_port()
        if store_port_prom:
            store_port_prom.wait()

        service_container = conn_man.try_connect(service_container_sr,
                                                 cast_as=storage_capnp.Store.Container) if service_container_sr else None
        for name, s in name_to_service.items():
            if service_container and load_last_or_store_services_callback:
                service_sr = load_last_or_store_services_callback(service_container, name, s)
            else:
                service_sr = restorer.save_str(s, name_to_service_srs.get(name, None),
                                               store_sturdy_refs=service_container is not None).wait()["sturdy_ref"]
            name_to_service_srs[name] = service_sr
            print("service:", name, "sr:", service_sr)
        print("restorer_sr:", restorer.sturdy_ref_str())

        register_services(conn_man, admin, reg_config)
    else:
        register_services(conn_man, admin, reg_config)
        if run_before_enter_eventloop:
            run_before_enter_eventloop()
        capnp.wait_forever()

    if run_before_enter_eventloop:
        run_before_enter_eventloop()
    server.run_forever()

# --------------------------------------------------------------------------------------------
