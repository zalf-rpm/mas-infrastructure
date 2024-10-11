@0xad51a0298da64a5f;

using Persistent = import "persistence.capnp".Persistent;

using Cxx = import "/capnp/c++.capnp";
$Cxx.namespace("mas::schema::frontend");

using Go = import "/capnp/go.capnp";
$Go.package("frontend");
$Go.import("github.com/zalf-rpm/mas-infrastructure/capnproto_schemas/gen/go/frontend");

using Common = import "common.capnp";
#using SturdyRef = import "persistence.capnp".SturdyRef;

#interface Admin {
    # admin functionality of the frontend

#    addPublicKey @0 (srOwner :SturdyRef.Owner, publicKey :Data);
#}

