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

import os
from pathlib import Path

PATH_TO_REPO = Path(os.path.realpath(__file__)).parent.parent.parent.parent.parent.parent
if str(PATH_TO_REPO) not in sys.path:
    sys.path.insert(1, str(PATH_TO_REPO))

import capnp
import capnproto_schemas.a_capnp as a_capnp

a_cap = capnp.TwoPartyClient("localhost:11111").bootstrap().cast_as(a_capnp.A)
txt = a_cap.method("______________PARAM______________").wait().res
print(txt)