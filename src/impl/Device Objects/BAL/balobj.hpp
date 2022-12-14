// SPDX-License-Identifier: MIT
//----------------------------------------------------------------------------
// Summary  : Declarations for the BAL object class.
// Copyright: Copyright (C) 2016-2022 HMS Technology Center Ravensburg GmbH, 
//            all rights reserved
//----------------------------------------------------------------------------

#pragma once

#include <vcisdk.h>

using namespace Ixxat::Vci4;
using namespace System;
using namespace System::Collections::Generic;


namespace Ixxat {
  namespace Vci4 {
    namespace Bal {

//*****************************************************************************
/// <summary>
///   This class implements the BAL object.
/// </summary>
//*****************************************************************************
private ref class BalObject : public IBalObject
{
  //--------------------------------------------------------------------
  // member variables
  //--------------------------------------------------------------------
  private:
    ::IBalObject*           m_pBalObj;    // pointer to the native device object
    PBALFEATURES            m_psBalInf;   // BAL features
    BalResourceCollection^  m_pSocCol;    // collection of available sockets

  //--------------------------------------------------------------------
  // member functions
  //--------------------------------------------------------------------
  private:
    HRESULT InitNew( ::IBalObject* pBalObj );
    void    Cleanup( void );

  internal:
    BalObject ( ::IVciDevice* pDevice );
    ~BalObject();

  //--------------------------------------------------------------------
  // IBalObject implementation
  //--------------------------------------------------------------------
  public:
    virtual property Version^               FirmwareVersion{ Version^               get(void); };
    virtual property BalResourceCollection^ Resources      { BalResourceCollection^ get(void); };

    virtual IBalResource^ OpenSocket( Byte  portNumber
                                    , Type^ socketType );
};

} // end of namespace Bal
} // end of namespace Vci4
} // end of namespace Ixxat
