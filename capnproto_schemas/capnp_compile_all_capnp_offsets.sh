#!/bin/sh

mkdir -p gen/capnp_offsets/test ; capnp compile -Izalfmas_capnp_schemas --src-prefix=zalfmas_capnp_schemas -ocapnp zalfmas_capnp_schemas/x.capnp > ./gen/capnp_offsets/test/x.capnp
mkdir -p gen/capnp_offsets/test ; capnp compile -Izalfmas_capnp_schemas --src-prefix=zalfmas_capnp_schemas -ocapnp zalfmas_capnp_schemas/a.capnp > ./gen/capnp_offsets/test/a.capnp

mkdir -p gen/capnp_offsets/climate ; capnp compile -Izalfmas_capnp_schemas --src-prefix=zalfmas_capnp_schemas -ocapnp zalfmas_capnp_schemas/climate.capnp > ./gen/capnp_offsets/climate/climate.capnp
mkdir -p gen/capnp_offsets/cluster ; capnp compile -Izalfmas_capnp_schemas --src-prefix=zalfmas_capnp_schemas -ocapnp zalfmas_capnp_schemas/cluster_admin_service.capnp > ./gen/capnp_offsets/cluster/cluster_admin_service.capnp
mkdir -p gen/capnp_offsets/common ; capnp compile -Izalfmas_capnp_schemas --src-prefix=zalfmas_capnp_schemas -ocapnp zalfmas_capnp_schemas/common.capnp > ./gen/capnp_offsets/common/common.capnp
mkdir -p gen/capnp_offsets/config ; capnp compile -Izalfmas_capnp_schemas --src-prefix=zalfmas_capnp_schemas -ocapnp zalfmas_capnp_schemas/config.capnp > ./gen/capnp_offsets/config/config.capnp
mkdir -p gen/capnp_offsets/crop ; capnp compile -Izalfmas_capnp_schemas --src-prefix=zalfmas_capnp_schemas -ocapnp zalfmas_capnp_schemas/crop.capnp > ./gen/capnp_offsets/crop/crop.capnp
mkdir -p gen/capnp_offsets/common ; capnp compile -Izalfmas_capnp_schemas --src-prefix=zalfmas_capnp_schemas -ocapnp zalfmas_capnp_schemas/date.capnp > ./gen/capnp_offsets/common/date.capnp
mkdir -p gen/capnp_offsets/fbp ; capnp compile -Izalfmas_capnp_schemas --src-prefix=zalfmas_capnp_schemas -ocapnp zalfmas_capnp_schemas/fbp.capnp > ./gen/capnp_offsets/fbp/fbp.capnp
#mkdir -p gen/capnp_offsets/frontend ; capnp compile -Izalfmas_capnp_schemas --src-prefix=zalfmas_capnp_schemas -ocapnp zalfmas_capnp_schemas/frontend.capnp > ./gen/capnp_offsets/frontend/frontend.capnp
mkdir -p gen/capnp_offsets/geo ; capnp compile -Izalfmas_capnp_schemas --src-prefix=zalfmas_capnp_schemas -ocapnp zalfmas_capnp_schemas/geo.capnp > ./gen/capnp_offsets/geo/geo.capnp
mkdir -p gen/capnp_offsets/grid ; capnp compile -Izalfmas_capnp_schemas --src-prefix=zalfmas_capnp_schemas -ocapnp zalfmas_capnp_schemas/grid.capnp > ./gen/capnp_offsets/grid/grid.capnp
mkdir -p gen/capnp_offsets/jobs ; capnp compile -Izalfmas_capnp_schemas --src-prefix=zalfmas_capnp_schemas -ocapnp zalfmas_capnp_schemas/jobs.capnp > ./gen/capnp_offsets/jobs/jobs.capnp
mkdir -p gen/capnp_offsets/management ; capnp compile -Izalfmas_capnp_schemas --src-prefix=zalfmas_capnp_schemas -ocapnp zalfmas_capnp_schemas/management.capnp > ./gen/capnp_offsets/management/management.capnp
mkdir -p gen/capnp_offsets/model ; capnp compile -Izalfmas_capnp_schemas --src-prefix=zalfmas_capnp_schemas -ocapnp zalfmas_capnp_schemas/model.capnp > ./gen/capnp_offsets/model/model.capnp
mkdir -p gen/capnp_offsets/persistence ; capnp compile -Izalfmas_capnp_schemas --src-prefix=zalfmas_capnp_schemas -ocapnp zalfmas_capnp_schemas/persistence.capnp > ./gen/capnp_offsets/persistence/persistence.capnp
mkdir -p gen/capnp_offsets/registry ; capnp compile -Izalfmas_capnp_schemas --src-prefix=zalfmas_capnp_schemas -ocapnp zalfmas_capnp_schemas/registry.capnp > ./gen/capnp_offsets/registry/registry.capnp
mkdir -p gen/capnp_offsets/service ; capnp compile -Izalfmas_capnp_schemas --src-prefix=zalfmas_capnp_schemas -ocapnp zalfmas_capnp_schemas/service.capnp > ./gen/capnp_offsets/service/service.capnp
mkdir -p gen/capnp_offsets/soil ; capnp compile -Izalfmas_capnp_schemas --src-prefix=zalfmas_capnp_schemas -ocapnp zalfmas_capnp_schemas/soil.capnp > ./gen/capnp_offsets/soil/soil.capnp
mkdir -p gen/capnp_offsets/storage ; capnp compile -Izalfmas_capnp_schemas --src-prefix=zalfmas_capnp_schemas -ocapnp zalfmas_capnp_schemas/storage.capnp > ./gen/capnp_offsets/storage/storage.capnp
#mkdir -p gen/capnp_offsets/vr ; capnp compile -Izalfmas_capnp_schemas --src-prefix=zalfmas_capnp_schemas -ocapnp zalfmas_capnp_schemas/vr.capnp > ./gen/capnp_offsets/vr/vr.capnp

