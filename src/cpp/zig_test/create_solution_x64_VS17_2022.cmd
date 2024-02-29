if not exist _cmake_win64 mkdir _cmake_win64
cd _cmake_win64
rem set CC="zig cc"
rem set CXX="zig c++"
cmake -G "Visual Studio 17 2022" .. -DCC="zig cc" -DCXX="zig c++" -DCMAKE_TOOLCHAIN_FILE=../../../../vcpkg/scripts/buildsystems/vcpkg.cmake -DVCPKG_TARGET_TRIPLET=x64-windows-static
cd ..
