@0x9c934ced19460717;

using Cxx = import "/capnp/c++.capnp";
$Cxx.namespace("mas::schema::config");

using Go = import "/capnp/go.capnp";
$Go.package("config");
$Go.import("github.com/zalf-rpm/mas-infrastructure/capnp_schemas/gen/go/config");

interface Service(C) {
    # service to supply 

    createConfig @0 () -> (config :C, noFurtherConfigs :Bool = false);
    # create a configuration 
}