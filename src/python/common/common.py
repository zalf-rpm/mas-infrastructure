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

import base64
import capnp
from collections import defaultdict
from datetime import date, timedelta
import json
from pathlib import Path
import numpy as np
import os
import pysodium
import socket
import sys
import time
import urllib.parse as urlp
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


def get_fbp_attr(ip, attr_name):
    if ip.attributes and attr_name:
        for kv in ip.attributes:
            if kv.key == attr_name:
                return kv.value
    return None


def copy_and_set_fbp_attrs(old_ip, new_ip, **kwargs):
    # no attributes to be copied?
    if not old_ip.attributes and len(kwargs) == 0:
        return

    # is there an old attribute to be updated?
    attr_name_to_new_index = {}
    if old_ip.attributes and len(kwargs) > 0:
        for i, kv in enumerate(old_ip.attributes):
            if kv.key in kwargs:
                attr_name_to_new_index[kv.key] = i
                break

    # init space for attributes in new IP
    new_attrs_size = len(old_ip.attributes) if old_ip.attributes else 0
    for k, _ in kwargs.items():
        if k not in attr_name_to_new_index:
            new_attrs_size += 1
            attr_name_to_new_index[k] = new_attrs_size - 1
    attrs = new_ip.init("attributes", new_attrs_size)

    # copy old attributes
    if old_ip.attributes:
        indices = list(attr_name_to_new_index.values())
        for i, kv in enumerate(old_ip.attributes):
            if i not in indices:
                attrs[i].key = kv.key
                attrs[i].value = kv.value

    # set new attribute if there
    for attr_name, new_index in attr_name_to_new_index.items():
        attrs[new_index].key = attr_name
        attrs[new_index].value = kwargs[attr_name]


def update_config(config, argv, print_config=False, allow_new_keys=False):
    if len(argv) > 1:
        for arg in argv[1:]:
            k, v = arg.split("=", maxsplit=1)
            if allow_new_keys or k in config:
                config[k] = v.lower() == "true" if v.lower() in ["true", "false"] else v
        if print_config:
            print(config)


#def sign_sr_token_by_sk_and_encode_base64(self, sr_token):
#   return base64.urlsafe_b64encode(pysodium.crypto_sign(sr_token, self._sign_pk))


def sturdy_ref_str(vat_sign_pk, host, port, sr_token=None):
    return "capnp://{vat_id}@{host}:{port}{sr_token}".format(
        vat_id=base64.urlsafe_b64encode(vat_sign_pk).decode("utf-8"),
        host=host,
        port=port,
        sr_token="/" + sr_token if sr_token else ""
    )


def sturdy_ref_str_from_sr(sturdy_ref):
    sign_pk = bytearray(32)
    sign_pk[0:8] = sturdy_ref.transient.vat.id.publicKey0.to_bytes(8, byteorder=sys.byteorder, signed=False)
    sign_pk[8:16] = sturdy_ref.transient.vat.id.publicKey1.to_bytes(8, byteorder=sys.byteorder, signed=False)
    sign_pk[16:24] = sturdy_ref.transient.vat.id.publicKey2.to_bytes(8, byteorder=sys.byteorder, signed=False)
    sign_pk[24:32] = sturdy_ref.transient.vat.id.publicKey3.to_bytes(8, byteorder=sys.byteorder, signed=False)
    return sturdy_ref_str(sign_pk, sturdy_ref.transient.vat.address.host, sturdy_ref.transient.vat.address.port,
                          sturdy_ref.transient.localRef.text)


class ReleaseSturdyRef(persistence_capnp.Persistent.ReleaseSturdyRef.Server):

    def __init__(self, release_func):
        self._release_func = release_func

    def release_context(self, context):  # release @0 () -> (success :Bool);
        self._release_func()


