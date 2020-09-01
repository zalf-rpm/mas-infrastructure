@0xc4b468a2826bb79b;

using Cxx = import "/capnp/c++.capnp";
$Cxx.namespace("mas::rpc");

interface A {
    method @0 (param :Text) -> (res :Text);
}