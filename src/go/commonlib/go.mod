module github.com/zalf-rpm/mas-infrastructure/src/go/commonlib

go 1.20

require (
	capnproto.org/go/capnp/v3 v3.0.1-alpha.2
	github.com/google/uuid v1.3.0
	github.com/pelletier/go-toml v1.9.5
	github.com/zalf-rpm/mas-infrastructure/capnproto_schemas/gen/go/common v0.0.0-20230412105359-2d45c32db41e
	github.com/zalf-rpm/mas-infrastructure/capnproto_schemas/gen/go/grid v0.0.0-20230713163933-4c7223175aeb
	github.com/zalf-rpm/mas-infrastructure/capnproto_schemas/gen/go/persistence v0.0.0-20230301145915-7fca44b89d25
	golang.org/x/crypto v0.6.0
)

require (
	github.com/colega/zeropool v0.0.0-20230505084239-6fb4a4f75381 // indirect
	github.com/zalf-rpm/mas-infrastructure/capnproto_schemas/gen/go/geo v0.0.0-20230208160538-deb034d36602 // indirect
	golang.org/x/sync v0.8.0 // indirect
)
