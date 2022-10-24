# Open source releases

## 4.1.0	28/10/2022

First open sourced version (MIT license)

- Loader: use Assembly.Location instead of deprecated Assembly.EscapedCodeBase
- additional TargetFrameworks net5.0-windows, net6.0-windows and netcoreapp3.1
- lookup platform specific assembly first in exe path (Assembly.GetEntryAssembly()), then in the path where the loader is located (Assembly.GetCallingAssembly()) (#8111)
- native components: use PlatformToolset v143 and WindowsTargetPlatformVersion 10.0.22000.0
- update to VCI4 core revision 246
- build: add Powershell/MSBuild based build script

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
