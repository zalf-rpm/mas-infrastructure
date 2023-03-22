# persistence.capnp
@0x855efed3475f6b26;
$import "/capnp/c++.capnp".namespace("mas::schema::persistence");
$import "/capnp/go.capnp".package("persistence");
$import "/capnp/go.capnp".import("github.com/zalf-rpm/mas-infrastructure/capnproto_schemas/gen/go/persistence");
struct VatId @0xe10a5d74d58bd18d {  # 32 bytes, 0 ptrs
  publicKey0 @0 :UInt64;  # bits[0, 64)
  publicKey1 @1 :UInt64;  # bits[64, 128)
  publicKey2 @2 :UInt64;  # bits[128, 192)
  publicKey3 @3 :UInt64;  # bits[192, 256)
}
struct Address @0xfb47810671a05b0d {  # 24 bytes, 1 ptrs
  union {  # tag bits [144, 160)
    ip6 :group {  # union tag = 0
      lower64 @0 :UInt64;  # bits[0, 64)
      upper64 @1 :UInt64;  # bits[64, 128)
    }
    host @3 :Text;  # ptr[0], union tag = 1
  }
  port @2 :UInt16;  # bits[128, 144)
}
struct VatPath @0xd9eccdf2dbc48087 {  # 0 bytes, 2 ptrs
  id @0 :VatId;  # ptr[0]
  address @1 :Address;  # ptr[1]
}
struct SturdyRef @0x886d68271d83de4d {  # 8 bytes, 1 ptrs
  union {  # tag bits [0, 16)
    transient @0 :Transient;  # ptr[0], union tag = 0
    stored @1 :Stored;  # ptr[0], union tag = 1
  }
  struct Owner @0xfdd799ed60c87723 {  # 0 bytes, 1 ptrs
    guid @0 :Text;  # ptr[0]
  }
  struct Transient @0xa42bd461f2a8a3c8 {  # 0 bytes, 2 ptrs
    vat @0 :VatPath;  # ptr[0]
    localRef @1 :AnyPointer;  # ptr[1]
  }
  struct Stored @0xcbe679a401315eb8 {  # 32 bytes, 0 ptrs
    key0 @0 :UInt64;  # bits[0, 64)
    key1 @1 :UInt64;  # bits[64, 128)
    key2 @2 :UInt64;  # bits[128, 192)
    key3 @3 :UInt64;  # bits[192, 256)
  }
}
interface Persistent @0xc1a7daa0dc36cb65 {
  save @0 SaveParams -> SaveResults;
  struct SaveParams @0xd5e0aac4225e0343 {  # 0 bytes, 1 ptrs
    sealFor @0 :SturdyRef.Owner;  # ptr[0]
  }
  struct SaveResults @0xdc5bd1ef982cec13 {  # 0 bytes, 2 ptrs
    sturdyRef @0 :SturdyRef;  # ptr[0]
    unsaveSR @1 :SturdyRef;  # ptr[1]
  }
}
interface Restorer @0x9fb6218427d92e3c {
  restore @0 RestoreParams -> (cap :Capability);
  struct RestoreParams @0xc541e5764a37d73a {  # 0 bytes, 2 ptrs
    localRef @0 :AnyPointer;  # ptr[0]
    sealedFor @1 :SturdyRef.Owner;  # ptr[1]
  }
}
interface HostPortResolver @0xaa8d91fab6d01d9f superclasses(import "/common.capnp".Identifiable, Restorer) {
  resolve @0 (id :Text) -> (host :Text, port :UInt16);
  interface Registrar @0xb0caf775704690b2 {
    register @0 (base64VatId :Text, host :Text, port :UInt16, alias :Text) -> (heartbeat :Heartbeat, secsHeartbeatInterval :UInt32);
    interface Heartbeat @0x87de92d2d68df26f {
      beat @0 () -> ();
    }
  }
}