class Restorer(persistence_capnp.Restorer.Server):

    def __init__(self):
        self._issued_sr_tokens = {}  # sr_token to {"sealed_for": owner_guid, "cap": capability}
        self._actions = []
        self._host = socket.gethostbyname(socket.gethostname())  # socket.getfqdn() #gethostname()
        self._port = None
        self._sign_pk, self._sign_sk = pysodium.crypto_sign_keypair()
        #self._box_pk, self._box_sk = pysodium.crypto_box_keypair()
        self.set_vat_id_from_sign_pk()
        self._owner_guid_to_sign_pk = {}  # owner guid to owner box public key
        self._storage_container = None
        self._restore_callback = None
        #self._vat_id = None

    def set_vat_id_from_sign_pk(self):
        self._vat_id = [
            int.from_bytes(self._sign_pk[0:8], byteorder=sys.byteorder, signed=False),
            int.from_bytes(self._sign_pk[8:16], byteorder=sys.byteorder, signed=False),
            int.from_bytes(self._sign_pk[16:24], byteorder=sys.byteorder, signed=False),
            int.from_bytes(self._sign_pk[24:32], byteorder=sys.byteorder, signed=False),
        ]

    @property
    def storage_container(self):
        return self._storage_container

    @storage_container.setter
    def storage_container(self, sc):
        self._storage_container = sc

    @property
    def port(self):
        return self._port

    @port.setter
    def port(self, p):
        self._port = p

    def store_port(self):
        if self.storage_container:
            return self.storage_container.getEntry(key="port").entry.setValue(value={"uint16Value": self.port})

    @property
    def host(self):
        return self._host

    @host.setter
    def host(self, h):
        self._host = h

    def init_port_from_container(self):
        def resp(resp):
            if not resp.isUnset:
                self.port = resp.value.uint16Value

        try:
            return self.storage_container.getEntry(key="port").entry.getValue().then(resp)
        except Exception as e:
            print("Couldn't initialize storage from container.", e)

    def init_vat_id_from_container(self):
        if not self.storage_container:
            return

        proms = []

        vat_sign_pk_prom = self.storage_container.getEntry(key="vatSignPK").entry.getValue()
        vat_sign_sk_prom = self.storage_container.getEntry(key="vatSignSK").entry.getValue()

        def pk_sk_resp(key, self, entryVal):
            # if there is no sign public key in the storage, we store the one generated by default
            if entryVal.isUnset:
                vat_sign_pk_req = self.storage_container.addEntry_request(key=key, replaceExisting=True)
                pkb = vat_sign_pk_req.init("value").init("uint8ListValue", len(self._sign_pk))
                for i in range(len(self._sign_pk)):
                    pkb[i] = self._sign_pk[i]
                return vat_sign_pk_req.send().then(lambda resp: resp.success)
            else:  # ok we got a sign public key
                bytes = entryVal.value.uint8ListValue
                arr = bytearray(len(bytes))
                for i in range(len(bytes)):
                    arr[i] = bytes[i]
                if key == "vatSignPK":
                    self._sign_pk = arr
                elif key == "vatSignSK":
                    self._sign_sk = arr
                return True

        try:
            proms.append(vat_sign_pk_prom.then(lambda entryVal: pk_sk_resp("vatSignPK", self, entryVal)))
            proms.append(vat_sign_sk_prom.then(lambda entryVal: pk_sk_resp("vatSignSK", self, entryVal)))

        except Exception as e:
            print("Couldn't initialize vat id from container.", e)

        return capnp.join_promises(proms).then(
            lambda read_keys: self.set_vat_id_from_sign_pk() if read_keys[0] and read_keys[1] else None)

    def set_owner_guid(self, owner_guid, owner_box_pk):
        self._owner_guid_to_sign_pk[owner_guid] = owner_box_pk

    #def verify_sr_token(self, sr_token_base64, vat_id_base64):
    #    # https://stackoverflow.com/questions/2941995/python-ignore-incorrect-padding-error-when-base64-decoding
    #    vat_id = base64.urlsafe_b64decode(vat_id_base64 + "==")
    #    try:
    #        sr_token = base64.urlsafe_b64decode(sr_token_base64 + "==")
    #        return (True, pysodium.crypto_sign_open(sr_token, vat_id))
    #    except ValueError:
    #        return (False, None)

    #def sign_sr_token_by_vat_and_encode_base64(self, sr_token):
    #    return base64.urlsafe_b64encode(pysodium.crypto_sign(sr_token, self._sign_pk))

    def get_cap_from_sr_token(self, sr_token, owner_guid=None):

        def get_cap(sr_data):
            if sr_data is None:
                return None
            elif "cap" in sr_data:
                return sr_data["cap"]
            elif len(sr_data["unsaveSRToken"]) > 0:  # restore an unsave action
                unsave_action = ReleaseSturdyRef(
                    lambda: self.unsave(sr_data["restore_token"]).then(
                        lambda success: self.unsave(sr_data["unsave_sr_token"]).then(
                            lambda success2: success and success2)))
                sr_data["cap"] = unsave_action
                return unsave_action
            elif self.restore_callback:  # restore a service object
                try:
                    cap = self._restore_callback(sr_data["restoreToken"])
                    sr_data["cap"] = cap
                    return cap
                except Exception as e:
                    pass
            return None

        def load_from_store_and_get_cap(sr_token):
            def value_resp(resp):
                if resp.isUnset:
                    return None
                value = resp.value.textValue
                sr_data = json.loads(value)
                self._issued_sr_tokens[sr_token] = sr_data
                if sr_data["sealed_for"] == owner_guid:
                    return sr_data
                else:
                    return None

            value_prom = self.storage_container.getEntry(key=sr_token).entry.getValue()
            return value_prom.then(value_resp).then(get_cap)

        # if there is an owner
        if owner_guid:
            # and we know about that owner
            if owner_guid in self._owner_guid_to_sign_pk:
                try:
                    unsigned_sr_token = pysodium.crypto_sign_open(sr_token, self._owner_guid_to_sign_pk[owner_guid])
                except ValueError:
                    return None
                data = self._issued_sr_tokens.get(unsigned_sr_token, None)
                # and that known owner was actually the one who sealed the token 
                if data:
                    if owner_guid == data["ownerGuid"]:
                        return get_cap(data)
                elif self.storage_container:
                    return load_from_store_and_get_cap(unsigned_sr_token)

            # if we don't know about that owner or the owner was not the one who sealed the token
            return None

        # if there is no owner
        if sr_token in self._issued_sr_tokens:
            return get_cap(self._issued_sr_tokens[sr_token])
        elif self.storage_container:
            return load_from_store_and_get_cap(sr_token)

    def sturdy_ref_str(self, sr_token=None):
        return sturdy_ref_str(self._sign_pk, self.host, self.port, sr_token)

    def sturdy_ref(self, sr_token=None): #, owner_guid=None):
        # if seal_for_owner_guid: then encrypt sr_token with seal_for_owner_guids stored public key
        #if owner_guid and sr_token:
        #    owner_pk = self._owner_guid_to_box_pk[owner_guid]
        #    sr_token = pysodium.crypto_sign(sr_token, owner_pk)
        return {
            "transient": {
                "vat": {
                    "id": {
                        "publicKey0": self._vat_id[0],
                        "publicKey1": self._vat_id[1],
                        "publicKey2": self._vat_id[2],
                        "publicKey3": self._vat_id[3],
                    },
                    "address": {
                        "host": self.host,
                        "port": self.port
                    }
                },
                "localRef": {"text": sr_token if sr_token else ""}
            }
        }

    def save(self, cap, fixed_sr_token=None, seal_for_owner_guid=None, create_unsave=True, restore_token=None):
        sr_token = fixed_sr_token if fixed_sr_token else str(uuid.uuid4())
        data = {"ownerGuid": seal_for_owner_guid, "restoreToken": restore_token}
        self._issued_sr_tokens[sr_token] = {**data, "cap": cap}
        store_proms = []
        if self.storage_container:
            sv_req = self.storage_container.getEntry(key=sr_token).entry.setValue_request()
            sv_req.init("value").textValue = json.dumps(data)
            store_proms.append(sv_req.send().then(lambda resp: resp.success))

        if create_unsave:
            unsave_sr_token = str(uuid.uuid4())
            unsave_action = ReleaseSturdyRef(
                lambda: self.unsave(sr_token).then(
                    lambda success: self.unsave(unsave_sr_token).then(
                        lambda success2: success and success2)))
            udata = {"ownerGuid": seal_for_owner_guid, "restoreToken": restore_token, "unsaveSRToken": unsave_sr_token}
            self._issued_sr_tokens[unsave_sr_token] = {**udata, "cap": unsave_action}
            if self.storage_container:
                sv_req = self.storage_container.getEntry(key=unsave_sr_token).entry.setValue_request()
                sv_req.init("value").textValue = json.dumps(udata)
                store_proms.append(sv_req.send().then(lambda resp: resp.success))

        res = {
            "sturdy_ref": self.sturdy_ref(sr_token),
            "unsave_sr": self.sturdy_ref(unsave_sr_token) if create_unsave else None
        }
        return capnp.join_promises(store_proms).then(lambda _: res)

    def save_str(self, cap, fixed_sr_token=None, seal_for_owner_guid=None, create_unsave=True, restore_token=None,
                 store_sturdy_refs=True):
        sr_token = fixed_sr_token if fixed_sr_token else str(uuid.uuid4())
        data = {"ownerGuid": seal_for_owner_guid, "restoreToken": restore_token}
        self._issued_sr_tokens[sr_token] = {**data, "cap": cap}
        store_proms = []
        if self.storage_container and store_sturdy_refs:
            sv_req = self.storage_container.getEntry(key=sr_token).entry.setValue_request()
            sv_req.init("value").textValue = json.dumps(data)
            store_proms.append(sv_req.send().then(lambda resp: resp.success))

        if create_unsave:
            unsave_sr_token = str(uuid.uuid4())
            # vat_signed_unsave_sr_token = self.sign_sr_token_by_vat_and_encode_base64(unsave_sr_token)
            unsave_action = ReleaseSturdyRef(
                lambda: self.unsave(sr_token).then(
                    lambda success: self.unsave(unsave_sr_token).then(
                        lambda success2: success and success2)))
            udata = {"ownerGuid": seal_for_owner_guid, "restoreToken": restore_token, "unsaveSRToken": unsave_sr_token}
            self._issued_sr_tokens[unsave_sr_token] = {**udata, "cap": unsave_action}
            if self.storage_container and store_sturdy_refs:
                sv_req = self.storage_container.getEntry(key=unsave_sr_token).entry.setValue_request()
                sv_req.init("value").textValue = json.dumps(udata)
                store_proms.append(sv_req.send().then(lambda resp: resp.success))

        res = {
            "sturdy_ref": self.sturdy_ref_str(sr_token),
            "sr_token": sr_token,
            "unsave_sr": self.sturdy_ref_str(unsave_sr_token) if create_unsave else None,
            "unsave_sr_token": unsave_sr_token if create_unsave else None
        }
        return capnp.join_promises(store_proms).then(lambda _: res)

    def unsave(self, sr_token, owner_guid=None):
        # if there is an owner
        if owner_guid:
            # and we know about that owner
            if owner_guid in self._owner_guid_to_sign_pk:
                verified_sr_token = pysodium.crypto_sign_open(sr_token, self._owner_guid_to_sign_pk[owner_guid])
                data = self._issued_sr_tokens.get(verified_sr_token, None)
                # and that known owner was actually the one who sealed the token 
                if data and owner_guid == data["ownerGuid"]:
                    del self._issued_sr_tokens[verified_sr_token]
        # if there is no owner
        else:
            del self._issued_sr_tokens[sr_token]

        if self.storage_container:
            return self.storage_container.removeEntry(key=sr_token)
        else:
            return capnp.Promise(True)

    @property
    def restore_callback(self):
        return self._restore_callback

    @restore_callback.setter
    def restore_callback(self, cb):
        self._restore_callback = cb

    # struct RestoreParams {
    #   localRef @0 :SturdyRef.Token;
    #   sealedBy @1 :SturdyRef.Owner;
    # }
    def restore_context(self, context):  # restore @0 RestoreParams -> (cap :Capability);
        owner_guid = context.params.sealedBy
        sr_token = context.params.localRef.as_text()
        context.results.cap = self.get_cap_from_sr_token(sr_token, owner_guid=owner_guid.guid if owner_guid else None)


