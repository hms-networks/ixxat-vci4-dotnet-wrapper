/*****************************************************************************
 IXXAT Automation GmbH
******************************************************************************
 Summary : Declarations for the LIN message class.
 Compiler: Microsoft VC++ 8
******************************************************************************
 all rights reserved
*****************************************************************************/

namespace Ixxat.Vci4.Bal.Lin
{
  using System;
  using System.Reflection;
  using System.Runtime.InteropServices;
  using System.Text;

  //*****************************************************************************
  /// <summary>
  ///   Enumeration of values that are used to specify the message type of a 
  ///   LIN message (see <c>LinMessage</c>).
  /// </summary>
  //*****************************************************************************
  public enum LinMessageType : int
  {
    /// <summary>
    ///   Normal message. 
    ///   All regular receive messages are of this type. 
    ///   The property <c>LinMessage.ProtId</c> contains the ID of the message, 
    ///   the property <c>LinMessage.TimeStamp</c> the receive time. 
    ///   The data field (accessible via the message's indexer) contains, 
    ///   according to length (see <c>DataLength</c>) the databytes of the message.
    /// . In master mode, messages of this type can also be transmitted. The ID 
    ///   must be entered in the property <c>LinMessage.ProtId</c> and in the data 
    ///   field (accessible via the message's indexer), depending on the length
    ///   (<c>ProtId</c>), the data to be transmitted. 
    ///   The property <c>LinMessage.TimeStamp</c> is set to 0. To transmit
    ///   only the ID without data, property <c>IdOnly</c> is set to true.
    /// </summary>
    Data        = 0x00,
    /// <summary>
    ///   Information message. 
    ///   This message type is entered in the receive buffers of all activated 
    ///   message monitors with certain events or with changes to the status of 
    ///   the controller. The property <c>Identifier</c> of the message always has the 
    ///   value 0xFF. The property Data[0] (Indexer of the message) contains 
    ///   one of the following values:
    ///   <list type="table">
    ///     <listheader>
    ///       <term>Constant</term>
    ///       <description>Meaning</description>
    ///     </listheader>
    ///     <item>
    ///        <term><c>LinMsgInfoValue.Start</c></term>
    ///        <description>
    ///          The LIN controller was started. The property <c>TimeStamp</c>  
    ///          contains the relative start time (normally 0).
    ///        </description>
    ///     </item>
    ///     <item>
    ///        <term><c>LinMsgInfoValue.Stop</c></term>
    ///        <description>
    ///          The LIN controller was stopped. The property <c>TimeStamp</c> 
    ///          contains the value 0. 
    ///        </description>
    ///     </item>
    ///     <item>
    ///        <term><c>LinMsgInfoValue.Reset</c></term>
    ///        <description>
    ///           The LIN controller was reset. The property <c>TimeStamp</c>
    ///           contains the value 0.
    ///        </description>
    ///     </item>
    ///   </list>
    /// </summary>
    Info        = 0x01,
    /// <summary>
    ///   Error message. 
    ///   This message type is entered in the receive buffers of all activated message 
    ///   monitors when bus errors occur as far as <c>LinOperatingModes.ErrFrame</c> 
    ///   was specified in the parameter OperatingMode when the method 
    ///   <c>ILinControl.InitLine</c> was called. The property <c>ProtId</c> of 
    ///   the message always has the value 0xFF. The time of the event is marked 
    ///   in the property <c>TimeStamp</c> of the message. The property Data[0] 
    ///   (indexer of the message) contains one of the following values:
    ///   <list type="table">
    ///     <listheader>
    ///       <term>Constant</term>
    ///       <description>Meaning</description>
    ///     </listheader>
    ///     <item>
    ///        <term><c>LinMsgError.Bit</c></term>
    ///        <description>Bit error</description>
    ///     </item>
    ///     <item>
    ///        <term><c>LinMsgError.Crc</c></term>
    ///        <description>Checksum error</description>
    ///     </item>
    ///     <item>
    ///        <term><c>LinMsgError.Parity</c></term>
    ///        <description>Parity error of the identifier</description>
    ///     </item>
    ///     <item>
    ///        <term><c>LinMsgError.SlaveNoResponse</c></term>
    ///        <description>Slave does not respond</description>
    ///     </item>
    ///     <item>
    ///        <term><c>LinMsgError.Sync</c></term>
    ///        <description>Invalid synchronization field</description>
    ///     </item>
    ///     <item>
    ///        <term><c>LinMsgError.NoBus</c></term>
    ///        <description>No bus activity</description>
    ///     </item>
    ///     <item>
    ///        <term><c>LinMsgError.Other</c></term>
    ///        <description>Other unspecified error</description>
    ///     </item>
    ///   </list>
    ///   The property Data[1] (indexer of the message) contains the low value byte 
    ///   of the current status (see <c>LinLineStatus</c>). The content of the other 
    ///   data field bytes is undefined.
    /// </summary>
    Error       = 0x02,   
    /// <summary>
    ///   Status message. 
    ///   This message type is entered in the receive buffers of all activated 
    ///   message monitors when the controller status changes. The property
    ///   <c>ProtId</c> of the message always has the value 0xFF. 
    ///   The time of the event is marked in the property <c>TimeStamp</c> of 
    ///   the message. The property Data[0] (indexer of the message) contains 
    ///   flags defined by <c>CanCtrlStatus</c>. 
    ///   The contents of the other data fields are undefined.
    /// </summary>
    Status      = 0x03,  
    /// <summary>
    ///   Only for transmit messages. Messages of this type generate a wake-up 
    ///   signal on the bus. The fields <c>TimeStamp</c>, <c>ProtId</c> and 
    ///   <c>DataLength</c> have no significance.
    /// </summary>
    Wakeup      = 0x04,  
    /// <summary>
    ///   Timer counter overrun. Messages of this type are generated in the event 
    ///   of an overrun of the 32 bit time stamp of LIN messages. The field 
    ///   <c>TimeStamp</c> of the message contains the time of the event 
    ///   (normally 0) and in the field <c>DataLength</c> the number of timer 
    ///   overruns after the last timer overrun message.
    /// </summary>
    TimeOverrun = 0x06,
    /// <summary>
    ///   Goto Sleep message. The fields <c>TimeStamp</c>, <c>ProtId</c> and 
    ///   <c>DataLength</c> have no significance.
    /// </summary>
    Sleep       = 0x05
  };


