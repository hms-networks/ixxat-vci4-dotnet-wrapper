@echo off
rem
rem path to VCISdk
rem
rem VciSDKDir is set by the VCI4 installation and points to the installed VCI4 header files
rem You can set the path to the headers by uncomment/adjust the following line
rem SET VciSDKDir=C:\Program Files\HMS\Ixxat VCI\sdk\vci

if not exist "%VciSDKDir%" echo "Error: VciSDKDir not found"

REM find latest VC install directory via vswhere
set VSWHEREEXE="%ProgramFiles(x86)%\Microsoft Visual Studio\Installer\vswhere.exe"

REM find latest VC install directory via vswhere
for /f "usebackq delims=" %%a in (`call %VSWHEREEXE% -version "[17.9,18.0)" -property installationPath`) do set VcInstallDir=%%a

set MSBUILD="%VcInstallDir%\MSBuild\Current\Bin\MSBuild.exe"
if not exist %MSBUILD% set MSBUILD="%VcInstallDir%\MSBuild\15.0\Bin\MSBuild.exe"
if not exist %MSBUILD% set MSBUILD="%VcInstallDir%\MSBuild\15.0\Bin\amd64\MSBuild.exe"
if not exist %MSBUILD% echo "Error: msbuild not found"

