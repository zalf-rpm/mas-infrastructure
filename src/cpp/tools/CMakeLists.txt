cmake_minimum_required(VERSION 3.12)
project(MAS-infrastructure-tools)

message(STATUS "-> MAS-infrastructure-tools")

if(NOT TARGET date_lib)
	message(STATUS "target: date_lib")
	add_subdirectory("date")
endif()

if(NOT TARGET coord_trans_lib)
	message(STATUS "target: coord_trans_lib")
	add_subdirectory("coord-trans")
endif()

if(NOT TARGET debug_lib)
	message(STATUS "target: debug_lib")
	add_subdirectory("debug")
endif()

if(NOT TARGET helpers_lib)
	message(STATUS "target: helpers_lib")
	add_subdirectory("helpers")
endif()

if(NOT TARGET read_ini_lib)
	message(STATUS "target: read_ini_lib")
	add_subdirectory("read-ini")
endif()

message(STATUS "<- MAS-infrastructure-tools")
