#/bin/sh

#cd capnp
#capnp compile -I.. -oc++:../gen/csharp/persistence persistent.capnp
#cd ..

capnp compile -I. -oc++:./gen/cpp/test a.capnp
capnp compile -I. -oc++:./gen/cpp/climate climate.capnp
capnp compile -I. -oc++:./gen/cpp/cluster cluster_admin_service.capnp
capnp compile -I. -oc++:./gen/cpp/common common.capnp
capnp compile -I. -oc++:./gen/cpp/config config.capnp
capnp compile -I. -oc++:./gen/cpp/crop crop.capnp
capnp compile -I. -oc++:./gen/cpp/common date.capnp
capnp compile -I. -oc++:./gen/cpp/fbp fbp.capnp
#capnp compile -I. -oc++:./gen/cpp/frontend frontend.capnp
capnp compile -I. -oc++:./gen/cpp/geo geo.capnp
capnp compile -I. -oc++:./gen/cpp/grid grid.capnp
capnp compile -I. -oc++:./gen/cpp/jobs jobs.capnp
capnp compile -I. -oc++:./gen/cpp/management management.capnp
capnp compile -I. -oc++:./gen/cpp/model model.capnp
capnp compile -I. -oc++:./gen/cpp/persistence persistence.capnp
capnp compile -I. -oc++:./gen/cpp/registry registry.capnp
capnp compile -I. -oc++:./gen/cpp/service service.capnp
capnp compile -I. -oc++:./gen/cpp/soil soil.capnp
#capnp compile -I. -oc++:./gen/cpp/vr vr.capnp

capnp compile -I. -oc++:./gen/cpp model/weberest/web-berest-data-import.capnp
capnp compile -I. -oc++:./gen/cpp model/yieldstat/yieldstat.capnp
capnp compile -I. -oc++:./gen/cpp model/monica/monica_management.capnp
capnp compile -I. -oc++:./gen/cpp model/monica/monica_params.capnp
capnp compile -I. -oc++:./gen/cpp model/monica/monica_state.capnp
capnp compile -I. -oc++:./gen/cpp model/monica/soil_params.capnp

