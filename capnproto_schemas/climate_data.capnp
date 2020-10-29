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
    cccmaCanEsm2 @0;# $Cxx.name("CCCma-CanESM2");
    ichecEcEarth @1;# $Cxx.name("ICHEC-EC-EARTH");
    ipslIpslCm5AMr @2;# $Cxx.name("IPSL-IPSL-CM5A-MR");
    mirocMiroc5 @3;# $Cxx.name("MIROC-MIROC5");
    mpiMMpiEsmLr @4;# $Cxx.name("MPI-M-MPI-ESM-LR");
    gfdlEsm4 @5;# $Cxx.name("GFDL-ESM4");
    ipslCm6aLr @6;# $Cxx.name("IPSL-CM6A-LR");
    mpiEsm12Hr @7;# $Cxx.name("MPI-ESM1-2-HR");
    mriEsm20 @8;# $Cxx.name("MRI-ESM2-0");
    ukesm10Ll @9;# $Cxx.name("UKESM1-0-LL");
    gswp3W5E5 @10; # Global Soil Wetness Project Phase 3
  }

  enum RCM {
    # regional circulation models
    
    clmcomCclm4817 @0;# $Cxx.name("CLMcom-CCLM4-8-17");
    gericsRemo2015 @1;# $Cxx.name("GERICS-REMO2015");
    knmiRacmo22E @2;# $Cxx.name("KNMI-RACMO22E");
    smhiRca4 @3;# $Cxx.name("SMHI-RCA4");
    clmcomBtuCclm4817 @4;# $Cxx.name("CLMcom-BTU-CCLM4-8-17");
    mpiCscRemo2009 @5;# $Cxx.name("MPI-CSC-REMO2009");
  }

  enum SSP {
    # Shared Socioeconomic Pathways

    ssp1 @0;# $Cxx.name("SSP1"); # SSP1: Sustainability (Taking the Green Road)
    ssp2 @1;# $Cxx.name("SSP2"); # SSP2: Middle of the Road
    ssp3 @2;# $Cxx.name("SSP3"); # SSP3: Regional Rivalry (A Rocky Road)
    ssp4 @3;# $Cxx.name("SSP4"); # SSP4: Inequality (A Road divided)  
    ssp5 @4;# $Cxx.name("SSP5"); # SSP5: Fossil-fueled Development (Taking the Highway)
  }

  enum RCP { 
    # Representative Concentration Pathway

    rcp19 @0;# $Cxx.name("RCP 1.9"); # RCP 1.9 is a pathway that limits global warming to below 1.5 °C, the aspirational goal of the Paris Agreement.
    rcp26 @1;# $Cxx.name("RCP 2.6"); # RCP 2.6 is a "very stringent" pathway.
    rcp34 @2;# $Cxx.name("RCP 3.4"); # As well as just providing another option a variant of RCP 3.4 includes considerable removal of greenhouse gases from the atmosphere.
    rcp45 @3;# $Cxx.name("RCP 4.5"); # Emissions in RCP 4.5 peak around 2040, then decline.
    rcp60 @4;# $Cxx.name("RCP 6"); # In RCP 6, emissions peak around 2080, then decline.
    rcp70 @5;# $Cxx.name("RCP 7"); # RCP 7 is a baseline outcome rather than a mitigation target.
    rcp85 @6;# $Cxx.name("RCP 8.5"); # In RCP 8.5 emissions continue to rise throughout the 21st century.
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
