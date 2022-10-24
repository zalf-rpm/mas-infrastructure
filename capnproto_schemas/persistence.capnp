@0x855efed3475f6b26;

#using Persistent = import "/capnp/persistent.capnp".Persistent;

using Cxx = import "/capnp/c++.capnp";
$Cxx.namespace("mas::schema::persistence");

using Go = import "/capnp/go.capnp";
$Go.package("persistence");
$Go.import("github.com/zalf-rpm/mas-infrastructure/capnp_schemas/gen/go/persistence");

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

  id      @0 :VatId;
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


interface Persistent {
  # simplified version of persistent.capnp::Persistent interface

  save @0 SaveParams -> SaveResults;
  # Save a capability persistently so that it can be restored by a future connection.  Not all
  # capabilities can be saved -- application interfaces should define which capabilities support
  # this and which do not.

  struct SaveParams {
    sealFor @0 :SturdyRef.Owner;
    # Seal the SturdyRef so that it can only be restored by the specified Owner. This is meant
    # to mitigate damage when a SturdyRef is leaked. See comments above.
    #
    # Leaving this value null may or may not be allowed; it is up to the realm to decide. If a
    # realm does allow a null owner, this should indicate that anyone is allowed to restore the
    # ref.
  }
  struct SaveResults {
    sturdyRef @0 :SturdyRef;

    unsaveSR  @1 :SturdyRef;
    # sturdy ref refering to an Common.Action capability to unsave the referenced capability
  }

  #save @0 () -> (sturdyRef :Text, srToken :Text, unsaveSR :Text, unsaveSRToken :Text);
  # create a sturdy ref to be able to restore this object and 
  # optionally return another SR refering to a Common.Action object representing the action to unsave this object
  # the actual sturdy ref tokens needed for the restorer service are optionally supplied as well
}


interface Restorer {
  # restore a capability from a sturdy ref

  struct RestoreParams {
    localRef @0 :AnyPointer;
    # local reference (sturdy ref token) to the capability to be restored

    sealedFor @1 :SturdyRef.Owner;
    # the owner of the sturdy ref to be restored
    # if everybody is allowed to restore the capability, this field should be null (unset)
  }

  restore @0 RestoreParams -> (cap :Capability);
  # restore from the localRef in a transient sturdy ref as live capabalility
}

