# common.capnp
@0x99f1c9a775a88ac9;
$import "/capnp/c++.capnp".namespace("mas::schema::common");
$import "/capnp/go.capnp".package("common");
$import "/capnp/go.capnp".import("github.com/zalf-rpm/mas-infrastructure/capnproto_schemas/gen/go/common");
struct IdInformation @0xd4cb7ecbfe03dad3 {  # 0 bytes, 3 ptrs
  id @0 :Text;  # ptr[0]
  name @1 :Text;  # ptr[1]
  description @2 :Text;  # ptr[2]
}
interface Identifiable @0xb2afd1cb599c48d5 {
  info @0 () -> IdInformation;
}
struct StructuredText @0xed6c098b67cad454 {  # 8 bytes, 1 ptrs
  value @0 :Text;  # ptr[0]
  structure :group {
    union {  # tag bits [0, 16)
      none @1 :Void;  # bits[0, 0), union tag = 0
      json @2 :Void;  # bits[0, 0), union tag = 1
      xml @3 :Void;  # bits[0, 0), union tag = 2
    }
  }
}
struct Value @0xe17592335373b246 {  # 16 bytes, 1 ptrs
  union {  # tag bits [64, 80)
    f64 @0 :Float64;  # bits[0, 64), union tag = 0
    f32 @1 :Float32;  # bits[0, 32), union tag = 1
    i64 @2 :Int64;  # bits[0, 64), union tag = 2
    i32 @3 :Int32;  # bits[0, 32), union tag = 3
    i16 @4 :Int16;  # bits[0, 16), union tag = 4
    i8 @5 :Int8;  # bits[0, 8), union tag = 5
    ui64 @6 :UInt64;  # bits[0, 64), union tag = 6
    ui32 @7 :UInt32;  # bits[0, 32), union tag = 7
    ui16 @8 :UInt16;  # bits[0, 16), union tag = 8
    ui8 @9 :UInt8;  # bits[0, 8), union tag = 9
    b @10 :Bool;  # bits[0, 1), union tag = 10
    t @11 :Text;  # ptr[0], union tag = 11
    d @12 :Data;  # ptr[0], union tag = 12
    p @13 :AnyPointer;  # ptr[0], union tag = 13
    cap @14 :Capability;  # ptr[0], union tag = 14
    lf64 @15 :List(Float64);  # ptr[0], union tag = 15
    lf32 @16 :List(Float32);  # ptr[0], union tag = 16
    li64 @17 :List(Int64);  # ptr[0], union tag = 17
    li32 @18 :List(Int32);  # ptr[0], union tag = 18
    li16 @19 :List(Int16);  # ptr[0], union tag = 19
    li8 @20 :List(Int8);  # ptr[0], union tag = 20
    lui64 @21 :List(UInt64);  # ptr[0], union tag = 21
    lui32 @22 :List(UInt32);  # ptr[0], union tag = 22
    lui16 @23 :List(UInt16);  # ptr[0], union tag = 23
    lui8 @24 :List(UInt8);  # ptr[0], union tag = 24
    lb @25 :List(Bool);  # ptr[0], union tag = 25
    lt @26 :List(Text);  # ptr[0], union tag = 26
    ld @27 :List(Data);  # ptr[0], union tag = 27
    lcap @28 :List(Capability);  # ptr[0], union tag = 28
  }
}
struct Pair @0xb9d4864725174733 (F, S) {  # 0 bytes, 2 ptrs
  fst @0 :F;  # ptr[0]
  snd @1 :S;  # ptr[1]
}
