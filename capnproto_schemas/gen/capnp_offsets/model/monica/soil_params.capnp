# model/monica/soil_params.capnp
@0xf83caca0747996ab;
$import "/capnp/c++.capnp".namespace("mas::schema::soil");
$import "/capnp/go.capnp".package("monica");
$import "/capnp/go.capnp".import("github.com/zalf-rpm/mas-infrastructure/capnproto_schemas/gen/go/model/monica");
struct SoilCharacteristicData @0xfc682227304e2281 {  # 0 bytes, 1 ptrs
  list @0 :List(Data);  # ptr[0]
  struct Data @0xeafaab57e025db63 {  # 8 bytes, 1 ptrs
    soilType @0 :Text;  # ptr[0]
    soilRawDensity @1 :Int16;  # bits[0, 16)
    airCapacity @2 :UInt8;  # bits[16, 24)
    fieldCapacity @3 :UInt8;  # bits[24, 32)
    nFieldCapacity @4 :UInt8;  # bits[32, 40)
  }
}
struct SoilCharacteristicModifier @0xe4eb0a9bb0e5bb53 {  # 0 bytes, 1 ptrs
  list @0 :List(Data);  # ptr[0]
  struct Data @0xa968a46ccde8b1b4 {  # 8 bytes, 1 ptrs
    soilType @0 :Text;  # ptr[0]
    organicMatter @1 :Float32;  # bits[0, 32)
    airCapacity @2 :Int8;  # bits[32, 40)
    fieldCapacity @3 :Int8;  # bits[40, 48)
    nFieldCapacity @4 :Int8;  # bits[48, 56)
  }
}
struct CapillaryRiseRate @0x9b169bc96bb3d24b {  # 0 bytes, 1 ptrs
  list @0 :List(Data);  # ptr[0]
  struct Data @0xb78a89c58fad885d {  # 8 bytes, 1 ptrs
    soilType @0 :Text;  # ptr[0]
    distance @1 :UInt8;  # bits[0, 8)
    rate @2 :Float32;  # bits[32, 64)
  }
}
