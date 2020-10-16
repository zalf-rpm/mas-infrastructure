@0x99f1c9a775a88ac9;

using Persistent = import "/capnp/persistent.capnp";

using Cxx = import "/capnp/c++.capnp";
$Cxx.namespace("mas::rpc");

using Go = import "lang/go.capnp";
$Go.package("common");
$Go.import("common");

struct Common {

  struct IdInformation {
    id @0 :Text; # could be a UUID4
    name @1 :Text;
    description @2 :Text;
  }

  interface Identifiable {
    # interface to retrieve id information from an object
    info @0 () -> IdInformation;
  }
  
  struct Date {
    # A standard Gregorian calendar date.

    year @0 :Int16;
    # The year. Must include the century.
    # Negative value indicates BC.

    month @1 :UInt8;   # Month number, 1-12.
    day @2 :UInt8;     # Day number, 1-31.
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

  struct VatId {
    # Taken from https://github.com/sandstorm-io/blackrock/blob/master/src/blackrock/cluster-rpc.capnp#L22
    # Identifies a machine.

    publicKey0 @0 :UInt64;
    publicKey1 @1 :UInt64;
    publicKey2 @2 :UInt64;
    publicKey3 @3 :UInt64;
    # The Vat's Curve25519 public key, interpreted as little-endian.
  }

  struct Address {
    # Taken from https://github.com/sandstorm-io/blackrock/blob/master/src/blackrock/cluster-rpc.capnp#L38
    # Address at which you might connect to a vat. Used for three-party hand-offs.
    #
    # Note that any vat that listens for connections on a port should also listen for unix domain
    # socket connections on the "abstract" name "sandstorm-<port>", so that other vats on the same machine
    # can connect via Unix sockets rather than IP.

    lower64 @0 :UInt64;
    upper64 @1 :UInt64;
    # Bits of the IPv6 address. Since IP is a big-endian spec, the "lower" bits are on the right, and
    # the "upper" bits on the left. E.g., if the address is "1:2:3:4:5:6:7:8", then the lower 64 bits
    # are "5:6:7:8" or 0x0005000600070008 while the upper 64 bits are "1:2:3:4" or 0x0001000200030004.
    #
    # Note that for an IPv4 address, according to the standard IPv4-mapped IPv6 address rules, you
    # would use code like this:
    #     uint32 ipv4 = (octet[0] << 24) | (octet[1] << 16) | (octet[2] << 8) | octet[3];
    #     dest.setLower64(0x0000FFFF00000000 | ipv4);
    #     dest.setUpper64(0);

    port @2 :UInt16;
  }

  struct VatPath {
    # Taken from https://github.com/sandstorm-io/blackrock/blob/master/src/blackrock/cluster-rpc.capnp#L60
    # Enough information to connect to a vat securely.

    id @0 :VatId;
    address @1 :Address;
  }

  struct SturdyRef {
    # Adapted from https://github.com/sandstorm-io/blackrock/blob/master/src/blackrock/cluster-rpc.capnp#L67
    # Parameterization of SturdyRef 

    struct Owner {
      # Owner of a SturdyRef, for sealing purposes. See discussion of sealing in
      # import "/capnp/persistent.capnp".Persistent.

      guid @0 :Text;
      # globally unique id, could also be an email address or domain name
    }

    union {
      transient @0 :Transient;
      stored @1 :Stored;
    }

    struct Transient {
      # Reference to an object hosted by some specific vat in the cluster, which will eventually
      # become invalid when that vat is taken out of rotation.

      vat @0 :VatPath;
      # The vat where the object is located.

      localRef @1 :AnyPointer;
      # A SturdyRef in the format defined by the vat.
    }

    struct Stored {
      # Reference to an object in long-term storage.

      key0 @0 :UInt64;
      key1 @1 :UInt64;
      key2 @2 :UInt64;
      key3 @3 :UInt64;
      # 256-bit object key. This both identifies the object and may serve as a symmetric key for
      # decrypting the object.
    }
  }


  interface Restorer(Token) {
    # restore a capability from a sturdy ref

    restore @0 (srToken :Token, owner :SturdyRef.Owner) -> (cap :Capability);
    # restore from the given sturdy ref token a live capability
    # if owner is set (not NULL) restorer should expect srToken to be encrypted by the owners private key

    drop @1 (srToken :Token, owner :SturdyRef.Owner);
    # drop the given token from the service
  }


  struct Registry {
    # an interface for registering 
    # there are two use cases for using the interface
    # a) ModelFactory
    #    e.g. a factory starts a model instance which has to register at the 
    #    factory in order for the factory to get the reference to the model
    #    instance -> in this case the factory can create a token which 
    #    the model instance uses to register itself at the factories registration
    #    service
    # b) DataServices
    #    - an administrator wants to register a new service at an registry
    #    - the registry registers the service and returns a newly created UUID 
    #      for the service
    #    an arbitrary user can register a capability at the registration service
    #    then the registry will 

    interface Unregister {
      # interface to unregister objects

      unregister @0 ();
      # unregister a previously registered object
    }

    interface Private(Object, CatType) {
      register @0 (obj :Object, regToken :Text, category :Category) -> (unreg :Unregister);
      # - register the given object using the registrationToken and return a capability to unregister again
      # - deleting this capability or calling unregister on it will do the same 
      # - the registry might only allow registration for tokens it already knows
      #   this depends on the implementation of the registry 
      #   use case a) would not allow registration requests for unknown tokens
      #   (not created by the registry itself)
    }

    interface Public(Object, CatType) {
      register @0 (obj :Object, category :CatType) -> (regToken :Text, unreg :Unregister);
      # register the given object and return the created token and unregister capability

      get @1 (regToken :Text) -> (obj :Object);
      # get an previously registered object for the given token
    }
  }


  struct ZmqPipelineAddresses {
    input @0 :Text;
    output @1 :Text;
  }


  interface CapHolder(Object) {
    # hold a capability to an object
    # give the sender the chance to know when the CapHolder object isn't used any more

    cap @0 () -> (object :Object);
    # reference to some object, which can be any pointer type like a List(Capability), Capability or Struct

    release @1 ();
    # release capability on server side (signaling that capability cap isn't needed anymore)
  }

  interface PersistCapHolder(Object) extends(CapHolder(Object), Persistent.Persistent(Text, Text)) {
    # persistent CapHolder which allows to get a token to recreate the CapHolder later
  }

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

}
