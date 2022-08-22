# common.capnp
@0x99f1c9a775a88ac9;
$import "/capnp/c++.capnp".namespace("mas::schema::common");
$import "/capnp/go.capnp".package("common");
$import "/capnp/go.capnp".import("github.com/zalf-rpm/mas-infrastructure/capnp_schemas/gen/go/common");
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
interface Callback @0x902904cd51bff117 {
  call @0 () -> ();
}
interface Action @0x9dd4e2c3d76e4587 superclasses(import "/persistence.capnp".Persistent) {
  do @0 () -> ();
}
interface Action1 @0xc12db9a9ae07a763 superclasses(import "/persistence.capnp".Persistent) {
  do @0 (p :AnyPointer) -> ();
}
interface Factory @0xa869f50b8c586ed9 (Input, Output) {
  produce @0 (in :Input) -> (out :Output);
}
struct ZmqPipelineAddresses @0xfe04fe97ba25a27e {  # 0 bytes, 2 ptrs
  input @0 :Text;  # ptr[0]
  output @1 :Text;  # ptr[1]
}
interface CapHolder @0xcac9c6537df1a097 (Object) {
  cap @0 () -> (object :Object);
  release @1 () -> () $import "/capnp/go.capnp".name("releaseCap");
}
interface IdentifiableHolder @0xee543d7c305d56f6 superclasses(Identifiable) {
  cap @0 () -> (cap :Identifiable);
  release @1 () -> () $import "/capnp/go.capnp".name("releaseCap");
}
struct ListEntry @0xc201bf46dd40051e (PointerType) {  # 0 bytes, 1 ptrs
  entry @0 :PointerType;  # ptr[0]
}
interface Stopable @0xce7e4202f09e314a {
  stop @0 () -> ();
}
struct Pair @0xb9d4864725174733 (F, S) {  # 0 bytes, 2 ptrs
  fst @0 :F;  # ptr[0]
  snd @1 :S;  # ptr[1]
}
struct LL @0xd67792aa3fc241be (H, T) {  # 0 bytes, 2 ptrs
  head @0 :H;  # ptr[0]
  tail @1 :T;  # ptr[1]
}
interface Clock @0xa8b91e2c1f8c929a (T) {
  tick @0 (time :T) -> ();
}
struct IP @0xd39ff99bbab1a74e {  # 0 bytes, 2 ptrs
  attributes @0 :List(KV);  # ptr[0]
  content @1 :AnyPointer;  # ptr[1]
  struct KV @0xb07588184ad8aac5 {  # 0 bytes, 2 ptrs
    key @0 :Text;  # ptr[0]
    value @1 :AnyPointer;  # ptr[1]
  }
}
interface Channel @0xf0c0f9413a3083be (V) {
  setBufferSize @0 (size :UInt64 = 1) -> ();
  reader @1 () -> (r :Reader);
  writer @2 () -> (w :Writer);
  endpoints @3 () -> (r :Reader, w :Writer);
  setAutoCloseSemantics @4 (cs :CloseSemantics) -> ();
  close @5 (waitForEmptyBuffer :Bool = true) -> ();
  enum CloseSemantics @0x956ee3f21ad6b221 {
    fbp @0;
    no @1;
  }
  struct Msg @0x876b422c6839e6b2 {  # 8 bytes, 1 ptrs
    union {  # tag bits [0, 16)
      value @0 :V;  # ptr[0], union tag = 0
      done @1 :Void;  # bits[0, 0), union tag = 1
    }
  }
  interface Reader @0x9c656810b30decd7 $import "/capnp/c++.capnp".name("ChanReader") {
    read @0 () -> Msg;
    close @1 () -> ();
  }
  interface Writer @0x9b5844944dc0f458 $import "/capnp/c++.capnp".name("ChanWriter") {
    write @0 Msg -> ();
    close @1 () -> ();
  }
}