class Identifiable(common_capnp.Identifiable.Server):

    def __init__(self, id=None, name=None, description=None):
        self._id = id if id else str(uuid.uuid4())
        self._name = name if name else "Unnamed_{}".format(self._id)
        self._description = description if description else ""
        self._init_info_func = None

    @property
    def init_info_func(self):
        return self._init_info_func

    @init_info_func.setter
    def init_info_func(self, f):
        self._init_info_func = f

    @property
    def id(self):
        return self._id

    @id.setter
    def id(self, i):
        self._id = i

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

    def info_context(self, context):  # () -> IdInformation;
        if self._init_info_func:
            self._init_info_func()
        r = context.results
        r.id = self.id
        r.name = self.name
        r.description = self.description


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


class Persistable(persistence_capnp.Persistent.Server):

    def __init__(self, restorer=None):
        self._restorer = restorer

    @property
    def restorer(self):
        return self._restorer

    @restorer.setter
    def restorer(self, r):
        self._restorer = r

    def save_context(self, context):  # save @0 () -> (sturdyRef :Text, unsaveSR :Text);
        def save_res(res):
            context.results.sturdyRef = res["sturdy_ref"]
            context.results.unsaveSR = res["unsave_sr"]

        if self.restorer:
            return self.restorer.save(self).then(save_res)


