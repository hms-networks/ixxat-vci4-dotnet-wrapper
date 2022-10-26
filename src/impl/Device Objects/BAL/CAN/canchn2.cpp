// SPDX-License-Identifier: MIT
//----------------------------------------------------------------------------
// Summary  : Implementation of the CAN channel class.
// Copyright: Copyright (C) 2016-2022 HMS Technology Center Ravensburg GmbH, 
//            all rights reserved
//----------------------------------------------------------------------------

#include "canchn2.hpp"
#include "vcinet.hpp"

using namespace Ixxat::Vci4::Bal::Can;

#pragma warning(disable:4669) // 'type cast' : unsafe conversion


//*****************************************************************************
/// <summary>
///   Constructor for VCI CAN channel objects.
/// </summary>
/// <param name="pBalObj">
///   Pointer to the native BAL object interface.
///   This parameter must not be NULL.
/// </param>
/// <param name="bPortNo">
///   Port number of the bus socket to open.
/// </param>
/// <param name="busTypeIndex">
///   Bus type related port number
///</param>
//*****************************************************************************
CanChannel2::CanChannel2( ::IBalObject* pBalObj
                      , Byte          bPortNo
                      , Byte          busTypeIndex)
          : CanSocket2(pBalObj, bPortNo, busTypeIndex)
{
  m_pCanChn = NULL;

  // FxCop: "Do not make initializations,
  // that have already been done by the runtime."
  // m_fExOpen = false; 
} 

//*****************************************************************************
/// <summary>
///   Destructor for VCI CAN channel objects.
/// </summary>
//*****************************************************************************
CanChannel2::~CanChannel2()
{
  Cleanup();
}

//*****************************************************************************
/// <summary>
///   This method initializes the channel object.
/// </summary>
/// <param name="fExclusive">
///   If this parameter is set to true the method tries to
///   create the channel in exclusive mode, otherwise the
///   method creates a shared CAN message channel.
/// </param>
/// <returns>
///   true on success, otherwise false.
/// </returns>
/// <exception cref="VciException">
///   Creation of native CAN channel failed.
/// </exception>
/// <exception cref="ArgumentNullException">
///   Native ICanSocket was NULL.
/// </exception>
//*****************************************************************************
bool CanChannel2::InitNew(bool fExclusive)
{
  HRESULT         hResult;
  ::ICanSocket2*   pSocket;
  ::ICanChannel2*  pCanChn;

  if ((nullptr == m_pCanChn) || (fExclusive != m_fExOpen))
  {
    Cleanup();

    pSocket = GetNativeSocket();
    if (nullptr != pSocket)
    {
      hResult = pSocket->CreateChannel(fExclusive, &pCanChn);
      pSocket->Release();

      if (hResult == VCI_OK)
      {
        m_fExOpen = fExclusive;
        m_pCanChn = pCanChn;
      }
      else
      {
        throw gcnew VciException(VciServerImpl::Instance(), hResult);
      }
    }
  }

  return( nullptr != m_pCanChn );
}

//*****************************************************************************
/// <summary>
///   This method performs tasks associated with freeing, releasing, or
///   resetting unmanaged resources.
/// </summary>
//*****************************************************************************
void CanChannel2::Cleanup(void)
{
  if (nullptr != m_pCanChn)
  {
    m_pCanChn->Release();
    m_pCanChn = nullptr;
    m_fExOpen = false;
  }
}

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
void CanChannel2::Initialize( UInt16 receiveFifoSize
                           , UInt16 transmitFifoSize
                           , UINT32 filterSize
                           , CanFilterModes filterMode
                           , bool   exclusive)
{
  HRESULT hResult;
  bool    fResult;

  fResult = InitNew(exclusive);
  if (true == fResult)
  {
    hResult = m_pCanChn->Initialize(receiveFifoSize, transmitFifoSize, filterSize, (UINT8)filterMode);
    if (hResult != VCI_OK)
    {
      throw gcnew VciException(VciServerImpl::Instance(), hResult);
    }
  }
  else
  {
    throw gcnew ObjectDisposedException(this->GetType()->FullName);
  }
}

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
///   transmitted from/to the CAN line (see also ICanControl::StartLine).
/// </remarks>
//*****************************************************************************
void CanChannel2::Activate(void)
{
  HRESULT hResult;

  if (nullptr != m_pCanChn)
  {
    hResult = m_pCanChn->Activate();
    if (hResult != VCI_OK)
    {
      throw gcnew VciException(VciServerImpl::Instance(), hResult);
    }
  }
  else
  {
    throw gcnew ObjectDisposedException(this->GetType()->FullName);
  }
}

