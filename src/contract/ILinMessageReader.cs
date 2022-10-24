/*****************************************************************************
 IXXAT Automation GmbH
******************************************************************************
 Summary : Declarations for the LIN message reader class.
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
  using System.Threading;
  using System.ComponentModel;


  //*****************************************************************************
  /// <summary>
  ///   This interface represents a LIN message reader. It's used to read 
  ///   received LIN messages from a LIN monitor (see <c>ILinMonitor</c>).
  ///   When no longer needed the LIN message reader object has to be disposed 
  ///   using the IDisposable interface. 
  ///   A LIN message reader object can be got via method 
  ///   <c>ILinMonitor.GetMessageReader()</c>. 
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
  ///   // Initialize channel non-exclusively
  ///   monitor.Initialize(100, false);
  ///   
  ///   // Get the message reader
  ///   ILinMessageReader reader = monitor.GetMessageReader();
  ///   
  ///   // Use message reader
  ///   // ...
  ///   
  ///   // Dispose the objects
  ///   reader.Dispose();
  ///   monitor.Dispose();
  ///   bal.Dispose();
  ///   </code>
  /// </example>
  //*****************************************************************************
  public interface ILinMessageReader : IDisposable 
  {
    //*****************************************************************************
    /// <summary>
    ///   Gets the capacity of the receive FIFO in number of messages.
    /// </summary>
    //*****************************************************************************
    ushort Capacity  { get; }

    //*****************************************************************************
    /// <summary>
    ///   Gets the number of currently unread messages within the receive FIFO.
    /// </summary>
    //*****************************************************************************
    ushort FillCount { get; }

    //*****************************************************************************
    /// <summary>
    ///   Gets or sets the threshold for the trigger event. If the receive
    ///   FIFO contains at least the specified number of messages, the event
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
    ushort Threshold { get; 
                       set; }

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
    ///   This method reads a single message from the front of the
    ///   receive FIFO and remove the message from the FIFO.
    /// </summary>
    /// <param name="message">
    ///   Reference to a LinMessage where the method stores the read the message.
    /// </param>
    /// <returns>
    ///   true on success, false if no message is available to read.
    /// </returns>
    //*****************************************************************************
    bool ReadMessage ( out ILinMessage    message );

    //*****************************************************************************
    /// <summary>
    ///   This method reads multiple LIN messages from the front of the
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
    ///     ILinMessage[] msgArray;
    ///
    ///     do
    ///     {
    ///       // Wait 100 msec for a message reception
    ///       if (mRxEvent.WaitOne(100, false))
    ///       {
    ///         if (mReader.ReadMessages(out msgArray) > 0)
    ///         {
    ///           foreach (ILinMessage entry in msgArray)
    ///           {
    ///             PrintMessage(entry);
    ///           }
    ///         }
    ///       }
    ///     } while (0 == mMustQuit);
    ///   </code>
    /// </example>
    //*****************************************************************************
    int ReadMessages(out ILinMessage[] msgarray);
  };


}
