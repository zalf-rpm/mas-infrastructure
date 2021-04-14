@0x855efed3475f6b26;

using Persistent = import "/capnp/persistent.capnp".Persistent;

using Cxx = import "/capnp/c++.capnp";
$Cxx.namespace("mas::rpc::persistence");

using Go = import "lang/go.capnp";
$Go.package("common");
$Go.import("common");

using Common = import "common.capnp";

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
  # socket connections on the "abstract" name "mas-infrastructure-<port>", so that other vats on the same machine
  # can connect via Unix sockets rather than IP.

  union {
    ip6 :group {
      lower64 @0 :UInt64;
      upper64 @1 :UInt64;
    }
    host @3 :Text;
  }
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
  # if owner is set (not NULL) restorer should expect srToken to be encrypted by the owners private key
}


interface ExternalPersistent(Token) {
  # external Persistent interface to create SturdyRefs for external capabilities

  #save @0 (cap :Capability, params :Persistent(Token, SturdyRef.Owner).SaveParams) -> ExternalSaveResults;
  # create a sturdy ref for the given capability and parameters
  # return the original SaveResults object and an unregister callback

  struct ExternalSaveResults {
    results @0 :Persistent(Token, SturdyRef.Owner).SaveResults;
    unreg @1 :Common.Callback;
  }

}



#interface A(T) {
#  m @0 () -> S;

#  interface CB1 {
#    call @0 ();
#  }

#  struct S {
#    res @0 :T;
#    callback @1 :CB1;
#  }
#}

#interface CB2 {
#  call @0 ();
#}

#interface B(T) {
#  m @0 () -> S;
 
#  struct S {
#    res @0 :T;
#    callback @1 :CB2;
#  }
#}