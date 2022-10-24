/*****************************************************************************
 IXXAT Automation GmbH
******************************************************************************
 Summary : Declarations for the CAN channel class.
 Compiler: Microsoft VC++ 8
******************************************************************************
 all rights reserved
*****************************************************************************/


/*************************************************************************
** namespace Ixxat.Vci4.Bal.Can
*************************************************************************/
namespace Ixxat.Vci4.Bal.Can 
{
  using System;

  //*****************************************************************************
  /// <summary>
  ///   Enumeration of flag values that are used to specify or signalize the 
  ///   operating mode of a CAN filter (see <c>CanChannel2</c>).
  /// </summary>
  //*****************************************************************************
  [Flags]
  public enum CanFilterModes : int
  {
    /// <summary>
    ///   Invalid or unknown filter mode (do not use for initialization)
    /// </summary>
    Invalid = 0x00, 
    /// <summary>
    ///   Lock filter (inhibit all IDs)
    /// </summary>
    Lock  = 0x01,  
    /// <summary>
    ///   Bypass filter (pass all IDs)
    /// </summary>
    Pass  = 0x02, 
    /// <summary>
    ///   Inclusive filtering (pass registered IDs)
    /// </summary>
    Inclusive  = 0x03,  
    /// <summary>
    ///   Exclusive filtering (inhibit registered IDs)
    /// </summary>
    Exclusive  = 0x04,
    /// <summary>
    ///   Pass self-reception messages from all channels
    /// </summary>
    PassSelfReceptions = 0x80,
  };


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
  ///   ICanChannel2 channel = bal.OpenSocket(0, typeof(ICanChannel2)) as ICanChannel2;
  ///   
  ///   // Initialize channel non-exclusively
  ///   channel.Initialize(100, 100, 100, CanFilterModes.Pass, false);
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
  public interface ICanChannel2 : ICanSocket2
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
    ICanMessageReader GetMessageReader();

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
    ICanMessageWriter GetMessageWriter();

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
    /// <param name="filterSize">
    ///   Size of the filter
    /// </param>
    /// <param name="filterMode">
    ///   Mode of the filter (see <c>CanFilterModes</c>)
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
    void Initialize(  ushort receiveFifoSize
                    , ushort transmitFifoSize
                    , uint   filterSize
                    , CanFilterModes filterMode
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

    //*****************************************************************************
    /// <summary>
    ///   This method returns the set filter mode for the given selection.
    /// </summary>
    /// <param name="bSelect">
    ///   Filter selection. This parameter can be either <c>CanFilter.Std</c>
    ///   to select the 11-bit acceptance filter, or <c>CanFilter.Ext</c> to
    ///   select the 29-bit acceptance filter.
    /// </param>
    /// <exception cref="ObjectDisposedException">
    ///   Object is already disposed or not initialized, yet.
    /// </exception>
    //*****************************************************************************
    byte  GetFilterMode(CanFilter   bSelect);

    //*****************************************************************************
    /// <summary>
    ///   This method sets the filter mode for the given selection.
    /// </summary>
    /// <param name="bSelect">
    ///   Filter selection. This parameter can be either <c>CanFilter.Std</c>
    ///   to select the 11-bit acceptance filter, or <c>CanFilter.Ext</c> to
    ///   select the 29-bit acceptance filter.
    /// </param>
    /// <param name="bMode">
    ///   Operating mode. This can be one of the following values:
    ///     <c>CanFilterModes.Lock</c> - lock filter (inhibit all IDs)
    ///     <c>CanFilterModes.Pass</c> - bypass filter (pass all IDs)
    ///     <c>CanFilterModes.Inclusive</c> - inclusive filtering (pass registered IDs)
    ///     <c>CanFilterModes.Exclusive</c> - exclusive filtering (inhibit registered IDs)
    /// </param>
    /// <returns></returns>
    /// <exception cref="ObjectDisposedException">
    ///   Object is already disposed or not initialized, yet.
    /// </exception>
    //*****************************************************************************
    byte  SetFilterMode(CanFilter      bSelect,
                        CanFilterModes bMode);

    //*****************************************************************************
    /// <summary>
    ///   This method sets the global acceptance filter. The global acceptance
    ///   filter enables the reception of CAN message identifiers specified by
    ///   the bit patterns passed in <paramref name="dwCode"/> and 
    ///   <paramref name="dwMask"/>. The message IDs enabled by this method are 
    ///   always accepted, even if the specified IDs are not registered within 
    ///   the filter list (see also <c>AddFilterIds</c>). The method can only be 
    ///    called if the CAN controller is in 'init' mode.  
    /// </summary>
    /// <param name="bSelect">
    ///   Filter selection. This parameter can be either <c>CanFilter.Std</c>
    ///   to select the 11-bit acceptance filter, or <c>CanFilter.Ext</c> to
    ///   select the 29-bit acceptance filter.
    /// </param>
    /// <param name="dwCode">
    ///   Acceptance code inclusive RTR bit. 
    /// </param>
    /// <param name="dwMask">
    ///   Mask that specifies the relevant bits within <paramref name="dwCode"/>. 
    ///   Relevant bits are specified by a 1 in the corresponding bit position,
    ///   non relevant bits are 0. 
    /// </param>
    /// <remarks>
    ///   The acceptance filter is defined by the acceptance code and acceptance 
    ///   mask. The bit pattern of CANIDs to be received are defined by the 
    ///   acceptance code. The corresponding acceptance mask allow to define 
    ///   certain bit positions to be don't care (bit x = 0). 
    /// </remarks>
    /// <example>
    ///   The values in <paramref name="dwCode"/> and <paramref name="dwMask"/> 
    ///   have the following format:
    ///   <code>
    ///   select = CanFilter.Std
    ///   
    ///        +----+----+----+----+ ~ +----+----+ ~ +---+---+---+---+
    ///    bit | 31 | 30 | 29 | 28 |   | 13 | 12 |   | 3 | 2 | 1 | 0 |
    ///        +----+----+----+----+ ~ +----+----+ ~ +---+---+---+---+
    ///        |  0 |  0 |  0 |  0 |   |  0 |ID11|   |ID2|ID1|ID0|RTR|
    ///        +----+----+----+----+ ~ +----+----+ ~ +---+---+---+---+
    ///   
    ///   select = CanFilter.Ext
    ///   
    ///        +----+----+----+----+ ~ +----+----+ ~ +---+---+---+---+
    ///    bit | 31 | 30 | 29 | 28 |   | 13 | 12 |   | 3 | 2 | 1 | 0 |
    ///        +----+----+----+----+ ~ +----+----+ ~ +---+---+---+---+
    ///        |  0 |  0 |ID28|ID27|   |ID12|ID11|   |ID2|ID1|ID0|RTR|
    ///        +----+----+----+----+ ~ +----+----+ ~ +---+---+---+---+
    ///   </code>  
    ///   The following example demonstates how to compute the 
    ///   <paramref name="dwCode"/> and <paramref name="dwMask"/> values to enable 
    ///   the standard IDs in the range from 0x100 to 0x103 whereas RTR is 0.
    ///   <code>
    ///    dwCode   = 001 0000 0000 0
    ///    dwMask   = 111 1111 1100 1
    ///    result = 001 0000 00xx 0
    ///   
    ///    enabled IDs:
    ///             001 0000 0000 0 (0x100, RTR = 0)
    ///             001 0000 0001 0 (0x101, RTR = 0)
    ///             001 0000 0010 0 (0x102, RTR = 0)
    ///             001 0000 0011 0 (0x103, RTR = 0)
    ///   </code>
    /// </example>
    /// <exception cref="VciException">
    ///   Setting acceptance filter failed.
    /// </exception>
    /// <exception cref="ObjectDisposedException">
    ///   Object is already disposed.  
    /// </exception>
    //*****************************************************************************
    void SetAccFilter(CanFilter   bSelect,
                      uint        dwCode,
                      uint        dwMask);

    //*****************************************************************************
    /// <summary>
    ///   This method registers the specified CAN message identifier or group
    ///   of identifiers at the specified filter list. IDs registered within the
    ///   filter list are accepted for reception. The method can only be called 
    ///   if the CAN controller is in 'init' mode.
    /// </summary>
    /// <param name="bSelect">
    ///   Filter selection. This parameter can be either <c>CanFilter.Std</c>
    ///   to select the 11-bit filter list, or <c>CanFilter.Ext</c> to
    ///   select the 29-bit filter list.
    /// </param>
    /// <param name="dwCode">
    ///   Message identifier (inclusive RTR) to add to the filter list.  
    /// </param>
    /// <param name="dwMask">
    ///   Mask that specifies the relevant bits within <paramref name="dwCode"/>. 
    ///   Relevant bits are specified by a 1 in the corresponding bit position,
    ///   non relevant bits are 0. 
    /// </param>
    /// <example>
    ///   The following example demonstates how to compute the 
    ///   <paramref name="dwCode"/> and <paramref name="dwMask"/> values to register 
    ///   the standard IDs in the range from 0x518 to 0x51B whereas RTR is 1.
    ///   <code>
    ///     dwCode   = 0101 0001 1000 1
    ///     dwMask   = 0111 1111 1100 1
    ///     result = 0101 0001 10xx 1
    /// 
    ///     IDs registered by this method:
    ///              0101 0001 1000 1 (0x518, RTR = 1)
    ///              0101 0001 1001 1 (0x519, RTR = 1)
    ///              0101 0001 1010 1 (0x51A, RTR = 1)
    ///              0101 0001 1011 1 (0x51B, RTR = 1)
    ///   </code>
    /// </example>
    /// <exception cref="VciException">
    ///   Registering filter Ids failed.  
    /// </exception>
    /// <exception cref="ObjectDisposedException">
    ///   Object is already disposed.  
    /// </exception>
    //*****************************************************************************
    void AddFilterIds(CanFilter  bSelect,
                      uint       dwCode,
                      uint       dwMask);

    //*****************************************************************************
    /// <summary>
    ///   This method removes the specified CAN message identifier or group
    ///   of identifiers from the specified filter list. The method can only be
    ///   called if the CAN controller is in 'init' mode.
    /// </summary>
    /// <param name="bSelect">
    ///   Filter selection. This parameter can be either <c>CanFilter.Std</c>
    ///   to select the 11-bit filter list, or <c>CanFilter.Ext</c> to
    ///   select the 29-bit filter list.
    /// </param>
    /// <param name="dwCode">
    ///   Message identifier (inclusive RTR) to remove from the filter list. 
    /// </param>
    /// <param name="dwMask">
    ///   Mask that specifies the relevant bits within <paramref name="dwCode"/>. 
    ///   Relevant bits are specified by a 1 in the corresponding bit position,
    ///   non relevant bits are 0. 
    /// </param>
    /// <example>
    ///   The following example demonstates how to compute the 
    ///   <paramref name="dwCode"/> and <paramref name="dwMask"/> values to remove 
    ///   the standard IDs in the range from 0x518 to 0x51B whereas RTR is 1.
    ///   <code>
    ///     dwCode   = 0101 0001 1000 1
    ///     dwMask   = 0111 1111 1100 1
    ///     result = 0101 0001 10xx 1
    /// 
    ///     IDs removed by this method:
    ///              0101 0001 1000 1 (0x518, RTR = 1)
    ///              0101 0001 1001 1 (0x519, RTR = 1)
    ///              0101 0001 1010 1 (0x51A, RTR = 1)
    ///              0101 0001 1011 1 (0x51B, RTR = 1)
    ///   </code>
    /// </example>
    /// <exception cref="VciException">
    ///   Removing filter Ids failed.  
    /// </exception>
    /// <exception cref="ObjectDisposedException">
    ///   Object is already disposed.  
    /// </exception>
    //*****************************************************************************
    void RemFilterIds(CanFilter  bSelect,
                      uint       dwCode,
                      uint       dwMask);
  };


}