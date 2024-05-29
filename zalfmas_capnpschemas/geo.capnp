@0x9090542079c7fc24;

using Cxx = import "/capnp/c++.capnp";
$Cxx.namespace("mas::schema::geo");

using Go = import "/capnp/go.capnp";
$Go.package("geo");
$Go.import("github.com/zalf-rpm/mas-infrastructure/capnproto_schemas/gen/go/geo");

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
  const gk4 :UInt32 = 31468;
  const gk3 :UInt32 = 31467; #3396;
}

struct UTMCoord {
  # an UTM coord

  zone @0 :UInt8; # zones from 1 to 60
  latitudeBand @1 :Text;

  r @2 :Float64; # right value
  h @3 :Float64; # height value
}

struct LatLonCoord {
  # a WGS84 coord EPSG4326

  lat @0 :Float64; # latitude
  lon @1 :Float64; # longitude
}

struct GKCoord {
  # a Gauss Krueger coordinate

  meridianNo @0 :UInt8;
  r @1 :Float64; # right value
  h @2 :Float64; # height value
}

struct Point2D {
  # a rectangular coordinate
  x @0 :Float64;
  y @1 :Float64;
}

struct RowCol {
  # a row and column coordinate
  row @0 :UInt64;
  col @1 :UInt64;
}

struct Coord {
  # an abstract geo coordinate

  union {
    gk      @0 :GKCoord;
    latlon  @1 :LatLonCoord;
    utm     @2 :UTMCoord;
    p2D     @3 :Point2D;
    rowcol  @4 :RowCol;
  }
}

struct RectBounds(CoordinateType) {
  tl @0 :CoordinateType; # top left corner
  br @1 :CoordinateType; # bottom right corner
}


