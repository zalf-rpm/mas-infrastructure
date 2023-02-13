# crop.capnp
@0xf98a24e1969df972;
$import "/capnp/c++.capnp".namespace("mas::schema::crop");
$import "/capnp/go.capnp".package("crop");
$import "/capnp/go.capnp".import("github.com/zalf-rpm/mas-infrastructure/capnproto_schemas/gen/go/crop");
interface Crop @0xe88d97a324bf5c84 superclasses(import "/common.capnp".Identifiable, import "/persistence.capnp".Persistent) {
  species @2 () -> (info :import "/common.capnp".IdInformation);
  cultivar @1 () -> (info :import "/common.capnp".IdInformation);
  parameters @0 () -> (params :AnyPointer);
}
