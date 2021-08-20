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

#------------------------------------------------------------------------------

class ConnectionManager:

    def __init__(self):
        self._connections = {}

    def connect(self, sturdy_ref, cast_as = None):

        # we assume that a sturdy ref url looks always like capnp://hash-digest-or-insecure@host:port/sturdy-ref-token
        try:
            if sturdy_ref[:8] == "capnp://":
                rest = sturdy_ref[8:]
                hash_digest, rest = rest.split("@") if "@" in rest else (None, rest)
                host, rest = rest.split(":")
                port, sr_token = rest.split("/") if "/" in rest else (rest, None)

                host_port = "{}:{}".format(host, port)
                if host_port in self._connections:
                    bootstrap_cap = self._connections[host_port]
                else:
                    bootstrap_cap = capnp.TwoPartyClient(host_port).bootstrap()
                    self._connections[host_port] = bootstrap_cap

                if sr_token:
                    restorer = bootstrap_cap.cast_as(persistence_capnp.Restorer)
                    dyn_obj_reader = restorer.restore(sr_token).wait().cap
                    return dyn_obj_reader.as_interface(cast_as) if cast_as else dyn_obj_reader
                else:
                    return bootstrap_cap.cast_as(cast_as) if cast_as else bootstrap_cap

        except Exception as e:
            print(e)
            return None

#------------------------------------------------------------------------------

# interface Callback
class CallbackImpl(common_capnp.Callback.Server):

    def __init__(self, callback, *args, exec_callback_on_del=False, **kwargs):
        self._args = args
        self._kwargs = kwargs
        self._callback = callback
        self._already_called = False
        self._exec_callback_on_del = exec_callback_on_del

    def __del__(self):
        if self._exec_callback_on_del and not self._already_called:
            self._callback(*self._args, **self._kwargs)

    def call(self, _context, **kwargs): # call @0 ();
        self._callback(*self._args, **self._kwargs)
        self._already_called = True

#------------------------------------------------------------------------------

# interface CapHolder(Object)
class CapHolderImpl(common_capnp.CapHolder.Server):

    def __init__(self, cap, cleanup_func, cleanup_on_del=False):
        self._cap = cap
        self._cleanup_func = cleanup_func
        self._already_cleaned_up = False
        self._cleanup_on_del = cleanup_on_del

    def __del__(self):
        if self._cleanup_on_del and not self._already_cleaned_up:
            self.cleanup_func()

    def cap_context(self, context): # cap @0 () -> (object :Object);
        context.results.cap = self._cap

    def release_context(self, context): # release @1 ();
        self._cleanup_func()
        self._cleanup_on_del = True

#------------------------------------------------------------------------------

# interface PersistCapHolder(Object) extends(CapHolder(Object), Persistent.Persistent(Text, Text)) {
class PersistCapHolderImpl(common_capnp.PersistCapHolder.Server):

    def __init__(self, cap, sturdy_ref, cleanup_func, cleanup_on_del=False):
        self._cap = cap
        self._sturdy_ref = sturdy_ref
        self._cleanup_func = cleanup_func
        self._already_cleaned_up = False
        self._cleanup_on_del = cleanup_on_del

    def __del__(self):
        if self._cleanup_on_del and not self._already_cleaned_up:
            self.cleanup_func()

    def cap_context(self, context): # cap @0 () -> (object :Object);
        context.results.cap = self._cap

    def release_context(self, context): # release @1 ();
        self._cleanup_func()
        self._cleanup_on_del = True

    def save_context(self, context): # save @0 SaveParams -> SaveResults;
        context.results.sturdyRef = self._sturdy_ref

#------------------------------------------------------------------------------

def main():
    pass
    #server = capnp.TwoPartyServer("*:8000", bootstrap=AdminMasterImpl()) #UserMasterImpl(AdminMasterImpl()))
    #server.run_forever()

if __name__ == '__main__':
    main()