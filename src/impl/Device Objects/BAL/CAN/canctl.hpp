// SPDX-License-Identifier: MIT
//----------------------------------------------------------------------------
// Summary  : Declarations for the CAN control class.
// Copyright: Copyright (C) 2016-2022 HMS Technology Center Ravensburg GmbH, 
//            all rights reserved
//----------------------------------------------------------------------------

#pragma once

#include <vcisdk.h>
#include "cansoc.hpp"


namespace Ixxat {
  namespace Vci4 {
    namespace Bal {
      namespace Can {


//*****************************************************************************
/// <summary>
///   This class implements a CAN control socket.
/// </summary>
//*****************************************************************************
private ref class CanControl : public CanSocket
                             , public ICanControl 
{
  //--------------------------------------------------------------------
  // member variables
  //--------------------------------------------------------------------
  private:
    ::ICanControl* m_pCanCtl; // pointer to the native control object

  //--------------------------------------------------------------------
  // member functions
  //--------------------------------------------------------------------
  private:
    HRESULT InitNew ( ::ICanControl*  pCanCtl );
    void Cleanup    ( void );

  internal:
    CanControl      ( ::IBalObject* pBalObj
                    , Byte          portNumber 
                    , Byte          busTypeIndex);
    ~CanControl     ( );

  //--------------------------------------------------------------------
  // ICanSocket implementation
  //--------------------------------------------------------------------
  public:
    virtual int   DetectBaud  ( UInt16 timeout, array<CanBitrate>^ bitrateTable );
    virtual void  InitLine    ( CanOperatingModes operatingMode, CanBitrate bitrate );
    virtual void  ResetLine   ( void );
    virtual void  StartLine   ( void );
    virtual void  StopLine    ( void );
    virtual void  SetAccFilter( CanFilter select, UInt32 code, UInt32 mask );
    virtual void  AddFilterIds( CanFilter select, UInt32 code, UInt32 mask );
    virtual void  RemFilterIds( CanFilter select, UInt32 code, UInt32 mask );
};


} // end of namespace Can
} // end of namespace Bal
} // end of namespace Vci4
} // end of namespace Ixxat


