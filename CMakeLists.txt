cmake_minimum_required(VERSION 3.27.8)
set(CMAKE_EXPORT_COMPILE_COMMANDS true)

set(CMAKE_SYSTEM_NAME Windows)
set(TOOLCHAIN_PREFIX x86_64-w64-mingw32)
set(CMAKE_C_COMPILER ${TOOLCHAIN_PREFIX}-gcc)
set(CMAKE_CXX_COMPILER ${TOOLCHAIN_PREFIX}-g++)

project(ReMOSD)

set(CMAKE_CXX_STANDARD 20)

set(PROJECT_SOURCE_DIR src)
file(GLOB_RECURSE SOURCES ${PROJECT_SOURCE_DIR}/*.cpp)

add_compile_options(
  -Wall -Wextra -Werror -Wfatal-errors
  -Wno-newline-eof -Wno-sign-compare
  -Wno-error=unused-parameter -Wno-error=unused-variable -Wno-error=unused-but-set-variable -Wno-error=unused-value
  -pedantic -pedantic-errors
)
add_link_options(
  -static
)

if(CMAKE_BUILD_TYPE STREQUAL "Release")
  add_link_options(-s)
endif()
# add_link_options($<$<CONFIG:RELEASE>:-s>)

add_executable(${PROJECT_NAME} ${SOURCES})