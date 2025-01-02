cmake_minimum_required(VERSION 3.14)

include(cmake/prelude.cmake)

project(
    LearnOpenGL
    VERSION 0.1.0
    DESCRIPTION "Short description"
    HOMEPAGE_URL "https://example.com/"
    LANGUAGES C CXX
)

include(cmake/project-is-top-level.cmake)
include(cmake/variables.cmake)

# ---- Declare library ----

add_subdirectory(Framework)

# ---- Declare executable ----

add_executable(LearnOpenGL_exe Chapters/main.cpp)
add_executable(LearnOpenGL::exe ALIAS LearnOpenGL_exe)

# set_property(TARGET LearnOpenGL_exe PROPERTY OUTPUT_NAME LearnOpenGL)

target_compile_features(LearnOpenGL_exe PRIVATE cxx_std_17)

target_link_libraries(LearnOpenGL_exe PRIVATE LearnOpenGL_Framework)

# ---- Install rules ----

if(NOT CMAKE_SKIP_INSTALL_RULES)
  include(cmake/install-rules.cmake)
endif()

# ---- Developer mode ----

if(NOT LearnOpenGL_DEVELOPER_MODE)
  return()
elseif(NOT PROJECT_IS_TOP_LEVEL)
  message(
      AUTHOR_WARNING
      "Developer mode is intended for developers of LearnOpenGL"
  )
endif()

include(cmake/dev-mode.cmake)
