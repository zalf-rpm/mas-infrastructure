cmake_minimum_required(VERSION 3.12)
project(MAS-infrastructure-soil)

message(STATUS "-> MAS-infrastructure-soil")

if(NOT TARGET helpers_lib)
    message(STATUS "target: helpers_lib")
    add_subdirectory(../tools/helpers helpers)
endif()

if(NOT TARGET json11_lib)
    message(STATUS "target: json11_lib")
    add_subdirectory(../json11 json11)
endif()

if(NOT TARGET capnp_schemas_lib)
    message(STATUS "target: capnp_schemas_lib")
    add_subdirectory(../../../capnproto_schemas capnproto_schemas)
endif()

add_library(soil_lib 
    STATIC 
    constants.h 
    constants.cpp 
    conversion.h 
    conversion.cpp 
    soil.h 
    soil.cpp
)
target_link_libraries(soil_lib 
    PUBLIC
    json11_lib
    capnp_schemas_lib
    helpers_lib 
)
target_include_directories(soil_lib 
    PUBLIC 
    ${CMAKE_CURRENT_SOURCE_DIR}    # for #include "soil.h"
    ${CMAKE_CURRENT_SOURCE_DIR}/.. # for #include "soil/soil.h"
)

if(MSVC AND MT_RUNTIME_LIB)
    target_compile_options(soil_lib PRIVATE "/MT$<$<CONFIG:Debug>:d>")
endif()

message(STATUS "<- MAS-infrastructure-soil")