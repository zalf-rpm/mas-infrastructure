# storage.capnp
@0x9755d0b34b9db39d;
$import "/capnp/c++.capnp".namespace("mas::schema::storage");
$import "/capnp/go.capnp".package("storage");
$import "/capnp/go.capnp".import("github.com/zalf-rpm/mas-infrastructure/capnproto_schemas/gen/go/storage");
interface Store @0xe69f958aa2386f06 superclasses(import "/common.capnp".Identifiable, import "/persistence.capnp".Persistent) {
  newContainer @0 (name :Text, description :Text) -> (container :Container);
  containerWithId @1 (id :Text) -> (container :Container);
  listContainers @2 () -> (containers :List(Container));
  removeContainer @3 (id :Text) -> (success :Bool);
  importContainer @4 (json :Text) -> (container :Container);
  interface Container @0x878131f45567ae62 superclasses(import "/common.capnp".Identifiable, import "/persistence.capnp".Persistent) {
    export @0 () -> (json :Text);
    downloadEntries @1 () -> (entries :List(import "/common.capnp".Pair(Text, Entry.Value)));
    listEntries @2 () -> (entries :List(Entry));
    getEntry @3 (key :Text) -> (entry :Entry);
    addEntry @6 (key :Text, value :Entry.Value, replaceExisting :Bool) -> (entry :Entry, success :Bool);
    removeEntry @4 (key :Text) -> (success :Bool);
    clear @5 () -> (success :Bool);
    interface Entry @0xfa1a243e7bf478c0 {
      getKey @0 () -> (key :Text);
      getValue @1 () -> (value :Value, isUnset :Bool);
      setValue @2 (value :Value) -> (success :Bool);
      struct Value @0xe2185cc449928f5c {  # 16 bytes, 1 ptrs
        union {  # tag bits [16, 32)
          boolValue @0 :Bool;  # bits[0, 1), union tag = 0
          boolListValue @1 :List(Bool);  # ptr[0], union tag = 1
          int8Value @2 :Int8;  # bits[0, 8), union tag = 2
          int8ListValue @3 :List(Int8);  # ptr[0], union tag = 3
          int16Value @4 :Int16;  # bits[0, 16), union tag = 4
          int16ListValue @5 :List(Int16);  # ptr[0], union tag = 5
          int32Value @6 :Int32;  # bits[32, 64), union tag = 6
          int32ListValue @7 :List(Int32);  # ptr[0], union tag = 7
          int64Value @8 :Int64;  # bits[64, 128), union tag = 8
          int64ListValue @9 :List(Int64);  # ptr[0], union tag = 9
          uint8Value @10 :UInt8;  # bits[0, 8), union tag = 10
          uint8ListValue @11 :List(UInt8);  # ptr[0], union tag = 11
          uint16Value @12 :UInt16;  # bits[0, 16), union tag = 12
          uint16ListValue @13 :List(UInt16);  # ptr[0], union tag = 13
          uint32Value @14 :UInt32;  # bits[32, 64), union tag = 14
          uint32ListValue @15 :List(UInt32);  # ptr[0], union tag = 15
          uint64Value @16 :UInt64;  # bits[64, 128), union tag = 16
          uint64ListValue @17 :List(UInt64);  # ptr[0], union tag = 17
          float32Value @18 :Float32;  # bits[32, 64), union tag = 18
          float32ListValue @19 :List(Float32);  # ptr[0], union tag = 19
          float64Value @20 :Float64;  # bits[64, 128), union tag = 20
          float64ListValue @21 :List(Float64);  # ptr[0], union tag = 21
          textValue @22 :Text;  # ptr[0], union tag = 22
          textListValue @23 :List(Text);  # ptr[0], union tag = 23
          dataValue @24 :Data;  # ptr[0], union tag = 24
          dataListValue @25 :List(Data);  # ptr[0], union tag = 25
          anyValue @26 :AnyStruct;  # ptr[0], union tag = 26
        }
      }
    }
  }
  struct ImportExportData @0x847d262cefd2f142 {  # 0 bytes, 3 ptrs
    info @0 :import "/common.capnp".IdInformation;  # ptr[0]
    entries @1 :List(import "/common.capnp".Pair(Text, Container.Entry.Value));  # ptr[1]
    isAnyValue @2 :List(Bool);  # ptr[2]
  }
}
