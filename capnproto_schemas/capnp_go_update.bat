
go install capnproto.org/go/capnp/v3/capnpc-go@v3.0.1-alpha.2
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

call capnp_compile_all.bat go

cd %WORKDIR%/gen/go/test
go mod tidy
cd %WORKDIR%/gen/go/climate
go mod tidy
cd %WORKDIR%/gen/go/cluster
go mod tidy
cd %WORKDIR%/gen/go/common
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
