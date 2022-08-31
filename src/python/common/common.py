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

PATH_TO_CAPNP_SCHEMAS = (PATH_TO_REPO / "capnproto_schemas").resolve()
abs_imports = [str(PATH_TO_CAPNP_SCHEMAS)]
common_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "common.capnp"), imports=abs_imports) 
persistence_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "persistence.capnp"), imports=abs_imports) 

#------------------------------------------------------------------------------

def get_fbp_attr(ip, attr_name):
    if ip.attributes and attr_name:
        for kv in ip.attributes:
            if kv.key == attr_name:
                return kv.value
    return None


def copy_fbp_attr(old_ip, new_ip, new_attr_name=None, new_attr_value=None):
    if not old_ip.attributes and not new_attr_name:
        return

    # if there find index of attribute to be set
    new_index = -1
    if old_ip.attributes and new_attr_name:
        for i, kv in enumerate(old_ip.attributes):
            if kv.key == new_attr_name:
                new_index = i
                break

    # init space for attributes in new IP
    new_attrs_size = len(old_ip.attributes) if old_ip.attributes else 0
    if new_index < 0 and new_attr_name and new_attr_value:
        new_attrs_size += 1
        new_index = new_attrs_size - 1
    attrs = new_ip.init("attributes", new_attrs_size)

    # copy old attributes
    if old_ip.attributes:
        for i, kv in enumerate(old_ip.attributes):
            if i != new_index:
                attrs[i].key = kv.key
                attrs[i].value = kv.value

    # set new attribute if there
    if new_index > -1:
        attrs[new_index].key = new_attr_name
        attrs[new_index].value = new_attr_value

#------------------------------------------------------------------------------

def update_config(config, argv, print_config=False, allow_new_keys=False):
    if len(argv) > 1:
        for arg in argv[1:]:
            k, v = arg.split("=", maxsplit=1)
            if not allow_new_keys and k in config:
                config[k] = v.lower() == "true" if v.lower() in ["true", "false"] else v 
        if print_config:
            print(config)

#------------------------------------------------------------------------------

class Restorer(persistence_capnp.Restorer.Server):

    def __init__(self):
        self._issued_sr_tokens = {} # sr_token to capability
        self._actions = []
        self._host = socket.gethostbyname(socket.gethostname()) #socket.getfqdn() #gethostname()
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


    def save(self, cap, sr_token=None, create_unsave=True):
        sr_token = sr_token if sr_token else str(uuid.uuid4())
        self._issued_sr_tokens[sr_token] = cap
        if create_unsave:
            unsave_sr_token = str(uuid.uuid4())
            unsave_action = Action(lambda: [self.unsave(sr_token), self.unsave(unsave_sr_token)]) 
            self._issued_sr_tokens[unsave_sr_token] = unsave_action
        return (self.sturdy_ref(sr_token), self.sturdy_ref(unsave_sr_token) if create_unsave else None)


    def unsave(self, sr_token): 
        if sr_token in self._issued_sr_tokens:
            del self._issued_sr_tokens[sr_token]


    def restore_context(self, context): # restore @0 (srToken :Text) -> (cap :Capability);
        srt = context.params.srToken
        if srt in self._issued_sr_tokens:
            context.results.cap = self._issued_sr_tokens[srt]

#------------------------------------------------------------------------------

class Identifiable(common_capnp.Identifiable.Server):

    def __init__(self, id=None, name=None, description=None):
        self._id = id if id else str(uuid.uuid4()) 
        self._name = name if name else "Unnamed_{}".format(self._id)
        self._description = description if description else ""


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


    def info_context(self, context): # () -> IdInformation;
        r = context.results
        r.id = self.id
        r.name = self.name
        r.description = self.description

#------------------------------------------------------------------------------

class Factory(Identifiable):

    def __init__(self, id=None, name=None, description=None):
        Identifiable.__init__(self, id, name, description)

        self._admin = None
        self._restorer = None


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

#------------------------------------------------------------------------------

class Persistable(persistence_capnp.Persistent.Server):

    def __init__(self, restorer=None):
        self._restorer = restorer


    @property
    def restorer(self):
        return self._restorer

    @restorer.setter
    def restorer(self, r):
        self._restorer = r


    def save_context(self, context): # save @0 () -> (sturdyRef :Text, unsaveSR :Text);
        if self.restorer:
            sr, unsave_sr = self.restorer.save(self)
            context.results.sturdyRef = sr
            context.results.unsaveSR = unsave_sr

#------------------------------------------------------------------------------

class ConnectionManager:

    def __init__(self):
        self._connections = {}


    def connect(self, sturdy_ref, cast_as = None):

        # we assume that a sturdy ref url looks always like capnp://hash-digest-or-insecure@host:port/sturdy-ref-token
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


    def try_connect(self, sturdy_ref, cast_as = None, retry_count=10, retry_secs=5, print_retry_msgs=True):
        while True:
            try:
                return self.connect(sturdy_ref, cast_as=cast_as)
            except Exception as e:
                print(e)
                if retry_count == 0:
                    if print_retry_msgs:
                        print("Couldn't connect to sturdy_ref at {}!".format(sturdy_ref))
                    return None
                retry_count -= 1
                if print_retry_msgs:
                    print("Trying to connect to {} again in {} secs!".format(sturdy_ref, retry_secs))
                time.sleep(retry_secs)
                retry_secs += 1

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

# interface Action
class Action(common_capnp.Action.Server):

    def __init__(self, action, *args, exec_action_on_del=False, **kwargs):
        self._args = args
        self._kwargs = kwargs
        self._action = action
        self._already_executed = False
        self._exec_action_on_del = exec_action_on_del

    def __del__(self):
        if self._exec_action_on_del and not self._already_executed:
            self._action(*self._args, **self._kwargs)

    def do_context(self, context): # do @0 () -> ();
        self._action(*self._args, **self._kwargs)
        self._already_executed = True

#------------------------------------------------------------------------------

# interface ValueHolder(T)
class ValueHolder(common_capnp.ValueHolder.Server):

    def __init__(self, value):
        self._value = value

    def value_context(self, context): # value @0 () -> (val :T);
        context.results.value = self._value

#------------------------------------------------------------------------------

# interface CapHolder(Object)
class CapHolderImpl(common_capnp.CapHolder.Server, Identifiable):

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

class IdentifiableHolder(common_capnp.IdentifiableHolder.Server, Identifiable):

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
"""
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
"""

#------------------------------------------------------------------------------

def load_capnp_module(path_and_type, def_type="Text"):
    capnp_type = def_type
    if path_and_type:
        p_and_t = path_and_type.split(":")
        if len(p_and_t) > 1:
            capnp_module_path, type_name = p_and_t
            capnp_module = capnp.load(capnp_module_path, imports=abs_imports)
            capnp_type = capnp_module.__dict__.get(type_name, def_type)
        elif len(p_and_t) > 0:
            capnp_type = p_and_t[0] 
    return capnp_type

#------------------------------------------------------------------------------

def load_capnp_modules(id_to_path_and_type, def_type="Text"):
    id_to_type = {}
    for name, path_and_type in id_to_path_and_type.items():
        capnp_type = def_type
        if path_and_type:
            p_and_t = path_and_type.split(":")
            if len(p_and_t) > 1:
                capnp_module_path, type_name = p_and_t
                capnp_module = capnp.load(capnp_module_path, imports=abs_imports)
                capnp_type = capnp_module.__dict__.get(type_name, def_type)
            elif len(p_and_t) > 0:
                capnp_type = p_and_t[0] 
        id_to_type[name] = capnp_type
    return id_to_type
