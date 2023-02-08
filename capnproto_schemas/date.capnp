@0xe8ea2bc38b07f62a;

using Cxx = import "/capnp/c++.capnp";
$Cxx.namespace("mas::schema::common");

using Go = import "/capnp/go.capnp";
$Go.package("common");
$Go.import("github.com/zalf-rpm/mas-infrastructure/capnproto_schemas/gen/go/common");

struct Date {
  # A standard Gregorian calendar date.

  year @0 :Int16;
  # The year. Must include the century.
  # Negative value indicates BC.

  month @1 :UInt8;   # Month number, 1-12.
  day @2 :UInt8;     # Day number, 1-31.
}