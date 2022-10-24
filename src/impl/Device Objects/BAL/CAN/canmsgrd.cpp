/*****************************************************************************
 IXXAT Automation GmbH
******************************************************************************
 Summary : Implementation of the CAN message reader class.
 Compiler: Microsoft VC++ 8
******************************************************************************
 all rights reserved
*****************************************************************************/

/*************************************************************************
** include files
*************************************************************************/
#include "canmsgrd.hpp"
#include "vcinet.hpp"

/*************************************************************************
** used namespaces
*************************************************************************/
using namespace Ixxat::Vci4::Bal::Can;

#pragma warning(disable:4669) // 'type cast' : unsafe conversion


//*****************************************************************************
/// <summary>
///   Constructor for CAN message reader objects
/// </summary>
/// <param name="pRxFifo">
///   Pointer to the native receive FIFO object interface.
///   This parameter must not be NULL.
/// </param>
/// <exception cref="ArgumentNullException">
///   Native IFifoReader was a null pointer.
/// </exception>
//*****************************************************************************
CanMessageReader::CanMessageReader(::ICanChannel*   pCanChan)
{
  m_isCanChannel2 = false;
  PFIFOREADER   pRxFifo;
  HRESULT hResult = pCanChan->GetReader(&pRxFifo);
  if (VCI_OK == hResult)
  {
    m_pRxFifo = pRxFifo;
  }
  else
  {
    throw gcnew VciException(VciServerImpl::Instance(), hResult);
  }
}

