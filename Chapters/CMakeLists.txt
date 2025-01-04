macro(makeLink src dest target)
  add_custom_command(TARGET ${target} POST_BUILD COMMAND ${CMAKE_COMMAND} -E create_symlink ${src} ${dest}  DEPENDS  ${dest} COMMENT "mklink ${src} -> ${dest}")
endmacro()

function(create_project_from_sources chapter subchapter)
	file(GLOB SOURCE
            "${chapter}/${subchapter}/*.h"
            "${chapter}/${subchapter}/*.cpp"
            "${chapter}/${subchapter}/*.vs"
            "${chapter}/${subchapter}/*.fs"
            "${chapter}/${subchapter}/*.tcs"
            "${chapter}/${subchapter}/*.tes"
            "${chapter}/${subchapter}/*.gs"
            "${chapter}/${subchapter}/*.cs"
    )

	set(TARGET_NAME "${chapter}__${subchapter}")
    add_executable(${TARGET_NAME} ${SOURCE})
    target_link_libraries(${TARGET_NAME} LearnOpenGL_Framework)

    target_compile_features(${TARGET_NAME} PRIVATE cxx_std_17)
    set_target_properties(${TARGET_NAME} PROPERTIES FOLDER "${chapter}")
    
    if(MSVC)
		target_compile_options(${TARGET_NAME} PRIVATE /std:c++17 /MP)
        target_link_options(${TARGET_NAME} PUBLIC /ignore:4099)
    endif(MSVC)
    if(WIN32)
        set_target_properties(${TARGET_NAME} PROPERTIES RUNTIME_OUTPUT_DIRECTORY "${CMAKE_SOURCE_DIR}/bin/${chapter}/${subchapter}")
        set_target_properties(${TARGET_NAME} PROPERTIES VS_DEBUGGER_WORKING_DIRECTORY "${CMAKE_SOURCE_DIR}/bin/${chapter}/${subchapter}/Debug")
    elseif(UNIX AND NOT APPLE)
        set_target_properties(${TARGET_NAME} PROPERTIES RUNTIME_OUTPUT_DIRECTORY "${CMAKE_SOURCE_DIR}/bin/${chapter}/${subchapter}")
    elseif(APPLE)
        set_target_properties(${TARGET_NAME} PROPERTIES RUNTIME_OUTPUT_DIRECTORY "${CMAKE_SOURCE_DIR}/bin/${chapter}/${subchapter}")
        set_target_properties(${TARGET_NAME} PROPERTIES RUNTIME_OUTPUT_DIRECTORY_DEBUG "${CMAKE_SOURCE_DIR}/bin/${chapter}/${subchapter}")
        set_target_properties(${TARGET_NAME} PROPERTIES RUNTIME_OUTPUT_DIRECTORY_RELEASE "${CMAKE_SOURCE_DIR}/bin/${chapter}/${subchapter}")
    endif(WIN32)

    # copy shader files to build directory
    file(GLOB SHADERS
             "${chapter}/${subchapter}/*.vs"
             # "${chapter}/${subchapter}/*.frag"
             "${chapter}/${subchapter}/*.fs"
             "${chapter}/${subchapter}/*.tcs"
             "${chapter}/${subchapter}/*.tes"
             "${chapter}/${subchapter}/*.gs"
             "${chapter}/${subchapter}/*.cs"
    )

	# copy dlls
	#file(GLOB DLLS "dlls/*.dll")
    foreach(SHADER ${SHADERS})
        if(WIN32)
            # configure_file(${SHADER} "test")
            add_custom_command(TARGET ${TARGET_NAME} PRE_BUILD COMMAND ${CMAKE_COMMAND} -E copy ${SHADER} $<TARGET_FILE_DIR:${TARGET_NAME}>)
			#add_custom_command(TARGET ${TARGET_NAME} PRE_BUILD COMMAND ${CMAKE_COMMAND} -E copy ${DLLS} $<TARGET_FILE_DIR:${TARGET_NAME}>)
        elseif(UNIX AND NOT APPLE)
            file(COPY ${SHADER} DESTINATION ${CMAKE_SOURCE_DIR}/bin/${chapter})
        elseif(APPLE)
            # create symbolic link for *.vs *.fs *.gs
            get_filename_component(SHADERNAME ${SHADER} TARGET_NAME)
            makeLink(${SHADER} ${CMAKE_SOURCE_DIR}/bin/${chapter}/${SHADERNAME} ${TARGET_NAME})
        endif(WIN32)
    endforeach(SHADER)
    # if compiling for visual studio, also use configure file for each project (specifically to set up working directory)
    # if(MSVC)
    #     configure_file(${CMAKE_SOURCE_DIR}/configuration/visualstudio.vcxproj.user.in ${CMAKE_CURRENT_BINARY_DIR}/${TARGET_NAME}.vcxproj.user @ONLY)
    # endif(MSVC)
endfunction()

file(GLOB CHAPTER_PATHS ${CMAKE_CURRENT_SOURCE_DIR}/*)
foreach(CHAPTER_PATH ${CHAPTER_PATHS})
    # get chapter name
    string(REGEX MATCH "([^/]*)$" TMP ${CHAPTER_PATH})
    set(CHAPTER ${TMP})
    # get subchapter names
    file(GLOB SUB_CHAPTERS RELATIVE ${CHAPTER_PATH} ${CHAPTER_PATH}/*)
    foreach(SUB_CHAPTER ${SUB_CHAPTERS})
        # message(WARNING "${CHAPTER}: ${SUB_CHAPTER}")
        create_project_from_sources(${CHAPTER} ${SUB_CHAPTER})
    endforeach()
endforeach()



# add_executable(LearnOpenGL_exe main.cpp)
# add_executable(LearnOpenGL::exe ALIAS LearnOpenGL_exe)

# # set_property(TARGET LearnOpenGL_exe PROPERTY OUTPUT_NAME LearnOpenGL)

# target_compile_features(LearnOpenGL_exe PRIVATE cxx_std_17)

# target_link_libraries(LearnOpenGL_exe PRIVATE LearnOpenGL_Framework)


