extern crate capnpc;

fn main() {
    let mut compiler = ::capnpc::CompilerCommand::new();
    compiler.import_path("../vcpkg/packages/capnproto_x64-windows-static/include/");
    compiler.file("../vcpkg/packages/capnproto_x64-windows-static/include/capnp/persistent.capnp").run().unwrap();
    compiler.src_prefix("../capnproto_schemas").file("../capnproto_schemas/climate_data.capnp").run().unwrap();
    compiler.src_prefix("../capnproto_schemas").file("../capnproto_schemas/model.capnp").run().unwrap();
    compiler.src_prefix("../capnproto_schemas").file("../capnproto_schemas/common.capnp").run().unwrap();
    compiler.src_prefix("../capnproto_schemas").file("../capnproto_schemas/geo_coord.capnp").run().unwrap();
    compiler.src_prefix("../capnproto_schemas").file("../capnproto_schemas/cluster_admin_service.capnp").run().unwrap();
    compiler.src_prefix("../capnproto_schemas").file("../capnproto_schemas/service.capnp").run().unwrap();
}
