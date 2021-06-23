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

import capnp
from datetime import date
import ftplib
import io
import os
from pathlib import Path
import sys

PATH_TO_REPO = Path(os.path.realpath(__file__)).parent.parent.parent.parent.parent.parent
if str(PATH_TO_REPO) not in sys.path:
    sys.path.insert(1, str(PATH_TO_REPO))

PATH_TO_PYTHON_CODE = PATH_TO_REPO / "src/python"
if str(PATH_TO_PYTHON_CODE) not in sys.path:
    sys.path.insert(1, str(PATH_TO_PYTHON_CODE))

abs_imports = ["capnproto_schemas"]
dwd_service_capnp = capnp.load("capnproto_schemas/web-berest-data-import.capnp", imports=abs_imports)

d = date(2021, 6, 20)

ftps = ftplib.FTP_TLS("srv-fds-tran.zalf.de", user="dwduser2021", passwd="")
ftps.prot_p()
ftps.cwd("dwd")

with io.BytesIO() as f:
    ftps.retrbinary(f"RETR FY60DWLA-{d:%Y%m%d}_0815.txt", f.write)
    dwla = f.getvalue().decode("cp1252")
    #print("DWLA:\n", dwla)
with io.BytesIO() as f:
    ftps.retrbinary(f"RETR FY60DWLB-{d:%Y%m%d}_0815.txt", f.write)
    dwlb = f.getvalue().decode("cp1252")
    #print("DWLB:\n", dwlb)

cap = capnp.TwoPartyClient("localhost:15000").bootstrap().cast_as(dwd_service_capnp.DWLABImport)
success = cap.importData(f"{d:%Y-%m-%d}", dwla, dwlb).wait()
print(success)