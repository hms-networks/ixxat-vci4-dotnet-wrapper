// SPDX-License-Identifier: MIT
//----------------------------------------------------------------------------
// Summary  : Implementation of the VCI device manager object class.
// Copyright: Copyright (C) 2016-2022 HMS Technology Center Ravensburg GmbH, 
//            all rights reserved
//----------------------------------------------------------------------------

#include "devman.hpp"
#include "vcinet.hpp"

using namespace Ixxat::Vci4;

//*****************************************************************************
/// <summary>
///   Constructor for VCI device manager objects.
/// </summary>
//*****************************************************************************
VciDeviceManager::VciDeviceManager()
{
  HRESULT     hResult = E_NOTIMPL;
  ::IVciDeviceManager* pDevMan = nullptr;
 
  if (VciGetDeviceManagerFunc)
  {
    hResult = VciGetDeviceManagerFunc(&pDevMan);
  }

  m_pDevMan = pDevMan;

  if (hResult != VCI_OK)
  {
    throw gcnew VciException(VciServerImpl::Instance(), hResult);
  }
}

//*****************************************************************************
/// <summary>
///   Destructor for VCI device manager objects.
/// </summary>
//*****************************************************************************
VciDeviceManager::~VciDeviceManager()
{
  if (nullptr != m_pDevMan)
  {
    m_pDevMan->Release();
    m_pDevMan = nullptr;
  }
}

//*****************************************************************************
/// <summary>
///   Gets the list of VCI device objects represeted by the 
///   <c>IVciDeviceList</c> interface.
/// </summary>
/// <returns>
///   The list of VCI device objects represeted by the <c>IVciDeviceList</c>
///   interface.
/// </returns>
/// <exception cref="VciException">
///   Thrown if creation of the device list failed.
/// </exception>
/// <exception cref="ObjectDisposedException">
///   Object is already disposed.
/// </exception>
//*****************************************************************************
IVciDeviceList^ VciDeviceManager::GetDeviceList()
{
  if (nullptr == m_pDevMan)
  {
    throw gcnew ObjectDisposedException(this->GetType()->FullName);
  }

  return( gcnew VciDeviceList(m_pDevMan) );
}

