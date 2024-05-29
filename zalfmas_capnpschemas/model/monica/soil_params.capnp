@0xf83caca0747996ab;

using Cxx = import "/capnp/c++.capnp";
$Cxx.namespace("mas::schema::soil");

using Go = import "/capnp/go.capnp";
$Go.package("monica");
$Go.import("github.com/zalf-rpm/mas-infrastructure/capnproto_schemas/gen/go/model/monica");

struct SoilCharacteristicData {
    struct Data {
        soilType        @0 :Text;   # KA5 soil type
        soilRawDensity  @1 :Int16;  # kg / m3
        airCapacity     @2 :UInt8;  # %
        fieldCapacity   @3 :UInt8;  # %
        nFieldCapacity  @4 :UInt8;  # %
    }
    list @0 :List(Data);
}

struct SoilCharacteristicModifier {
    struct Data {
        soilType        @0 :Text;
        organicMatter   @1 :Float32;    # %
        airCapacity     @2 :Int8;       # %
        fieldCapacity   @3 :Int8;       # %
        nFieldCapacity  @4 :Int8;       # %

    }
    list @0 :List(Data);
}

struct CapillaryRiseRate {
    struct Data {
        soilType    @0 :Text;
        distance    @1 :UInt8;  # dm
        rate        @2 :Float32;
    }
    list @0 :List(Data);
}