  //*****************************************************************************
  /// <summary>
  ///   Enumeration of information values supplied in first data field byte of
  ///   an info frame <c>LinMessage</c> 
  ///   (LinMessage.MessageType == LinMessageType.Info).
  /// </summary>
  //*****************************************************************************
  public enum LinMsgInfoValue 
  {
    /// <summary>
    ///   Start of LIN controller
    /// </summary>
    Start = 1,
    /// <summary>
    ///   Stop of LIN controller
    /// </summary>
    Stop = 2,
    /// <summary>
    ///   Reset of LIN controller
    /// </summary>
    Reset = 3
  };


  //*****************************************************************************
  /// <summary>
  ///   Enumeration of error information values supplied in first data field byte 
  ///   of an eror frame <c>LinMessage</c> 
  ///   (LinMessage.MessageType == LinMessageType.Error).
  /// </summary>
  //*****************************************************************************
  public enum LinMsgError 
  {
    /// <summary>
    ///   Bit error
    /// </summary>
    Bit = 1,
    /// <summary>
    ///   Checksum error
    /// </summary>
    Crc = 2,
    /// <summary>
    ///   Parity error of the identifier
    /// </summary>
    Parity = 3,
    /// <summary>
    ///   Slave does not respond
    /// </summary>
    SlaveNoResponse = 4,
    /// <summary>
    ///   Invalid synchronization field
    /// </summary>
    Sync = 5,
    /// <summary>
    ///   No bus activity
    /// </summary>
    NoBus = 6,
    /// <summary>
    ///   Other unspecified error
    /// </summary>
    Other = 7
  };



  //*****************************************************************************
  /// <summary>
  ///   Managed image of native struct LINMSGINFO.
  /// </summary>
  /// <remarks>
  ///   We have to declare a managed version of native struct LINMSGINFO to 
  ///   prevent problems because of the following compiler error C4368.
  ///   (see <c>mngtLINMSG</c>).
  /// </remarks>
  //*****************************************************************************
  [StructLayout(LayoutKind.Sequential)]
  public struct mgdLINMSGINFO
  {
    ///<summary>protected id</summary>
    public byte bPid;
    ///<summary>message type (see LIN_MSGTYPE_ constants)</summary>
    public byte bType;
    ///<summary>data length</summary>
    public byte bDlen;
    ///<summary>flags (see LIN_MSGFLAGS_ constants)</summary>
    public byte bFlags;   
  };

  // disable: CS1591: Missing XML comment for publicly visible type or member
  #pragma warning disable 1591

