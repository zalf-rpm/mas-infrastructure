@0xa01d3ae410eb4518;

using Cxx = import "/capnp/c++.capnp";
$Cxx.namespace("mas::rpc");

using Common = import "common.capnp".Common;
using Model = import "model.capnp".Model;
using Geo = import "geo_coord.capnp".Geo;
using Date = import "date.capnp".Date;

struct Climate {

  enum GCM {
    # global circulation models
    cccmaCanEsm2 @0;
    ichecEcEarth @1;
    ipslIpslCm5AMr @2;
    mirocMiroc5 @3;
    mpiMMpiEsmLr @4;
  }

  enum RCM {
    # regional circulation models
    
    clmcomCclm4817 @0;
    gericsRemo2015 @1;
    knmiRacmo22E @2;
    smhiRca4 @3;
    clmcomBtuCclm4817 @4;
    mpiCscRemo2009 @5;
  }

  enum SSP {
    # Shared Socioeconomic Pathways

    ssp1 @0; # SSP1: Sustainability (Taking the Green Road)
    ssp2 @1; # SSP2: Middle of the Road
    ssp3 @2; # SSP3: Regional Rivalry (A Rocky Road)
    ssp4 @3; # SSP4: Inequality (A Road divided)  
    ssp5 @4; # SSP5: Fossil-fueled Development (Taking the Highway)
  }

  enum RCP { 
    # Representative Concentration Pathway

    rcp19 @0; # RCP1.9 is a pathway that limits global warming to below 1.5 °C, the aspirational goal of the Paris Agreement.
    rcp26 @1; # RCP 2.6 is a "very stringent" pathway.
    rcp34 @2; # As well as just providing another option a variant of RCP3.4 includes considerable removal of greenhouse gases from the atmosphere.
    rcp45 @3; # Emissions in RCP 4.5 peak around 2040, then decline.
    rcp60 @4; # In RCP 6, emissions peak around 2080, then decline.
    rcp70 @5; # RCP7 is a baseline outcome rather than a mitigation target.
    rcp85 @6; # In RCP 8.5 emissions continue to rise throughout the 21st century.
  }

  struct EnsembleMember {
    # r<N>i<M>p<L>

    r @0 :UInt16; # realization number
    i @1 :UInt16; # initialization method indicator
    p @2 :UInt16; # perturbed physics number 
  }
  
  struct Metadata {
    # metadata describing a set of climate data

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
      }
    }

    entries @0 :List(Entry);
    # the actual metadata entries

    interface Info {
      forOne @0 (entry :Entry) -> Common.IdInformation;
      forAll @1 () -> (all :List(Common.Pair(Entry, Common.IdInformation)));
    }  
    info @1 :Info;
    # get id information about metadata, if available
  }


  interface Dataset {
    metadata @0 () -> Metadata;
    # get metadata for these data

    closestTimeSeriesAt @1 (geoCoord :Geo.Coord) -> (timeSeries :TimeSeries);  
    # closest TimeSeries object which represents the whole time series 
    # of the climate realization at the give climate coordinate

    timeSeriesAt @2 (locationId :Text) -> (timeSeries :TimeSeries);
    # return time series at location 

    locations @3 () -> (locations :List(Location));
    # all the climate locations this dataset has
  }


  struct MetaPlusData {
    meta @0 :Metadata;
    data @1 :Dataset;
  }


  enum Element {
    tmin @0; # [°C] minimum temperature
    tavg @1; # [°C] average temperature
    tmax @2; # [°C] maximum temperature
    precip @3; # [mm] precipitation
    globrad @4; # [MJ m-2 d-1] global radiation
    wind @5; # [m s-1] windspeed
    sunhours @6; # [-] sunshine hours
    cloudamount @7; # [% 0-100] cloudcover
    relhumid @8; # [% 0-100] relative humidity
    airpress @9; # [] air pressure
    vaporpress @10; # [kPa] vapor pressure
    co2 @11; # [ppm] atmospheric CO2 concentration
    o3 @12; # [ppm] atmospheric O3 concentration
    et0 @13; # [] 
		dewpointTemp @14; # [°C]
  }


  struct Location {
    # represents a particular (even virtual) climate location

    id @0 :Common.IdInformation;
    # id information for this location

    heightNN @1 :Float32;
    # the locations height over NN

    geoCoord @2 :Geo.Coord;
    # the locations geo coordinate

    timeSeries @3 :TimeSeries;
    # time series at this location
  }


  interface TimeSeries {
    # a series of climate elements from start to end date

    enum Resolution {
      # which time resolution data may have 
      daily @0;
      hourly @1;
    }

    resolution @0 () -> (resolution :Resolution);
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

    metadata @7 () -> Metadata;
    # the metadata for this time series

    location @8 () -> Location;
    # location of this time series
  }

  interface Service extends(Common.Identifiable) {
    getAvailableDatasets @0 () -> (datasets :List(MetaPlusData));
    # get a list of all available datasets

    getDatasetsFor @1 (template :Metadata) -> (datasets :List(Dataset));
    # get a reference to the simulation with given id
  }

}
