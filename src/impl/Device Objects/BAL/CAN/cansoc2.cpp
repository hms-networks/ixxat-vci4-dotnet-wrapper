/*****************************************************************************
 IXXAT Automation GmbH
******************************************************************************
 Summary : Implementation of the CAN socket class.
 Compiler: Microsoft VC++ 8
******************************************************************************
 all rights reserved
*****************************************************************************/

/*************************************************************************
** include files
*************************************************************************/
#include "cansoc2.hpp"
#include "vcinet.hpp"


/*************************************************************************
** used namespaces
*************************************************************************/
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
CanSocket2::CanSocket2( ::IBalObject* pBalObj
                    , Byte          portNumber
                    , Byte          busTypeIndex)
         : BalResource(portNumber, VciBusType::Can, busTypeIndex)
{
  HRESULT       hResult;
  ::ICanSocket2* pSocket;

  m_pSocket   = nullptr;
  m_psCanCap  = nullptr;

  if (nullptr != pBalObj)
  {
    hResult = pBalObj->OpenSocket( portNumber
                                 , IID_ICanSocket2
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
CanSocket2::~CanSocket2()
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
HRESULT CanSocket2::InitNew(::ICanSocket2* pSocket)
{
  HRESULT hResult;

  Cleanup();

  if (nullptr != pSocket)
  {
    // We have to dynamically allocate the native struct because
    // native structs are no longer valid as member of managed classes !!
    m_psCanCap = new CANCAPABILITIES2;
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
void CanSocket2::Cleanup(void)
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
::ICanSocket2* CanSocket2::GetNativeSocket()
{
  ::ICanSocket2* pSocket = m_pSocket;

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
CanCtrlType CanSocket2::ControllerType::get()
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
CanBusCouplings CanSocket2::BusCoupling::get()
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
CanFeatures CanSocket2::Features::get()
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
bool CanSocket2::SupportsStdOrExtFrames::get()
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
bool CanSocket2::SupportsStdAndExtFrames::get()
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
bool CanSocket2::SupportsRemoteFrames::get()
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
bool CanSocket2::SupportsErrorFrames::get()
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
bool CanSocket2::SupportsBusLoadComputation::get()
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
bool CanSocket2::SupportsExactMessageFilter::get()
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
bool CanSocket2::SupportsListenOnlyMode::get()
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
bool CanSocket2::SupportsCyclicMessageScheduler::get()
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
bool CanSocket2::SupportsErrorFrameGeneration::get()
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
bool CanSocket2::SupportsDelayedTransmission::get()
{
  if (nullptr == m_psCanCap)
  {
    throw gcnew ObjectDisposedException(this->GetType()->FullName);
  }

  return( (m_psCanCap->dwFeatures & CAN_FEATURE_DELAYEDTX) != 0);
}

//*****************************************************************************
/// <summary>
///   Gets a value indicating if the CAN socket supports single shot messages
/// </summary>
/// <exception cref="ObjectDisposedException">
///   Object is already disposed.
/// </exception>
//*****************************************************************************
bool CanSocket2::SupportsSingleShotMessages::get()
{
  if (nullptr == m_psCanCap)
  {
    throw gcnew ObjectDisposedException(this->GetType()->FullName);
  }

  return( (m_psCanCap->dwFeatures & CAN_FEATURE_SINGLESHOT) != 0);
}

//*****************************************************************************
/// <summary>
///   Gets a value indicating if the CAN socket supports high priority messages
/// </summary>
/// <exception cref="ObjectDisposedException">
///   Object is already disposed.
/// </exception>
//*****************************************************************************
bool CanSocket2::SupportsHighPriorityMessages::get()
{
  if (nullptr == m_psCanCap)
  {
    throw gcnew ObjectDisposedException(this->GetType()->FullName);
  }

  return( (m_psCanCap->dwFeatures & CAN_FEATURE_HIGHPRIOR) != 0);
}

//*****************************************************************************
/// <summary>
///   Gets a value indicating if the CAN socket supports auto baudrate detection 
/// </summary>
/// <exception cref="ObjectDisposedException">
///   Object is already disposed.
/// </exception>
//*****************************************************************************
bool CanSocket2::SupportsAutoBaudrateDetection::get()
{
  if (nullptr == m_psCanCap)
  {
    throw gcnew ObjectDisposedException(this->GetType()->FullName);
  }

  return( (m_psCanCap->dwFeatures & CAN_FEATURE_AUTOBAUD) != 0);
}

//*****************************************************************************
/// <summary>
///   Gets a value indicating if the CAN socket supports extended data length 
///   messages.
/// </summary>
/// <exception cref="ObjectDisposedException">
///   Object is already disposed.
/// </exception>
//*****************************************************************************
bool CanSocket2::SupportsExtendedDataLength::get()
{
  if (nullptr == m_psCanCap)
  {
    throw gcnew ObjectDisposedException(this->GetType()->FullName);
  }

  return( (m_psCanCap->dwFeatures & CAN_FEATURE_EXTDATA) != 0);
}

//*****************************************************************************
/// <summary>
///   Gets a value indicating if the CAN socket supports fast data bit rate 
///   messages.
/// </summary>
/// <exception cref="ObjectDisposedException">
///   Object is already disposed.
/// </exception>
//*****************************************************************************
bool CanSocket2::SupportsFastDataRate::get()
{
  if (nullptr == m_psCanCap)
  {
    throw gcnew ObjectDisposedException(this->GetType()->FullName);
  }

  return( (m_psCanCap->dwFeatures & CAN_FEATURE_FASTDATA) != 0);
}

//*****************************************************************************
/// <summary>
///   Gets a value indicating if the CAN socket supports the ISO CAN-FD 
///   format.
/// </summary>
/// <exception cref="ObjectDisposedException">
///   Object is already disposed.
/// </exception>
//*****************************************************************************
bool CanSocket2::SupportsIsoCanFdFrames::get()
{
  if (nullptr == m_psCanCap)
  {
    throw gcnew ObjectDisposedException(this->GetType()->FullName);
  }

  return( (m_psCanCap->dwFeatures & CAN_FEATURE_ISOFRAME) != 0);
}

//*****************************************************************************
/// <summary>
///   Gets a value indicating if the CAN socket supports the non-ISO CAN-FD
///   format.
/// </summary>
/// <exception cref="ObjectDisposedException">
///   Object is already disposed.
/// </exception>
//*****************************************************************************
bool CanSocket2::SupportsNonIsoCanFdFrames::get()
{
  if (nullptr == m_psCanCap)
  {
    throw gcnew ObjectDisposedException(this->GetType()->FullName);
  }

  return( (m_psCanCap->dwFeatures & CAN_FEATURE_NONISOFRM) != 0);
}

//*****************************************************************************
/// <summary>
///   Gets a value indicating if the CAN socket supports 64-bit timestamps
/// </summary>
/// <exception cref="ObjectDisposedException">
///   Object is already disposed.
/// </exception>
//*****************************************************************************
bool CanSocket2::Supports64BitTimeStamps::get()
{
  if (nullptr == m_psCanCap)
  {
    throw gcnew ObjectDisposedException(this->GetType()->FullName);
  }

  return( (m_psCanCap->dwFeatures & CAN_FEATURE_64BITTSC) != 0);
}

//*****************************************************************************
/// <summary>
///   Gets the can clock frequency in Hz.
/// </summary>
/// <returns>
///   The frequency to the can clock timer in Hz.
/// </returns>
/// <exception cref="ObjectDisposedException">
///   Object is already disposed.
/// </exception>
//*****************************************************************************
UInt32 CanSocket2::CanClockFrequency::get()
{
  if (nullptr == m_psCanCap)
  {
    throw gcnew ObjectDisposedException(this->GetType()->FullName);
  }

  return( m_psCanCap->dwCanClkFreq );
}

//*****************************************************************************
/// <summary>
///   Gets the minimum bit timing values for arbitration bit rate.
/// </summary>
/// <returns>
///   The minimum arbitration bitrate.
/// </returns>
/// <exception cref="ObjectDisposedException">
///   Object is already disposed.
/// </exception>
//*****************************************************************************
CanBitrate2 CanSocket2::MinimumArbitrationBitrate::get() 
{ 
  if (nullptr == m_psCanCap)
  {
    throw gcnew ObjectDisposedException(this->GetType()->FullName);
  }

  CanBitrate2 bitrate((CanBitrateMode)m_psCanCap->sSdrRangeMin.dwMode, 
                                      m_psCanCap->sSdrRangeMin.dwBPS,
                                      m_psCanCap->sSdrRangeMin.wTS1,
                                      m_psCanCap->sSdrRangeMin.wTS2,
                                      m_psCanCap->sSdrRangeMin.wSJW,
                                      m_psCanCap->sSdrRangeMin.wTDO);

  return( bitrate );
}

//*****************************************************************************
/// <summary>
///   Gets the maximum bit timing values for the arbitration bit rate.
/// </summary>
/// <returns>
///   The maximum arbitration bit rate.
/// </returns>
/// <exception cref="ObjectDisposedException">
///   Object is already disposed.
/// </exception>
//*****************************************************************************
CanBitrate2 CanSocket2::MaximumArbitrationBitrate::get()
{ 
  if (nullptr == m_psCanCap)
  {
    throw gcnew ObjectDisposedException(this->GetType()->FullName);
  }

  CanBitrate2 bitrate((CanBitrateMode)m_psCanCap->sSdrRangeMax.dwMode, 
                                      m_psCanCap->sSdrRangeMax.dwBPS,
                                      m_psCanCap->sSdrRangeMax.wTS1,
                                      m_psCanCap->sSdrRangeMax.wTS2,
                                      m_psCanCap->sSdrRangeMax.wSJW,
                                      m_psCanCap->sSdrRangeMax.wTDO);

  return( bitrate );
}

//*****************************************************************************
/// <summary>
///   Gets the minimum bit timing values for fast data bit rate.
/// </summary>
/// <returns>
///   The minimum fast data bit rate.
/// </returns>
/// <exception cref="ObjectDisposedException">
///   Object is already disposed.
/// </exception>
//*****************************************************************************
CanBitrate2 CanSocket2::MinimumFastDataBitrate::get()
{ 
  if (nullptr == m_psCanCap)
  {
    throw gcnew ObjectDisposedException(this->GetType()->FullName);
  }

  CanBitrate2 bitrate((CanBitrateMode)m_psCanCap->sFdrRangeMin.dwMode, 
                                      m_psCanCap->sFdrRangeMin.dwBPS,
                                      m_psCanCap->sFdrRangeMin.wTS1,
                                      m_psCanCap->sFdrRangeMin.wTS2,
                                      m_psCanCap->sFdrRangeMin.wSJW,
                                      m_psCanCap->sFdrRangeMin.wTDO);

  return( bitrate );
}

//*****************************************************************************
/// <summary>
///   Gets the maximum bit timing values for fast data bit rate.
/// </summary>
/// <returns>
///   The maximum fast data bit rate.
/// </returns>
/// <exception cref="ObjectDisposedException">
///   Object is already disposed.
/// </exception>
//*****************************************************************************
CanBitrate2 CanSocket2::MaximumFastDataBitrate::get()
{ 
  if (nullptr == m_psCanCap)
  {
    throw gcnew ObjectDisposedException(this->GetType()->FullName);
  }

  CanBitrate2 bitrate((CanBitrateMode)m_psCanCap->sFdrRangeMax.dwMode, 
                                      m_psCanCap->sFdrRangeMax.dwBPS,
                                      m_psCanCap->sFdrRangeMax.wTS1,
                                      m_psCanCap->sFdrRangeMax.wTS2,
                                      m_psCanCap->sFdrRangeMax.wSJW,
                                      m_psCanCap->sFdrRangeMax.wTDO);

  return( bitrate );
}

//*****************************************************************************
/// <summary>
///   Gets the clock frequency of the time stamp counter [Hz]
/// </summary>
/// <returns>
///   The clock frequency of the time stamp counter [Hz]
/// </returns>
/// <exception cref="ObjectDisposedException">
///   Object is already disposed.
/// </exception>
//*****************************************************************************
UInt32 CanSocket2::TimeStampCounterClockFrequency::get()
{
  if (nullptr == m_psCanCap)
  {
    throw gcnew ObjectDisposedException(this->GetType()->FullName);
  }

  return( m_psCanCap->dwTscClkFreq );
}

//*****************************************************************************
/// <summary>
///   Gets the divisor factor of the time stamp counter. 
///   The time stamp counter provides the time stamp for CAN messages. 
///   The frequency of the time stamp counter is calculated from the frequency 
///   of the can clock timer (<c>CanClockFrequency</c>) divided by the value 
///   specified here.
/// </summary>
/// <returns>
///   The divisor factor of the time stamp counter.
/// </returns>
/// <exception cref="ObjectDisposedException">
///   Object is already disposed.
/// </exception>
//*****************************************************************************
UInt32 CanSocket2::TimeStampCounterDivisor::get()
{
  if (nullptr == m_psCanCap)
  {
    throw gcnew ObjectDisposedException(this->GetType()->FullName);
  }

  return( m_psCanCap->dwTscDivisor );
}

//*****************************************************************************
/// <summary>
///   Gets the clock frequency of cyclic message scheduler [Hz]
/// </summary>
/// <returns>
///   The clock frequency of cyclic message scheduler [Hz]
/// </returns>
/// <exception cref="ObjectDisposedException">
///   Object is already disposed.
/// </exception>
//*****************************************************************************
UInt32 CanSocket2::CyclicMessageTimerClockFrequency::get()
{
  if (nullptr == m_psCanCap)
  {
    throw gcnew ObjectDisposedException(this->GetType()->FullName);
  }

  return( m_psCanCap->dwCmsClkFreq );
}

//*****************************************************************************
/// <summary>
///   Gets the divisor factor for the timer of the cyclic transmit list
///   (See <c>ICanScheduler2</c>. The frequency of this timer is calculated 
///   from the frequency of the can clock timer (<c>CanClockFrequency</c>) divided 
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
UInt32 CanSocket2::CyclicMessageTimerDivisor::get()
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
UInt32 CanSocket2::MaxCyclicMessageTicks::get()
{
  if (nullptr == m_psCanCap)
  {
    throw gcnew ObjectDisposedException(this->GetType()->FullName);
  }

  return( m_psCanCap->dwCmsMaxTicks );
}

//*****************************************************************************
/// <summary>
///   Gets the clock frequency of the delayed message transmitter [Hz]
/// </summary>
/// <returns>
///   The clock frequency of the delayed message transmitter [Hz]
/// </returns>
/// <exception cref="ObjectDisposedException">
///   Object is already disposed.
/// </exception>
//*****************************************************************************
UInt32 CanSocket2::DelayedTXTimerClockFrequency::get()
{
  if (nullptr == m_psCanCap)
  {
    throw gcnew ObjectDisposedException(this->GetType()->FullName);
  }

  return( m_psCanCap->dwDtxClkFreq );
}

//*****************************************************************************
/// <summary>
///   Gets the divisor factor for the timer used for delayed transmission of 
///   messages. The frequency of this timer is calculated from the frequency 
///   of the can clock timer (<c>CanClockFrequency</c>) divided by the value 
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
UInt32 CanSocket2::DelayedTXTimerDivisor::get()
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
UInt32 CanSocket2::MaxDelayedTXTicks::get()
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
CanLineStatus2 CanSocket2::LineStatus::get()
{
  HRESULT       hResult;

  if (nullptr != m_pSocket)
  {
    CANLINESTATUS2 sStatus;
    hResult = m_pSocket->GetLineStatus(&sStatus);
    if (hResult != VCI_OK)
    {
      throw gcnew VciException(VciServerImpl::Instance(), hResult);
    }

    return CanLineStatus2((CanOperatingModes)sStatus.bOpMode, (CanExtendedOperatingModes)sStatus.bExMode, sStatus.bBusLoad, (CanCtrlStatus)sStatus.dwStatus, 
      CanBitrate2((CanBitrateMode)sStatus.sBtpSdr.dwMode, 
                  sStatus.sBtpSdr.dwBPS,
                  sStatus.sBtpSdr.wTS1,
                  sStatus.sBtpSdr.wTS2,
                  sStatus.sBtpSdr.wSJW,
                  sStatus.sBtpSdr.wTDO),
      CanBitrate2((CanBitrateMode)sStatus.sBtpFdr.dwMode, 
                  sStatus.sBtpFdr.dwBPS,
                  sStatus.sBtpFdr.wTS1,
                  sStatus.sBtpFdr.wTS2,
                  sStatus.sBtpFdr.wSJW,
                  sStatus.sBtpFdr.wTDO));
  }
  else
  {
    throw gcnew ObjectDisposedException(this->GetType()->FullName);
  }
}

#pragma warning(default:4669) // 'type cast' : unsafe conversion

