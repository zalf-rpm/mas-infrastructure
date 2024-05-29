@0x9755d0b34b9db39d;

using Cxx = import "/capnp/c++.capnp";
$Cxx.namespace("mas::schema::storage");

using Go = import "/capnp/go.capnp";
$Go.package("storage");
$Go.import("github.com/zalf-rpm/mas-infrastructure/capnproto_schemas/gen/go/storage");

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
          boolValue         @0  :Bool;
          boolListValue     @1  :List(Bool);
          int8Value         @2  :Int8;
          int8ListValue     @3  :List(Int8);
          int16Value        @4  :Int16;
          int16ListValue    @5  :List(Int16);
          int32Value        @6  :Int32;
          int32ListValue    @7  :List(Int32);
          int64Value        @8  :Int64;
          int64ListValue    @9  :List(Int64);
          uint8Value        @10 :UInt8;
          uint8ListValue    @11 :List(UInt8);
          uint16Value       @12 :UInt16;
          uint16ListValue   @13 :List(UInt16);
          uint32Value       @14 :UInt32;
          uint32ListValue   @15 :List(UInt32);
          uint64Value       @16 :UInt64;
          uint64ListValue   @17 :List(UInt64);
          float32Value      @18 :Float32;
          float32ListValue  @19 :List(Float32);
          float64Value      @20 :Float64;
          float64ListValue  @21 :List(Float64);
          textValue         @22 :Text;
          textListValue     @23 :List(Text);
          dataValue         @24 :Data;
          dataListValue     @25 :List(Data);
          anyValue          @26 :AnyStruct;
        }
      }

      getKey    @0 () -> (key :Text);
      # get key

      getValue  @1 () -> (value :Value, isUnset :Bool);
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

    getEntry          @3 (key :Text) -> (entry :Entry);
    # get an entry by key, potentially creating a new one (find out via entry.getValue().isUnset())

    addEntry          @6 (key :Text, value :Entry.Value, replaceExisting :Bool = false) -> (entry :Entry, success :Bool);
    # add a new entry, replace an existing entry if needed, return the entry and a success flag
    
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