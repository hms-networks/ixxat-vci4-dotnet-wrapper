// SPDX-License-Identifier: MIT
//----------------------------------------------------------------------------
// Summary  : Declarations for the BAL object class.
// Copyright: Copyright (C) 2016-2022 HMS Technology Center Ravensburg GmbH, 
//            all rights reserved
//----------------------------------------------------------------------------

using Ixxat.Vci4;
using System.Collections.Generic;

namespace Ixxat.Vci4.Bal 
{
  using System;
  using System.ComponentModel;
  using System.Collections;

  //*****************************************************************************
  /// <summary>
  ///   This interface represents a BAL (Bus Access Layer) object.
  ///   When no longer needed the BAL object has to be disposed using the 
  ///   IDisposable interface. 
  ///   Get the BAL object of a device by using method 
  ///   <c>IVciDevice.OpenBusAccessLayer()</c>.
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
  ///   IVciDevice device = ...
  ///   IBalObject bal = device.OpenBusAccessLayer();
  ///   // Use bal here
  ///   // ...
  ///   // Dispose object to release native resources
  ///   bal.Dispose();
  ///   device.Dispose();
  ///   </code>
  /// </example>
  //*****************************************************************************
  public interface IBalObject : IDisposable
  {
    //*****************************************************************************
    /// <summary>
    ///   Gets the firmware version.
    /// </summary>
    /// <returns>
    ///   The firmware version.
    /// </returns>
    /// <exception cref="ObjectDisposedException">
    ///   Object is already disposed.
    /// </exception>
    //*****************************************************************************
    Version              FirmwareVersion { get; }

    //*****************************************************************************
    /// <summary>
    ///   Gets a <c>BalResourceCollection</c> that can be used to iterate through
    ///   the available BAL resources or to directly access such one via a
    ///   collection index.
    /// </summary>
    /// <returns>
    ///   A reference to the collection of BAL resources.
    /// </returns>
    /// <exception cref="ObjectDisposedException">
    ///   Object is already disposed.
    /// </exception>
    //*****************************************************************************
    BalResourceCollection Resources { get; }

    //*****************************************************************************
    /// <summary>
    ///   This method opens the specified bus socket.
    /// </summary>
    /// <param name="portNumber">
    ///   Number of the bus socket to open. This parameter must be within the 
    ///   range of 0 to <c>Resources.Count</c> - 1.
    /// </param>
    /// <param name="socketType">
    ///   Type of the bus socket to open. The supported socket types
    ///   are depending on the <c>BusType</c> of the BAL resource specified by the 
    ///   <c>portNumber</c> parameter.
    ///   I.e. for a CAN bus socket the following <c>socketTypes</c> are supported:
    ///     ICanSocket, 
    ///     ICanControl, 
    ///     ICanChannel, 
    ///     ICanScheduler.
    ///   It's possible have several socketType open at the same time (i.e.
    ///   ICanControl and ICanChannel).
    /// </param>
    /// <returns>
    ///   If the method succeeds it returns the opened bus socket object as 
    ///   <c>IBalResource</c> reference. This reference can be casted to
    ///   the type specified by parameter <paramref name="socketType"/>.
    ///   If the method fails it returns a null reference (Nothing in
    ///   VisualBasic).
    ///   When no longer needed the returned socket object has to be disposed using 
    ///   the IDisposable interface. 
    /// </returns>
    /// <remarks>
    ///   The type of the bus socket is implicitly specified by the
    ///   <c>portNumber</c> parameter (see <c>IBalResource.BusType</c> property).
    ///
    ///   The VCI interfaces provide access to native driver resources. Because the 
    ///   .NET garbage collector is only designed to manage memory, but not 
    ///   native OS and driver resources the caller is responsible to release this 
    ///   resources via calling <c>IDisposable.Dispose()</c> when the object is no 
    ///   longer needed. Otherwise native memory and resource leaks may occure.
    /// </remarks>
    /// <exception cref="VciException">
    ///   Opening socket failed.
    /// </exception>
    /// <exception cref="ObjectDisposedException">
    ///   Object is already disposed.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    ///   The specified port number is out of range.
    /// </exception>
    /// <exception cref="NotImplementedException">
    ///   There's no implementation for the specified <paramref name="socketType"/>.
    /// </exception>
    //*****************************************************************************
    IBalResource OpenSocket( byte  portNumber
                           , Type  socketType );
  };


}