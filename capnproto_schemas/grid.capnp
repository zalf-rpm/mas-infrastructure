@0xd373e9739460aa23;

using Cxx = import "/capnp/c++.capnp";
$Cxx.namespace("mas::rpc::grid");

#using Go = import "lang/go.capnp";
#$Go.package("dataServices");
#$Go.import("dataServices");

using Common = import "common.capnp";
using Geo = import "geo_coord.capnp";

enum Aggregation {
  # how to aggregate multiple values if the output resolution is lower than the grids base resolution
  # no prefix = no weights or interpolation of outer partial cell values
  # w prefix = weight outer partial cell values by cell area
  # i prefix = interpolate outer partial cell values

  none      @0;   # no aggregation/default
  avg       @8;   # average of cell values
  wAvg      @1;   # area weighted average 
  iAvg      @6;   # interpolated average
  median    @9;   # median of cell values
  wMedian   @2;   # area weighted median
  iMedian   @7;   # interpolated median
  min       @3;   # minimum
  wMin      @12;  # area weighted minimum
  iMin      @13;  # interpolated minimum
  max       @4;   # maximum
  wMax      @14;  # area weighted maximum
  iMax      @15;  # interpolated maximum
  sum       @5;   # sum of all cells values
  wSum      @10;  # area weighted sum
  iSum      @11;  # interpolated sum
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

  struct AggregationPart {
    value     @0 :Value;   # original cell value
    rowCol    @1 :RowCol;
    areaFrac  @2 :Float64; # area weight
    iValue    @3 :Float64; # interpolated value
  }

  closestValueAt @0 (latlonCoord :Geo.LatLonCoord, 
                     ignoreNoData :Bool = true, 
                     resolution :UInt64 = 0, 
                     agg :Aggregation = none, 
                     returnRowCols :Bool = false, 
                     includeAggParts :Bool = false) 
                     -> (val :Value, tl :RowCol, br :RowCol, aggParts :List(AggregationPart));
  # Get data at given lat/lon geo coordinate. If a resolution is given which is larger (larger number)
  # than the grids base resolution and aggregation as how to aggregate multiple values has to be given.
  # If no data values should not be ignored, then the closest no data cell will be returned.
  # If rows and cols should be returned then tl is the top left corner and br the bottom right corner the used data rectangle.
  # If includeAggParts is true, then a list of intermediate aggregation data values is returned, as well as if 
  # agg is set to none and no aggregation can happen.

  valueAt @4 (row :UInt64, col :UInt64, 
              resolution :UInt64, 
              agg :Aggregation = none,
              includeAggParts :Bool = false) 
              -> (val :Value, aggParts :List(AggregationPart));
  # Get data at a particular cell identified by row and column, potentially aggregated if resolution is lower.
  # If resolution is lower, but agg is none, then val will be null or 0 default and aggParts will be returned.
  # includeAggParts returns the intermediate aggregation in any case

  resolution @1 () -> (res :UInt64);
  # which resolution is this grid using

  dimension @2 () -> (rows :UInt64, cols :UInt64);
  # dimension of the grid

  noDataValue @3 () -> (nodata :Value);
  # the used no data value for the grid

  latLonBounds @5 (useCellCenter :Bool = false) -> (tl :Geo.LatLonCoord, tr :Geo.LatLonCoord, br :Geo.LatLonCoord, bl :Geo.LatLonCoord);
  # return the lat lon boundary of the grid

  interface Callback {
    sendCells @0 (cells :List(RowCol)) -> ();
  }

  streamCells @6 (callback :Callback, maxNoOfCellsPerSend :UInt64 = 100) -> ();
  # stream all cells to client in chunks of maxNoOfCellsPerSend
}
