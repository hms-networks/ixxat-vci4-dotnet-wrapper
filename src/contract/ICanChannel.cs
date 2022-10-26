/*****************************************************************************
 IXXAT Automation GmbH
******************************************************************************
 Summary : Declarations for the CAN channel class.
 Compiler: Microsoft VC++ 8
******************************************************************************
 all rights reserved
*****************************************************************************/

namespace Ixxat.Vci4.Bal.Can 
{
  using System;

  //*****************************************************************************
  /// <summary>
  ///   This interface represents a CAN communication channel and is used to 
  ///   receive and transmit CAN messages.
  ///   When no longer needed the CAN communication channel object has to be 
  ///   disposed using the IDisposable interface. 
  ///   A CAN communication channel can be got via method 
  ///   <c>IBalObject.OpenSocket()</c>. Unless the CAN socket is not already 
  ///   in use exclusively, it's possible to open several concurrently 
  ///   communication channels on the same socket.
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
  ///   // Open communication channel on first CAN socket
  ///   ICanChannel channel = bal.OpenSocket(0, typeof(ICanChannel)) as ICanChannel;
  ///   
  ///   // Initialize channel non-exclusively
  ///   channel.Initialize(100, 100, false);
  ///   
  ///   // Use channel
  ///   // ...
  ///   
  ///   // Dispose channel an BAL
  ///   channel.Dispose();
  ///   bal.Dispose();
  ///   </code>
  /// </example>
  //*****************************************************************************
  public interface ICanChannel : ICanSocket
  {
    //*****************************************************************************
    /// <summary>
    ///   Gets the current status of the CAN channel.
    /// </summary>
    /// <exception cref="VciException">
    ///   Getting channel status failed.
    /// </exception>
    /// <exception cref="ObjectDisposedException">
    ///   Object is already disposed or not initialized, yet.
    /// </exception>
    //*****************************************************************************
    CanChannelStatus ChannelStatus { get; }

    //*****************************************************************************
    /// <summary>
    ///   Gets a reference to a new instance of a message reader object for the 
    ///   channel. This message reader provides access to the channel's receive 
    ///   buffer.
    ///   CAN messages received from the CAN line can be read from this object.
    /// </summary>
    /// <returns>
    ///   A reference to the message reader of the channel.
    ///   When no longer needed the message reader object has to be 
    ///   disposed using the IDisposable interface. 
    /// </returns>
    /// <remarks>
    ///   The VCI interfaces provide access to native driver resources. Because the 
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
    ICanMessageReader  GetMessageReader( );

    //*****************************************************************************
    /// <summary>
    ///   Gets a reference to a new instance of a message writer object for the 
    ///   channel. THis message writer provides access to the channel's transmit 
    ///   buffer.
    ///   CAN messages written to this transmit buffer are transmitted over the 
    ///   CAN line.
    /// </summary>
    /// <returns>
    ///   A reference to the message writer of the channel.
    ///   When no longer needed the message writer object has to be 
    ///   disposed using the IDisposable interface. 
    /// </returns>
    /// <remarks>
    ///   The VCI interfaces provide access to native driver resources. Because the 
    ///   .NET garbage collector is only designed to manage memory, but not 
    ///   native OS and driver resources the caller is responsible to release this 
    ///   resources via calling <c>IDisposable.Dispose()</c> when the object is no 
    ///   longer needed. Otherwise native memory and resource leaks may occure.
    /// </remarks>
    /// <exception cref="VciException">
    ///   Getting the message writer failed.
    /// </exception>
    /// <exception cref="ObjectDisposedException">
    ///   Object is already disposed or not initialized, yet.
    /// </exception>
    //*****************************************************************************
    ICanMessageWriter  GetMessageWriter( );

    //*****************************************************************************
    /// <summary>
    ///   This method initializes the CAN channel. This method must be called
    ///   prior to any other method of the interface.
    /// </summary>
    /// <param name="receiveFifoSize">
    ///   Size of the receive buffer (number of CAN messages)
    /// </param>
    /// <param name="transmitFifoSize">
    ///   Size of the transmit buffer (number  of CAN messages)
    /// </param>
    /// <param name="exclusive">
    ///   If this parameter is set to true the method tries
    ///   to initialize the channel for exclusive use. If set
    ///   to false, the method initializes the channel for
    ///   shared access.
    /// </param>
    /// <exception cref="VciException">
    ///   Channel initialization failed.
    /// </exception>
    /// <exception cref="ObjectDisposedException">
    ///   Object is already disposed.
    /// </exception>
    /// <remarks>
    ///   The channel is deactivated after this method returns an must be
    ///   activeted by an Activate() method call.
    ///   The method can be called more than once to reconfigure the size
    ///   of the receive or transmit FIFOs.
    /// </remarks>
    //*****************************************************************************
    void Initialize( ushort receiveFifoSize
                   , ushort transmitFifoSize
                   , bool   exclusive );

    //*****************************************************************************
    /// <summary>
    ///   This method activates the CAN channel. After activating the channel,
    ///   CAN messages can be transmitted and received through the message writer
    ///   and message reader.
    /// </summary>
    /// <exception cref="VciException">
    ///   Channel activation failed.
    /// </exception>
    /// <exception cref="ObjectDisposedException">
    ///   Object is already disposed or not initialized, yet.
    /// </exception>
    /// <remarks>
    ///   The CAN line must be started, otherwise no messages are received or
    ///   transmitted from/to the CAN line (see also ICanControl.StartLine).
    /// </remarks>
    //*****************************************************************************
    void Activate  ( );

    //*****************************************************************************
    /// <summary>
    ///   This method deactivates the CAN channel. After deactivating the channel,
    ///   no further CAN messages are transmitted or received to/from the CAN line.
    /// </summary>
    /// <exception cref="VciException">
    ///   Channel deactivation failed.
    /// </exception>
    /// <exception cref="ObjectDisposedException">
    ///   Object is already disposed or not initialized, yet.
    /// </exception>
    //*****************************************************************************
    void Deactivate( );
  };


}