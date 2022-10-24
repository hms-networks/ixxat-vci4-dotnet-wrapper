/*****************************************************************************
 IXXAT Automation GmbH
******************************************************************************
 Summary : Declarations for the CAN message reader class.
 Compiler: Microsoft VC++ 8
******************************************************************************
 all rights reserved
*****************************************************************************/


/*************************************************************************
** namespace Ixxat.Vci4.Bal.Can
*************************************************************************/
namespace Ixxat.Vci4.Bal.Can 
{
  /*************************************************************************
  ** used namespace
  *************************************************************************/
  using System;
  using System.ComponentModel;
  using System.Threading;


  //*****************************************************************************
  /// <summary>
  ///   This interface represents a CAN message reader. It's used to read 
  ///   received CAN messages from a CAN communication channel 
  ///   (see <c>ICanChannel</c>).
  ///   When no longer needed the CAN message reader object has to be disposed 
  ///   using the IDisposable interface. 
  ///   A CAN message reader object can be got via method 
  ///   <c>ICanChannel.GetMessageReader()</c>. 
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
  ///   // Get the message reader
  ///   ICanMessageReader reader = channel.GetMessageReader();
  ///   
  ///   // Use message reader
  ///   // ...
  ///   
  ///   // Dispose the objects
  ///   reader.Dispose();
  ///   channel.Dispose();
  ///   bal.Dispose();
  ///   </code>
  /// </example>
  //*****************************************************************************
  public interface ICanMessageReader : IDisposable 
  {
    //*****************************************************************************
    /// <summary>
    ///   Gets the capacity of the receive FIFO in number of CAN messages.
    /// </summary>
    //*****************************************************************************
    ushort Capacity  { get; }

    //*****************************************************************************
    /// <summary>
    ///   Gets the number of currently unread CAN messages within the receive FIFO.
    /// </summary>
    //*****************************************************************************
    ushort FillCount { get; }

    //*****************************************************************************
    /// <summary>
    ///   Gets or sets the threshold for the trigger event. If the receive
    ///   FIFO contains at least the specified number of CAN messages, the event
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
    ///   This method assigns an event object to the message reader. The event
    ///   is set to the signaled state if the number of available messages within
    ///   the receive FIFO exceed the currently set receive threshold.
    /// </summary>
    /// <param name="fifoEvent">
    ///   The event object which is to be set if the number of available 
    ///   messages within the receive FIFO exceed the currently set receive 
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
    ///   This method assigns an event object to the message reader. The event
    ///   is set to the signaled state if the number of available messages within
    ///   the receive FIFO exceed the currently set receive threshold.
    /// </summary>
    /// <param name="fifoEvent">
    ///   The event object which is to be set if the number of available 
    ///   messages within the receive FIFO exceed the currently set receive 
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
    ///   This method reads a single CAN message from the front of the
    ///   receive FIFO and remove the message from the FIFO.
    /// </summary>
    /// <param name="message">
    ///   Reference to a CanMessage where the method stores the read the message.
    /// </param>
    /// <returns>
    ///   true on success, false if no message is available to read.
    /// </returns>
    //*****************************************************************************
    bool ReadMessage ( out ICanMessage    message );

    //*****************************************************************************
    /// <summary>
    ///   This method reads a single CAN message from the front of the
    ///   receive FIFO and remove the message from the FIFO.
    /// </summary>
    /// <param name="message">
    ///   Reference to a CanMessage where the method stores the read the message.
    /// </param>
    /// <returns>
    ///   true on success, false if no message is available to read.
    /// </returns>
    //*****************************************************************************
    bool ReadMessage(out ICanMessage2 message);

    //*****************************************************************************
    /// <summary>
    ///   This method reads multiple CAN messages from the front of the
    ///   receive FIFO and removes the messages from the FIFO.
    /// </summary>
    /// <param name="msgarray">
    ///   Reference to a array to store messages into.
    /// </param>
    /// <returns>
    ///   number of messages read.
    /// </returns>
    /// <example>
    ///   <code>
    ///     ICanMessage[] msgArray;
    ///
    ///     do
    ///     {
    ///       // Wait 100 msec for a message reception
    ///       if (mRxEvent.WaitOne(100, false))
    ///       {
    ///         if (mReader.ReadMessages(out msgArray) > 0)
    ///         {
    ///           foreach (ICanMessage entry in msgArray)
    ///           {
    ///             PrintMessage(entry);
    ///           }
    ///         }
    ///       }
    ///     } while (0 == mMustQuit);
    ///   </code>
    /// </example>
    //*****************************************************************************
    int ReadMessages(out ICanMessage[] msgarray);

    //*****************************************************************************
    /// <summary>
    ///   This method reads multiple CAN messages from the front of the
    ///   receive FIFO and removes the messages from the FIFO.
    /// </summary>
    /// <param name="msgarray">
    ///   Reference to a array to store messages into.
    /// </param>
    /// <returns>
    ///   number of messages read.
    /// </returns>
    /// <example>
    ///   <code>
    ///     ICanMessage2[] msgArray;
    ///
    ///     do
    ///     {
    ///       // Wait 100 msec for a message reception
    ///       if (mRxEvent.WaitOne(100, false))
    ///       {
    ///         if (mReader.ReadMessages(out msgArray) > 0)
    ///         {
    ///           foreach (ICanMessage2 entry in msgArray)
    ///           {
    ///             PrintMessage(entry);
    ///           }
    ///         }
    ///       }
    ///     } while (0 == mMustQuit);
    ///   </code>
    /// </example>
    //*****************************************************************************
    int ReadMessages(out ICanMessage2[] msgarray);
  };


}