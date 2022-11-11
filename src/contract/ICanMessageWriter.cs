// SPDX-License-Identifier: MIT
//----------------------------------------------------------------------------
// Summary  : Declarations for the CAN message writer class.
// Copyright: Copyright (C) 2016-2022 HMS Technology Center Ravensburg GmbH, 
//            all rights reserved
//----------------------------------------------------------------------------

namespace Ixxat.Vci4.Bal.Can 
{
  using System;
  using System.ComponentModel;
  using System.Threading;


  //*****************************************************************************
  /// <summary>
  ///   This interface represents a CAN message writer. It's used to write 
  ///   CAN messages to a CAN communication channel for transmission.
  ///   (see <c>ICanChannel</c>).
  ///   When no longer needed the CAN message writer object has to be disposed 
  ///   using the IDisposable interface. 
  ///   A CAN message writer object can be got via method 
  ///   <c>ICanChannel.GetMessageWriter()</c>. 
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
  ///   // Get the message writer
  ///   ICanMessageWriter writer = channel.GetMessageWriter();
  ///   
  ///   // Use message writer
  ///   // ...
  ///   
  ///   // Dispose the objects
  ///   writer.Dispose();
  ///   channel.Dispose();
  ///   bal.Dispose();
  ///   </code>
  /// </example>
  //*****************************************************************************
  public interface ICanMessageWriter : IDisposable
  {
    //*****************************************************************************
    /// <summary>
    ///   Gets the capacity of the transmit FIFO in number of CAN messages.
    /// </summary>
    //*****************************************************************************
    ushort Capacity  { get; }

    //*****************************************************************************
    /// <summary>
    ///   Gets the number of currently free CAN messages within the transmit FIFO.
    /// </summary>
    //*****************************************************************************
    ushort FreeCount { get; }

    //*****************************************************************************
    /// <summary>
    ///   Gets or sets the threshold for the trigger event. If the transmit
    ///   FIFO contains at least the specified number of free entries, the event
    ///   specified by a <c>AssignEvent</c> method call is set to the signaled 
    ///   state.
    /// </summary>
    /// <exception cref="VciException">
    ///   Setting Threshold failed.
    /// </exception>
    /// <exception cref="ObjectDisposedException">
    ///   Object is already disposed.
    /// </exception>
    //*****************************************************************************
    ushort Threshold { get; set; }

    //*****************************************************************************
    /// <summary>
    ///   This method locks the access to the FIFO. 
    ///   Use the Lock()/Unlock() pair if you access the FIFO 
    ///   from different threads.
    /// </summary>
    /// <exception cref="VciException">
    ///   Lock failed.
    /// </exception>
    /// <exception cref="ObjectDisposedException">
    ///   Object is already disposed.
    /// </exception>
    //*****************************************************************************
    void Lock();

    //*****************************************************************************
    /// <summary>
    ///   This method releases the access to the FIFO. 
    ///   Use the Lock()/Unlock() pair if you access the FIFO 
    ///   from different threads.
    /// </summary>
    /// <exception cref="VciException">
    ///   Unlock failed.
    /// </exception>
    /// <exception cref="ObjectDisposedException">
    ///   Object is already disposed.
    /// </exception>
    //*****************************************************************************
    void Unlock();

    //*****************************************************************************
    /// <summary>
    ///   This method assigns an event object to the message writer. The event
    ///   is set to the signaled state if the number free entries within the 
    ///   transmit FIFO reaches or exceed the currently set threshold.
    /// </summary>
    /// <param name="fifoEvent">
    ///   The event object which is set to the signaled state if the number free 
    ///   entries within the transmit FIFO reaches or exceed the currently set 
    ///   threshold.
    /// </param>
    /// <exception cref="VciException">
    ///   Assigning the event failed.
    /// </exception>
    /// <exception cref="ObjectDisposedException">
    ///   Object is already disposed.
    /// </exception>
    //*****************************************************************************
    void AssignEvent ( AutoResetEvent      fifoEvent );

    //*****************************************************************************
    /// <summary>
    ///   This method assigns an event object to the message writer. The event
    ///   is set to the signaled state if the number free entries within the 
    ///   transmit FIFO reaches or exceed the currently set threshold.
    /// </summary>
    /// <param name="fifoEvent">
    ///   The event object which is set to the signaled state if the number free 
    ///   entries within the transmit FIFO reaches or exceed the currently set 
    ///   threshold.
    /// </param>
    /// <exception cref="VciException">
    ///   Assigning the event failed.
    /// </exception>
    /// <exception cref="ObjectDisposedException">
    ///   Object is already disposed.
    /// </exception>
    //*****************************************************************************
    void AssignEvent ( ManualResetEvent    fifoEvent );

    //*****************************************************************************
    /// <summary>
    ///   This method places a single CAN message at the end of the
    ///   transmit FIFO and returns without waiting for the message to
    ///   be transmitted.
    /// </summary>
    /// <param name="message">
    ///   The CanMessage to send.
    /// </param>
    /// <returns>
    ///   If the method succeeds it returns true. The method returns false
    ///   if there is not enought free space available within the transmit FIFO
    ///   to add the message.
    /// </returns>
    //*****************************************************************************
    bool SendMessage ( ICanMessage          message );

    //*****************************************************************************
    /// <summary>
    ///   This method places a single CAN message at the end of the
    ///   transmit FIFO and returns without waiting for the message to
    ///   be transmitted.
    /// </summary>
    /// <param name="message">
    ///   The CanMessage to send.
    /// </param>
    /// <returns>
    ///   If the method succeeds it returns true. The method returns false
    ///   if there is not enought free space available within the transmit FIFO
    ///   to add the message.
    /// </returns>
    //*****************************************************************************
    bool SendMessage(ICanMessage2 message);
  };


}