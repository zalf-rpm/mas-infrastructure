module github.com/zalf-rpm/mas-infrastructure/capnproto_schemas/gen/go/common

go 1.19

require (
	capnproto.org/go/capnp/v3 v3.0.0-alpha.24
	github.com/zalf-rpm/mas-infrastructure/capnproto_schemas/gen/go/persistence v0.0.0-20230208124858-035e95fdea9f
)
replace (
	github.com/zalf-rpm/mas-infrastructure/capnproto_schemas/gen/go/persistence => ../persistence
)



require golang.org/x/sync v0.0.0-20201020160332-67f06af15bc9 // indirect
