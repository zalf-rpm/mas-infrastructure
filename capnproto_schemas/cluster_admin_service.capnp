@0xf3c1b27d6da9d0fa;

using Cxx = import "/capnp/c++.capnp";
$Cxx.namespace("mas::rpc");

using Go = import "/capnp/go.capnp";
$Go.package("cluster");
$Go.import("github.com/zalf-rpm/mas-infrastructure/capnp_schemas/cluster");

using Common = import "common.capnp";
using Model = import "model.capnp";
#using Geo = import "geo_coord.capnp".Geo;

struct Cluster {

  interface AdminMaster extends(Common.Identifiable) {
    # entry point for cluster administration
    # is responsible to actually manage connected ClusterAdminNodes

    registerModelInstanceFactory @0 (aModelId :Text, aFactory :ModelInstanceFactory) -> (unregister :Common.Callback);
    # register a model instance factory for the given model id

    availableModels @1 () -> (factories :List(ModelInstanceFactory));
    # get instance factories to all the available models on registered runtimes
  }


  interface UserMaster extends(Common.Identifiable) {
    # entry point for users (bootstrap capability)
    # probably basically a facade around AdminMaster

    availableModels @0 () -> (factories :List(ModelInstanceFactory));
    # get instance factories to all the available models to the user
  }


  interface Runtime extends(Common.Identifiable) {
    # interface representing a runtime for models
    # could be the whole cluster (when used with SLURM), a single cluster node or a different system
    # the runtime controls the resources of a single system, but doesn't know about any particular model or it's requirements
    # -> so the Runtime holds just general information about the system, memory use, number of cores used etc.
    
    registerModelInstanceFactory @0 (aModelId :Text, aFactory :ModelInstanceFactory) -> (unregister :Common.Callback);
    # register a model instance factory for the given model id

    availableModels @1 () -> (factories :List(ModelInstanceFactory));
    # the model instance factories this runtime has access to

    numberOfCores @2 () -> (cores :Int16);
    # how many cores does the runtime offer

    freeNumberOfCores @3 () -> (cores :Int16);
    # how many cores are still unused

    reserveNumberOfCores @4 (reserveCores :Int16, aModelId :Text) -> (reservedCores :Int16);
    # try to reserve number of reserveCores for model aModelId and return the number of actually reservedCores
  }

  

  interface ModelInstanceFactory extends(Common.Identifiable) {
    # interface to create model instances on a particular runtime

    registerModelInstance @5 (instance :Capability, registrationToken :Text = "") -> (unregister :Common.Callback);
    #registerModelInstance @5 [X] (instance :X, registrationToken :Text = "") -> (unregister :Common.Callback);
    # register the given instance of some model optionally given the registration token and receive a
    # callback to unregister this instance again

    modelId @4 () -> (id :Text);
    # return the id of the model this factory creates instances of
    
    newInstance @0 () -> (instance :Common.CapHolder);
    # return a new instance of the model

    #newInstances @1 (numberOfInstances :Int16) -> (instances :Common.CapHolder(List(Common.CapHolder)));
    newInstances @1 (numberOfInstances :Int16) -> (instances :Common.CapHolder(List(Common.ListEntry(Common.CapHolder))));
    # return the requested number of model instances

    newCloudViaZmqPipelineProxies @2 (numberOfInstances :Int16) -> (proxyAddresses :Common.CapHolder(Common.ZmqPipelineAddresses));
    # return the TCP addresses of two ZeroMQ proxies to access the given number of instances of the model

    newCloudViaProxy @3 (numberOfInstances :Int16) -> (proxy :Common.CapHolder);
    # return a model proxy acting as a gateway to the requested number of model instances

    restoreSturdyRef @6 (sturdyRef :Text) -> (cap :Common.CapHolder);
    # return the capability holder for the given sturdyRef if available
  }
}




