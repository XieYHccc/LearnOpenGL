set(TARGET_NAME LearnOpenGL_Framework)

file(GLOB_RECURSE HEADER_FILES "*.h")
file(GLOB_RECURSE SOURCE_FILES "*.cpp" "*.c")

set(THIRD_PARTY_DIR ${CMAKE_CURRENT_SOURCE_DIR}/Vendor)
source_group(TREE "${CMAKE_CURRENT_SOURCE_DIR}" FILES ${HEADER_FILES} ${SOURCE_FILES})


#---- create library ------
add_library(${TARGET_NAME} ${HEADER_FILES} ${SOURCE_FILES})

target_compile_features(${TARGET_NAME} PRIVATE cxx_std_17)
target_compile_features(${TARGET_NAME} PRIVATE c_std_17)
set_target_properties(${TARGET_NAME} PROPERTIES FOLDER "Core")

# link dependencies
find_package(fmt REQUIRED)
find_package(glm REQUIRED)
find_package(glfw3 REQUIRED)
find_package(assimp REQUIRED)
target_link_libraries(${TARGET_NAME} PUBLIC fmt::fmt glfw glm::glm assimp::assimp)

target_include_directories(
  ${TARGET_NAME} 
  PUBLIC $<BUILD_INTERFACE:${PROJECT_SOURCE_DIR}>
  $<INSTALL_INTERFACE:include/${TARGET_NAME}-${PROJECT_VERSION}>
)

target_include_directories(
  ${TARGET_NAME} 
  PUBLIC $<BUILD_INTERFACE:${THIRD_PARTY_DIR}>
  $<INSTALL_INTERFACE:include/${TARGET_NAME}-${PROJECT_VERSION}>
)