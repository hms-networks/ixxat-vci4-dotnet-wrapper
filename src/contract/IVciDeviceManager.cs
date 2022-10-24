/*****************************************************************************
 IXXAT Automation GmbH
******************************************************************************
 Summary : Declarations for the VCI device manager object class.
 Compiler: Microsoft VC++ 8
******************************************************************************
 all rights reserved
*****************************************************************************/

/*************************************************************************
** namespace Ixxat.Vci4
*************************************************************************/
namespace Ixxat.Vci4 
{
  using System;
  using System.ComponentModel;


  //*****************************************************************************
  /// <summary>
  ///   The VCI device manager manages the list of VCI device objects.
  ///   Use the static <c>VciServer</c> class to get a VCI device manager
  ///   object.
  ///   When no longer needed the VCI device manager object has to be disposed 
  ///   using the IDisposable interface. 
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
  ///   // Use deviceManager here
  ///   // ...
  ///   // Dispose object to release native resources
  ///   deviceManager.Dispose();
  ///   </code>
  /// </example>
  //*****************************************************************************
  public interface IVciDeviceManager : IDisposable
  {
    //*****************************************************************************
    /// <summary>
    ///   Gets the list of VCI device objects represeted by a
    ///   <c>IVciDeviceList</c> interface.
    /// </summary>
    /// <returns>
    ///   The list of VCI device objects represeted by a <c>IVciDeviceList</c>
    ///   interface.
    /// </returns>
    /// <exception cref="VciException">
    ///   Thrown if creation of the device list failed.
    /// </exception>
    /// <exception cref="ObjectDisposedException">
    ///   Object is already disposed.
    /// </exception>
    //*****************************************************************************
    IVciDeviceList GetDeviceList();
  };


} 
