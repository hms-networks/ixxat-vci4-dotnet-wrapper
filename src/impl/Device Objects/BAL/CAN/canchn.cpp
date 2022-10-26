/*****************************************************************************
 IXXAT Automation GmbH
******************************************************************************
 Summary : Implementation of the CAN channel class.
 Compiler: Microsoft VC++ 8
******************************************************************************
 all rights reserved
*****************************************************************************/

#include "canchn.hpp"
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
CanChannel::CanChannel( ::IBalObject* pBalObj
                      , Byte          bPortNo
                      , Byte          busTypeIndex)
          : CanSocket(pBalObj, bPortNo, busTypeIndex)
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
CanChannel::~CanChannel()
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
bool CanChannel::InitNew(bool fExclusive)
{
  HRESULT         hResult;
  ::ICanSocket*   pSocket;
  ::ICanChannel*  pCanChn;

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
void CanChannel::Cleanup(void)
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
void CanChannel::Initialize( UInt16 receiveFifoSize
                           , UInt16 transmitFifoSize
                           , bool   exclusive)
{
  HRESULT hResult;
  bool    fResult;

  fResult = InitNew(exclusive);
  if (true == fResult)
  {
    hResult = m_pCanChn->Initialize(receiveFifoSize, transmitFifoSize);
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
void CanChannel::Activate(void)
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
void CanChannel::Deactivate(void)
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
CanChannelStatus CanChannel::ChannelStatus::get()
{
  HRESULT          hResult;

  if (nullptr != m_pCanChn)
  {
    CANCHANSTATUS sStatus;
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
ICanMessageReader^ CanChannel::GetMessageReader()
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
ICanMessageWriter^ CanChannel::GetMessageWriter()
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

#pragma warning(default:4669) // 'type cast' : unsafe conversion
