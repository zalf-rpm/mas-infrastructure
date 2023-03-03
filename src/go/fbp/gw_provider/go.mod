module github.com/zalf-rpm/mas-infrastructure/src/go/fbp/gw_provider

go 1.19

require (
	github.com/google/uuid v1.3.0
	github.com/zalf-rpm/mas-infrastructure/capnproto_schemas/gen/go/common v0.0.0-20230213122748-e44d2e5bf661
)

replace (
	github.com/zalf-rpm/mas-infrastructure/capnproto_schemas/gen/go/commonm => ../../../../capnproto_schemas/gen/go/common
	github.com/zalf-rpm/mas-infrastructure/capnproto_schemas/gen/go/persistence => ../../../../capnproto_schemas/gen/go/persistence
)

require (
	capnproto.org/go/capnp/v3 v3.0.0-alpha.24 // indirect
	github.com/zalf-rpm/mas-infrastructure/capnproto_schemas/gen/go/persistence v0.0.0-20230213122748-e44d2e5bf661 // indirect
	golang.org/x/sync v0.0.0-20201020160332-67f06af15bc9 // indirect
)
