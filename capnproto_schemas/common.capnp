@0x99f1c9a775a88ac9;

using Cxx = import "/capnp/c++.capnp";
$Cxx.namespace("mas::schema::common");

using Go = import "/capnp/go.capnp";
$Go.package("common");
$Go.import("github.com/zalf-rpm/mas-infrastructure/capnp_schemas/gen/go/common");

using Persistent = import "persistence.capnp".Persistent;
using Date = import "date.capnp".Date;

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


interface Callback {
  # interface to a callback object

  call @0 () -> ();
  # call the function associated with the callback object
}


interface Action extends(Persistent) {
  # interface to an arbitrary unparameterised action object

  do @0 () -> ();
  # execute the action represented by this object
  # any parameter can be null representing optional parameters
}


interface Action1 extends(Persistent) {
  # interface to an arbitrary unparameterised action object

  do @0 (p :AnyPointer) -> ();
  # execute the action represented by this object with one parameter
}


interface Factory(Input, Output) {
  # minimal interface to produce some output from input

  produce @0 (in :Input) -> (out :Output);
}


struct ZmqPipelineAddresses {
  input @0 :Text;
  output @1 :Text;
}

interface ValueHolder(T) {
  # an interface to a remote value 

  value @0 () -> (val :T);
  # get the referenced value
}

interface CapHolder(Object) {
  # hold a capability to an object
  # give the sender the chance to know when the CapHolder object isn't used any more

  cap @0 () -> (object :Object);
  # reference to some object, which can be any pointer type like a List(Capability), Capability or Struct

  release @1 () $Go.name("releaseCap"); 
  # release capability on server side (signaling that capability cap isn't needed anymore)
}


interface IdentifiableHolder extends(Identifiable) {
  # hold a capability to an object
  # give the sender the chance to know when the CapHolder object isn't used any more

  cap @0 () -> (cap :Identifiable);
  # reference to some object, which can be any pointer type like a List(Capability), Capability or Struct

  release @1 () $Go.name("releaseCap"); 
  # release capability on server side (signaling that capability cap isn't needed anymore)
}


#interface PersistCapHolder(Object) extends(CapHolder(Object), Persistent) {
  # persistent CapHolder which allows to get a token to recreate the CapHolder later
#}

struct ListEntry(PointerType) {
  entry @0 :PointerType;
}


#interface Cancelable {
#  cancel @0 () -> ();
#}


interface Stopable {
  # a capability to stop something
  stop @0 () -> ();
}

struct Pair(F, S) {
  fst @0 :F;
  snd @1 :S;
}

struct LL(H, T) {
  head @0 :H;
  tail @1 :T;
}

interface Clock(T) {
  # represents a syncronizing clock

  tick @0 (time :T);
  # forward clock one step to time T (which could also be just a Common.Date)
}

struct IP {
	# an FBP information packet

	struct KV {
		key 	@0 :Text;
		value 	@1 :AnyPointer;
	}
	attributes @0 :List(KV);
	# key value pair attributes attached to IP additional to main content

	content @1 :AnyPointer;
	# main content of IP
}

interface Channel(V) {
  # a potentially buffered channel to transport values of type V

  enum CloseSemantics {
    fbp   @0; # close channel automatically if there are no writers anymore and buffer is empty = no upstream data
    no    @1; # keep channel open until close message received
  }

  struct Msg {
    union {
      value @0 :V;
      done  @1 :Void;
    }
  }

  interface Reader $Cxx.name("ChanReader") {
    read  @0 () -> Msg;
    close @1 ();
  }

  interface Writer $Cxx.name("ChanWriter") {
    write @0 Msg;
    close @1 ();
  }

  setBufferSize @0 (size :UInt64 = 1);
  # set buffer size of channel, lowest allowed value = 1, meaning basically no buffer

  reader        @1 () -> (r :Reader);
  # get just a reader

  writer        @2 () -> (w :Writer);
  # get just a writer

  endpoints     @3 () -> (r :Reader, w :Writer);
  # get both endpoints of channel

  setAutoCloseSemantics @4 (cs :CloseSemantics);
  # set semantics when to automatically close this channel

  close         @5 (waitForEmptyBuffer :Bool = true);
  # close this channel
  # wait for empty buffer or kill channel right away
}

