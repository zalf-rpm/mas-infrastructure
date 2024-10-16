# model/monica/monica_management.capnp
@0x93337c65a295d42f;
$import "/capnp/c++.capnp".namespace("mas::schema::model::monica");
$import "/capnp/go.capnp".package("monica");
$import "/capnp/go.capnp".import("github.com/zalf-rpm/mas-infrastructure/capnproto_schemas/gen/go/model/monica");
struct ILRDates @0xa1f99f32eea02590 {  # 0 bytes, 5 ptrs
  sowing @0 :import "/date.capnp".Date;  # ptr[0]
  earliestSowing @1 :import "/date.capnp".Date;  # ptr[1]
  latestSowing @2 :import "/date.capnp".Date;  # ptr[2]
  harvest @3 :import "/date.capnp".Date;  # ptr[3]
  latestHarvest @4 :import "/date.capnp".Date;  # ptr[4]
}
enum EventType @0xd0290daf8de9f2b0 {
  sowing @0;
  automaticSowing @1;
  harvest @2;
  automaticHarvest @3;
  irrigation @4;
  tillage @5;
  organicFertilization @6;
  mineralFertilization @7;
  nDemandFertilization @8;
  cutting @9;
  setValue @10;
  saveState @11;
}
enum PlantOrgan @0xb33447204cdf022c {
  root @0;
  leaf @1;
  shoot @2;
  fruit @3;
  strukt @4;
  sugar @5;
}
struct Event @0xcf672ab379467704 {  # 8 bytes, 4 ptrs
  type @0 :ExternalType;  # bits[0, 16)
  info @1 :import "/common.capnp".IdInformation;  # ptr[0]
  union {  # tag bits [16, 32)
    at :group {  # union tag = 0
      date @2 :import "/date.capnp".Date;  # ptr[1]
    }
    between :group {  # union tag = 1
      earliest @3 :import "/date.capnp".Date;  # ptr[1]
      latest @4 :import "/date.capnp".Date;  # ptr[2]
    }
    after :group {  # union tag = 2
      event @5 :Type;  # ptr[1]
      days @6 :UInt16;  # bits[32, 48)
    }
  }
  params @7 :AnyPointer;  # ptr[3]
  runAtStartOfDay @8 :Bool;  # bits[48, 49)
  enum ExternalType @0xe5484dc513ee11e0 {
    sowing @0;
    automaticSowing @1;
    harvest @2;
    automaticHarvest @3;
    irrigation @4;
    tillage @5;
    organicFertilization @6;
    mineralFertilization @7;
    nDemandFertilization @8;
    cutting @9;
  }
  enum PhenoStage @0xb2bf3a5557791bc1 {
    emergence @0;
    flowering @1;
    anthesis @2;
    maturity @3;
  }
  struct Type @0xb91010c363e568a4 {  # 8 bytes, 0 ptrs
    union {  # tag bits [16, 32)
      external @0 :ExternalType;  # bits[0, 16), union tag = 0
      internal @1 :PhenoStage;  # bits[0, 16), union tag = 1
    }
  }
}
struct Params @0xcb20e21466098705 {  # 0 bytes, 0 ptrs
  struct Sowing @0xc6880d1c13ec14dc {  # 8 bytes, 2 ptrs
    cultivar @0 :Text;  # ptr[0]
    plantDensity @1 :UInt16;  # bits[0, 16)
    crop @2 :import "/crop.capnp".Crop;  # ptr[1]
  }
  struct AutomaticSowing @0xd1bfc1c9617d9453 {  # 64 bytes, 2 ptrs
    sowing @9 :Sowing;  # ptr[1]
    minTempThreshold @0 :Float64;  # bits[0, 64)
    daysInTempWindow @1 :UInt16;  # bits[64, 80)
    minPercentASW @2 :Float64;  # bits[128, 192)
    maxPercentASW @3 :Float64 = 100;  # bits[192, 256)
    max3dayPrecipSum @4 :Float64;  # bits[256, 320)
    maxCurrentDayPrecipSum @5 :Float64;  # bits[320, 384)
    tempSumAboveBaseTemp @6 :Float64;  # bits[384, 448)
    baseTemp @7 :Float64;  # bits[448, 512)
    avgSoilTemp @8 :AvgSoilTemp;  # ptr[0]
    struct AvgSoilTemp @0x846f567433b186d1 {  # 24 bytes, 0 ptrs
      soilDepthForAveraging @0 :Float64 = 0.3;  # bits[0, 64)
      daysInSoilTempWindow @1 :UInt16;  # bits[64, 80)
      sowingIfAboveAvgSoilTemp @2 :Float64;  # bits[128, 192)
    }
  }
  struct Harvest @0x8feb941d70f2a468 {  # 8 bytes, 1 ptrs
    exported @0 :Bool = true;  # bits[0, 1)
    optCarbMgmtData @1 :OptCarbonMgmtData;  # ptr[0]
    enum CropUsage @0xa9a9bc941e963701 {
      greenManure @0;
      biomassProduction @1;
    }
    struct OptCarbonMgmtData @0xaf49ab9bbe76e375 {  # 40 bytes, 0 ptrs
      optCarbonConservation @0 :Bool;  # bits[0, 1)
      cropImpactOnHumusBalance @1 :Float64;  # bits[64, 128)
      cropUsage @2 :CropUsage = biomassProduction;  # bits[16, 32)
      residueHeq @3 :Float64;  # bits[128, 192)
      organicFertilizerHeq @4 :Float64;  # bits[192, 256)
      maxResidueRecoverFraction @5 :Float64;  # bits[256, 320)
    }
  }
  struct AutomaticHarvest @0xf805d22fabb80702 {  # 40 bytes, 1 ptrs
    harvest @5 :Harvest;  # ptr[0]
    minPercentASW @0 :Float64;  # bits[0, 64)
    maxPercentASW @1 :Float64 = 100;  # bits[64, 128)
    max3dayPrecipSum @2 :Float64;  # bits[128, 192)
    maxCurrentDayPrecipSum @3 :Float64;  # bits[192, 256)
    harvestTime @4 :Event.PhenoStage = maturity;  # bits[256, 272)
  }
  struct Cutting @0x8460dac6abff7ed9 {  # 8 bytes, 1 ptrs
    cuttingSpec @0 :List(Spec);  # ptr[0]
    cutMaxAssimilationRatePercentage @1 :Float64;  # bits[0, 64)
    enum CL @0xe444f780b29541a7 {
      cut @0;
      left @1;
    }
    enum Unit @0x94d32947f136655e {
      percentage @0;
      biomass @1;
      lai @2;
    }
    struct Spec @0xfae5dcfccbb93a23 {  # 24 bytes, 0 ptrs
      organ @0 :PlantOrgan;  # bits[0, 16)
      value @1 :Float64;  # bits[64, 128)
      unit @2 :Unit;  # bits[16, 32)
      cutOrLeft @3 :CL;  # bits[32, 48)
      exportPercentage @4 :Float64 = 100;  # bits[128, 192)
    }
  }
  struct MineralFertilization @0xa363d226e178debd {  # 8 bytes, 1 ptrs
    partition @0 :Parameters;  # ptr[0]
    amount @1 :Float64;  # bits[0, 64)
    struct Parameters @0xc75b5ef2e9b05c2d {  # 24 bytes, 2 ptrs
      id @0 :Text;  # ptr[0]
      name @1 :Text;  # ptr[1]
      carbamid @2 :Float64;  # bits[0, 64)
      nh4 @3 :Float64;  # bits[64, 128)
      no3 @4 :Float64;  # bits[128, 192)
    }
  }
  struct NDemandFertilization @0xc7c14e92e0cd461c {  # 24 bytes, 1 ptrs
    nDemand @0 :Float64;  # bits[0, 64)
    partition @1 :MineralFertilization.Parameters;  # ptr[0]
    depth @2 :Float64;  # bits[64, 128)
    stage @3 :UInt8 = 1;  # bits[128, 136)
  }
  struct OrganicFertilization @0xb492838c7fed50b0 {  # 16 bytes, 1 ptrs
    params @0 :Parameters;  # ptr[0]
    amount @1 :Float64;  # bits[0, 64)
    incorporation @2 :Bool;  # bits[64, 65)
    struct OrganicMatterParameters @0x95cdc661a6600137 {  # 104 bytes, 0 ptrs
      aomDryMatterContent @0 :Float64;  # bits[0, 64)
      aomNH4Content @1 :Float64;  # bits[64, 128)
      aomNO3Content @2 :Float64;  # bits[128, 192)
      aomCarbamidContent @3 :Float64;  # bits[192, 256)
      aomSlowDecCoeffStandard @4 :Float64;  # bits[256, 320)
      aomFastDecCoeffStandard @5 :Float64;  # bits[320, 384)
      partAOMToAOMSlow @6 :Float64;  # bits[384, 448)
      partAOMToAOMFast @7 :Float64;  # bits[448, 512)
      cnRatioAOMSlow @8 :Float64;  # bits[512, 576)
      cnRatioAOMFast @9 :Float64;  # bits[576, 640)
      partAOMSlowToSMBSlow @10 :Float64;  # bits[640, 704)
      partAOMSlowToSMBFast @11 :Float64;  # bits[704, 768)
      nConcentration @12 :Float64;  # bits[768, 832)
    }
    struct Parameters @0xba0c11cf818d29fd {  # 0 bytes, 3 ptrs
      params @0 :OrganicMatterParameters;  # ptr[0]
      id @1 :Text;  # ptr[1]
      name @2 :Text;  # ptr[2]
    }
  }
  struct Tillage @0xaa49811a4e3e2c59 {  # 8 bytes, 0 ptrs
    depth @0 :Float64 = 0.3;  # bits[0, 64)
  }
  struct Irrigation @0xd90939a58e404ff8 {  # 8 bytes, 1 ptrs
    amount @0 :Float64;  # bits[0, 64)
    params @1 :Parameters;  # ptr[0]
    struct Parameters @0xaec9e089e87f1599 {  # 16 bytes, 0 ptrs
      nitrateConcentration @0 :Float64;  # bits[0, 64)
      sulfateConcentration @1 :Float64;  # bits[64, 128)
    }
  }
}
interface Service @0xbfda1920aff38c07 superclasses(import "/common.capnp".Identifiable) {
  managementAt @0 import "/geo.capnp".LatLonCoord -> (mgmt :List(Event));
}
