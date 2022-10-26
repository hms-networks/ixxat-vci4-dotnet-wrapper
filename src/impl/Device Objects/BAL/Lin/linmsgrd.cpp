/*****************************************************************************
 IXXAT Automation GmbH
******************************************************************************
 Summary : Implementation of the LIN message reader class.
 Compiler: Microsoft VC++ 8
******************************************************************************
 all rights reserved
*****************************************************************************/

#include "linmsgrd.hpp"
#include "vcinet.hpp"

using namespace Ixxat::Vci4::Bal::Lin;

#pragma warning(disable:4669) // 'type cast' : unsafe conversion


//*****************************************************************************
/// <summary>
///   Constructor for LIN message reader objects
/// </summary>
/// <param name="pRxFifo">
///   Pointer to the native receive FIFO object interface.
///   This parameter must not be NULL.
/// </param>
/// <exception cref="ArgumentNullException">
///   Native IFifoReader was a null pointer.
/// </exception>
//*****************************************************************************
LinMessageReader::LinMessageReader(::ILinMonitor* pLinMon)
{
  PFIFOREADER   pRxFifo;
  HRESULT hResult = pLinMon->GetReader(&pRxFifo);
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
///   Destructor for message reader objects.
/// </summary>
//*****************************************************************************
LinMessageReader::~LinMessageReader()
{
  Cleanup();
}

//*****************************************************************************
/// <summary>
///   This method performs tasks associated with freeing, releasing, or
///   resetting unmanaged resources.
/// </summary>
//*****************************************************************************
void LinMessageReader::Cleanup(void)
{
  if (nullptr != m_pRxFifo)
  {
    m_pRxFifo->Release();
    m_pRxFifo = nullptr;
  }
}

//*****************************************************************************
/// <summary>
///   Gets the capacity of the receive FIFO in number of messages.
/// </summary>
/// <returns>
///   The capacity of the receive FIFO.
/// </returns>
//*****************************************************************************
UInt16 LinMessageReader::Capacity::get()
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
///   Gets the number of currently unread messages within the receive FIFO.
/// </summary>
/// <returns>
///   The number of currently unread messages within the receive FIFO.
/// </returns>
//*****************************************************************************
UInt16 LinMessageReader::FillCount::get()
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
///   The number of currently unread messages within the receive FIFO.
/// </returns>
//*****************************************************************************
UInt16 LinMessageReader::Threshold::get()
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
///   FIFO contains at least the specified number of messages, the event
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
void LinMessageReader::Threshold::set(UInt16 threshold)
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
void LinMessageReader::AssignEvent(AutoResetEvent^ fifoEvent)
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
void LinMessageReader::AssignEvent(ManualResetEvent^ fifoEvent)
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
///   This method reads a single message from the front of the
///   receive FIFO and remove the message from the FIFO.
/// </summary>
/// <param name="message">
///   Reference to a LinMessage where the method stores the read the message.
/// </param>
/// <returns>
///   true on success. false if no message is available to read.
/// </returns>
/// <exception cref="ObjectDisposedException">
///   Object is already disposed.
/// </exception>
//*****************************************************************************
bool LinMessageReader::ReadMessage([Out] ILinMessage^% message)
{
  HRESULT       hResult;
  bool          fResult = false;
  
  if (nullptr == m_pRxFifo)
  {
    throw gcnew ObjectDisposedException(this->GetType()->FullName);
  }

  LinMessage msg;

  pin_ptr<mgdLINMSG> pMsg = &msg.m_LinMsg;
  hResult = m_pRxFifo->GetDataEntry((PLINMSG)pMsg);
  if (hResult == VCI_OK)
  {
    message = msg;
  }

  fResult = (hResult == VCI_OK);
  return( fResult );
}

//*****************************************************************************
/// <summary>
///   This method reads multiple messages from the front of the
///   receive FIFO. The method removes the messages from the FIFO.
/// </summary>
/// <param name="messages">
///   One-dimensional array of messages where the method stores the 
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
int LinMessageReader::ReadMessages(array<ILinMessage^>^% messages)
{
  UInt16  wCount = 0;
  PLINMSG pMsg;

  if (nullptr == m_pRxFifo)
  {
    throw gcnew ObjectDisposedException(this->GetType()->FullName);
  }

  if (m_pRxFifo->AcquireRead((PVOID*) &pMsg, &wCount) == VCI_OK)
  {

    messages = gcnew array< ILinMessage^ >(wCount);

    for (UInt16 index = 0; index < wCount; index++)
    {
      LinMessage^ msg = gcnew LinMessage();

      msg->SetValue(*(mgdLINMSG*)pMsg);

      messages[index] = msg;

      pMsg++;
    }

    m_pRxFifo->ReleaseRead(wCount);
  }

  return( wCount );
}

#pragma warning(default:4669) // 'type cast' : unsafe conversion