//*****************************************************************************
/// <summary>
///   This method deactivates the CAN channel. After deactivating the channel,
///   no further CAN messages are transmitted or received to/from the CAN line.
/// </summary>
/// <returns>
///   true on success, otherwise false.
/// </returns>
/// <exception cref="VciException">
///   Channel deactivation failed.
/// </exception>
/// <exception cref="ObjectDisposedException">
///   Object is already disposed or not initialized, yet.
/// </exception>
//*****************************************************************************
void CanChannel2::Deactivate(void)
{
  HRESULT hResult;

  if (nullptr != m_pCanChn)
  {
    hResult = m_pCanChn->Deactivate();
    if (hResult != VCI_OK)
    {
      throw gcnew VciException(VciServerImpl::Instance(), hResult);
    }
  }
  else
  {
    throw gcnew ObjectDisposedException(this->GetType()->FullName);
  }
}

//*****************************************************************************
/// <summary>
///   Gets the current status of the CAN channel.
/// </summary>
/// <returns>
///   The current status of the CAN channel.
/// </returns>
/// <exception cref="VciException">
///   Getting channel status failed.
/// </exception>
/// <exception cref="ObjectDisposedException">
///   Object is already disposed or not initialized, yet.
/// </exception>
//*****************************************************************************
CanChannelStatus CanChannel2::ChannelStatus::get()
{
  HRESULT        hResult;

  if (nullptr != m_pCanChn)
  {
    CANCHANSTATUS2 sStatus;
    hResult = m_pCanChn->GetStatus(&sStatus);
    if (hResult != VCI_OK)
    {
      throw gcnew VciException(VciServerImpl::Instance(), hResult);
    }

    return CanChannelStatus(sStatus.fActivated, sStatus.fRxOverrun, sStatus.bRxFifoLoad, sStatus.bTxFifoLoad);
  }
  else
  {
    throw gcnew ObjectDisposedException(this->GetType()->FullName);
  }
}

//*****************************************************************************
/// <summary>
///   Gets a reference to the message reader of the channel which provides
///   access to the channel's receive buffer.
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
ICanMessageReader^ CanChannel2::GetMessageReader()
{
  CanMessageReader^ pReader = nullptr;

  if (nullptr != m_pCanChn)
  {
    pReader = gcnew CanMessageReader(m_pCanChn);
  }
  else
  {
    throw gcnew ObjectDisposedException(this->GetType()->FullName);
  }

  return( pReader );
}

//*****************************************************************************
/// <summary>
///   Gets a reference to the message writer of the channel which provides
///   access to the channel's transmit buffer.
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
ICanMessageWriter^ CanChannel2::GetMessageWriter()
{
  CanMessageWriter^ pWriter = nullptr;

  if (nullptr != m_pCanChn)
  {
    pWriter = gcnew CanMessageWriter(m_pCanChn);
  }
  else
  {
    throw gcnew ObjectDisposedException(this->GetType()->FullName);
  }

  return( pWriter );
}

//*****************************************************************************
/// <summary>
///   This method returns the set filter mode for the given selection.
/// </summary>
/// <param name="bSelect">
///   Filter selection. This parameter can be either <c>CanFilter::Std</c>
///   to select the 11-bit acceptance filter, or <c>CanFilter::Ext</c> to
///   select the 29-bit acceptance filter.
/// </param>
/// <exception cref="ObjectDisposedException">
///   Object is already disposed or not initialized, yet.
/// </exception>
//*****************************************************************************
UINT8 CanChannel2::GetFilterMode(CanFilter bSelect)
{
  HRESULT           hResult;
  UINT8 bMode;

  if (nullptr != m_pCanChn)
  {
    hResult = m_pCanChn->GetFilterMode((UINT8)bSelect, &bMode);
    if (hResult != VCI_OK)
    {
      throw gcnew VciException(VciServerImpl::Instance(), hResult);
    }
  }
  else
  {
    throw gcnew ObjectDisposedException(this->GetType()->FullName);
  }

  return( bMode );
}


