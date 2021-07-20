@0xd373e9739460aa23;

using Cxx = import "/capnp/c++.capnp";
$Cxx.namespace("mas::rpc::grid");

#using Go = import "lang/go.capnp";
#$Go.package("dataServices");
#$Go.import("dataServices");

using Common = import "common.capnp";
using Persistent = import "/capnp/persistent.capnp".Persistent;
using Restorer = import "persistence.capnp".Restorer;
using Geo = import "geo_coord.capnp";

enum Aggregation {
  # how to aggregate multiple values if the output resolution is lower than the grids base resolution

  none    @0; # no aggregation/default
  avg     @1; # average
  median  @2; # median
  min     @3; # minimum
  max     @4; # maximum
  sum     @5; # sum
}


interface Grid extends(Common.Identifiable) {
  # interface to a grid of data

  struct Value {
    union {
      f  @0 :Float64;
      i  @1 :Int64;
      ui @2 :UInt64;
      no @3 :Bool;
    }
  }

  struct RowCol {
    row @0 :UInt64;
    col @1 :UInt64;
  }

  closestValueAt @0 (latlonCoord :Geo.LatLonCoord, ignoreNoData :Bool = true, resolution :UInt64 = 0, agg :Aggregation = none, returnRowCols :Bool = false) -> (val :Value, tl :RowCol, br :RowCol);
  # Get data at given lat/lon geo coordinate. If a resolution is given which is larger (larger number)
  # than the grids base resolution and aggregation as how to aggregate multiple values has to be given.
  # If no data values should not be ignored, then the closest no data cell will be returned.
  # If rows and cols should be returned then tl is the top left corner and br the bottom right corner the used data rectangle

  valueAt @4 (row :UInt64, col :UInt64, resolution :UInt64, agg :Aggregation = none) -> (val :Value);
  # get data at a particular cell identified by row and column, potentially aggregated if resolution is lower

  resolution @1 () -> (res :UInt64);
  # which resolution is this grid using

  dimension @2 () -> (rows :UInt64, cols :UInt64);
  # dimension of the grid

  noDataValue @3 () -> (nodata :Value);
  # the used no data value for the grid

  latLonBounds @5 (useCellCenter :Bool = false) -> (tl :Geo.LatLonCoord, tr :Geo.LatLonCoord, br :Geo.LatLonCoord, bl :Geo.LatLonCoord);
  # return the lat lon boundary of the grid

}

