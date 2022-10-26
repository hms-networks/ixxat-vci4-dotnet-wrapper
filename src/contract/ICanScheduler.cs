/*****************************************************************************
 IXXAT Automation GmbH
******************************************************************************
 Summary : Declarations for the CAN message scheduler class.
 Compiler: Microsoft VC++ 8
******************************************************************************
 all rights reserved
*****************************************************************************/

namespace Ixxat.Vci4.Bal.Can 
{
  using System;
  using System.Reflection;
  using System.Runtime.InteropServices;


  // disable: CS1591: Missing XML comment for publicly visible type or member
#pragma warning disable 1591

  //*****************************************************************************
  /// <summary>
  ///   Managed image of native struct CANCYCLICTXMSG2.
  /// </summary>
  //*****************************************************************************
  [StructLayout(LayoutKind.Sequential)]
  public struct mgdCANCYCLICTXMSG
  {
    ///<summary>cycle time for the message in ticks</summary>
    public ushort wCycleTime;
    ///<summary>auto increment mode (see CAN_CTXMSG_INC_ const)</summary>
    public byte bIncrMode;
    ///<summary>index of the byte within abData[] to increment</summary>
    public byte bByteIndex; 
    ///<summary>CAN message identifier (INTEL format)</summary>
    public uint dwMsgId;
    ///<summary>message information (bit field)</summary>
    public mgdCANMSGINFO uMsgInfo;
    ///<summary>message data</summary>
    // (UINT8[8] is native declaration and
    // array<UINT8> is not what we want !!)
    public byte bData1;
    public byte bData2;
    public byte bData3;
    public byte bData4;
    public byte bData5;
    public byte bData6;
    public byte bData7;
    public byte bData8;
  };

  // enable: CS1591: Missing XML comment for publicly visible type or member
#pragma warning restore 1591

  //*****************************************************************************
  /// <summary>
  ///   Enumeration of modes for cyclic CAN transmit messages.
  /// </summary>
  //*****************************************************************************
  public enum CanCyclicTXIncMode : int
  {
    /// <summary>
    ///   No automatic increment of a message field occurs.
    /// </summary>
    NoInc = 0x00, 
    /// <summary>
    ///   Increments the CAN ID of the message after every transmit process by 1. 
    ///   If the CAN ID reaches the value 2048 (11-bit ID) or 536.870.912 (29-bit ID), 
    ///   there is an automatic overrun to 0.
    /// </summary>
    IncId = 0x01,  
    /// <summary>
    ///   Increments an 8-bit value in the data field of the message. The data field 
    ///   to be incremented is defined in the proerty <c>AutoIncrementIndex</c>. If 
    ///   the maximum value 255 is exceeded, there is an automatic overrun to 0.
    /// </summary>
    Inc8  = 0x02,  
    /// <summary>
    ///   Increments a 16-bit value in the data field of the message. The least 
    ///   significant byte of the 16-bit value to be incremented is defined in the 
    ///   property <c>AutoIncrementIndex</c>. The most significant byte is in 
    ///   Data[AutoIncrementIndex+1]. If the maximum value 65535 is exceeded, there 
    ///   is an automatic overrun to 0.
    /// </summary>
    Inc16 = 0x03   
  };


  //*****************************************************************************
  /// <summary>
  ///   Enumeration of values that indicate the status of a cyclic CAN 
  ///   transmit message.
  /// </summary>
  //*****************************************************************************
  public enum CanCyclicTXStatus : int
  {
    /// <summary>
    ///   The message entry is empty
    /// </summary>
    Empty = 0x00, 
    /// <summary>
    ///   Processing is in progress
    /// </summary>
    Busy  = 0x01,  
    /// <summary>
    ///   Processing has completed
    /// </summary>
    Done  = 0x02    
  };


