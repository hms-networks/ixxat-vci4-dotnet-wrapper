/*****************************************************************************
 IXXAT Automation GmbH
******************************************************************************
 Summary : Declarations for the CAN message class.
 Compiler: Microsoft VC++ 8
******************************************************************************
 all rights reserved
*****************************************************************************/

#pragma once

/*************************************************************************
** include files
*************************************************************************/
#include <vcisdk.h>

/*************************************************************************
** namespace Ixxat.Vci4.Bal.Can
*************************************************************************/
namespace Ixxat {
  namespace Vci4 {
    namespace Bal {
      namespace Can {

/*************************************************************************
** used namespaces
*************************************************************************/
using namespace System;
using namespace System::Reflection;
using namespace System::Runtime::InteropServices;
using namespace Ixxat::Vci4;


//*****************************************************************************
/// <summary>
///   This value class represents a CAN message. CAN messages can be received
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
///     canMsg.TimeStamp = 0;
///     canMsg.Identifier = 0x100;
///     canMsg.FrameType = CanMsgFrameType.Data;
///     canMsg.DataLength = 8;
///     canMsg.SelfReceptionRequest = true;  // show this message in the console window
///     
///     for (Byte i = 0; i < canMsg.DataLength; i++)
///     {
///       canMsg[i] = i;
///     }
///     
///     // Write the CAN message into the transmit FIFO
///     writer.SendMessage(canMsg);
///   </code>
/// </example>
//*****************************************************************************
public value class CanMessage : public ICanMessage2
{
  //--------------------------------------------------------------------
  // member variables
  //--------------------------------------------------------------------
  private:
    static CanMessage ms_EmptyMsg;

  internal:
    mgdCANMSG m_CanMsg; // CAN message structure

  //--------------------------------------------------------------------
  // properties
  //--------------------------------------------------------------------
  public:
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
    virtual property UInt32 TimeStamp 
    {
      /// <summary>
      ///   Gets the time stamp of this CAN message.
      /// </summary>
      /// <returns>
      ///   The time stamp of this CAN message.
      /// </returns>
      UInt32 get(void) sealed
      {
        return( m_CanMsg.dwTime );
      };

      /// <summary>
      ///   Sets the time stamp of this CAN message.
      /// </summary>
      /// <param name="timestamp">
      ///   Time stamp value to set.
      /// </param>
      void set(UInt32 timestamp) sealed
      {
        m_CanMsg.dwTime = timestamp;
      };
    }

    //*****************************************************************************
    /// <summary>
    ///   Gets or sets the CAN identifier of this CAN message.
    /// </summary>
    //*****************************************************************************
    virtual property UInt32 Identifier 
    {
      /// <summary>
      ///   Gets the CAN identifier of this CAN message.
      /// </summary>
      /// <returns>
      ///   The CAN identifier of this CAN message.
      /// </returns>
      UInt32 get(void) sealed
      {
        return( m_CanMsg.dwMsgId );
      };

      /// <summary>
      ///   Sets the CAN identifier of this CAN message.
      /// </summary>
      /// <param name="canId">
      ///   CAN identifier value to set.
      /// </param>
      void set(UInt32 canId) sealed
      {
        m_CanMsg.dwMsgId = canId;
      };
    }

    //*****************************************************************************
    /// <summary>
    ///   Gets a value indicating the frame type of this CAN message.
    /// </summary>
    //*****************************************************************************
    virtual property CanMsgFrameType FrameType 
    {
      /// <summary>
      ///   Gets a value indicating the frame type of this CAN message.
      /// </summary>
      /// <returns>
      ///   A value indicating the frame type of this CAN message.
      /// </returns>
      CanMsgFrameType get(void) sealed
      {
        pin_ptr<mgdCANMSGINFO> pMngtInfo = &m_CanMsg.uMsgInfo;
        return( (CanMsgFrameType) ((PCANMSGINFO)pMngtInfo)->Bytes.bType );
      };

      /// <summary>
      ///   Sets a value indicating the frame type of this CAN message.
      /// </summary>
      /// <paramref name="frameType">
      ///   The message frame type to set.
      /// </paramref>
      void set(CanMsgFrameType frameType) sealed
      {
        pin_ptr<mgdCANMSGINFO> pMngtInfo = &m_CanMsg.uMsgInfo;
        ((PCANMSGINFO)pMngtInfo)->Bytes.bType = (UINT8)frameType;
      }
    }

    //*****************************************************************************
    /// <summary>
    ///   Gets a value indicating the filter that accepted the message.
    /// </summary>
    //*****************************************************************************
    virtual property CanMsgAccReason AcceptReason 
    {
      /// <summary>
      ///   Gets a value indicating the filter that accepted the message.
      /// </summary>
      /// <returns>
      ///   A value indicating the filter that accepted the message.
      /// </returns>
      CanMsgAccReason get(void) sealed
      {
        pin_ptr<mgdCANMSGINFO> pMngtInfo = &m_CanMsg.uMsgInfo;
        return( (CanMsgAccReason) ((PCANMSGINFO)pMngtInfo)->Bytes.bAccept );
      };
    }

    //*****************************************************************************
    /// <summary>
    ///   Gets or sets the data length of this CAN message.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">
    ///   The data length value to be set is out of range [0;8].
    /// </exception>
    //*****************************************************************************
    virtual property Byte DataLength 
    {
      /// <summary>
      ///   Gets the data length of this CAN message.
      /// </summary>
      /// <returns>
      ///   The data length of this CAN message.
      /// </returns>
      Byte get(void) sealed
      {
        pin_ptr<mgdCANMSGINFO> pMngtInfo = &m_CanMsg.uMsgInfo;
        return( ((PCANMSGINFO)pMngtInfo)->Bits.dlc );
      };

      /// <summary>
      ///   Sets the data length of this CAN message.
      /// </summary>
      /// <param name="length">
      ///   The data length value of this CAN message to set.
      /// </param>
      /// <exception cref="ArgumentOutOfRangeException">
      ///   The data length value to be set is out of range [0;8].
      /// </exception>
      void set(Byte length) sealed
      {
        if (length <= CAN_SDLC_MAX)
        {
          pin_ptr<mgdCANMSGINFO> pMngtInfo = &m_CanMsg.uMsgInfo;
          ((PCANMSGINFO)pMngtInfo)->Bits.dlc = length;
        }
        else
        {
          throw gcnew ArgumentOutOfRangeException("length");
        }
      };
    }

    //*****************************************************************************
    /// <summary>
    ///   Gets a value indicating whether this message was the last
    ///   message which could be placed in the receive FIFO, before
    ///   this overflows.
    /// </summary>
    //*****************************************************************************
    virtual property bool PossibleOverrun 
    {
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
      bool get(void) sealed
      {
        pin_ptr<mgdCANMSGINFO> pMngtInfo = &m_CanMsg.uMsgInfo;
        return( ((PCANMSGINFO)pMngtInfo)->Bits.ovr != 0 );
      };
    }

    //*****************************************************************************
    /// <summary>
    ///   Gets or sets a value indicating whether this message is the result of
    ///   a self reception request.
    /// </summary>
    //*****************************************************************************
    virtual property bool SelfReceptionRequest 
    {
      /// <summary>
      ///   Gets a value indicating whether this message is the result of
      ///   a self reception request.
      /// </summary>
      /// <returns>
      ///   true if this message is the result of a self reception request, 
      ///   otherwise false.
      /// </returns>
      bool get(void) sealed
      {
        pin_ptr<mgdCANMSGINFO> pMngtInfo = &m_CanMsg.uMsgInfo;
        return( ((PCANMSGINFO)pMngtInfo)->Bits.srr != 0 );
      };

      /// <summary>
      ///   Sets a value indicating whether this message should be send as
      ///   self reception request.
      /// </summary>
      /// <param name="selfReception">
      ///   true if the message should be send as self reception
      ///   request, otherwise false.
      /// </param>
      void set(bool selfReception) sealed
      {
        pin_ptr<mgdCANMSGINFO> pMngtInfo = &m_CanMsg.uMsgInfo;
        ((PCANMSGINFO)pMngtInfo)->Bits.srr = selfReception ? 1 : 0;
      };
    }

    //*****************************************************************************
    /// <summary>
    ///   Gets or sets a value indicating whether this message is a remote
    ///   transmission request.
    /// </summary>
    //*****************************************************************************
    virtual property bool RemoteTransmissionRequest 
    {
      /// <summary>
      ///   Gets a value indicating whether this message is a remote
      ///   transmission request.
      /// </summary>
      /// <returns>
      ///   true if this message is a remote transmission request, 
      ///   otherwise false.
      /// </returns>
      bool get(void) sealed
      {
        pin_ptr<mgdCANMSGINFO> pMngtInfo = &m_CanMsg.uMsgInfo;
        return( ((PCANMSGINFO)pMngtInfo)->Bits.rtr != 0 );
      };

      /// <summary>
      ///   Sets a value indicating whether this message should be send as
      ///   remote transmission request.
      /// </summary>
      /// <param name="remoteRequest">
      ///   true if the message should be send as remote transmission 
      ///   request, otherwise false.
      /// </param>
      void set(bool remoteRequest) sealed
      {
        pin_ptr<mgdCANMSGINFO> pMngtInfo = &m_CanMsg.uMsgInfo;
        ((PCANMSGINFO)pMngtInfo)->Bits.rtr = remoteRequest ? 1 : 0;
      };
    }

    //*****************************************************************************
    /// <summary>
    ///   Gets or sets a value indicating whether this message has extended
    ///   frame format (29-bit id).
    /// </summary>
    //*****************************************************************************
    virtual property bool ExtendedFrameFormat 
    {
      /// <summary>
      ///   Gets a value indicating whether this message has extended
      ///   frame format (29-bit id).
      /// </summary>
      /// <returns>
      ///   true if this message has extended frame format, 
      ///   otherwise false.
      /// </returns>
      bool get(void) sealed
      {
        pin_ptr<mgdCANMSGINFO> pMngtInfo = &m_CanMsg.uMsgInfo;
        return( ((PCANMSGINFO)pMngtInfo)->Bits.ext != 0 );
      };

      /// <summary>
      ///   Sets a value indicating whether this message has extended frame
      ///   format (29-bit id).
      /// </summary>
      /// <param name="extended">
      ///   true if the message has extended frame format, 
      ///   otherwise false.
      /// </param>
      void set(bool extended) sealed
      {
        pin_ptr<mgdCANMSGINFO> pMngtInfo = &m_CanMsg.uMsgInfo;
        ((PCANMSGINFO)pMngtInfo)->Bits.ext = extended ? 1 : 0;
      };
    }

    //*****************************************************************************
    /// <summary>
    ///   Gets or sets the single shot mode bit of the frame.
    /// </summary>
    //*****************************************************************************
    virtual property bool SingleShotMode
    {
      /// <summary>
      ///   Gets a value indicating whether this message has extended
      ///   frame format (29-bit id).
      /// </summary>
      /// <returns>
      ///   true if this message has extended frame format, 
      ///   otherwise false.
      /// </returns>
      bool get(void) sealed
      {
        pin_ptr<mgdCANMSGINFO> pMngtInfo = &m_CanMsg.uMsgInfo;
        return( ((PCANMSGINFO)pMngtInfo)->Bits.ssm != 0 );
      };

      /// <summary>
      ///   Sets a value indicating whether this message has extended frame
      ///   format (29-bit id).
      /// </summary>
      /// <param name="extended">
      ///   true if the message has extended frame format, 
      ///   otherwise false.
      /// </param>
      void set(bool singleshot) sealed
      {
        pin_ptr<mgdCANMSGINFO> pMngtInfo = &m_CanMsg.uMsgInfo;
        ((PCANMSGINFO)pMngtInfo)->Bits.ssm = singleshot ? 1 : 0;
      };
    }

    //*****************************************************************************
    /// <summary>
    ///   Gets or sets the high priority bit of the frame.
    /// </summary>
    //*****************************************************************************
    virtual property bool HighPriorityMsg
    {
      /// <summary>
      ///   Gets a value indicating whether this message has extended
      ///   frame format (29-bit id).
      /// </summary>
      /// <returns>
      ///   true if this message has extended frame format, 
      ///   otherwise false.
      /// </returns>
      bool get(void) sealed
      {
        pin_ptr<mgdCANMSGINFO> pMngtInfo = &m_CanMsg.uMsgInfo;
        return( ((PCANMSGINFO)pMngtInfo)->Bits.hpm != 0 );
      };

      /// <summary>
      ///   Sets a value indicating whether this message has extended frame
      ///   format (29-bit id).
      /// </summary>
      /// <param name="extended">
      ///   true if the message has extended frame format, 
      ///   otherwise false.
      /// </param>
      void set(bool highprio) sealed
      {
        pin_ptr<mgdCANMSGINFO> pMngtInfo = &m_CanMsg.uMsgInfo;
        ((PCANMSGINFO)pMngtInfo)->Bits.hpm = highprio ? 1 : 0;
      };
    }

    //*****************************************************************************
    /// <summary>
    ///   Gets or sets a value indicating whether this message has extended
    ///   data length.
    /// </summary>
    //*****************************************************************************
    virtual property bool ExtendedDataLength
    {
      /// <summary>
      ///   Gets a value indicating whether this message has extended
      ///   data length.
      /// </summary>
      /// <returns>
      ///   true if this message has extended data length, 
      ///   otherwise false.
      /// </returns>
      bool get(void) sealed
      {
        pin_ptr<mgdCANMSGINFO> pMngtInfo = &m_CanMsg.uMsgInfo;
        return( ((PCANMSGINFO)pMngtInfo)->Bits.edl != 0 );
      };

      /// <summary>
      ///   Sets a value indicating whether this message has extended 
      ///   data length.
      /// </summary>
      /// <param name="extended">
      ///   true if the message has extended data length, 
      ///   otherwise false.
      /// </param>
      void set(bool extdatalen) sealed
      {
        pin_ptr<mgdCANMSGINFO> pMngtInfo = &m_CanMsg.uMsgInfo;
        ((PCANMSGINFO)pMngtInfo)->Bits.edl = extdatalen ? 1 : 0;
      };
    }

    //*****************************************************************************
    /// <summary>
    ///   Gets or sets the fast data rate bit of the frame.
    /// </summary>
    //*****************************************************************************
    virtual property bool FastDataRate
    {
      /// <summary>
      ///   Gets a value indicating whether this message has extended
      ///   frame format (29-bit id).
      /// </summary>
      /// <returns>
      ///   true if this message has extended frame format, 
      ///   otherwise false.
      /// </returns>
      bool get(void) sealed
      {
        pin_ptr<mgdCANMSGINFO> pMngtInfo = &m_CanMsg.uMsgInfo;
        return( ((PCANMSGINFO)pMngtInfo)->Bits.fdr != 0 );
      };

      /// <summary>
      ///   Sets a value indicating whether this message has extended frame
      ///   format (29-bit id).
      /// </summary>
      /// <param name="extended">
      ///   true if the message has extended frame format, 
      ///   otherwise false.
      /// </param>
      void set(bool fastdata) sealed
      {
        pin_ptr<mgdCANMSGINFO> pMngtInfo = &m_CanMsg.uMsgInfo;
        ((PCANMSGINFO)pMngtInfo)->Bits.fdr = fastdata ? 1 : 0;
      };
    }

    //*****************************************************************************
    /// <summary>
    ///   Gets or sets the fast error state indicator of the frame.
    /// </summary>
    //*****************************************************************************
    virtual property bool ErrorStateIndicator
    {
      /// <summary>
      ///   Gets a value indicating whether this message has extended
      ///   frame format (29-bit id).
      /// </summary>
      /// <returns>
      ///   true if this message has extended frame format, 
      ///   otherwise false.
      /// </returns>
      bool get(void) sealed
      {
        pin_ptr<mgdCANMSGINFO> pMngtInfo = &m_CanMsg.uMsgInfo;
        return( ((PCANMSGINFO)pMngtInfo)->Bits.esi != 0 );
      };

      /// <summary>
      ///   Sets a value indicating whether this message has extended frame
      ///   format (29-bit id).
      /// </summary>
      /// <param name="extended">
      ///   true if the message has extended frame format, 
      ///   otherwise false.
      /// </param>
      void set(bool errorstate) sealed
      {
        pin_ptr<mgdCANMSGINFO> pMngtInfo = &m_CanMsg.uMsgInfo;
        ((PCANMSGINFO)pMngtInfo)->Bits.esi = errorstate ? 1 : 0;
      };
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
    ///     ...
    ///
    ///     for (Byte i = 0; i < canMsg.DataLength; i++)
    ///     {
    ///       canMsg[i] = i;
    ///     }
    ///   </code>
    /// </example>
    //*****************************************************************************
    virtual property Byte default[int] 
    {
      /// <summary>
      ///   Gets a single data byte at the specified index from this 
      ///   CAN message.
      /// </summary>
      /// <param name="index">
      ///   Index of the data byte to get.
      /// </param>
      /// <returns>
      ///   The data byte at the specified index.
      /// </returns>
      /// <exception cref="ArgumentOutOfRangeException">
      ///   The specified index is out of range [0;<c>DataLength</c>].
      /// </exception>
      Byte get(Int32 index) sealed
      {
        if ((index >= 0) && (index < CAN_SDLC_MAX))
        {
          pin_ptr<mgdCANMSG> pMngtMsg = &m_CanMsg;
          return( ((PCANMSG)pMngtMsg)->abData[index] );
        }
        else
        {
          throw gcnew ArgumentOutOfRangeException("index");
        }
      };

      /// <summary>
      ///   Sets a single data byte at the specified index within this
      ///   CAN message.
      /// </summary>
      /// <param name="index">
      ///   Index of the data byte to set
      /// </param>
      /// <param name="value">
      ///   Value for the data byte
      /// </param>
      /// <exception cref="ArgumentOutOfRangeException">
      ///   The specified index is out of range [0;<c>DataLength</c>].
      /// </exception>
      void set(Int32 index, Byte value) sealed
      {
        if ((index >= 0) && (index < CAN_SDLC_MAX))
        {
          pin_ptr<mgdCANMSG> pMngtMsg = &m_CanMsg;
          ((PCANMSG)pMngtMsg)->abData[index] = value;
        }
        else
        {
          throw gcnew ArgumentOutOfRangeException("index");
        }
      };
    }

  //--------------------------------------------------------------------
  // member functions
  //--------------------------------------------------------------------
  private:
    //*****************************************************************************
    /// <summary>
    ///   Static constructor for initialization of static members
    /// </summary>
    //*****************************************************************************
    static CanMessage()
    {
      ms_EmptyMsg.Clear();
    }

  public:
    //*****************************************************************************
    /// <summary>
    ///   This method clears the contents of this CAN message.
    /// </summary>
    //*****************************************************************************
    virtual void Clear()
    {
      static CANMSG Empty = {0};
      pin_ptr<mgdCANMSG> pMsg = &m_CanMsg;
      *(PCANMSG)pMsg = Empty;
    };

    //*****************************************************************************
    /// <summary>
    ///   Determines whether the specified Object is equal to the current Object.
    /// </summary>
    /// <pararm name ="obj">
    ///   The Object to compare with the current Object.
    /// </pararm>
    /// <returns>
    ///   true if the specified Object is equal to the current Object; 
    ///   otherwise, false.
    /// </returns>
    //*****************************************************************************
    virtual bool Equals(Object^ obj) override
    {
      // Check for null values and compare run-time types.
      if (obj == nullptr || GetType() != obj->GetType()) 
        return false;

      CanMessage^ p = (CanMessage^)obj;
      size_t length = sizeof(mgdCANMSG);

      if (CanMsgFrameType::Data == p->FrameType)
      {
        length += p->DataLength;
      }

      mgdCANMSG msg = p->ToCANMSG();
      pin_ptr<mgdCANMSG> pMsg1 =  &msg;
      pin_ptr<mgdCANMSG> pMsg2 =  &m_CanMsg;

      return(0 == memcmp(pMsg1, pMsg2, length));
    }

    //*****************************************************************************
    /// <summary>
    ///   Serves as a hash function for a particular type. GetHashCode is suitable 
    ///   for use in hashing algorithms and data structures like a hash table.
    /// </summary>
    /// <returns>
    ///   A hash code for the current Object. 
    /// </returns>
    //*****************************************************************************
    virtual int GetHashCode () override
    {
      return Identifier;
    }

    //*****************************************************************************
    /// <summary>
    ///   This method returns a String that represents the current object.
    /// </summary>
    /// <returns>
    ///   A String that represents the current object.
    /// </returns>
    //*****************************************************************************
    virtual String^ ToString() override
    {
      String^ msg = nullptr;

      switch(FrameType)
      {
      case CanMsgFrameType::Data:
        {
          System::Text::StringBuilder^ builder = gcnew System::Text::StringBuilder();
          String^ type = RemoteTransmissionRequest ? "RTR" : "Data";
          builder->AppendFormat("{0} : {1} [{2:D3}] Dlc={3}", TimeStamp, type, Identifier, DataLength);
          if(!RemoteTransmissionRequest)
          {
            for(int iDataIdx = 0; iDataIdx < Math::Min(DataLength, (Byte)CAN_SDLC_MAX); ++iDataIdx)
            {
              builder->AppendFormat(" {0:X2}", default[iDataIdx]);
            }
          }
          msg = builder->ToString();
        }
        break;
      case CanMsgFrameType::Info:
        msg = String::Format("{0} : Info {1}", TimeStamp, (CanMsgInfoValue)default[0]);
        break;
      case CanMsgFrameType::Error:
        msg = String::Format("{0} : Error {1}", TimeStamp, (CanMsgError)default[0]);
        break;
      case CanMsgFrameType::Status:
        msg = String::Format("{0} : Status {1}", TimeStamp, (CanCtrlStatus)default[0]);
        break;
      case CanMsgFrameType::TimeReset:
        msg = String::Format("{0} : TimeReset", TimeStamp);
        break;
      case CanMsgFrameType::TimeOverrun:
        msg = String::Format("{0} : TimeOverrun : Count={1}", TimeStamp, Identifier);
        break;
      case CanMsgFrameType::Wakeup:
        msg = String::Format("{0} : Wakeup", TimeStamp);
        break;
      }

      return msg;
    };

    mgdCANMSG ToCANMSG()
    {
      return m_CanMsg;
    }

    mgdCANMSG2 ToCANMSG2()
    {
      mgdCANMSG2 local;

      local.dwMsgId = m_CanMsg.dwMsgId;
      local.dwTime = m_CanMsg.dwTime;
      local.uMsgInfo = m_CanMsg.uMsgInfo;
      local.bData1 = m_CanMsg.bData1;
      local.bData2 = m_CanMsg.bData2;
      local.bData3 = m_CanMsg.bData3;
      local.bData4 = m_CanMsg.bData4;
      local.bData5 = m_CanMsg.bData5;
      local.bData6 = m_CanMsg.bData6;
      local.bData7 = m_CanMsg.bData7;
      local.bData8 = m_CanMsg.bData8;

      return local;
    }

    void SetValue(mgdCANMSG rawmsg)
    {
      m_CanMsg = rawmsg;
    }
};

} // end of namespace Can
} // end of namespace Bal
} // end of namespace Vci4
} // end of namespace Ixxat

