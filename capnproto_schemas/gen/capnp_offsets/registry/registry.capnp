# registry.capnp
@0xfe1be0c39c7e8269;
$import "/capnp/c++.capnp".namespace("mas::schema::registry");
$import "/capnp/go.capnp".package("registry");
$import "/capnp/go.capnp".import("github.com/zalf-rpm/mas-infrastructure/capnp_schemas/gen/go/registry");
interface Admin @0xf503f3237666574e superclasses(import "/common.capnp".Identifiable) {
  addCategory @0 (category :import "/common.capnp".IdInformation, upsert :Bool) -> (success :Bool);
  removeCategory @1 (categoryId :Text, moveObjectsToCategoryId :Text) -> (removedObjects :List(import "/common.capnp".Identifiable));
  moveObjects @2 (objectIds :List(Text), toCatId :Text) -> (movedObjectIds :List(Text));
  removeObjects @3 (objectIds :List(Text)) -> (removedObjects :List(import "/common.capnp".Identifiable));
  registry @4 () -> (registry :Registry);
}
interface Registry @0xca7b4bd1600633b8 superclasses(import "/common.capnp".Identifiable) {
  supportedCategories @0 () -> (cats :List(import "/common.capnp".IdInformation));
  categoryInfo @1 (categoryId :Text) -> import "/common.capnp".IdInformation;
  entries @2 (categoryId :Text) -> (entries :List(Entry));
  struct Entry @0xc17987510cf7ac13 {  # 0 bytes, 3 ptrs
    categoryId @0 :Text;  # ptr[0]
    ref @1 :import "/common.capnp".Identifiable;  # ptr[1]
    name @2 :Text;  # ptr[2]
  }
}
interface Registrar @0xabaef93c36f2d1ea superclasses(import "/common.capnp".Identifiable) {
  register @0 RegParams -> (unreg :import "/common.capnp".Action, reregSR :import "/persistence.capnp".SturdyRef);
  struct CrossDomainRestore @0xaa1198dd7e71b20e {  # 0 bytes, 2 ptrs
    vatId @0 :import "/persistence.capnp".VatId;  # ptr[0]
    restorer @1 :import "/persistence.capnp".Restorer;  # ptr[1]
  }
  struct RegParams @0xe5a84717ea75fb0d {  # 0 bytes, 4 ptrs
    cap @0 :import "/common.capnp".Identifiable;  # ptr[0]
    regName @1 :Text;  # ptr[1]
    categoryId @2 :Text;  # ptr[2]
    xDomain @3 :CrossDomainRestore;  # ptr[3]
  }
}
