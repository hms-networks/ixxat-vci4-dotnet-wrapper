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

function CreateVersionFileContent {
    param( $Major, $Minor, $Patch, $Copyright, $CompanyName, $SourceRevisionId )

   return "// generated by buildscript

#ifndef _LIBVER_H_
#define _LIBVER_H_

// macro to convert version number to text
#define LIB_VERSION_ASTEXT2(x)     #x
#define LIB_VERSION_ASTEXT(x) LIB_VERSION_ASTEXT2(x)

/*****************************************************************************
* LIB version number
****************************************************************************/
// Major version number
#define LIB_A_VERSION      $Major
#define LIB_A_VERSION_STR  LIB_VERSION_ASTEXT(LIB_A_VERSION)

// Minor version number
#define LIB_B_VERSION      $Minor
#define LIB_B_VERSION_STR  LIB_VERSION_ASTEXT(LIB_B_VERSION)

// Revision number
#define LIB_C_VERSION      $Patch
#define LIB_C_VERSION_STR  LIB_VERSION_ASTEXT(LIB_C_VERSION)

// Build number
#define LIB_D_VERSION      0
#define LIB_D_VERSION_STR  LIB_VERSION_ASTEXT(LIB_D_VERSION)

// debug/release
//
#ifdef _DEBUG
#define LIB_VI_BUILDTYPE_STR   `"debug`"
#else
#define LIB_VI_BUILDTYPE_STR   `"release`"
#endif

/*****************************************************************************
* LIB version information
****************************************************************************/
#define LIB_VI_COPYRIGHT_STR     `"$Copyright`"
#define LIB_VI_COMPANY_NAME_STR  `"$CompanyName`"
#define LIB_VI_PRODUCT_NAME_STR  `"vci4net`"
#define LIB_VI_SPECIALBUILD_STR  `"$SourceRevisionId`"

#define LIB_VI_FILE_VERS      LIB_A_VERSION, LIB_B_VERSION, LIB_C_VERSION, LIB_D_VERSION
#define LIB_VI_FILE_VERS_STR  LIB_VERSION_ASTEXT(LIB_VI_FILE_VERS)

#endif
"
}

$SnExePaths = @( 
    "c:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.8.1 Tools",
    "c:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.8 Tools",
    "c:\Program Files (x86)\Microsoft SDKs\Windows\v8.0A\bin\NETFX 4.0 Tools",
    "c:\Program Files (x86)\Microsoft SDKs\Windows\v7.0A\bin" )

$SnExe = ""
foreach($p in $SnExePaths) {
    $checkpath = Join-Path -Path $p -ChildPath "sn.exe"
    if (Test-Path $checkpath -PathType Leaf) {
        $SnExe = $checkpath
        break
    }
}

if ($SnExe -eq "")
{
    throw "Failed to locate sn.exe"
}
Write-Host "sn.exe:  $SnExe"

