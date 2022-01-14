@0xe7e7e2edc72e660c;

using Cxx = import "/capnp/c++.capnp";
$Cxx.namespace("mas::rpc::jobs");

using Go = import "/capnp/go.capnp";
$Go.package("jobs");
$Go.import("github.com/zalf-rpm/mas-infrastructure/capnp_schemas/jobs");

using LatLngCoord = import "geo_coord.capnp".LatLonCoord;

struct Job {
    latLngCoords    @0 :List(LatLngCoord);
    noFurtherJobs   @1 :Bool = false;
}

interface Service {
    nextJob @0 () -> (job :Job);
}