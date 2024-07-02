@0xb80c8fd14e523f9b;

using Cxx = import "/capnp/c++.capnp";
$Cxx.namespace("mas::schema::model::yieldstat");

using Go = import "/capnp/go.capnp";
$Go.package("yieldstat");
$Go.import("github.com/zalf-rpm/mas-infrastructure/capnproto_schemas/gen/go/model/yieldstat");

using Crop = import "/crop.capnp";

enum ResultId {
    primaryYield                @0; # primary yield for the crop (e.g. the actual fruit)
    dryMatter                   @1; # primary yield converted to dry matter
    carbonInAboveGroundBiomass  @2; # carbon content in surface biomass
    sumFertilizer               @3; # sum of applied fertilizer for that crop during growth period
    sumIrrigation               @4; # sum of used irrigation water for the crop during growth period
    primaryYieldCU              @5; # the primary yield but in GE
}    

struct RestInput {
    useDevTrend         @0  :Bool = false;
	
    useCO2Increase      @1  :Bool = true;

    dgm                 @2  :Float64 = 0;
    hft                 @3  :UInt8 = 0;
    nft                 @4  :UInt8 = 0;
    sft                 @5  :UInt8 = 0;
    slope               @6  :UInt8 = 0;
    steino              @7  :UInt8 = 0;
    az                  @8  :UInt8 = 0;
    klz                 @9  :UInt8 = 0;
    stt                 @10 :UInt8 = 0;
    germanFederalStates @11 :Int8 = -1;

    getDryYearWaterNeed @12 :Bool = false;
}

struct Result {
    struct ResultToValue {
        id      @0 :ResultId;
        value   @1 :Float64;
    }

    cultivar    @0 :Text; #
    isNoData    @1 :Bool = false; # no results for given cultivar
    values      @2 :List(ResultToValue);
}

struct Output {
    struct YearToResult {
        year    @0 :Int16;
        result  @1 :Result;
    }

    id          @0 :Text;
    runFailed   @1 :Bool = false;
    reason      @2 :Text;
    results     @3 :List(YearToResult);
}