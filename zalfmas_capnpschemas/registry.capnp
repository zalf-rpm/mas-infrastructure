@0xfe1be0c39c7e8269;

#using Persistent = import "/capnp/persistent.capnp";

using Cxx = import "/capnp/c++.capnp";
$Cxx.namespace("mas::schema::registry");

using Go = import "/capnp/go.capnp";
$Go.package("registry");
$Go.import("github.com/zalf-rpm/mas-infrastructure/capnproto_schemas/gen/go/registry");

using Identifiable = import "common.capnp".Identifiable;
using IdInformation = import "common.capnp".IdInformation;
using Persistence = import "persistence.capnp";

interface Admin extends(Identifiable) {
  # administrative interface to a registry

  addCategory @0 (category :IdInformation, upsert :Bool = false) -> (success :Bool);
  # add a category to the registry
  # if upsert = true, update an existing category with the same id else ignore the add

  removeCategory @1 (categoryId :Text, moveObjectsToCategoryId :Text) -> (removedObjects :List(Identifiable));
  # remove a category and move registered objects from category into other category
  # if moveObjectsToCategoryId is NULL, remove objects from registry -> removedObjects contains the list of removed refs
  # if moveObjectsToCategory is actually not existing treat it as if it would be null

  moveObjects @2 (objectIds :List(Text), toCatId :Text) -> (movedObjectIds :List(Text));
  # move objects to another category
  # if toCatId is null, don't move the objects
  # if toCatId doesn't exist, treat it as if it would be null

  removeObjects @3 (objectIds :List(Text)) -> (removedObjects :List(Identifiable));
  # remove objects from registry and return the actually removed ones (in case some haven't been there)

  registry @4 () -> (registry :Registry);
  # return the registry administered by this interface
}


interface Registry extends(Identifiable) {
  # public registry interface for capabilites implementing at least the identifiable interface
  # the registry might make its entries persistent by wrapping the Identifiables into a persistent wrapper

  supportedCategories @0 () -> (cats :List(IdInformation));
  # what categories does this registry support

  categoryInfo @1 (categoryId :Text) -> IdInformation;
  # get information to a particular category

  struct Entry {
    categoryId  @0 :Text;
    ref         @1 :Identifiable;  
    name        @2 :Text;
  }

  entries @2 (categoryId :Text) -> (entries :List(Entry));
  # return the entries registered under the given category
  # given a NULL category id, maybe return all entries
}


interface Registrar extends(Identifiable) {
  # simple interface to register something
  # use case: a registry creates a sturdy ref of a Registrar capability and 
  # this sturdy ref is used to register a service at the registry

  struct CrossDomainRestore {
    # data needed to allow restoring registered caps across domains

    vatId     @0 :Persistence.VatId;
    # identify the caps Vat

    restorer  @1 :Persistence.Restorer;
    # capability to the restorer
  }

  struct RegParams {
    cap         @0 :Identifiable;
    regName     @1 :Text;
    categoryId  @2 :Text;
    xDomain     @3 :CrossDomainRestore;
  }

  interface Unregister {
    unregister @0 () -> (success :Bool);
  }

  register @0 RegParams -> (unreg :Unregister, reregSR :Persistence.SturdyRef);
  # register the given identifiable capability with the given name
  # under the given categoryId which must match one of the supported categories
  # if categoryId or name are null, nothing is registered
  # return an unregister action and a sturdy ref to reregister Funcs.Action taken the new capability in case of
  # a necessary restart of the service with the same internal registry id, regName and categoryId
}
