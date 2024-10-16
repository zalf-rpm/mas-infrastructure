cmake_minimum_required(VERSION 3.12)
project(MAS-infrastructure-capnp-schemas)

message(STATUS "-> MAS-infrastructure-capnp-schemas-gen-c++")

find_package(CapnProto CONFIG REQUIRED)
find_package(ZLIB)

add_library(capnp_schemas_lib
		STATIC
		common/common.capnp.h
		common/common.capnp.c++
		common/date.capnp.h
		common/date.capnp.c++

		climate/climate.capnp.h
		climate/climate.capnp.c++

		cluster/cluster_admin_service.capnp.h
		cluster/cluster_admin_service.capnp.c++

		crop/crop.capnp.h
		crop/crop.capnp.c++

		fbp/fbp.capnp.h
		fbp/fbp.capnp.c++

		geo/geo.capnp.h
		geo/geo.capnp.c++

		management/management.capnp.h
		management/management.capnp.c++

		model/model.capnp.h
		model/model.capnp.c++

		model/monica/monica_management.capnp.h
		model/monica/monica_management.capnp.c++
		model/monica/monica_params.capnp.h
		model/monica/monica_params.capnp.c++
		model/monica/soil_params.capnp.h
		model/monica/soil_params.capnp.c++
		model/monica/monica_state.capnp.h
		model/monica/monica_state.capnp.c++

		persistence/persistence.capnp.h
		persistence/persistence.capnp.c++

		registry/registry.capnp.h
		registry/registry.capnp.c++

		service/service.capnp.h
		service/service.capnp.c++

		soil/soil.capnp.h
		soil/soil.capnp.c++

		storage/storage.capnp.h
		storage/storage.capnp.c++


		model/yieldstat/yieldstat.capnp.h
		model/yieldstat/yieldstat.capnp.c++

		test/x.capnp.h
		test/x.capnp.c++
)

target_link_libraries(capnp_schemas_lib 
	PUBLIC 
	CapnProto::capnp
	CapnProto::capnpc
	CapnProto::capnp-rpc
	CapnProto::kj
	CapnProto::kj-async
	CapnProto::kj-http
	CapnProto::kj-gzip
	CapnProto::capnp-json	
)
target_include_directories(capnp_schemas_lib
		PUBLIC
		${CMAKE_CURRENT_SOURCE_DIR}
		${CMAKE_CURRENT_SOURCE_DIR}/common
		${CMAKE_CURRENT_SOURCE_DIR}/climate
		${CMAKE_CURRENT_SOURCE_DIR}/cluster
		${CMAKE_CURRENT_SOURCE_DIR}/crop
		${CMAKE_CURRENT_SOURCE_DIR}/fbp
		${CMAKE_CURRENT_SOURCE_DIR}/geo
		${CMAKE_CURRENT_SOURCE_DIR}/management
		${CMAKE_CURRENT_SOURCE_DIR}/model
		${CMAKE_CURRENT_SOURCE_DIR}/model/monica
		${CMAKE_CURRENT_SOURCE_DIR}/persistence
		${CMAKE_CURRENT_SOURCE_DIR}/registry
		${CMAKE_CURRENT_SOURCE_DIR}/service
		${CMAKE_CURRENT_SOURCE_DIR}/soil
		${CMAKE_CURRENT_SOURCE_DIR}/storage
		${CMAKE_CURRENT_SOURCE_DIR}/model/yieldstat
		${CMAKE_CURRENT_SOURCE_DIR}/test
)
message(STATUS ${CMAKE_CURRENT_SOURCE_DIR})

if(MSVC AND MT_RUNTIME_LIB)
	target_compile_options(capnp_schemas_lib PRIVATE "/MT$<$<CONFIG:Debug>:d>")
endif()

#message(STATUS "capnp csd: " ${CMAKE_CURRENT_SOURCE_DIR} " cbd: " ${CMAKE_CURRENT_BINARY_DIR})

message(STATUS "<- MAS-infrastructure-capnp-schemas-gen-c++")
