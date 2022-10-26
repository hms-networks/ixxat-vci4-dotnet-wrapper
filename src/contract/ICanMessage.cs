/*****************************************************************************
 IXXAT Automation GmbH
******************************************************************************
 Summary : Declarations for the CAN message class.
 Compiler: Microsoft VC++ 8
******************************************************************************
 all rights reserved
*****************************************************************************/

namespace Ixxat.Vci4.Bal.Can 
{
  using System;
  using System.Reflection;
  using System.Runtime.InteropServices;


  //*****************************************************************************
  /// <summary>
  ///   Enumeration of values that are used to specify the frame type of a 
  ///   CAN message (see <c>CanMessage</c>).
  /// </summary>
  //*****************************************************************************
  public enum CanMsgFrameType : int
  {
    /// <summary>
    ///   Normal message. 
    ///   All regular receive messages are of this type. The property <c>Identifier</c>
    ///   contains the ID of the message, the field <c>TimeStamp</c> the time of 
    ///   reception. The data field (accessible via the message's indexer) contain 
    ///   according to length (see <c>DataLength</c>) the databytes of the message. 
    ///   With transmit messages the IDs are to be entered in the property 
    ///   <c>Identifier</c> and the databytes according to length in the property
    ///   <c>DataLength</c>. The property <c>TimeStamp</c> is normally set to 0, 
    ///   unless the message is to be transmitted with a delay. In this case the 
    ///   delay time is to be specified in ticks.
    /// </summary>
    Data        = 0,
    /// <summary>
    ///   Information message. 
    ///   This message type is entered in the receive buffers of all activated 
    ///   message channels with certain events or with changes to the status of 
    ///   the controller. The property <c>Identifier</c> of the message always has the 
    ///   value 0xFFFFFFFF. The property Data[0] (Indexer of the message) contains 
    ///   one of the following values:
    ///   <list type="table">
    ///     <listheader>
    ///       <term>Constant</term>
    ///       <description>Meaning</description>
    ///     </listheader>
    ///     <item>
    ///        <term><c>CanMsgInfoValue.Start</c></term>
    ///        <description>
    ///          The CAN controller was started. The property <c>TimeStamp</c>  
    ///          contains the relative start time (normally 0).
    ///        </description>
    ///     </item>
    ///     <item>
    ///        <term><c>CanMsgInfoValue.Stop</c></term>
    ///        <description>
    ///          The CAN controller was stopped. The property <c>TimeStamp</c> 
    ///          contains the value 0. 
    ///        </description>
    ///     </item>
    ///     <item>
    ///        <term><c>CanMsgInfoValue.Reset</c></term>
    ///        <description>
    ///           The CAN controller was reset. The property <c>TimeStamp</c>
    ///           contains the value 0.
    ///        </description>
    ///     </item>
    ///   </list>
    /// </summary>
    Info        = 1,    
    /// <summary>
    ///   Error message. 
    ///   This message type is entered in the receive buffers of all activated message 
    ///   channels when bus errors occur if the flag <c>CanOperatingModes.ErrFrame</c> 
    ///   was specified in the parameter operatingMode when the method 
    ///   <c>ICanControl.InitLine</c>  was called. The property <c>Identifier</c> of 
    ///   the message always has the value 0xFFFFFFFF. The time of the event is marked 
    ///   in the property <c>TimeStamp</c> of the message. The property Data[0] 
    ///   (indexer of the message) contains one of the following values:
    ///   <list type="table">
    ///     <listheader>
    ///       <term>Constant</term>
    ///       <description>Meaning</description>
    ///     </listheader>
    ///     <item>
    ///        <term><c>CanMsgError.Stuff</c></term>
    ///        <description>Bit stuff error</description>
    ///     </item>
    ///     <item>
    ///        <term><c>CanMsgError.Form</c></term>
    ///        <description>Format error</description>
    ///     </item>
    ///     <item>
    ///        <term><c>CanMsgError.Acknowledge</c></term>
    ///        <description>Acknowledge error</description>
    ///     </item>
    ///     <item>
    ///        <term><c>CanMsgError.Bit</c></term>
    ///        <description>Bit error</description>
    ///     </item>
    ///     <item>
    ///        <term><c>CanMsgError.Crc</c></term>
    ///        <description>CRC error</description>
    ///     </item>
    ///     <item>
    ///        <term><c>CanMsgError.Other</c></term>
    ///        <description>Other unspecified error</description>
    ///     </item>
    ///   </list>
    /// </summary>
    Error       = 2,   
    /// <summary>
    ///   Status message. 
    ///   This message type is entered in the receive buffers of all activated 
    ///   message channels when the controller status changes. The property
    ///   <c>Identifier</c> of the message always has the value 0xFFFFFFFF. 
    ///   The time of the event is marked in the property <c>TimeStamp</c> of 
    ///   the message. The property Data[0] (indexer of the message) contains 
    ///   flags defined by <c>CanCtrlStatus</c>. 
    ///   The contents of the other data fields are undefined.
    /// </summary>
    Status      = 3,  
    /// <summary>
    ///   Not currently used, or reserved for future extensions.
    /// </summary>
    Wakeup      = 4,  
    /// <summary>
    ///   Timer overrun. 
    ///   Messages of this type are generated when an overrun of the 32-bit 
    ///   time stamp of CAN messages occurs. The time of the event (normally 0) 
    ///   is given in the property <c>TimeStamp</c> of the message and the number 
    ///   of timer overruns after the last timer overrun message in the <c>Identifier</c>. 
    ///   The contents of the data fields are undefined.
    /// </summary>
    TimeOverrun = 5, 
    /// <summary>
    ///   Not currently used, or reserved for future extensions.
    /// </summary>
    TimeReset   = 6  
  };


