@0xd3f8859c7688b76b;

using Cxx = import "/capnp/c++.capnp";
$Cxx.namespace("mas::rpc::service");

using Go = import "/capnp/go.capnp";
$Go.package("registry");
$Go.import("github.com/zalf-rpm/mas-infrastructure/capnp_schemas/registry");

using Common = import "common.capnp";
using Soil = import "soil_data.capnp";
using Climate = import "climate_data.capnp";
using Model = import "model.capnp";

enum ServiceType {
    unknown @0 ; # an unknown service
    soil @1; #Soil.Service
    climate @2; #Climate.Service
    modelInstanceFactory @3; #Model.InstanceFactory
    modelEnvInstance @4; #Model.EnvInstance
}

interface Registry extends(Common.Identifiable) {
# the bootstrap interface to the different services
    
    struct Entry {
        regToken @0 :Text;
        type @1 :ServiceType;
        service @2 :Common.Identifiable; # a service should at least be identifiable
    }

    struct Query {
        union {
        all @0 :Void;
        type @1 :ServiceType;
        }
    }

    getAvailableServices @0 Query -> (services :List(Entry));
    # get all available services according to the query

    getService @1 [Service] (regToken :Text) -> (services :Service);
    # get service registered under given registration token

    registerService @2 (type :ServiceType, service :Common.Identifiable) -> (regToken :Text, unreg :Common.Callback);
    # register a service with a certain type 
    # returns the registration token to get the service again and an unregister capability
}