mkdir -p gen/capnp_offsets/model/weberest ; capnp compile -Izalfmas_capnp_schemas --src-prefix=zalfmas_capnp_schemas -ocapnp zalfmas_capnp_schemas/model/weberest/web-berest-data-import.capnp > ./gen/capnp_offsets/model/weberest/web-berest-data-import.capnp
mkdir -p gen/capnp_offsets/model/yieldstat ; capnp compile -Izalfmas_capnp_schemas --src-prefix=zalfmas_capnp_schemas -ocapnp zalfmas_capnp_schemas/model/yieldstat/yieldstat.capnp > ./gen/capnp_offsets/model/yieldstat/yieldstat.capnp
mkdir -p gen/capnp_offsets/model/monica ; capnp compile -Izalfmas_capnp_schemas --src-prefix=zalfmas_capnp_schemas -ocapnp zalfmas_capnp_schemas/model/monica/monica_management.capnp > ./gen/capnp_offsets/model/monica/monica_management.capnp
mkdir -p gen/capnp_offsets/model/monica ; capnp compile -Izalfmas_capnp_schemas --src-prefix=zalfmas_capnp_schemas -ocapnp zalfmas_capnp_schemas/model/monica/monica_params.capnp > ./gen/capnp_offsets/model/monica/monica_params.capnp
mkdir -p gen/capnp_offsets/model/monica ; capnp compile -Izalfmas_capnp_schemas --src-prefix=zalfmas_capnp_schemas -ocapnp zalfmas_capnp_schemas/model/monica/monica_state.capnp > ./gen/capnp_offsets/model/monica/monica_state.capnp
mkdir -p gen/capnp_offsets/model/monica ; capnp compile -Izalfmas_capnp_schemas --src-prefix=zalfmas_capnp_schemas -ocapnp zalfmas_capnp_schemas/model/monica/soil_params.capnp > ./gen/capnp_offsets/model/monica/soil_params.capnp
