cmake_minimum_required(VERSION 3.12)
project(MAS-infrastructure-climate-climate_file_io)

message(STATUS "-> MAS-infrastructure-climate-climate_file_io")

if(NOT TARGET debug_lib)
	message(STATUS "target: debug_lib")
	add_subdirectory(../../tools/debug debug)
endif()
get_target_property(debug_interface_includes debug_lib INTERFACE_INCLUDE_DIRECTORIES)

if(NOT TARGET helpers_lib)
	message(STATUS "target: helpers_lib")
	add_subdirectory(../../tools/helpers helpers)
endif()
get_target_property(helpers_interface_includes helpers_lib INTERFACE_INCLUDE_DIRECTORIES)

if(NOT TARGET json11_lib)
	message(STATUS "target: json11_lib")
	add_subdirectory(../../json11 json11)
endif()
get_target_property(json11_interface_includes json11_lib INTERFACE_INCLUDE_DIRECTORIES)

if(NOT TARGET climate_common_lib)
	message(STATUS "target: climate_common_lib")
	add_subdirectory("../climate-common" climate_common)
endif()
get_target_property(climate_common_interface_includes climate_common_lib INTERFACE_INCLUDE_DIRECTORIES)

add_library(climate_file_io_lib 
	STATIC 
	../climate-file-io.h 
	../climate-file-io.cpp
)
target_link_libraries(climate_file_io_lib 
	PUBLIC 
	climate_common_lib 
	json11_lib
	helpers_lib
	debug_lib
)
target_include_directories(climate_file_io_lib 
	INTERFACE
	${debug_interface_includes}
	${helpers_interface_includes}
	${json11_interface_includes}
	${climate_common_interface_includes}
	${CMAKE_CURRENT_SOURCE_DIR}/..
)

if(MSVC AND MT_RUNTIME_LIB)
	target_compile_options(climate_file_io_lib PRIVATE "/MT$<$<CONFIG:Debug>:d>")
endif()

message(STATUS "<- MAS-infrastructure-climate-climate_file_io")