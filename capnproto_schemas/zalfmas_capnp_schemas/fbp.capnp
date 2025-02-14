@0xbf602c4868dbb22f;

using Cxx = import "/capnp/c++.capnp";
$Cxx.namespace("mas::schema::fbp");

using Go = import "/capnp/go.capnp";
$Go.package("fbp");
$Go.import("github.com/zalf-rpm/mas-infrastructure/capnproto_schemas/gen/go/fbp");

using Persistent = import "persistence.capnp".Persistent;
using Identifiable = import "common.capnp".Identifiable;

struct IP {
  # an FBP information packet

  struct KV {
    key 	@0 :Text;
    desc 	@1 :Text; # optional human readable info on what value is
    value @2 :AnyPointer;  # would often be a common.Value
  }
  attributes @0 :List(KV);
  # key value pair attributes attached to IP additional to main content

  content @1 :AnyPointer;
  # main content of IP
}

interface Channel(V) extends(Identifiable, Persistent) {
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

  struct StartupInfo {
    # information about the startup of a channel

    bufferSize @0 :UInt64;
    # size of the buffer

    closeSemantics @1 :CloseSemantics;
    # semantics of closing the channel

    channelSR @2 :Text;
    # sturdy reference to the channel

    readerSRs @3 :List(Text);
    # sturdy references to the readers

    writerSRs @4 :List(Text);
    # sturdy references to the writers
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

interface PortCallbackRegistrar {
  # interface to register callbacks for ports

  interface PortCallback {
    newInPort @0 (name :Text, readerCap :Channel(IP).Reader);
    newOutPort @1 (name :Text, writerCap :Channel(IP).Writer);
  }

  registerCallback @0 (callback :PortCallback);
  # register a callback for the ports of a process
}