//*****************************************************************************
/// <summary>
///   This method sets the filter mode for the given selection.
/// </summary>
/// <param name="bSelect">
///   Filter selection. This parameter can be either <c>CanFilter::Std</c>
///   to select the 11-bit acceptance filter, or <c>CanFilter::Ext</c> to
///   select the 29-bit acceptance filter.
/// </param>
/// <exception cref="ObjectDisposedException">
///   Object is already disposed or not initialized, yet.
/// </exception>
//*****************************************************************************
UINT8 CanChannel2::SetFilterMode(CanFilter      bSelect,
                                 CanFilterModes bMode)
{
  HRESULT           hResult;
  UINT8 bPrev;

  if (nullptr != m_pCanChn)
  {
    hResult = m_pCanChn->SetFilterMode((UINT8)bSelect, (UINT8)bMode, &bPrev);
    if (hResult != VCI_OK)
    {
      throw gcnew VciException(VciServerImpl::Instance(), hResult);
    }
  }
  else
  {
    throw gcnew ObjectDisposedException(this->GetType()->FullName);
  }

  return( bPrev );
}

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
///   Filter selection. This parameter can be either <c>CanFilter::Std</c>
///   to select the 11-bit acceptance filter, or <c>CanFilter::Ext</c> to
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
///   select = CanFilter::Std
///   
///        +----+----+----+----+ ~ +----+----+ ~ +---+---+---+---+
///    bit | 31 | 30 | 29 | 28 |   | 13 | 12 |   | 3 | 2 | 1 | 0 |
///        +----+----+----+----+ ~ +----+----+ ~ +---+---+---+---+
///        |  0 |  0 |  0 |  0 |   |  0 |ID11|   |ID2|ID1|ID0|RTR|
///        +----+----+----+----+ ~ +----+----+ ~ +---+---+---+---+
///   
///   select = CanFilter::Ext
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
void CanChannel2::SetAccFilter(CanFilter bSelect,
                               UINT32    dwCode,
                               UINT32    dwMask)
{
  HRESULT           hResult;

  if (nullptr != m_pCanChn)
  {
    hResult = m_pCanChn->SetAccFilter((UINT8)bSelect, dwCode, dwMask);
    if (hResult != VCI_OK)
    {
      throw gcnew VciException(VciServerImpl::Instance(), hResult);
    }
  }
  else
  {
    throw gcnew ObjectDisposedException(this->GetType()->FullName);
  }
}

//*****************************************************************************
/// <summary>
///   This method registers the specified CAN message identifier or group
///   of identifiers at the specified filter list. IDs registered within the
///   filter list are accepted for reception. The method can only be called 
///   if the CAN controller is in 'init' mode.
/// </summary>
/// <param name="bSelect">
///   Filter selection. This parameter can be either <c>CanFilter::Std</c>
///   to select the 11-bit filter list, or <c>CanFilter::Ext</c> to
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
void CanChannel2::AddFilterIds(CanFilter bSelect,
                               UINT32    dwCode,
                               UINT32    dwMask)
{
  HRESULT           hResult;

  if (nullptr != m_pCanChn)
  {
    hResult = m_pCanChn->AddFilterIds((UINT8)bSelect, dwCode, dwMask);
    if (hResult != VCI_OK)
    {
      throw gcnew VciException(VciServerImpl::Instance(), hResult);
    }
  }
  else
  {
    throw gcnew ObjectDisposedException(this->GetType()->FullName);
  }
}

//*****************************************************************************
/// <summary>
///   This method removes the specified CAN message identifier or group
///   of identifiers from the specified filter list. The method can only be
///   called if the CAN controller is in 'init' mode.
/// </summary>
/// <param name="bSelect">
///   Filter selection. This parameter can be either <c>CanFilter::Std</c>
///   to select the 11-bit filter list, or <c>CanFilter::Ext</c> to
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
void CanChannel2::RemFilterIds(CanFilter bSelect,
                               UINT32 dwCode,
                               UINT32 dwMask)
{
  HRESULT           hResult;

  if (nullptr != m_pCanChn)
  {
    hResult = m_pCanChn->RemFilterIds((UINT8)bSelect, dwCode, dwMask);
    if (hResult != VCI_OK)
    {
      throw gcnew VciException(VciServerImpl::Instance(), hResult);
    }
  }
  else
  {
    throw gcnew ObjectDisposedException(this->GetType()->FullName);
  }
}

#pragma warning(default:4669) // 'type cast' : unsafe conversion
