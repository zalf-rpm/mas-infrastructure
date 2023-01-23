cmake_minimum_required(VERSION 3.12)
project(MAS-infrastructure-soil)

message(STATUS "-> MAS-infrastructure-soil")

# possibly create dependent libs first
#------------------------------------------------------------------------------

if(NOT TARGET helpers_lib)
	message(STATUS "target: helpers_lib")
	add_subdirectory(../tools/helpers helpers)
endif()
get_target_property(helpers_interface_includes helpers_lib INTERFACE_INCLUDE_DIRECTORIES)

if(NOT TARGET capnp_schemas_lib)
	message(STATUS "target: capnp_schemas_lib")
	add_subdirectory(../../../capnproto_schemas capnproto_schemas)
endif()
get_target_property(capnp_schemas_interface_includes capnp_schemas_lib INTERFACE_INCLUDE_DIRECTORIES)

#------------------------------------------------------------------------------

add_library(soil_lib 
	STATIC 
	constants.h 
	constants.cpp 
	conversion.h 
	conversion.cpp 
	soil.h 
	soil.cpp
)
target_link_libraries(soil_lib 
	PUBLIC 
	helpers_lib 
	capnp_schemas_lib
)
target_include_directories(soil_lib 
	INTERFACE 
	${capnp_schemas_interface_includes}
	${helpers_interface_includes}
	${CMAKE_CURRENT_SOURCE_DIR}
)

if(MSVC AND MT_RUNTIME_LIB)
	target_compile_options(soil_lib PRIVATE "/MT$<$<CONFIG:Debug>:d>")
endif()

message(STATUS "<- MAS-infrastructure-soil")