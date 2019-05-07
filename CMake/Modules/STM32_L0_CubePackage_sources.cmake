#
# Copyright (c) 2019 The nanoFramework project contributors
# See LICENSE file in the project root for full license information.
#

# set include directories
list(APPEND STM32_CubePackage_INCLUDE_DIRS "${PROJECT_BINARY_DIR}/STM32_CubePackage_Source/Drivers/STM32L0xx_HAL_Driver/Inc")

# source files
set(STM32_CubePackage_SRCS

    # add HAL files here as required
)

foreach(SRC_FILE ${STM32_CubePackage_SRCS})
    set(STM32_CubePackage_SRC_FILE SRC_FILE-NOTFOUND)
    find_file(STM32_CubePackage_SRC_FILE ${SRC_FILE}
        PATHS 

        "${PROJECT_BINARY_DIR}/STM32_CubePackage_Source/Drivers/STM32L0xx_HAL_Driver/Src"

        CMAKE_FIND_ROOT_PATH_BOTH
    )
    # message("${SRC_FILE} >> ${STM32_CubePackage_SRC_FILE}") # debug helper
    list(APPEND STM32_CubePackage_SOURCES ${STM32_CubePackage_SRC_FILE})
endforeach()


include(FindPackageHandleStandardArgs)

FIND_PACKAGE_HANDLE_STANDARD_ARGS(STM32_CubePackage DEFAULT_MSG STM32_CubePackage_INCLUDE_DIRS STM32_CubePackage_SOURCES)