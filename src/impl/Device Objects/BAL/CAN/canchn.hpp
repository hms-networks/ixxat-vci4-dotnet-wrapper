// SPDX-License-Identifier: MIT
//----------------------------------------------------------------------------
// Summary  : Declarations for the CAN channel class.
// Copyright: Copyright (C) 2016-2022 HMS Technology Center Ravensburg GmbH, 
//            all rights reserved
//----------------------------------------------------------------------------

#pragma once

#include <vcisdk.h>
#include "cansoc.hpp"
#include "canmsgrd.hpp"
#include "canmsgwr.hpp"


namespace Ixxat {
  namespace Vci4 {
    namespace Bal {
      namespace Can {


//*****************************************************************************
/// <summary>
///   This class implements a CAN channel.
/// </summary>
//*****************************************************************************
private ref class CanChannel : public CanSocket
                             , public ICanChannel
{
  //--------------------------------------------------------------------
  // member variables
  //--------------------------------------------------------------------
  private:
    ::ICanChannel* m_pCanChn; // pointer to the native channel object
    bool           m_fExOpen; // channel is opened in exclusive mode


  //--------------------------------------------------------------------
  // member functions
  //--------------------------------------------------------------------
  private:
    bool InitNew( bool fExclusive );
    void Cleanup( void );

  internal:
    CanChannel  ( ::IBalObject* pBalObj
                , Byte          bPortNo 
                , Byte          busTypeIndex);
    ~CanChannel ( );

  //--------------------------------------------------------------------
  // ICanChannel implementation
  //--------------------------------------------------------------------
  public:

    virtual property CanChannelStatus ChannelStatus { CanChannelStatus get(void); };

    virtual ICanMessageReader^ GetMessageReader(void);
    virtual ICanMessageWriter^ GetMessageWriter(void);

    virtual void Initialize( UInt16 receiveFifoSize
                           , UInt16 transmitFifoSize
                           , bool   exclusive );
    virtual void Activate  ( void );
    virtual void Deactivate( void );
};


} // end of namespace Can
} // end of namespace Bal
} // end of namespace Vci4
} // end of namespace Ixxat

