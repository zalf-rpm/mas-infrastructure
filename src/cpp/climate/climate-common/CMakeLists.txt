cmake_minimum_required(VERSION 3.12)
project(MAS-infrastructure-climate-climate_common)

message(STATUS "-> MAS-infrastructure-climate-climate_common")

if(NOT TARGET helpers_lib)
	message(STATUS "target: helpers_lib")
	add_subdirectory(../../tools/helpers helpers)
endif()

if(NOT TARGET date_lib)
  message(STATUS "target: date_lib")
	add_subdirectory(../../tools/date date)
endif()

if(NOT TARGET json11_lib)
	message(STATUS "target: json11_lib")
	add_subdirectory(../../json11 json11)
endif()

add_library(climate_common_lib 
	STATIC 
	../climate-common.h 
	../climate-common.cpp
)

target_link_libraries(climate_common_lib 
	PUBLIC 
	date_lib
	helpers_lib
	json11_lib
)

target_include_directories(climate_common_lib 
	PUBLIC 
	${CMAKE_CURRENT_SOURCE_DIR}/..    # for #include "climate-common.h"
	${CMAKE_CURRENT_SOURCE_DIR}/../.. # for #include "climate/climate-common.h"
)
if(MSVC AND MT_RUNTIME_LIB)
	target_compile_options(climate_common_lib PRIVATE "/MT$<$<CONFIG:Debug>:d>")
endif()

message(STATUS "<- MAS-infrastructure-climate-climate_common")
