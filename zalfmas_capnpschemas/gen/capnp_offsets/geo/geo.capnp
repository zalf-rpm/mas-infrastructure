# geo.capnp
@0x9090542079c7fc24;
$import "/capnp/c++.capnp".namespace("mas::schema::geo");
$import "/capnp/go.capnp".package("geo");
$import "/capnp/go.capnp".import("github.com/zalf-rpm/mas-infrastructure/capnproto_schemas/gen/go/geo");
enum CoordType @0xe529b4deb322ece8 {
  gk @0;
  utm @1;
  latlon @2;
}
struct EPSG @0xb79427a74eb97fc0 {  # 0 bytes, 0 ptrs
  const wgs84 @0xcdaf64c4789f2b7d :UInt32 = 4326;
  const utm21S @0xc8fb53981e470885 :UInt32 = 32721;
  const utm32N @0xcc67dee69497e2f3 :UInt32 = 25832;
  const gk5 @0x958c02356c8797e1 :UInt32 = 31469;
  const gk4 @0xe4afdddddec2511d :UInt32 = 31468;
  const gk3 @0xf5b9e8307038ad86 :UInt32 = 31467;
}
struct UTMCoord @0xeb1acd255e40f049 {  # 24 bytes, 1 ptrs
  zone @0 :UInt8;  # bits[0, 8)
  latitudeBand @1 :Text;  # ptr[0]
  r @2 :Float64;  # bits[64, 128)
  h @3 :Float64;  # bits[128, 192)
}
struct LatLonCoord @0xecf1fc3039cc8ffb {  # 16 bytes, 0 ptrs
  lat @0 :Float64;  # bits[0, 64)
  lon @1 :Float64;  # bits[64, 128)
}
struct GKCoord @0x97ff7d61786091ae {  # 24 bytes, 0 ptrs
  meridianNo @0 :UInt8;  # bits[0, 8)
  r @1 :Float64;  # bits[64, 128)
  h @2 :Float64;  # bits[128, 192)
}
struct Point2D @0xc88fb91c1e6986e2 {  # 16 bytes, 0 ptrs
  x @0 :Float64;  # bits[0, 64)
  y @1 :Float64;  # bits[64, 128)
}
struct RowCol @0xb0c6993e13e314ad {  # 16 bytes, 0 ptrs
  row @0 :UInt64;  # bits[0, 64)
  col @1 :UInt64;  # bits[64, 128)
}
struct Coord @0xb8f6a6192a7359f8 {  # 8 bytes, 1 ptrs
  union {  # tag bits [0, 16)
    gk @0 :GKCoord;  # ptr[0], union tag = 0
    latlon @1 :LatLonCoord;  # ptr[0], union tag = 1
    utm @2 :UTMCoord;  # ptr[0], union tag = 2
    p2D @3 :Point2D;  # ptr[0], union tag = 3
    rowcol @4 :RowCol;  # ptr[0], union tag = 4
  }
}
struct RectBounds @0xb952dbe83866da4a (CoordinateType) {  # 0 bytes, 2 ptrs
  tl @0 :CoordinateType;  # ptr[0]
  br @1 :CoordinateType;  # ptr[1]
}
