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
struct Pair @0xb9d4864725174733 (F, S) {  # 0 bytes, 2 ptrs
  fst @0 :F;  # ptr[0]
  snd @1 :S;  # ptr[1]
}
