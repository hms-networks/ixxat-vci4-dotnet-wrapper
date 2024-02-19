@echo off
rem
rem path to VCISdk
rem
rem VciSDKDir is set by the VCI4 installation and points to the installed VCI4 header files
rem You can set the path to the headers by uncomment/adjust the following line
rem SET VciSDKDir=C:\Program Files\HMS\Ixxat VCI\sdk\vci

if not exist "%VciSDKDir%" echo "Error: VciSDKDir not found"

REM VS2008 install
set VcInstallDir=c:\Program Files (x86)\Microsoft Visual Studio 9.0\

REM MSBUILD from VS2019
REM set MSBUILD="c:\Program Files (x86)\Microsoft Visual Studio\2019\Professional\MSBuild\Current\Bin\amd64\MSBuild.exe"
REM if not exist %MSBUILD% set MSBUILD="%VcInstallDir%\MSBuild\15.0\Bin\MSBuild.exe"
REM if not exist %MSBUILD% set MSBUILD="%VcInstallDir%\MSBuild\15.0\Bin\amd64\MSBuild.exe"
REM if not exist %MSBUILD% echo "Error: msbuild not found"