  //*****************************************************************************
  /// <summary>
  ///   Managed image of native struct LINMSG.
  /// </summary>
  /// <remarks>
  ///   We have to declare a managed version of native struct LINMSG to prevent
  ///   problems because of the following compiler error:
  ///   error C4368: cannot define 'm_LinMsg' as a member of managed 
  ///                'Ixxat::Vci4::Bal::Lin::LinMessage': mixed types are not 
  ///                supported
  /// </remarks>
  //*****************************************************************************
  [StructLayout(LayoutKind.Sequential)]
  public struct mgdLINMSG
  {
    ///<summary>time stamp for receive message [ms]</summary>
    public uint dwTime;
    ///<summary>message information (bit field)</summary>
    public mgdLINMSGINFO uMsgInfo;
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
  ///   This class represents a LIN message. LIN messages can be received
  ///   via a LIN monitor (<c>ILinMonitor</c>) and transmitted via a LIN control
  ///   (<c>ILinControl</c>). The LIN data field can be accessed via the 
  ///   indexer property.
  /// </summary>
  /// <example>
  ///   <code>
  ///     IMessageFactory factory = VciServer.Instance().MsgFactory;
  ///     ILinMessage message = (ICanMessage)factory.CreateMsg(typeof(ILinMessage));
  ///       
  ///     // Set DLC = 2
  ///     message.DataLength = 2;
  ///     // Initialize data field with 0xFF 0x08
  ///     message[0] = 0xFF;
  ///     message[1] = 0x08;
  ///   </code>
  /// </example>
  //*****************************************************************************
  public interface ILinMessage
  {
    //--------------------------------------------------------------------
    // properties
    //--------------------------------------------------------------------

    //*****************************************************************************
    /// <summary>
    ///   With receive messages, this field contains the relative reception time 
    ///   of the message in ticks. The resolution of a tick can be calculated from 
    ///   the properties <c>ILinSocket.ClockFrequency</c> and 
    ///   <c>ILinSocket.TimeStampCounterDivisor</c> in accordance with the 
    ///   following formula:
    ///   <code>
    ///     Resolution [s] = TimeStampCounterDivisor / ClockFrequency 
    ///   </code>
    /// </summary>
    //*****************************************************************************
    uint TimeStamp
    {
      get;
      set;
    }

    //*****************************************************************************
    /// <summary>
    ///   Gets or sets the protected identifier of this LIN message.
    /// </summary>
    //*****************************************************************************
    byte ProtId
    {
      get;
      set;
    }

    //*****************************************************************************
    /// <summary>
    ///   Gets a value indicating the frame type of this message.
    /// </summary>
    //*****************************************************************************
    LinMessageType MessageType
    {
      get;
      set;
    }

    //*****************************************************************************
    /// <summary>
    ///   Gets or sets the data length of this message.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">
    ///   The data length value to be set is out of range [0;8].
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
    ///   Gets or sets a value indicating if it is a message with extended
    ///   checksum in accordance with LIN 2.0.
    /// </summary>
    //*****************************************************************************
    bool ExtendedCrc
    {
      get;
      set;
    }

    //*****************************************************************************
    /// <summary>
    ///   Gets a value indicating that the LIN controller itself transmitted the 
    ///   message, i.e. with messages for which the controller has an entry
    ///   in the response table.
    /// </summary>
    //*****************************************************************************
    bool SenderOfResponse
    {
      get;
      set;
    }

    //*****************************************************************************
    /// <summary>
    ///   Gets or sets a value indicating if the message should be sent without
    ///   data (only with identifier).
    /// </summary>
    //*****************************************************************************
    bool IdOnly
    {
      get;
      set;
    }

    //*****************************************************************************
    /// <summary>
    ///   Gets or sets a single data byte at the specified index from this
    ///   message. This property represents the indexer property.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">
    ///   The specified index is out of range [0;<c>DataLength</c>].
    /// </exception>
    /// <example>
    ///   <code>
    ///     IMessageFactory factory = VciServer.Instance().MsgFactory;
    ///     ILinMessage message = (ICanMessage)factory.CreateMsg(typeof(ILinMessage));
    ///       
    ///     // Set DLC = 2
    ///     message.DataLength = 2;
    ///     // Initialize data field with 0xFF 0x08
    ///     message[0] = 0xFF;
    ///     message[1] = 0x08;
    ///   </code>
    /// </example>
    //*****************************************************************************
    byte this[int index]           
    {
      get;
      set;
    }
  }


}