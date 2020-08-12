@0xd3f8859c7688b76b;

#using Go = import "lang/go.capnp";
#$Go.package("dataServices");
#$Go.import("dataServices");

using Date = import "date.capnp".Date;
using Geo = import "geo_coord.capnp".Geo;
using SoilData = import "soil_data.capnp".SoilData;
using ClimateData = import "climate_data.capnp".ClimateData;

interface DataServices {
  # the bootstrap interface to the different data services

  getAvailableSoilDataServices @0 () -> (availableSoilDataServices :List(SoilData.Service));
  getSoilDataService @1 (id :UInt64) -> (soilDataService :SoilData.Service);

  getAvailableClimateDataServices @2 () -> (availableClimateDataServices :List(ClimateData.Service));
  getClimateDataService @3 (id :UInt64) -> (climateDataService :ClimateData.Service);

  #landkreisId @1 (gkCoord :GKCoord) :Int64;
}
