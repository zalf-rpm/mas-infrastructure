@0xf98a24e1969df972;

using Cxx = import "/capnp/c++.capnp";
$Cxx.namespace("mas::rpc::crop");

using Common = import "common.capnp".Common;

interface Crop extends(Common.Identifiable) {
    # represents a crop

    parameters @0 () -> (params :AnyPointer);
    # pointer to a model specific parameter struct
}