#/bin/sh

capnp compile -I. -ocapnp a.capnp > ./gen/capnp_offsets/test/a.capnp
capnp compile -I. -ocapnp climate.capnp > ./gen/capnp_offsets/climate/climate.capnp
capnp compile -I. -ocapnp cluster_admin_service.capnp > ./gen/capnp_offsets/cluster/cluster_admin_service.capnp
capnp compile -I. -ocapnp common.capnp > ./gen/capnp_offsets/common/common.capnp
capnp compile -I. -ocapnp config.capnp > ./gen/capnp_offsets/config/config.capnp
capnp compile -I. -ocapnp crop.capnp > ./gen/capnp_offsets/crop/crop.capnp
capnp compile -I. -ocapnp date.capnp > ./gen/capnp_offsets/common/date.capnp
capnp compile -I. -ocapnp fbp.capnp > ./gen/capnp_offsets/fbp/fbp.capnp
#capnp compile -I. -ocapnp frontend.capnp > ./gen/capnp_offsets/frontend/frontend.capnp
capnp compile -I. -ocapnp geo.capnp > ./gen/capnp_offsets/geo/geo.capnp
capnp compile -I. -ocapnp grid.capnp > ./gen/capnp_offsets/grid/grid.capnp
capnp compile -I. -ocapnp jobs.capnp > ./gen/capnp_offsets/jobs/jobs.capnp
capnp compile -I. -ocapnp management.capnp > ./gen/capnp_offsets/management/management.capnp
capnp compile -I. -ocapnp model.capnp > ./gen/capnp_offsets/model/model.capnp
capnp compile -I. -ocapnp persistence.capnp > ./gen/capnp_offsets/persistence/persistence.capnp
capnp compile -I. -ocapnp registry.capnp > ./gen/capnp_offsets/registry/registry.capnp
capnp compile -I. -ocapnp service.capnp > ./gen/capnp_offsets/service/service.capnp
capnp compile -I. -ocapnp soil.capnp > ./gen/capnp_offsets/soil/soil.capnp
capnp compile -I. -ocapnp storage.capnp > ./gen/capnp_offsets/storage/storage.capnp
#capnp compile -I. -ocapnp vr.capnp > ./gen/capnp_offsets/vr/vr.capnp

capnp compile -I. -ocapnp model/weberest/web-berest-data-import.capnp > ./gen/capnp_offsets/model/weberest/web-berest-data-import.capnp
capnp compile -I. -ocapnp model/yieldstat/yieldstat.capnp > ./gen/capnp_offsets/model/yieldstat/yieldstat.capnp
capnp compile -I. -ocapnp model/monica/monica_params.capnp > ./gen/capnp_offsets/model/monica/monica_params.capnp
capnp compile -I. -ocapnp model/monica/monica_state.capnp > ./gen/capnp_offsets/model/monica/monica_state.capnp
capnp compile -I. -ocapnp model/monica/soil_params.capnp > ./gen/capnp_offsets/model/monica/soil_params.capnp
