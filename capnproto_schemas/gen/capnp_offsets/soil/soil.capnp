# soil.capnp
@0xff3f350f11891951;
$import "/capnp/c++.capnp".namespace("mas::schema::soil");
$import "/capnp/go.capnp".package("soil");
$import "/capnp/go.capnp".import("github.com/zalf-rpm/mas-infrastructure/capnproto_schemas/gen/go/soil");
enum SType @0xc2e4a3c8ff61b40a {
  unknown @0;
  ka5 @1;
}
enum PropertyName @0x9e391ae1c6cd2567 {
  soilType @0;
  sand @1;
  clay @2;
  silt @3;
  pH @4;
  sceleton @5;
  organicCarbon @6;
  organicMatter @7;
  bulkDensity @8;
  rawDensity @9;
  fieldCapacity @10;
  permanentWiltingPoint @11;
  saturation @12;
  soilMoisture @13;
  soilWaterConductivityCoefficient @14;
  ammonium @15;
  nitrate @16;
  cnRatio @17;
  inGroundwater @18;
  impenetrable @19;
}
struct Layer @0x984640f05b3ada4f {  # 8 bytes, 2 ptrs
  properties @0 :List(Property);  # ptr[0]
  size @1 :Float32;  # bits[0, 32)
  description @2 :Text;  # ptr[1]
  struct Property @0x92f4b81bcfdb71b0 {  # 8 bytes, 1 ptrs
    name @0 :PropertyName;  # bits[0, 16)
    union {  # tag bits [16, 32)
      f32Value @1 :Float32;  # bits[32, 64), union tag = 0
      bValue @2 :Bool;  # bits[32, 33), union tag = 1
      type @3 :Text;  # ptr[0], union tag = 2
      unset @4 :Void;  # bits[0, 0), union tag = 3
    }
  }
}
struct Query @0xbd4065087e22ca0d {  # 8 bytes, 2 ptrs
  mandatory @0 :List(PropertyName);  # ptr[0]
  optional @1 :List(PropertyName);  # ptr[1]
  onlyRawData @2 :Bool = true;  # bits[0, 1)
  struct Result @0xbf4e1b07ad88943f {  # 8 bytes, 2 ptrs
    failed @0 :Bool;  # bits[0, 1)
    mandatory @1 :List(PropertyName);  # ptr[0]
    optional @2 :List(PropertyName);  # ptr[1]
  }
}
struct Profile @0xff67c2a593419c29 {  # 8 bytes, 2 ptrs
  id @2 :Text;  # ptr[1]
  layers @0 :List(Layer);  # ptr[0]
  percentageOfArea @1 :Float32 = 100;  # bits[0, 32)
}
interface Service @0xa09aa71427dc64e1 superclasses(import "/common.capnp".Identifiable, import "/persistence.capnp".Persistent) {
  checkAvailableParameters @0 Query -> Query.Result;
  getAllAvailableParameters @1 (onlyRawData :Bool) -> (mandatory :List(PropertyName), optional :List(PropertyName));
  profilesAt @2 (coord :import "/geo.capnp".LatLonCoord, query :Query) -> (profiles :List(Profile));
}
