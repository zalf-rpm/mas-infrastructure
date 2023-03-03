module github.com/zalf-rpm/mas-infrastructure/src/go/commonlib

go 1.20

require (
	capnproto.org/go/capnp/v3 v3.0.0-alpha.24
	github.com/google/uuid v1.3.0
	github.com/zalf-rpm/mas-infrastructure/capnproto_schemas/gen/go/common v0.0.0-20230301145915-7fca44b89d25
	github.com/zalf-rpm/mas-infrastructure/capnproto_schemas/gen/go/persistence v0.0.0-20230301145915-7fca44b89d25
	golang.org/x/crypto v0.6.0
)

require golang.org/x/sync v0.0.0-20201020160332-67f06af15bc9 // indirect
