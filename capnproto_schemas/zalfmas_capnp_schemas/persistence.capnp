@0x855efed3475f6b26;

#using Persistent = import "/capnp/persistent.capnp".Persistent;

using Cxx = import "/capnp/c++.capnp";
$Cxx.namespace("mas::schema::persistence");

using Go = import "/capnp/go.capnp";
$Go.package("persistence");
$Go.import("github.com/zalf-rpm/mas-infrastructure/capnproto_schemas/gen/go/persistence");

using Identifiable = import "common.capnp".Identifiable;

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

  vat @0 :VatPath;
  # The vat where the object is located.

  localRef @1 :Token;
  # A SturdyRef in the format defined by the vat.

  struct Token {
    union {
      text @0 :Text; # token as text, e.g. UUID4
      data @1 :Data; # token as data, e.g. owner signed text (e.g. UUID4)
    }
  }
}

interface Heartbeat {
  beat @0 () -> ();
}

interface Persistent {
  # adjusted version of persistent.capnp::Persistent interface

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
    sturdyRef               @0 :SturdyRef;
    # the actual sturdy reference

    unsaveSR                @1 :SturdyRef;
    # sturdy ref referring to an ReleaseSturdyRef capability to unsave the referenced capability

    #heartbeat               @2 :Heartbeat;
    # heartbeat capability to keep the sturdy ref registration alive
    # if the heartbeat is null, you have to assume the saved capability can be lost at any time

    #secsHeartbeatInterval   @3 :UInt32;
    # interval in seconds to call the heartbeat capability
  }

  interface ReleaseSturdyRef {
    release @0 () -> (success :Bool) $Go.name("releaseSR"); 
  }
}

interface Restorer {
  # restore a capability from a sturdy ref

  struct RestoreParams {
    localRef @0 :SturdyRef.Token;
    # local reference (sturdy ref token) to the capability to be restored

    sealedBy @1 :SturdyRef.Owner;
    # the owner of the sturdy ref to be restored
    # if everybody is allowed to restore the capability, this field should be null (unset)
    # if sealedBy is set, the localRef must be signed by the private key of the owner matching
    # the public key registered with the restorer service else the capability cannot be restored
  }

  restore @0 RestoreParams -> (cap :Capability);
  # restore from the localRef in a transient sturdy ref as live capability
}

interface HostPortResolver extends(Identifiable, Restorer) {
  # resolve an id (either base64 encoded VatId or a text alias) to a host and port
  # acts also as restorer for the registrar capabilities

  interface Registrar { 
    # register a services base64 encoded VatId to a host and port (and optionally a plain text alias)

    struct RegisterParams {
      base64VatId @0 :Text;
      # base64 encoded VatId of the service to register

      host @1 :Text;
      # host name or ip address of the service to register

      port @2 :UInt16;
      # port of the service to register

      alias @3 :Text;
      # optional plain text alias for the service to register
    }

    register @0 RegisterParams -> (heartbeat :Heartbeat, secsHeartbeatInterval :UInt32);
    # Register a vat-id to a host and port and optionally an alias.
    # Returns a Heartbeat capability to call regularly to keep the registration alive.
    # If heartbeat is null, then the registration was not successful.
    # Call heartbeat at least every secsHeartbeatInterval seconds.
    # If a heartbeat ist missed, the vat-id/alias will be unregistered.
  }

  resolve @0 (id :Text) -> (host :Text, port :UInt16) $Go.name("ResolveIdent") ;
  # resolve an id (either base64 encoded VatId or plain text alias) to a host and port
}

interface Gateway extends(Identifiable, Restorer) {
  # interface to "register" caps at a gateway so they can be reached from
  # the gateways outer side and be saved to the gateways restorer

  register @0 (cap :Capability) -> RegResults;
  # Register a capability at the gateway and return sturdy refs pointing to the gateway
  # which can be used to access the capability later

  struct RegResults {
    sturdyRef               @0 :SturdyRef;
    # the actual sturdy reference to the registered capability

    heartbeat               @1 :Heartbeat;
    # heartbeat capability to keep the sturdy ref registration alive
    # if the heartbeat is null, you have to assume the saved capability can be lost at any time

    secsHeartbeatInterval   @2 :UInt32;
    # interval in seconds to call the heartbeat capability
  }
}