module github.com/zalf-rpm/mas-infrastructure/capnproto_schemas/gen/go/crop

go 1.19

require (
	capnproto.org/go/capnp/v3 v3.0.0-alpha.24
	github.com/zalf-rpm/mas-infrastructure/capnproto_schemas/gen/go/common v0.0.0-20230208160538-deb034d36602
	github.com/zalf-rpm/mas-infrastructure/capnproto_schemas/gen/go/persistence v0.0.0-20230208150301-1e037e4be0ba
)

replace github.com/zalf-rpm/mas-infrastructure/capnproto_schemas/gen/go/common => ../common

require golang.org/x/sync v0.0.0-20201020160332-67f06af15bc9 // indirect