class ConnectionManager:

    def __init__(self, restorer=None):
        self._connections = {}
        self._restorer = restorer if restorer else Restorer()

    def connect(self, sturdy_ref, cast_as=None):
        try:
            host = None
            port = None
            sr_token = None
            owner_guid = None
            bootstrap_interface_id = None
            sturdy_ref_interface_id = None

            if type(sturdy_ref) == str:
                # we assume that a sturdy ref url looks always like
                # capnp://vat-id_base64-curve25519-public-key@host:port/sturdy-ref-token
                # ?owner_guid = optional_owner_global_unique_id
                # & b_iid = optional_bootstrap_interface_id
                # & sr_iid = optional_the_sturdy_refs_remote_interface_id
                # capnp://vat-id_base64-curve25519-public-key@host:port/sturdy-ref-token_base64
                url = urlp.urlparse(sturdy_ref)

                if url.scheme == "capnp":
                    #vat_id_base64 = url.username
                    host = url.hostname
                    port = url.port
                    if len(url.query) > 0:
                        q = urlp.parse_qs(url.query)
                        owner_guid = q.get("owner_guid", None)
                        bootstrap_interface_id = q.get("b_iid", None)
                        sturdy_ref_interface_id = q.get("sr_iid", None)
                    if len(url.path) > 1:
                        sr_token = url.path[1:]
                        # sr_token is base64 encoded if there's an owner (because of signing)
                        if owner_guid:
                            sr_token = base64.urlsafe_b64decode(sr_token + "==")
            else:
                vat_path = sturdy_ref.transient.vat
                #vat_id = vat_path.id
                host = vat_path.address.host
                port = vat_path.address.port
                sr_token = sturdy_ref.transient.localRef.as_text()

            host_port = str(host) + (":" + str(port) if port else "")
            if host_port in self._connections:
                bootstrap_cap = self._connections[host_port]
            else:
                bootstrap_cap = capnp.TwoPartyClient(host_port).bootstrap()
                self._connections[host_port] = bootstrap_cap

            if sr_token:
                restorer = bootstrap_cap.cast_as(persistence_capnp.Restorer)
                res_req = restorer.restore_request()
                res_req.localRef = sr_token
                dyn_obj_reader = res_req.send().wait().cap
                if dyn_obj_reader is not None:
                    return dyn_obj_reader.as_interface(cast_as) if cast_as else dyn_obj_reader
            else:
                return bootstrap_cap.cast_as(cast_as) if cast_as else bootstrap_cap

        except Exception as e:
            print("Exception in common.py::ConnectionManager.connect: {}".format(e))
            return None

        return None

    def try_connect(self, sturdy_ref, cast_as=None, retry_count=10, retry_secs=5, print_retry_msgs=True):
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


