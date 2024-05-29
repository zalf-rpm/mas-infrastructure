@0xc4b468a2826bb79b;

using Cxx = import "/capnp/c++.capnp";
$Cxx.namespace("mas::schema::model::weberest");

using Java = import "/capnp/java.capnp";
$Java.package("de.zalf.mas");
$Java.outerClassname("WebBerestDWDImport");

using Go = import "/capnp/go.capnp";
$Go.package("weberest");
$Go.import("github.com/zalf-rpm/mas-infrastructure/capnproto_schemas/gen/go/model/weberest");

interface DWLABImport {
    importData @0 (id :Text, dwla :Data, dwlb :Data) -> (id :Text, successA :Bool, successB :Bool);
}
