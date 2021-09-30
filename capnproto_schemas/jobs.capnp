@0xe7e7e2edc72e660c;

using Cxx = import "/capnp/c++.capnp";
$Cxx.namespace("mas::rpc::jobs");

using LatLngCoord = import "geo_coord.capnp".LatLonCoord;

struct Job {
    latLngCoords    @0 :List(LatLngCoord);
    noFurtherJobs   @1 :Bool = false;
}

interface Service {
    nextJob @0 () -> (job :Job);
}