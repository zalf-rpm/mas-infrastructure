@0xb30a3af53cea6b3e;

using Cxx = import "/capnp/c++.capnp";
$Cxx.namespace("mas::rpc::management");

using Date = import "date.capnp".Date;
using Common = import "common.capnp".Common;
using Geo = import "geo_coord.capnp".Geo;
using MonicaParams = import "monica/monica_params.capnp";

enum Cultivar {
  alfalfaClovergrassLeyMix  @0;

  alfalfa                   @1;

  bacharia                  @2;

  barleySpring              @3;
  barleyWinter              @4;

  cloverGrassLey            @5;

  cottonBrMid               @6;
  cottonLong                @7;
  cottonMid                 @8;
  cottonShort               @9;

  einkorn                   @10;

  emmer                     @11;

  fieldPea24                @12;
  fieldPea26                @13;

  grapevine                 @14;

  maizeGrain                @15;
  maizeSilage               @16;

  mustard                   @17;

  oatCompound               @18;

  oilRadish                 @19;

  phacelia                  @20;

  potatoModeratelyEarly     @21;

  rapeWinter                @22;

  ryeGrass                  @23;

  ryeSilageWinter           @24;
  ryeSpring                 @25;
  ryeWinter                 @26;

  sorghum                   @27;

  soybean0                  @28;
  soybean00                 @29;
  soybean000                @30;
  soybean0000               @31;
  soybeanI                  @32;
  soybeanII                 @33;
  soybeanIII                @34;
  soybeanIV                 @35;
  soybeanV                  @36;
  soybeanVI                 @37;
  soybeanVII                @38;
  soybeanVIII               @39;
  soybeanIX                 @40;
  soybeanX                  @41;
  soybeanXI                 @42;
  soybeanXII                @43;
    
  sudanGrass                @44;

  sugarBeet                 @45;

  sugarcaneTransplant       @46;
  sugarcaneRatoon           @47;

  tomatoField               @48;

  triticaleSpring           @49;
  triticaleWinter           @50;

  wheatDurum                @51;
  wheatSpring               @52;
  wheatWinter               @53;
}

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
    cultivar      @0 :Cultivar;
    plantDensity  @1 :UInt16 = 0;
  }

  struct AutomaticSowing {
    sowing                      @11 :Sowing;

	  minTempThreshold            @0  :Float64;
    daysInTempWindow            @1  :UInt16;
    minPercentASW               @2  :Float64;
	  maxPercentASW               @3  :Float64;
	  max3dayPrecipSum            @4  :Float64;
	  maxCurrentDayPrecipSum      @5  :Float64;
	  tempSumAboveBaseTemp        @6  :Float64;
	  baseTemp                    @7  :Float64;

    avgSoilTemp :group {
      soilDepthForAveraging     @8  :Float64;
		  daysInSoilTempWindow      @9  :UInt16;
		  sowingIfAboveAvgSoilTemp  @10 :Float64;
  	}
  }

  struct Harvest {
    enum CropUsage {
      greenManure       @0;
      biomassProduction @1;
    }

    struct OptCarbonMgmtData {
      optCarbonConservation     @0 :Bool;
      cropImpactOnHumusBalance  @1 :Float64;
      cropUsage                 @2 :CropUsage;
      residueHeq                @3 :Float64;
      organicFertilizerHeq      @4 :Float64;
      maxResidueRecoverFraction @5 :Float64;
    }

    exported        @0 :Bool = true;
    optCarbMgmtData @1 :OptCarbonMgmtData;
  }

  struct AutomaticHarvest {
    harvest                 @5 :Harvest;

	  minPercentASW           @0 :Float64;
	  maxPercentASW           @1 :Float64;
	  max3dayPrecipSum        @2 :Float64;
	  maxCurrentDayPrecipSum  @3 :Float64;
	  harvestTime             @4 :Event.PhenoStage = maturity;
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
    partition @0 :MonicaParams.MineralFertilizerParameters;
    amount    @1 :Float64;
  }

  struct NDemandFertilization {
	  nDemand   @0 :Float64;
    partition @1 :MonicaParams.MineralFertilizerParameters;
	  depth     @2 :Float64;
	  stage     @3 :UInt8 = 1;
  }

  struct OrganicFertilization {
    params        @0 :MonicaParams.OrganicFertilizerParameters;
	  amount        @1 :Float64;
	  incorporation @2 :Bool = false;
  }

  struct Tillage {
    depth @0 :Float64;
  }

  struct Irrigation {
    amount @0 :Float64;
    params @1 :MonicaParams.IrrigationParameters;
  }


}




interface Service extends(Common.Identifiable) {
  # management service

  managementAt @0 Geo.LatLonCoord -> (mgmt :List(Event));
  # return a management at given location

}
  

