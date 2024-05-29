@0xf3c1b27d6da9d0fa;

using Cxx = import "/capnp/c++.capnp";
$Cxx.namespace("mas::schema::cluster");

using Go = import "/capnp/go.capnp";
$Go.package("cluster");
$Go.import("github.com/zalf-rpm/mas-infrastructure/capnproto_schemas/gen/go/cluster");

using Identifiable = import "common.capnp".Identifiable;
using Model = import "model.capnp";

struct Cluster {

  interface Unregister {
    unregister @0 () -> (success :Bool);
  }

  interface AdminMaster extends(Identifiable) {
    # entry point for cluster administration
    # is responsible to actually manage connected ClusterAdminNodes

    registerModelInstanceFactory @0 (aModelId :Text, aFactory :ModelInstanceFactory) -> (unregister :Unregister);
    # register a model instance factory for the given model id

    availableModels @1 () -> (factories :List(ModelInstanceFactory));
    # get instance factories to all the available models on registered runtimes
  }


  interface UserMaster extends(Identifiable) {
    # entry point for users (bootstrap capability)
    # probably basically a facade around AdminMaster

    availableModels @0 () -> (factories :List(ModelInstanceFactory));
    # get instance factories to all the available models to the user
  }


  interface Runtime extends(Identifiable) {
    # interface representing a runtime for models
    # could be the whole cluster (when used with SLURM), a single cluster node or a different system
    # the runtime controls the resources of a single system, but doesn't know about any particular model or it's requirements
    # -> so the Runtime holds just general information about the system, memory use, number of cores used etc.
    
    registerModelInstanceFactory @0 (aModelId :Text, aFactory :ModelInstanceFactory) -> (unregister :Unregister);
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

  struct ZmqPipelineAddresses {
    input @0 :Text;
    output @1 :Text;
  }

  interface ValueHolder(T) {
    # an interface to a remote value 

    value @0 () -> (val :T);
    # get the referenced value

    release @1 () $Go.name("releaseValue"); 
    # release value on server side (signaling that value isn't needed anymore)
  }


  interface ModelInstanceFactory extends(Identifiable) {
    # interface to create model instances on a particular runtime

    registerModelInstance @5 (instance :Capability, registrationToken :Text = "") -> (unregister :Unregister);
    #registerModelInstance @5 [X] (instance :X, registrationToken :Text = "") -> (unregister :Callback);
    # register the given instance of some model optionally given the registration token and receive a
    # callback to unregister this instance again

    modelId @4 () -> (id :Text);
    # return the id of the model this factory creates instances of
    
    newInstance @0 () -> (instance :ValueHolder);
    # return a new instance of the model

    #newInstances @1 (numberOfInstances :Int16) -> (instances :ValueHolder(List(ValueHolder)));
    newInstances @1 (numberOfInstances :Int16) -> (instances :ValueHolder(List(ValueHolder)));
    # return the requested number of model instances

    newCloudViaZmqPipelineProxies @2 (numberOfInstances :Int16) -> (proxyAddresses :ValueHolder(ZmqPipelineAddresses));
    # return the TCP addresses of two ZeroMQ proxies to access the given number of instances of the model

    newCloudViaProxy @3 (numberOfInstances :Int16) -> (proxy :ValueHolder);
    # return a model proxy acting as a gateway to the requested number of model instances

    restoreSturdyRef @6 (sturdyRef :Text) -> (cap :ValueHolder);
    # return the capability holder for the given sturdyRef if available
  }
}




