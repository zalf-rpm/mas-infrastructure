module github.com/zalf-rpm/mas-infrastructure/src/go/services/gw_service

go 1.20

require (
	capnproto.org/go/capnp/v3 v3.0.0-alpha-29
	github.com/batchatco/go-native-netcdf v0.0.0-20230103061018-5849c1f424b1
	github.com/google/go-cmp v0.5.9
	github.com/google/uuid v1.3.0
	github.com/stretchr/testify v1.8.2
	github.com/zalf-rpm/mas-infrastructure/capnproto_schemas/gen/go/grid v0.0.0-20230713163933-4c7223175aeb
	github.com/zalf-rpm/mas-infrastructure/capnproto_schemas/gen/go/persistence v0.0.0-20230324104523-84603c6779f4
	github.com/zalf-rpm/mas-infrastructure/src/go/commonlib v0.0.0-20230504170559-ad98d70ce1d5
	gonum.org/v1/gonum v0.12.0
)

require (
	github.com/batchatco/go-thrower v0.0.0-20200827035905-5cb7337f6be6 // indirect
	github.com/davecgh/go-spew v1.1.1 // indirect
	github.com/pelletier/go-toml v1.9.5 // indirect
	github.com/pmezard/go-difflib v1.0.0 // indirect
	github.com/zalf-rpm/mas-infrastructure/capnproto_schemas/gen/go/common v0.0.0-20230412105359-2d45c32db41e // indirect
	github.com/zalf-rpm/mas-infrastructure/capnproto_schemas/gen/go/geo v0.0.0-20230208160538-deb034d36602 // indirect
	golang.org/x/crypto v0.6.0 // indirect
	golang.org/x/sync v0.3.0 // indirect
	gopkg.in/yaml.v3 v3.0.1 // indirect
	zenhack.net/go/util v0.0.0-20230607025951-8b02fee814ae // indirect
)

replace github.com/zalf-rpm/mas-infrastructure/src/go/commonlib => ../../commonlib