  //*****************************************************************************
  /// <summary>
  ///   Enumeration of values that are used to specify the filter that indicated 
  ///   the acceptance of a CAN message (see <c>CanMessage</c>).
  /// </summary>
  //*****************************************************************************
  public enum CanMsgAccReason : int
  {
    /// <summary>
    ///   Message not accepted
    /// </summary>
    Reject  = 0x00,  
    /// <summary>
    ///   Message always accepted
    /// </summary>
    Always  = 0xFF,  
    /// <summary>
    ///   Message accepted by 1. filter
    /// </summary>
    Filter1 = 0x01,
    /// <summary>
    ///   Message accepted by 2. filter
    /// </summary>
    Filter2 = 0x02 
  };


  //*****************************************************************************
  /// <summary>
  ///   Enumeration of information values supplied in first data field byte of
  ///   an info frame <c>CanMessage</c> 
  ///   (CanMessage.FrameType == CanMsgFrameType.Info).
  /// </summary>
  //*****************************************************************************
  public enum CanMsgInfoValue 
  {
    /// <summary>
    ///   Start of CAN controller
    /// </summary>
    Start = 1,
    /// <summary>
    ///   Stop of CAN controller
    /// </summary>
    Stop  = 2,
    /// <summary>
    ///   Reset of CAN controller
    /// </summary>
    Reset = 3
  };


  //*****************************************************************************
  /// <summary>
  ///   Enumeration of error information values supplied in first data field byte 
  ///   of an eror frame <c>CanMessage</c> 
  ///   (CanMessage.FrameType == CanMsgFrameType.Error).
  /// </summary>
  //*****************************************************************************
  public enum CanMsgError 
  {
    /// <summary>
    ///   Stuff error
    /// </summary>
    Stuff = 1,
    /// <summary>
    ///   Form error
    /// </summary>
    Form = 2,
    /// <summary>
    ///   Acknowledgment error
    /// </summary>
    Acknowledge = 3,
    /// <summary>
    ///   Bit error
    /// </summary>
    Bit = 4,
    /// <summary>
    ///   fast data bit error
    /// </summary>
    Fdb = 5,
    /// <summary>
    ///   CRC error
    /// </summary>
    Crc = 6,
    /// <summary>
    ///   Data length error
    /// </summary>
    Dlc = 7,
    /// <summary>
    ///   Other (unspecified) error
    /// </summary>
    Other = 8
  };


