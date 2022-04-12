@0xbf602c4868dbb22f;

using Cxx = import "/capnp/c++.capnp";
$Cxx.namespace("mas::schema::fbp");

using Go = import "/capnp/go.capnp";
$Go.package("fbp");
$Go.import("github.com/zalf-rpm/mas-infrastructure/capnp_schemas/gen/go/fbp");

interface Component {
	# act as bootstrap for component thread

	struct NameToPort {
		name 	@0 :Text;
		port	@1 :Capability;
	}

	setInputPorts 	@0 (ports :List(NameToPort));
	# set the connected input ports

	setOutputPorts 	@1 (ports :List(NameToPort));
	# set the connected input ports

	stop 			@2 ();
	# stop the component
}

interface Input {
	# input port 

	interface Reader $Cxx.name("InpReader") {
		read @0 () -> (value :AnyPointer);
	}

	interface Writer $Cxx.name("InpWriter") {
		write @0 (value :AnyPointer);
	}

	#send 	@0 (data :AnyPointer); 
	# send some data to the input port

	close 	@0 ();
	# close this port
}


interface InputArray {
	# array input port which consists of a dynamic set of ports of the same type
	send 	@0 (at :UInt8, data :AnyPointer); 
	# send some data to array port at

	close 	@1 (at :UInt8);
	# close array port at at
}

interface Config {
	# configure interface to setup an FBP component via sturdy refs

	struct NameToSR {
		name 	@0 :Text;
		sr		@1 :Text;
	}

	setup @0 (config :List(NameToSR));
	# the component will connect it's output ports with name to the downstream 
	# component via the given sturdy reference sr
}