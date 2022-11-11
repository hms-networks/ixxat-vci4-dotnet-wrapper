// SPDX-License-Identifier: MIT
//----------------------------------------------------------------------------
// Summary  : Declarations for the VCI device enumerator object class.
// Copyright: Copyright (C) 2016-2022 HMS Technology Center Ravensburg GmbH, 
//            all rights reserved
//----------------------------------------------------------------------------

namespace Ixxat.Vci4 
{
  using System;
  using System.ComponentModel;
  using System.Threading;
  using System.Collections;

  //*****************************************************************************
  /// <summary>
  ///   This interface represents the list of installed VCI devices.
  ///   When no longer needed the VCI device list object has to be disposed 
  ///   using the IDisposable interface. 
  ///   To observe changes within this list use the <c>AssignEvent</c> methods 
  ///   to register an event. This event is set to the signaled state whenever 
  ///   the contents of the device list changes.
  ///   Use <c>GetEnumerator()</c> to enumerate the list of VCI device objects
  ///   whereas each VCI device object can be accessed by it's <c>IVciDevice</c> 
  ///   interface.
  ///   The enumerator object returned by <c>GetEnumerator()</c> also has to be
  ///   disposed using the IDisposable interface. 
  ///   Excplicitly disposing the enumerator object can be omitted when using 
  ///   the C# foreach statement. foreach implicitly disposes the enumerator.
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
  public interface IVciDeviceList : IEnumerable, IDisposable
  {
    //*****************************************************************************
    /// <summary>
    ///   This method assigns an event object to the list. The event is
    ///   set to the signaled state whenever the contents of the device list
    ///   changes.
    /// </summary>
    /// <param name="changeEvent">
    ///   The event object which is to be set whenever the contents of the device 
    ///   list changes.
    /// </param>
    /// <exception cref="VciException">
    ///   Assigning the event failed.
    /// </exception>
    /// <exception cref="ArgumentNullException">
    ///   Parameter changeEvent was a null reference.
    /// </exception>
    /// <exception cref="ObjectDisposedException">
    ///   Object is already disposed.
    /// </exception>
    //*****************************************************************************
    void AssignEvent( AutoResetEvent   changeEvent );

    //*****************************************************************************
    /// <summary>
    ///   This method assigns an event object to the list. The event is
    ///   set to the signaled state whenever the contents of the device list
    ///   changes.
    /// </summary>
    /// <param name="changeEvent">
    ///   The event object which is to be set whenever the contents of the device 
    ///   list changes.
    /// </param>
    /// <exception cref="VciException">
    ///   Assigning the event failed.
    /// </exception>
    /// <exception cref="ArgumentNullException">
    ///   Parameter changeEvent was a null reference.
    /// </exception>
    /// <exception cref="ObjectDisposedException">
    ///   Object is already disposed.
    /// </exception>
    //*****************************************************************************
    void AssignEvent( ManualResetEvent changeEvent );
  };


}