@0x93337c65a295d42f;

using Cxx = import "/capnp/c++.capnp";
$Cxx.namespace("mas::schema::model::monica");

using Go = import "/capnp/go.capnp";
$Go.package("monica");
$Go.import("github.com/zalf-rpm/mas-infrastructure/capnproto_schemas/gen/go/model/monica");

using Date = import "/date.capnp".Date;
using Common = import "/common.capnp";
using Geo = import "/geo.capnp";
using Crop = import "/crop.capnp";

struct ILRDates {
  sowing          @0 :Date;
  earliestSowing  @1 :Date;
  latestSowing    @2 :Date;
  harvest         @3 :Date;
  latestHarvest   @4 :Date;
}

#enum MineralFertilizer {
#  ahls  @0;
#  alzon @1;
#  an    @2;
#  ap    @3;
#  as    @4;
#  ash   @5;
#  cf4   @6;
#  cp1   @7;
#  cp2   @8;
#  cp3   @9;
#  npk   @10;
#  ns    @11;
#  u     @12;
#  uan   @13;
#  uas   @14;
#  uni   @15;
#}

#enum OrganicFertilizer {
#  ash   @0;
#  cadlm @1;
#  cam   @2;
#  cas   @3;
#  cau   @4;
#  dgdlm @5;
#  gwc   @6;
#  hodlm @7;
#  mc    @8;
#  ms    @9;
#  oic   @10;
#  pidlm @11;
#  pim   @12;
#  pis   @13;
#  piu   @14;
#  piudk @15;
#  plw   @16;
#  podlm @17;
#  pom   @18;
#  soy   @19;
#  ss    @20;
#  tudlm @21;
#  weeds @22;
#  ws    @23;
#}

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
    # can be null then only the general cultivar is known, but no specific parameters
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
    struct Parameters {
      id          @0 :Text;
      name        @1 :Text;
      carbamid    @2 :Float64; # [%]
      nh4         @3 :Float64; # [%]
      no3         @4 :Float64; # [%]
    }

    partition @0 :Parameters;
    amount    @1 :Float64;
  }

  struct NDemandFertilization {
	  nDemand   @0 :Float64;
    partition @1 :MineralFertilization.Parameters;
	  depth     @2 :Float64;
	  stage     @3 :UInt8 = 1;
  }

  struct OrganicFertilization {
    struct OrganicMatterParameters {
      aomDryMatterContent         @0  :Float64; # Dry matter content of added organic matter [kg DM kg FM-1]
      aomNH4Content               @1  :Float64; # Ammonium content in added organic matter [kg N kg DM-1]
      aomNO3Content               @2  :Float64; # Nitrate content in added organic matter [kg N kg DM-1]
      aomCarbamidContent          @3  :Float64; # Carbamide content in added organic matter [kg N kg DM-1]

      aomSlowDecCoeffStandard     @4  :Float64; # Decomposition rate coefficient of slow AOM at standard conditions [d-1]
      aomFastDecCoeffStandard     @5  :Float64; # Decomposition rate coefficient of fast AOM at standard conditions [d-1]

      partAOMToAOMSlow            @6  :Float64; # Part of AOM that is assigned to the slowly decomposing pool [kg kg-1]
      partAOMToAOMFast            @7  :Float64; # Part of AOM that is assigned to the rapidly decomposing pool [kg kg-1]

      cnRatioAOMSlow              @8  :Float64; # C to N ratio of the slowly decomposing AOM pool []
      cnRatioAOMFast              @9  :Float64; # C to N ratio of the rapidly decomposing AOM pool []

      partAOMSlowToSMBSlow        @10 :Float64; # Part of AOM slow consumed by slow soil microbial biomass [kg kg-1]
      partAOMSlowToSMBFast        @11 :Float64; # Part of AOM slow consumed by fast soil microbial biomass [kg kg-1]

      nConcentration              @12 :Float64;
    }

    struct Parameters {
      params  @0 :OrganicMatterParameters;
      id      @1 :Text;
      name    @2 :Text;
    }

    params        @0 :Parameters;
	  amount        @1 :Float64;
	  incorporation @2 :Bool = false;
  }

  struct Tillage {
    depth @0 :Float64 = 0.3;
  }

  struct Irrigation {
    struct Parameters {
      nitrateConcentration @0 :Float64;   # nitrate concentration [mg dm-3]
      sulfateConcentration @1 :Float64;   # sulfate concentration [mg dm-3]
    }

    amount @0 :Float64;
    params @1 :Parameters;
  }
}

#interface FertilizerService extends(Common.Identifiable) {
#  # service to return predefined fertilizers
#
#  #struct Entry(T) {
#  #  info  @0 :Common.IdInformation;
#  #  ref   @1 :Common.ValueHolder(T);
#  #}
#
#  struct Entry {
#    info  @0 :Common.IdInformation;
#    ref   @1 :Common.AnyValueHolder;
#  }
#
#  #availableMineralFertilizers     @2 () -> (entries :List(Entry(Params.MineralFertilization.Parameters)));
#  availableMineralFertilizers     @2 () -> (entries :List(Entry));
#  # return list of all available mineral fertilizers with references to value holders
#
#  mineralFertilizer               @4 (id :Text) -> (fert :Params.MineralFertilization.Parameters);
#  
#  #availableOrganicFertilizers    @3 () -> (entries :List(Entry(Params.OrganicFertilization.OrganicMatterParameters)));
#  availableOrganicFertilizers    @3 () -> (entries :List(Entry));
#  # return list of all available organic fertilizers with references to value holders
#
#  organicFertilizer               @5 (id :Text) -> (fert :Params.OrganicFertilization.OrganicMatterParameters);
#
#  mineralFertilizerPartitionFor   @0 (minFert :MineralFertilizer) -> (partition :Params.MineralFertilization.Parameters);
#  # get mineral fertilizer parameters by name/id
#
#  organicFertilizerParametersFor  @1 (orgFert :OrganicFertilizer) -> (params :Params.OrganicFertilization.Parameters);
#  # get organic fertilizer parameters by name/id
#}


interface Service extends(Common.Identifiable) {
  # management service

  managementAt @0 Geo.LatLonCoord -> (mgmt :List(Event));
  # return a management at given location

}
  
