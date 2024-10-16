@0xffd06af2f026177b;

using Go = import "/capnp/go.capnp";
$Go.package("test");
$Go.import("github.com/zalf-rpm/mas-infrastructure/capnproto_schemas/gen/go/test");

struct S {
    c @0 :X;
}

interface X {
    m @0 (i :Int64) -> (t :Text);
}

interface Y {
    m @0 (hello :Text);
}

interface A {
    m @0 (n :Int64) -> (r :Float64);
}
