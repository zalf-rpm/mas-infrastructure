module github.com/zalf-rpm/mas-infrastructure/src/go/test/test_client

go 1.20

require (
	capnproto.org/go/capnp/v3 v3.0.1-alpha.2
	github.com/zalf-rpm/mas-infrastructure/capnproto_schemas/gen/go/test v0.0.0-20230222184210-1b7d7cf765c1
)

replace github.com/zalf-rpm/mas-infrastructure/capnproto_schemas/gen/go/test => ../../../../capnproto_schemas/gen/go/test

require (
	github.com/colega/zeropool v0.0.0-20230505084239-6fb4a4f75381 // indirect
	golang.org/x/sync v0.8.0 // indirect
)
