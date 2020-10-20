@0xfe1be0c39c7e8269;

using Persistent = import "/capnp/persistent.capnp";

using Cxx = import "/capnp/c++.capnp";
$Cxx.namespace("mas::rpc::registry");

using Go = import "lang/go.capnp";
$Go.package("common");
$Go.import("common");

using Common = import "common.capnp".Common;


interface Admin {
  # administrative interface to a registry

  addCategory @0 Common.IdInformation -> (success :Bool);
  # add a category to the registry

  removeCategory @1 (categoryId :Text, moveEntriesToCategoryId :Text) -> (entriesMoved :List(Common.Identifiable));
  # remove a category and move registered entries from category into other category
  # if moveEntriesToCategoryId is NULL, remove entries from registry -> entriesMoved contains the removed refs

  moveObjects @2 (entries :List(Common.Identifiable), fromCatId :Text, toCatId :Text) -> (movedEntries :List(Common.Identifiable));
  # move objects from one category into another category

  removeEntries @3 (entries :List(Common.Identifiable)) -> (removedEntries :List(Common.Identifiable));
  # remove entries from registry and return the actually removed ones (in case some haven't been there)

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
    categoryId @0 :Text;
    ref @1 :Common.Identifiable;  
  }

  entries @2 (categoryId :Text) -> (entries :List(Entry));
  # return the entries registered under the given category
  # given a NULL category id, maybe return all entries
}


interface Registrator {
  # simple interface to register something
  # use case: a registry creates a sturdy ref of a register callback and 
  # this sturdy ref is used to register a service at the registry

  register @0 (ref :Common.Identifiable, categoryId :Text) -> (unreg :Common.Callback);
  # register the given identifiable reference under the optional categoryId 
  # return an unregister callback
}
