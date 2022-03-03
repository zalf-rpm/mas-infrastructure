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
import time
import uuid

PATH_TO_REPO = Path(os.path.realpath(__file__)).parent.parent.parent.parent
if str(PATH_TO_REPO) not in sys.path:
    sys.path.insert(1, str(PATH_TO_REPO))

PATH_TO_PYTHON_CODE = PATH_TO_REPO / "src/python"
if str(PATH_TO_PYTHON_CODE) not in sys.path:
    sys.path.insert(1, str(PATH_TO_PYTHON_CODE))

PATH_TO_CAPNP_SCHEMAS = PATH_TO_REPO / "capnproto_schemas"
abs_imports = [str(PATH_TO_CAPNP_SCHEMAS)]
common_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "common.capnp"), imports=abs_imports) 
persistence_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "persistence.capnp"), imports=abs_imports) 
service_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "service.capnp"), imports=abs_imports)

#------------------------------------------------------------------------------

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

#------------------------------------------------------------------------------

class Admin(service_capnp.Admin.Server):

    def __init__(self, service, timeout = 0):
        self._service = service
        self._timeout = timeout
        self._timeout_prom = None
        self.make_timeout()
        self._unreg_sturdy_refs = {}  #name to (unreg action, rereg sturdy ref)


    def store_unreg_data(self, name, unreg_action, rereg_sr):
        self._unreg_sturdy_refs[name] = (unreg_action, rereg_sr)


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
