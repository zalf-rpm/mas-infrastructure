@0xc4b468a2826bb79b;


using Cxx = import "/capnp/c++.capnp";
$Cxx.namespace("mas::rpc");

interface A {
    method @0 (param :Text) -> (res :Text);
    m @1 (id :UInt64) -> (count :UInt64);
}

interface S {
    getCB @0 () -> (cb :CB);
}

interface CB {
    getD @0 () -> (d :D);
}

interface D {
    getData @0 () -> (i :UInt64, data :Data);#List(UInt64));
}

struct X {
    t @0 :Text;
}