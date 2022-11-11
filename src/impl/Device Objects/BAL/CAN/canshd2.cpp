// SPDX-License-Identifier: MIT
//----------------------------------------------------------------------------
// Summary  : Implementation of the CAN message scheduler class.
// Copyright: Copyright (C) 2016-2022 HMS Technology Center Ravensburg GmbH, 
//            all rights reserved
//----------------------------------------------------------------------------

#include "canshd2.hpp"
#include "vcinet.hpp"


using namespace Ixxat::Vci4::Bal::Can;

/*##########################################################################*/
/*### Methods for CanCyclicTXMsg2 class                                  ###*/
/*##########################################################################*/


//*****************************************************************************
/// <summary>
///   Constructor for cyclic CAN transmit message objects.
/// </summary>
//*****************************************************************************
CanCyclicTXMsg2::CanCyclicTXMsg2(CanScheduler2^ pSched)
{
  m_pCanShd = pSched;
  Cleanup();
}

//*****************************************************************************
/// <summary>
///   Destructor for cyclic CAN transmit message objects.
/// </summary>
//*****************************************************************************
CanCyclicTXMsg2::~CanCyclicTXMsg2()
{
  Reset();
}

//*****************************************************************************
/// <summary>
///   This method cleans up the cyclic CAN transmit message.
/// </summary>
//*****************************************************************************
void CanCyclicTXMsg2::Cleanup(void)
{
  static CANCYCLICTXMSG2 sEmpty = {0};
  pin_ptr<mgdCANCYCLICTXMSG2> pCanMsg = &m_CanMsg;

  *(PCANCYCLICTXMSG2)pCanMsg  = sEmpty;
  m_wHandle = 0xFFFF;
  m_eStatus = CanCyclicTXStatus::Empty;
  m_isDirty = true;
}

//*****************************************************************************
/// <summary>
///   This method sets the status of the CAN cyclic message object. 
/// </summary>
/// <param name="status">
///   New status value of the cyclic transmit message.
/// </param>
//*****************************************************************************
void CanCyclicTXMsg2::SetStat(CanCyclicTXStatus status)
{
  m_eStatus = status;
}

//*****************************************************************************
/// <summary>
///   This method starts processing of this cyclic transmit message.
/// </summary>
/// <param name="repeatCount">
///   Number of repetitions the message should be sent.
///   If this parameter is set to 0, the message is sent endlessly.
/// </param>
/// <returns>
///   true on success, otherwise false.
/// </returns>
/// <exception cref="VciException">
///   Starting the cyclic transmit message failed.
/// </exception>
/// <exception cref="ObjectDisposedException">
///   Object is already disposed or not yet added to a scheduler.
/// </exception>
//*****************************************************************************
void CanCyclicTXMsg2::Start(UInt16 repeatCount)
{
  if (nullptr != m_pCanShd)
  {
    if (m_isDirty)
    {
      m_pCanShd->InternalRemMessage(this);
      m_pCanShd->InternalAddMessage(this);
      m_isDirty = false;
    }

    m_pCanShd->InternalStartMessage(this, repeatCount);
  }
  else
  {
    throw gcnew ObjectDisposedException(this->GetType()->FullName);
  }
}

//*****************************************************************************
/// <summary>
///   This method stops processing of this cyclic transmit message.
/// </summary>
/// <exception cref="ObjectDisposedException">
///   Object is already disposed or not yet added to a scheduler.
/// </exception>
//*****************************************************************************
void CanCyclicTXMsg2::Stop(void)
{
  if (nullptr != m_pCanShd)
  {
    m_pCanShd->InternalStopMessage(this);
  }
  else
  {
    throw gcnew ObjectDisposedException(this->GetType()->FullName);
  }
}

//*****************************************************************************
/// <summary>
///   This method removes the cyclic transmit message from the scheduler
///   and resets the contents of this object.
/// </summary>
//*****************************************************************************
void CanCyclicTXMsg2::Reset(void)
{
  if (nullptr != m_pCanShd)
  {
    m_pCanShd->InternalRemMessage(this);
  }

  Cleanup();
}

