import asyncio

import common.python.capnp_async_helpers as async_helpers

import capnp
#capnp.add_import_hook(additional_paths=["capnproto_schemas"])
#import climate_data_capnp
import capnproto_schemas.soil_data_capnp as soil_data_capnp

#csv_timeseries_cap = capnp.TwoPartyClient("localhost:11002").bootstrap().cast_as(climate_data_capnp.Climate.TimeSeries)
#header = csv_timeseries_cap.header().wait().header
#print(header)

#async def main():

#    client = await async_helpers.connect_to_server(6003)
#    soil_service = client.bootstrap().cast_as(soil_data_capnp.Soil.Service)
#    params = soil_service.getAllAvailableParameters().wait().params

#    print(soil_service)

soil_service = capnp.TwoPartyClient("localhost:6003").bootstrap().cast_as(soil_data_capnp.Soil.Service)
params = soil_service.getAllAvailableParameters().wait()
print(params)

#profiles = soil_service.profilesAt(
#    coord={"lat": 53.0, "lon": 12.5},
#    query={
#        "mandatory": [{"sand": 0}, {"clay": 0}, {"bulkDensity": 0}, {"organicCarbon": 0}],
#        "optional": [{"pH": 0}],
#        "onlyRawData": False
#    }
#).wait().profiles
#print(profiles)

profiles = soil_service.allLocations(
    mandatory=[{"sand": 0}, {"clay": 0}, {"bulkDensity": 0}, {"organicCarbon": 0}],
    optional=[{"pH": 0}],
    onlyRawData=False
).wait().profiles
latlon_and_cap = profiles[0]  # at the moment there is always just one profile being returned
cap_list = latlon_and_cap.snd
cap = cap_list[0]
p = latlon_and_cap.snd[0].cap().wait().object
print(p)


#soil_service = capnp.TwoPartyClient("localhost:6003").bootstrap().cast_as(soil_data_capnp.Soil.Service)
#profiles = soil_service.profilesAt(
#    coord={"lat": 53.0, "lon": 12.5},
#    query={
#        "mandatory": [{"sand": 0}, {"clay": 0}, {"bulkDensity": 0}, {"organicCarbon": 0}],
#        "optional": [{"pH": 0}]
#    }
#).wait().profiles

#print(profiles)



#if __name__ == '__main__':
#    asyncio.run(main())
