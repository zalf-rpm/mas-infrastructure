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

#from datetime import date, timedelta
#import json
import capnp
import os
from pathlib import Path
import socket
import sys
import time
import uuid

PATH_TO_REPO = Path(os.path.realpath(__file__)).parent.parent.parent.parent.parent
if str(PATH_TO_REPO) not in sys.path:
    sys.path.insert(1, str(PATH_TO_REPO))

PATH_TO_PYTHON_CODE = PATH_TO_REPO / "src/python"
if str(PATH_TO_PYTHON_CODE) not in sys.path:
    sys.path.insert(1, str(PATH_TO_PYTHON_CODE))

import csv_file_based as csv_based

abs_imports = ["capnproto_schemas"]
climate_data_capnp = capnp.load("capnproto_schemas/climate_data.capnp", imports=abs_imports)

class Factory(climate_data_capnp.CSVTimeSeriesFactory.Server):

    def __init__(self, id=None, name=None, description=None, port=None):
        self._id = id if id else str(uuid.uuid4()) 
        self._name = name if name else "Unnamed Factory " + self._id 
        self._description = description if description else ""

        self._time_series_caps = []
        self._issued_sr_tokens = []
        self._host = socket.getfqdn() #gethostname()
        self._port = port


    @property
    def port(self):
        return self._port
    
    @port.setter
    def port(self, p):
        self._port = p


    def info(self, _context, **kwargs): # () -> IdInformation;
        r = _context.results
        r.id = self._id
        r.name = self._name
        r.description = self._description


    def create_context(self, context): # create @0 (fromCSV :Text, config :CSVConfig) -> (timeSeries :TimeSeries, error :Text);
        c = context.params.config
        csv = context.params.fromCSV

        if csv is None:
            context.results.error = "fromCSV parameter was null"
        else:    
            try:
                header_map = {}
                if c.headerMap:
                    for p in c.headerMap:
                        header_map[p.fst] = p.snd
                
                pandas_csv_config = {}
                if c.sep:
                    pandas_csv_config["sep"] = c.sep

                skip = []
                for i in range(0, c.skipLinesToHeader):
                    skip.append(i)
                header_line = 0 if len(skip) == 0 else skip[-1] + 1
                for i in range(header_line + 1, header_line + 1 + c.skipLinesFromHeaderToData):
                    skip.append(i)
                pandas_csv_config["skip_rows"] = skip

                ts = csv_based.TimeSeries.from_csv_string(csv, header_map=header_map, pandas_csv_config=pandas_csv_config)
                self._time_series_caps.append(ts)
                context.results.timeSeries = ts
            except Exception as e:
                context.results.error = str(e)


    def save_context(self, context): # save @0 SaveParams -> SaveResults;
        if self.port:
            id = uuid.uuid4()
            self._issued_sr_tokens.append(str(id))
            context.results.sturdyRef = "capnp://insecure@{host}:{port}/{sr_token}".format(host=self._host, port=self.port, sr_token=id)
         

    def restore_context(self, context): # restore @0 (srToken :Token, owner :SturdyRef.Owner) -> (cap :Capability);
        if context.params.srToken in self._issued_sr_tokens:
            context.results.cap = self


    def drop_context(self, context): # drop @1 (srToken :Token, owner :SturdyRef.Owner);
        self._issued_sr_tokens.remove(context.params.srToken)



#------------------------------------------------------------------------------

def main(server="*", port=11005):

    config = {
        "port": str(port),
        "server": server
    }
    # read commandline args only if script is invoked directly from commandline
    if len(sys.argv) > 1 and __name__ == "__main__":
        for arg in sys.argv[1:]:
            k, v = arg.split("=")
            if k in config:
                config[k] = v
    print(config)

    server = capnp.TwoPartyServer(config["server"] + ":" + config["port"], bootstrap=Factory())
    server.run_forever()


if __name__ == '__main__':
    main()
