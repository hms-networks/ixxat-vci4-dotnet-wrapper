// SPDX-License-Identifier: MIT
//----------------------------------------------------------------------------
// Summary  : Declarations for the CAN message scheduler class.
// Copyright: Copyright (C) 2016-2022 HMS Technology Center Ravensburg GmbH, 
//            all rights reserved
//----------------------------------------------------------------------------

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
  public struct mgdCANCYCLICTXMSG2
  {
    ///<summary>cycle time for the message in ticks</summary>
    public ushort         wCycleTime;
    ///<summary>auto increment mode (see CAN_CTXMSG_INC_ const)</summary>
    public byte           bIncrMode;
    ///<summary>index of the byte within abData[] to increment</summary>
    public byte           bByteIndex; 
    ///<summary>CAN message identifier (INTEL format)</summary>
    public uint           dwMsgId;
    ///<summary>message information (bit field)</summary>
    public mgdCANMSGINFO  uMsgInfo;
    ///<summary>message data</summary>
    // (UINT8[8] is native declaration and
    // array<UINT8> is not what we want !!)
    public byte           bData1;
    public byte           bData2;
    public byte           bData3;
    public byte           bData4;
    public byte           bData5;
    public byte           bData6;
    public byte           bData7;
    public byte           bData8;
    public byte           bData9;
    public byte           bData10;
    public byte           bData11;
    public byte           bData12;
    public byte           bData13;
    public byte           bData14;
    public byte           bData15;
    public byte           bData16;
    public byte           bData17;
    public byte           bData18;
    public byte           bData19;
    public byte           bData20;
    public byte           bData21;
    public byte           bData22;
    public byte           bData23;
    public byte           bData24;
    public byte           bData25;
    public byte           bData26;
    public byte           bData27;
    public byte           bData28;
    public byte           bData29;
    public byte           bData30;
    public byte           bData31;
    public byte           bData32;
    public byte           bData33;
    public byte           bData34;
    public byte           bData35;
    public byte           bData36;
    public byte           bData37;
    public byte           bData38;
    public byte           bData39;
    public byte           bData40;
    public byte           bData41;
    public byte           bData42;
    public byte           bData43;
    public byte           bData44;
    public byte           bData45;
    public byte           bData46;
    public byte           bData47;
    public byte           bData48;
    public byte           bData49;
    public byte           bData50;
    public byte           bData51;
    public byte           bData52;
    public byte           bData53;
    public byte           bData54;
    public byte           bData55;
    public byte           bData56;
    public byte           bData57;
    public byte           bData58;
    public byte           bData59;
    public byte           bData60;
    public byte           bData61;
    public byte           bData62;
    public byte           bData63;
    public byte           bData64;
  };

  // enable: CS1591: Missing XML comment for publicly visible type or member
#pragma warning restore 1591


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
  ///   ICanScheduler2 scheduler = bal.OpenSocket(0, typeof(ICanScheduler2)) as ICanScheduler2;
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
  public interface ICanScheduler2 : ICanSocket2
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
    ICanCyclicTXMsg2 AddMessage( );
  };

  //*****************************************************************************
  /// <summary>
  ///   This class represents a cyclic CAN transmit message. 
  ///   An instance of CanCyclicTXMsg2 has to be added to a CAN scheduler
  ///   (see <c>ICanScheduler2</c>) before it's transmission can be started.
  /// </summary>
  /// <example>
  ///   <code>
  ///   IBalObject bal = ...
  ///   // Open scheduler on first CAN socket
  ///   ICanScheduler2 scheduler = bal.OpenSocket(0, typeof(ICanScheduler2)) as ICanScheduler2;
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
  public interface ICanCyclicTXMsg2 : ICanMessage2
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