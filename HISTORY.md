# Open source releases

## 4.1.12	07/02/2025

- NuGet package: fix manifest generation step for ClickOnce projects by fixing link element file names in .targets files

## 4.1.11	07/02/2025

- NuGet package: fix destination paths of native components

## 4.1.10	16/01/2025

- NuGet package: fix path to arm native component
- update history

## 4.1.9	16/01/2025

- NuGet package: fix path to arm native component

## 4.1.8	16/01/2025

- NuGet package: fix copying of transitive native components

## 4.1.7	17/05/2024

- add support for ARM and ARM64 targets (tested on Raspberry Pi with Windows 11 ARM64)
- switch native component project to .NET SDK 4.8 to fix error MSB3644

## 4.1.6	19/02/2024

- fix signing of StrongName variant
- move build logic trom  msbuild .prj file to build.ps1

## 4.1.5	09/02/2024

- fix CanCyclicTXMsg2::AutoIncrementIndex maximum check (CANFD data len, 0..63)
- update sample to set cyclicMsg.AutoIncrementMode
- fix internal dirty flag handling in cyclic message objects: setting the same AutoIncrementMode cleared the internal dirty flag and led to msg start to fail

## 4.1.4	16/10/2023

- use unique target names for Publish and Build targets to avoid first target to be skipped

## 4.1.3	05/10/2023

- support dotnet publish command: fix deploy to PublishDir with additional targets in .target files

## 4.1.2	26/07/2023

- fix heuristic in VciDevice::UniqueHardwareId::get() which tries to identify the hardware id format (GUID or string) because the implementation did not treat HMS serial numbers as strings

## 4.1.1	24/01/2023

- Use correct value to initialize CANBTRTABLE.bIndex attribute before calling ICanControl::DetectBaud()
- Add mstest based unittests
- Move msgfactory creation to the VciServerImpl ctor as it should not depend on the load state of the vciapi.dll

## 4.1.0	11/11/2022

First open source version (MIT license)

- loader: use Assembly.Location instead of deprecated Assembly.EscapedCodeBase
- loader: additional static Instance() which accepts an assemblyloadpath as parameter
- additional TargetFrameworks net5.0-windows, net6.0-windows and netcoreapp3.1
- lookup platform specific assembly first in exe path (Assembly.GetEntryAssembly()), then in the path where the loader is located (Assembly.GetCallingAssembly()). These assemblies are now located in the <searchdir>/vcinet subdirectory to be able to deploy the correct Ijwhost.dll for each target. (#8111)
- native components: use PlatformToolset v143 and WindowsTargetPlatformVersion 10.0.22000.0
- update to VCI4 core revision 246
- build: add Powershell/MSBuild based build script
- manuals: update manuals to version 1.3


# Older releases (binary only)

## 4.0.439	14/14/2018

minor adjustments to build script

## 4.0.436	12/14/2018

Resolved issues:
- fix version record of native assemblies
- sync to .NET core variant
   fix assembly copyrights
   changed loader code to load architecture specific implementation (vcinet.x??.dll) from same directory as the loader

## 4.0.423	10/23/2018

Resolved issues:
- fix VciDevice::DriverVersion():   return Release/Build version numbers (#7356)
- fix VciDevice::HardwareVersion(): return Branch/Build version numbers
- switch help file generation to SandcastelBuilder v2018.7.8.0
- switch to VCI4 core revision 207
- added missing LinFeatures flags (Sleep and Wakeup)

## 4.0.355	2/9/2017

Resolved issues:
- inherit interface of cyclic messages from rx/tx message interface
- fixed status update of cyclic messages

## 4.0.346	2/7/2017

Resolved issues:
- fixed description text for 833 kbit/s CAN FD bitrate ("CANFD 833 kbit/s")
- added CANFD250KBit
- fixed arbitration bitrates of ShortLineCANFDBitRates to CanBitrate2.CANFD500KBit
- fixed arbitration bitrates of LongLineCANFDBitRates to CanBitrate2.CANFD250KBit

## 4.0.340	2/2/2017

Resolved issues:
- fixed thrown exception in CanControl2::InitLine() if InitLine fails

## 4.0.328	12/22/2016

Resolved issues:
- fixed refcount bug in CanChannel/CanChannel2::GetMessageReader method
- fixed refcount bug in CanChannel/CanChannel2::GetMessageWriter method

## 4.0.324	12/16/2016

Resolved issues:
- removed ToValue/SetValue methods from message interfaces
- inherit ICanMessage2 from ICanMessage
- fixed CanMessageReader::ReadMessage(ICanMessage) which crashed when reading frames from a CanChannel2 instance
- fixed CanMessageReader::ReadMessages to create the correct frame type instances depending on the source channel type
- CanMessageWriter::SendMessage now tries to convert the frame depending on the destination channel type. If this fails e.g. when sending a extended data length CAN FD frame to a CanChannel instance an ArgumentException is thrown.
- fixed bitrate setting for CanBitrate2.Cia500KBit
- fixed bitrate setting for CanBitrate2.CANFD6667KBit, CanBitrate2.CANFD8000KBit, CanBitrate2.CANFD10000KBit

## 4.0.314	12/7/2016

Resolved issues:
- CanChannelStatus: removed useless compare operators
- LinLineStatus: fixed visibility of various properties
- LinMonitorStatus: fixed visibility of various properties
- CanBitrate2: fixed visibility of CanFdBitRates and CiaBitRates property
- CAN-FD bitrates: specified CANFD and IFI CAN-FD specific properties
- CAN-FD bitrates: fixed SSP offsets (TDO) for 8000 and 10000 kbit/s

## 4.0.308	12/6/2016

Resolved issues:
- fixed implementation of CanMessageReader::ReadMessages and LinMessageReader::ReadMessages

## 4.0.304	11/30/2016

Resolved issues:
- added ToString() to CanLineStatus
- added ToString() to CanLineStatus2
- turned ILinLineStatus into struct LinLineStatus
- turned ILinMonitorStatus into struct LinMonitorStatus
- added Busload and Bitrate property to LinLineStatus
- fixed implementation of LineStatus/MonitorStatus property
- fixed range check in CanCyclicTXMsg2::DataLength()

## 4.0.291	11/24/2016

Resolved issues:
- fixed result of MsgFactory::CreateMsg when msg of type ILinMessage is requested

## 4.0.288	11/18/2016

Resolved issues:
- implemented CanScheduler/CanScheduler2 and support for cyclic message objects
- fixed assembly properties of Ixxat.Vci4 and Ixxat.Vci4.Contract assembly
- vcinet.?.dll: set product version equal to file version

## 4.0.255	8/30/2016

First release for VCI4.
