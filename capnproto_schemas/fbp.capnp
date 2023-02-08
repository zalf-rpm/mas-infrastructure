@0xbf602c4868dbb22f;

using Cxx = import "/capnp/c++.capnp";
$Cxx.namespace("mas::schema::fbp");

using Go = import "/capnp/go.capnp";
$Go.package("fbp");
$Go.import("github.com/zalf-rpm/mas-infrastructure/capnproto_schemas/gen/go/fbp");

interface Component {
	# act as bootstrap for component thread

	struct NameToPort {
		name 	@0 :Text;
		port	@1 :Capability;
	}

	setupPorts 		@0 (inPorts :List(NameToPort), outPorts :List(NameToPort));
	# setup the ports

	stop 			@1 ();
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
