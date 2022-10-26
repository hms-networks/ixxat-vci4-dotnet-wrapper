/*****************************************************************************
 IXXAT Automation GmbH
******************************************************************************
 Summary : Declarations for the LIN message class.
 Compiler: Microsoft VC++ 8
******************************************************************************
 all rights reserved
*****************************************************************************/

#pragma once

#include <vcisdk.h>


namespace Ixxat {
  namespace Vci4 {
    namespace Bal {
      namespace Lin {

using namespace System;
using namespace System::Reflection;
using namespace System::Runtime::InteropServices;
using namespace System::Text;


//*****************************************************************************
/// <summary>
///   This value class represents a LIN message. LIN messages can be received
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

public value class LinMessage : public ILinMessage
{
  //--------------------------------------------------------------------
  // member variables
  //--------------------------------------------------------------------
  private:
    static LinMessage ms_EmptyMsg;

  internal:
    mgdLINMSG m_LinMsg; // LIN message structure

  //--------------------------------------------------------------------
  // properties
  //--------------------------------------------------------------------
  public:
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
    virtual property UInt32 TimeStamp 
    {
      /// <summary>
      ///   Gets the time stamp of this message.
      /// </summary>
      /// <returns>
      ///   The time stamp of this message.
      /// </returns>
      UInt32 get(void) sealed
      {
        return( m_LinMsg.dwTime );
      };

      /// <summary>
      ///   Sets the time stamp of this message.
      /// </summary>
      /// <param name="timestamp">
      ///   Time stamp value to set.
      /// </param>
      void set(UInt32 timestamp) sealed
      {
        m_LinMsg.dwTime = timestamp;
      };
    }

    //*****************************************************************************
    /// <summary>
    ///   Gets or sets the protected identifier of this LIN message.
    /// </summary>
    //*****************************************************************************
    virtual property Byte ProtId 
    {
      /// <summary>
      ///   Gets the protected identifier of this message.
      /// </summary>
      /// <returns>
      ///   The CAN protected identifier of this message.
      /// </returns>
      Byte get(void) sealed
      {
        return( m_LinMsg.uMsgInfo.bPid );
      };

      /// <summary>
      ///   Sets the protected identifier of this message.
      /// </summary>
      /// <param name="protId">
      ///   Protected identifier value to set.
      /// </param>
      void set(Byte protId) sealed
      {
        m_LinMsg.uMsgInfo.bPid = protId;
      };
    }

    //*****************************************************************************
    /// <summary>
    ///   Gets a value indicating the frame type of this message.
    /// </summary>
    //*****************************************************************************
    virtual property LinMessageType MessageType 
    {
      /// <summary>
      ///   Gets a value indicating the message type of this message.
      /// </summary>
      /// <returns>
      ///   A value indicating the message type of this message.
      /// </returns>
      LinMessageType get(void) sealed
      {
        return (LinMessageType)m_LinMsg.uMsgInfo.bType;
      };

      /// <summary>
      ///   Sets a value indicating the message type of this message.
      /// </summary>
      /// <paramref name="messageType">
      ///   The message frame type to set.
      /// </paramref>
      void set(LinMessageType messageType) sealed
      {
        m_LinMsg.uMsgInfo.bType = (UINT8)messageType;
      }
    }

    //*****************************************************************************
    /// <summary>
    ///   Gets or sets the data length of this message.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">
    ///   The data length value to be set is out of range [0;8].
    /// </exception>
    //*****************************************************************************
    virtual property Byte DataLength 
    {
      /// <summary>
      ///   Gets the data length of this message.
      /// </summary>
      /// <returns>
      ///   The data length of this message.
      /// </returns>
      Byte get(void) sealed
      {
        return m_LinMsg.uMsgInfo.bDlen;
      };

      /// <summary>
      ///   Sets the data length of this message.
      /// </summary>
      /// <param name="length">
      ///   The data length value of this message to set.
      /// </param>
      /// <exception cref="ArgumentOutOfRangeException">
      ///   The data length value to be set is out of range [0;8].
      /// </exception>
      void set(Byte length) sealed
      {
        if (length <= 8)
        {
          m_LinMsg.uMsgInfo.bDlen = length;
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
        pin_ptr<mgdLINMSGINFO> pMngtInfo = &m_LinMsg.uMsgInfo;
        return( ((PLINMSGINFO)pMngtInfo)->Bits.ovr != 0 );
      };
    }

    //*****************************************************************************
    /// <summary>
    ///   Gets or sets a value indicating if it is a message with extended
    ///   checksum in accordance with LIN 2.0.
    /// </summary>
    //*****************************************************************************
    virtual property bool ExtendedCrc 
    {
      /// <summary>
      ///   Gets a value indicating if it is a message with extended
      ///   checksum in accordance with LIN 2.0.
      /// </summary>
      /// <returns>
      ///   A value indicating if it is a message with extended
      ///   checksum in accordance with LIN 2.0.
      /// </returns>
      bool get(void) sealed
      {
        pin_ptr<mgdLINMSGINFO> pMngtInfo = &m_LinMsg.uMsgInfo;
        return( ((PLINMSGINFO)pMngtInfo)->Bits.ecs != 0 );
      };

      /// <summary>
      ///   Sets a value indicating if it is a message with extended
      ///   checksum in accordance with LIN 2.0.
      /// </summary>
      /// <param name="value" >
      ///   A value indicating if it is a message with extended
      ///   checksum in accordance with LIN 2.0.
      /// </param>
      void set(bool value) sealed
      {
        pin_ptr<mgdLINMSGINFO> pMngtInfo = &m_LinMsg.uMsgInfo;
        ((PLINMSGINFO)pMngtInfo)->Bits.ecs = value ? 1 : 0;
      }
    }

    //*****************************************************************************
    /// <summary>
    ///   Gets a value indicating that the LIN controller itself transmitted the 
    ///   message, i.e. with messages for which the controller has an entry
    ///   in the response table.
    /// </summary>
    //*****************************************************************************
    virtual property bool SenderOfResponse 
    {
      /// <summary>
      ///   Gets a value indicating that the LIN controller itself transmitted the 
      ///   message.
      /// </summary>
      /// <returns>
      ///   A value indicating that the LIN controller itself transmitted the 
      ///   message.
      /// </returns>
      bool get(void) sealed
      {
        pin_ptr<mgdLINMSGINFO> pMngtInfo = &m_LinMsg.uMsgInfo;
        return( ((PLINMSGINFO)pMngtInfo)->Bits.sor != 0 );
      };

      /// <summary>
      ///   Set a value indicating that the LIN controller itself should
      //    transmit the response when the identifier of this message
      //    was received.
      /// </summary>
      /// <returns>
      ///   A value indicating that the LIN controller itself transmitted the 
      ///   message.
      /// </returns>
      void set(bool value) sealed
      {
        pin_ptr<mgdLINMSGINFO> pMngtInfo = &m_LinMsg.uMsgInfo;
        ((PLINMSGINFO)pMngtInfo)->Bits.sor = value ? 1 : 0;
      }
    }

    //*****************************************************************************
    /// <summary>
    ///   Gets or sets a value indicating if the message should be sent without
    ///   data (only with identifier).
    /// </summary>
    //*****************************************************************************
    virtual property bool IdOnly
    {
      /// <summary>
      ///   Gets a value indicating if the message should be sent without data.
      /// </summary>
      /// <returns>
      ///   A value indicating if the message should be sent without data.
      /// </returns>
      bool get(void) sealed
      {
        pin_ptr<mgdLINMSGINFO> pMngtInfo = &m_LinMsg.uMsgInfo;
        return( ((PLINMSGINFO)pMngtInfo)->Bits.ido != 0 );
      };

      /// <summary>
      ///   Sets a value indicating if the message should be sent without data.
      /// </summary>
      /// <param name="value" >
      ///   A value indicating if the message should be sent without data.
      /// </param>
      void set(bool value) sealed
      {
        pin_ptr<mgdLINMSGINFO> pMngtInfo = &m_LinMsg.uMsgInfo;
        ((PLINMSGINFO)pMngtInfo)->Bits.ido = value ? 1 : 0;
      }
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
    virtual property Byte default[int]
    {
      /// <summary>
      ///   Gets a single data byte at the specified index from this 
      ///   message.
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
      Byte get(int index) sealed
      {
        if ((index >= 0) && (index < 8))
        {
          pin_ptr<mgdLINMSG> pMngtMsg = &m_LinMsg;
          return( ((PLINMSG)pMngtMsg)->abData[index] );
        }
        else
        {
          throw gcnew ArgumentOutOfRangeException("index");
        }
      };

      /// <summary>
      ///   Sets a single data byte at the specified index within this
      ///   message.
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
        if ((index >= 0) && (index < 8))
        {
          pin_ptr<mgdLINMSG> pMngtMsg = &m_LinMsg;
          ((PLINMSG)pMngtMsg)->abData[index] = value;
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
    static LinMessage()
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
      static LINMSG Empty = {0};
      pin_ptr<mgdLINMSG> pMsg = &m_LinMsg;
      *(PLINMSG)pMsg = Empty;
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

      LinMessage^ p = (LinMessage^)obj;

      pin_ptr<mgdLINMSG> pMsg1 = &m_LinMsg;
      pin_ptr<mgdLINMSG> pMsg2 = &p->m_LinMsg;

      size_t length = sizeof(mgdLINMSG);
      if (LinMessageType::Data == MessageType)
      {
        length += DataLength;
      }

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
      return ProtId;
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

      switch(MessageType)
      {
      case LinMessageType::Data:
        {
          StringBuilder^ builder = gcnew StringBuilder();
          builder->AppendFormat("{0} : Data [{1:D3}]", TimeStamp, ProtId);
          for(int iDataIdx = 0; iDataIdx < DataLength; ++iDataIdx)
          {
            builder->AppendFormat(" {0:X2}", default[iDataIdx]);
          }
          msg = builder->ToString();
        }
        break;
      case LinMessageType::Info:
        msg = String::Format("{0} : Info {1}", TimeStamp, (LinMsgInfoValue)default[0]);
        break;
      case LinMessageType::Error:
        msg = String::Format("{0} : Error {1}", TimeStamp, (LinMsgError)default[0]);
        break;
      case LinMessageType::Status:
        msg = String::Format("{0} : Status {1}", TimeStamp, (LinCtrlStatus)default[0]);
        break;
      case LinMessageType::Sleep:
        msg = String::Format("{0} : Sleep", TimeStamp);
        break;
      case LinMessageType::TimeOverrun:
        msg = String::Format("{0} : TimeOverrun : Count={1}", TimeStamp, DataLength);
        break;
      case LinMessageType::Wakeup:
        msg = String::Format("{0} : Wakeup", TimeStamp);
        break;
      }

      return msg;
    };

    virtual mgdLINMSG ToValue() 
    {
      return m_LinMsg;
    }

    virtual void SetValue(mgdLINMSG rawmsg)
    {
      m_LinMsg = rawmsg;
    }
};

} // end of namespace Lin
} // end of namespace Bal
} // end of namespace Vci4
} // end of namespace Ixxat

