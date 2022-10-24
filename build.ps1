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
#  To use the first snk file use
#
#  .\build.ps1 -Version 4.1.0 -AssemblyKeyFile $(Get-ChildItem -Filter mykeyfile.snk | Select-Object -First 1)
#

$ErrorActionPreference = "Stop"
$LocateMSBuild = $true
$MinimumMSBuildVersion = $true
if (Get-Command msbuild -ErrorAction SilentlyContinue)
{
    $MSBuildVersion = [Version](msbuild /nologo /version)
    $LocateMSBuild = $MSBuildVersion.Major -lt $MinimumMSBuildVersion
    if (!$LocateMSBuild)
    {
        $MSBuild = "msbuild"
    }
}

if ($LocateMSBuild)
{
    $MSBuildHome = @("Enterprise", "Professional", "BuildTools", "Community") |ForEach-Object {
        "C:\Program Files\Microsoft Visual Studio\2022\$_\MSBuild\Current"
    } |Where-Object { Test-Path "$_\bin\msbuild.exe" } | Select-Object -First 1

    if (!$MSBuildHome)
    {
        throw "Failed to locate msbuild"
    }

    $MSBuild = "$MSBuildHome\bin\msbuild.exe"
}

if ($AssemblyKeyFile -ne "")
{
    if (!(Test-Path $AssemblyKeyFile)) {
        throw "specified AssemblyKeyFile $AssemblyKeyFile not found"
    }
}

$Properties = @{
    Version = $Version
    SourceRevisionId = $(git rev-parse --short HEAD) || $(throw "Could not determine source revision")
    RepositoryUrl =    $(git remote get-url origin)  || $(throw "Could not determine repository URL")
    RepositoryType = "git"
    RepositoryBranch = "main"
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
