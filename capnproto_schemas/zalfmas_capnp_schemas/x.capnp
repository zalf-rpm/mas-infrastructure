@0xffd06af2f026177b;

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
