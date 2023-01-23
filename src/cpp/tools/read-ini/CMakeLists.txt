cmake_minimum_required(VERSION 3.12)
project(MAS-infrastructure-tools-read_ini)

message(STATUS "-> MAS-infrastructure-tools-read_ini")

if(NOT TARGET date_lib)
  message(STATUS "target: date_lib")
	add_subdirectory(../date date)
endif()
get_target_property(date_interface_includes date_lib INTERFACE_INCLUDE_DIRECTORIES)

if(NOT TARGET helpers_lib)
	message(STATUS "target: helpers_lib")
	add_subdirectory(../helpers helpers)
endif()
get_target_property(helpers_interface_includes helpers_lib INTERFACE_INCLUDE_DIRECTORIES)

add_library(read_ini_lib 
	STATIC 
	../read-ini.h 
	../read-ini.cpp
)
target_link_libraries(read_ini_lib 
	PUBLIC 
	date_lib 
	helpers_lib
)
target_include_directories(read_ini_lib 
	INTERFACE 
	${CMAKE_CURRENT_SOURCE_DIR}/..
	${date_interface_includes}
	${helpers_interface_includes}
)

if(MSVC AND MT_RUNTIME_LIB)
	target_compile_options(read_ini_lib PRIVATE "/MT$<$<CONFIG:Debug>:d>")
endif()

message(STATUS "<- MAS-infrastructure-tools-read_ini")