# fbp.capnp
@0xbf602c4868dbb22f;
$import "/capnp/c++.capnp".namespace("mas::schema::fbp");
$import "/capnp/go.capnp".package("fbp");
$import "/capnp/go.capnp".import("github.com/zalf-rpm/mas-infrastructure/capnp_schemas/gen/go/fbp");
interface Component @0xd717ff7d6815a6b0 {
  setupPorts @0 (inPorts :List(NameToPort), outPorts :List(NameToPort)) -> ();
  stop @1 () -> ();
  struct NameToPort @0xf77095186c3c4f65 {  # 0 bytes, 2 ptrs
    name @0 :Text;  # ptr[0]
    port @1 :Capability;  # ptr[1]
  }
}
interface Input @0x9f6bf783c59ae53f {
  close @0 () -> ();
  interface Reader @0xd21817ccd00e3d80 $import "/capnp/c++.capnp".name("InpReader") {
    read @0 () -> (value :AnyPointer);
  }
  interface Writer @0xfb9b181fea82028a $import "/capnp/c++.capnp".name("InpWriter") {
    write @0 (value :AnyPointer) -> ();
  }
}
interface InputArray @0x9dc72eab4c0686c7 {
  send @0 (at :UInt8, data :AnyPointer) -> ();
  close @1 (at :UInt8) -> ();
}
