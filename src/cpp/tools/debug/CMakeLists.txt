cmake_minimum_required(VERSION 3.12)
project(MAS-infrastructure-tools-debug)

message(STATUS "-> MAS-infrastructure-tools-debug")

add_library(debug_lib 
    STATIC 
    ../debug.h 
    ../debug.cpp
)

target_include_directories(debug_lib 
    INTERFACE
    ${CMAKE_CURRENT_SOURCE_DIR}/..      # for debug.h
    ${CMAKE_CURRENT_SOURCE_DIR}/../..   # for tools/debug.h
)

if(MSVC AND MT_RUNTIME_LIB)
    target_compile_options(debug_lib PRIVATE "/MT$<$<CONFIG:Debug>:d>")
endif()

message(STATUS "<- MAS-infrastructure-tools-debug")
