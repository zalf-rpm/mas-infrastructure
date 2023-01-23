cmake_minimum_required(VERSION 3.12)
project(MAS-infrastructure-zeromq)

message(STATUS "-> MAS-infrastructure-zeromq")

if(NOT TARGET date_lib)
  message(STATUS "target: date_lib")
	add_subdirectory(../tools/date date)
endif()
get_target_property(date_interface_includes date_lib INTERFACE_INCLUDE_DIRECTORIES)

if(NOT TARGET json11_lib)
	message(STATUS "target: json11_lib")
	add_subdirectory(../json11 json11)
endif()
get_target_property(json11_interface_includes json11_lib INTERFACE_INCLUDE_DIRECTORIES)

find_package(ZeroMQ CONFIG REQUIRED)
#if(UNIX)
#	set(DL_LIB_NAME dl)
#endif()
add_library(zeromq_lib 
	STATIC 
	zmq-helper.h 
	zmq-helper.cpp 
	zmq.hpp 
	zhelpers.hpp
)
target_link_libraries(zeromq_lib 
	PUBLIC 
	libzmq-static 
	json11_lib 
	date_lib
)
target_include_directories(zeromq_lib 
	INTERFACE 
	${CMAKE_CURRENT_SOURCE_DIR}
	${date_interface_includes}
	${json11_interface_includes}
)

if(MSVC AND MT_RUNTIME_LIB)
	target_compile_options(zeromq_lib PRIVATE "/MT$<$<CONFIG:Debug>:d>")
endif()

message(STATUS "<- MAS-infrastructure-zeromq")