  //*****************************************************************************
/// <summary>
///   Managed image of native struct CANMSGINFO.
/// </summary>
/// <remarks>
///   We have to declare a managed version of native struct CANMSGINFO to 
///   prevent problems because of the following compiler error C4368.
///   (see <c>mngtCANMSG</c>).
/// </remarks>
//*****************************************************************************
[StructLayout(LayoutKind.Sequential)]
public struct mgdCANMSGINFO
{
  ///<summary>type (see CAN_MSGTYPE_ constants)</summary>
  public byte bType;
  ///<summary>reserved</summary>
  public byte bReserved;
  ///<summary>flags (see CAN_MSGFLAGS_ constants)</summary>
  public byte bFlags;
  ///<summary>accept code (see CAN_ACCEPT_ constants)</summary>
  public byte bAccept;
};

// disable: CS1591: Missing XML comment for publicly visible type or member
#pragma warning disable 1591

//*****************************************************************************
/// <summary>
///   Managed image of native struct CANMSG.
/// </summary>
/// <remarks>
///   We have to declare a managed version of native struct CANMSG to prevent
///   problems because of the following compiler error:
///   error C4368: cannot define 'm_CanMsg' as a member of managed 
///                'Ixxat::Vci4::Bal::Can::CanMessage': mixed types are not 
///                supported
/// </remarks>
//*****************************************************************************
[StructLayout(LayoutKind.Sequential)]
public struct mgdCANMSG
{
  ///<summary>time stamp for receive message</summary>
  public uint           dwTime;
  ///<summary>CAN message identifier (INTEL format)</summary>
  public uint           dwMsgId;
  ///<summary>message information (bit field)</summary>
  public mgdCANMSGINFO  uMsgInfo;
  ///<summary>message data</summary>
  // (UINT8[8] is native declaration and
  // array<UINT8> is not what we want !!)
  public byte bData1;    
  public byte           bData2;
  public byte           bData3;
  public byte           bData4;
  public byte           bData5;
  public byte           bData6;
  public byte           bData7;
  public byte           bData8;
};

// enable: CS1591: Missing XML comment for publicly visible type or member
#pragma warning restore 1591

  //*****************************************************************************
  /// <summary>
  ///   This class represents a CAN message. CAN messages can be received
  ///   and transmitted via the message reader (<c>ICanMessageReader</c>) and 
  ///   the message writer (<c>ICanMessageWriter</c>) of a CAN channel 
  ///   (<c>ICanChannel</c>). 
  ///   The CAN data field can be accessed via the indexer property.
  /// </summary>
  /// <example>
  ///   <code>
  ///     IMessageFactory factory = VciServer.Instance().MsgFactory;
  ///     ICanMessage canMsg = (ICanMessage)factory.CreateMsg(typeof(ICanMessage));
  ///       
  ///     canMsg.TimeStamp  = 0;
  ///     canMsg.Identifier = 0x100;
  ///     canMsg.FrameType  = CanMsgFrameType.Data;
  ///     canMsg.DataLength = 8;
  ///     canMsg.SelfReceptionRequest = true;  // show this message in the console window
  ///     
  ///     for (Byte i = 0; i &lt; canMsg.DataLength; i++)
  ///     {
  ///       canMsg[i] = i;
  ///     }
  ///     
  ///     // Write the CAN message into the transmit FIFO
  ///     mWriter.SendMessage(canMsg);
  ///   </code>
  /// </example>
  //*****************************************************************************
  public interface ICanMessage 
  {
    //*****************************************************************************
    /// <summary>
    ///   With receive messages, this field contains the relative reception time 
    ///   of the message in ticks. The resolution of a tick can be calculated from 
    ///   the properties <c>ICanSocket.ClockFrequency</c> and 
    ///   <c>ICanSocket.TimeStampCounterDivisor</c> in accordance with the 
    ///   following formula:
    ///   <code>
    ///     Resolution [s] = TimeStampCounterDivisor / ClockFrequency 
    ///   </code>
    ///   With transmit messages, the field defines with how many ticks delay the 
    ///   message is to be transmitted to the bus. The delay time between the last 
    ///   message transmitted and the new message can be calculated with the 
    ///   properties <c>ICanSocket.ClockFrequency</c> and 
    ///   <c>ICanSocket.DelayedTXTimerDivisor</c> in accordance with the following 
    ///   formula:
    ///   <code>
    ///     delay time [s] = (DelayedTXTimerDivisor / ClockFrequency) * TimeStamp 
    ///   </code>
    ///   The maximum possible delay time is defined by the property 
    ///   <c>ICanSocket.MaxDelayedTXTicks</c>.
    /// </summary>
    //*****************************************************************************
    uint  TimeStamp 
    {
      get;
      set;
    }

    //*****************************************************************************
    /// <summary>
    ///   Gets or sets the CAN identifier of this CAN message.
    /// </summary>
    //*****************************************************************************
    uint Identifier 
    {
      get;
      set;
    }

    //*****************************************************************************
    /// <summary>
    ///   Gets a value indicating the frame type of this CAN message.
    /// </summary>
    //*****************************************************************************
    CanMsgFrameType FrameType 
    {
      get;
      set;
    }

