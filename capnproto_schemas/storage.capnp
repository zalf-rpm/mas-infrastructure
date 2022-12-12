@0x9755d0b34b9db39d;

using Cxx = import "/capnp/c++.capnp";
$Cxx.namespace("mas::schema::storage");

using Go = import "/capnp/go.capnp";
$Go.package("storage");
$Go.import("github.com/zalf-rpm/mas-infrastructure/capnp_schemas/gen/go/storage");

using Common = import "common.capnp";
using Persistent = import "persistence.capnp".Persistent;

interface Store extends(Common.Identifiable, Persistent) {
  # simple storage service interface
  
  interface Container extends(Common.Identifiable, Persistent) {
    # a container for storing objects

    interface Entry {
    # interface to access an object in store

      struct Value {
        union {
          boolValue       @0  :Bool;
          boolListValue   @1  :List(Bool);
          intValue        @2  :Int64;
          intListValue    @3  :List(Int64);
          floatValue      @4  :Float64;
          floatListValue  @5  :List(Float64);
          textValue       @6  :Text;
          textListValue   @7  :List(Text);
          dataValue       @8  :Data;
          dataListValue   @9  :List(Data);
          anyValue        @10 :AnyStruct;
        }
      }

      getKey    @0 () -> (key :Text);
      # get key

      getValue  @1 () -> (value :Value);
      # get value

      setValue  @2 (value :Value) -> (success :Bool);
      # update or set value
    }

    export            @0 () -> (json :Text);
    # export the container (serialized as capnp JSON)
    
    downloadEntries   @1 () -> (entries :List(Common.Pair(Text, Entry.Value)));
    # download all entries in the container as list of key-value pairs

    listEntries       @2 () -> (entries :List(Entry));
    # download all entries in the container

    getEntry          @3 (key :Text) -> (entry :Entry, isNew :Bool);
    # get an entry by key, potentially creating a new one

    removeEntry       @4 (key :Text) -> (success :Bool);
    # remove an entry from the container

    clear             @5 () -> (success :Bool);
    # remove all entries from the container
  }

  newContainer    @0 (name :Text, description :Text) -> (container :Container);
  # create a new container object

  containerWithId @1 (id :Text) -> (container :Container);
  # get a container object with the given id

  listContainers  @2 () -> (containers :List(Container));
  # list all containers

  removeContainer @3 (id :Text) -> (success :Bool);
  # remove a container with the given id

  importContainer @4 (json :Text) -> (container :Container);
  # import container (serialized as capnp JSON)

  struct ImportExportData {
    # data to be imported or exported

    info        @0 :Common.IdInformation;
    # the id information of the container

    entries     @1 :List(Common.Pair(Text, Container.Entry.Value));
    # the objects in the container

    isAnyValue  @2 :List(Bool);
    # whether the entry's value was an anyValue which got serialized as base64 textValue
  }
}