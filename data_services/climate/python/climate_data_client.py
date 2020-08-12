import time

import capnp
capnp.add_import_hook(additional_paths=["../vcpkg/packages/capnproto_x64-windows-static/include/", "../capnproto_schemas/"])
import climate_data_capnp

def main():
    #address = parse_args().address

    rust_client = capnp.TwoPartyClient("localhost:4000")
    client = capnp.TwoPartyClient("localhost:8000")

    rust_climate_service = rust_client.bootstrap().cast_as(climate_data_capnp.Climate.DataService)
    climate_service = client.bootstrap().cast_as(climate_data_capnp.Climate.DataService)

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
                #ts.realizationInfo().then(lambda r: print(r.realInfo)).wait()
                #ts.scenarioInfo().then(lambda r: print(r.scenInfo)).wait()
                #ts.simulationInfo().then(lambda r: print(r.simInfo)).wait()

                #data_p = ts.data()
                #data_p = data_p.then(lambda d: print(d.data))

                #sub_ts_p = ts.subrange({"year": 2010, "month": 1, "day": 1}, {"year": 2011, "month": 1, "day": 15})
                #sub_ts = sub_ts_p.wait().timeSeries

                

                #subh_ts = ts.subheader(["tmin", "precip"]).wait().timeSeries
                #subh_ts.data().then(lambda d: print(d.data)).wait()

                #sub_ts.data().then(lambda d: print(d.data)).wait()

                #v = data_p.wait()
                #print("v:", v)


                #print(sub_data)
                #print(data)


    
    models = rust_climate_service.models().wait().models
    if len(models) > 0:
        model = models[0]
        #req = model.run_request()
        #req.data = ts
        #result = req.send().wait().result
        tavg_ts = ts.subheader(["tavg"]).wait().timeSeries
        start_time = time.perf_counter()
        result = model.run(tavg_ts).wait().result
        end_time = time.perf_counter()
        print("rust:", result, "time:", (end_time - start_time), "s")
        

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



if __name__ == '__main__':
    main()