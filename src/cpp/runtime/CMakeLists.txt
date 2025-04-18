cmake_minimum_required(VERSION 3.12)
project(climate_data_server)

add_compile_definitions(NO_MYSQL)
set(MT_RUNTIME_LIB 1)

add_subdirectory(../util/tools/date util/date)
add_subdirectory(../util/tools/helpers util/helpers)
#add_subdirectory(../util/tools/read-ini util/read-ini)
add_subdirectory(../util/tools/debug util/debug)
#add_subdirectory(../util/climate/climate-file-io util/climate-file-io)
#add_subdirectory(../util/soil util/soil)
#add_subdirectory(../util/db/db util/db)
#add_subdirectory(../util/zeromq util/zeromq)

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
		elseif(MSVC_VERSION GREATER_EQUAL 1910)
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

include_directories(
"src"
C:/Users/berg.ZALF-AD/libRed)

message(STATUS "cmake_generator = ${CMAKE_GENERATOR}")
message(STATUS "size_of_void =  ${CMAKE_SIZEOF_VOID_P}")
if(${CMAKE_SIZEOF_VOID_P} STREQUAL "4")
    set(ARCH "x86")
elseif(${CMAKE_SIZEOF_VOID_P} STREQUAL "8")
    set(ARCH "x64")
endif()

link_directories(
$ENV{HOME}/lib
C:/Users/berg.ZALF-AD/libRed
)

find_package(Threads REQUIRED)

#define folder structure in vs solution corresponding to real folder structure
GroupSources("src" ${PROJECT_SOURCE_DIR})
GroupSources("util" "${PROJECT_SOURCE_DIR}/..")

#------------------------------------------------------------------------------

set_absolute_path(CAPNP_SCHEMAS_DIR "../capnproto_schemas")

find_package(CapnProto CONFIG REQUIRED)

set(CAPNPC_SRC_PREFIX ${CAPNP_SCHEMAS_DIR})
capnp_generate_cpp(CAPNP_SRCS CAPNP_HDRS 
${CAPNP_SCHEMAS_DIR}/common.capnp 
${CAPNP_SCHEMAS_DIR}/date.capnp
${CAPNP_SCHEMAS_DIR}/climate.capnp
${CAPNP_SCHEMAS_DIR}/model.capnp
${CAPNP_SCHEMAS_DIR}/geo.capnp
${CAPNP_SCHEMAS_DIR}/cluster_admin_service.capnp)
message(STATUS "CAPNP_HDRS: " "${CAPNP_HDRS}")
message(STATUS "CAPNP_SRCS: " "${CAPNP_SRCS}")

# create monica-capnp-server executable, the server running monica via CapNProto RPC
set(SOURCE_FILES
	src/main.cpp
	src/common.h
	src/common.cpp
	src/cluster_monica_instance_factory.h
	src/cluster_monica_instance_factory.cpp
)
set(SOURCE 
	${SOURCE_FILES}
	${CAPNP_HDRS}
	${CAPNP_SRCS}
)
add_executable(climate_data_server ${SOURCE})
target_link_libraries(climate_data_server
	${CMAKE_THREAD_LIBS_INIT}
	${CMAKE_DL_LIBS}
	CapnProto::capnp
	CapnProto::capnpc
	CapnProto::capnp-rpc
	CapnProto::kj
	CapnProto::kj-async
	debug_lib
	helpers_lib
	date_lib
	#monica_lib
)
target_include_directories(climate_data_server PRIVATE ${CMAKE_CURRENT_BINARY_DIR})
if (MSVC)
	target_compile_options(climate_data_server PRIVATE "/MT$<$<CONFIG:Debug>:d>")
endif()

#------------------------------------------------------------------------------

add_executable(climate_data_server_test
    src/test_main.cpp
    ${CAPNP_HDRS}
	${CAPNP_SRCS})
target_link_libraries(climate_data_server_test
	${CMAKE_THREAD_LIBS_INIT}
	${CMAKE_DL_LIBS}
	CapnProto::capnp
	CapnProto::capnpc
	CapnProto::capnp-rpc
	CapnProto::kj
	CapnProto::kj-async
	libRed
	#debug_lib
	#helpers_lib
	#date_lib
	#monica_lib
)
target_include_directories(climate_data_server_test PRIVATE ${CMAKE_CURRENT_BINARY_DIR})
if (MSVC)
	target_compile_options(climate_data_server_test PRIVATE "/MT$<$<CONFIG:Debug>:d>")
endif()