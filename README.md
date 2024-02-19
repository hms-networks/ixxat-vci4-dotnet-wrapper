# VCI4 .NET Wrapper (for VS2008/.NET35)

## Attention

Note, that this is a special variant compiled with VS2008 targeting .NET 3.5.
Reference this package only in .NET 3.5 projects.

## Introduction

This project contains the source of the VCI4 .NET Wrapper (VCI4 = Virtual Communication Interface Version 4), 
a component that provides access to the VCI4 C++-API.
VCI4 is a driver/application framework to access HMS CAN or LIN interfaces on the Windows OS,
for details see https://www.ixxat.com/technical-support/support/windows-driver-software

This package is released as a binary package via NuGet for application developers,
for details see https://www.nuget.org/packages/Ixxat.Vci4.net35
There is also a strong named variant which is available from https://www.nuget.org/packages/Ixxat.Vci4.net35.StrongName

## Open source

The main purpose to release this component as open source is to give application developers 
a better insight into the internal workings of the component.
As it is mostly a 1:1 wrapper to the underlying VCI4 C++-API there is little encoded functionality, 
nethertheless access to the source could help clarify unclear behaviour or help detect bugs.

Developers which need more control could alter the component or use it as a template to implement their own 
device access component on top of the VCI4 C++-API.


## For users

See the user documentation in /doc/README.md


## For developers

The component is split into three parts: 

- a contract assembly (interface declaration assembly)
- a loader
- a managed C++ implementation

The C++ part is compiled twice to x86 and x64 targets.
A .NET application only references the loader and the contract assembly and could therefore be kept target agnostic (AnyCPU).
The loader checks the platform and loads the corresponding target specific assembly.

To compile the component start a Powershell console and execute

    PS> .\build.ps1 -Version=4.1.0

If you want to create a strong named version add the AssemblyKeyFile parameter

    PS> .\build.ps1 -Version=4.1.0 -AssemblyKeyFile=mykeyfile.snk

For more information have a look at the build script .\build.ps1

### Prerequisites

- VisualStudio 2008
- nuget.exe client version 6.3.0
- optional Sandcastle Help File Builder version 2021.11.7.0

The VCI4 .NET wrapper sits on top of the VCI4 C++ API so you need the VCI4 driver setup installed.

### Source tree overview

    \doc           user documentation
    \images        package icon
    \nuget         files for nuget packaging (manifest files)
    \src\contract  Contract assembly
    \src\help      Help file builder project
    \src\loader    Loader component (API entry point)
    \src\impl      Component implementation (mixed assembly source)
    \samples       minimal set of samples

## Further documentation

See the folder /doc/manual for the user manuals which also gives a system overview.

Installing the VCI4 Setup provides additional manuals for the available VCI4 APIs.
