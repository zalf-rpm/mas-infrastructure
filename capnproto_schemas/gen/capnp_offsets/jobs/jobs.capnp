# jobs.capnp
@0xe7e7e2edc72e660c;
$import "/capnp/c++.capnp".namespace("mas::schema::jobs");
$import "/capnp/go.capnp".package("jobs");
$import "/capnp/go.capnp".import("github.com/zalf-rpm/mas-infrastructure/capnp_schemas/gen/go/jobs");
struct Job @0xa05b60b71ca38848 (Payload) {  # 8 bytes, 1 ptrs
  data @0 :Payload;  # ptr[0]
  noFurtherJobs @1 :Bool;  # bits[0, 1)
}
interface Service @0xb8745454d013cbf0 superclasses(import "/common.capnp".Identifiable, import "/persistence.capnp".Persistent) {
  nextJob @0 () -> (job :Job);
}
