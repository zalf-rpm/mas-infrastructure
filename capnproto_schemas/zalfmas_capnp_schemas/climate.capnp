@0xa01d3ae410eb4518;

using Cxx = import "/capnp/c++.capnp";
$Cxx.namespace("mas::schema::climate");

using Go = import "/capnp/go.capnp";
$Go.package("climate");
$Go.import("github.com/zalf-rpm/mas-infrastructure/capnproto_schemas/gen/go/climate");

using Persistent = import "persistence.capnp".Persistent;
using Restorer = import "persistence.capnp".Restorer;
using Common = import "common.capnp";
using Model = import "model.capnp";
using Geo = import "geo.capnp";
using Date = import "date.capnp".Date;

enum GCM {
  # global circulation models
  cccmaCanEsm2    @0;   # CCCma-CanESM2
  ichecEcEarth    @1;   # ICHEC-EC-EARTH
  ipslIpslCm5AMr  @2;   # IPSL-IPSL-CM5A-MR
  mirocMiroc5     @3;   # MIROC-MIROC5
  mpiMMpiEsmLr    @4;   # MPI-M-MPI-ESM-LR
  gfdlEsm4        @5;   # GFDL-ESM4
  ipslCm6aLr      @6;   # IPSL-CM6A-LR
  mpiEsm12Hr      @7;   # MPI-ESM1-2-HR
  mriEsm20        @8;   # MRI-ESM2-0
  ukesm10Ll       @9;   # UKESM1-0-LL
  gswp3W5E5       @10;  # Global Soil Wetness Project Phase 3
  mohcHadGem2Es   @11;  # MOHC-HadGEM2-ES
}

enum RCM {
  # regional circulation models
  
  clmcomCclm4817    @0; # CLMcom-CCLM4-8-17
  gericsRemo2015    @1; # GERICS-REMO2015
  knmiRacmo22E      @2; # KNMI-RACMO22E
  smhiRca4          @3; # SMHI-RCA4
  clmcomBtuCclm4817 @4; # CLMcom-BTU-CCLM4-8-17
  mpiCscRemo2009    @5; # MPI-CSC-REMO2009
  uhohWrf361H       @6; # UHOH-WRF361H
}

enum SSP {
  # Shared Socioeconomic Pathways

  ssp1 @0;  # SSP1 # SSP1: Sustainability (Taking the Green Road)
  ssp2 @1;  # SSP2 # SSP2: Middle of the Road
  ssp3 @2;  # SSP3 # SSP3: Regional Rivalry (A Rocky Road)
  ssp4 @3;  # SSP4 # SSP4: Inequality (A Road divided)  
  ssp5 @4;  # SSP5 # SSP5: Fossil-fueled Development (Taking the Highway)
}

enum RCP { 
  # Representative Concentration Pathway

  rcp19 @0; # RCP 1.9 # RCP 1.9 is a pathway that limits global warming to below 1.5 °C, the aspirational goal of the Paris Agreement.
  rcp26 @1; # RCP 2.6 # RCP 2.6 is a "very stringent" pathway.
  rcp34 @2; # RCP 3.4 # As well as just providing another option a variant of RCP 3.4 includes considerable removal of greenhouse gases from the atmosphere.
  rcp45 @3; # RCP 4.5 # Emissions in RCP 4.5 peak around 2040, then decline.
  rcp60 @4; # RCP 6   # In RCP 6, emissions peak around 2080, then decline.
  rcp70 @5; # RCP 7   # RCP 7 is a baseline outcome rather than a mitigation target.
  rcp85 @6; # RCP 8.5 # In RCP 8.5 emissions continue to rise throughout the 21st century.
}

struct EnsembleMember {
  # r<N>i<M>p<L>f<F>

  r @0 :UInt16; # realization number
  i @1 :UInt16; # initialization method indicator
  p @2 :UInt16; # perturbed physics number 
  f @3 :UInt16; # forcing number
}

struct Metadata {
  # metadata describing a set of climate data

  interface Supported {
    # this interface allows access to all supported Metadata elements by the implementing service
  
    #struct Category

    categories @0 () -> (types :List(Common.IdInformation));
    # types can be things like GCMs, RCMs, SSPs, RCPs etc
    
