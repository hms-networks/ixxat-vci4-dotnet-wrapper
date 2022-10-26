// SPDX-License-Identifier: MIT
//----------------------------------------------------------------------------
// Summary  : Declarations for the VCI device manager object class.
// Copyright: Copyright (C) 2016-2022 HMS Technology Center Ravensburg GmbH, 
//            all rights reserved
//----------------------------------------------------------------------------

#pragma once

#include <vcisdk.h>
#include "devenu.hpp"


namespace Ixxat {
  namespace Vci4 {

//*****************************************************************************
/// <summary>
///   The VCI device manager object manages the list of VCI device objects.
/// </summary>
//*****************************************************************************
private ref class VciDeviceManager : public IVciDeviceManager
{
  //--------------------------------------------------------------------
  // member variables
  //--------------------------------------------------------------------
  private:
    ::IVciDeviceManager * m_pDevMan; // native IVciDeviceManager interface

  //--------------------------------------------------------------------
  // member functions
  //--------------------------------------------------------------------
  internal:
    VciDeviceManager();
    ~VciDeviceManager();

  //--------------------------------------------------------------------
  // IVciDeviceManager implementation
  //--------------------------------------------------------------------
  public:
    virtual IVciDeviceList^ GetDeviceList(void);
};

} // end of namespace Vci4
} // end of namespace Ixxat
