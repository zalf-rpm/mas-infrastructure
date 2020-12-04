@0x9090542079c7fc24;

using Cxx = import "/capnp/c++.capnp";
$Cxx.namespace("mas::rpc");

using Go = import "lang/go.capnp";
$Go.package("geo");
$Go.import("geo_coord.capnp");

struct Geo {
  # namespace for the geo coord datastructures
  
  enum CoordType {
    gk @0; # Gauss-Krüger 
    utm @1; # UTM
    latlon @2; # 
  }

  struct EPSG {
    # an object with an EPSG code
    const wgs84 :UInt32 = 4326;
    const utm21S :UInt32 = 32721;
    const utm32N :UInt32 = 25832;
    const gk5 :UInt32 = 31469;
    const gk3 :UInt32 = 31467; #3396;
  }

  struct UTMCoord {
    # an UTM coord

    zone @0 :UInt8; # zones from 1 to 60
    latitudeBand @1 :Text;

    r @2 :Int64; # right value
    h @3 :Int64; # height value
  }

  struct LatLonCoord {
    # a WGS84 coord EPSG4326

    lat @0 :Float64; # latitude
    lon @1 :Float64; # longitude
  }

  struct GKCoord {
    # a Gauss Krueger coordinate

    meridianNo @0 :UInt8;
    r @1 :Int64; # right value
    h @2 :Int64; # height value
  }

  struct Point2D {
    # a rectangular coordinate
    x @0 :Int64;
    y @1 :Int64;
  }

  struct Coord {
    # an abstract geo coordinate

    union {
      gk @0 :GKCoord;
      latlon @1 :LatLonCoord;
      utm @2 :UTMCoord;
      p2D @3 :Point2D;
    }
  }

  struct RectBounds(CoordinateType) {
    tl @0 :CoordinateType; # top left corner
    br @1 :CoordinateType; # bottom right corner
  }

}