    supportedValues @ 1 (typeId :Text) -> (values :List(Common.IdInformation));
    # the values a supported type can take on
  }

  struct Value {
    union {
      text @0 :Text;
      float @1 :Float64;
      int @2 :Int64;
      bool @3 :Bool;
      date @4 :Date;
    }
  }


  #struct Entry {
  #  typeId @0 :Text;
  #  
  #  value :union {
  #    text @1 :Text;
  #    float @2 :Float64;
  #    int @3 :Int64;
  #    bool @4 :Bool;
  #    date @5 :Date;
  #  }
  #}

  #entries @0 :List(Entry);

  struct Entry {
    union {
      gcm @0 :GCM;
      rcm @1 :RCM;
      historical @2 :Void;
      rcp @3 :RCP;
      ssp @4 :SSP;
      ensMem @5 :EnsembleMember;
      version @6 :Text;
      start @7 :Date;
      end @8 :Date;
      co2 @9 :Float32;
      picontrol @10 :Void;
      description @11 :Text;
    }
  }

  entries @0 :List(Entry);
  # the actual metadata entries

  interface Information {
    forOne @0 (entry :Entry) -> Common.IdInformation;
    forAll @1 () -> (all :List(Common.Pair(Entry, Common.IdInformation)));
  }  
  info @1 :Information;
  # get id information about metadata, if available
}


interface Dataset extends(Common.Identifiable, Persistent) {
  # represent a set of TimeSeries

  metadata            @0 () -> Metadata;
  # get metadata for these data

  closestTimeSeriesAt @1 (latlon :Geo.LatLonCoord) -> (timeSeries :TimeSeries);  
  # closest TimeSeries object which represents the whole time series 
  # of the climate realization at the give climate coordinate

  timeSeriesAt        @2 (locationId :Text) -> (timeSeries :TimeSeries);
  # return time series at location 

  locations           @3 () -> (locations :List(Location));
  # all the climate locations this dataset has

  interface GetLocationsCallback {
    nextLocations @0 (maxCount :Int64) -> (locations :List(Location));
    # get the next locations, if available, but maximum maxCount locations at one time
  }
  streamLocations     @4 (startAfterLocationId :Text) -> (locationsCallback :GetLocationsCallback);
  # stream locations, instead of providing all at once
  # if startAfterLocationId is empty, start at the beginning, else after the location with that id
  # assumes stream returns locations always in same order
}


struct MetaPlusData {
  meta @0 :Metadata;
  data @1 :Dataset;
}


enum Element {
  tmin @0; # [°C] minimum temperature
  tavg @1; # [°C] average temperature
  tmax @2; # [°C] maximum temperature
  precip @3; # [mm] [kg m-2] precipitation
  globrad @4; # [MJ m-2] global radiation
  wind @5; # [m s-1] windspeed
  sunhours @6; # [-] sunshine hours
  cloudamount @7; # [% 0-100] cloudcover
  relhumid @8; # [% 0-100] relative humidity
  airpress @9; # [hPa] air pressure
  vaporpress @10; # [hPa] vapor pressure
  co2 @11; # [ppm] atmospheric CO2 concentration
  o3 @12; # [ppm] atmospheric O3 concentration
  et0 @13; # [] 
  dewpointTemp @14; # dew point temperature [°C]
  specificHumidity @15; # [g kg-1] = [(g water vapor) (kg air)-1]
  snowfallFlux @16; # [mm] [kg m-2] snowfall flux
  surfaceDownwellingLongwaveRadiation @17; # [MJ m-2] surface downwelling longwave radiation
}


struct Location {
  # represents a particular (even virtual) climate location

  id          @0 :Common.IdInformation;
  # id information for this location

  heightNN    @1 :Float32;
  # the locations height over NN

  latlon      @2 :Geo.LatLonCoord;
  # the latitude/longitude coordinate of the location

  timeSeries  @3 :TimeSeries;
  # time series at this location

  struct KV {
    key   @0 :Text;
    value @1 :AnyPointer;
  }
  customData  @4 :List(KV);
  # custom data for this location, e.g. row/column in a grid etc.
}


interface TimeSeries extends(Common.Identifiable, Persistent) {
  # a series of climate elements from start to end date

  enum Resolution {
    # which time resolution data may have 
    daily @0;
    hourly @1;
  }

