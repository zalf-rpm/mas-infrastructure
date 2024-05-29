# config.capnp
@0x9c934ced19460717;
$import "/capnp/c++.capnp".namespace("mas::schema::config");
$import "/capnp/go.capnp".package("config");
$import "/capnp/go.capnp".import("github.com/zalf-rpm/mas-infrastructure/capnproto_schemas/gen/go/config");
interface Service @0x860d660620aefcda (C) {
  nextConfig @0 () -> (config :C, noFurtherConfigs :Bool);
}
