/*****************************************************************************
 IXXAT Automation GmbH
******************************************************************************
 Summary : Declarations for the CAN control class.
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
  ///   This interface represents a CAN control unit and is used to control a
  ///   CAN line. Controlling consists of initialisation, starting/stoping 
  ///   the CAN line and adjusting filter settings.
  ///   When no longer needed the CAN contol object has to be disposed using the 
  ///   IDisposable interface. 
  ///   A CAN control object can be got via method <c>IBalObject.OpenSocket()</c>. 
  ///   The CAN control cannot be opened twice at the same time. Therefore a
  ///   second try to open the CAN control via <c>IBalObject.OpenSocket()</c>
  ///   fails until the successfully opened CAN control object is explicitly
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
  ///   // Open communication channel on first CAN socket
  ///   ICanControl control = bal.OpenSocket(0, typeof(ICanControl)) as ICanControl;
  ///   
  ///   // Initialize CAN line
  ///   control.InitLine(CanOperatingModes.Standard, CanBitrate.Cia250KBit);
  ///   
  ///   // Use CAN line
  ///   // ...
  ///   
  ///   // Dispose control and BAL
  ///   control.Dispose();
  ///   bal.Dispose();
  ///   </code>
  /// </example>
  //*****************************************************************************
  public interface ICanControl : ICanSocket
  {
    //*****************************************************************************
    /// <summary>
    ///   This method detects the actual bit rate of the CAN line to which the
    ///   controller is connected.
    /// </summary>
    /// <param name="timeout">
    ///   Timeout in milliseconds to wait between two successive receive messages.
    /// </param>
    /// <param name="bitrateTable">
    ///   One-dimensional array of initialized CanBitrate objects
    ///   which contains possible values for the bit timing register
    ///   to be tested.
    /// </param>
    /// <returns>
    ///   If the method succeeds it returns the index of the detected CanBitrate
    ///   entry within the specified array.
    ///   If the method detects no baud rate it returns -1.
    /// </returns>
    /// <remarks>
    ///   The method detects the actual bit rate beginning at the first entry
    ///   within the specified array and switches to the next entry until the
    ///   correct baud rate is detected or the table limit is reached. If the
    ///   time between two successive receive messages exceed the value specified
    ///   by the <paramref name="timeout"/> parameter, the method throws a 
    ///   <c>VciException</c>.
    ///   The total execution time of the method can be determined by the
    ///   following formula:
    ///   TotalExecutionTime [ms] = <paramref name="timeout"/> * <paramref name="bitrateTable"/>.Length
    /// </remarks>
    /// <exception cref="VciException">
    ///   VCI_E_TIMEOUT: Time between two successive receive messages exceed the value specified
    ///   by the <paramref name="timeout"/> parameter.
    ///   otherwise: see error message
    /// </exception>
    //*****************************************************************************
    int DetectBaud(ushort timeout, CanBitrate[] bitrateTable);

    //*****************************************************************************
    /// <summary>
    ///   This method initialize the CAN line in the specified operating mode
    ///   and bit transfer rate. The method also performs a reset of the CAN
    ///   controller hardware and disables the reception of CAN messages.  
    /// </summary>
    /// <param name="operatingMode">
    ///   Operating mode of the CAN controller 
    /// </param>
    /// <param name="bitrate">
    ///   Bit timing value according to Philips SJA1000 at 16MHz 
    /// </param>
    /// <remarks>
    ///   The <paramref name="operatingMode"/> parameter defines the operating mode 
    ///   of the CAN controller. The operating mode can be a combination of the 
    ///   following primary operating mode flags:
    ///   <list type="bullet">
    ///
    ///     <item>
    ///       <description><c>CanOperatingModes.Standard</c> - standard frame format (11 bit identifier)</description>
    ///     </item>
    ///     <item>
    ///       <description><c>CanOperatingModes.Extended</c> - extended frame format (29 bit identifier)</description>
    ///     </item>
    ///   </list>
    ///   
    ///   Optionally, the following flags can be combined with the primary operating
    ///   mode flags:
    ///   
    ///   <list type="bullet">
    ///     <item>
    ///       <description><c>CanOperatingModes.ListOnly</c> - listen only mode</description>
    ///     </item>
    ///     <item>
    ///       <description><c>CanOperatingModes.ErrFrame</c> - accept error frames</description>
    ///     </item>
    ///     <item>
    ///       <description><c>CanOperatingModes.LowSpeed</c> - use low speed bus interface</description>
    ///     </item>
    ///   </list>
    ///   
    ///   The bit transfer rate for the CAN controller is specified by the 
    ///   <paramref name="bitrate"/> parameter. The timing value must be set 
    ///   according to the values of the bit timing register 0 and 1 of an Philips 
    ///   SJA1000 CAN controller at a frequency of 16 MHz. See the Philips SJA1000 
    ///   datasheet for more information of how to compute the timing values for a 
    ///   given bit rate.
    /// </remarks>
    /// <exception cref="VciException">
    ///   CAN line initialization failed.
    /// </exception>
    /// <exception cref="ObjectDisposedException">
    ///   Object is already disposed.
    /// </exception>
    //*****************************************************************************
    void  InitLine    ( CanOperatingModes operatingMode, CanBitrate bitrate );

    //*****************************************************************************
    /// <summary>
    ///   This method reset the CAN line to it's initial state. The method
    ///   aborts a currently busy transmit message and switch the CAN controller
    ///   into init mode. The method additionally clears the standard and
    ///   extended mode ID filter. 
    /// </summary>
    /// <exception cref="VciException">
    ///   Resetting CAN line failed.  
    /// </exception>
    /// <exception cref="ObjectDisposedException">
    ///   Object is already disposed. 
    /// </exception>
    //*****************************************************************************
    void  ResetLine   ( );

    //*****************************************************************************
    /// <summary>
    ///   This method starts the CAN line and switch it into running mode.
    ///   After starting the CAN line, CAN messages can be transmitted over
    ///   the message channel.  
    /// </summary>
    /// <exception cref="VciException">
    ///   Starting CAN line failed.  
    /// </exception>
    /// <exception cref="ObjectDisposedException">
    ///   Object is already disposed. 
    /// </exception>
    //*****************************************************************************
    void  StartLine   ( );

    //*****************************************************************************
    /// <summary>
    ///   This method stops the CAN line an switch it into init mode. After
    ///   stopping the CAN controller no further CAN messages are transmitted
    ///   over the message channel. Other than <c>ResetLine</c>, this method does
    ///   not abort a currently busy transmit message and does not clear the
    ///   standard and extended mode ID filter.
    /// </summary>
    /// <exception cref="VciException">
    ///   Stopping CAN line failed.
    /// </exception>
    /// <exception cref="ObjectDisposedException">
    ///   Object is already disposed.  
    /// </exception>
    //*****************************************************************************
    void  StopLine    ( );

    //*****************************************************************************
    /// <summary>
    ///   This method sets the global acceptance filter. The global acceptance
    ///   filter enables the reception of CAN message identifiers specified by
    ///   the bit patterns passed in <paramref name="code"/> and 
    ///   <paramref name="mask"/>. The message IDs enabled by this method are 
    ///   always accepted, even if the specified IDs are not registered within 
    ///   the filter list (see also <c>AddFilterIds</c>). The method can only be 
    ///    called if the CAN controller is in 'init' mode.  
    /// </summary>
    /// <param name="select">
    ///   Filter selection. This parameter can be either <c>CanFilter.Std</c>
    ///   to select the 11-bit acceptance filter, or <c>CanFilter.Ext</c> to
    ///   select the 29-bit acceptance filter.
    /// </param>
    /// <param name="code">
    ///   Acceptance code inclusive RTR bit. 
    /// </param>
    /// <param name="mask">
    ///   Mask that specifies the relevant bits within <paramref name="code"/>. 
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
    ///   The values in <paramref name="code"/> and <paramref name="mask"/> 
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
    ///   <paramref name="code"/> and <paramref name="mask"/> values to enable 
    ///   the standard IDs in the range from 0x100 to 0x103 whereas RTR is 0.
    ///   <code>
    ///    code   = 001 0001 1000 0
    ///    mask   = 111 1111 1100 1
    ///    result = 001 0001 10xx 0
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
    void  SetAccFilter( CanFilter select, uint   code, uint   mask );

    //*****************************************************************************
    /// <summary>
    ///   This method registers the specified CAN message identifier or group
    ///   of identifiers at the specified filter list. IDs registered within the
    ///   filter list are accepted for reception. The method can only be called 
    ///   if the CAN controller is in 'init' mode.
    /// </summary>
    /// <param name="select">
    ///   Filter selection. This parameter can be either <c>CanFilter.Std</c>
    ///   to select the 11-bit filter list, or <c>CanFilter.Ext</c> to
    ///   select the 29-bit filter list.
    /// </param>
    /// <param name="code">
    ///   Message identifier (inclusive RTR) to add to the filter list.  
    /// </param>
    /// <param name="mask">
    ///   Mask that specifies the relevant bits within <paramref name="code"/>. 
    ///   Relevant bits are specified by a 1 in the corresponding bit position,
    ///   non relevant bits are 0. 
    /// </param>
    /// <example>
    ///   The following example demonstates how to compute the 
    ///   <paramref name="code"/> and <paramref name="mask"/> values to register 
    ///   the standard IDs in the range from 0x518 to 0x51B whereas RTR is 1.
    ///   <code>
    ///     code   = 0101 0001 1000 1
    ///     mask   = 0111 1111 1100 1
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
    void  AddFilterIds( CanFilter select, uint   code, uint   mask );

    //*****************************************************************************
    /// <summary>
    ///   This method removes the specified CAN message identifier or group
    ///   of identifiers from the specified filter list. The method can only be
    ///   called if the CAN controller is in 'init' mode.
    /// </summary>
    /// <param name="select">
    ///   Filter selection. This parameter can be either <c>CanFilter.Std</c>
    ///   to select the 11-bit filter list, or <c>CanFilter.Ext</c> to
    ///   select the 29-bit filter list.
    /// </param>
    /// <param name="code">
    ///   Message identifier (inclusive RTR) to remove from the filter list. 
    /// </param>
    /// <param name="mask">
    ///   Mask that specifies the relevant bits within <paramref name="code"/>. 
    ///   Relevant bits are specified by a 1 in the corresponding bit position,
    ///   non relevant bits are 0. 
    /// </param>
    /// <example>
    ///   The following example demonstates how to compute the 
    ///   <paramref name="code"/> and <paramref name="mask"/> values to remove 
    ///   the standard IDs in the range from 0x518 to 0x51B whereas RTR is 1.
    ///   <code>
    ///     code   = 0101 0001 1000 1
    ///     mask   = 0111 1111 1100 1
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
    void  RemFilterIds( CanFilter select, uint   code, uint   mask );
  };


}