  resolution @0 () -> (resolution :Resolution);
  # the time resolution of the data contained in this time series

  range @1 () -> (startDate :Date, endDate :Date);
  # the date range this time series spans

  header @2 () -> (header :List(Element));
  # the order of the climate elements this time series contains

  data @3 () -> (data :List(List(Float32)));
  # the actual climate data a list (the days) of a list (the actual days data) of data
  # e.g. [[tavg-day1, precip-day1, globrad-day-1, ...], [tavg-day2, precip-day2, globrad-day-2, ...], ...]

  dataT @4 () -> (data :List(List(Float32)));
  # the transposed version of the data() method = a list (the elements) of a list (the actual elements data) of data
  # e.g. [[tavg-day1, tavg-day-2, ...], [precip-day1, precip-day2, ...], [globrad-day-1, globrad-day-2, ...]]

  subrange @5 (start :Date, end :Date) -> (timeSeries :TimeSeries);
  # create a subrange of the current time series
  # if "from" or "to" are not set, creates a subrange from the start or to the end of the current time series

  subheader @6 (elements :List(Element)) -> (timeSeries :TimeSeries);
  # create a time series with just the given header elements

  metadata @7 () -> Metadata;
  # the metadata for this time series

  location @8 () -> Location;
  # location of this time series
}


struct TimeSeriesData {
  # a complete set of time series data as plain data, useful for instance in FBP flows

  data          @0 :List(List(Float32));
  # the actual data

  isTransposed  @1 :Bool;
  # is the data or dataT result of TimeSeries

  header        @2 :List(Element);
  # the header information belonging to the data

  startDate     @3 :Date;
  endDate       @4 :Date;
  # start and end date of the data

  resolution    @5 :TimeSeries.Resolution = daily;
  # resolution of the data
}


interface Service extends(Common.Identifiable, Persistent) {
  # climate data service 

  getAvailableDatasets @0 () -> (datasets :List(MetaPlusData));
  # get a list of all available datasets

  getDatasetsFor @1 (template :Metadata) -> (datasets :List(Dataset));
  # get all datasets matching the given metadata template

  #supportedMetadata @2 () -> (cap :Metadata.Supported);
  # return a capability to an interface describing the service's supported metadata
}


interface CSVTimeSeriesFactory extends(Common.Identifiable) {
  # create TimeSeries capabilities from data

  struct CSVConfig {
    sep @0 :Text = ",";
    # which separator is being used in the csv data

    headerMap @1 :List(Common.Pair(Text, Text));
    # list of mappings from header names in csv data to supported ones, e.g. windspeed -> wind, avg_temp -> tavg, ...

    skipLinesToHeader @2 :Int16 = 0;
    # how many lines to skip until header line (counted from top of file)

    skipLinesFromHeaderToData @3 :Int16 = 1;
    # how many lines to skip until data start (counted from header line to start of data)
  }

  create @0 (csvData :Text, config :CSVConfig) -> (timeseries :TimeSeries, error :Text);
  # create a time series from the given structured text
}


interface AlterTimeSeriesWrapper extends(TimeSeries) {
  # wraps a time series, but allows to alter the data represented by the time series

  wrappedTimeSeries @0 () -> (timeSeries :TimeSeries);
  # return the actually wrapped time series capability

  struct Altered {
    element @0 :Element;
    value @1 :Float32;
    type @2 :AlterType;
  }

  alteredElements @1 () -> (list :List(Altered));
  # list the elements and their altered value

  enum AlterType {
    add @0;
    mul @1;
  }

  alter @2 (desc :Altered, asNewTimeSeries :Bool = false)  -> (timeSeries :TimeSeries);
  # given the description "desc" alter element by some amount depending on declared type and optionally create a new time series capability for this alteration

  remove @3 (alteredElement :Element);
  # remove the altered element

  replaceWrappedTimeSeries @4 (timeSeries :TimeSeries); 
  # replace the wrapped time series 
}


interface AlterTimeSeriesWrapperFactory extends(Common.Identifiable) {
  # create AlterTimeSeriesWrapper capabilities from existing TimeSeries capabilities

  wrap @0 (timeSeries :TimeSeries) -> (wrapper :AlterTimeSeriesWrapper);
  # wrap the given time series and return the wrapper
}
