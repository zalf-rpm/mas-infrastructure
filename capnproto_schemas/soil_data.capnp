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

  struct DepthAndLayerParams {
    size @0 :Float32; # [m]
    # size of the soil layer

    params @1 :LayerParameters;
    # a set of soil parameters for the layer
  }

  struct Profile {
    profile @0 :List(DepthAndLayerParams);
    # a soil profile is a list of layers

    percentageOfArea @1 :Float32 = 100.0;
    # how many percent of some area are represented by this soil profile
  }


  interface Service extends(Common.Identifiable) {
    # service for soil data

    profilesAt @0 Geo.Coord -> (profiles :List(Profile));
    # Get soil profiles at a given geo coordinate. This might be multiple profiles if a profile doesn't cover 100% of the area

    allProfiles @1 () -> (profiles :List(Common.Pair(Geo.Coord, List(Common.CapHolder(Profile)))));
    # Get a list of capabilities back to all available soil profiles
    # returned is a list of pairs of the geo coord and a list of capabilitites to the profiles at this geo coord
  }

}

