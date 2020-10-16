@0x8405c4b8907e57b8;

using Persistent = import "/capnp/persistent.capnp";

using Cxx = import "/capnp/c++.capnp";
$Cxx.namespace("mas::rpc");

using Go = import "lang/go.capnp";
$Go.package("common");
$Go.import("common");

using Common = import "common.capnp".Common;

interface Frontend {

    interface Admin {
        # admin functionality of the frontend

        addPublicKey @0 (srOwner :Common.SturdyRef.Owner, publicKey :Data);
    }

}

