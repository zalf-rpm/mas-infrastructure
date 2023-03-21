cmake_minimum_required(VERSION 3.12)
project(MAS-infrastructure-capnp-schemas)

message(STATUS "-> MAS-infrastructure-capnp-schemas")

find_package(CapnProto CONFIG REQUIRED)

set(CAPNPC_SRC_PREFIX ${CAPNP_SCHEMAS_DIR})

capnp_generate_cpp(CAPNP_SRCS CAPNP_HDRS 
	common.capnp 
	climate.capnp
	cluster_admin_service.capnp
	crop.capnp
	date.capnp
	geo.capnp
	management.capnp
	model.capnp
	model/monica/monica_management.capnp
	model/monica/monica_params.capnp
	model/monica/soil_params.capnp
	model/monica/monica_state.capnp
	persistence.capnp
	restore_resolve.capnp
	registry.capnp
	soil.capnp
	storage.capnp
	model/yieldstat/yieldstat.capnp
	x.capnp
)
message(STATUS "CAPNP_HDRS: " ${CAPNP_HDRS})
message(STATUS "CAPNP_SRCS: " ${CAPNP_SRCS})	

add_library(capnp_schemas_lib 
	STATIC 
	${CAPNP_HDRS} 
	${CAPNP_SRCS}
)

target_link_libraries(capnp_schemas_lib 
	PUBLIC 
	CapnProto::capnp
	CapnProto::capnpc
	CapnProto::capnp-rpc
	CapnProto::kj
	CapnProto::kj-async
	CapnProto::capnp-json	
)
target_include_directories(capnp_schemas_lib PUBLIC ${CMAKE_CURRENT_BINARY_DIR})

if(MSVC AND MT_RUNTIME_LIB)
	target_compile_options(capnp_schemas_lib PRIVATE "/MT$<$<CONFIG:Debug>:d>")
endif()

#message(STATUS "capnp csd: " ${CMAKE_CURRENT_SOURCE_DIR} " cbd: " ${CMAKE_CURRENT_BINARY_DIR})

message(STATUS "<- MAS-infrastructure-capnp-schemas")