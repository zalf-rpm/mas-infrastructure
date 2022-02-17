@0xf52adf98d2bbc6c0;

using Persistent = import "/capnp/persistent.capnp".Persistent;
using Common = import "common.capnp"

using Cxx = import "/capnp/c++.capnp";
$Cxx.namespace("mas::rpc::service");

using Go = import "/capnp/go.capnp";
$Go.package("service");
$Go.import("github.com/zalf-rpm/mas-infrastructure/capnp_schemas/gen/go/service");

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

  updateIdentity @3 Common.IdInformation;
  # update the identity of this service
}


interface Factory extends(Common.Identifiable) {
  # interface to create service instances

  struct InitParams {
    timeoutSeconds  @0 :UInt64 = 3600;  # 1h
    registrySR      @1 :Text;           # Sturdy reference for service to register itself at
    
  }

  struct AccessInfo {
    adminSR   @0 :Text; # Sturdy reference to Admin interface
    serviceSR @1 :Text; # Sturdy reference to actual service
  }

  create @0 InitParams -> AccessInfo;
  # create a service with the given intial parameters and 
  # receive information how to access the service in return

  


}

