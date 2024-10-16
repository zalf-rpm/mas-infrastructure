# date.capnp
@0xe8ea2bc38b07f62a;
$import "/capnp/c++.capnp".namespace("mas::schema::common");
$import "/capnp/go.capnp".package("common_date");
$import "/capnp/go.capnp".import("github.com/zalf-rpm/mas-infrastructure/capnproto_schemas/gen/go/common_date");
struct Date @0x97e6feac0322118d {  # 8 bytes, 0 ptrs
  year @0 :Int16;  # bits[0, 16)
  month @1 :UInt8;  # bits[16, 24)
  day @2 :UInt8;  # bits[24, 32)
}
