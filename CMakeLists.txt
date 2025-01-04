cmake_minimum_required(VERSION 3.14)

include(cmake/prelude.cmake)

project(
    LearnOpenGL
    VERSION 0.1.0
    DESCRIPTION "Learning project for OpenGL"
    HOMEPAGE_URL "https://github.com/XieYHccc/LearnOpenGL.git"
    LANGUAGES C CXX
)

include(cmake/project-is-top-level.cmake)
include(cmake/variables.cmake)

# ---- framwork for all chapters ----

add_subdirectory(Framework)

# ---- chapters ----
add_subdirectory(Chapters)

# ---- Install rules ----

# if(NOT CMAKE_SKIP_INSTALL_RULES)
#   include(cmake/install-rules.cmake)
# endif()

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