function RebuildVSProject {

    # specify key file attribute
    if ($AssemblyKeyFile -notlike '') {
        $Env:BuildSignAssembly = "True"
        $Env:BuildAssemblyKeyFile = $AssemblyKeyFile
    }
    else {
        $Env:BuildSignAssembly = "False"
        $Env:BuildAssemblyKeyFile = ""
    }

    $ContractProject = "src\contract\Ixxat.Vci4.Contract.csproj"
    $LoaderProject = "src\loader\Ixxat.Vci4.csproj"

    # restore nuget packages
    & "dotnet.exe" restore $ContractProject
    & "dotnet.exe" restore $LoaderProject

    # rebuild managed components
    & "$MSBuild" $ContractProject -t:Rebuild -p:Configuration="Release"
    & "$MSBuild" $LoaderProject -t:Rebuild -p:Configuration="Release"

    # rebuild mixed assemblies
    $frameworks = @( "net40", "netcoreapp3.1", "net5.0-windows", "net6.0-windows" )
    foreach($f in $frameworks) {
        & "$MSBuild" "src\vcinetnative.sln" -t:Rebuild -p:Configuration=Release /p:Platform=x86 /p:Framework=$f
        & "$MSBuild" "src\vcinetnative.sln" -t:Rebuild -p:Configuration=Release /p:Platform=x64 /p:Framework=$f
        if ($f -notlike "net40") {
            # arm target not supported for net40 framework
            & "$MSBuild" "src\vcinetnative.sln" -t:Rebuild -p:Configuration=Release /p:Platform=arm /p:Framework=$f
        }
        & "$MSBuild" "src\vcinetnative.sln" -t:Rebuild -p:Configuration=Release /p:Platform=arm64 /p:Framework=$f
    }

    # check if assemblies are strong named
    if ($AssemblyKeyFile -notlike '') {
        foreach ($file in Get-ChildItem -Path "bin\Release\" -Recurse -Filter "*.dll" -Exclude "Ijwhost.dll" ) {
            & "$SnExe" -T $file || $(throw "Assembly `"$file`" not signed")
        }
    }
}

$NugetLocalDir=".\downloads"
$NugetFileName="nuget.exe"
$NugetLocalPath=Join-Path -path $NugetLocalDir -childPath $NugetFileName
$NugetClient = $NugetLocalPath
Write-Host "NugetClient: $NugetClient"

function CreateNugetPackages {

    # create list of properties
    $NugetProperties = "SourceRevisionId=$SourceRevisionId"

    $NugetTargetDir = "nuget"

    $Packagefile = ($AssemblyKeyFile -like '') ? "$NugetTargetDir\Ixxat.Vci4.nuspec" : "$NugetTargetDir\Ixxat.Vci4.StrongName.nuspec"
    $ManualPackagefile = "$NugetTargetDir\Ixxat.Vci4.Manual.nuspec"

    Push-Location -Path "$NugetTargetDir"

    # delete any .nupkg files
    Remove-Item "*.nupkg"

    Pop-Location
    
    # pack nuget package
    Write-Host "`"$NugetClient`" pack -BasePath `"$NugetTargetDir`" $Packagefile -Version $Version -Properties $NugetProperties"
    & "$NugetClient" pack -BasePath "$NugetTargetDir" $Packagefile -Version $Version -Properties $NugetProperties

    # pack symbol package
    Write-Host "`"$NugetClient`" pack -BasePath `"$NugetTargetDir`" $Packagefile -Version $Version -Properties $NugetProperties -Symbols -SymbolPackageFormat snupkg"
    & "$NugetClient" pack -BasePath "$NugetTargetDir" $Packagefile -Version $Version -Properties $NugetProperties -Symbols -SymbolPackageFormat snupkg

    # pack manual package
    Write-Host "`"$NugetClient`" pack -BasePath `"$NugetTargetDir`" $ManualPackagefile -Version $Version -Properties $NugetProperties"
    & "$NugetClient" pack -BasePath "$NugetTargetDir" $ManualPackagefile -Version $Version -Properties $NugetProperties

}

function DownloadNugetClient {
    # locate download nuget client
    $NugetCliUrl="https://dist.nuget.org/win-x86-commandline/latest/nuget.exe"
    if (!(Test-Path $NugetLocalPath)) {
        if (!(Test-Path $NugetLocalDir)) {
            New-Item -path . -name $NugetLocalDir -type directory
        }
    
        Invoke-WebRequest -Uri $NugetCliUrl -OutFile $NugetLocalPath
    }
}


# check if specified AssemblyKeyFile is present
if ($AssemblyKeyFile -ne "")
{
    if (!(Test-Path $AssemblyKeyFile)) {
        throw "specified AssemblyKeyFile $AssemblyKeyFile not found"
    }
}

$SourceRevisionId = $(git rev-parse --short HEAD) || $(throw "Could not determine source revision")
$RepositoryUrl =    $(git remote get-url origin)  || $(throw "Could not determine repository URL")
$RepositoryType =   "git"
$RepositoryBranch = "main"

$AssemblyVersion= ($Version -split "-")[0]
$FileVersion    = ($Version -split "-")[0]

$AssemblyVersion -match "(\d+).(\d+).(\d+).*"
$Major = $Matches[1]
$Minor = $Matches[2]
$Patch = $Matches[3]

$InformationalVersion = ($InformationalVersion -like '') ? $Version : $InformationalVersion
$Authors =     "HMS Networks"
$CompanyName = "HMS Technology Center Ravensburg GmbH"
$Description = "Access CAN interfaces from Ixxat/HMS Networks via .NET and VCI4-API"
$Copyright =   "Copyright (C) 2016-2022 HMS Technology Center Ravensburg GmbH, all rights reserved"

Write-Host "Download Nuget client..."
DownloadNugetClient

Write-Host "Building ..."

# create version file
CreateVersionFileContent -Major $Major -Minor $Minor -Patch $Patch -Copyright $Copyright -CompanyName $CompanyName - SourceRevisionId $SourceRevisionId | Out-File "./src/inc/libver.h"

# build 
RebuildVSProject

Write-Host "Create packages ..."
CreateNugetPackages
