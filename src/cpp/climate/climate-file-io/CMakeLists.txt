cmake_minimum_required(VERSION 3.12)
project(MAS-infrastructure-climate-climate_file_io)

message(STATUS "-> MAS-infrastructure-climate-climate_file_io")

# https://cmake.org/cmake/help/latest/prop_tgt/CXX_STANDARD.html
set(CMAKE_CXX_STANDARD 17)
set(CMAKE_CXX_STANDARD_REQUIRED ON)
set(CMAKE_CXX_EXTENSIONS OFF)

find_package(CapnProto CONFIG REQUIRED)
find_package(ZLIB)

if (NOT TARGET debug_lib)
    message(STATUS "target: debug_lib")
    add_subdirectory(../../tools/debug debug)
endif ()

if (NOT TARGET helpers_lib)
    message(STATUS "target: helpers_lib")
    add_subdirectory(../../tools/helpers helpers)
endif ()

if (NOT TARGET json11_lib)
    message(STATUS "target: json11_lib")
    add_subdirectory(../../json11 json11)
endif ()

if (NOT TARGET climate_common_lib)
    message(STATUS "target: climate_common_lib")
    add_subdirectory(../climate-common climate_common)
endif ()

if (NOT TARGET common_lib)
    message(STATUS "target: common_lib")
    add_subdirectory(../common common)
endif ()

add_library(climate_file_io_lib
        STATIC
        ../climate-file-io.h
        ../climate-file-io.cpp
        )

target_link_libraries(climate_file_io_lib
        PUBLIC
        CapnProto::kj
        CapnProto::kj-gzip
        climate_common_lib
        json11_lib
        helpers_lib
        debug_lib
        common_lib
        )

target_include_directories(climate_file_io_lib
        PUBLIC
        ${CMAKE_CURRENT_SOURCE_DIR}/..     # for #include "climate-file-io.h"
        ${CMAKE_CURRENT_SOURCE_DIR}/../..  # for #include "climate/climate-file-io.h"
        )

if (MSVC AND MT_RUNTIME_LIB)
    target_compile_options(climate_file_io_lib PRIVATE "/MT$<$<CONFIG:Debug>:d>")
endif ()

message(STATUS "<- MAS-infrastructure-climate-climate_file_io")