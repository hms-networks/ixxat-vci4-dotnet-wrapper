/*****************************************************************************
 IXXAT Automation GmbH
******************************************************************************
 Summary : Implementation of the CAN socket class.
 Compiler: Microsoft VC++ 8
******************************************************************************
 all rights reserved
*****************************************************************************/

#include "cansoc.hpp"
#include "vcinet.hpp"

using namespace Ixxat::Vci4::Bal::Can;

#pragma warning(disable:4669) // 'type cast' : unsafe conversion

//*****************************************************************************
/// <summary>
///   Constructor for CAN socket objects.
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
CanSocket::CanSocket( ::IBalObject* pBalObj
                    , Byte          portNumber
                    , Byte          busTypeIndex)
         : BalResource(portNumber, VciBusType::Can, busTypeIndex)
{
  HRESULT       hResult;
  ::ICanSocket* pSocket;

  m_pSocket   = nullptr;
  m_psCanCap  = nullptr;

  if (nullptr != pBalObj)
  {
    hResult = pBalObj->OpenSocket( portNumber
                                 , IID_ICanSocket
                                 , (PVOID*) &pSocket);
    if (hResult == VCI_OK)
    {
      try
      {
        hResult = InitNew(pSocket);
      }
      finally
      {
        pSocket->Release();
      }
    }

    if (hResult != VCI_OK)
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
///   Destructor for CAN socket objects.
/// </summary>
//*****************************************************************************
CanSocket::~CanSocket()
{
  Cleanup();
}

//*****************************************************************************
/// <summary>
///   This method initializes a newly created socket object.
/// </summary>
/// <param name="pSocket">
///   Pointer to the native CAN socket object.
///   This parameter must not be NULL.
/// </param>
/// <returns>
///   VCI_OK if succeeded, otherwise a VCI error code.
/// </returns>
/// <exception cref="OutOfMemoryException">
///   Memory allocation failed.
/// </exception>
//*****************************************************************************
HRESULT CanSocket::InitNew(::ICanSocket* pSocket)
{
  HRESULT hResult;

  Cleanup();

  if (nullptr != pSocket)
  {
    // We have to dynamically allocate the native struct because
    // native structs are no longer valid as member of managed classes !!
    m_psCanCap = new CANCAPABILITIES;
    if (nullptr == m_psCanCap)
    {
      throw gcnew InsufficientMemoryException();
    }

    hResult = pSocket->GetCapabilities(m_psCanCap);
    if (hResult == VCI_OK)
    {
      m_pSocket = pSocket;
      m_pSocket->AddRef();
    }
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
void CanSocket::Cleanup(void)
{
  if (nullptr != m_pSocket)
  {
    m_pSocket->Release();
    m_pSocket = nullptr;
  }

  if (nullptr != m_psCanCap)
  {
    delete m_psCanCap;
    m_psCanCap = nullptr;
  }
}

//*****************************************************************************
/// <summary>
///   This method gets a pointer to the native CAN socket object.
/// </summary>
/// <returns>
///   If the method succeeds it returns a pointer to the native CAN socket
///   object, otherwise the method returns NULL.
/// </returns>
/// <remarks>
///   The caller must Release the pointer returned by this method if it is
///   no longer needed.
/// </remarks>
//*****************************************************************************
::ICanSocket* CanSocket::GetNativeSocket()
{
  ::ICanSocket* pSocket = m_pSocket;

  if (nullptr != pSocket)
  {
    pSocket->AddRef();
  }

  return( pSocket );
}

//*****************************************************************************
/// <summary>
///   Gets the type of controller used by the CAN socket.
/// </summary>
/// <returns>
///   The type of controller used by the CAN socket (see <c>CanCtrlType</c>).
/// </returns>
/// <exception cref="ObjectDisposedException">
///   Object is already disposed.
/// </exception>
//*****************************************************************************
CanCtrlType CanSocket::ControllerType::get()
{
  if (nullptr == m_psCanCap)
  {
    throw gcnew ObjectDisposedException(this->GetType()->FullName);
  }

  return( (CanCtrlType) m_psCanCap->wCtrlType );
}

//*****************************************************************************
/// <summary>
///   Gets the type of bus coupling used by the CAN controller.
/// </summary>
/// <returns>
///   Type of the bus coupling used by the CAN controller 
///   (see <c>CanBusCouplings</c>).
/// </returns>
/// <exception cref="ObjectDisposedException">
///   Object is already disposed.
/// </exception>
//*****************************************************************************
CanBusCouplings CanSocket::BusCoupling::get()
{
  if (nullptr == m_psCanCap)
  {
    throw gcnew ObjectDisposedException(this->GetType()->FullName);
  }

  return( (CanBusCouplings) m_psCanCap->wBusCoupling );
}

//*****************************************************************************
/// <summary>
///   Gets a flag field indicating the features supported by the CAN 
///   controller.
/// </summary>
/// <returns>
///   A flag field indicating the features supported by the CAN 
///   controller.
/// </returns>
/// <exception cref="ObjectDisposedException">
///   Object is already disposed.
/// </exception>
//*****************************************************************************
CanFeatures CanSocket::Features::get()
{
  if (nullptr == m_psCanCap)
  {
    throw gcnew ObjectDisposedException(this->GetType()->FullName);
  }

  return( (CanFeatures) m_psCanCap->dwFeatures );
}

//*****************************************************************************
/// <summary>
///   Gets a value indicating if the CAN socket supports only
///   standard (11-bit) or extended (29-bit) message frames exclusively and
///   not both frame formats at the same time.
/// </summary>
/// <returns>
///   true if the socket supports only standard (11-bit) or extended (29-bit) 
///   message frames exclusively and not both frame formats at the same time. 
///   Otherwise false.
/// </returns>
/// <exception cref="ObjectDisposedException">
///   Object is already disposed.
/// </exception>
//*****************************************************************************
bool CanSocket::SupportsStdOrExtFrames::get()
{
  if (nullptr == m_psCanCap)
  {
    throw gcnew ObjectDisposedException(this->GetType()->FullName);
  }

  return( (m_psCanCap->dwFeatures & CAN_FEATURE_STDOREXT) != 0);
}

//*****************************************************************************
/// <summary>
///   Gets a value indicating if the CAN socket supports standard (11-bit) 
///   and extended (29-bit) message frames simultanously.
/// </summary>
/// <returns>
///   true if the socket supports standard (11-bit) and extended (29-bit) 
///   message frames simultanously. Otherwise false.
/// </returns>
/// <exception cref="ObjectDisposedException">
///   Object is already disposed.
/// </exception>
//*****************************************************************************
bool CanSocket::SupportsStdAndExtFrames::get()
{
  if (nullptr == m_psCanCap)
  {
    throw gcnew ObjectDisposedException(this->GetType()->FullName);
  }

  return( (m_psCanCap->dwFeatures & CAN_FEATURE_STDANDEXT) != 0);
}

//*****************************************************************************
/// <summary>
///   Gets a value indicating if the CAN socket supports remote transfer 
///   requests.
/// </summary>
/// <returns>
///   true if the socket supports remote transfer requests. 
///   Otherwise false.
/// </returns>
/// <exception cref="ObjectDisposedException">
///   Object is already disposed.
/// </exception>
//*****************************************************************************
bool CanSocket::SupportsRemoteFrames::get()
{
  if (nullptr == m_psCanCap)
  {
    throw gcnew ObjectDisposedException(this->GetType()->FullName);
  }

  return( (m_psCanCap->dwFeatures & CAN_FEATURE_RMTFRAME) != 0);
}

//*****************************************************************************
/// <summary>
///   Gets a value indicating if the CAN socket supports error frames.
/// </summary>
/// <returns>
///   true if the socket supports error frames. Otherwise false.
/// </returns>
/// <exception cref="ObjectDisposedException">
///   Object is already disposed.
/// </exception>
//*****************************************************************************
bool CanSocket::SupportsErrorFrames::get()
{
  if (nullptr == m_psCanCap)
  {
    throw gcnew ObjectDisposedException(this->GetType()->FullName);
  }

  return( (m_psCanCap->dwFeatures & CAN_FEATURE_ERRFRAME) != 0);
}

//*****************************************************************************
/// <summary>
///   Gets a value indicating if the CAN socket supports bus load computation.
/// </summary>
/// <returns>
///   true if the socket supports bus load computation. Otherwise false.
/// </returns>
/// <exception cref="ObjectDisposedException">
///   Object is already disposed.
/// </exception>
//*****************************************************************************
bool CanSocket::SupportsBusLoadComputation::get()
{
  if (nullptr == m_psCanCap)
  {
    throw gcnew ObjectDisposedException(this->GetType()->FullName);
  }

  return( (m_psCanCap->dwFeatures & CAN_FEATURE_BUSLOAD) != 0);
}

//*****************************************************************************
/// <summary>
///   Gets a value indicating if the CAN socket supports exact filtering of 
///   CAN messages.
/// </summary>
/// <returns>
///   true if the socket supports exact filtering of CAN messages. 
///   Otherwise false.
/// </returns>
/// <exception cref="ObjectDisposedException">
///   Object is already disposed.
/// </exception>
//*****************************************************************************
bool CanSocket::SupportsExactMessageFilter::get()
{
  if (nullptr == m_psCanCap)
  {
    throw gcnew ObjectDisposedException(this->GetType()->FullName);
  }

  return( (m_psCanCap->dwFeatures & CAN_FEATURE_IDFILTER) != 0);
}

//*****************************************************************************
/// <summary>
///   Gets a value indicating if the CAN socket supports listen only mode.
/// </summary>
/// <returns>
///   true if the socket supports listen only mode. Otherwise false.
/// </returns>
/// <exception cref="ObjectDisposedException">
///   Object is already disposed.
/// </exception>
//*****************************************************************************
bool CanSocket::SupportsListenOnlyMode::get()
{
  if (nullptr == m_psCanCap)
  {
    throw gcnew ObjectDisposedException(this->GetType()->FullName);
  }

  return( (m_psCanCap->dwFeatures & CAN_FEATURE_LISTONLY) != 0);
}

//*****************************************************************************
/// <summary>
///   Gets a value indicating if the CAN socket supports a cyclic message 
///   scheduler.
/// </summary>
/// <returns>
///   true if the socket supports a cyclic message scheduler. Otherwise false.
/// </returns>
/// <exception cref="ObjectDisposedException">
///   Object is already disposed.
/// </exception>
//*****************************************************************************
bool CanSocket::SupportsCyclicMessageScheduler::get()
{
  if (nullptr == m_psCanCap)
  {
    throw gcnew ObjectDisposedException(this->GetType()->FullName);
  }

  return( (m_psCanCap->dwFeatures & CAN_FEATURE_SCHEDULER) != 0);
}

//*****************************************************************************
/// <summary>
///   Gets a value indicating if the CAN socket supports the generation of 
///   error message frames.
/// </summary>
/// <returns>
///   true if the socket supports the generation of error message frames. 
///   Otherwise false.
/// </returns>
/// <exception cref="ObjectDisposedException">
///   Object is already disposed.
/// </exception>
//*****************************************************************************
bool CanSocket::SupportsErrorFrameGeneration::get()
{
  if (nullptr == m_psCanCap)
  {
    throw gcnew ObjectDisposedException(this->GetType()->FullName);
  }

  return( (m_psCanCap->dwFeatures & CAN_FEATURE_GENERRFRM) != 0);
}

//*****************************************************************************
/// <summary>
///   Gets a value indicating if the CAN socket supports delayed transmission 
///   of CAN message frames.
/// </summary>
/// <returns>
///   True if the socket supports delayed transmission of CAN message frames. 
///   Otherwise false.
/// </returns>
/// <exception cref="ObjectDisposedException">
///   Object is already disposed.
/// </exception>
//*****************************************************************************
bool CanSocket::SupportsDelayedTransmission::get()
{
  if (nullptr == m_psCanCap)
  {
    throw gcnew ObjectDisposedException(this->GetType()->FullName);
  }

  return( (m_psCanCap->dwFeatures & CAN_FEATURE_DELAYEDTX) != 0);
}

//*****************************************************************************
/// <summary>
///   Gets the frequency to the primary timer in Hz.
/// </summary>
/// <returns>
///   The frequency to the primary timer in Hz.
/// </returns>
/// <exception cref="ObjectDisposedException">
///   Object is already disposed.
/// </exception>
//*****************************************************************************
UInt32 CanSocket::ClockFrequency::get()
{
  if (nullptr == m_psCanCap)
  {
    throw gcnew ObjectDisposedException(this->GetType()->FullName);
  }

  return( m_psCanCap->dwClockFreq );
}

//*****************************************************************************
/// <summary>
///   Gets the divisor factor of the time stamp counter. 
///   The time stamp counter provides the time stamp for CAN messages. 
///   The frequency of the time stamp counter is calculated from the frequency 
///   of the primary timer (<c>ClockFrequency</c>) divided by the value 
///   specified here.
/// </summary>
/// <returns>
///   The divisor factor of the time stamp counter.
/// </returns>
/// <exception cref="ObjectDisposedException">
///   Object is already disposed.
/// </exception>
//*****************************************************************************
UInt32 CanSocket::TimeStampCounterDivisor::get()
{
  if (nullptr == m_psCanCap)
  {
    throw gcnew ObjectDisposedException(this->GetType()->FullName);
  }

  return( m_psCanCap->dwTscDivisor );
}

//*****************************************************************************
/// <summary>
///   Gets the divisor factor for the timer of the cyclic transmit list
///   (See <c>ICanScheduler</c>. The frequency of this timer is calculated 
///   from the frequency of the primary timer (<c>ClockFrequency</c>) divided 
///   by the value specified here. If no cyclic transmit list is available, 
///   property <c>CyclicMessageTimerDivisor</c> has the value 0.
/// </summary>
/// <returns>
///   The divisor factor for the timer of the cyclic transmit list
/// </returns>
/// <exception cref="ObjectDisposedException">
///   Object is already disposed.
/// </exception>
//*****************************************************************************
UInt32 CanSocket::CyclicMessageTimerDivisor::get()
{
  if (nullptr == m_psCanCap)
  {
    throw gcnew ObjectDisposedException(this->GetType()->FullName);
  }

  return( m_psCanCap->dwCmsDivisor );
}

//*****************************************************************************
/// <summary>
///   Gets the maximum cycle time of the CAN message scheduler in number of 
///   ticks.
/// </summary>
/// <returns>
///   Maximum cycle time of the CAN message scheduler in number of ticks.
/// </returns>
/// <exception cref="ObjectDisposedException">
///   Object is already disposed.
/// </exception>
//*****************************************************************************
UInt32 CanSocket::MaxCyclicMessageTicks::get()
{
  if (nullptr == m_psCanCap)
  {
    throw gcnew ObjectDisposedException(this->GetType()->FullName);
  }

  return( m_psCanCap->dwCmsMaxTicks );
}

//*****************************************************************************
/// <summary>
///   Gets the divisor factor for the timer used for delayed transmission of 
///   messages. The frequency of this timer is calculated from the frequency 
///   of the primary timer (<c>ClockFrequency</c>) divided by the value 
///   specified here. If delayed transmission is not supported by the 
///   adapter, property <c>DelayedTXTimerDivisor</c> has the value 0.
/// </summary>
/// <returns>
///   The divisor factor for the timer used for delayed transmission of 
///   messages
/// </returns>
/// <exception cref="ObjectDisposedException">
///   Object is already disposed.
/// </exception>
//*****************************************************************************
UInt32 CanSocket::DelayedTXTimerDivisor::get()
{
  if (nullptr == m_psCanCap)
  {
    throw gcnew ObjectDisposedException(this->GetType()->FullName);
  }

  return( m_psCanCap->dwDtxDivisor );
}

//*****************************************************************************
/// <summary>
///   Gets the maximum delay time of the delayed CAN message transmitter in 
///   number of ticks.
/// </summary>
/// <returns>
///   Maximum cycle time of the CAN message scheduler in number of ticks.
/// </returns>
/// <exception cref="ObjectDisposedException">
///   Object is already disposed.
/// </exception>
//*****************************************************************************
UInt32 CanSocket::MaxDelayedTXTicks::get()
{
  if (nullptr == m_psCanCap)
  {
    throw gcnew ObjectDisposedException(this->GetType()->FullName);
  }

  return( m_psCanCap->dwDtxMaxTicks );
}

//*****************************************************************************
/// <summary>
///   Gets the current status of the CAN line.
/// </summary>
/// <returns>
///   The current status of the CAN line.
/// </returns>
/// <exception cref="VciException">
///   Getting CAN line status failed.
/// </exception>
/// <exception cref="ObjectDisposedException">
///   Object is already disposed.
/// </exception>
//*****************************************************************************
CanLineStatus CanSocket::LineStatus::get()
{
  HRESULT       hResult;

  if (nullptr != m_pSocket)
  {
    CANLINESTATUS sStatus;
    hResult = m_pSocket->GetLineStatus(&sStatus);
    if (hResult != VCI_OK)
    {
      throw gcnew VciException(VciServerImpl::Instance(), hResult);
    }

    return CanLineStatus((CanOperatingModes)sStatus.bOpMode, sStatus.bBusLoad, (CanCtrlStatus)sStatus.dwStatus, CanBitrate(sStatus.bBtReg0, sStatus.bBtReg1));
  }
  else
  {
    throw gcnew ObjectDisposedException(this->GetType()->FullName);
  }
}

#pragma warning(default:4669) // 'type cast' : unsafe conversion

