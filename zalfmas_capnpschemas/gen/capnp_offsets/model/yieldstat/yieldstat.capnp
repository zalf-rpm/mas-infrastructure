# model/yieldstat/yieldstat.capnp
@0xb80c8fd14e523f9b;
$import "/capnp/c++.capnp".namespace("mas::schema::model::yieldstat");
$import "/capnp/go.capnp".package("yieldstat");
$import "/capnp/go.capnp".import("github.com/zalf-rpm/mas-infrastructure/capnproto_schemas/gen/go/model/yieldstat");
enum ResultId @0xcfe218c48d227e0d {
  primaryYield @0;
  dryMatter @1;
  carbonInAboveGroundBiomass @2;
  sumFertilizer @3;
  sumIrrigation @4;
  primaryYieldCU @5;
}
struct RestInput @0xa47f8d65869200af {  # 24 bytes, 0 ptrs
  useDevTrend @0 :Bool;  # bits[0, 1)
  useCO2Increase @1 :Bool = true;  # bits[1, 2)
  dgm @2 :Float64;  # bits[64, 128)
  hft @3 :UInt8;  # bits[8, 16)
  nft @4 :UInt8;  # bits[16, 24)
  sft @5 :UInt8;  # bits[24, 32)
  slope @6 :UInt8;  # bits[32, 40)
  steino @7 :UInt8;  # bits[40, 48)
  az @8 :UInt8;  # bits[48, 56)
  klz @9 :UInt8;  # bits[56, 64)
  stt @10 :UInt8;  # bits[128, 136)
  germanFederalStates @11 :Int8 = -1;  # bits[136, 144)
  getDryYearWaterNeed @12 :Bool;  # bits[2, 3)
}
struct Result @0x8db55634a0e7d054 {  # 8 bytes, 2 ptrs
  cultivar @0 :Text;  # ptr[0]
  isNoData @1 :Bool;  # bits[0, 1)
  values @2 :List(ResultToValue);  # ptr[1]
  struct ResultToValue @0x8d365bd4f0136fc0 {  # 16 bytes, 0 ptrs
    id @0 :ResultId;  # bits[0, 16)
    value @1 :Float64;  # bits[64, 128)
  }
}
struct Output @0x932a681f81b4be19 {  # 8 bytes, 3 ptrs
  id @0 :Text;  # ptr[0]
  runFailed @1 :Bool;  # bits[0, 1)
  reason @2 :Text;  # ptr[1]
  results @3 :List(YearToResult);  # ptr[2]
  struct YearToResult @0xa008c533888c3a5e {  # 8 bytes, 1 ptrs
    year @0 :Int16;  # bits[0, 16)
    result @1 :Result;  # ptr[0]
  }
}
