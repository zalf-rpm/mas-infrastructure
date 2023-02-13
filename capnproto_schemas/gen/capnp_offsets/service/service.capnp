# service.capnp
@0xf52adf98d2bbc6c0;
$import "/capnp/c++.capnp".namespace("mas::schema::service");
$import "/capnp/go.capnp".package("service");
$import "/capnp/go.capnp".import("github.com/zalf-rpm/mas-infrastructure/capnproto_schemas/gen/go/service");
interface Admin @0xfec1f88b198df649 superclasses(import "/common.capnp".Identifiable) {
  heartbeat @0 () -> ();
  setTimeout @1 (seconds :UInt64) -> ();
  stop @2 () -> ();
  identities @3 () -> (infos :List(import "/common.capnp".IdInformation));
  updateIdentity @4 (oldId :Text, newInfo :import "/common.capnp".IdInformation) -> ();
}
interface SimpleFactory @0xaba5829222c213cb superclasses(import "/common.capnp".Identifiable) {
  create @0 () -> (caps :List(import "/common.capnp".Identifiable));
}
interface Factory @0x8ab0ecb99c269c7f (Payload) superclasses(import "/common.capnp".Identifiable) {
  create @0 CreateParams -> AccessInfo;
  serviceInterfaceNames @1 () -> (names :List(Text));
  struct CreateParams @0xc2b88517ccaa9197 {  # 8 bytes, 2 ptrs
    timeoutSeconds @0 :UInt64 = 3600;  # bits[0, 64)
    interfaceNameToRegistrySR @1 :List(import "/common.capnp".Pair(Text, Text));  # ptr[0]
    msgPayload @2 :Payload;  # ptr[1]
  }
  struct AccessInfo @0xb9816a53df7cb62e {  # 0 bytes, 3 ptrs
    adminCap @0 :Capability;  # ptr[0]
    serviceCaps @1 :List(import "/common.capnp".Identifiable);  # ptr[1]
    error @2 :Text;  # ptr[2]
  }
}
