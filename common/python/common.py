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

import sys
import os
from datetime import date, timedelta
import json
import time
from collections import defaultdict
import uuid

import capnp
capnp.add_import_hook(additional_paths=["capnproto_schemas"])
import common_capnp
import geo_coord_capnp as geo

#------------------------------------------------------------------------------

def name_to_proj(name, default=None):
    return {
        "wgs84": Proj(init="epsg:{}".format(geo.Geo.EPSG.wgs84)),
        "latlon": Proj(init="epsg:{}".format(geo.Geo.EPSG.wgs84)),
        "gk3": Proj(init="epsg:{}".format(geo.Geo.EPSG.gk3)),
        "gk5": Proj(init="epsg:{}".format(geo.Geo.EPSG.gk5)),
        "utm21S": Proj(init="epsg:{}".format(geo.Geo.EPSG.utm21S)),
        "utm32N": Proj(init="epsg:{}".format(geo.Geo.EPSG.utm32N))
    }.get(name, default)

#------------------------------------------------------------------------------


# interface Callback
class CallbackImpl(common_capnp.Common.Callback.Server):

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
class CapHolderImpl(common_capnp.Common.CapHolder.Server):

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
class PersistCapHolderImpl(common_capnp.Common.PersistCapHolder.Server):

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