cmake_minimum_required(VERSION 3.12)
project(MAS-infrastructure-climate)

message(STATUS "-> MAS-infrastructure-climate")

if(NOT TARGET climate_common_lib)
	message(STATUS "target: climate_common_lib")
	add_subdirectory("climate-common")
endif()

if(NOT TARGET climate_lib)
	message(STATUS "target: climate_lib")
	add_subdirectory("climate")
endif()

if(NOT TARGET climate_file_io_lib)
	message(STATUS "target: climate_file_io_lib")
	add_subdirectory("climate-file-io")
endif()

message(STATUS "<- MAS-infrastructure-climate")
