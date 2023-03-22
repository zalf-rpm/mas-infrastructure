@0x99f1c9a775a88ac9;

using Cxx = import "/capnp/c++.capnp";
$Cxx.namespace("mas::schema::common");

using Go = import "/capnp/go.capnp";
$Go.package("common");
$Go.import("github.com/zalf-rpm/mas-infrastructure/capnproto_schemas/gen/go/common");

struct IdInformation {
  id @0 :Text; # could be a UUID4
  name @1 :Text;
  description @2 :Text;
}

interface Identifiable {
  # interface to retrieve id information from an object
  info @0 () -> IdInformation;
}

struct StructuredText {
  # some structured text, always encoded in UTF-8

  value @0 :Text;
  # text stream

  structure :union {
    # structural type
    none @1 :Void; # just normal text
    json @2 :Void; # it's JSON
    xml @3 :Void; # it's XML
  }
}

#interface Factory(Input, Output) {
  # minimal interface to produce some output from input

#  produce @0 (in :Input) -> (out :Output);
#}

struct Pair(F, S) {
  fst @0 :F;
  snd @1 :S;
}

#struct LL(H, T) {
#  head @0 :H;
#  tail @1 :T;
#}

#interface Clock(T) {
  # represents a syncronizing clock

#  tick @0 (time :T);
  # forward clock one step to time T (which could also be just a Common.Date)
#}

