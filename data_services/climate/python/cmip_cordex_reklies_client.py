#!/usr/bin/python
# -*- coding: UTF-8

# This Source Code Form is subject to the terms of the Mozilla Public
# License, v. 2.0. If a copy of the MPL was not distributed with this
# file, You can obtain one at http://mozilla.org/MPL/2.0/. */

# Authors:
# Michael Berg-Mohnicke <michael.berg@zalf.de>
#
# Maintainers:
# Currently maintained by the authors.
#
# This file has been created at the Institute of
# Landscape Systems Analysis at the ZALF.
# Copyright (C: Leibniz Centre for Agricultural Landscape Research (ZALF)

import os
import sys
import json
import time

import capnp
capnp.add_import_hook(additional_paths=["../capnproto_schemas/", "../capnproto_schemas/capnp_schemas/"])
import model_capnp as m
import climate_data_capnp as cd

def main():

    config = {
        "port": "6666",
        "server": "localhost"
    }
    # read commandline args only if script is invoked directly from commandline
    if len(sys.argv) > 1 and __name__ == "__main__":
        for arg in sys.argv[1:]:
            k, v = arg.split("=")
            if k in config:
                config[k] = v

    print("config:", config)

    """
    climate_service = capnp.TwoPartyClient("localhost:8000").bootstrap().cast_as(cd.Climate.DataService)

    sims_prom = climate_service.simulations_request().send()
    sims = sims_prom.wait()
    
    if len(sims.simulations) > 0:
        info_prom = sims.simulations[1].info()
        info = info_prom.wait()
        print("id:", info.info.id)

        sim = sims.simulations[1]
        scens_p = sim.scenarios()
        scens = scens_p.wait()

        if len(scens.scenarios) > 0:
            scen = scens.scenarios[0]
            reals_p = scen.realizations()
            reals = reals_p.wait()

            if len(reals.realizations) > 0:
                real = reals.realizations[0]
                ts_p = real.closestTimeSeriesAt({"latlon": {"lat": 52.5, "lon": 14.1}})
                ts = ts_p.wait().timeSeries

                ts.range().then(lambda r: print(r)).wait()
                ts.realizationInfo().then(lambda r: print(r.realInfo)).wait()
                ts.scenarioInfo().then(lambda r: print(r.scenInfo)).wait()
                ts.simulationInfo().then(lambda r: print(r.simInfo)).wait()
    """

    #cmip_service = capnp.TwoPartyClient("login01.cluster.zalf.de:11001").bootstrap().cast_as(cd.Climate.Service)
    cmip_service = capnp.TwoPartyClient("localhost:9000").bootstrap().cast_as(cd.Climate.Service)
    #header = csv_time_series.header().wait().header

    print(cmip_service.info().wait())

    all_dss = cmip_service.getAvailableDatasets().wait().datasets
    mpd0 = all_dss[0]
    es = mpd0.meta.entries
    ds = mpd0.data
    ts = ds.closestTimeSeriesAt({"latlon": {"lat": 50.0, "lon": 12.0}}).wait().timeSeries
    h = ts.header().wait().header
    data = ts.data().wait().data
    loc = ts.location().wait()

    some_dss = cmip_service.getDatasetsFor({"entries": [{"gcm": "ichecEcEarth"}]}).wait().datasets
    ds2 = some_dss[0]
    md2 = ds2.metadata().wait()
    print(md2)
    info2 = md2.info.forOne({"gcm": 0}).wait()
    print(info2)
    infos2 = md2.info.forAll().wait().all
    print(infos2)
    locs = ds2.locations().wait().locations


    proms = []
    for i in range(10):
        env["customId"] = str(i)
        proms.append(monica_instance.run({"rest": {"value": json.dumps(env), "structure": {"json": None}}, "timeSeries": csv_time_series}))

    
    for i in range (10):
        ps = proms[i*50:i*50+50]

        for res in capnp.join_promises(ps).wait():
            if len(res.result.value) > 0:
                print(json.loads(res.result.value)["customId"]) #.result["customId"])

    return

    reslist = capnp.join_promises(proms).wait()
    #reslist.wait()
    
    for res in reslist:
        print(json.loads(res.result.value)["customId"]) #.result["customId"])

    #        .then(lambda res: setattr(_context.results, "result", \
    #           self.calc_yearly_tavg(res[2].startDate, res[2].endDate, res[0].header, res[1].data))) 

    #result_j = monica_instance.runEnv({"jsonEnv": json.dumps(env), "timeSeries": csv_time_series}).wait().result
    #result = json.loads(result_j)

    #print("result:", result)


    """
    #req = model.run_request()
    #req.data = ts
    #result = req.send().wait().result
    tavg_ts = ts.subheader(["tavg"]).wait().timeSeries
    start_time = time.perf_counter()
    result = model.run(tavg_ts).wait().result
    end_time = time.perf_counter()
    print("rust:", result, "time:", (end_time - start_time), "s")
    """     

    """
    models = climate_service.models().wait().models
    if len(models) > 0:
        model = models[0]
        #req = model.run_request()
        #req.data = ts
        #result = req.send().wait().result
        tavg_ts = ts.subheader(["tavg"]).wait().timeSeries
        start_time = time.perf_counter()
        result = model.run(tavg_ts).wait().result
        end_time = time.perf_counter()
        print("python:", result, "time:", (end_time - start_time), "s")

    #text_prom = data_services.getText_request().send()
    #text = text_prom.wait()

    #gk_coord = data_services.getCoord_request().send().wait()

    #req = data_services.getSoilDataService_request()
    #req.id = 1
    #sds_prom = req.send()
    #sds = sds_prom.wait()
    #soil_req = sds_prom.soilDataService.getSoilIdAt_request()
    #soil_req.gkCoord.meridianNo = 5
    #soil_req.gkCoord.r = 1000
    #soil_req.gkCoord.h = 2000
    #soilId_prom = soil_req.send()
    #resp = soilId_prom.wait()
    
    #print(resp.soilId)
    """


if __name__ == '__main__':
    main()
