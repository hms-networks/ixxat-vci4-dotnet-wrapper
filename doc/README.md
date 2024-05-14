# Ixxat.VCI4

## Introduction

This package contains the VCI4 .NET Wrapper (VCI4 = Virtual Communication Interface Version 4), 
a component that provides access to the VCI4 C++-API.
VCI4 is a driver/application framework to access HMS CAN or LIN interfaces on the Windows OS,
for details see https://www.ixxat.com/technical-support/support/windows-driver-software

This package is released as a binary package via NuGet for application developers,
for details see https://www.nuget.org/packages/Ixxat.Vci4
There is also a strong named variant which is available from https://www.nuget.org/packages/Ixxat.Vci4.StrongName

## Prerequisites

The VCI4 .NET wrapper sits on top of the VCI4 C++ API so you need the VCI4 driver setup installed.

The target dependent mixed assemblies are dynamically linked against the VC runtime. Make sure the appropriate
runtime library is installed.
For information and downloads see https://learn.microsoft.com/en-us/cpp/windows/latest-supported-vc-redist

## Usage

Simply install the package reference to your project via NuGet. 
After that you should be able to get the device manager within your project

      IVciDeviceManager? deviceManager = null;

      try
      {
        // Get the device manager
        deviceManager = VciServer.Instance()!.DeviceManager;
        ...
      }

and access the other functionality like enumerating/opening devices.
For more information see the examples and the user documentation on 
https://github.com/hms-networks/ixxat-vci4-dotnet-wrapper.

## Internals

The package supports different platforms:

    - x86 (aka. Win32)
    - x64
    - arm (not supported for net40)
    - arm64

and targets:

    - net40 (Framework)
    - netcoreapp3.1 (NetCore)
    - net5.0-windows10.0 (NetCore)
    - net6.0-windows10.0 (NetCore)

There is a subtle difference between Framework and the NetCore targets:
NetCore targets need a special .NET Core app host DLL (ijwhost.dll) 
which is target specific. For more information see
https://learn.microsoft.com/en-us/dotnet/core/porting/cpp-cli

To keep this DLL and the native components together they are located in a 
separate vcinet subdirectory and loaded from there dynamically.

So the layout of the target bin directory is as follows:

    /bin
        Ixxat.Vci4.dll
        Ixxat.Vci4.Contract.dll
        /vcinet
            /x64
              Ijwhost.dll       (not for net40)
              vcinet.x64.dll
            /x86
              Ijwhost.dll       (not for net40)
              vcinet.x86.dll
            /arm
              Ijwhost.dll       (not for net40)
              vcinet.arm.dll
            /arm64
              Ijwhost.dll       (not for net40)
              vcinet.arm64.dll

The app will reference Ixxat.Vci4.dll and Ixxat.Vci4.Contract.dll,
The specific vcinet.<platform>.dll is loaded on demand depending on the platform
the app is running on.
By default this path is below a directory (/vcinet/<platform>) where the Ixxat.Vci4.dll is placed,
but VciServer.Instance() takes a parameter for the basepath you want to load the 
native components from.

