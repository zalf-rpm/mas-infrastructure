module github.com/zalf-rpm/mas-infrastructure/capnproto_schemas/gen/go/crop

go 1.19

require (
	capnproto.org/go/capnp/v3 v3.0.0-alpha-29
	github.com/zalf-rpm/mas-infrastructure/capnproto_schemas/gen/go/common v0.0.0-20230208160538-deb034d36602
	github.com/zalf-rpm/mas-infrastructure/capnproto_schemas/gen/go/persistence v0.0.0-20230208160538-deb034d36602
	github.com/zalf-rpm/mas-infrastructure/capnproto_schemas/gen/go/registry v0.0.0-20230412105359-2d45c32db41e
)

replace github.com/zalf-rpm/mas-infrastructure/capnproto_schemas/gen/go/common => ../common

require (
	golang.org/x/sync v0.3.0 // indirect
	zenhack.net/go/util v0.0.0-20230607025951-8b02fee814ae // indirect
)
