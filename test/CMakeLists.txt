add_executable(${PROJECT_NAME}_Test ./src/main.cpp)
target_include_directories(${PROJECT_NAME}_Test PUBLIC ../include ${CMAKE_BINARY_DIR}/include)
target_link_libraries(${PROJECT_NAME}_Test PUBLIC ${PROJECT_NAME})
set_target_properties(${PROJECT_NAME}_Test PROPERTIES
        RUNTIME_OUTPUT_DIRECTORY ${CMAKE_BINARY_DIR}/bin
        LIBRARY_OUTPUT_DIRECTORY ${CMAKE_BINARY_DIR}/bin)
