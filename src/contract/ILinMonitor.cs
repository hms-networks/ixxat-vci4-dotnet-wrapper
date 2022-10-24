/*****************************************************************************
 IXXAT Automation GmbH
******************************************************************************
 Summary : Declarations for the LIN channel class.
 Compiler: Microsoft VC++ 8
******************************************************************************
 all rights reserved
*****************************************************************************/


/*************************************************************************
** namespace Ixxat.Vci4.Bal.Lin
*************************************************************************/
namespace Ixxat.Vci4.Bal.Lin 
{
  /*************************************************************************
  ** used namespace
  *************************************************************************/
  using System;

  //*****************************************************************************
  /// <summary>
  ///   This interface represents a LIN communication monitor and is used to 
  ///   receive LIN messages.
  ///   When no longer needed the LIN communication monitor object has to be 
  ///   disposed using the IDisposable interface. 
  ///   A LIN communication monitor can be got via method 
  ///   <c>IBalObject.OpenSocket()</c>. Unless the CAN socket is not already 
  ///   in use exclusively, it's possible to open several concurrently 
  ///   communication monitors on the same socket.
  /// </summary>
  /// <remarks>
  ///   The VCI interfaces provides access to native driver resources. Because the 
  ///   .NET garbage collector is only designed to manage memory, but not 
  ///   native OS and driver resources the caller is responsible to release this 
  ///   resources via calling <c>IDisposable.Dispose()</c> when the object is no 
  ///   longer needed. Otherwise native memory and resource leaks may occure.
  /// </remarks>
  /// <example>
  ///   <code>
  ///   IBalObject bal = ...
  ///   // Open monitor on first LIN socket
  ///   ILinMonitor monitor = bal.OpenSocket(0, typeof(ILinMonitor)) as ILinMonitor;
  ///   
  ///   // Initialize monitor non-exclusively
  ///   monitor.Initialize(100, false);
  ///   
  ///   // Use monitor
  ///   // ...
  ///   
  ///   // Dispose the objects
  ///   monitor.Dispose();
  ///   bal.Dispose();
  ///   </code>
  /// </example>
  //*****************************************************************************
  public interface ILinMonitor : ILinSocket
  {
    //*****************************************************************************
    /// <summary>
    ///   Gets the current status of the LIN monitor.
    /// </summary>
    /// <exception cref="VciException">
    ///   Getting channel status failed.
    /// </exception>
    /// <exception cref="ObjectDisposedException">
    ///   Object is already disposed or not initialized, yet.
    /// </exception>
    //*****************************************************************************
    LinMonitorStatus MonitorStatus { get; }

    //*****************************************************************************
    /// <summary>
    ///   Gets a reference to a new instance of a message reader object for the 
    ///   monitor. This message reader provides access to the monitor's receive 
    ///   buffer.
    ///   LIN messages received from the LIN line can be read from this object.
    /// </summary>
    /// <returns>
    ///   A reference to the message reader of the monitor.
    ///   When no longer needed the message reader object has to be 
    ///   disposed using the IDisposable interface. 
    /// </returns>
    /// <remarks>
    ///   The VCI interfaces provides access to native driver resources. Because the 
    ///   .NET garbage collector is only designed to manage memory, but not 
    ///   native OS and driver resources the caller is responsible to release this 
    ///   resources via calling <c>IDisposable.Dispose()</c> when the object is no 
    ///   longer needed. Otherwise native memory and resource leaks may occure.
    /// </remarks>
    /// <exception cref="VciException">
    ///   Getting the message reader failed.
    /// </exception>
    /// <exception cref="ObjectDisposedException">
    ///   Object is already disposed or not initialized, yet.
    /// </exception>
    //*****************************************************************************
    ILinMessageReader GetMessageReader();

    //*****************************************************************************
    /// <summary>
    ///   This method initializes the LIN monitor. This method must be called
    ///   prior to any other method of the interface.
    /// </summary>
    /// <param name="receiveFifoSize">
    ///   Size of the receive buffer (number of CAN messages)
    /// </param>
    /// <param name="exclusive">
    ///   If this parameter is set to true the method tries
    ///   to initialize the monitor for exclusive use. If set
    ///   to false, the method initializes the monitor for
    ///   shared access.
    /// </param>
    /// <exception cref="VciException">
    ///   Monitor initialization failed.
    /// </exception>
    /// <exception cref="ObjectDisposedException">
    ///   Object is already disposed.
    /// </exception>
    /// <remarks>
    ///   The monitor is deactivated after this method returns an must be
    ///   activeted by an Activate() method call.
    ///   The method can be called more than once to reconfigure the size
    ///   of the receive or transmit FIFOs.
    /// </remarks>
    //*****************************************************************************
    void Initialize( ushort receiveFifoSize
                   , bool   exclusive );

    //*****************************************************************************
    /// <summary>
    ///   This method activates the LIN monitor. After activating the channel,
    ///   LIN messages can be received through the message reader.
    /// </summary>
    /// <exception cref="VciException">
    ///   Monitor activation failed.
    /// </exception>
    /// <exception cref="ObjectDisposedException">
    ///   Object is already disposed or not initialized, yet.
    /// </exception>
    /// <remarks>
    ///   The LIN line must be started, otherwise no messages are received or
    ///   transmitted from/to the LIN line (see also ILinControl.StartLine).
    /// </remarks>
    //*****************************************************************************
    void Activate  ( );

    //*****************************************************************************
    /// <summary>
    ///   This method deactivates the LIN monitor. After deactivating the monitor,
    ///   no further LIN messages are received from the LIN line.
    /// </summary>
    /// <exception cref="VciException">
    ///   Monitor deactivation failed.
    /// </exception>
    /// <exception cref="ObjectDisposedException">
    ///   Object is already disposed or not initialized, yet.
    /// </exception>
    //*****************************************************************************
    void Deactivate( );
  };


}