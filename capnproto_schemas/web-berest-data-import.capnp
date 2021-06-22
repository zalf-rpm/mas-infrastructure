@0xc4b468a2826bb79b;

using Cxx = import "/capnp/c++.capnp";
$Cxx.namespace("mas::rpc");

using Java = import "/capnp/java.capnp";
$Java.package("de.zalf.mas");
$Java.outerClassname("WebBerestDWDImport");

interface DWLABImport {
    importData @0 (dwla :Text, dwlb :Text) -> (success :Bool);
}