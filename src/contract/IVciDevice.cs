/*****************************************************************************
 IXXAT Automation GmbH
******************************************************************************
 Summary : Declarations for the VCI device object class.
 Compiler: Microsoft VC++ 8
******************************************************************************
 all rights reserved
*****************************************************************************/

namespace Ixxat.Vci4 
{
  using System;
  using System.ComponentModel;

  //*****************************************************************************
  /// <summary>
  ///   This interface represents a VCI device object.
  ///   When no longer needed the VCI device object has to be disposed using 
  ///   the IDisposable interface. 
  ///   Get a VCI device object via the device list of <c>IVciDeviceManager</c>.
  /// </summary>
  /// <remarks>
  ///   The VCI interfaces provide access to native driver resources. Because the 
  ///   .NET garbage collector is only designed to manage memory, but not 
  ///   native OS and driver resources the caller is responsible to release this 
  ///   resources via calling <c>IDisposable.Dispose()</c> when the object is no 
  ///   longer needed. Otherwise native memory and resource leaks may occure.
  /// </remarks>
  /// <example>
  ///   <code>
  ///   IVciDeviceManager deviceManager = VciServer.Instance().DeviceManager;
  ///   IVciDeviceList devices = deviceManager.GetDeviceList();
  ///   foreach(IVciDevice device in devices)
  ///   {
  ///     // Use device here
  ///     // ...
  ///     // Dispose object to release native resources
  ///     device.Dispose();
  ///   }
  ///   devices.Dispose();
  ///   deviceManager.Dispose();
  ///   </code>
  /// </example>
  //*****************************************************************************
  public interface IVciDevice : IDisposable
  {
    //*****************************************************************************
    /// <summary>
    ///   Gets the unique VCI object id of the device.
    /// </summary>
    /// <returns>
    ///   Unique VCI object id of the device.
    /// </returns>
    /// <exception cref="ObjectDisposedException">
    ///   Object is already disposed.
    /// </exception>
    //*****************************************************************************
    long               VciObjectId       { get; }

    //*****************************************************************************
    /// <summary>
    ///   Gets the ID of the device class. Each device driver identifies its device 
    ///   class in the form of a globally unique ID (GUID). Different adapters 
    ///   belong to different device classes. Applications can use the device 
    ///   class to distinguish between an IPC-I165/PCI and a PC-I04/PCI card, for 
    ///   example.
    /// </summary>
    /// <returns>
    ///   ID of the device class.
    /// </returns>
    /// <exception cref="ObjectDisposedException">
    ///   Object is already disposed.
    /// </exception>
    //*****************************************************************************
    Guid               DeviceClass       { get; }

    //*****************************************************************************
    /// <summary>
    ///   Gets the version of the VCI device driver.
    /// </summary>
    /// <returns>
    ///   Version of the VCI device driver.
    /// </returns>
    /// <exception cref="ObjectDisposedException">
    ///   Object is already disposed.
    /// </exception>
    //*****************************************************************************
    Version            DriverVersion     { get; }

    //*****************************************************************************
    /// <summary>
    ///   Gets the version of the VCI device hardware.
    /// </summary>
    /// <returns>
    ///   Version of the VCI device hardware.
    /// </returns>
    /// <exception cref="ObjectDisposedException">
    ///   Object is already disposed.
    /// </exception>
    //*****************************************************************************
    Version            HardwareVersion   { get; }

    //*****************************************************************************
    /// <summary>
    ///   Gets the unique ID of the adapter. Each adapter has a unique ID that can 
    ///   be used to distinguish between two PC-I04/PCI cards, for example. 
    ///   Because this value can be either a GUID or a string with the serial 
    ///   number the retrieved value is either a string reference or a boxed Guid 
    ///   instance. 
    /// </summary>
    /// <returns>
    ///   Unique hardware id of the device.
    /// </returns>
    /// <exception cref="ObjectDisposedException">
    ///   Object is already disposed.
    /// </exception>
    //*****************************************************************************
    Object             UniqueHardwareId  { get; }

    //*****************************************************************************
    /// <summary>
    ///   Gets the device description string.
    /// </summary>
    /// <returns>
    ///   The device description string.
    /// </returns>
    /// <exception cref="ObjectDisposedException">
    ///   Object is already disposed.
    /// </exception>
    //*****************************************************************************
    string             Description       { get; }

    //*****************************************************************************
    /// <summary>
    ///   Gets the device manufacturer string.
    /// </summary>
    /// <returns>
    ///   The device manufacturer string.
    /// </returns>
    /// <exception cref="ObjectDisposedException">
    ///   Object is already disposed.
    /// </exception>
    //*****************************************************************************
    string             Manufacturer      { get; }

    //*****************************************************************************
    /// <summary>
    ///   Gets a description of the hardware equipment of the device.
    /// </summary>
    /// <returns>
    ///   The retrieved array contains a <c>VciCtrlInfo</c> for each 
    ///   existing fieldbus controller.
    /// </returns>
    /// <exception cref="ObjectDisposedException">
    ///   Object is already disposed.
    /// </exception>
    //*****************************************************************************
    IVciCtrlInfo[] Equipment { get; }

    //*****************************************************************************
    /// <summary>
    ///   This method is called to open the Bus Access Layer.
    /// </summary>
    /// <returns>
    ///   If succeeded a reference to the Bus Access Layer, otherwise a null 
    ///   reference (Nothing in VisualBasic).
    ///   When no longer needed the BAL object has to be disposed using the 
    ///   IDisposable interface. 
    /// </returns>
    /// <remarks>
    ///   The VCI interfaces provide access to native driver resources. Because the 
    ///   .NET garbage collector is only designed to manage memory, but not 
    ///   native OS and driver resources the caller is responsible to release this 
    ///   resources via calling <c>IDisposable.Dispose()</c> when the object is no 
    ///   longer needed. Otherwise native memory and resource leaks may occure.
    /// </remarks>
    /// <exception cref="ObjectDisposedException">
    ///   Object is already disposed.
    /// </exception>
    //*****************************************************************************
    Ixxat.Vci4.Bal.IBalObject OpenBusAccessLayer();
  };


}