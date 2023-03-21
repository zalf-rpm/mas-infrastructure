@0x9273388a9624d430;

using Cxx = import "/capnp/c++.capnp";
$Cxx.namespace("mas::schema::model");

using Go = import "/capnp/go.capnp";
$Go.package("models");
$Go.import("github.com/zalf-rpm/mas-infrastructure/capnproto_schemas/gen/go/models");

using Common = import "common.capnp";
using Geo = import "geo.capnp";
using Climate = import "climate.capnp";
using Soil = import "soil.capnp";
using Mgmt = import "management.capnp";
using Persistent = import "persistence.capnp".Persistent;
using Restorer = import "restore_resolve.capnp".Restorer;

struct XYResult {
  xs @0 :List(Float64); # x axis values
  ys @1 :List(Float64); # y axis values
}

struct Stat {
  # statistics

  enum Type {
    min @0; # maximum
    max @1; # minimum
    sd @2; # standard deviation
    avg @3; # average
    median @4; # median
  }

  type @0 :Type = avg; #type
  vs @1 :List(Float64); # values
}

struct XYPlusResult {
  # a result including potentially some statistical information like min, max and standard deviation

  xy @0 :XYResult; # xy result
  stats @1 :List(Stat); # additional results
}


interface ClimateInstance extends(Common.Identifiable) {
  # an interface to run a "climate" model, which is basically using just climate data

  run @0 (timeSeries :Climate.TimeSeries) -> (result :XYResult);
  # run model just on a single time series

  runSet @1 (dataset :List(Climate.TimeSeries)) -> (result :XYPlusResult);
  # run model on a set of time series
}


struct Env(RestInput) {

  rest @0 :RestInput;
  # rest of environment (often Common.StructuredText)

  timeSeries @1 :Climate.TimeSeries;
  # climate data  

  soilProfile @2 :Soil.Profile;
  # soil profile to use for a model run

  mgmtEvents @3 :List(Mgmt.Event);
  # a list of management events
}


interface EnvInstance(RestInput, Output) extends(Common.Identifiable, Persistent, Common.Stopable) {
  # an interface to run a model against an environment of input data

  run @0 (env :Env(RestInput)) -> (result :Output);
  # run a model via an input environment and return result (often as Common.StructuredText)
}


interface EnvInstanceProxy(RestInput, Output) extends(EnvInstance(RestInput, Output)) {
  # the EnvInstance interface with the ability to forward run messages to registered EnvInstances

  registerEnvInstance @0 (instance :EnvInstance(RestInput, Output)) -> (unregister :Common.Action);
  # register an instance of an EnvInstance model to get jobs transparently forwarded to by the proxy
}


interface InstanceFactory(InstanceType) extends(Common.Identifiable) {
  # interface to create unshared model instances 

  modelInfo @0 () -> Common.IdInformation;
  # return information about the model this factory creates instances of
  
  newInstance @1 () -> (instance :InstanceType);
  # return a new instance of the model

  newInstances @2 (numberOfInstances :Int16) -> (instances :List(Common.ListEntry(InstanceType)));
  # return the requested number of model instances
}
