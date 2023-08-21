module github.com/zalf-rpm/mas-infrastructure/src/go/test/test_client

go 1.20

require (
	capnproto.org/go/capnp/v3 v3.0.0-alpha-29
	github.com/zalf-rpm/mas-infrastructure/capnproto_schemas/gen/go/test v0.0.0-20230222184210-1b7d7cf765c1
)

replace github.com/zalf-rpm/mas-infrastructure/capnproto_schemas/gen/go/test => ../../../../capnproto_schemas/gen/go/test

require (
	golang.org/x/sync v0.3.0 // indirect
	zenhack.net/go/util v0.0.0-20230607025951-8b02fee814ae // indirect
)
