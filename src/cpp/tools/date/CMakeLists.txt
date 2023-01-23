cmake_minimum_required(VERSION 3.12)
project(MAS-infrastructure-tools-date)

message(STATUS "-> MAS-infrastructure-tools-date")

if(CAPNPROTO_SERIALIZATION_SUPPORT)
	if(NOT TARGET capnp_schemas_lib)
		message(STATUS "target: capnp_schemas_lib")
		add_subdirectory(../../../../capnproto_schemas capnproto_schemas)
	endif()
	get_target_property(capnp_schemas_interface_includes capnp_schemas_lib INTERFACE_INCLUDE_DIRECTORIES)
endif()

add_library(date_lib 
	STATIC 
	../date.h 
	../date.cpp
)
if(CAPNPROTO_SERIALIZATION_SUPPORT)
	target_link_libraries(date_lib PUBLIC capnp_schemas_lib)
	target_include_directories(date_lib INTERFACE ${capnp_schemas_interface_includes})
endif()
target_include_directories(date_lib INTERFACE ${CMAKE_CURRENT_SOURCE_DIR}/..)
if(MSVC AND MT_RUNTIME_LIB)
	target_compile_options(date_lib PRIVATE "/MT$<$<CONFIG:Debug>:d>")
endif()

message(STATUS "<- MAS-infrastructure-tools-date")