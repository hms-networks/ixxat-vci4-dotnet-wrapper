// SPDX-License-Identifier: MIT
//----------------------------------------------------------------------------
// Summary  : Implementation of the CAN message writer class.
// Copyright: Copyright (C) 2016-2022 HMS Technology Center Ravensburg GmbH, 
//            all rights reserved
//----------------------------------------------------------------------------

#include "canmsgwr.hpp"
#include "vcinet.hpp"

using namespace Ixxat::Vci4::Bal::Can;

#pragma warning(disable:4669) // 'type cast' : unsafe conversion


//*****************************************************************************
/// <summary>
///   Constructor for CAN message writer objects
/// </summary>
/// <param name="pTxFifo">
///   Pointer to the native transmit FIFO object interface.
///   This parameter must not be NULL.
/// </param>
/// <exception cref="ArgumentNullException">
///   Native IFifoWriter was a null pointer.
/// </exception>
//*****************************************************************************
CanMessageWriter::CanMessageWriter(::ICanChannel*   pCanChan)
{
  m_isCanChannel2 = false;
  PFIFOWRITER   pTxFifo;
  HRESULT hResult = pCanChan->GetWriter(&pTxFifo);
  if (VCI_OK == hResult)
  {
    m_pTxFifo = pTxFifo;
  }
  else
  {
    throw gcnew VciException(VciServerImpl::Instance(), hResult);
  }
}

//*****************************************************************************
/// <summary>
///   Constructor for CAN message writer objects
/// </summary>
/// <param name="pTxFifo">
///   Pointer to the native transmit FIFO object interface.
///   This parameter must not be NULL.
/// </param>
/// <exception cref="ArgumentNullException">
///   Native IFifoWriter was a null pointer.
/// </exception>
//*****************************************************************************
CanMessageWriter::CanMessageWriter(::ICanChannel2*   pCanChan)
{
  m_isCanChannel2 = true;
  PFIFOWRITER   pTxFifo;
  HRESULT hResult = pCanChan->GetWriter(&pTxFifo);
  if (VCI_OK == hResult)
  {
    m_pTxFifo = pTxFifo;
  }
  else
  {
    throw gcnew VciException(VciServerImpl::Instance(), hResult);
  }
}

//*****************************************************************************
/// <summary>
///   Destructor for CAN message reader objects.
/// </summary>
//*****************************************************************************
CanMessageWriter::~CanMessageWriter()
{
  Cleanup();
}

//*****************************************************************************
/// <summary>
///   This method performs tasks associated with freeing, releasing, or
///   resetting unmanaged resources.
/// </summary>
//*****************************************************************************
void CanMessageWriter::Cleanup(void)
{
  if (nullptr != m_pTxFifo)
  {
    m_pTxFifo->Release();
    m_pTxFifo = NULL;
  }
}

//*****************************************************************************
/// <summary>
///   Gets the capacity of the transmit FIFO in number of CAN messages.
/// </summary>
/// <returns>
///   The capacity of the transmit FIFO in number of CAN messages.
/// </returns>
//*****************************************************************************
UInt16 CanMessageWriter::Capacity::get()
{
  UInt16 wCapacity;

  if (nullptr != m_pTxFifo)
  {
    m_pTxFifo->GetCapacity(&wCapacity);
  }
  else
  {
    wCapacity = 0;
  }

  return( wCapacity );
}

//*****************************************************************************
/// <summary>
///   Gets the number of currently free CAN messages within the transmit FIFO.
/// </summary>
/// <returns>
///   The number of currently free CAN messages within the transmit FIFO.
/// </returns>
//*****************************************************************************
UInt16 CanMessageWriter::FreeCount::get()
{
  UInt16 wCount;

  if (nullptr != m_pTxFifo)
  {
    m_pTxFifo->GetFreeCount(&wCount);
  }
  else
  {
    wCount = 0;
  }

  return( wCount );
}