//*****************************************************************************
/// <summary>
///   Gets the current status of this cyclic CAN message.
/// </summary>
/// <returns>
///   The current status of this cyclic CAN transmit message.
/// </returns>
//*****************************************************************************
CanCyclicTXStatus CanCyclicTXMsg2::Status::get()
{
  // try to update the message statii
  if (nullptr != m_pCanShd)
  {
    m_pCanShd->UpdateStatus();
  }
  return( m_eStatus );
}

//*****************************************************************************
/// <summary>
///   Gets the cycle time of this cyclic CAN transmit message in number of 
///   clock ticks.
/// </summary>
/// <returns>
///   The cycle time of this cyclic CAN transmit message in number of 
///   clock ticks.
/// </returns>
//*****************************************************************************
UInt16 CanCyclicTXMsg2::CycleTicks::get()
{
  return( m_CanMsg.wCycleTime );
}

//*****************************************************************************
/// <summary>
///   Sets the cycle time of this cyclic CAN transmit message in number of 
///   clock ticks.
/// </summary>
/// <param name="ticks">
///   Cycle time in number of ticks. This parameter must be in the range 1 
///   to <c>MaxCyclicMessageTicks</c>.
/// </param>
/// <remarks>
///   The contents of a cyclic CAN transmit message can be only changed
///   as long as the message is not registered at the scheduler. A call
///   of this method is silently ignored if the message is currently
///   registered at the scheduler.
/// </remarks>
//*****************************************************************************
void CanCyclicTXMsg2::CycleTicks::set(UInt16 ticks)
{
  m_isDirty = (m_CanMsg.wCycleTime != ticks);
  m_CanMsg.wCycleTime = ticks;
}

//*****************************************************************************
/// <summary>
///   Gets the auto-increment mode of this cyclic CAN transmit message.
/// </summary>
/// <returns>
///   The auto-increment mode of this cyclic CAN transmit message.
/// </returns>
//*****************************************************************************
CanCyclicTXIncMode CanCyclicTXMsg2::AutoIncrementMode::get()
{
  return( (CanCyclicTXIncMode) m_CanMsg.bIncrMode );
}

//*****************************************************************************
/// <summary>
///   Sets the auto-increment mode of this cyclic CAN transmit message.
/// </summary>
/// <param name="mode">
///   Auto-increment mode. This parameter can be one of the following 
///   constants:
///     <c>CanCyclicTXIncMode.NoInc</c> - no auto-increment
///     <c>CanCyclicTXIncMode.IncId</c> - auto-increment the CAN message identifier
///     <c>CanCyclicTXIncMode.Inc8</c>  - auto-increment a 8-bit data field
///     <c>CanCyclicTXIncMode.Inc16</c> - auto-increment a 16-bit data field
/// </param>
/// <remarks>
///   If <paramref name="mode"/> is set to either 
///   <c>CanCyclicTXIncMode.Inc8</c> or <c>CanCyclicTXIncMode.Inc16</c>, 
///   the <c>AutoIncrementIndex</c> property specifies the index of the first 
///   byte within the CAN message to be auto-incremented.
///   The contents of a cyclic CAN transmit message can be only changed
///   as long as the message is not registered at the scheduler. A call
///   of this method is silently ignored if the message is currently
///   registered at the scheduler.
/// </remarks>
//*****************************************************************************
void CanCyclicTXMsg2::AutoIncrementMode::set(CanCyclicTXIncMode mode)
{
  m_isDirty = (m_CanMsg.bIncrMode != (UINT8) mode);
  m_CanMsg.bIncrMode = (UINT8) mode;
}

