Param (
    [Parameter(Mandatory=$true, ValueFromPipeline=$true)]
    [string]$Version= $(throw "-Version is required."),
    [Parameter(Mandatory=$false, ValueFromPipeline=$true)]
    [string]$AssemblyKeyFile=""
)

#
#  Usage:
#  
#  .\build.ps1 -Version 4.1.0 [-AssemblyKeyFile mykeyfile.snk]
#
#  Version parameter is mandatory and specifies the component and package version for the final result.
#  If you specify the AssemblyKeyFile parameter the given key is used to crete a strong named assembly.
#  In this case the resulting package is named Ixxat.Vci4.StrongName.<version>.nupkg.
#  To use the first snk file use
#
#  .\build.ps1 -Version 4.1.0 -AssemblyKeyFile $(Get-ChildItem -Filter *.snk | Select-Object -First 1)
#

$ErrorActionPreference = "Stop"

# locate msbuild
$MSBuild = ""
$MinimumMSBuildVersion = 17
if (Get-Command msbuild -ErrorAction SilentlyContinue)
{
    $MSBuild = "msbuild"
}
else
{
    $MSBuildHome = @("Enterprise", "Professional", "BuildTools", "Community") |ForEach-Object {
        "C:\Program Files\Microsoft Visual Studio\2022\$_\MSBuild\Current"
    } |Where-Object { Test-Path "$_\bin\msbuild.exe" } | Select-Object -First 1

    if (!$MSBuildHome)
    {
        throw "Failed to locate msbuild home directory"
    }

    $MSBuild = "$MSBuildHome\bin\msbuild.exe"
}

if ($MSBuild -ne "")
{
    $MSBuildVersion = [Version](Invoke-Command { . $MSBuild /nologo /version })
    if ($MSBuildVersion.Major -lt $MinimumMSBuildVersion)
    {
        throw 'msbuild version is {0}, but at least major version {1} is expected' -f $MSBuildVersion, $MinimumMSBuildVersion
    }
}
else
{
    throw "Failed to locate msbuild"
}

# locate download nuget client
$NugetLocalDir=".\downloads"
$NugetFileName="nuget.exe"
$NugetLocalPath=Join-Path -path $NugetLocalDir -childPath $NugetFileName
$NugetCliUrl="https://dist.nuget.org/win-x86-commandline/latest/nuget.exe"
if (!(Test-Path $NugetLocalPath)) {
    if (!(Test-Path $NugetLocalDir)) {
        New-Item -path . -name $NugetLocalDir -type directory
    }

    Invoke-WebRequest -Uri $NugetCliUrl -OutFile $NugetLocalPath
}

# check if specified AssemblyKeyFile is present
if ($AssemblyKeyFile -ne "")
{
    if (!(Test-Path $AssemblyKeyFile)) {
        throw "specified AssemblyKeyFile $AssemblyKeyFile not found"
    }
}

# now set properties and call MSBuild
$Properties = @{
    Version = $Version
    SourceRevisionId = $(git rev-parse --short HEAD) || $(throw "Could not determine source revision")
    RepositoryUrl =    $(git remote get-url origin)  || $(throw "Could not determine repository URL")
    RepositoryType = "git"
    RepositoryBranch = "main"
    NugetClient = $NugetLocalPath
    AssemblyKeyFileAttribute = $AssemblyKeyFile
}

$MSBuildProperties = $Properties.GetEnumerator() | Where-Object { 
    $_.Value
} | ForEach-Object { 
    "/p:{0}={1}" -f $_.Key,$_.Value 
}

Write-Host "Building ..."
&$MSBuild vci4net.prj $MSBuildProperties
if ($LastExitCode)
{
    exit $LastExitCode
}
