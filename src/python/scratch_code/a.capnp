@0xc4b468a2826bb79b;

using Cxx = import "/capnp/c++.capnp";
$Cxx.namespace("mas::rpc::test");

using Java = import "/capnp/java.capnp";
$Java.package("de.zalf.mas");
$Java.outerClassname("OuterA");

using Go = import "/capnp/go.capnp";
$Go.package("test");
$Go.import("github.com/zalf-rpm/mas-infrastructure/capnproto_schemas/gen/go/test");

struct Env(RestInput) {
  rest @0 :RestInput;
  timeSeries @1 :Capability;
  soilProfile @2 :Capability;
}

interface A(S, T) {
    method @0 (param :Env(S)) -> (res :T);
}