project(MAS-infrastructure-capnp-schemas)

message(STATUS "-> MAS-infrastructure-capnp-schemas")

find_package(CapnProto CONFIG REQUIRED)

set(CAPNPC_SRC_PREFIX ${CAPNP_SCHEMAS_DIR})
capnp_generate_cpp(CAPNP_SRCS CAPNP_HDRS 
common.capnp 
climate_data.capnp
cluster_admin_service.capnp
crop.capnp
date.capnp
geo_coord.capnp
management.capnp
model.capnp
model/monica/monica_params.capnp
model/monica/soil_params.capnp
model/monica/monica_state.capnp
persistence.capnp
registry.capnp
soil_data.capnp
model/yieldstat/yieldstat.capnp
)
message(STATUS "CAPNP_HDRS: " ${CAPNP_HDRS})
message(STATUS "CAPNP_SRCS: " ${CAPNP_SRCS})	

add_library(capnp_schemas_lib STATIC ${CAPNP_HDRS} ${CAPNP_SRCS})
target_link_libraries(capnp_schemas_lib PUBLIC CapnProto::capnp-rpc CapnProto::capnp-json)
#target_include_directories(capnp_schemas_lib PUBLIC ${CMAKE_CURRENT_SOURCE_DIR} ..)
if(MSVC AND MT_RUNTIME_LIB)
	target_compile_options(capnp_schemas_lib PRIVATE "/MT$<$<CONFIG:Debug>:d>")
endif()

#add_custom_target(
#    generate-data-capnp-h
#    DEPENDS ${CMAKE_CURRENT_BINARY_DIR}/../../mas-infrastructure/capnproto_schemas/date.capnp.h
#    )

message(STATUS "capnp cbd: " ${CMAKE_CURRENT_BINARY_DIR})

message(STATUS "<- MAS-infrastructure-capnp-schemas")