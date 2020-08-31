@0xd3f8859c7688b76b;

using Cxx = import "/capnp/c++.capnp";
$Cxx.namespace("mas::rpc");

#using Go = import "lang/go.capnp";
#$Go.package("dataServices");
#$Go.import("dataServices");

using Common = import "common.capnp".Common;

struct Service {

  struct Type {
    enum Fixed {
      soil @0; #Soil.Service
      climate @1; #Climate.Service
    }

    union {
      fixed @0 :Fixed;
      other @1 :Text;
      none @2 :Void;
    }
  }

  interface Registry extends(Common.Identifiable) {
    # the bootstrap interface to the different data services
    
    struct Entry {
      registerId @0 :Text;
      type @1 :Type;
      service @2 :Common.Identifiable;
    }

    struct QueryType {
      union {
        all @0 :Void;
        type @1 :Type;
      }
    }

    getAvailableServices @0 QueryType -> (services :List(Entry));

    getService @1 (id :Text) -> Entry;

    interface Unregister {
      unregister @0 ();
    }

    registerService @2 (type :Type, service :Common.Identifiable) -> (unregister :Unregister);
  }

}