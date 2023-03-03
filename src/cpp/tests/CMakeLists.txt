cmake_minimum_required(VERSION 3.12)
project(Mas-infrastructure-tests)

message(STATUS "-> Mas-infrastructure-tests")

set(MT_RUNTIME_LIB 1)

if(NOT TARGET capnp_schemas_lib)
    message(STATUS "target: capnp_schemas_lib")
    add_subdirectory(../../../capnproto_schemas capnproto_schemas)
endif()

if(NOT TARGET common_lib)
    message(STATUS "target: common_lib")
    add_subdirectory(../common common)
endif()

if(NOT TARGET helpers_lib)
    message(STATUS "target: helpers_lib")
    add_subdirectory(../tools/helpers tools/helpers)
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

set_absolute_path(COMMON_DIR "../common")

# create test_writer to write to channel
add_executable(test_writer 
    test_writer_main.cpp
)
target_link_libraries(test_writer
    PUBLIC
    ${CMAKE_THREAD_LIBS_INIT}
    ${CMAKE_DL_LIBS}
    capnp_schemas_lib
    helpers_lib
    common_lib
)
#target_include_directories(test_writer PRIVATE ${COMMON_DIR} ${CMAKE_CURRENT_BINARY_DIR})
if (MSVC)
    target_compile_options(test_writer PRIVATE "/MT$<$<CONFIG:Debug>:d>")
endif()

#------------------------------------------------------------------------------

# create test_reader to read from channel
add_executable(test_reader 
    test_reader_main.cpp
)
target_link_libraries(test_reader
    ${CMAKE_THREAD_LIBS_INIT}
    ${CMAKE_DL_LIBS}
    capnp_schemas_lib
    helpers_lib
    common_lib
)
#target_include_directories(test_reader PRIVATE ${COMMON_DIR} ${CMAKE_CURRENT_BINARY_DIR})
if (MSVC)
    target_compile_options(test_reader PRIVATE "/MT$<$<CONFIG:Debug>:d>")
endif()

#------------------------------------------------------------------------------

# create test_reader_srv directly serve interface 
add_executable(test_reader_srv 
    test_reader_srv_main.cpp
)
target_link_libraries(test_reader_srv
    ${CMAKE_THREAD_LIBS_INIT}
    ${CMAKE_DL_LIBS}
    capnp_schemas_lib
    helpers_lib
    common_lib
)
#target_include_directories(test_reader_srv PRIVATE ${COMMON_DIR} ${CMAKE_CURRENT_BINARY_DIR})
if (MSVC)
    target_compile_options(test_reader_srv PRIVATE "/MT$<$<CONFIG:Debug>:d>")
endif()

#------------------------------------------------------------------------------

# create test_writer_srv to directly connect to other process
add_executable(test_writer_srv 
    test_writer_srv_main.cpp
)
target_link_libraries(test_writer_srv
    ${CMAKE_THREAD_LIBS_INIT}
    ${CMAKE_DL_LIBS}
    capnp_schemas_lib
    helpers_lib
    common_lib
)
#target_include_directories(test_writer_srv PRIVATE ${COMMON_DIR} ${CMAKE_CURRENT_BINARY_DIR})
if (MSVC)
    target_compile_options(test_writer_srv PRIVATE "/MT$<$<CONFIG:Debug>:d>")
endif()

#------------------------------------------------------------------------------

# create test_reader_writer within single process or thread
add_executable(test_reader_writer 
    test_reader_writer_main.cpp
)
target_link_libraries(test_reader_writer
    ${CMAKE_THREAD_LIBS_INIT}
    ${CMAKE_DL_LIBS}
    capnp_schemas_lib
    helpers_lib
    common_lib
)
#target_include_directories(test_reader_writer PRIVATE ${COMMON_DIR} ${CMAKE_CURRENT_BINARY_DIR})
if (MSVC)
    target_compile_options(test_reader_writer PRIVATE "/MT$<$<CONFIG:Debug>:d>")
endif()

#------------------------------------------------------------------------------

# create test_storage
add_executable(test_storage 
    test_storage_main.cpp
)
target_link_libraries(test_storage
    ${CMAKE_THREAD_LIBS_INIT}
    ${CMAKE_DL_LIBS}
    capnp_schemas_lib
    helpers_lib
    common_lib
)
#target_include_directories(test_storage PRIVATE ${COMMON_DIR} ${CMAKE_CURRENT_BINARY_DIR})
if (MSVC)
    target_compile_options(test_storage PRIVATE "/MT$<$<CONFIG:Debug>:d>")
endif()

#------------------------------------------------------------------------------

# create test_storage
add_executable(test_services 
    test_services_main.cpp
)
target_link_libraries(test_services
    ${CMAKE_THREAD_LIBS_INIT}
    ${CMAKE_DL_LIBS}
    capnp_schemas_lib
    helpers_lib
    common_lib
)
#target_include_directories(test_services PRIVATE ${COMMON_DIR} ${CMAKE_CURRENT_BINARY_DIR})
if (MSVC)
    target_compile_options(test_services PRIVATE "/MT$<$<CONFIG:Debug>:d>")
endif()

#------------------------------------------------------------------------------

# create test_reader_srv directly serve interface 
add_executable(test_a 
    test_a_main.cpp
)
target_link_libraries(test_a
    ${CMAKE_THREAD_LIBS_INIT}
    ${CMAKE_DL_LIBS}
    capnp_schemas_lib
    helpers_lib
    common_lib
)
#target_include_directories(test_reader_srv PRIVATE ${COMMON_DIR} ${CMAKE_CURRENT_BINARY_DIR})
if (MSVC)
    target_compile_options(test_a PRIVATE "/MT$<$<CONFIG:Debug>:d>")
endif()

#------------------------------------------------------------------------------


message(STATUS "<- Mas-infrastructure-tests")