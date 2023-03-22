# management.capnp
@0xb30a3af53cea6b3e;
$import "/capnp/c++.capnp".namespace("mas::schema::management");
$import "/capnp/go.capnp".package("management");
$import "/capnp/go.capnp".import("github.com/zalf-rpm/mas-infrastructure/capnproto_schemas/gen/go/management");
enum EventType @0x82a74595175b71a3 {
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
enum PlantOrgan @0xc2d50914b83d42de {
  root @0;
  leaf @1;
  shoot @2;
  fruit @3;
  strukt @4;
  sugar @5;
}
struct Event @0x9c5dedfd679ac842 {  # 8 bytes, 4 ptrs
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
  enum ExternalType @0xf082ec2d0eb50c9b {
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
  enum PhenoStage @0x8fa09457bc1bfc34 {
    emergence @0;
    flowering @1;
    anthesis @2;
    maturity @3;
  }
  struct Type @0xe1ed73d59c8ce359 {  # 8 bytes, 0 ptrs
    union {  # tag bits [16, 32)
      external @0 :ExternalType;  # bits[0, 16), union tag = 0
      internal @1 :PhenoStage;  # bits[0, 16), union tag = 1
    }
  }
}
struct Params @0x9d247c812334c917 {  # 0 bytes, 0 ptrs
  struct Sowing @0x80ce153f3bc9a9e8 {  # 8 bytes, 2 ptrs
    cultivar @0 :Text;  # ptr[0]
    plantDensity @1 :UInt16;  # bits[0, 16)
    crop @2 :import "/crop.capnp".Crop;  # ptr[1]
  }
  struct AutomaticSowing @0xcfcf44997e7ceab4 {  # 64 bytes, 2 ptrs
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
    struct AvgSoilTemp @0x9d81d2bf4cd0f868 {  # 24 bytes, 0 ptrs
      soilDepthForAveraging @0 :Float64 = 0.3;  # bits[0, 64)
      daysInSoilTempWindow @1 :UInt16;  # bits[64, 80)
      sowingIfAboveAvgSoilTemp @2 :Float64;  # bits[128, 192)
    }
  }
  struct Harvest @0xeed4e55bb04289ef {  # 8 bytes, 1 ptrs
    exported @0 :Bool = true;  # bits[0, 1)
    optCarbMgmtData @1 :OptCarbonMgmtData;  # ptr[0]
    enum CropUsage @0x8f0cbec420589373 {
      greenManure @0;
      biomassProduction @1;
    }
    struct OptCarbonMgmtData @0x8cb6b3e3c50d3665 {  # 40 bytes, 0 ptrs
      optCarbonConservation @0 :Bool;  # bits[0, 1)
      cropImpactOnHumusBalance @1 :Float64;  # bits[64, 128)
      cropUsage @2 :CropUsage = biomassProduction;  # bits[16, 32)
      residueHeq @3 :Float64;  # bits[128, 192)
      organicFertilizerHeq @4 :Float64;  # bits[192, 256)
      maxResidueRecoverFraction @5 :Float64;  # bits[256, 320)
    }
  }
  struct AutomaticHarvest @0xe3a37e340f816cd1 {  # 40 bytes, 1 ptrs
    harvest @5 :Harvest;  # ptr[0]
    minPercentASW @0 :Float64;  # bits[0, 64)
    maxPercentASW @1 :Float64 = 100;  # bits[64, 128)
    max3dayPrecipSum @2 :Float64;  # bits[128, 192)
    maxCurrentDayPrecipSum @3 :Float64;  # bits[192, 256)
    harvestTime @4 :Event.PhenoStage = maturity;  # bits[256, 272)
  }
  struct Cutting @0xfec75f2ddd43431d {  # 8 bytes, 1 ptrs
    cuttingSpec @0 :List(Spec);  # ptr[0]
    cutMaxAssimilationRatePercentage @1 :Float64;  # bits[0, 64)
    enum CL @0x825bb2508c0b37b2 {
      cut @0;
      left @1;
    }
    enum Unit @0xf0c763e472409ba2 {
      percentage @0;
      biomass @1;
      lai @2;
    }
    struct Spec @0x9a221e04faf79efc {  # 24 bytes, 0 ptrs
      organ @0 :PlantOrgan;  # bits[0, 16)
      value @1 :Float64;  # bits[64, 128)
      unit @2 :Unit;  # bits[16, 32)
      cutOrLeft @3 :CL;  # bits[32, 48)
      exportPercentage @4 :Float64 = 100;  # bits[128, 192)
    }
  }
  struct MineralFertilization @0xd3da30ea7b25d921 {  # 8 bytes, 1 ptrs
    fertilizer @0 :Fertilizer;  # ptr[0]
    amount @1 :Float64;  # bits[0, 64)
  }
  struct NDemandFertilization @0x953375ac67d4f573 {  # 24 bytes, 1 ptrs
    nDemand @0 :Float64;  # bits[0, 64)
    fertilizer @1 :Fertilizer;  # ptr[0]
    depth @2 :Float64;  # bits[64, 128)
    stage @3 :UInt8 = 1;  # bits[128, 136)
  }
  struct OrganicFertilization @0xe98c76fb0fb0b2cd {  # 16 bytes, 1 ptrs
    fertilizer @0 :Fertilizer;  # ptr[0]
    amount @1 :Float64;  # bits[0, 64)
    incorporation @2 :Bool;  # bits[64, 65)
  }
  struct Tillage @0x88a5848ef8603554 {  # 8 bytes, 0 ptrs
    depth @0 :Float64 = 0.3;  # bits[0, 64)
  }
  struct Irrigation @0x87feb816363ff43c {  # 8 bytes, 1 ptrs
    amount @0 :Float64;  # bits[0, 64)
    nutrientConcentrations @1 :List(Nutrient);  # ptr[0]
  }
}
struct Nutrient @0xaafe4332e17aa43e {  # 16 bytes, 0 ptrs
  nutrient @0 :Name;  # bits[0, 16)
  value @1 :Float64;  # bits[64, 128)
  unit @2 :Unit;  # bits[16, 32)
  enum Name @0xbc6b579acf43fb6e {
    urea @0;
    ammonia @1;
    nitrate @2;
    phosphorus @3;
    potassium @4;
    sulfate @5;
    organicC @6;
    organicN @7;
    organicP @8;
    organicNFast @9;
    organicNSlow @10;
  }
  enum Unit @0x987b68b57edbbdb6 {
    none @0;
    fraction @1;
    percent @2;
  }
}
interface Fertilizer @0x8c4cb8d60ae5aec7 superclasses(import "/common.capnp".Identifiable, import "/persistence.capnp".Persistent) {
  nutrients @0 () -> (nutrients :List(Nutrient));
  parameters @1 () -> (params :AnyPointer);
}
interface FertilizerService @0xbbb7aeae0d097e05 superclasses(import "/registry.capnp".Registry) {
}
interface Service @0xc876b729b7d7f6d9 superclasses(import "/common.capnp".Identifiable) {
  managementAt @0 import "/geo.capnp".LatLonCoord -> (mgmt :List(Event));
}
