cmake_minimum_required(VERSION 3.12)
project(MAS-infrastructure-db)

message(STATUS "-> MAS-infrastructure-db")

if(NOT TARGET sqlite3_lib)
	message(STATUS "target: sqlite3_lib")
	add_subdirectory("sqlite3")
endif()

if(NOT TARGET db_lib)
	message(STATUS "target: db_lib")
	add_subdirectory("db")
endif()

message(STATUS "<- MAS-infrastructure-db")
