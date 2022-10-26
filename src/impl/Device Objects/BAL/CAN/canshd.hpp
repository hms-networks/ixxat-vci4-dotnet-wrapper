// SPDX-License-Identifier: MIT
//----------------------------------------------------------------------------
// Summary  : Declarations for the CAN message scheduler class.
// Copyright: Copyright (C) 2016-2022 HMS Technology Center Ravensburg GmbH, 
//            all rights reserved
//----------------------------------------------------------------------------

#pragma once

#include <vcisdk.h>
#include "canshd.hpp"
#include "cansoc.hpp"
#include "canmsg.hpp"


namespace Ixxat {
  namespace Vci4 {
    namespace Bal {
      namespace Can {

using namespace System::Reflection;

// forward decls
ref class CanScheduler;

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
public ref class  CanCyclicTXMsg sealed
  : public ICanCyclicTXMsg
{
  //--------------------------------------------------------------------
  // member variables
  //--------------------------------------------------------------------
  internal:
    CanScheduler^       m_pCanShd; // pointer to the native scheduler object
    mgdCANCYCLICTXMSG   m_CanMsg;  // cyclic CAN message structure
    UInt16              m_wHandle; // handle of the cyclic transmit message
    CanCyclicTXStatus   m_eStatus; // current message status
    bool                m_isDirty; // if it is dirty we have to create a new object on next Start()

    //--------------------------------------------------------------------
    // ICanCyclicTXMsg implementation
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
        return( 0 );
      };

      /// <summary>
      ///   Sets the time stamp of this CAN message.
      /// </summary>
      /// <param name="timestamp">
      ///   Time stamp value to set.
      /// </param>
      void set(UInt32 timestamp) sealed
      {
        // do nothing
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
          pin_ptr<mgdCANCYCLICTXMSG> pMngtMsg = &m_CanMsg;
          return( ((PCANCYCLICTXMSG)pMngtMsg)->abData[index] );
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
          pin_ptr<mgdCANCYCLICTXMSG> pMngtMsg = &m_CanMsg;
          ((PCANCYCLICTXMSG)pMngtMsg)->abData[index] = value;
        }
        else
        {
          throw gcnew ArgumentOutOfRangeException("index");
        }
      };
    }


    virtual property CanCyclicTXStatus  Status                    { CanCyclicTXStatus get(void); };

    virtual property UInt16             CycleTicks                { UInt16 get(void); 
                                                                    void set(UInt16 ticks); };

    virtual property CanCyclicTXIncMode AutoIncrementMode         { CanCyclicTXIncMode get(void); 
                                                                    void set(CanCyclicTXIncMode  mode); };

    virtual property Byte               AutoIncrementIndex        { Byte get(void); 
                                                                    void set(Byte index); };


    virtual void Start      ( UInt16 repeatCount );
    virtual void Stop       ( void );
    virtual void Reset      ( void );

  //--------------------------------------------------------------------
  // member functions
  //--------------------------------------------------------------------
  internal:
    void SetStat    ( CanCyclicTXStatus status );
    void Cleanup    ( void );
    ~CanCyclicTXMsg ( );

  public:
    //*****************************************************************************
    /// <summary>
    ///   This method stops/removes and clears the contents of this CAN message.
    ///   Same as Reset().
    /// </summary>
    //*****************************************************************************
    virtual void Clear()
    {
      // stop/remove and cleanup
      Reset();
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

      CanCyclicTXMsg^ p = (CanCyclicTXMsg^)obj;
      size_t length = sizeof(mgdCANCYCLICTXMSG);

      if (CanMsgFrameType::Data == p->FrameType)
      {
        length += p->DataLength;
      }

      mgdCANCYCLICTXMSG msg = p->m_CanMsg;
      pin_ptr<mgdCANCYCLICTXMSG> pMsg1 =  &msg;
      pin_ptr<mgdCANCYCLICTXMSG> pMsg2 =  &m_CanMsg;

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
    virtual int GetHashCode() override
    {
      return Identifier;
    }

  public:
    CanCyclicTXMsg  ( CanScheduler^ pSched );
};


//*****************************************************************************
/// <summary>
///   This class implements the CAN scheduler.
/// </summary>
//*****************************************************************************
private ref class CanScheduler : public CanSocket
                               , public ICanScheduler
{
  //--------------------------------------------------------------------
  // member variables
  //--------------------------------------------------------------------
  private:
    ::ICanScheduler*          m_pCanShd;  // native scheduler object
    array<CanCyclicTXMsg^>^   m_aCtxMsg;  // array for cyclic TX messages


  //--------------------------------------------------------------------
  // member functions
  //--------------------------------------------------------------------
  private:
    HRESULT InitNew           ( ::ICanScheduler* pCanShd );
    void    Cleanup           ( void );
    void    ResetScheduler    ( void );

  internal:
    CanScheduler    ( ::IBalObject* pBalObj
                    , Byte          portNumber 
                    , Byte          busTypeIndex);
   ~CanScheduler    ( void );


  //--------------------------------------------------------------------
  // ICanScheduler implementation
  //--------------------------------------------------------------------
  public:
    virtual void Suspend     ( void );
    virtual void Resume      ( void );
    virtual void Reset       ( void );
    virtual void UpdateStatus( void );
    virtual ICanCyclicTXMsg^ AddMessage( void );

  internal:
    void InternalAddMessage( CanCyclicTXMsg^ cyclicTXMessage );
    void InternalRemMessage  ( CanCyclicTXMsg^ cyclicTXMessage );
    void InternalStartMessage( CanCyclicTXMsg^ cyclicTXMessage, UInt16 repeatCount );
    void InternalStopMessage ( CanCyclicTXMsg^ cyclicTXMessage );
};

} // end of namespace Can
} // end of namespace Bal
} // end of namespace Vci4
} // end of namespace Ixxat
