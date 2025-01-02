install(
    TARGETS LearnOpenGL_exe
    RUNTIME COMPONENT LearnOpenGL_Runtime
)

if(PROJECT_IS_TOP_LEVEL)
  include(CPack)
endif()
