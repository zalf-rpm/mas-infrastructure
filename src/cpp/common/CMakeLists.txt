cmake_minimum_required(VERSION 3.12)
project(Mas-infrastructure-common)

message(STATUS "-> Mas-infrastructure-common")

# https://cmake.org/cmake/help/latest/prop_tgt/CXX_STANDARD.html
set(CMAKE_CXX_STANDARD 17)
set(CMAKE_CXX_STANDARD_REQUIRED ON)
set(CMAKE_CXX_EXTENSIONS OFF)

set(MT_RUNTIME_LIB 1)
add_compile_definitions(NO_MYSQL)
add_compile_definitions(SODIUM_STATIC)

if(NOT TARGET capnp_schemas_lib)
	message(STATUS "target: capnp_schemas_lib")
	add_subdirectory(../../../capnproto_schemas capnproto_schemas)
endif()

if(NOT TARGET helpers_lib)
	message(STATUS "target: helpers_lib")
	add_subdirectory(../tools/helpers tools/helpers)
endif()

if(NOT TARGET json11_lib)
	message(STATUS "target: json11_lib")
	add_subdirectory(../json11 json11)
endif()

if(NOT TARGET sqlite3_lib)
	message(STATUS "target: sqlite3_lib")
	add_subdirectory(../db/sqlite3 db/sqlite3)
endif()

#set absolute filenames (to resolve .. in paths)
macro(set_absolute_path var_name path)
	get_filename_component(toAbsPath ${path} ABSOLUTE)
	set(${var_name} ${toAbsPath})
endmacro(set_absolute_path)

#define delete file from path
macro(remove_file list_name file_name)
	get_filename_component(toAbsPath ${file_name} ABSOLUTE)
	list(REMOVE_ITEM ${list_name} "${toAbsPath}")
endmacro(remove_file)

#define source_groups 
macro(GroupSources curdir baseDir)
   file(GLOB children RELATIVE ${baseDir}/${curdir} ${baseDir}/${curdir}/*)
   foreach(child ${children})
		  if(IS_DIRECTORY ${baseDir}/${curdir}/${child})
				  GroupSources(${curdir}/${child} ${baseDir})
		  else()
				string(REPLACE "/" "\\" groupname ${curdir})
			source_group(${groupname} FILES ${baseDir}/${curdir}/${child})
		  endif()
   endforeach()
endmacro()

if(MSVC)
	# disable security warnings in windows
	add_definitions(-D_CRT_SECURE_NO_WARNINGS)
	
	# define MS Visual Studio toolset version
	if(NOT DEFINED MSVC_TOOLSET_VERSION)
		if(MSVC_VERSION GREATER_EQUAL 1930)
		  set(MSVC_TOOLSET_VERSION "") # leave unknown for future versions
		elseif(MSVC_VERSION GREATER_EQUAL 1920)
		  set(MSVC_TOOLSET_VERSION 142) # VS 2019
		elseif(MSVC_VERSION GREATER_Echannel_mainQUAL 1910)
		  set(MSVC_TOOLSET_VERSION 141) # VS 2017
		elseif(MSVC_VERSION EQUAL 1900)
		  set(MSVC_TOOLSET_VERSION 14) # VS 2015
		else()
		 # we do not support older version
		endif()
	endif()
endif()

#set(CMAKE_CXX_STANDARD 14)

# setup the target group variable, used to group targets into folders in IDEs
set_property(GLOBAL PROPERTY USE_FOLDERS ON)

message(STATUS "cmake_generator = ${CMAKE_GENERATOR}")
message(STATUS "size_of_void =  ${CMAKE_SIZEOF_VOID_P}")
if(${CMAKE_SIZEOF_VOID_P} STREQUAL "4")
    set(ARCH "x86")
elseif(${CMAKE_SIZEOF_VOID_P} STREQUAL "8")
    set(ARCH "x64")
endif()

find_package(Threads REQUIRED)

#define folder structure in vs solution corresponding to real folder structure
GroupSources("src" ${PROJECT_SOURCE_DIR})
GroupSources("util" "${PROJECT_SOURCE_DIR}/..")

#------------------------------------------------------------------------------

find_package(unofficial-sodium CONFIG REQUIRED)

# create common_lib
add_library(common_lib
	channel.h
	channel.cpp
	common.h
	common.cpp
	restorer.h
	restorer.cpp
	restorable-service-main.h
	restorable-service-main.cpp
	rpc-connection-manager.h
	rpc-connection-manager.cpp
	dll-exports.h
	sole.hpp
)
target_link_libraries(common_lib
	PUBLIC
	${CMAKE_THREAD_LIBS_INIT}
	${CMAKE_DL_LIBS}
	capnp_schemas_lib
	helpers_lib
	json11_lib
	unofficial-sodium::sodium
)
target_include_directories(common_lib 
	PUBLIC
	${CMAKE_CURRENT_SOURCE_DIR}     # for #include "common.h"
	${CMAKE_CURRENT_SOURCE_DIR}/..  # for #include "common/common.h"
)
if (MSVC)
	target_compile_options(common_lib PRIVATE "/MT$<$<CONFIG:Debug>:d>")
endif()

#------------------------------------------------------------------------------

# create channel
add_executable(channel 
	channel-main.cpp
)
target_link_libraries(channel common_lib)
#target_include_directories(channel PRIVATE ${CMAKE_CURRENT_BINARY_DIR})
if (MSVC)
	target_compile_options(channel PRIVATE "/MT$<$<CONFIG:Debug>:d>")
endif()

#------------------------------------------------------------------------------

# create storage service
add_executable(storage-service 
	storage-service.h
	storage-service.cpp
	storage-service-main.cpp
)
target_link_libraries(storage-service
	sqlite3_lib
	common_lib
)
#target_include_directories(storage-service PRIVATE ${CMAKE_CURRENT_BINARY_DIR})
if (MSVC)
	target_compile_options(storage-service PRIVATE "/MT$<$<CONFIG:Debug>:d>")
endif()

#------------------------------------------------------------------------------

# create storage service
add_executable(host-port-resolver-service
		host-port-resolver.h
		host-port-resolver.cpp
		host-port-resolver-main.cpp
		)
target_link_libraries(host-port-resolver-service
		common_lib
		)
#target_include_directories(storage-service PRIVATE ${CMAKE_CURRENT_BINARY_DIR})
if (MSVC)
	target_compile_options(host-port-resolver-service PRIVATE "/MT$<$<CONFIG:Debug>:d>")
endif()

#------------------------------------------------------------------------------


message(STATUS "<- Mas-infrastructure-common")