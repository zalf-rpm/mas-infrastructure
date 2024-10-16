# fbp.capnp
@0xbf602c4868dbb22f;
$import "/capnp/c++.capnp".namespace("mas::schema::fbp");
$import "/capnp/go.capnp".package("fbp");
$import "/capnp/go.capnp".import("github.com/zalf-rpm/mas-infrastructure/capnproto_schemas/gen/go/fbp");
struct IP @0xaf0a1dc4709a5ccf {  # 0 bytes, 2 ptrs
  attributes @0 :List(KV);  # ptr[0]
  content @1 :AnyPointer;  # ptr[1]
  struct KV @0x9e9e5391e0c499e6 {  # 0 bytes, 3 ptrs
    key @0 :Text;  # ptr[0]
    desc @1 :Text;  # ptr[1]
    value @2 :AnyPointer;  # ptr[2]
  }
}
interface Channel @0x9c62c32b2ff2b1e8 (V) superclasses(import "/common.capnp".Identifiable, import "/persistence.capnp".Persistent) {
  setBufferSize @0 (size :UInt64 = 1) -> ();
  reader @1 () -> (r :Reader);
  writer @2 () -> (w :Writer);
  endpoints @3 () -> (r :Reader, w :Writer);
  setAutoCloseSemantics @4 (cs :CloseSemantics) -> ();
  close @5 (waitForEmptyBuffer :Bool = true) -> ();
  enum CloseSemantics @0xa8d787cae7e0b243 {
    fbp @0;
    no @1;
  }
  struct Msg @0xd5b512f4bcd0aa2e {  # 8 bytes, 1 ptrs
    union {  # tag bits [0, 16)
      value @0 :V;  # ptr[0], union tag = 0
      done @1 :Void;  # bits[0, 0), union tag = 1
    }
  }
  struct StartupInfo @0xe3d7a3237f175028 {  # 16 bytes, 3 ptrs
    bufferSize @0 :UInt64;  # bits[0, 64)
    closeSemantics @1 :CloseSemantics;  # bits[64, 80)
    channelSR @2 :Text;  # ptr[0]
    readerSRs @3 :List(Text);  # ptr[1]
    writerSRs @4 :List(Text);  # ptr[2]
  }
  interface Reader @0x8bc69192f3bc97cc $import "/capnp/c++.capnp".name("ChanReader") {
    read @0 () -> Msg;
    close @1 () -> ();
  }
  interface Writer @0xf7fec613b4a8c79f $import "/capnp/c++.capnp".name("ChanWriter") {
    write @0 Msg -> ();
    close @1 () -> ();
  }
}
interface PortCallbackRegistrar @0x8dff741cb4dfa00c {
  registerCallback @0 (callback :PortCallback) -> ();
  interface PortCallback @0xbcdf87a68541a8ef {
    newInPort @0 (name :Text, readerCap :Channel(IP).Reader) -> ();
    newOutPort @1 (name :Text, writerCap :Channel(IP).Writer) -> ();
  }
}