//*****************************************************************************
/// <summary>
///   Gets the current threshold for the trigger event.
/// </summary>
/// <returns>
///   The number of currently unread CAN messages within the receive FIFO.
/// </returns>
//*****************************************************************************
UInt16 CanMessageWriter::Threshold::get()
{
  UInt16 wThreshold;

  if (nullptr != m_pTxFifo)
  {
    m_pTxFifo->GetThreshold(&wThreshold);
  }
  else
  {
    wThreshold = 0;
  }

  return( wThreshold );
}

//*****************************************************************************
/// <summary>
///   Sets the threshold for the trigger event. If the transmit
///   FIFO contains at least the specified number of free entries, the event
///   specified by a AssignEvent method call is set to the signaled state.
/// </summary>
/// <param name="threshold">
///   Threshold for the event trigger.
/// </param>
/// <exception cref="VciException">
///   Setting Threshold failed.
/// </exception>
/// <exception cref="ObjectDisposedException">
///   Object is already disposed.
/// </exception>
//*****************************************************************************
void CanMessageWriter::Threshold::set(UInt16 threshold)
{
  HRESULT hResult;

  if (nullptr != m_pTxFifo)
  {
    hResult = m_pTxFifo->SetThreshold(threshold);
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
///   This method locks the access to the FIFO. 
///   Use the Lock()/Unlock() pair if you access the FIFO 
///   from different threads.
/// </summary>
/// <exception cref="VciException">
///   Lock failed.
/// </exception>
/// <exception cref="ObjectDisposedException">
///   Object is already disposed.
/// </exception>
//*****************************************************************************
void CanMessageWriter::Lock()
{
  if (nullptr != m_pTxFifo)
  {
    HRESULT hResult = m_pTxFifo->Lock();
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
///   This method releases the access to the FIFO. 
///   Use the Lock()/Unlock() pair if you access the FIFO 
///   from different threads.
/// </summary>
/// <exception cref="VciException">
///   Unlock failed.
/// </exception>
/// <exception cref="ObjectDisposedException">
///   Object is already disposed.
/// </exception>//*****************************************************************************
void CanMessageWriter::Unlock()
{
  if (nullptr != m_pTxFifo)
  {
    HRESULT hResult = m_pTxFifo->Unlock();
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
///   This method assigns an event object to the message writer. The event
///   is set to the signaled state if the number free entries within the 
///   transmit FIFO reaches or exceed the currently set threshold.
/// </summary>
/// <param name="fifoEvent">
///   The event object which is set to the signaled state if the number free 
///   entries within the transmit FIFO reaches or exceed the currently set 
///   threshold.
/// </param>
/// <exception cref="VciException">
///   Assigning the event failed.
/// </exception>
/// <exception cref="ArgumentNullException">
///   Parameter fifoEvent was a null reference.
/// </exception>
/// <exception cref="ObjectDisposedException">
///   Object is already disposed.
/// </exception>
//*****************************************************************************
void CanMessageWriter::AssignEvent(AutoResetEvent^ fifoEvent)
{
  HRESULT hResult;

  if (nullptr == fifoEvent)
  {
    throw gcnew ArgumentNullException();
  }

  if (nullptr != m_pTxFifo)
  {
    hResult = m_pTxFifo->AssignEvent((HANDLE) fifoEvent->Handle);
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
///   This method assigns an event object to the message writer. The event
///   is set to the signaled state if the number free entries within the 
///   transmit FIFO reaches or exceed the currently set threshold.
/// </summary>
/// <param name="fifoEvent">
///   The event object which is set to the signaled state if the number free 
///   entries within the transmit FIFO reaches or exceed the currently set 
///   threshold.
/// </param>
/// <exception cref="VciException">
///   Assigning the event failed.
/// </exception>
/// <exception cref="ArgumentNullException">
///   Parameter fifoEvent was a null reference.
/// </exception>
/// <exception cref="ObjectDisposedException">
///   Object is already disposed.
/// </exception>
//*****************************************************************************
void CanMessageWriter::AssignEvent(ManualResetEvent^ fifoEvent)
{
  HRESULT hResult;
  
  if (nullptr == fifoEvent)
  {
    throw gcnew ArgumentNullException();
  }

  if (nullptr != m_pTxFifo)
  {
    hResult = m_pTxFifo->AssignEvent((HANDLE) fifoEvent->Handle);
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

mgdCANMSG ConvertToCANMSG(Object^ message)
{
  bool converted = false;

  mgdCANMSG local;
  CanMessage^ castmsg = dynamic_cast<CanMessage^> (message);
  if (castmsg)
  {
    local = castmsg->ToCANMSG();
    converted = true;
  }
  else
  {
    CanMessage2^ castmsg = dynamic_cast<CanMessage2^> (message);
    if (castmsg)
    {
      if (castmsg->DataLength > CAN_SDLC_MAX)
      {
        // may result in a shortened message -> prevent conversion
        throw gcnew ArgumentException("Parameter must be a standard CAN message (dlc < 8)", "message");
      }
      else
      {
        local = castmsg->ToCANMSG();
        converted = true;
      }
    }
  }

  if (!converted)
  {
    throw gcnew ArgumentException("Parameter must be a CAN message", "message");
  }

  return local;
}

mgdCANMSG2 ConvertToCANMSG2(Object^ message)
{
  bool converted = false;

  mgdCANMSG2 local;
  CanMessage^ castmsg = dynamic_cast<CanMessage^> (message);
  if (castmsg)
  {
    local = castmsg->ToCANMSG2();
    converted = true;
  }
  else
  {
    CanMessage2^ castmsg = dynamic_cast<CanMessage2^> (message);
    if (castmsg)
    {
      local = castmsg->ToCANMSG2();
      converted = true;
    }
  }

  if (!converted)
  {
    throw gcnew ArgumentException("Parameter must be a CAN message", "message");
  }

  return local;
}

//*****************************************************************************
/// <summary>
///   This method places a single CAN message at the end of the
///   transmit FIFO and returns without waiting for the message to
///   be transmitted.
/// </summary>
/// <param name="message">
///   Reference to the CanMessage to send.
/// </param>
/// <returns>
///   If the method succeeds it returns true. The method returns false
///   if there is not enought free space available within the transmit FIFO
///   to add the message.
/// </returns>
/// <exception cref="ObjectDisposedException">
///   Object is already disposed.
/// </exception>
//*****************************************************************************
bool CanMessageWriter::SendMessage(ICanMessage^ message)
{
  HRESULT       hResult;
  bool          fResult = false;
  
  if (nullptr == m_pTxFifo)
  {
    throw gcnew ObjectDisposedException(this->GetType()->FullName);
  }

  if (m_isCanChannel2)
  {
    mgdCANMSG2 msg = ConvertToCANMSG2(message);

    pin_ptr<mgdCANMSG2> pCanMsg = &msg;
    hResult = m_pTxFifo->PutDataEntry((PCANMSG2)pCanMsg);
    fResult = (hResult == VCI_OK);
  }
  else
  {
    mgdCANMSG msg = ConvertToCANMSG(message);

    pin_ptr<mgdCANMSG> pCanMsg = &msg;
    hResult = m_pTxFifo->PutDataEntry((PCANMSG)pCanMsg);
    fResult = (hResult == VCI_OK);
  }

  return( fResult );
}

//*****************************************************************************
/// <summary>
///   This method places multiple CAN messages at the end of the
///   transmit FIFO and returns without waiting for the messages to
///   be transmitted.
/// </summary>
/// <param name="messages">
///   One-dimensional array of CAN messages to send.
/// </param>
/// <returns>
///   The number of entered messages.
/// </returns>
/// <exception cref="ObjectDisposedException">
///   Object is already disposed.
/// </exception>
//*****************************************************************************
/*
int CanMessageWriter::SendMessages(array<CanMessage>^ messages)
{
  int     iResult;
  int     iLength;
  int     iLowIdx;
  UInt16  wCount;
  UInt16  wDone;
  PCANMSG pCanMsg;

  if (nullptr == m_pTxFifo)
  {
    throw gcnew ObjectDisposedException(this->GetType()->FullName);
  }

  iResult = 0;

  if (nullptr != messages)
  {
    iLength = messages->GetLength(0);
    iLowIdx = messages->GetLowerBound(0);
  }
  else
  {
    iLength = 0;
    iLowIdx = 0;
  }

  while (iResult < iLength)
  {
    if (m_pTxFifo->AcquireWrite((PVOID*) &pCanMsg, &wCount) == VCI_OK)
    {
      for (wDone = 0; (wDone < wCount) && (iResult < iLength); wDone++)
      {
        pin_ptr<mgdCANMSG> pSrcMsg = &messages[iLowIdx+iResult].m_CanMsg;
        *pCanMsg = *(PCANMSG)pSrcMsg;
        iResult++;
        pCanMsg++;
      }

      m_pTxFifo->ReleaseWrite(wDone);
    }
    else
    {
      break;
    }
  }

  return( iResult );
}
*/

//*****************************************************************************
/// <summary>
///   This method places a single CAN message at the end of the
///   transmit FIFO and returns without waiting for the message to
///   be transmitted.
/// </summary>
/// <param name="message">
///   Reference to the CanMessage to send.
/// </param>
/// <returns>
///   If the method succeeds it returns true. The method returns false
///   if there is not enought free space available within the transmit FIFO
///   to add the message.
/// </returns>
/// <exception cref="ObjectDisposedException">
///   Object is already disposed.
/// </exception>
//*****************************************************************************t
bool CanMessageWriter::SendMessage(ICanMessage2^ message)
{
  HRESULT       hResult;
  bool          fResult = false;
  
  if (nullptr == m_pTxFifo)
  {
    throw gcnew ObjectDisposedException(this->GetType()->FullName);
  }

  if (m_isCanChannel2)
  {
    mgdCANMSG2 msg = ConvertToCANMSG2(message);

    pin_ptr<mgdCANMSG2> pCanMsg = &msg;
    hResult = m_pTxFifo->PutDataEntry((PCANMSG2)pCanMsg);
    fResult = (hResult == VCI_OK);
  }
  else
  {
    mgdCANMSG msg = ConvertToCANMSG(message);

    pin_ptr<mgdCANMSG> pCanMsg = &msg;
    hResult = m_pTxFifo->PutDataEntry((PCANMSG)pCanMsg);
    fResult = (hResult == VCI_OK);
  }

  return( fResult );
}

//*****************************************************************************
/// <summary>
///   This method places multiple CAN messages at the end of the
///   transmit FIFO and returns without waiting for the messages to
///   be transmitted.
/// </summary>
/// <param name="messages">
///   One-dimensional array of CAN messages to send.
/// </param>
/// <returns>
///   The number of entered messages.
/// </returns>
/// <exception cref="ObjectDisposedException">
///   Object is already disposed.
/// </exception>
//*****************************************************************************
/*
int CanMessageWriter::SendMessages(array<CanMessage2>^ messages)
{
  int     iResult;
  int     iLength;
  int     iLowIdx;
  UInt16  wCount;
  UInt16  wDone;
  PCANMSG2 pCanMsg;

  if (nullptr == m_pTxFifo)
  {
    throw gcnew ObjectDisposedException(this->GetType()->FullName);
  }

  iResult = 0;

  if (nullptr != messages)
  {
    iLength = messages->GetLength(0);
    iLowIdx = messages->GetLowerBound(0);
  }
  else
  {
    iLength = 0;
    iLowIdx = 0;
  }

  while (iResult < iLength)
  {
    if (m_pTxFifo->AcquireWrite((PVOID*) &pCanMsg, &wCount) == VCI_OK)
    {
      for (wDone = 0; (wDone < wCount) && (iResult < iLength); wDone++)
      {
        pin_ptr<mgdCANMSG2> pSrcMsg = &messages[iLowIdx+iResult].m_CanMsg;
        *pCanMsg = *(PCANMSG2)pSrcMsg;
        iResult++;
        pCanMsg++;
      }

      m_pTxFifo->ReleaseWrite(wDone);
    }
    else
    {
      break;
    }
  }

  return( iResult );
}
*/


#pragma warning(default:4669) // 'type cast' : unsafe conversion