//*****************************************************************************
/// <summary>
///   Constructor for CAN message reader objects
/// </summary>
/// <param name="pRxFifo">
///   Pointer to the native receive FIFO object interface.
///   This parameter must not be NULL.
/// </param>
/// <exception cref="ArgumentNullException">
///   Native IFifoReader was a null pointer.
/// </exception>
//*****************************************************************************
CanMessageReader::CanMessageReader(::ICanChannel2*   pCanChan)
{
  m_isCanChannel2 = true;
  ::IFifoReader*    pRxFifo;
  HRESULT hResult = pCanChan->GetReader(&pRxFifo);
  if (VCI_OK == hResult)
  {
    m_pRxFifo = pRxFifo;
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
CanMessageReader::~CanMessageReader()
{
  Cleanup();
}

//*****************************************************************************
/// <summary>
///   This method performs tasks associated with freeing, releasing, or
///   resetting unmanaged resources.
/// </summary>
//*****************************************************************************
void CanMessageReader::Cleanup(void)
{
  if (nullptr != m_pRxFifo)
  {
    m_pRxFifo->Release();
    m_pRxFifo = nullptr;
  }
}

//*****************************************************************************
/// <summary>
///   Gets the capacity of the receive FIFO in number of CAN messages.
/// </summary>
/// <returns>
///   The capacity of the receive FIFO.
/// </returns>
//*****************************************************************************
UInt16 CanMessageReader::Capacity::get()
{
  UInt16 wCapacity;

  if (nullptr != m_pRxFifo)
  {
    m_pRxFifo->GetCapacity(&wCapacity);
  }
  else
  {
    wCapacity = 0;
  }

  return( wCapacity );
}

//*****************************************************************************
/// <summary>
///   Gets the number of currently unread CAN messages within the receive FIFO.
/// </summary>
/// <returns>
///   The number of currently unread CAN messages within the receive FIFO.
/// </returns>
//*****************************************************************************
UInt16 CanMessageReader::FillCount::get()
{
  UInt16 wCount;

  if (nullptr != m_pRxFifo)
  {
    m_pRxFifo->GetFillCount(&wCount);
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
UInt16 CanMessageReader::Threshold::get()
{
  UInt16 wThreshold;

  if (nullptr != m_pRxFifo)
  {
    m_pRxFifo->GetThreshold(&wThreshold);
  }
  else
  {
    wThreshold = 0;
  }

  return( wThreshold );
}

//*****************************************************************************
/// <summary>
///   Sets the threshold for the trigger event. If the receive
///   FIFO contains at least the specified number of CAN messages, the event
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
void CanMessageReader::Threshold::set(UInt16 threshold)
{
  HRESULT hResult;

  if (nullptr != m_pRxFifo)
  {
    hResult = m_pRxFifo->SetThreshold(threshold);
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
void CanMessageReader::Lock()
{
  if (nullptr != m_pRxFifo)
  {
    HRESULT hResult = m_pRxFifo->Lock();
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
void CanMessageReader::Unlock()
{
  if (nullptr != m_pRxFifo)
  {
    HRESULT hResult = m_pRxFifo->Unlock();
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
///   This method assigns an event object to the message reader. The event
///   is set to the signaled state if the number of available messages within
///   the receive FIFO exceed the currently set receive threshold.
/// </summary>
/// <param name="fifoEvent">
///   The event object which is to be set if the number of available 
///   messages within the receive FIFO exceed the currently set receive 
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
void CanMessageReader::AssignEvent(AutoResetEvent^ fifoEvent)
{
  HRESULT hResult;

  if (nullptr == fifoEvent)
  {
    throw gcnew ArgumentNullException();
  }
  
  if (nullptr != m_pRxFifo)
  {
    hResult = m_pRxFifo->AssignEvent((HANDLE) fifoEvent->Handle);
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
///   This method assigns an event object to the message reader. The event
///   is set to the signaled state if the number of available messages within
///   the receive FIFO exceed the currently set receive threshold.
/// </summary>
/// <param name="fifoEvent">
///   The event object which is to be set if the number of available 
///   messages within the receive FIFO exceed the currently set receive 
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
void CanMessageReader::AssignEvent(ManualResetEvent^ fifoEvent)
{
  HRESULT hResult;

  if (nullptr == fifoEvent)
  {
    throw gcnew ArgumentNullException();
  }

  if (nullptr != m_pRxFifo)
  {
    hResult = m_pRxFifo->AssignEvent((HANDLE) fifoEvent->Handle);
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
///   This method reads a single CAN message from the front of the
///   receive FIFO and remove the message from the FIFO.
/// </summary>
/// <param name="message">
///   Reference to a CanMessage where the method stores the read the message.
/// </param>
/// <returns>
///   true on success. false if no message is available to read.
/// </returns>
/// <exception cref="ObjectDisposedException">
///   Object is already disposed.
/// </exception>
//*****************************************************************************
bool CanMessageReader::ReadMessage(ICanMessage^% message)
{
  HRESULT       hResult;
  bool          fResult = false;
  
  if (nullptr == m_pRxFifo)
  {
    throw gcnew ObjectDisposedException(this->GetType()->FullName);
  }

  if (m_isCanChannel2)
  {
    CanMessage2 msg;

    pin_ptr<mgdCANMSG2> pCanMsg = &msg.m_CanMsg;
    hResult = m_pRxFifo->GetDataEntry((PCANMSG2)pCanMsg);
    if (hResult == VCI_OK)
    {
      message = msg;
    }
  }
  else
  {
    CanMessage msg;

    pin_ptr<mgdCANMSG> pCanMsg = &msg.m_CanMsg;
    hResult = m_pRxFifo->GetDataEntry((PCANMSG)pCanMsg);
    if (hResult == VCI_OK)
    {
      message = msg;
    }
  }

  fResult = (hResult == VCI_OK);
  return( fResult );
}

//*****************************************************************************
/// <summary>
///   This method reads a single CAN message from the front of the
///   receive FIFO and remove the message from the FIFO.
/// </summary>
/// <param name="message">
///   Reference to a CanMessage where the method stores the read the message.
/// </param>
/// <returns>
///   true on success. false if no message is available to read.
/// </returns>
/// <exception cref="ObjectDisposedException">
///   Object is already disposed.
/// </exception>
//*****************************************************************************
bool CanMessageReader::ReadMessage(ICanMessage2^% message)
{
  HRESULT       hResult;
  bool          fResult = false;
  
  if (nullptr == m_pRxFifo)
  {
    throw gcnew ObjectDisposedException(this->GetType()->FullName);
  }

  if (m_isCanChannel2)
  {
    CanMessage2 msg;

    pin_ptr<mgdCANMSG2> pCanMsg = &msg.m_CanMsg;
    hResult = m_pRxFifo->GetDataEntry((PCANMSG2)pCanMsg);
    if (hResult == VCI_OK)
    {
      message = msg;
    }
  }
  else
  {
    CanMessage msg;

    pin_ptr<mgdCANMSG> pCanMsg = &msg.m_CanMsg;
    hResult = m_pRxFifo->GetDataEntry((PCANMSG)pCanMsg);
    if (hResult == VCI_OK)
    {
      message = msg;
    }
  }

  fResult = (hResult == VCI_OK);
  return( fResult );
}

//*****************************************************************************
/// <summary>
///   This method reads multiple CAN messages from the front of the
///   receive FIFO. The method removes the messages from the FIFO.
/// </summary>
/// <param name="messages">
///   One-dimensional array of CAN messages where the method stores the 
///   received messages. The size of this array specifies the maximum
///   number of message that can be read with the method call.
/// </param>
/// <returns>
///   The number of read messages if succeeded.
///   0 if no message is available to read.
/// </returns>
/// <exception cref="ObjectDisposedException">
///   Object is already disposed.
/// </exception>
//*****************************************************************************
int CanMessageReader::ReadMessages(array<ICanMessage^>^% messages)
{
  UInt16  wCount = 0;
  PCANMSG pCanMsg;

  if (nullptr == m_pRxFifo)
  {
    throw gcnew ObjectDisposedException(this->GetType()->FullName);
  }

  if (m_pRxFifo->AcquireRead((PVOID*) &pCanMsg, &wCount) == VCI_OK)
  {

    messages = gcnew array< ICanMessage^ >(wCount);

    for (UInt16 index = 0; index < wCount; index++)
    {
      if (m_isCanChannel2)
      {
        CanMessage2^ msg = gcnew CanMessage2();
        msg->SetValue(*(mgdCANMSG2*)pCanMsg);
        messages[index] = msg;
      }
      else
      {
        CanMessage^ msg = gcnew CanMessage();
        msg->SetValue(*(mgdCANMSG*)pCanMsg);
        messages[index] = msg;
      }

      pCanMsg++;
    }

    m_pRxFifo->ReleaseRead(wCount);
  }

  return( wCount );
}

//*****************************************************************************
/// <summary>
///   This method reads multiple CAN messages from the front of the
///   receive FIFO. The method removes the messages from the FIFO.
/// </summary>
/// <param name="messages">
///   One-dimensional array of CAN messages where the method stores the 
///   received messages. The size of this array specifies the maximum
///   number of message that can be read with the method call.
/// </param>
/// <returns>
///   The number of read messages if succeeded.
///   0 if no message is available to read.
/// </returns>
/// <exception cref="ObjectDisposedException">
///   Object is already disposed.
/// </exception>
//*****************************************************************************
int CanMessageReader::ReadMessages(array<ICanMessage2^>^% messages)
{
  UInt16  wCount = 0;
  PCANMSG2 pCanMsg;

  if (nullptr == m_pRxFifo)
  {
    throw gcnew ObjectDisposedException(this->GetType()->FullName);
  }

  if (m_pRxFifo->AcquireRead((PVOID*) &pCanMsg, &wCount) == VCI_OK)
  {

    messages = gcnew array< ICanMessage2^ >(wCount);
    
    for (UInt16 index = 0; index < wCount; index++)
    {
      if (m_isCanChannel2)
      {
        CanMessage2^ msg = gcnew CanMessage2();
        msg->SetValue(*(mgdCANMSG2*)pCanMsg);
        messages[index] = msg;
      }
      else
      {
        CanMessage^ msg = gcnew CanMessage();
        msg->SetValue(*(mgdCANMSG*)pCanMsg);
        messages[index] = msg;
      }

      pCanMsg++;
    }

    m_pRxFifo->ReleaseRead(wCount);
  }

  return( wCount );
}

#pragma warning(default:4669) // 'type cast' : unsafe conversion
