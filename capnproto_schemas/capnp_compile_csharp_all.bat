mkdir csharp-out
cd csharp-out
capnp compile --src-prefix ..\..\capnproto\c++\src\capnp -I ..\..\capnproto\c++\src\ -ocsharp ..\..\capnproto\c++\src\capnp\persistent.capnp
capnp compile --src-prefix .. -I ..\..\capnproto\c++\src\ -ocsharp ..\common.capnp
capnp compile --src-prefix .. -I ..\..\capnproto\c++\src\ -ocsharp ..\climate_data.capnp
capnp compile --src-prefix .. -I ..\..\capnproto\c++\src\ -ocsharp ..\geo_coord.capnp
capnp compile --src-prefix .. -I ..\..\capnproto\c++\src\ -ocsharp ..\model.capnp
rem capnp compile --src-prefix .. -I ..\..\capnproto\c++\src\ -ocsharp service.capnp
capnp compile --src-prefix .. -I ..\..\capnproto\c++\src\ -ocsharp ..\cluster_admin_service.capnp
cd ..

rem capnp compile -I ..\capnproto\c++\src\ -ocsharp:csharp-out common.capnp
rem capnp compile -I ..\capnproto\c++\src\ -ocsharp:csharp-out climate_data.capnp
rem capnp compile -I ..\capnproto\c++\src\ -ocsharp:csharp-out geo_coord.capnp
rem capnp compile -I ..\capnproto\c++\src\ -ocsharp:csharp-out model.capnp
rem capnp compile -I ..\capnproto\c++\src\ -oc++:csharp-out service.capnp
rem capnp compile -I ..\capnproto\c++\src\ -ocsharp:csharp-out cluster_admin_service.capnp
