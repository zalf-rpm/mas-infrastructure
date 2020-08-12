mkdir cpp-out
capnp compile -I ..\capnproto\c++\src\ -oc++:cpp-out common.capnp
capnp compile -I ..\capnproto\c++\src\ -oc++:cpp-out climate_data.capnp
capnp compile -I ..\capnproto\c++\src\ -oc++:cpp-out geo_coord.capnp
capnp compile -I ..\capnproto\c++\src\ -oc++:cpp-out model.capnp
rem capnp compile -I ..\capnproto\c++\src\ -oc++:cpp-out service.capnp
capnp compile -I ..\capnproto\c++\src\ -oc++:cpp-out cluster_admin_service.capnp
