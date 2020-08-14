@0xff3f350f11891951;

using Cxx = import "/capnp/c++.capnp";
$Cxx.namespace("mas::rpc");

#using Go = import "lang/go.capnp";
#$Go.package("dataServices");
#$Go.import("dataServices");

using Common = import "common.capnp".Common;
using Date = import "date.capnp".Date;
using Geo = import "geo_coord.capnp".Geo;

struct Soil {
  
  struct LayerParameters {
    ka5SoilType @0 :Text; # soiltype according to KA5 classification
    
    sand @1 :Float32; # [% 0-1] sand content
    clay @2 :Float32; # [% 0-1] clay content
    silt @3 :Float32; # [% 0-1] silt content
    
    pH @4 :Float32; # pH value
    
    sceleton @5 :Float32; # [vol% 0-1] sceleton
    
    organic :union {
      carbon @6 :Float32; # [mass% 0-1] soil organic carbon
      matter @7 :Float32; # [mass% 0-1] soil organic matter
    }
    
    density :union {
      bulk @8 :Float32; # [kg m-3] soil bulk density
      raw @9 :Float32; # [kg m-3] soil raw density
    }

    fieldCapacity @10 :Float32; # [vol% 0-1]
    permanentWiltingPoint @11 :Float32; # [vol% 0-1]
    saturation @12 :Float32; # [vol% 0-1]

    initialSoilMoisture @13 :Float32; # [% 0-1] initial soilmoisture in this layer

    soilWaterConductivityCoefficient @14 :Float32; # [] lambda value

    ammonium @15 :Float32; # [kg NH4-N m-3] soil ammonium content
    nitrate @16 :Float32; # [kg NO3-N m-3] soil nitrate content

    cnRatio @17 :Float32; # [] C/N ratio

    isInGroundwater @18 :Bool; # lies layer in/below groundwater level

    isImpenetrable @19 :Bool; # can layer be penetrated by plant
  }

  struct Parameter {
    union {
      ka5SoilType @0 :Text; # soiltype according to KA5 classification
  
      sand @1 :Float32; # [% 0-1] sand content
      clay @2 :Float32; # [% 0-1] clay content
      silt @3 :Float32; # [% 0-1] silt content
      
      pH @4 :Float32; # pH value
      
      sceleton @5 :Float32; # [vol% 0-1] sceleton
      
      organicCarbon @6 :Float32; # [mass% 0-1] soil organic carbon
      organicMatter @7 :Float32; # [mass% 0-1] soil organic matter
      
      bulkDensity @8 :Float32; # [kg m-3] soil bulk density
      rawDensity @9 :Float32; # [kg m-3] soil raw density

      fieldCapacity @10 :Float32; # [vol% 0-1]
      permanentWiltingPoint @11 :Float32; # [vol% 0-1]
      saturation @12 :Float32; # [vol% 0-1]

      initialSoilMoisture @13 :Float32; # [% 0-1] initial soilmoisture in this layer

      soilWaterConductivityCoefficient @14 :Float32; # [] lambda value

      ammonium @15 :Float32; # [kg NH4-N m-3] soil ammonium content
      nitrate @16 :Float32; # [kg NO3-N m-3] soil nitrate content

      cnRatio @17 :Float32; # [] C/N ratio

      isInGroundwater @18 :Bool; # lies layer in/below groundwater level

      isImpenetrable @19 :Bool; # can layer be penetrated by plant

      size @20 :Float32; # [m]
    }
  }

  struct Layer {
    # a layer consists of a list of soil parameters
    params @0 :List(Parameter);
  }


  struct Query {
    # a query tells which soil parameters are mandatory and which are optional
    # a query fails if the mandatory parameters can't be delivered

    struct Result {
      # tell if the query failed and return the available parameters
      
      failed @0 :Bool; # some mandatory params where not available
      mandatory @1 :List(Parameter); # the mandatory parameters which where available
      optional @2 :List(Parameter); # the optional parameters which where available
    }

    mandatory @0 :List(Parameter);
    optional @1 :List(Parameter);
  }


  struct Profile {
    profile @0 :List(Layer);
    # a soil profile is a list of layers

    percentageOfArea @1 :Float32 = 100.0;
    # how many percent of some area are represented by this soil profile
  }


  interface Service extends(Common.Identifiable) {
    # service for soil data

    checkAvailableParameters @2 Query -> Query.Result;
    # check if the parameters given in Query are available

    getAllAvailableParameters @3 () -> (params :List(Parameter));
    # get all the available parameters in this service

    profilesAt @0 (coord :Geo.LatLonCoord, query :Query) -> (profiles :List(Profile));
    # Get soil profiles at a given geo coordinate. This might be multiple profiles if a profile doesn't cover 100% of the area

    allProfiles @1 Query -> (profiles :List(Common.Pair(Geo.LatLonCoord, List(Common.CapHolder(Profile)))));
    # Get a list of capabilities back to all available soil profiles
    # returned is a list of pairs of the geo coord and a list of capabilitites to the profiles at this geo coord
  }

}