  //*****************************************************************************
  /// <summary>
  ///   This interface represents a CAN scheduler. A CAN scheduler provides the
  ///   functionality to cyclically transmit CAN messages. Optionally such a 
  ///   transmit message automatically be altered form one trasmission to the
  ///   next (identifier or a piece of data).
  ///   The CAN scheduler provides methods to establish, start and stop cyclic 
  ///   transmit messages for a CAN line.
  ///   When no longer needed the CAN scheduler object has to be disposed using 
  ///   the IDisposable interface. 
  ///   A CAN scheduler object can be got via method <c>IBalObject.OpenSocket()</c>. 
  ///   The CAN scheduler cannot be opened twice at the same time. Therefore a
  ///   second try to open the CAN scheduler via <c>IBalObject.OpenSocket()</c>
  ///   fails until the successfully opened CAN scheduler object is explicitly
  ///   disposed.
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
  ///   // Open scheduler on first CAN socket
  ///   ICanScheduler scheduler = bal.OpenSocket(0, typeof(ICanScheduler2)) as ICanScheduler;
  ///   
  ///   // Create and init cyclic transmit message
  ///   ICanCyclicTXMsg message;
  ///   message = scheduler.AddMessage();
  ///   
  ///   message.Identifier         = 0x100;
  ///   message.DataLength         = 1;
  ///   message[0]                 = 0xAF;
  ///   message.AutoIncrementMode  = CanCyclicTXIncMode.Inc8;
  ///   message.AutoIncrementIndex = 0;
  ///   message.CycleTicks = 1;
  ///   
  ///   // Start endless transmission
  ///   message.Start(0);
  ///   
  ///   //...
  ///
  ///   // Dispose scheduler
  ///   scheduler.Dispose();
  ///   bal.Dispose();
  ///   </code>
  /// </example>
  //*****************************************************************************
  public interface ICanScheduler : ICanSocket
  {
    //*****************************************************************************
    /// <summary>
    ///   This method suspends execution of the scheduler and stops processing
    ///   of all currently registered messages.
    /// </summary>
    /// <exception cref="VciException">
    ///   Suspending scheduler failed.
    /// </exception>
    /// <exception cref="ObjectDisposedException">
    ///   Object is already disposed.
    /// </exception>
    //*****************************************************************************
    void Suspend     ( );

    //*****************************************************************************
    /// <summary>
    ///   This method resumes execution of the scheduler and starts processing
    ///   of all currently registered message.
    /// </summary>
    /// <exception cref="VciException">
    ///   Resuming scheduler failed.
    /// </exception>
    /// <exception cref="ObjectDisposedException">
    ///   Object is already disposed.
    /// </exception>
    //*****************************************************************************
    void Resume      ( );

    //*****************************************************************************
    /// <summary>
    ///   This method suspends execution of the scheduler and removes all
    ///   currently registered messages.
    /// </summary>
    /// <exception cref="VciException">
    ///   Resetting scheduler failed.
    /// </exception>
    /// <exception cref="ObjectDisposedException">
    ///   Object is already disposed.
    /// </exception>
    //*****************************************************************************
    void Reset       ( );

    //*****************************************************************************
    /// <summary>
    ///   This method updates the status of the scheduler and all currently
    ///   registered messages.
    /// </summary>
    //*****************************************************************************
    void UpdateStatus( );

    //*****************************************************************************
    /// <summary>
    ///   This method adds a new cyclic transmit message to the scheduler.
    /// </summary>
    /// <result>
    ///   Reference to the added cyclic transmit message.
    /// </result>
    /// <exception cref="VciException">
    ///   Adding the cyclic transmit message to the scheduler failed.
    /// </exception>
    /// <exception cref="ObjectDisposedException">
    ///   Object is already disposed.
    /// </exception>
    /// <exception cref="IndexOutOfRangeException">
    ///   The transmit object cannot be registered at the scheduler because
    ///   the maximum number of supported transmit object has already been reached.
    /// </exception>
    /// <exception cref="ArgumentException">
    ///   The specified transmit object is a null reference.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///   The specified transmit object is already registered at a scheduler.
    /// </exception>
    /// <remarks>
    ///   The method only adds messages with <c>FrameType</c> set to 
    ///   <c>CanMsgFrameType.Data</c>.
    /// </remarks>
    //*****************************************************************************
    ICanCyclicTXMsg AddMessage();
  };



  //*****************************************************************************
  /// <summary>
  ///   This class represents a cyclic CAN transmit message. 
  ///   An instance of CanCyclicTXMsg has to be added to a CAN scheduler
  ///   (see <c>ICanScheduler</c>) before it's transmission can be started.
  /// </summary>
  /// <example>
  ///   <code>
  ///   IBalObject bal = ...
  ///   // Open scheduler on first CAN socket
  ///   ICanScheduler scheduler = bal.OpenSocket(0, typeof(ICanScheduler2)) as ICanScheduler;
  ///   
  ///   // Create and init cyclic transmit message
  ///   ICanCyclicTXMsg message;
  ///   message = scheduler.AddMessage();
  ///   
  ///   message.Identifier         = 0x100;
  ///   message.DataLength         = 1;
  ///   message[0]                 = 0xAF;
  ///   message.AutoIncrementMode  = CanCyclicTXIncMode.Inc8;
  ///   message.AutoIncrementIndex = 0;
  ///   message.CycleTicks = 1;
  ///   
  ///   // Start endless transmission
  ///   message.Start(0);
  ///   
  ///   //...
  ///
  ///   // Dispose scheduler
  ///   scheduler.Dispose();
  ///   bal.Dispose();
  ///  </code>
  /// </example>
  //*****************************************************************************
  public interface ICanCyclicTXMsg : ICanMessage
  {
    //--------------------------------------------------------------------
    // properties
    //--------------------------------------------------------------------

