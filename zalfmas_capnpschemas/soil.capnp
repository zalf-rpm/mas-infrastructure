@0xff3f350f11891951;

using Cxx = import "/capnp/c++.capnp";
$Cxx.namespace("mas::schema::soil");

using Go = import "/capnp/go.capnp";
$Go.package("soil");
$Go.import("github.com/zalf-rpm/mas-infrastructure/capnproto_schemas/gen/go/soil");

using Persistent = import "persistence.capnp".Persistent;
using Restorer = import "persistence.capnp".Restorer;
using Identifiable = import "common.capnp".Identifiable;
using Geo = import "geo.capnp";

enum SType {
  # soil types

  unknown @0; # marks an unknown soil type
  ka5     @1; # KA5 classification
}

enum PropertyName {
  # layer properties

  soilType                          @0; # soil type

  sand                              @1; # [%] sand content
  clay                              @2; # [%] clay content
  silt                              @3; # [%] silt content

  pH                                @4; # pH value

  sceleton                          @5; # [vol%] sceleton

  organicCarbon                     @6; # [mass%] soil organic carbon
  organicMatter                     @7; # [mass%] soil organic matter

  bulkDensity                       @8; # [kg m-3] soil bulk density
  rawDensity                        @9; # [kg m-3] soil raw density

  fieldCapacity                     @10; # [vol%]
  permanentWiltingPoint             @11; # [vol%]
  saturation                        @12; # [vol%]

  soilMoisture                      @13; # [%] initial soil moisture in this layer

  soilWaterConductivityCoefficient  @14; # [] lambda value

  ammonium                          @15; # [kg NH4-N m-3] soil ammonium content
  nitrate                           @16; # [kg NO3-N m-3] soil nitrate content

  cnRatio                           @17; # [] C/N ratio

  inGroundwater                     @18; # lies layer in/below groundwater level

  impenetrable                      @19; # can layer be penetrated by plant
}

struct Layer {
  # describes a single soil layer

  struct Property {
    name @0 :PropertyName; # name of the layer property
    union {
      f32Value  @1 :Float32;
      bValue    @2 :Bool;
      type      @3 :Text;
      unset     @4 :Void;
    }
  }

  # a layer consists of a list of soil parameters
  properties    @0 :List(Property);

  size          @1 :Float32; # [m]

  description   @2 :Text; # some human understandable description of the layer
}


struct Query {
  # a query tells which soil parameters are mandatory and which are optional
  # a query fails if the mandatory parameters can't be delivered

  struct Result {
    # tell if the query failed and return the available parameters
    
    failed      @0 :Bool; # some mandatory params where not available
    mandatory   @1 :List(PropertyName); # the mandatory parameters which where available
    optional    @2 :List(PropertyName); # the optional parameters which where available
  }

  mandatory     @0 :List(PropertyName); # these parameters are really needed
  optional      @1 :List(PropertyName); # these parameters are optional
  
  onlyRawData   @2 :Bool = true;
  # just return data which are physically available from the data source
  # if set to false, data can be generated from the raw data to allow more 
  # params to be available mandatory
}

struct ProfileData {
  layers            @0 :List(Layer);
  # a soil profile is a list of layers

  percentageOfArea  @1 :Float32 = 100.0;
  # how many percent of some area are represented by this soil profile
}

interface Profile extends (Identifiable, Persistent) {
  # a capability to a soil profile

  data        @0 () -> ProfileData;
  # the data of the profile

  geoLocation @1 () -> Geo.LatLonCoord;
  # the geo location of the profile
}

interface Service extends(Identifiable, Persistent) {
  # service for soil data

  checkAvailableParameters @0 Query -> Query.Result;
  # check if the parameters given in Query are available

  getAllAvailableParameters @1 (onlyRawData :Bool) -> (mandatory :List(PropertyName), optional :List(PropertyName));
  # get all the available parameters in this service

  closestProfilesAt @2 (coord :Geo.LatLonCoord, query :Query) -> (profiles :List(Profile));
  # Get soil profiles closest to a given geo coordinate. This might be multiple profiles if a profile doesn't cover 100% of the area.

  interface Stream {
    nextProfiles @0 (maxCount :Int64 = 100) -> (profiles :List(Profile));
    # receive next batch of profiles, if available, but maximum maxCount profiles at one time
  }

  streamAllProfiles @3 Query -> (allProfiles :Stream);
  # stream all profiles to client in batches
}

