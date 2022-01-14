@0xfe1be0c39c7e8269;

#using Persistent = import "/capnp/persistent.capnp";

using Cxx = import "/capnp/c++.capnp";
$Cxx.namespace("mas::rpc::registry");

using Go = import "/capnp/go.capnp";
$Go.package("registry");
$Go.import("github.com/zalf-rpm/mas-infrastructure/capnp_schemas/registry");

using Common = import "common.capnp";

interface Admin {
  # administrative interface to a registry

  addCategory @0 (category :Common.IdInformation, upsert :Bool = false) -> (success :Bool);
  # add a category to the registry
  # if upsert = true, update an existing category with the same id else ignore the add

  removeCategory @1 (categoryId :Text, moveObjectsToCategoryId :Text) -> (removedObjects :List(Common.Identifiable));
  # remove a category and move registered objects from category into other category
  # if moveObjectsToCategoryId is NULL, remove objects from registry -> removedObjects contains the list of removed refs
  # if moveObjectsToCategory is actually not existing treat it as if it would be null

  moveObjects @2 (objectIds :List(Text), toCatId :Text) -> (movedObjectIds :List(Text));
  # move objects to another category
  # if toCatId is null, don't move the objects
  # if toCatId doesn't exist, treat it as if it would be null

  removeObjects @3 (objectIds :List(Text)) -> (removedObjects :List(Common.Identifiable));
  # remove objects from registry and return the actually removed ones (in case some haven't been there)

  registry @4 () -> (registry :Registry);
  # return the registry administered by this interface
}


interface Registry extends(Common.Identifiable) {
  # public registry interface for capabilites implementing at least the identifiable interface
  # the registry might make its entries persistent by wrapping the Identifiables into a persistent wrapper

  supportedCategories @0 () -> (cats :List(Common.IdInformation));
  # what categories does this registry support

  categoryInfo @1 (categoryId :Text) -> Common.IdInformation;
  # get information to a particular category

  struct Entry {
    categoryId  @0 :Text;
    ref         @1 :Common.Identifiable;  
    name        @2 :Text;
  }

  entries @2 (categoryId :Text) -> (entries :List(Entry));
  # return the entries registered under the given category
  # given a NULL category id, maybe return all entries
  # forceRefInfos = true will mandatorily include refInfo = ref->info()
  # else it is optional, but might still be included
}


interface Registrar(SturdyRef) {
  # simple interface to register something
  # use case: a registry creates a sturdy ref of a Registrar capability and 
  # this sturdy ref is used to register a service at the registry

  register @0 (ref :Common.Identifiable, regName :Text, categoryId :Text) -> (unreg :Common.Callback, rereg :SturdyRef);
  # register the given identifiable reference with the given name
  # under the given categoryId which must match one of the supported categories
  # if categoryId or name are null, nothing is registered
  # return an unregister callback and a sturdy ref to reregister in case of
  # a necessary restart of the service with the same internal registry id
}
