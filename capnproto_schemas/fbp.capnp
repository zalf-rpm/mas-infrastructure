@0xbf602c4868dbb22f;

using Cxx = import "/capnp/c++.capnp";
$Cxx.namespace("mas::rpc::fbp");

using Go = import "/capnp/go.capnp";
$Go.package("fbp");
$Go.import("github.com/zalf-rpm/mas-infrastructure/capnp_schemas/fbp");

interface Input {
	input @0 (data :Text); # send some data to the input port
}

interface Output {
	output @0 (data :Text); # send some data to the output port
}