//*****************************************************************************
/// <summary>
///   Gets the index of the auto-incremented data field of this cyclic CAN 
///   transmit message.
/// </summary>
/// <returns>
///   The index of the auto-incremented data field of this cyclic CAN transmit 
///   message.
/// </returns>
/// <remarks>
///   If <c>AutoIncrementMode</c> is set to <c>CanCyclicTXIncMode.Inc8</c> 
///   the result of this method specifies the byte within the data field wich 
///   is auto-incremented after each transmission of the CAN message. If 
///   <c>AutoIncrementMode</c> is set to <c>CanCyclicTXIncMode.Inc16</c> the 
///   result specifies the least significant byte within the data field which 
///   is auto-incremented after each transmission of the CAN message. The most 
///   significant byte of the auto-incremented data field is at 
///   <c>Data</c>[result+1]. It's not possible to read the actual value of the 
///   auto-incremented data field. <c>Data</c>[result] will always return the 
///   initial value of the data field when the message was added to the 
///   scheduler.
/// </remarks>
//*****************************************************************************
Byte CanCyclicTXMsg2::AutoIncrementIndex::get()
{
  return( m_CanMsg.bByteIndex );
}

//*****************************************************************************
/// <summary>
///   Sets the index of the auto-incremented data field of this cyclic CAN
///   transmit message.
/// </summary>
/// <param name="index">
///   Index of the data field to be auto-incremented (see also Remarks section 
///   below).
/// </param>
/// <remarks>
///   If <c>AutoIncrementMode</c> is set to <c>CanCyclicTXIncMode.Inc8</c> the 
///   parameter <paramref name="index"/> specifies the byte within the data 
///   field wich is auto-incremented after each transmission of the CAN message. 
///   If <c>AutoIncrementMode</c> is set to <c>CanCyclicTXIncMode.Inc16</c>
///   the parameter <paramref name="index"/> specifies the least significant 
///   byte within the data field which is auto-incremented after each 
///   transmission of the CAN message. The most significant byte of the 
///   auto-incremented data field is at <c>Data</c>[Index+1].
///   The contents of a cyclic CAN transmit message can be only changed
///   as long as the message is not registered at the scheduler. A call
///   of this method is silently ignored if the message is currently
///   registered at the scheduler.
/// </remarks>
/// <exception cref="ArgumentOutOfRangeException">
///   The specified data index is out of range.
/// </exception>
//*****************************************************************************
void CanCyclicTXMsg2::AutoIncrementIndex::set(Byte index)
{
  if (index < 8)
  {
    m_isDirty = (m_CanMsg.bByteIndex != index);
    m_CanMsg.bByteIndex = index;
  }
  else
  {
    throw gcnew ArgumentOutOfRangeException("index");
  }
}

/*##########################################################################*/
/*### Methods for CanScheduler2 class                                    ###*/
/*##########################################################################*/


//*****************************************************************************
/// <summary>
///   Constructor for VCI CAN scheduler objects.
/// </summary>
/// <param name="pBalObj">
///   Pointer to the native BAL object interface. 
///   This parameter must not be NULL.
/// </param>
/// <param name="portNumber">
///   Port number of the bus socket to open.
/// </param>
/// <param name="busTypeIndex">
///   Bus type related port number
///</param>
/// <exception cref="VciException">
///   Creation of CAN socket failed.
/// </exception>
/// <exception cref="ArgumentNullException">
///   Native IBalObject was a null pointer.
/// </exception>
/// <exception cref="OutOfMemoryException">
///   Memory allocation failed.
/// </exception>
//*****************************************************************************
CanScheduler2::CanScheduler2(::IBalObject*  pBalObj
                          , Byte          portNumber
                          , Byte          busTypeIndex)
            : CanSocket2(pBalObj, portNumber, busTypeIndex)
{
  HRESULT           hResult;
  ::ICanScheduler2*  pCanShd;

  m_pCanShd = nullptr;
  m_aCtxMsg = gcnew array<CanCyclicTXMsg2^>(CAN_MAX_CTX_MSGS);

  if (nullptr != pBalObj)
  {
    hResult = pBalObj->OpenSocket( portNumber
                                 , IID_ICanScheduler2
                                 , (PVOID*) &pCanShd);

    if (hResult == VCI_OK)
    {
      // This check has to be done because socket ICanScheduler2 can be
      // opened although it's not supported. Even the ICanScheduler2 methods
      // return an error (VCI_E_NOTIMPLEMENTED).
      if (!SupportsCyclicMessageScheduler)
      {
        pCanShd->Release();
        throw gcnew NotImplementedException();
      }

      hResult = InitNew(pCanShd);
      pCanShd->Release();
    }
    else
    {
      throw gcnew VciException(VciServerImpl::Instance(), hResult);
    }
  }
  else
  {
    throw gcnew ArgumentNullException();
  }
}

