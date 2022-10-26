// SPDX-License-Identifier: MIT
//----------------------------------------------------------------------------
// Summary  : Implementation of the LIN socket class.
// Copyright: Copyright (C) 2016-2022 HMS Technology Center Ravensburg GmbH, 
//            all rights reserved
//----------------------------------------------------------------------------

#include "linsoc.hpp"
#include "vcinet.hpp"


using namespace Ixxat::Vci4::Bal::Lin;

#pragma warning(disable:4669) // 'type cast' : unsafe conversion

//*****************************************************************************
/// <summary>
///   Constructor for LIN socket objects.
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
///   Creation of LIN socket failed.
/// </exception>
/// <exception cref="ArgumentNullException">
///   Native IBalObject was a null pointer.
/// </exception>
/// <exception cref="OutOfMemoryException">
///   Memory allocation failed.
/// </exception>
//*****************************************************************************
LinSocket::LinSocket( ::IBalObject* pBalObj
                    , Byte          portNumber
                    , Byte          busTypeIndex)
         : BalResource(portNumber, VciBusType::Lin, busTypeIndex)
{
  HRESULT       hResult;
  ::ILinSocket* pSocket;

  m_pSocket   = nullptr;
  m_psLinCap  = nullptr;

  if (nullptr != pBalObj)
  {
    hResult = pBalObj->OpenSocket( portNumber
                                 , IID_ILinSocket
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
///   Destructor for LIN socket objects.
/// </summary>
//*****************************************************************************
LinSocket::~LinSocket()
{
  Cleanup();
}

//*****************************************************************************
/// <summary>
///   This method initializes a newly created socket object.
/// </summary>
/// <param name="pSocket">
///   Pointer to the native LIN socket object.
///   This parameter must not be NULL.
/// </param>
/// <returns>
///   VCI_OK if succeeded, otherwise a VCI error code.
/// </returns>
/// <exception cref="OutOfMemoryException">
///   Memory allocation failed.
/// </exception>
//*****************************************************************************
HRESULT LinSocket::InitNew(::ILinSocket* pSocket)
{
  HRESULT hResult;

  Cleanup();

  if (nullptr != pSocket)
  {
    // We have to dynamically allocate the native struct because
    // native structs are no longer valid as member of managed classes !!
    m_psLinCap = new LINCAPABILITIES;
    if (nullptr == m_psLinCap)
    {
      throw gcnew InsufficientMemoryException();
    }

    hResult = pSocket->GetCapabilities(m_psLinCap);
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
void LinSocket::Cleanup(void)
{
  if (nullptr != m_pSocket)
  {
    m_pSocket->Release();
    m_pSocket = nullptr;
  }

  if (nullptr != m_psLinCap)
  {
    delete m_psLinCap;
    m_psLinCap = nullptr;
  }
}

//*****************************************************************************
/// <summary>
///   This method gets a pointer to the native LIN socket object.
/// </summary>
/// <returns>
///   If the method succeeds it returns a pointer to the native LIN socket
///   object, otherwise the method returns NULL.
/// </returns>
/// <remarks>
///   The caller must Release the pointer returned by this method if it is
///   no longer needed.
/// </remarks>
//*****************************************************************************
::ILinSocket* LinSocket::GetNativeSocket()
{
  ::ILinSocket* pSocket = m_pSocket;

  if (nullptr != pSocket)
  {
    pSocket->AddRef();
  }

  return( pSocket );
}

//*****************************************************************************
/// <summary>
///   Gets a flag field indicating the features supported by the LIN 
///   controller.
/// </summary>
/// <returns>
///   A flag field indicating the features supported by the LIN 
///   controller.
/// </returns>
/// <exception cref="ObjectDisposedException">
///   Object is already disposed.
/// </exception>
//*****************************************************************************
LinFeatures LinSocket::Features::get()
{
  if (nullptr == m_psLinCap)
  {
    throw gcnew ObjectDisposedException(this->GetType()->FullName);
  }

  return( (LinFeatures) m_psLinCap->dwFeatures );
}


//*****************************************************************************
/// <summary>
///   Gets a value indicating if the LIN socket supports LIN master mode.
/// </summary>
/// <exception cref="ObjectDisposedException">
///   Object is already disposed.
/// </exception>
//*****************************************************************************
bool LinSocket::SupportsMasterMode::get()
{ 
  if (nullptr == m_psLinCap)
  {
    throw gcnew ObjectDisposedException(this->GetType()->FullName);
  }

  return( (m_psLinCap->dwFeatures & LIN_FEATURE_MASTER) != 0);
}

//*****************************************************************************
/// <summary>
///   Gets a value indicating if the LIN socket supports automatic baudrate
///   detection.
/// </summary>
/// <exception cref="ObjectDisposedException">
///   Object is already disposed.
/// </exception>
//*****************************************************************************
bool LinSocket::SupportsAutorate::get()
{ 
  if (nullptr == m_psLinCap)
  {
    throw gcnew ObjectDisposedException(this->GetType()->FullName);
  }

  return( (m_psLinCap->dwFeatures & LIN_FEATURE_AUTORATE) != 0);
}

//*****************************************************************************
/// <summary>
///   Gets a value indicating if the LIN socket supports error frame reception.
/// </summary>
/// <exception cref="ObjectDisposedException">
///   Object is already disposed.
/// </exception>
//*****************************************************************************
bool LinSocket::SupportsErrorFrames::get()
{ 
  if (nullptr == m_psLinCap)
  {
    throw gcnew ObjectDisposedException(this->GetType()->FullName);
  }

  return( (m_psLinCap->dwFeatures & LIN_FEATURE_ERRFRAME) != 0);
};

//*****************************************************************************
/// <summary>
///   Gets a value indicating if the LIN socket supports bus load computation.
/// </summary>
/// <exception cref="ObjectDisposedException">
///   Object is already disposed.
/// </exception>
//*****************************************************************************
bool LinSocket::SupportsBusLoadComputation::get()
{ 
  if (nullptr == m_psLinCap)
  {
    throw gcnew ObjectDisposedException(this->GetType()->FullName);
  }

  return( (m_psLinCap->dwFeatures & LIN_FEATURE_BUSLOAD) != 0);
};

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
UInt32 LinSocket::ClockFrequency::get()
{
  if (nullptr == m_psLinCap)
  {
    throw gcnew ObjectDisposedException(this->GetType()->FullName);
  }

  return( m_psLinCap->dwClockFreq );
}

//*****************************************************************************
/// <summary>
///   Gets the divisor factor of the time stamp counter. 
///   The time stamp counter provides the time stamp for LIN messages. 
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
UInt32 LinSocket::TimeStampCounterDivisor::get()
{
  if (nullptr == m_psLinCap)
  {
    throw gcnew ObjectDisposedException(this->GetType()->FullName);
  }

  return( m_psLinCap->dwTscDivisor );
}

//*****************************************************************************
/// <summary>
///   Gets the current status of the LIN line.
/// </summary>
/// <returns>
///   The current status of the LIN line.
/// </returns>
/// <exception cref="VciException">
///   Getting LIN line status failed.
/// </exception>
/// <exception cref="ObjectDisposedException">
///   Object is already disposed.
/// </exception>
//*****************************************************************************
LinLineStatus LinSocket::LineStatus::get()
{
  HRESULT       hResult;

  if (nullptr != m_pSocket)
  {
    LINLINESTATUS sStatus;
    hResult = m_pSocket->GetLineStatus(&sStatus);
    if (hResult != VCI_OK)
    {
      throw gcnew VciException(VciServerImpl::Instance(), hResult);
    }

    return LinLineStatus((LinOperatingModes)sStatus.bOpMode, sStatus.bBusLoad, (LinCtrlStatus)sStatus.dwStatus, sStatus.wBitrate);
  }
  else
  {
    throw gcnew ObjectDisposedException(this->GetType()->FullName);
  }
}

#pragma warning(default:4669) // 'type cast' : unsafe conversion

