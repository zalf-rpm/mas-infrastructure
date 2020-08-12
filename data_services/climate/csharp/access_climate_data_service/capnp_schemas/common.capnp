@0x99f1c9a775a88ac9;

using Persistent = import "/capnp/persistent.capnp";

using Cxx = import "/capnp/c++.capnp";
$Cxx.namespace("mas::rpc");

using Go = import "/lang/go.capnp";
$Go.package("common");
$Go.import("common");

struct Common {

  struct IdInformation {
    id @0 :Text; # could be a UUID4
    name @1 :Text;
    description @2 :Text;
  }

  interface Identifiable {
    # interface to retrieve id information from an object
    info @0 () -> (info :IdInformation);
  }
  
  struct Date {
    # A standard Gregorian calendar date.

    year @0 :Int16;
    # The year. Must include the century.
    # Negative value indicates BC.

    month @1 :UInt8;   # Month number, 1-12.
    day @2 :UInt8;     # Day number, 1-31.
  }
  
  struct StructuredText {
    # some structured text, always encoded in UTF-8

    value @0 :Text;
    # text stream

    structure :union {
      # structural type

      none @1 :Void; # just normal text
      json @2 :Void; # it's JSON
      xml @3 :Void; # it's XML
    }
  }


  interface Callback {
    # interface to a callback object

    call @0 () -> ();
    # call the function associated with the callback object
  }


  interface Registry {
    # an interface for registering 

    interface Unregister {
      # interface to unregister objects

      unregister @0 ();
      # unregister a previously registered object
    }

    register @0 [Object] (object :Object, registrationToken :Text = "") -> (unregister :Unregister);
    # register the given object using optionally the given token and return a capability to unregister again
    # deleting this capability or calling unregister on it will do the same 
  }


  struct ZmqPipelineAddresses {
    input @0 :Text;
    output @1 :Text;
  }


  interface CapHolder(CapType) extends(Persistent.Persistent(Text, Text)) {
    # hold a capability aka a pointer to an object
    # give the sender the chance to know when the CapHolder object isn't used any more

    cap @0 () -> (cap :CapType);
    # reference to some capability, which can be any pointer type like a :List(Capability) or just a :Capability

    release @1 ();
    # release capability on server side (signaling that capability cap isn't needed anymore)
  }


  struct ListEntry(PointerType) {
    entry @0 :PointerType;
  }


  #interface Cancelable {
  #  cancel @0 () -> ();
  #}


  interface Stopable {
    # a capability to stop something
    stop @0 () -> ();
  }

}
