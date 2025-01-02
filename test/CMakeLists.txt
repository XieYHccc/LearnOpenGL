# Parent project does not export its library target, so this CML implicitly
# depends on being added from it, i.e. the testing is done only from the build
# tree and is not feasible from an install location

project(LearnOpenGLTests LANGUAGES CXX)

# ---- Dependencies ----

find_package(Catch2 REQUIRED)
include(Catch)

# ---- Tests ----

add_executable(LearnOpenGL_test source/LearnOpenGL_test.cpp)
target_link_libraries(
    LearnOpenGL_test PRIVATE
    LearnOpenGL_Framework
    Catch2::Catch2WithMain
)
target_compile_features(LearnOpenGL_test PRIVATE cxx_std_17)

catch_discover_tests(LearnOpenGL_test)

# ---- End-of-file commands ----

add_folders(Test)