    //*****************************************************************************
    /// <summary>
    ///   Gets the current status of this cyclic CAN message.
    /// </summary>
    /// <returns>
    ///   The current status of this cyclic CAN transmit message.
    /// </returns>
    //*****************************************************************************
    CanCyclicTXStatus Status                     { get; }

    //*****************************************************************************
    /// <summary>
    ///   Gets or Sets the cycle time of this cyclic CAN transmit message in :
    ///   number of clock ticks (See <c>ICanSocket.ClockFrequency</c> and 
    ///   <c>ICanSocket.CyclicMessageTimerDivisor</c>). Clock ticks have a valid 
    ///   range of 1 to <c>ICanSocket.MaxCyclicMessageTicks</c>.
    ///   The cycle time can be calculated in accordance with the following formula:
    ///   <code>
    ///     cycle time [s] = (CyclicMessageTimerDivisor / ClockFrequency) * CycleTime 
    ///   </code>
    /// </summary>
    /// <remarks>
    ///   The contents of a cyclic CAN transmit message can only be changed
    ///   as long as the message is not registered at the scheduler. A call
    ///   of this method is silently ignored if the message is currently
    ///   registered at the scheduler.
    /// </remarks>
    //*****************************************************************************
    ushort      CycleTicks                       { get; 
                                                   set; }

    //*****************************************************************************
    /// <summary>
    ///   Gets or sets the auto-increment mode of this cyclic CAN transmit message.
    ///   Auto-increment mode. This can be one of the following constants:
    ///     <c>CanCyclicTXIncMode.NoInc</c> - no auto-increment
    ///     <c>CanCyclicTXIncMode.IncId</c> - auto-increment the CAN message identifier
    ///     <c>CanCyclicTXIncMode.Inc8</c>  - auto-increment a 8-bit data field
    ///     <c>CanCyclicTXIncMode.Inc16</c> - auto-increment a 16-bit data field
    /// </summary>
    /// <remarks>
    ///   If <c>AutoIncrementMode</c> is set to either 
    ///   <c>CanCyclicTXIncMode.Inc8</c> or <c>CanCyclicTXIncMode.Inc16</c>, 
    ///   the <c>AutoIncrementIndex</c> property specifies the index of the first 
    ///   byte within the CAN message to be auto-incremented.
    ///   The contents of a cyclic CAN transmit message can be only changed
    ///   as long as the message is not registered at the scheduler. A call
    ///   of this method is silently ignored if the message is currently
    ///   registered at the scheduler.
    /// </remarks>
    //*****************************************************************************
    CanCyclicTXIncMode AutoIncrementMode         { get; 
                                                   set; }

    //*****************************************************************************
    /// <summary>
    ///   Gets or sets the index of the auto-incremented data field of this cyclic 
    ///   CAN transmit message.
    /// </summary>
    /// <remarks>
    ///   If <c>AutoIncrementMode</c> is set to <c>CanCyclicTXIncMode.Inc8</c> the 
    ///   property <propref name="AutoIncrementIndex"/> specifies the byte within the data 
    ///   field which is auto-incremented after each transmission of the CAN message. 
    ///   If <c>AutoIncrementMode</c> is set to <c>CanCyclicTXIncMode.Inc16</c>
    ///   the property <propref name="AutoIncrementIndex"/> specifies the least significant 
    ///   byte within the data field which is auto-incremented after each 
    ///   transmission of the CAN message. The most significant byte of the 
    ///   auto-incremented data field is at <c>Data</c>[Index+1].
    ///   The contents of a cyclic CAN transmit message can be only changed
    ///   as long as the message is not registered at the scheduler. A call
    ///   of this method is silently ignored if the message is currently
    ///   registered at the scheduler.
    /// </remarks>
    /// <exception cref="ArgumentOutOfRangeException">
    ///   The specified data index is out of range.
    /// </exception>
    //*****************************************************************************
    byte        AutoIncrementIndex               { get; 
                                                   set; }

    //--------------------------------------------------------------------
    // methods
    //--------------------------------------------------------------------

    //*****************************************************************************
    /// <summary>
    ///   Starts a cyclic message
    /// </summary>
    /// <param name="repeatCount">Number of repeats. Zero repeats endless.</param>
    //*****************************************************************************
    void Start(ushort repeatCount);

    //*****************************************************************************
    /// <summary>
    ///   Stops a cyclic message
    /// </summary>
    //*****************************************************************************
    void Stop();

    //*****************************************************************************
    /// <summary>
    ///   Resets a cyclic message
    /// </summary>
    //*****************************************************************************
    void Reset();
  };


}