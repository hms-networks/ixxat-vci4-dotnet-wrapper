// SPDX-License-Identifier: MIT
//----------------------------------------------------------------------------
// Summary  : Implementation of the LIN monitor class.
// Copyright: Copyright (C) 2016-2022 HMS Technology Center Ravensburg GmbH, 
//            all rights reserved
//----------------------------------------------------------------------------

#include "linmon.hpp"
#include "vcinet.hpp"


using namespace Ixxat::Vci4::Bal::Lin;

#pragma warning(disable:4669) // 'type cast' : unsafe conversion


//*****************************************************************************
/// <summary>
///   Constructor for VCI LIN monitor objects.
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
LinMonitor::LinMonitor( ::IBalObject* pBalObj
                      , Byte          bPortNo
                      , Byte          busTypeIndex)
          : LinSocket(pBalObj, bPortNo, busTypeIndex)
{
  m_pLinMon = NULL;

  // FxCop: "Do not make initializations,
  // that have already been done by the runtime."
  // m_fExOpen = false; 
} 

//*****************************************************************************
/// <summary>
///   Destructor for VCI LIN monitor objects.
/// </summary>
//*****************************************************************************
LinMonitor::~LinMonitor()
{
  Cleanup();
}

//*****************************************************************************
/// <summary>
///   This method initializes the monitor object.
/// </summary>
/// <param name="fExclusive">
///   If this parameter is set to true the method tries to
///   create the monitor in exclusive mode, otherwise the
///   method creates a shared monitor.
/// </param>
/// <returns>
///   true on success, otherwise false.
/// </returns>
/// <exception cref="VciException">
///   Creation of native monitor failed.
/// </exception>
/// <exception cref="ArgumentNullException">
///   Native ILinSocket was NULL.
/// </exception>
//*****************************************************************************
bool LinMonitor::InitNew(bool fExclusive)
{
  HRESULT         hResult;
  ::ILinSocket*   pSocket;
  ::ILinMonitor*  pLinMon;

  if ((nullptr == m_pLinMon) || (fExclusive != m_fExOpen))
  {
    Cleanup();

    pSocket = GetNativeSocket();
    if (nullptr != pSocket)
    {
      hResult = pSocket->CreateMonitor(fExclusive, &pLinMon);
      pSocket->Release();

      if (hResult == VCI_OK)
      {
        m_fExOpen = fExclusive;
        m_pLinMon = pLinMon;
      }
      else
      {
        throw gcnew VciException(VciServerImpl::Instance(), hResult);
      }
    }
  }

  return( nullptr != m_pLinMon );
}

//*****************************************************************************
/// <summary>
///   This method performs tasks associated with freeing, releasing, or
///   resetting unmanaged resources.
/// </summary>
//*****************************************************************************
void LinMonitor::Cleanup(void)
{
  if (nullptr != m_pLinMon)
  {
    m_pLinMon->Release();
    m_pLinMon = nullptr;
    m_fExOpen = false;
  }
}

//*****************************************************************************
/// <summary>
///   This method initializes the monitor. This method must be called
///   prior to any other method of the interface.
/// </summary>
/// <param name="receiveFifoSize">
///   Size of the receive buffer (number of messages)
/// </param>
/// <param name="exclusive">
///   If this parameter is set to true the method tries
///   to initialize the monitor for exclusive use. If set
///   to false, the method initializes the monitor for
///   shared access.
/// </param>
/// <exception cref="VciException">
///   Monitor initialization failed.
/// </exception>
/// <exception cref="ObjectDisposedException">
///   Object is already disposed.
/// </exception>
/// <remarks>
///   The monitor is deactivated after this method returns an must be
///   activeted by an Activate() method call.
///   The method can be called more than once to reconfigure the size
///   of the receive FIFOs.
/// </remarks>
//*****************************************************************************
void LinMonitor::Initialize( UInt16 receiveFifoSize
                           , bool   exclusive)
{
  HRESULT hResult;
  bool    fResult;

  fResult = InitNew(exclusive);
  if (true == fResult)
  {
    hResult = m_pLinMon->Initialize(receiveFifoSize);
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
///   This method activates the monitor. After activating the monitor,
///   messages can received through the message reader.
/// </summary>
/// <exception cref="VciException">
///   Monitor activation failed.
/// </exception>
/// <exception cref="ObjectDisposedException">
///   Object is already disposed or not initialized, yet.
/// </exception>
/// <remarks>
///   The LIN line must be started, otherwise no messages are received from 
///   the LIN line (see also ILinControl::StartLine).
/// </remarks>
//*****************************************************************************
void LinMonitor::Activate(void)
{
  HRESULT hResult;

  if (nullptr != m_pLinMon)
  {
    hResult = m_pLinMon->Activate();
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
///   This method deactivates the monitor. After deactivating the monitor,
///   no further messages are received from the LIN line.
/// </summary>
/// <returns>
///   true on success, otherwise false.
/// </returns>
/// <exception cref="VciException">
///   Monitor deactivation failed.
/// </exception>
/// <exception cref="ObjectDisposedException">
///   Object is already disposed or not initialized, yet.
/// </exception>
//*****************************************************************************
void LinMonitor::Deactivate(void)
{
  HRESULT hResult;

  if (nullptr != m_pLinMon)
  {
    hResult = m_pLinMon->Deactivate();
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
///   Gets the current status of the monitor.
/// </summary>
/// <returns>
///   The current status of the monitor.
/// </returns>
/// <exception cref="VciException">
///   Getting monitor status failed.
/// </exception>
/// <exception cref="ObjectDisposedException">
///   Object is already disposed or not initialized, yet.
/// </exception>
//*****************************************************************************
LinMonitorStatus LinMonitor::MonitorStatus::get()
{
  HRESULT       hResult;

  if (nullptr != m_pLinMon)
  {
    LINMONITORSTATUS sStatus;
    hResult = m_pLinMon->GetStatus(&sStatus);
    if (hResult != VCI_OK)
    {
      throw gcnew VciException(VciServerImpl::Instance(), hResult);
    }

    return LinMonitorStatus(sStatus.fActivated != 0, sStatus.fRxOverrun != 0, sStatus.bRxFifoLoad);
  }
  else
  {
    throw gcnew ObjectDisposedException(this->GetType()->FullName);
  }
}

//*****************************************************************************
/// <summary>
///   Gets a reference to the message reader of the monitor which provides
///   access to the monitor's receive buffer.
///   Messages received from the LIN line can be read from this object.
/// </summary>
/// <returns>
///   A reference to the message reader of the monitor.
///   When no longer needed the message reader object has to be 
///   disposed using the IDisposable interface. 
/// </returns>
/// <remarks>
///   The VCI interfaces provides access to native driver resources. Because the 
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
ILinMessageReader^ LinMonitor::GetMessageReader()
{
  LinMessageReader^ pReader = nullptr;

  if (nullptr != m_pLinMon)
  {
    pReader = gcnew LinMessageReader(m_pLinMon);
  }
  else
  {
    throw gcnew ObjectDisposedException(this->GetType()->FullName);
  }

  return( pReader );
}

#pragma warning(default:4669) // 'type cast' : unsafe conversion
