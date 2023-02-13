# climate.capnp
@0xa01d3ae410eb4518;
$import "/capnp/c++.capnp".namespace("mas::schema::climate");
$import "/capnp/go.capnp".package("climate");
$import "/capnp/go.capnp".import("github.com/zalf-rpm/mas-infrastructure/capnproto_schemas/gen/go/climate");
enum GCM @0xce396869eede9f10 {
  cccmaCanEsm2 @0;
  ichecEcEarth @1;
  ipslIpslCm5AMr @2;
  mirocMiroc5 @3;
  mpiMMpiEsmLr @4;
  gfdlEsm4 @5;
  ipslCm6aLr @6;
  mpiEsm12Hr @7;
  mriEsm20 @8;
  ukesm10Ll @9;
  gswp3W5E5 @10;
  mohcHadGem2Es @11;
}
enum RCM @0x8671dec53083e351 {
  clmcomCclm4817 @0;
  gericsRemo2015 @1;
  knmiRacmo22E @2;
  smhiRca4 @3;
  clmcomBtuCclm4817 @4;
  mpiCscRemo2009 @5;
  uhohWrf361H @6;
}
enum SSP @0xd3780ae416347aee {
  ssp1 @0;
  ssp2 @1;
  ssp3 @2;
  ssp4 @3;
  ssp5 @4;
}
enum RCP @0x8ef30778310c94cc {
  rcp19 @0;
  rcp26 @1;
  rcp34 @2;
  rcp45 @3;
  rcp60 @4;
  rcp70 @5;
  rcp85 @6;
}
struct EnsembleMember @0xc8caacd1cd5da434 {  # 8 bytes, 0 ptrs
  r @0 :UInt16;  # bits[0, 16)
  i @1 :UInt16;  # bits[16, 32)
  p @2 :UInt16;  # bits[32, 48)
}
struct Metadata @0xfb36d2e966556db0 {  # 0 bytes, 2 ptrs
  entries @0 :List(Entry);  # ptr[0]
  info @1 :Information;  # ptr[1]
  interface Supported @0xab06444b30722e01 {
    categories @0 () -> (types :List(import "/common.capnp".IdInformation));
    supportedValues @1 (typeId :Text) -> (values :List(import "/common.capnp".IdInformation));
  }
  struct Value @0xc48e24c968a234db {  # 16 bytes, 1 ptrs
    union {  # tag bits [0, 16)
      text @0 :Text;  # ptr[0], union tag = 0
      float @1 :Float64;  # bits[64, 128), union tag = 1
      int @2 :Int64;  # bits[64, 128), union tag = 2
      bool @3 :Bool;  # bits[64, 65), union tag = 3
      date @4 :import "/date.capnp".Date;  # ptr[0], union tag = 4
    }
  }
  struct Entry @0x85af7fea06d0820c {  # 8 bytes, 1 ptrs
    union {  # tag bits [16, 32)
      gcm @0 :GCM;  # bits[0, 16), union tag = 0
      rcm @1 :RCM;  # bits[0, 16), union tag = 1
      historical @2 :Void;  # bits[0, 0), union tag = 2
      rcp @3 :RCP;  # bits[0, 16), union tag = 3
      ssp @4 :SSP;  # bits[0, 16), union tag = 4
      ensMem @5 :EnsembleMember;  # ptr[0], union tag = 5
      version @6 :Text;  # ptr[0], union tag = 6
      start @7 :import "/date.capnp".Date;  # ptr[0], union tag = 7
      end @8 :import "/date.capnp".Date;  # ptr[0], union tag = 8
      co2 @9 :Float32;  # bits[32, 64), union tag = 9
      picontrol @10 :Void;  # bits[0, 0), union tag = 10
      description @11 :Text;  # ptr[0], union tag = 11
    }
  }
  interface Information @0xc781edeab8160cb7 {
    forOne @0 (entry :Entry) -> import "/common.capnp".IdInformation;
    forAll @1 () -> (all :List(import "/common.capnp".Pair(Entry, import "/common.capnp".IdInformation)));
  }
}
interface Dataset @0xf635fdd1f05960f0 superclasses(import "/common.capnp".Identifiable, import "/persistence.capnp".Persistent) {
  metadata @0 () -> Metadata;
  closestTimeSeriesAt @1 (latlon :import "/geo.capnp".LatLonCoord) -> (timeSeries :TimeSeries);
  timeSeriesAt @2 (locationId :Text) -> (timeSeries :TimeSeries);
  locations @3 () -> (locations :List(Location));
  streamLocations @4 (startAfterLocationId :Text) -> (locationsCallback :GetLocationsCallback);
  interface GetLocationsCallback @0xd61ba043f14fe175 {
    nextLocations @0 (maxCount :Int64) -> (locations :List(Location));
  }
}
struct MetaPlusData @0xd7a67fec5f22e5a0 {  # 0 bytes, 2 ptrs
  meta @0 :Metadata;  # ptr[0]
  data @1 :Dataset;  # ptr[1]
}
enum Element @0xe35760b4db5ab564 {
  tmin @0;
  tavg @1;
  tmax @2;
  precip @3;
  globrad @4;
  wind @5;
  sunhours @6;
  cloudamount @7;
  relhumid @8;
  airpress @9;
  vaporpress @10;
  co2 @11;
  o3 @12;
  et0 @13;
  dewpointTemp @14;
}
struct Location @0x85ba7385f313fe19 {  # 8 bytes, 4 ptrs
  id @0 :import "/common.capnp".IdInformation;  # ptr[0]
  heightNN @1 :Float32;  # bits[0, 32)
  latlon @2 :import "/geo.capnp".LatLonCoord;  # ptr[1]
  timeSeries @3 :TimeSeries;  # ptr[2]
  customData @4 :List(KV);  # ptr[3]
  struct KV @0xc5fd13a53ae6d46a {  # 0 bytes, 2 ptrs
    key @0 :Text;  # ptr[0]
    value @1 :AnyPointer;  # ptr[1]
  }
}
interface TimeSeries @0xa7769f40fe6e6de8 superclasses(import "/common.capnp".Identifiable, import "/persistence.capnp".Persistent) {
  resolution @0 () -> (resolution :Resolution);
  range @1 () -> (startDate :import "/date.capnp".Date, endDate :import "/date.capnp".Date);
  header @2 () -> (header :List(Element));
  data @3 () -> (data :List(List(Float32)));
  dataT @4 () -> (data :List(List(Float32)));
  subrange @5 (start :import "/date.capnp".Date, end :import "/date.capnp".Date) -> (timeSeries :TimeSeries);
  subheader @6 (elements :List(Element)) -> (timeSeries :TimeSeries);
  metadata @7 () -> Metadata;
  location @8 () -> Location;
  enum Resolution @0xb466cacf63ec03c2 {
    daily @0;
    hourly @1;
  }
}
struct TimeSeriesData @0xf1c1ccf59bc6964f {  # 8 bytes, 4 ptrs
  data @0 :List(List(Float32));  # ptr[0]
  isTransposed @1 :Bool;  # bits[0, 1)
  header @2 :List(Element);  # ptr[1]
  startDate @3 :import "/date.capnp".Date;  # ptr[2]
  endDate @4 :import "/date.capnp".Date;  # ptr[3]
  resolution @5 :TimeSeries.Resolution;  # bits[16, 32)
}
interface Service @0xfe7d08d4352b0c5f superclasses(import "/common.capnp".Identifiable, import "/persistence.capnp".Persistent) {
  getAvailableDatasets @0 () -> (datasets :List(MetaPlusData));
  getDatasetsFor @1 (template :Metadata) -> (datasets :List(Dataset));
}
interface CSVTimeSeriesFactory @0xa418c26cc59929d9 superclasses(import "/common.capnp".Identifiable) {
  create @0 (csvData :Text, config :CSVConfig) -> (timeseries :TimeSeries, error :Text);
  struct CSVConfig @0xeba81ca9f46690b8 {  # 8 bytes, 2 ptrs
    sep @0 :Text = ",";  # ptr[0]
    headerMap @1 :List(import "/common.capnp".Pair(Text, Text));  # ptr[1]
    skipLinesToHeader @2 :Int16;  # bits[0, 16)
    skipLinesFromHeaderToData @3 :Int16 = 1;  # bits[16, 32)
  }
}
interface AlterTimeSeriesWrapper @0xe1f480ef979784b2 superclasses(TimeSeries) {
  wrappedTimeSeries @0 () -> (timeSeries :TimeSeries);
  alteredElements @1 () -> (list :List(Altered));
  alter @2 (desc :Altered, asNewTimeSeries :Bool) -> (timeSeries :TimeSeries);
  remove @3 (alteredElement :Element) -> ();
  struct Altered @0xd085b9baf390bec5 {  # 8 bytes, 0 ptrs
    element @0 :Element;  # bits[0, 16)
    value @1 :Float32;  # bits[32, 64)
    type @2 :AlterType;  # bits[16, 32)
  }
  enum AlterType @0xb5dd785107c358ca {
    add @0;
    mul @1;
  }
}
interface AlterTimeSeriesWrapperFactory @0xc5f12df0a2a52744 superclasses(import "/common.capnp".Identifiable) {
  wrap @0 (timeSeries :TimeSeries) -> (wrapper :AlterTimeSeriesWrapper);
}
