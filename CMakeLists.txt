cmake_minimum_required(VERSION 3.26)

project(ServerEase VERSION 0.1.0
                   DESCRIPTION "ServerEase Launcher is a third-party cross-platform Minecraft Launcher designed for Minecraft Servers that require a proprietary client."
                   LANGUAGES CXX)

configure_file(./include/property.h.in ${CMAKE_BINARY_DIR}/include/property.h)
file(GLOB_RECURSE SRC ./src/*.cpp ./include/*.h ${PROJECT_BINARY_DIR}/include/*.h)

add_library(${PROJECT_NAME} SHARED ${SRC})
target_include_directories(${PROJECT_NAME} PUBLIC ./include ${PROJECT_BINARY_DIR}/include)
set_target_properties(${PROJECT_NAME} PROPERTIES
        RUNTIME_OUTPUT_DIRECTORY ${CMAKE_BINARY_DIR}/bin
        LIBRARY_OUTPUT_DIRECTORY ${CMAKE_BINARY_DIR}/bin)

add_subdirectory(./test)

if (CMAKE_SYSTEM_NAME MATCHES "Windows")
    MESSAGE("Current Compiling System is Microsoft Windows.")
    add_compile_definitions(WINDOWS)
elseif (CMAKE_SYSTEM_NAME MATCHES "Linux")
    MESSAGE("Current Compiling System is GNU/Linux.")
    add_compile_definitions(WINDOWS)
elseif (CMAKE_SYSTEM_NAME MATCHES "Darwin")
    MESSAGE("Current Compiling System is Apple macOS.")
    add_compile_definitions(WINDOWS)
endif ()
