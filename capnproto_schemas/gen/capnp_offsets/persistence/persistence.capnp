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
struct SturdyRef @0x886d68271d83de4d {  # 0 bytes, 2 ptrs
  vat @0 :VatPath;  # ptr[0]
  localRef @1 :Token;  # ptr[1]
  struct Owner @0xfdd799ed60c87723 {  # 0 bytes, 1 ptrs
    guid @0 :Text;  # ptr[0]
  }
  struct Token @0xfa412bb47f11b488 {  # 8 bytes, 1 ptrs
    union {  # tag bits [0, 16)
      text @0 :Text;  # ptr[0], union tag = 0
      data @1 :Data;  # ptr[0], union tag = 1
    }
  }
}
interface Heartbeat @0x9fb3bdfad147ca3a {
  beat @0 () -> ();
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
  interface ReleaseSturdyRef @0x8f700f81169f2e52 {
    release @0 () -> (success :Bool) $import "/capnp/go.capnp".name("releaseSR");
  }
}
interface Restorer @0x9fb6218427d92e3c {
  restore @0 RestoreParams -> (cap :Capability);
  struct RestoreParams @0xc541e5764a37d73a {  # 0 bytes, 2 ptrs
    localRef @0 :SturdyRef.Token;  # ptr[0]
    sealedBy @1 :SturdyRef.Owner;  # ptr[1]
  }
}
interface HostPortResolver @0xaa8d91fab6d01d9f superclasses(import "/common.capnp".Identifiable, Restorer) {
  resolve @0 (id :Text) -> (host :Text, port :UInt16) $import "/capnp/go.capnp".name("ResolveIdent");
  interface Registrar @0xb0caf775704690b2 {
    register @0 RegisterParams -> (heartbeat :Heartbeat, secsHeartbeatInterval :UInt32);
    struct RegisterParams @0xbf018f62ff460d0f {  # 8 bytes, 3 ptrs
      base64VatId @0 :Text;  # ptr[0]
      host @1 :Text;  # ptr[1]
      port @2 :UInt16;  # bits[0, 16)
      alias @3 :Text;  # ptr[2]
    }
  }
}
interface Gateway @0x8f9c2c0a602f27ed superclasses(import "/common.capnp".Identifiable, Restorer) {
  register @0 (cap :Capability) -> RegResults;
  struct RegResults @0xa232c65d79e97faa {  # 8 bytes, 2 ptrs
    sturdyRef @0 :SturdyRef;  # ptr[0]
    heartbeat @1 :Heartbeat;  # ptr[1]
    secsHeartbeatInterval @2 :UInt32;  # bits[0, 32)
  }
}
