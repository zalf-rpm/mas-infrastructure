#/bin/sh

capnp compile -I. -ogo:./gen/go/persistence capnp/persistent.capnp

capnp compile -I. -ogo:./gen/go/test a.capnp
capnp compile -I. -ogo:./gen/go/climate climate_data.capnp
capnp compile -I. -ogo:./gen/go/cluster cluster_admin_service.capnp
capnp compile -I. -ogo:./gen/go/common common.capnp
capnp compile -I. -ogo:./gen/go/crop crop.capnp
capnp compile -I. -ogo:./gen/go/common date.capnp
capnp compile -I. -ogo:./gen/go/fbp fbp.capnp
capnp compile -I. -ogo:./gen/go/frontend frontend.capnp
capnp compile -I. -ogo:./gen/go/geo geo_coord.capnp
capnp compile -I. -ogo:./gen/go/grid grid.capnp
capnp compile -I. -ogo:./gen/go/jobs jobs.capnp
capnp compile -I. -ogo:./gen/go/management management.capnp
capnp compile -I. -ogo:./gen/go/models model.capnp
capnp compile -I. -ogo:./gen/go/persistence persistence.capnp
capnp compile -I. -ogo:./gen/go/registry registry.capnp
capnp compile -I. -ogo:./gen/go/soil soil_data.capnp
capnp compile -I. -ogo:./gen/go/vr vr.capnp

capnp compile -I. -ogo:./gen/go models/weberest/web-berest-data-import.capnp
capnp compile -I. -ogo:./gen/go models/yieldstat/yieldstat.capnp
capnp compile -I. -ogo:./gen/go models/monica/monica_params.capnp
capnp compile -I. -ogo:./gen/go models/monica/monica_state.capnp
capnp compile -I. -ogo:./gen/go models/monica/soil_params.capnp

