cmake_minimum_required(VERSION 3.12)
project(MAS-infrastructure-tools-date)

message(STATUS "-> MAS-infrastructure-tools-date")

#add_compile_definitions(CAPNPROTO_SERIALIZATION_SUPPORT)
#set(CAPNPROTO_SERIALIZATION_SUPPORT 1)

if(CAPNPROTO_SERIALIZATION_SUPPORT AND NOT TARGET capnp_schemas_lib)
    message(STATUS "target: capnp_schemas_lib")
    add_subdirectory(../../../../capnproto_schemas capnproto_schemas)
endif()

add_library(date_lib 
    STATIC 
    ../date.h 
    ../date.cpp
)
if(CAPNPROTO_SERIALIZATION_SUPPORT)
    target_link_libraries(date_lib PUBLIC capnp_schemas_lib)
endif()

target_include_directories(date_lib 
    PUBLIC 
    ${CMAKE_CURRENT_SOURCE_DIR}/.. # refer to date.h via date.h
    ${CMAKE_CURRENT_SOURCE_DIR}/../.. # be able to use also tools/date.h
)

if(MSVC AND MT_RUNTIME_LIB)
    target_compile_options(date_lib PRIVATE "/MT$<$<CONFIG:Debug>:d>")
endif()

message(STATUS "<- MAS-infrastructure-tools-date")