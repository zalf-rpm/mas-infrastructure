@0xd3f8859c7688b76b;

using Cxx = import "/capnp/c++.capnp";
$Cxx.namespace("mas::rpc");

#using Go = import "lang/go.capnp";
#$Go.package("dataServices");
#$Go.import("dataServices");

using Common = import "common.capnp".Common;
using Soil = import "soil_data.capnp".Soil
using Climate = import "climate_data.capnp".Climate
using Model = import "model.capnp".Model

struct Service {

  enum ServiceType {
    unknown @0 ; # an unknown service
    soilService @1; #Soil.Service
    climateService @2; #Climate.Service
    modelInstanceFactory @3; #Model.InstanceFactory
  }

  interface Registry extends(Common.Identifiable) {
    # the bootstrap interface to the different services
    
    struct Entry {
      regToken @0 :Text;
      type @1 :ServiceType;
      service @1 :Common.Identifiable; # a service should at least be identifiable
    }

    struct Query {
      union {
        all @0 :Void;
        type @1 :ServiceType;
      }
    }

    getAvailableServices @0 Query -> (services :List(Entry));
    # get all available services according to the query

    getService [Service] @7 (regToken :Text) -> (services :Service);
    # get service registered under given registration token

    registerService @2 (type :ServiceType, service :Common.Identifiable) -> (regToken :Text, unreg :Common.Registry.Unregister);
    # register a service with a certain type 
    # returns the registration token to get the service again and an unregister capability
  }

}