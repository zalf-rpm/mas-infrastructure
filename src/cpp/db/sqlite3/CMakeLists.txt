cmake_minimum_required(VERSION 3.12)
project(MAS-infrastructure-db-sqlite3)

message(STATUS "-> MAS-infrastructure-db-sqlite3")

add_library(sqlite3_lib 
	STATIC 
	sqlite3.h 
	sqlite3.c
)

if(UNIX)
	target_link_libraries(sqlite3_lib PUBLIC ${CMAKE_DL_LIBS})
endif()

target_include_directories(sqlite3_lib 
	PUBLIC
	${CMAKE_CURRENT_SOURCE_DIR}     # for #include sqlite3.h
	${CMAKE_CURRENT_SOURCE_DIR}/..  # for #include sqlite3/sqlite3.h
)

if(MSVC AND MT_RUNTIME_LIB)
	target_compile_options(sqlite3_lib PRIVATE "/MT$<$<CONFIG:Debug>:d>")
endif()

message(STATUS "<- MAS-infrastructure-db-sqlite3")