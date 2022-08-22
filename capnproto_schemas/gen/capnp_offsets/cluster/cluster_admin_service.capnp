# cluster_admin_service.capnp
@0xf3c1b27d6da9d0fa;
$import "/capnp/c++.capnp".namespace("mas::schema::cluster");
$import "/capnp/go.capnp".package("cluster");
$import "/capnp/go.capnp".import("github.com/zalf-rpm/mas-infrastructure/capnp_schemas/gen/go/cluster");
struct Cluster @0xf7485d56d6f20e7d {  # 0 bytes, 0 ptrs
  interface AdminMaster @0xbf24278c65f633ce superclasses(import "/common.capnp".Identifiable) {
    registerModelInstanceFactory @0 (aModelId :Text, aFactory :ModelInstanceFactory) -> (unregister :import "/common.capnp".Callback);
    availableModels @1 () -> (factories :List(ModelInstanceFactory));
  }
  interface UserMaster @0xec42c6df28354b60 superclasses(import "/common.capnp".Identifiable) {
    availableModels @0 () -> (factories :List(ModelInstanceFactory));
  }
  interface Runtime @0xf849848fea5c4776 superclasses(import "/common.capnp".Identifiable) {
    registerModelInstanceFactory @0 (aModelId :Text, aFactory :ModelInstanceFactory) -> (unregister :import "/common.capnp".Callback);
    availableModels @1 () -> (factories :List(ModelInstanceFactory));
    numberOfCores @2 () -> (cores :Int16);
    freeNumberOfCores @3 () -> (cores :Int16);
    reserveNumberOfCores @4 (reserveCores :Int16, aModelId :Text) -> (reservedCores :Int16);
  }
  interface ModelInstanceFactory @0xfd9959998f9f0ebe superclasses(import "/common.capnp".Identifiable) {
    registerModelInstance @5 (instance :Capability, registrationToken :Text = "") -> (unregister :import "/common.capnp".Callback);
    modelId @4 () -> (id :Text);
    newInstance @0 () -> (instance :import "/common.capnp".CapHolder);
    newInstances @1 (numberOfInstances :Int16) -> (instances :import "/common.capnp".CapHolder(List(import "/common.capnp".ListEntry(import "/common.capnp".CapHolder))));
    newCloudViaZmqPipelineProxies @2 (numberOfInstances :Int16) -> (proxyAddresses :import "/common.capnp".CapHolder(import "/common.capnp".ZmqPipelineAddresses));
    newCloudViaProxy @3 (numberOfInstances :Int16) -> (proxy :import "/common.capnp".CapHolder);
    restoreSturdyRef @6 (sturdyRef :Text) -> (cap :import "/common.capnp".CapHolder);
  }
}
