@0xc5af57f16cf2c8fd;

using Cxx = import "/capnp/c++.capnp";
$Cxx.namespace("mas::schema::persistence");

using Go = import "/capnp/go.capnp";
$Go.package("persistence");
$Go.import("github.com/zalf-rpm/mas-infrastructure/capnproto_schemas/gen/go/persistence");

using Common = import "common.capnp";
using SturdyRef = import "persistence.capnp".SturdyRef;

interface Restorer {
  # restore a capability from a sturdy ref
  
  #interface Save {
  #  struct SaveParams {
  #    cap           @0 :Capability;
  #
  #    fixedSRToken  @1 :AnyPointer;
  #
  #    sealFor       @2 :SturdyRef.Owner;
  #
  #    createUnsave  @3 :Bool = true;
  #
  #    restoreToken  @4 :AnyPointer;
  #  }
  #
  #  struct SaveResults {
  #    sturdyRef @0 :SturdyRef;
  #    # the sturdy ref to be used to restore the capability
  #
  #    unsaveSR @1 :SturdyRef;
  #    # sturdy ref refering to an Common.Action capability to unsave the referenced capability
  #  }
  #
  #  save @0 SaveParams -> SaveResults;
  #}

  struct RestoreParams {
    localRef @0 :AnyPointer;
    # local reference (sturdy ref token) to the capability to be restored

    sealedFor @1 :SturdyRef.Owner;
    # the owner of the sturdy ref to be restored
    # if everybody is allowed to restore the capability, this field should be null (unset)
    # if sealedFor is set, the localRef must be signed by the private key of the owner matching 
    # the public key registered with the restorer service else the capability cannot be restored
  }

  restore @0 RestoreParams -> (cap :Capability);
  # restore from the localRef in a transient sturdy ref as live capability
}

interface HostPortResolver extends(Common.Identifiable, Restorer) {
  # resolve an id (either base64 encoded VatId or a text alias) to a host and port
  # acts also as restorer for the registrar capabilities

  interface Registrar { 
    # register a services base64 encoded VatId to a host and port (and optionally a plain text alias)

    register @0 (base64VatId :Text, host :Text, port :UInt16, alias :Text) -> (heartbeat :Common.Action, secsHeartbeatInterval :UInt32);
    # register a vat-id to a host and port and optionally an alias
    # returns a capability which is a Common.Action to call regularly to keep the registration alive
    # call heartbeat at least every secsHeartbeatInterval seconds
    # if a heartbeat ist missed, the id will be unregistered
  }

  resolve @0 (id :Text) -> (host :Text, port :UInt16);
  # resolve an id (either base64 encoded VatId or plain text alias) to a host and port
}



