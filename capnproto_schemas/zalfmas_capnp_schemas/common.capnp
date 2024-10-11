@0x99f1c9a775a88ac9;

using Cxx = import "/capnp/c++.capnp";
$Cxx.namespace("mas::schema::common");

using Go = import "/capnp/go.capnp";
$Go.package("common");
$Go.import("github.com/zalf-rpm/mas-infrastructure/capnproto_schemas/gen/go/common");

struct IdInformation {
  id @0 :Text; # could be a UUID4
  name @1 :Text;
  description @2 :Text;
}

interface Identifiable {
  # interface to retrieve id information from an object
  info @0 () -> IdInformation;
}

struct StructuredText {
  # some structured text, always encoded in UTF-8

  value @0 :Text;
  # text stream

  structure :union {
    # structural type
    none    @1 :Void; # just normal text
    json    @2 :Void; # it's JSON
    xml     @3 :Void; # it's XML
  }
}

struct Value {
  union {
    f64   @0  :Float64;
    f32   @1  :Float32;
    i64   @2  :Int64;
    i32   @3  :Int32;
    i16   @4  :Int16;
    i8    @5  :Int8;
    ui64  @6  :UInt64;
    ui32  @7  :UInt32;
    ui16  @8  :UInt16;
    ui8   @9  :UInt8;
    b     @10 :Bool;
    t     @11 :Text;
    d     @12 :Data;
    p     @13 :AnyPointer;
    cap   @14 :Capability;
    lf64  @15  :List(Float64);
    lf32  @16  :List(Float32);
    li64  @17  :List(Int64);
    li32  @18  :List(Int32);
    li16  @19  :List(Int16);
    li8   @20  :List(Int8);
    lui64 @21  :List(UInt64);
    lui32 @22  :List(UInt32);
    lui16 @23  :List(UInt16);
    lui8  @24  :List(UInt8);
    lb    @25  :List(Bool);
    lt    @26  :List(Text);
    ld    @27  :List(Data);
    lcap  @28  :List(Capability);
  }
}

#interface Factory(Input, Output) {
  # minimal interface to produce some output from input

#  produce @0 (in :Input) -> (out :Output);
#}

struct Pair(F, S) {
  fst @0 :F;
  snd @1 :S;
}

#struct LL(H, T) {
#  head @0 :H;
#  tail @1 :T;
#}

#interface Clock(T) {
  # represents a syncronizing clock

#  tick @0 (time :T);
  # forward clock one step to time T (which could also be just a Common.Date)
#}

