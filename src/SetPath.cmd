rem
rem path to VCISdk
rem
SET VciSDKDir=\\fs-rav-win-002.hms.se\Artifacts\IndustrialCom\Drivers\VCI4\core\246\SDK

REM find latest VC install directory via vswhere
for /f "usebackq tokens=*" %%i in (`vswhere -version "[17.0,18.0)" -products * -requires Microsoft.VisualStudio.Component.VC.Tools.x86.x64 -property installationPath`) do (
  set VcInstallDir=%%i
)

set MSBUILD="%VcInstallDir%\MSBuild\Current\Bin\MSBuild.exe"
if not exist %MSBUILD% set MSBUILD="%VcInstallDir%\MSBuild\15.0\Bin\MSBuild.exe"
if not exist %MSBUILD% set MSBUILD="%VcInstallDir%\MSBuild\15.0\Bin\amd64\MSBuild.exe"
if not exist %MSBUILD% echo "Error: msbuild not found"

