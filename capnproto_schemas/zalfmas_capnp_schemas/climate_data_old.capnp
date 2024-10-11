@0xe4a500b7cd054877;

using Cxx = import "/capnp/c++.capnp";
$Cxx.namespace("mas::rpc");

using Common = import "common.capnp".Common;
using Model = import "model.capnp".Model;
using Geo = import "geo.capnp".Geo;

struct ClimateData {

  interface ListTests {
    testLF32 @0 () -> (test1 :List(Float32));
    testLLF32 @1 () -> (test1 :List(List(Float32)));
    testLI32 @2 () -> (test1 :List(Int32));
    testLLI32 @3 () -> (test1 :List(List(Int32)));
  }

  enum Element {
    tmin @0; # [°C]
    tavg @1; # [°C]
    tmax @2; # [°C]
    precip @3; # [mm]
    globrad @4; # [MJ m-2 d-1]
    wind @5; # [m s-1]
    sunhours @6; # [-]
    cloudamount @7; # [% 0-100]
    relhumid @8; # [% 0-100]
    airpress @9; # []
    vaporpress @10; # [kPa]
    co2 @11; # [ppm]
    o3 @12; # [ppm]
    et0 @13; # []
		dewpointTemp @14; # [°C]
  }

  interface Station extends(Common.Identifiable) {
    # represents a particular (even virtual) climate station of a given climate simulation
    # and also a collection of climate time series at this location

    simulationInfo @0 () -> (simInfo :Common.IdInformation);
    # id information about the climate simulation this climate station belongs to

    heightNN @1 () -> (heightNN :Float32);
    # the stations height over NN

    geoCoord @2 () -> (geoCoord :Geo.Coord);
    # the climate stations geo coordinate

    allTimeSeries @3 () -> (allTimeSeries :List(TimeSeries));
    # get all time series available at this station 

    timeSeriesFor @4 (scenarioId :Text, realizationId :Text) -> (timeSeries :TimeSeries);
    # get the timeSeries at this station for the given scenario and realization
  }

  enum TimeResolution {
    # which time resolution data may have 
    daily @0;
    hourly @1;
  }

  interface TimeSeries {
    # a series of climate elements from start to end date

    resolution @0 () -> (resolution :TimeResolution);
    # the time resolution of the data contained in this time series

    range @1 () -> (startDate :Common.Date, endDate :Common.Date);
    # the date range this time series spans

    header @2 () -> (header :List(Element));
    # the order of the climate elements this time series contains

    data @3 () -> (data :List(List(Float32)));
    # the actual climate data a list (the days) of a list (the actual days data) of data

    dataT @4 () -> (data :List(List(Float32)));
    # the transposed version of the data() method = a list (the elements) of a list (the actual elements data) of data

    subrange @5 (from :Common.Date, to :Common.Date) -> (timeSeries :TimeSeries);
    # create a subrange of the current time series

    subheader @6 (elements :List(Element)) -> (timeSeries :TimeSeries);
    # create a time series with just the given header elements

    simulationInfo @7 () -> (simulationInfo :Common.IdInformation);
    # which simulation does the time series belong to

    scenarioInfo @8 () -> (scenarioInfo :Common.IdInformation);
    # which scenario does the time series belong to

    realizationInfo @9 () -> (realizationInfo :Common.IdInformation);
    # which realization does the time series belong to
  }

  interface Simulation extends(Common.Identifiable) {
    # represents a climate simulation

    scenarios @0 () -> (scenarios :List(Scenario));
    # all the scenarios this simulation offers

    stations @1 () -> (stations :List(Station));
    # all the climate stations this simulation has
  }

  interface Scenario extends(Common.Identifiable) {
    # represents a climate scenario of a particular climate simulation

    simulationInfo @0 () -> (simulationInfo :Common.IdInformation);
    # the climate simulation the scenario belongs to

    realizations @1 () -> (realizations :List(Realization));
    # all climate realizations the scenario offers
  }

  interface Realization extends(Common.Identifiable) {
    # represents a realization of a particular climate scenario

    scenarioInfo @0 () -> (scenarioInfo :Common.IdInformation);
    # the climate scenario this realization belongs to

    closestTimeSeriesAt @1 (geoCoord :Geo.Coord) -> (timeSeries :List(TimeSeries));
    # closest TimeSeries object which represents the whole time series 
    # of the climate realization at the give climate coordinate
  }

  interface Service extends(Common.Identifiable) {
    getAvailableSimulations @0 () -> (availableSimulations :List(Simulation));
    # get a list of all available simulations

    getSimulation @1 (id :UInt64) -> (simulation :Simulation);
    # get a reference to the simulation with given id
  }

}
