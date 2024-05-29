@0xc4b468a2826bb79b;

using Cxx = import "/capnp/c++.capnp";
$Cxx.namespace("mas::rpc::test");

using Java = import "/capnp/java.capnp";
$Java.package("de.zalf.mas");
$Java.outerClassname("OuterA");

using Go = import "/capnp/go.capnp";
$Go.package("test");
$Go.import("github.com/zalf-rpm/mas-infrastructure/capnproto_schemas/gen/go/test");

interface A {
    method @0 (param :Text) -> (res :Text);
}