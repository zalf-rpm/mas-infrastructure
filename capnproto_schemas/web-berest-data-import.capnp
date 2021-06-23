@0xc4b468a2826bb79b;

using Cxx = import "/capnp/c++.capnp";
$Cxx.namespace("mas::rpc");

using Java = import "/capnp/java.capnp";
$Java.package("de.zalf.mas");
$Java.outerClassname("WebBerestDWDImport");

interface DWLABImport {
    importData @0 (id :Text, dwla :Text, dwlb :Text) -> (id :Text, successA :Bool, successB :Bool);
}