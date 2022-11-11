// SPDX-License-Identifier: MIT
//----------------------------------------------------------------------------
// Summary  : Declarations for the LIN socket class.
// Copyright: Copyright (C) 2016-2022 HMS Technology Center Ravensburg GmbH, 
//            all rights reserved
//----------------------------------------------------------------------------

namespace Ixxat.Vci4.Bal.Lin 
{
  using System;

  //*****************************************************************************
  /// <summary>
  ///   Enumeration of flag values that are used to signalize the features 
  ///   supported by a LIN controller (see <c>ILinSocket</c>).
  /// </summary>
  //*****************************************************************************
  [Flags]
  public enum LinFeatures 
  {
    /// <summary>
    ///   Indicates if the LIN master mode is supported
    /// </summary>
    Master      = 0x0001,
    /// <summary>
    ///   Indicates if automatic bitrate detection is supported
    /// </summary>
    Autorate    = 0x0002,
    /// <summary>
    ///   Indicates if reception of error frames is supported
    /// </summary>
    ErrFrame    = 0x0004,  
    /// <summary>
    ///   Indicates if bus load measurement is supported
    /// </summary>
    Busload     = 0x0008,
    /// <summary>
    ///   Supports sleep message (master only)
    /// </summary>
    Sleep       = 0x0010,
    /// <summary>
    ///   Supports wakeup message
    /// </summary>
    Wakeup      = 0x0020
  };


  //*****************************************************************************
  /// <summary>
  ///   <c>ILinSocket</c> provides the properties and capabilities of a
  ///   LIN controller.
  ///   When no longer needed the LIN socket object has to be disposed using the 
  ///   IDisposable interface. 
  ///   A LIN socket object can be got via method <c>IBalObject.OpenSocket()</c>.
  ///   Additionally <c>ILinSocket</c> is the base interface for several other
  ///   LIN bus specific socket interfaces like <c>ILinControl</c> and 
  ///   <c>ILinMonitor</c>.
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
  ///   IBalObject bal = ...
  ///   // Open first LIN socket
  ///   ILinSocket socket = bal.OpenSocket(0, typeof(ILinSocket)) as ILinSocket;
  ///   
  ///   // Use socket
  ///   // ...
  ///   
  ///   // Dispose socket an BAL
  ///   socket.Dispose();
  ///   bal.Dispose();
  ///   </code>
  /// </example>
  //*****************************************************************************
  public interface ILinSocket : IBalResource
  {
    //*****************************************************************************
    /// <summary>
    ///   Gets the current status of the LIN line.
    /// </summary>
    /// <exception cref="VciException">
    ///   Getting LIN line status failed.
    /// </exception>
    /// <exception cref="ObjectDisposedException">
    ///   Object is already disposed.
    /// </exception>
    //*****************************************************************************
    LinLineStatus LineStatus { get; }

    //*****************************************************************************
    /// <summary>
    ///   Gets a flag field indicating the features supported by the LIN controller.
    /// </summary>
    /// <exception cref="ObjectDisposedException">
    ///   Object is already disposed.
    /// </exception>
    //*****************************************************************************
    LinFeatures Features { get; }

    //*****************************************************************************
    /// <summary>
    ///   Gets a value indicating if the LIN socket supports LIN master mode.
    /// </summary>
    /// <exception cref="ObjectDisposedException">
    ///   Object is already disposed.
    /// </exception>
    //*****************************************************************************
    bool SupportsMasterMode { get; }

    //*****************************************************************************
    /// <summary>
    ///   Gets a value indicating if the LIN socket supports automatic baudrate
    ///   detection.
    /// </summary>
    /// <exception cref="ObjectDisposedException">
    ///   Object is already disposed.
    /// </exception>
    //*****************************************************************************
    bool SupportsAutorate { get; }

    //*****************************************************************************
    /// <summary>
    ///   Gets a value indicating if the LIN socket supports error frame reception.
    /// </summary>
    /// <exception cref="ObjectDisposedException">
    ///   Object is already disposed.
    /// </exception>
    //*****************************************************************************
    bool SupportsErrorFrames { get; }

    //*****************************************************************************
    /// <summary>
    ///   Gets a value indicating if the LIN socket supports bus load computation.
    /// </summary>
    /// <exception cref="ObjectDisposedException">
    ///   Object is already disposed.
    /// </exception>
    //*****************************************************************************
    bool SupportsBusLoadComputation { get; }

    //*****************************************************************************
    /// <summary>
    ///   Gets the frequency to the primary timer in Hz.
    /// </summary>
    /// <exception cref="ObjectDisposedException">
    ///   Object is already disposed.
    /// </exception>
    //*****************************************************************************
    uint ClockFrequency { get; }

    //*****************************************************************************
    /// <summary>
    ///   Gets the divisor factor of the time stamp counter. 
    ///   The time stamp counter provides the time stamp for LIN messages. 
    ///   The frequency of the time stamp counter is calculated from the frequency 
    ///   of the primary timer (<c>ClockFrequency</c>) divided by the value 
    ///   specified here.
    /// </summary>
    /// <exception cref="ObjectDisposedException">
    ///   Object is already disposed.
    /// </exception>
    //*****************************************************************************
    uint TimeStampCounterDivisor { get; }
  };


}