# interface Callback
# class CallbackImpl(common_capnp.Callback.Server):
#
#     def __init__(self, callback, *args, exec_callback_on_del=False, **kwargs):
#         self._args = args
#         self._kwargs = kwargs
#         self._callback = callback
#         self._already_called = False
#         self._exec_callback_on_del = exec_callback_on_del
#
#     def __del__(self):
#         if self._exec_callback_on_del and not self._already_called:
#             self._callback(*self._args, **self._kwargs)
#
#     def call(self, _context, **kwargs):  # call @0 ();
#         self._callback(*self._args, **self._kwargs)
#         self._already_called = True


# interface Action
# class Action(common_capnp.Action.Server):
#
#     def __init__(self, action, *args, exec_action_on_del=False, **kwargs):
#         self._args = args
#         self._kwargs = kwargs
#         self._action = action
#         self._already_executed = False
#         self._exec_action_on_del = exec_action_on_del
#
#     def __del__(self):
#         if self._exec_action_on_del and not self._already_executed:
#             self._action(*self._args, **self._kwargs)
#
#     def do_context(self, context):  # do @0 () -> ();
#         self._action(*self._args, **self._kwargs)
#         self._already_executed = True


# interface ValueHolder(T)
# class ValueHolder(common_capnp.ValueHolder.Server, Persistable):
#
#     def __init__(self, value, restorer=None):
#         Persistable.__init__(self, restorer)
#         self._value = value
#
#     def val(self):
#         return self._value
#
#     def value_context(self, context):  # value @0 () -> (val :T);
#         context.results.val = self.val


