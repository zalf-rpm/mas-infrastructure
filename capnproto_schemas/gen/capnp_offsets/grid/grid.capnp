# grid.capnp
@0xd373e9739460aa23;
$import "/capnp/c++.capnp".namespace("mas::schema::grid");
$import "/capnp/go.capnp".package("grid");
$import "/capnp/go.capnp".import("github.com/zalf-rpm/mas-infrastructure/capnproto_schemas/gen/go/grid");
enum Aggregation @0xa5ecdc7767a6b301 {
  none @0;
  avg @8;
  wAvg @1;
  iAvg @6;
  median @9;
  wMedian @2;
  iMedian @7;
  min @3;
  wMin @12;
  iMin @13;
  max @4;
  wMax @14;
  iMax @15;
  sum @5;
  wSum @10;
  iSum @11;
}
interface Grid @0xe42973b29661e3c6 superclasses(import "/common.capnp".Identifiable, import "/persistence.capnp".Persistent) {
  closestValueAt @0 (latlonCoord :import "/geo.capnp".LatLonCoord, ignoreNoData :Bool = true, resolution :UInt64, agg :Aggregation, returnRowCols :Bool, includeAggParts :Bool) -> (val :Value, tl :RowCol, br :RowCol, aggParts :List(AggregationPart));
  valueAt @4 (row :UInt64, col :UInt64, resolution :UInt64, agg :Aggregation, includeAggParts :Bool) -> (val :Value, aggParts :List(AggregationPart));
  resolution @1 () -> (res :UInt64);
  dimension @2 () -> (rows :UInt64, cols :UInt64);
  noDataValue @3 () -> (nodata :Value);
  latLonBounds @5 (useCellCenter :Bool) -> (tl :import "/geo.capnp".LatLonCoord, tr :import "/geo.capnp".LatLonCoord, br :import "/geo.capnp".LatLonCoord, bl :import "/geo.capnp".LatLonCoord);
  streamCells @6 (callback :Callback, maxNoOfCellsPerSend :UInt64 = 100) -> ();
  struct Value @0xfe2e0dfae573d9d0 {  # 16 bytes, 0 ptrs
    union {  # tag bits [64, 80)
      f @0 :Float64;  # bits[0, 64), union tag = 0
      i @1 :Int64;  # bits[0, 64), union tag = 1
      ui @2 :UInt64;  # bits[0, 64), union tag = 2
      no @3 :Bool;  # bits[0, 1), union tag = 3
    }
  }
  struct RowCol @0xb9e2d85d086206ff {  # 16 bytes, 0 ptrs
    row @0 :UInt64;  # bits[0, 64)
    col @1 :UInt64;  # bits[64, 128)
  }
  struct AggregationPart @0xac444617ef333a1d {  # 16 bytes, 2 ptrs
    value @0 :Value;  # ptr[0]
    rowCol @1 :RowCol;  # ptr[1]
    areaFrac @2 :Float64;  # bits[0, 64)
    iValue @3 :Float64;  # bits[64, 128)
  }
  interface Callback @0xd639518280cb55d3 {
    sendCells @0 (cells :List(RowCol)) -> ();
  }
}
