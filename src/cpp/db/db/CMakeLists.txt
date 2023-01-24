cmake_minimum_required(VERSION 3.12)
project(MAS-infrastructure-db-db)

message(STATUS "-> MAS-infrastructure-db-db")

if(NOT TARGET date_lib)
  message(STATUS "target: date_lib")
	add_subdirectory(../../tools/date date)
endif()

if(NOT TARGET read_ini_lib)
	message(STATUS "target: read_ini_lib")
	add_subdirectory(../../tools/read-ini read_ini)
endif()

if(NOT TARGET sqlite3_lib)
	message(STATUS "target: sqlite3_lib")
	add_subdirectory(../sqlite3 sqlite3)
endif()

add_library(db_lib 
	STATIC 
	../db.h 
	../db.cpp 
	../abstract-db-connections.h 
	../abstract-db-connections.cpp
)

target_link_libraries(db_lib 
	PUBLIC 
	sqlite3_lib 
	date_lib 
	read_ini_lib
)

target_include_directories(db_lib 
	PUBLIC 
	${CMAKE_CURRENT_SOURCE_DIR}/..    # for db.h
	${CMAKE_CURRENT_SOURCE_DIR}/../.. # for db/db.h 
)

if(MSVC AND MT_RUNTIME_LIB)
	target_compile_options(db_lib PRIVATE "/MT$<$<CONFIG:Debug>:d>")
endif()

message(STATUS "<- MAS-infrastructure-db-db")
