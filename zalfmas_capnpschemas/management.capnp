@0xb30a3af53cea6b3e;

using Cxx = import "/capnp/c++.capnp";
$Cxx.namespace("mas::schema::management");

using Go = import "/capnp/go.capnp";
$Go.package("management");
$Go.import("github.com/zalf-rpm/mas-infrastructure/capnproto_schemas/gen/go/management");

using Date = import "date.capnp".Date;
using Common = import "common.capnp";
using Geo = import "geo.capnp";
using Crop = import "crop.capnp";
using Persistent = import "persistence.capnp".Persistent;
using Registry = import "registry.capnp".Registry;

enum EventType {
  sowing                    @0;
  automaticSowing           @1;

  harvest                   @2;
  automaticHarvest          @3;

  irrigation                @4;

  tillage                   @5;

  organicFertilization      @6;
  mineralFertilization      @7;
  nDemandFertilization      @8;

  cutting                   @9;

  setValue                  @10;

  saveState                 @11;
}

enum PlantOrgan {
  root @0;
  leaf @1;
  shoot @2;
  fruit @3;
  strukt @4;
  sugar @5;
}

struct Event {
  enum ExternalType {
    sowing                    @0;
    automaticSowing           @1;

    harvest                   @2;
    automaticHarvest          @3;

    irrigation                @4;

    tillage                   @5;

    organicFertilization      @6;
    mineralFertilization      @7;
    nDemandFertilization      @8;

    cutting                   @9;

    #setValue                  @10;

    #saveState                 @11;
  }

  enum PhenoStage { 
    emergence @0;
    flowering @1;
    anthesis  @2;
    maturity  @3;
  }

  struct Type {
    union {
      external @0 :ExternalType;
      internal @1 :PhenoStage;
    }
  }

  type            @0 :ExternalType;
  info            @1 :Common.IdInformation;

  union {
    at :group {
      date        @2 :Date;
    }
    between :group {
      earliest    @3 :Date;
      latest      @4 :Date;
    }
    after :group {
      event       @5 :Type;
      days        @6 :UInt16;
    }
  }

  params          @7 :AnyPointer;

  runAtStartOfDay @8 :Bool;
}

struct Params {
  struct Sowing {
    cultivar      @0 :Text;
    plantDensity  @1 :UInt16 = 0;
    crop          @2 :Crop.Crop;
    # can be null then only the general cultivar name is known, but no specific parameters
    # or a model doesn't really have or need particular parameters
  }

  struct AutomaticSowing {
    struct AvgSoilTemp {
      soilDepthForAveraging     @0  :Float64  = 0.3;
		  daysInSoilTempWindow      @1  :UInt16;
		  sowingIfAboveAvgSoilTemp  @2  :Float64;
    }

    sowing                      @9  :Sowing;

	  minTempThreshold            @0  :Float64;
    daysInTempWindow            @1  :UInt16;
    minPercentASW               @2  :Float64  = 0;
	  maxPercentASW               @3  :Float64  = 100;
	  max3dayPrecipSum            @4  :Float64;
	  maxCurrentDayPrecipSum      @5  :Float64;
	  tempSumAboveBaseTemp        @6  :Float64;
	  baseTemp                    @7  :Float64;

    avgSoilTemp                 @8  :AvgSoilTemp; 
  }

  struct Harvest {
    enum CropUsage {
      greenManure       @0;
      biomassProduction @1;
    }

    struct OptCarbonMgmtData {
      optCarbonConservation     @0 :Bool      = false;
      cropImpactOnHumusBalance  @1 :Float64   = 0;
      cropUsage                 @2 :CropUsage = biomassProduction;
      residueHeq                @3 :Float64   = 0;
      organicFertilizerHeq      @4 :Float64   = 0;
      maxResidueRecoverFraction @5 :Float64   = 0;
    }

    exported        @0 :Bool = true;
    optCarbMgmtData @1 :OptCarbonMgmtData;
  }

  struct AutomaticHarvest {
    harvest                 @5 :Harvest;

	  minPercentASW           @0 :Float64;
	  maxPercentASW           @1 :Float64           = 100;
	  max3dayPrecipSum        @2 :Float64;
	  maxCurrentDayPrecipSum  @3 :Float64;
	  harvestTime             @4 :Event.PhenoStage  = maturity;
  }

  struct Cutting {
    enum CL { 
      cut   @0;
      left  @1;
    }
	  enum Unit { 
      percentage  @0;
      biomass     @1;
      lai         @2;
    }

    struct Spec {
      organ             @0 :PlantOrgan;
      value             @1 :Float64;
		  unit              @2 :Unit        = percentage;
		  cutOrLeft         @3 :CL          = cut;
      exportPercentage  @4 :Float64     = 100.0; 
    }

    cuttingSpec                       @0 :List(Spec);
    cutMaxAssimilationRatePercentage  @1 :Float64;
	}

  struct MineralFertilization {
    fertilizer  @0 :Fertilizer;
    amount      @1 :Float64;
  }

  struct NDemandFertilization {
	  nDemand     @0 :Float64;
    fertilizer  @1 :Fertilizer;
	  depth       @2 :Float64;
	  stage       @3 :UInt8 = 1;
  }

  struct OrganicFertilization {
    fertilizer    @0 :Fertilizer;
	  amount        @1 :Float64;
	  incorporation @2 :Bool = false;
  }

  struct Tillage {
    depth @0 :Float64 = 0.3;
  }

  struct Irrigation {
    amount                  @0 :Float64;
    nutrientConcentrations  @1 :List(Nutrient);
    # if set, concentration of nutrients in water [kg m-3]
  }
}

struct Nutrient {
  enum Name {
    urea          @0;   # Carbamide / Harnstoff
    ammonia       @1;   # NH4 / Ammonium
    nitrate       @2;   # NO3 / Nitrat
    phosphorus    @3;   # Phosphor
    potassium     @4;   # Kalium
    sulfate       @5;   # Schwefel
    organicC      @6;   # organic carbon
    organicN      @7;   # organic nitrate
    organicP      @8;   # organic phosphorus
    organicNFast  @9;   # organic N fast fraction
    organicNSlow  @10;  # organic N slow fraction
  }

  enum Unit {
    none          @0; # none
    fraction      @1; # fraction
    percent       @2; # percent 
  }

  nutrient  @0 :Name;
  value     @1 :Float64;
  unit      @2 :Unit;
}

interface Fertilizer extends(Common.Identifiable, Persistent) {
  # represents a fertilizer

  nutrients  @0 () -> (nutrients :List(Nutrient));
  # return list of nutrients this fertilizer is made of

  parameters @1 () -> (params :AnyPointer);
  # if not null a specific parameterset for this fertilizer
  # a tradeoff to support additional model specific parameters
}

interface FertilizerService extends(Registry) {}
# fertilizer service should be just a registry.capnp::Registry of the available fertilizers

interface Service extends(Common.Identifiable) {
  # management service

  managementAt @0 Geo.LatLonCoord -> (mgmt :List(Event));
  # return a management at given location

}
  
