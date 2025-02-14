@0xe7e7e2edc72e660c;

using Cxx = import "/capnp/c++.capnp";
$Cxx.namespace("mas::schema::jobs");

using Go = import "/capnp/go.capnp";
$Go.package("jobs");
$Go.import("github.com/zalf-rpm/mas-infrastructure/capnproto_schemas/gen/go/jobs");

using Persistent = import "persistence.capnp".Persistent;
using Identifiable = import "common.capnp".Identifiable;

struct Job(Payload) {
    data            @0 :Payload;
    noFurtherJobs   @1 :Bool = false;
}

interface Service extends(Identifiable, Persistent) {
    nextJob @0 () -> (job :Job);
}