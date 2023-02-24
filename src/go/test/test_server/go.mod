module github.com/zalf-rpm/mas-infrastructure/src/go/test/test_server

go 1.20

require (
	capnproto.org/go/capnp/v3 v3.0.0-alpha.24
	github.com/zalf-rpm/mas-infrastructure/capnproto_schemas/gen/go/test v0.0.0-20230222184210-1b7d7cf765c1
)
replace (
	github.com/zalf-rpm/mas-infrastructure/capnproto_schemas/gen/go/test => ../../../../capnproto_schemas/gen/go/test
)
require golang.org/x/sync v0.0.0-20201020160332-67f06af15bc9 // indirect