# interface AnyValueHolder
# class AnyValueHolder(common_capnp.AnyValueHolder.Server, Persistable):
#
#     def __init__(self, value, restorer=None):
#         Persistable.__init__(self, restorer)
#         self._value = value
#
#     @property
#     def val(self):
#         return self._value
#
#     def value_context(self, context):  # value @0 () -> (val :AnyPointer);
#         context.results.val = self.val


# interface CapHolder(Object)
# class CapHolderImpl(common_capnp.CapHolder.Server, Identifiable):
#
#     def __init__(self, cap, cleanup_func, cleanup_on_del=False):
#         self._cap = cap
#         self._cleanup_func = cleanup_func
#         self._already_cleaned_up = False
#         self._cleanup_on_del = cleanup_on_del
#
#     def __del__(self):
#         if self._cleanup_on_del and not self._already_cleaned_up:
#             self.cleanup_func()
#
#     def cap_context(self, context):  # cap @0 () -> (object :Object);
#         context.results.cap = self._cap
#
#     def release_context(self, context):  # release @1 ();
#         self._cleanup_func()
#         self._cleanup_on_del = True


# class IdentifiableHolder(common_capnp.IdentifiableHolder.Server, Identifiable):
#
#     def __init__(self, cap, cleanup_func, cleanup_on_del=False):
#         self._cap = cap
#         self._cleanup_func = cleanup_func
#         self._already_cleaned_up = False
#         self._cleanup_on_del = cleanup_on_del
#
#     def __del__(self):
#         if self._cleanup_on_del and not self._already_cleaned_up:
#             self.cleanup_func()
#
#     def cap_context(self, context):  # cap @0 () -> (object :Object);
#         context.results.cap = self._cap
#
#     def release_context(self, context):  # release @1 ();
#         self._cleanup_func()
#         self._cleanup_on_del = True


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
