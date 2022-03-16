#/bin/sh

#cd capnp
#capnp compile -I.. -ocsharp:../gen/csharp/persistence persistent.capnp
#cd ..

capnp compile -I. -ocsharp:./gen/csharp/test a.capnp
capnp compile -I. -ocsharp:./gen/csharp/climate climate_data.capnp
capnp compile -I. -ocsharp:./gen/csharp/cluster cluster_admin_service.capnp
capnp compile -I. -ocsharp:./gen/csharp/common common.capnp
capnp compile -I. -ocsharp:./gen/csharp/config config.capnp
capnp compile -I. -ocsharp:./gen/csharp/crop crop.capnp
capnp compile -I. -ocsharp:./gen/csharp/common date.capnp
capnp compile -I. -ocsharp:./gen/csharp/fbp fbp.capnp
#capnp compile -I. -ocsharp:./gen/csharp/frontend frontend.capnp
capnp compile -I. -ocsharp:./gen/csharp/geo geo_coord.capnp
capnp compile -I. -ocsharp:./gen/csharp/grid grid.capnp
capnp compile -I. -ocsharp:./gen/csharp/jobs jobs.capnp
capnp compile -I. -ocsharp:./gen/csharp/management management.capnp
capnp compile -I. -ocsharp:./gen/csharp/model model.capnp
capnp compile -I. -ocsharp:./gen/csharp/persistence persistence.capnp
capnp compile -I. -ocsharp:./gen/csharp/registry registry.capnp
capnp compile -I. -ocsharp:./gen/csharp/service service.capnp
capnp compile -I. -ocsharp:./gen/csharp/soil soil_data.capnp
#capnp compile -I. -ocsharp:./gen/csharp/vr vr.capnp

capnp compile -I. -ocsharp:./gen/csharp model/weberest/web-berest-data-import.capnp
capnp compile -I. -ocsharp:./gen/csharp model/yieldstat/yieldstat.capnp
capnp compile -I. -ocsharp:./gen/csharp model/monica/monica_params.capnp
capnp compile -I. -ocsharp:./gen/csharp model/monica/monica_state.capnp
capnp compile -I. -ocsharp:./gen/csharp model/monica/soil_params.capnp

