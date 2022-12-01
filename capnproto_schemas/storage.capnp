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
        boolValue       @1  :Bool;
        boolListValue   @7  :List(Bool);
        intValue        @2  :Int64;
        intListValue    @8  :List(Int64);
        floatValue      @3  :Float64;
        floatListValue  @9  :List(Float64);
        textValue       @4  :Text;
        textListValue   @10 :List(Text);
        dataValue       @5  :Data;
        dataListValue   @11 :List(Data);
        anyValue        @6  :AnyStruct;
      }
      # the value of the object
    }
    
    export        @0 () -> (json :Text);
    # export the container (serialized as capnp JSON)
    
    listObjects   @1 () -> (objects :List(Object));
    # list all objects in the container

    getObject     @2 (key :Text) -> (object :Object);
    # get an object by key

    addObject     @3 (object :Object) -> (success :Bool);
    # add an object to the container

    removeObject  @4 (key :Text) -> (success :Bool);
    # remove an object from the container

    clear         @5 () -> (success :Bool);
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

  importContainer @4 (json :Text) -> (container :Container);
  # import container (serialized as capnp JSON)

  struct ImportExportData {
    # data to be imported or exported

    info        @0 :Common.IdInformation;
    # the id information of the container

    objects     @1 :List(Container.Object);
    # the objects in the container

    isAnyValue  @2 :List(Bool);
    # whether the object value was an anyValue which got serialized as base64 textValue
  }
}