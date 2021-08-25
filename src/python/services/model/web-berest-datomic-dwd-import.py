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
from datetime import date, datetime
import ftplib
import io
import os
from pathlib import Path
import schedule
import sys
import threading
import time

PATH_TO_REPO = Path(os.path.realpath(__file__)).parent.parent.parent.parent.parent.parent
if str(PATH_TO_REPO) not in sys.path:
    sys.path.insert(1, str(PATH_TO_REPO))

PATH_TO_PYTHON_CODE = PATH_TO_REPO / "src/python"
if str(PATH_TO_PYTHON_CODE) not in sys.path:
    sys.path.insert(1, str(PATH_TO_PYTHON_CODE))

PATH_TO_CAPNP_SCHEMAS = PATH_TO_REPO / "capnproto_schemas"
abs_imports = [str(PATH_TO_CAPNP_SCHEMAS)]
dwd_service_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "web-berest-data-import.capnp"), imports=abs_imports)


def run_continuously(interval=1):
    """Continuously run, while executing pending jobs at each
    elapsed time interval.
    @return cease_continuous_run: threading. Event which can
    be set to cease continuous run. Please note that it is
    *intended behavior that run_continuously() does not run
    missed jobs*. For example, if you've registered a job that
    should run every minute and you set a continuous run
    interval of one hour then your job won't be run 60 times
    at each interval but only once.
    """
    cease_continuous_run = threading.Event()

    class ScheduleThread(threading.Thread):
        @classmethod
        def run(cls):
            while not cease_continuous_run.is_set():
                schedule.run_pending()
                time.sleep(interval)

    continuous_thread = ScheduleThread()
    continuous_thread.start()
    return cease_continuous_run


def task(ftps_host, ftps_user, ftps_pwd, import_host="localhost", import_port="15000", specific_date = None):
    print("Running import task at", datetime.now())
    d = date.today() if specific_date is None else specific_date

    ftps = ftplib.FTP_TLS(ftps_host, user=ftps_user, passwd=ftps_pwd)
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

    cap = capnp.TwoPartyClient(import_host + ":" + import_port).bootstrap().cast_as(dwd_service_capnp.DWLABImport)
    success = cap.importData(f"{d:%Y-%m-%d}", dwla, dwlb).wait()
    print("Import succeeded?", success)

if __name__ == '__main__':
    config = {
        "ftps_host": "srv-fds-tran.zalf.de",
        "ftps_user": "dwduser2021",
        "ftps_pwd": None,
        "import_host": "localhost",
        "import_port": "15000",
        "run_at": "11:00",
        "dates" : None
    }
    # read commandline args only if script is invoked directly from commandline
    if len(sys.argv) > 1 and __name__ == "__main__":
        for arg in sys.argv[1:]:
            k, v = arg.split("=")
            if k in config:
                config[k] = v
    print("config used:", config)

    if config["ftps_pwd"] is None:
        print("ftps_pwd is missing!")
        exit(0)

    import_dates = None
    if config["dates"] is not None:
        import_dates = list(map(lambda d: date.fromisoformat(d), config["dates"].split(",")))

    if import_dates is None:
        schedule.every().day.at(config["run_at"]).do(task, config["ftps_host"], config["ftps_user"], config["ftps_pwd"], \
            import_host=config["import_host"], import_port=config["import_port"])
        run_continuously(60) #check every minute
    else:
        for import_date in import_dates:
            task(config["ftps_host"], config["ftps_user"], config["ftps_pwd"], specific_date=import_date, \
                import_host=config["import_host"], import_port=config["import_port"])
