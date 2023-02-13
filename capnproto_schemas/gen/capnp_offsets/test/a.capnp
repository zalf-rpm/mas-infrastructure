# a.capnp
@0xc4b468a2826bb79b;
$import "/capnp/c++.capnp".namespace("mas::rpc::test");
$import "/capnp/java.capnp".package("de.zalf.mas");
$import "/capnp/java.capnp".outerClassname("OuterA");
$import "/capnp/go.capnp".package("test");
$import "/capnp/go.capnp".import("github.com/zalf-rpm/mas-infrastructure/capnproto_schemas/gen/go/test");
interface A @0xba9eff6fb3abc84f {
  method @0 (param :Text) -> (res :Text);
}