    //*****************************************************************************
    /// <summary>
    ///   Gets a value indicating the filter that accepted the message.
    /// </summary>
    /// <returns>
    ///   A value indicating the filter that accepted the message.
    /// </returns>
    //*****************************************************************************
    CanMsgAccReason AcceptReason 
    {
      get;
    }

    //*****************************************************************************
    /// <summary>
    ///   Gets or sets the data length of this CAN/CANFD message.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">
    ///   The data length value to be set is out of range [0;64].
    /// </exception>
    //*****************************************************************************
    byte DataLength 
    {
      get;
      set;
    }

    //*****************************************************************************
    /// <summary>
    ///   Gets a value indicating whether this message was the last
    ///   message which could be placed in the receive FIFO, before
    ///   this overflows.
    /// </summary>
    /// <returns>
    ///   A value indicating whether this message was the last
    ///   message which could be placed in the receive FIFO, before
    ///   this overflows.
    /// </returns>
    //*****************************************************************************
    bool PossibleOverrun 
    {
      get;
    }

    //*****************************************************************************
    /// <summary>
    ///   Gets or sets a value indicating whether this message is the result of
    ///   a self reception request.
    /// </summary>
    //*****************************************************************************
    bool SelfReceptionRequest 
    {
      get;
      set;
    }

    //*****************************************************************************
    /// <summary>
    ///   Gets or sets a value indicating whether this message is a remote
    ///   transmission request.
    /// </summary>
    //*****************************************************************************
    bool RemoteTransmissionRequest 
    {
      get;
      set;
    }

    //*****************************************************************************
    /// <summary>
    ///   Gets or sets a value indicating whether this message has extended
    ///   frame format (29-bit id).
    /// </summary>
    //*****************************************************************************
    bool ExtendedFrameFormat 
    {
      get;
      set;
    }

    //*****************************************************************************
    /// <summary>
    ///   Gets or sets the single shot mode bit of the frame.
    /// </summary>
    //*****************************************************************************
    bool SingleShotMode
    {
      get;
      set;
    }

    //*****************************************************************************
    /// <summary>
    ///   Gets or sets the high priority bit of the frame.
    /// </summary>
    //*****************************************************************************
    bool HighPriorityMsg
    {
      get;
      set;
    }

    //*****************************************************************************
    /// <summary>
    ///   Gets or sets the extended data length bit of the frame.
    /// </summary>
    //*****************************************************************************
    bool ExtendedDataLength
    {
      get;
      set;
    }

    //*****************************************************************************
    /// <summary>
    ///   Gets or sets the fast data rate bit of the frame.
    /// </summary>
    //*****************************************************************************
    bool FastDataRate
    {
      get;
      set;
    }

    //*****************************************************************************
    /// <summary>
    ///   Gets or sets the fast error state indicator of the frame.
    /// </summary>
    //*****************************************************************************
    bool ErrorStateIndicator
    {
      get;
      set;
    }

    //*****************************************************************************
    /// <summary>
    ///   Gets or sets a single data byte at the specified index from this
    ///   CAN message. This property represents the indexer property.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">
    ///   The specified index is out of range [0;<c>DataLength</c>].
    /// </exception>
    /// <example>
    ///   <code>
    ///     IMessageFactory factory = VciServer.Instance().MsgFactory;
    ///     ICanMessage canMsg = (ICanMessage)factory.CreateMsg(typeof(ICanMessage));
    ///       
    ///     canMsg.TimeStamp  = 0;
    ///     canMsg.Identifier = 0x100;
    ///     canMsg.FrameType  = CanMsgFrameType.Data;
    ///     canMsg.DataLength = 8;
    ///     canMsg.SelfReceptionRequest = true;  // show this message in the console window
    ///     
    ///     for (Byte i = 0; i &lt; canMsg.DataLength; i++)
    ///     {
    ///       canMsg[i] = i;
    ///     }
    ///     
    ///     // Write the CAN message into the transmit FIFO
    ///     mWriter.SendMessage(canMsg);
    ///   </code>
    /// </example>
    //*****************************************************************************
    byte this[int index] 
    {
      get;
      set;
    }

    //--------------------------------------------------------------------
    // member functions
    //--------------------------------------------------------------------

    //*****************************************************************************
    /// <summary>
    ///   This method clears the contents of this CAN message.
    /// </summary>
    //*****************************************************************************
    void Clear();
  };


}