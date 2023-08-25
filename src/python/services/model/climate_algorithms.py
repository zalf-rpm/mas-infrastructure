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
from datetime import date, timedelta
import json
import numpy as np
import os
import pandas as pd
from pathlib import Path
from pyproj import CRS, Transformer
from scipy.interpolate import NearestNDInterpolator
import sys
import time

PATH_TO_REPO = Path(os.path.realpath(__file__)).parent.parent.parent.parent.parent
if str(PATH_TO_REPO) not in sys.path:
    sys.path.insert(1, str(PATH_TO_REPO))

PATH_TO_PYTHON_CODE = PATH_TO_REPO / "src/python"
if str(PATH_TO_PYTHON_CODE) not in sys.path:
    sys.path.insert(1, str(PATH_TO_PYTHON_CODE))

from pkgs.climate import common_climate_data_capnp_impl as ccdi

PATH_TO_CAPNP_SCHEMAS = PATH_TO_REPO / "capnproto_schemas"
abs_imports = [str(PATH_TO_CAPNP_SCHEMAS)]
climate_data_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "climate.capnp"), imports=abs_imports)
reg_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "service_registry.capnp"), imports=abs_imports)
model_capnp = capnp.load(str(PATH_TO_CAPNP_SCHEMAS / "model.capnp"), imports=abs_imports)


class YearlyTavg(model_capnp.ClimateInstance.Server):

    def __init__(self):
        pass

    def runSet(self, dataset, **kwargs):  # (dataset :List(TimeSeries)) -> (result :XYPlusResult);
        pass

    def run(self, timeSeries, _context, **kwargs):  # (timeSeries :TimeSeries) -> (result :XYResult);
        # return timeSeries.header().then(lambda res: setattr(_context.results, "result", {"xs": [1,2,3], "ys": [2,3,4]}))
        return capnp.join_promises([timeSeries.header(), timeSeries.data(), timeSeries.range()]) \
            .then(lambda res: setattr(_context.results, "result", \
                                      self.calc_yearly_tavg(res[2].startDate, res[2].endDate, res[0].header,
                                                            res[1].data)))

    def calc_yearly_tavg(self, start_date, end_date, headers, data):
        "calculate the average temperature for all the years in the data"

        start_date = ccdi.create_date(start_date)
        end_date = ccdi.create_date(end_date)

        current_year = start_date.year
        current_sum_t = 0
        current_day_count = 0
        years = []
        tavgs = []
        for day in range((end_date - start_date).days + 1):

            current_date = start_date + timedelta(days=day)

            if current_year != current_date.year:
                years.append(current_year)
                tavgs.append(round(current_sum_t / current_day_count, 2))
                current_year = current_date.year
                current_sum_t = 0
                current_day_count = 0

            current_sum_t += data[day][0]
            current_day_count += 1

        return {"xs": years, "ys": tavgs}


def main():
    # address = parse_args().address

    # server = capnp.TwoPartyServer("*:8000", bootstrap=DataServiceImpl("/home/berg/archive/data/"))
    server = capnp.TwoPartyServer("*:8000", bootstrap=YearlyTavg())
    server.run_forever()


if __name__ == '__main__':
    main()
