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

    struct Object {
      key           @0 :Text;
      # the key of the object

      value :union {
        boolValue   @1 :Bool;
        intValue    @2 :Int64;
        floatValue  @3 :Float64;
        textValue   @4 :Text;
        dataValue   @5 :Data;
        anyValue    @6 :AnyStruct;
      }
      # the value of the object
    }

    importData    @0 (data :Data) -> (success :Bool);
    # import data into the container

    exportData    @1 () -> (data :Data);
    # export data from the container
    
    listObjects   @2 () -> (objects :List(Object));
    # list all objects in the container

    getObject     @3 (key :Text) -> (object :Object);
    # get an object by key

    addObject     @4 (object :Object) -> (success :Bool);
    # add an object to the container

    removeObject  @5 (key :Text) -> (success :Bool);
    # remove an object from the container

    clear         @6 () -> (success :Bool);
    # remove all objects from the container
  }

  newContainer    @0 (name :Text, description :Text) -> (container :Container);
  # create a new container object

  containerWithId @1 (id :Text) -> (container :Container);
  # get a container object with the given id

  listContainers  @2 () -> (containers :List(Container));
  # list all containers

  removeContainer @3 (id :Text) -> (success :Bool);
  # remove a container with the given id
}