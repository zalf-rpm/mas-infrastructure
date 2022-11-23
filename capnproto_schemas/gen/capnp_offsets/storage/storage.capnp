# storage.capnp
@0x9755d0b34b9db39d;
$import "/capnp/c++.capnp".namespace("mas::schema::storage");
$import "/capnp/go.capnp".package("storage");
$import "/capnp/go.capnp".import("github.com/zalf-rpm/mas-infrastructure/capnp_schemas/gen/go/storage");
interface Store @0xe69f958aa2386f06 {
  newContainer @0 import "/common.capnp".IdInformation -> (container :Container);
  containerWithId @1 (id :Text) -> (container :Container);
  listContainers @2 () -> (containers :List(Container));
  removeContainer @3 (id :Text) -> (success :Bool);
  interface Container @0x878131f45567ae62 superclasses(import "/common.capnp".Identifiable, import "/persistence.capnp".Persistent) {
    importData @0 (data :Data) -> (success :Bool);
    exportData @1 () -> (data :Data);
    listObjects @2 () -> (objects :List(Object));
    getObject @3 (key :Text) -> (object :Object);
    addObject @4 (object :Object) -> (success :Bool);
    removeObject @5 (key :Text) -> (success :Bool);
    clear @6 () -> (success :Bool);
    struct Object @0xaa5c23f54650f8ae {  # 16 bytes, 2 ptrs
      key @0 :Text;  # ptr[0]
      value :group {
        union {  # tag bits [16, 32)
          boolValue @1 :Bool;  # bits[0, 1), union tag = 0
          intValue @2 :Int64;  # bits[64, 128), union tag = 1
          floatValue @3 :Float64;  # bits[64, 128), union tag = 2
          textValue @4 :Text;  # ptr[1], union tag = 3
          dataValue @5 :Data;  # ptr[1], union tag = 4
          anyValue @6 :AnyStruct;  # ptr[1], union tag = 5
        }
      }
    }
  }
}
