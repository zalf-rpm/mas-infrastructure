@0xf52adf98d2bbc6c0;

using Cxx = import "/capnp/c++.capnp";
$Cxx.namespace("mas::rpc::service");

using Go = import "/capnp/go.capnp";
$Go.package("service");
$Go.import("github.com/zalf-rpm/mas-infrastructure/capnp_schemas/gen/go/service");

using Common = import "common.capnp";
using Restorer = import "persistence.capnp".Restorer;

interface Admin {
  # interface to administer service

  heartbeat @0 ();
  # keep service alive

  setTimeout @1 (seconds :UInt64);
  # change timeout when service will stop itself
  # within this timeout a heartbeat message has to be received 
  # or the service received a normal message

  stop @2 ();
  # stop this service immediately

  identity @3 () -> Common.IdInformation;

  updateIdentity @4 Common.IdInformation;
  # update the identity of this service
}


interface SimpleFactory extends(Common.Identifiable) {
  # minimal interface to create service instances

  create @0 () -> (caps :List(Common.Identifiable));
  # create a service, receiving a list of capabilities 
  # to the services interfaces implementing the Identifiable interface
}


interface Factory(Payload) extends(Common.Identifiable) {
  # configurable interface to create service instances with user choosen timeout and registration capabilities

  struct CreateParams {
    timeoutSeconds    @0 :UInt64 = 3600;                  # 1h
    interfaceNameToRegistrySR  @1 :List(Common.Pair(Text, Text));  # where should the created service register it's interfaces via given sturdy references
    msgPayload        @2 :Payload;                        # generic payload to create message
  }

  struct AccessInfo {
    adminCap    @0 :Capability;                 # Capability to Admin interface
    serviceCaps @1 :List(Common.Identifiable);  # List of capabilities to actual service
    error       @2 :Text;                       # error message if create failed
  }

  create @0 CreateParams -> AccessInfo;
  # create a service with the given parameters and 
  # receive information how to access the service in return

  serviceInterfaceNames @1 () -> (names :List(Text)); 
  # return a list of the names the factory will return on a create message
  # these names are the ones to be used for the sturdy ref maps in CreateParams
}
