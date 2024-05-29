
go install capnproto.org/go/capnp/v3/capnpc-go@latest
rem current dir
set "WORKDIR=%~dp0"
echo %WORKDIR%

cd %WORKDIR%/gen/go/test
go get -u capnproto.org/go/capnp/v3/
cd %WORKDIR%/gen/go/climate
go get -u capnproto.org/go/capnp/v3/
cd %WORKDIR%/gen/go/cluster
go get -u capnproto.org/go/capnp/v3/
cd %WORKDIR%/gen/go/common
go get -u capnproto.org/go/capnp/v3/
cd %WORKDIR%/gen/go/common_date
go get -u capnproto.org/go/capnp/v3/
cd %WORKDIR%/gen/go/config
go get -u capnproto.org/go/capnp/v3/
cd %WORKDIR%/gen/go/crop
go get -u capnproto.org/go/capnp/v3/
cd %WORKDIR%/gen/go/common
go get -u capnproto.org/go/capnp/v3/
cd %WORKDIR%/gen/go/fbp
go get -u capnproto.org/go/capnp/v3/
cd %WORKDIR%/gen/go/geo
go get -u capnproto.org/go/capnp/v3/
cd %WORKDIR%/gen/go/grid
go get -u capnproto.org/go/capnp/v3/
cd %WORKDIR%/gen/go/jobs
go get -u capnproto.org/go/capnp/v3/
cd %WORKDIR%/gen/go/management
go get -u capnproto.org/go/capnp/v3/
cd %WORKDIR%/gen/go/model
go get -u capnproto.org/go/capnp/v3/
cd %WORKDIR%/gen/go/persistence
go get -u capnproto.org/go/capnp/v3/
cd %WORKDIR%/gen/go/registry
go get -u capnproto.org/go/capnp/v3/
cd %WORKDIR%/gen/go/service
go get -u capnproto.org/go/capnp/v3/
cd %WORKDIR%/gen/go/soil
go get -u capnproto.org/go/capnp/v3/
cd %WORKDIR%/gen/go/storage
go get -u capnproto.org/go/capnp/v3/
cd %WORKDIR%/gen/go/model/weberest
go get -u capnproto.org/go/capnp/v3/
cd %WORKDIR%/gen/go/model/yieldstat
go get -u capnproto.org/go/capnp/v3/
cd %WORKDIR%/gen/go/model/monica
go get -u capnproto.org/go/capnp/v3/

cd %WORKDIR%

capnp compile -I. -ogo:./gen/go/test a.capnp
capnp compile -I. -ogo:./gen/go/climate climate.capnp
capnp compile -I. -ogo:./gen/go/cluster cluster_admin_service.capnp
capnp compile -I. -ogo:./gen/go/common common.capnp
capnp compile -I. -ogo:./gen/go/config config.capnp
capnp compile -I. -ogo:./gen/go/crop crop.capnp
capnp compile -I. -ogo:./gen/go/common_date date.capnp
capnp compile -I. -ogo:./gen/go/fbp fbp.capnp

capnp compile -I. -ogo:./gen/go/geo geo.capnp
capnp compile -I. -ogo:./gen/go/grid grid.capnp
capnp compile -I. -ogo:./gen/go/jobs jobs.capnp
capnp compile -I. -ogo:./gen/go/management management.capnp
capnp compile -I. -ogo:./gen/go/model model.capnp
capnp compile -I. -ogo:./gen/go/persistence persistence.capnp
capnp compile -I. -ogo:./gen/go/registry registry.capnp
capnp compile -I. -ogo:./gen/go/service service.capnp
capnp compile -I. -ogo:./gen/go/soil soil.capnp
capnp compile -I. -ogo:./gen/go/storage storage.capnp

capnp compile -I. -ogo:./gen/go model/weberest/web-berest-data-import.capnp
capnp compile -I. -ogo:./gen/go model/yieldstat/yieldstat.capnp

capnp compile -I. -ogo:./gen/go model/monica/monica_management.capnp
capnp compile -I. -ogo:./gen/go model/monica/monica_params.capnp
capnp compile -I. -ogo:./gen/go model/monica/monica_state.capnp
capnp compile -I. -ogo:./gen/go model/monica/soil_params.capnp


cd %WORKDIR%/gen/go/test
go mod tidy
cd %WORKDIR%/gen/go/climate
go mod tidy
cd %WORKDIR%/gen/go/cluster
go mod tidy
cd %WORKDIR%/gen/go/common
go mod tidy
cd %WORKDIR%/gen/go/common_date
go mod tidy
cd %WORKDIR%/gen/go/config
go mod tidy
cd %WORKDIR%/gen/go/crop
go mod tidy
cd %WORKDIR%/gen/go/common
go mod tidy
cd %WORKDIR%/gen/go/fbp
go mod tidy
cd %WORKDIR%/gen/go/geo
go mod tidy
cd %WORKDIR%/gen/go/grid
go mod tidy
cd %WORKDIR%/gen/go/jobs
go mod tidy
cd %WORKDIR%/gen/go/management
go mod tidy
cd %WORKDIR%/gen/go/model
go mod tidy
cd %WORKDIR%/gen/go/persistence
go mod tidy
cd %WORKDIR%/gen/go/registry
go mod tidy
cd %WORKDIR%/gen/go/service
go mod tidy
cd %WORKDIR%/gen/go/soil
go mod tidy
cd %WORKDIR%/gen/go/storage
go mod tidy
cd %WORKDIR%/gen/go/model/weberest
go mod tidy
cd %WORKDIR%/gen/go/model/yieldstat
go mod tidy
cd %WORKDIR%/gen/go/model/monica
