# cluster_admin_service.capnp
@0xf3c1b27d6da9d0fa;
$import "/capnp/c++.capnp".namespace("mas::schema::cluster");
$import "/capnp/go.capnp".package("cluster");
$import "/capnp/go.capnp".import("github.com/zalf-rpm/mas-infrastructure/capnproto_schemas/gen/go/cluster");
struct Cluster @0xf7485d56d6f20e7d {  # 0 bytes, 0 ptrs
  interface Unregister @0xe8b1f7a192651bbe {
    unregister @0 () -> (success :Bool);
  }
  interface AdminMaster @0xbf24278c65f633ce superclasses(import "/common.capnp".Identifiable) {
    registerModelInstanceFactory @0 (aModelId :Text, aFactory :ModelInstanceFactory) -> (unregister :Unregister);
    availableModels @1 () -> (factories :List(ModelInstanceFactory));
  }
  interface UserMaster @0xec42c6df28354b60 superclasses(import "/common.capnp".Identifiable) {
    availableModels @0 () -> (factories :List(ModelInstanceFactory));
  }
  interface Runtime @0xf849848fea5c4776 superclasses(import "/common.capnp".Identifiable) {
    registerModelInstanceFactory @0 (aModelId :Text, aFactory :ModelInstanceFactory) -> (unregister :Unregister);
    availableModels @1 () -> (factories :List(ModelInstanceFactory));
    numberOfCores @2 () -> (cores :Int16);
    freeNumberOfCores @3 () -> (cores :Int16);
    reserveNumberOfCores @4 (reserveCores :Int16, aModelId :Text) -> (reservedCores :Int16);
  }
  struct ZmqPipelineAddresses @0xc9034ba2becc2a64 {  # 0 bytes, 2 ptrs
    input @0 :Text;  # ptr[0]
    output @1 :Text;  # ptr[1]
  }
  interface ValueHolder @0xd6acf080dcf2b4c8 (T) {
    value @0 () -> (val :T);
    release @1 () -> () $import "/capnp/go.capnp".name("releaseValue");
  }
  interface ModelInstanceFactory @0xfd9959998f9f0ebe superclasses(import "/common.capnp".Identifiable) {
    registerModelInstance @5 (instance :Capability, registrationToken :Text = "") -> (unregister :Unregister);
    modelId @4 () -> (id :Text);
    newInstance @0 () -> (instance :ValueHolder);
    newInstances @1 (numberOfInstances :Int16) -> (instances :ValueHolder(List(ValueHolder)));
    newCloudViaZmqPipelineProxies @2 (numberOfInstances :Int16) -> (proxyAddresses :ValueHolder(ZmqPipelineAddresses));
    newCloudViaProxy @3 (numberOfInstances :Int16) -> (proxy :ValueHolder);
    restoreSturdyRef @6 (sturdyRef :Text) -> (cap :ValueHolder);
  }
}
