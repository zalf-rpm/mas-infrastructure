@0xad51a0298da64a5f;

using Persistent = import "/capnp/persistent.capnp";

using Cxx = import "/capnp/c++.capnp";
$Cxx.namespace("mas::rpc::frontend");

using Go = import "lang/go.capnp";
$Go.package("common");
$Go.import("common");

using Common = import "common.capnp".Common;
using SturdyRef = import "persistence.capnp".SturdyRef;

interface Admin {
    # admin functionality of the frontend

    addPublicKey @0 (srOwner :SturdyRef.Owner, publicKey :Data);
}

