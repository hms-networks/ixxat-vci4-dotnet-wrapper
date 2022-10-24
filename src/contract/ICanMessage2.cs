/*****************************************************************************
 IXXAT Automation GmbH
******************************************************************************
 Summary : Declarations for the CAN message class.
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
  ** used namespaces
  *************************************************************************/
  using System;
  using System.Collections.Generic;
  using System.Collections.ObjectModel;
  using System.Runtime.InteropServices;

  // disable: CS1591: Missing XML comment for publicly visible type or member
  #pragma warning disable 1591

  //*****************************************************************************
  /// <summary>
  ///   Managed image of native struct CANMSG2.
  /// </summary>
  /// <remarks>
  ///   We have to declare a managed version of native struct CANMSG2 to prevent
  ///   problems because of the following compiler error:
  ///   error C4368: cannot define 'm_CanMsg' as a member of managed 
  ///                'Ixxat::Vci4::Bal::Can::CanMessage': mixed types are not 
  ///                supported
  /// </remarks>
  //*****************************************************************************
  [StructLayout(LayoutKind.Sequential)]
  public struct mgdCANMSG2
  {
    ///<summary>time stamp for receive message</summary>
    public uint         dwTime;
    ///<summary>reserved (set to 0)</summary>
    public uint         _rsvd_;
    ///<summary>CAN message identifier (INTEL format)</summary>
    public uint         dwMsgId;
    ///<summary>message information (bit field)</summary>
    public mgdCANMSGINFO uMsgInfo;
    ///<summary>message data</summary>
    // (UINT8[8] is native declaration and
    // array<UINT8> is not what we want !!)
    public byte         bData1;     
    public byte         bData2;
    public byte         bData3;
    public byte         bData4;
    public byte         bData5;
    public byte         bData6;
    public byte         bData7;
    public byte         bData8;
    public byte         bData9;
    public byte         bData10;
    public byte         bData11;
    public byte         bData12;
    public byte         bData13;
    public byte         bData14;
    public byte         bData15;
    public byte         bData16;
    public byte         bData17;
    public byte         bData18;
    public byte         bData19;
    public byte         bData20;
    public byte         bData21;
    public byte         bData22;
    public byte         bData23;
    public byte         bData24;
    public byte         bData25;
    public byte         bData26;
    public byte         bData27;
    public byte         bData28;
    public byte         bData29;
    public byte         bData30;
    public byte         bData31;
    public byte         bData32;
    public byte         bData33;
    public byte         bData34;
    public byte         bData35;
    public byte         bData36;
    public byte         bData37;
    public byte         bData38;
    public byte         bData39;
    public byte         bData40;
    public byte         bData41;
    public byte         bData42;
    public byte         bData43;
    public byte         bData44;
    public byte         bData45;
    public byte         bData46;
    public byte         bData47;
    public byte         bData48;
    public byte         bData49;
    public byte         bData50;
    public byte         bData51;
    public byte         bData52;
    public byte         bData53;
    public byte         bData54;
    public byte         bData55;
    public byte         bData56;
    public byte         bData57;
    public byte         bData58;
    public byte         bData59;
    public byte         bData60;
    public byte         bData61;
    public byte         bData62;
    public byte         bData63;
    public byte         bData64;
  };

  // enable: CS1591: Missing XML comment for publicly visible type or member
  #pragma warning restore 1591

  //*****************************************************************************
  /// <summary>
  ///   This class represents a CAN FD message. CAN FD messages can be received
  ///   and transmitted via the message reader (<c>ICanMessageReader</c>) and 
  ///   the message writer (<c>ICanMessageWriter</c>) of a CAN channel 
  ///   (<c>ICanChannel2</c>). 
  ///   The CAN data field can be accessed via the indexer property.
  /// </summary>
  /// <example>
  ///   <code>
  ///     IMessageFactory factory = VciServer.Instance().MsgFactory;
  ///     ICanMessage2 canMsg = (ICanMessage)factory.CreateMsg(typeof(ICanMessage2));
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
  public interface ICanMessage2 : ICanMessage
  {
  };


}
