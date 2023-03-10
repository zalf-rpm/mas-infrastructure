@0xf98a24e1969df972;

using Cxx = import "/capnp/c++.capnp";
$Cxx.namespace("mas::schema::crop");

using Go = import "/capnp/go.capnp";
$Go.package("crop");
$Go.import("github.com/zalf-rpm/mas-infrastructure/capnproto_schemas/gen/go/crop");

using Common = import "common.capnp";
using Persistent = import "persistence.capnp".Persistent;
using Registry = import "registry.capnp".Registry;

interface Crop extends(Common.Identifiable, Persistent) {
  # represents a crop

  species     @2 () -> (info :Common.IdInformation);
  # what species is this crop

  cultivar    @1 () -> (info :Common.IdInformation);
  # what is the cultivar of this crop

  parameters  @0 () -> (params :AnyPointer);
  # pointer to a model specific parameters
}

interface Service extends(Registry) {}
# a crop service should be just a registry.capnp::Registry of crops