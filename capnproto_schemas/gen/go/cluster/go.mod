module github.com/zalf-rpm/mas-infrastructure/capnproto_schemas/gen/go/cluster

go 1.19

require (
	capnproto.org/go/capnp/v3 v3.0.1-alpha.2
	github.com/zalf-rpm/mas-infrastructure/capnproto_schemas/gen/go/common v0.0.0-20230208160538-deb034d36602
)

replace github.com/zalf-rpm/mas-infrastructure/capnproto_schemas/gen/go/common => ../common

require (
	github.com/colega/zeropool v0.0.0-20230505084239-6fb4a4f75381 // indirect
	golang.org/x/sync v0.8.0 // indirect
)
