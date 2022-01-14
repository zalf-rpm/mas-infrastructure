#/bin/sh

capnp compile -I. -ogo capnp/persistent.capnp

capnp compile -I. -ogo climate_data.capnp
capnp compile -I. -ogo cluster_admin_service.capnp
capnp compile -I. -ogo common.capnp
capnp compile -I. -ogo crop.capnp
capnp compile -I. -ogo date.capnp
#capnp compile -I. -ogo fbp.capnp
#capnp compile -I. -ogo frontend.capnp
capnp compile -I. -ogo geo_coord.capnp
capnp compile -I. -ogo grid.capnp
capnp compile -I. -ogo jobs.capnp
capnp compile -I. -ogo management.capnp
capnp compile -I. -ogo model.capnp
capnp compile -I. -ogo persistence.capnp
capnp compile -I. -ogo registry.capnp
capnp compile -I. -ogo soil_data.capnp
#capnp compile -I. -ogo vr.capnp
#capnp compile -I. -ogo web-berest-data-import.capnp

capnp compile -I. -ogo models/yieldstat.capnp
capnp compile -I. -ogo models/monica/monica_params.capnp
capnp compile -I. -ogo models/monica/monica_state.capnp
capnp compile -I. -ogo models/monica/soil_params.capnp

