# Ixxat.VCI4

## Introduction

This package contains the VCI4 .NET Wrapper (VCI4 = Virtual Communication Interface Version 4), 
a component that provides access to the VCI4 C++-API.
VCI4 is a driver/application framework to access HMS CAN or LIN interfaces on the Windows OS,
for details see https://www.ixxat.com/technical-support/support/windows-driver-software

This package is released as a binary package via NuGet for application developers,
for details see https://www.nuget.org/packages/Ixxat.Vci4.net35
There is also a strong named variant which is available from https://www.nuget.org/packages/Ixxat.Vci4.net35.StrongName

## Usage

Simply install the package reference to your project via NuGet. 
After that you should be able to get the device manager within your project

      IVciDeviceManager deviceManager = null;

      try
      {
        // Get the device manager
        deviceManager = VciServer.Instance().DeviceManager;
        ...
      }

and access the other functionality like enumerating/opening devices.
For more information see the examples and the user documentation on 
https://github.com/hms-networks/ixxat-vci4-dotnet-wrapper.

## Internals

The package supports this targets:

    - net35 (Framework)

The native components are located in a 
separate vcinet subdirectory and loaded from there dynamically.

So the layout of the target bin directory is as follows:

    /bin
        Ixxat.Vci4.dll
        Ixxat.Vci4.Contract.dll
        /vcinet
            /x64
              vcinet.x64.dll
            /x86
              vcinet.x86.dll

The app will reference Ixxat.Vci4.dll and Ixxat.Vci4.Contract.dll,
vcinet.x64.dll or vcinet.x86.dll are loaded on demand depending on the platform
the app is running on.
By default this path is below a directory (/vcinet/<platform>) where the Ixxat.Vci4.dll is placed,
but VciServer.Instance() takes a parameter for the basepath you want to load the 
native components from.

