@0xe8ea2bc38b07f62a;

#using Go = import "go.capnp";
#$Go.package("date");
#$Go.import("date");

struct Date {
  # A standard Gregorian calendar date.

  year @0 :Int16;
  # The year. Must include the century.
  # Negative value indicates BC.

  month @1 :UInt8;   # Month number, 1-12.
  day @2 :UInt8;     # Day number, 1-31.
}