//*****************************************************************************
/// <summary>
///   Destructor for VCI CAN scheduler interface objects.
/// </summary>
//*****************************************************************************
CanScheduler2::~CanScheduler2()
{
  Cleanup();
}

//*****************************************************************************
/// <summary>
///   This method initialize a newly CAN scheduler object.
/// </summary>
/// <param name="pCanShd">
///   Pointer to the native CAN scheduler object.
///   This parameter must not be NULL.
/// </param>
/// <returns>
///   VCI_OK is succeeded, otherwise a VCI error code.
/// </returns>
//*****************************************************************************
HRESULT CanScheduler2::InitNew(::ICanScheduler2* pCanShd)
{
  HRESULT hResult;

  Cleanup();

  if (nullptr != pCanShd)
  {
    m_pCanShd = pCanShd;
    m_pCanShd->AddRef();
    hResult = VCI_OK;
  }
  else
  {
    hResult = VCI_E_INVALIDARG;
  }

  return( hResult );
}

//*****************************************************************************
/// <summary>
///   This method performs tasks associated with freeing, releasing, or
///   resetting unmanaged resources.
/// </summary>
//*****************************************************************************
void CanScheduler2::Cleanup(void)
{
  ResetScheduler();

  if (nullptr != m_pCanShd)
  {
    m_pCanShd->Release();
    m_pCanShd = nullptr;
  }
}

