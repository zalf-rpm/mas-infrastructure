# model.capnp
@0x9273388a9624d430;
$import "/capnp/c++.capnp".namespace("mas::schema::model");
$import "/capnp/go.capnp".package("models");
$import "/capnp/go.capnp".import("github.com/zalf-rpm/mas-infrastructure/capnproto_schemas/gen/go/models");
struct XYResult @0x851d47c6ccdecf08 {  # 0 bytes, 2 ptrs
  xs @0 :List(Float64);  # ptr[0]
  ys @1 :List(Float64);  # ptr[1]
}
struct Stat @0xa6be2e805ea10a68 {  # 8 bytes, 1 ptrs
  type @0 :Type = avg;  # bits[0, 16)
  vs @1 :List(Float64);  # ptr[0]
  enum Type @0xbd33bb6d8cbd9ed2 {
    min @0;
    max @1;
    sd @2;
    avg @3;
    median @4;
  }
}
struct XYPlusResult @0x8f86b66260d02d1d {  # 0 bytes, 2 ptrs
  xy @0 :XYResult;  # ptr[0]
  stats @1 :List(Stat);  # ptr[1]
}
interface ClimateInstance @0xdfcfeb783c4948fc superclasses(import "/common.capnp".Identifiable) {
  run @0 (timeSeries :import "/climate.capnp".TimeSeries) -> (result :XYResult);
  runSet @1 (dataset :List(import "/climate.capnp".TimeSeries)) -> (result :XYPlusResult);
}
struct Env @0xb7fc866ef1127f7c (RestInput) {  # 0 bytes, 4 ptrs
  rest @0 :RestInput;  # ptr[0]
  timeSeries @1 :import "/climate.capnp".TimeSeries;  # ptr[1]
  soilProfile @2 :import "/soil.capnp".Profile;  # ptr[2]
  mgmtEvents @3 :List(import "/management.capnp".Event);  # ptr[3]
}
interface EnvInstance @0xa5feedafa5ec5c4a (RestInput, Output) superclasses(import "/common.capnp".Identifiable, import "/persistence.capnp".Persistent, import "/service.capnp".Stopable) {
  run @0 (env :Env(RestInput)) -> (result :Output);
}
interface EnvInstanceProxy @0x87cbebfc1164a24a (RestInput, Output) superclasses(EnvInstance(RestInput, Output)) {
  registerEnvInstance @0 (instance :EnvInstance(RestInput, Output)) -> (unregister :Unregister);
  interface Unregister @0xc727892bd5c66f88 {
    unregister @0 () -> (success :Bool);
  }
}
interface InstanceFactory @0xce552eef738a45ea superclasses(import "/common.capnp".Identifiable) {
  modelInfo @0 () -> import "/common.capnp".IdInformation;
  newInstance @1 () -> (instance :import "/common.capnp".Identifiable);
  newInstances @2 (numberOfInstances :Int16) -> (instances :List(import "/common.capnp".Identifiable));
}