//*****************************************************************************
/// <summary>
///   This method resumes execution of the scheduler and starts processing
///   of all currently registered message.
/// </summary>
/// <exception cref="VciException">
///   Resuming scheduler failed.
/// </exception>
/// <exception cref="ObjectDisposedException">
///   Object is already disposed.
/// </exception>
//*****************************************************************************
void CanScheduler2::Resume(void)
{
  HRESULT hResult;

  if (nullptr != m_pCanShd)
  {
    hResult = m_pCanShd->Resume();
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
///   This method suspends execution of the scheduler and stops processing
///   of all currently registered messages.
/// </summary>
/// <exception cref="VciException">
///   Suspending scheduler failed.
/// </exception>
/// <exception cref="ObjectDisposedException">
///   Object is already disposed.
/// </exception>
//*****************************************************************************
void CanScheduler2::Suspend(void)
{
  HRESULT hResult;

  if (nullptr != m_pCanShd)
  {
    hResult = m_pCanShd->Suspend();
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
///   This method suspends execution of the scheduler and removes all
///   currently registered messages.
/// </summary>
/// <exception cref="VciException">
///   Resetting scheduler failed.
/// </exception>
//*****************************************************************************
void CanScheduler2::ResetScheduler( void )
{
  HRESULT hResult;

  if (nullptr != m_pCanShd)
  {
    hResult = m_pCanShd->Reset();

    for (int i = 0; i < m_aCtxMsg->Length; i++)
    {
      if (nullptr != m_aCtxMsg[i])
      {
        m_aCtxMsg[i]->Cleanup();
        m_aCtxMsg[i] = nullptr;
      }
    }

    if (hResult != VCI_OK)
    {
      throw gcnew VciException(VciServerImpl::Instance(), hResult);
    }
  }
}

//*****************************************************************************
/// <summary>
///   This method suspends execution of the scheduler and removes all
///   currently registered messages.
/// </summary>
/// <exception cref="VciException">
///   Resetting scheduler failed.
/// </exception>
/// <exception cref="ObjectDisposedException">
///   Object is already disposed.
/// </exception>
//*****************************************************************************
void CanScheduler2::Reset(void)
{
  if (nullptr == m_pCanShd)
  {
    throw gcnew ObjectDisposedException(this->GetType()->FullName);
  }

  ResetScheduler();
}

//*****************************************************************************
/// <summary>
///   This method updates the status of the scheduler and all currently
///   registered messages.
/// </summary>
//*****************************************************************************
void CanScheduler2::UpdateStatus(void)
{
  CANSCHEDULERSTATUS2 sStatus;

  if (nullptr != m_pCanShd)
  {
    if (m_pCanShd->GetStatus(&sStatus) == VCI_OK)
    {
      for (Byte i = 0; i < m_aCtxMsg->Length; i++)
      {
        if (nullptr != m_aCtxMsg[i])
        {
          m_aCtxMsg[i]->SetStat((CanCyclicTXStatus) sStatus.abMsgStat[i]);
        }
      }
    }
  }
}

ICanCyclicTXMsg2^ CanScheduler2::AddMessage( void )
{
  return gcnew CanCyclicTXMsg2(this);
}

//*****************************************************************************
/// <summary>
///   This method adds a new cyclic transmit message to the scheduler.
/// </summary>
/// <param name="cyclicTXMessage">
///   Reference to the initialized cyclic transmit message to add.
/// </param>
/// <exception cref="VciException">
///   Adding the cyclic transmit message to the scheduler failed.
/// </exception>
/// <exception cref="ObjectDisposedException">
///   Object is already disposed.
/// </exception>
/// <exception cref="IndexOutOfRangeException">
///   The transmit object cannot be registered at the scheduler because
///   the maximum number of supported transmit object has already been reached.
/// </exception>
/// <exception cref="ArgumentException">
///   The specified transmit object is a null reference.
/// </exception>
/// <exception cref="InvalidOperationException">
///   The specified transmit object is already registered at a scheduler.
/// </exception>
/// <remarks>
///   The method only adds messages with <c>FrameType</c> set to 
///   <c>CanMsgFrameType.Data</c>.
/// </remarks>
//*****************************************************************************
void CanScheduler2::InternalAddMessage(CanCyclicTXMsg2^ cyclicTXMessage)
{
  HRESULT hResult;

  if (nullptr != m_pCanShd)
  {
    if (nullptr != cyclicTXMessage)
    {
      if (nullptr != cyclicTXMessage->m_pCanShd)
      {
        Int32 dwHandle = 0xFFFFFFFF;

        //
        // check message id
        //
        if (cyclicTXMessage->ExtendedFrameFormat)
        {
          hResult = cyclicTXMessage->Identifier < 0x20000000 ?
                    VCI_OK : VCI_E_INVALIDARG;
        }
        else
        {
          hResult = cyclicTXMessage->Identifier < 0x800 ?
                    VCI_OK : VCI_E_INVALIDARG;
        }

        //
        // add the message to the scheduler
        //
        if (hResult == VCI_OK)
        {
          pin_ptr<mgdCANCYCLICTXMSG2> pMngtMsg = &cyclicTXMessage->m_CanMsg;
          hResult = m_pCanShd->AddMessage((PCANCYCLICTXMSG2)pMngtMsg, (PUINT32) &dwHandle);
        }

        //
        // add the message to the object list
        //
        if (hResult == VCI_OK)
        {
          if ((dwHandle < m_aCtxMsg->Length) && (nullptr == m_aCtxMsg[dwHandle]))
          {
            cyclicTXMessage->m_pCanShd = this;
            cyclicTXMessage->m_wHandle = (UInt16) dwHandle;
            m_aCtxMsg[dwHandle] = cyclicTXMessage;
          }
          else
          {
            m_pCanShd->RemMessage(dwHandle);
            throw gcnew IndexOutOfRangeException();
          }
        }
        else
        {
          throw gcnew VciException(VciServerImpl::Instance(), hResult);
        }
      }
      else
      {
        throw gcnew InvalidOperationException();
      }
    }
    else
    {
      throw gcnew ArgumentException();
    }
  }
  else
  {
    throw gcnew ObjectDisposedException(this->GetType()->FullName);
  }
}

//*****************************************************************************
/// <summary>
///   This method removes the specified cyclic transmit message from
///   the scheduler.
/// </summary>
/// <param name="cyclicTXMessage">
///   The cyclic transmit message to remove.
/// </param>
//*****************************************************************************
void CanScheduler2::InternalRemMessage(CanCyclicTXMsg2^ cyclicTXMessage)
{
  if (nullptr == m_pCanShd)
  {
    throw gcnew ObjectDisposedException(this->GetType()->FullName);
  }

  if ((nullptr != cyclicTXMessage) &&
      (cyclicTXMessage->m_pCanShd == this) &&
      (cyclicTXMessage->m_wHandle != 0xFFFF))
  {
    m_pCanShd->RemMessage(cyclicTXMessage->m_wHandle);
    m_aCtxMsg[cyclicTXMessage->m_wHandle] = nullptr;
  }
}

//*****************************************************************************
/// <summary>
///   This method starts processing of the specified cyclic transmit message.
/// </summary>
/// <param name="cyclicTXMessage">
///   Reference to the cyclic transmit message to start.
/// </param>
/// <param name="repeatCount">
///   Number of repetitions the message should be sent. 
///   If this parameter is set to 0, the message is sent
///   endlessly.
/// </param>
/// <exception cref="VciException">
///   Starting the cyclic transmit message failed.
/// </exception>
/// <exception cref="ObjectDisposedException">
///   Object is already disposed.
/// </exception>
/// <exception cref="ArgumentException">
///   The specified trasmit object is a null reference or not registered
///   at this scheduler.
/// </exception>
//*****************************************************************************
void CanScheduler2::InternalStartMessage( CanCyclicTXMsg2^ cyclicTXMessage
                               , UInt16          repeatCount)
{
  if (nullptr == m_pCanShd)
  {
    throw gcnew ObjectDisposedException(this->GetType()->FullName);
  }

  if ((nullptr != cyclicTXMessage) &&
      (cyclicTXMessage->m_pCanShd == this) &&
      (cyclicTXMessage->m_wHandle != 0xFFFF))
  {
    HRESULT hResult;

    hResult = m_pCanShd->StartMessage(cyclicTXMessage->m_wHandle, repeatCount);

    if (hResult == VCI_OK)
    {
      cyclicTXMessage->m_eStatus = CanCyclicTXStatus::Busy;
    }
    else
    {
      throw gcnew VciException(VciServerImpl::Instance(), hResult);
    }
  }
  else
  {
    throw gcnew ArgumentException();
  }
}

//*****************************************************************************
/// <summary>
///   This method stops processing of this cyclic transmit message.
/// </summary>
/// <param name="cyclicTXMessage">
///   Reference to the cyclic transmit message to stop.
/// </param>
/// <exception cref="VciException">
///   Stopping the cyclic transmit message failed.
/// </exception>
/// <exception cref="ObjectDisposedException">
///   Object is already disposed.
/// </exception>
//*****************************************************************************
void CanScheduler2::InternalStopMessage(CanCyclicTXMsg2^ cyclicTXMessage)
{
  if (nullptr == m_pCanShd)
  {
    throw gcnew ObjectDisposedException(this->GetType()->FullName);
  }

  if ((nullptr != cyclicTXMessage) &&
      (cyclicTXMessage->m_pCanShd == this) &&
      (cyclicTXMessage->m_wHandle != 0xFFFF))
  {
    HRESULT hResult;

    hResult = m_pCanShd->StopMessage(cyclicTXMessage->m_wHandle);

    if (hResult == VCI_OK)
    {
      cyclicTXMessage->m_eStatus = CanCyclicTXStatus::Done;
    }
    else
    {
      throw gcnew VciException(VciServerImpl::Instance(), hResult);
    }
  }
}
