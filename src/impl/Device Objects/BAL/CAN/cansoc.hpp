// SPDX-License-Identifier: MIT
//----------------------------------------------------------------------------
// Summary  : Declarations for the CAN socket class.
// Copyright: Copyright (C) 2016-2022 HMS Technology Center Ravensburg GmbH, 
//            all rights reserved
//----------------------------------------------------------------------------

#pragma once

#include <vcisdk.h>
#include "..\balres.hpp"
#include "..\balobj.hpp"


namespace Ixxat {
  namespace Vci4 {
    namespace Bal {
      namespace Can {

using namespace Ixxat::Vci4::Bal;


//*****************************************************************************
/// <summary>
///   This class implements a CAN socket.
/// </summary>
//*****************************************************************************
private ref class CanSocket : public BalResource,
                              public ICanSocket
{
  //--------------------------------------------------------------------
  // member variables
  //--------------------------------------------------------------------
  private:
    ::ICanSocket*     m_pSocket;  // pointer to the native socket object
    PCANCAPABILITIES  m_psCanCap; // CAN capabilities

  //--------------------------------------------------------------------
  // member functions
  //--------------------------------------------------------------------
  private:
    HRESULT InitNew               ( ::ICanSocket* pSocket );
    void Cleanup                  ( void );

  protected:
    ::ICanSocket* GetNativeSocket ( );

  internal:
    CanSocket                     ( ::IBalObject* pBalObj
                                  , Byte          portNumber 
                                  , Byte          busTypeIndex);
    ~CanSocket                    ( );

  //--------------------------------------------------------------------
  // ICanSocket implementation
  //--------------------------------------------------------------------
  public:
    virtual property CanCtrlType      ControllerType                { CanCtrlType     get(void) sealed; };
    virtual property CanBusCouplings  BusCoupling                   { CanBusCouplings get(void) sealed; };
    virtual property UInt32           ClockFrequency                { UInt32          get(void) sealed; };
    virtual property UInt32           TimeStampCounterDivisor       { UInt32          get(void) sealed; };
    virtual property UInt32           CyclicMessageTimerDivisor     { UInt32          get(void) sealed; };
    virtual property UInt32           MaxCyclicMessageTicks         { UInt32          get(void) sealed; };
    virtual property UInt32           DelayedTXTimerDivisor         { UInt32          get(void) sealed; };
    virtual property UInt32           MaxDelayedTXTicks             { UInt32          get(void) sealed; };
    virtual property CanLineStatus    LineStatus                    { CanLineStatus   get(void) sealed; };
    virtual property CanFeatures      Features                      { CanFeatures     get(void) sealed; };
    virtual property bool             SupportsStdOrExtFrames        { bool            get(void) sealed; };
    virtual property bool             SupportsStdAndExtFrames       { bool            get(void) sealed; };
    virtual property bool             SupportsRemoteFrames          { bool            get(void) sealed; };
    virtual property bool             SupportsErrorFrames           { bool            get(void) sealed; };
    virtual property bool             SupportsBusLoadComputation    { bool            get(void) sealed; };
    virtual property bool             SupportsExactMessageFilter    { bool            get(void) sealed; };
    virtual property bool             SupportsListenOnlyMode        { bool            get(void) sealed; };
    virtual property bool             SupportsCyclicMessageScheduler{ bool            get(void) sealed; };
    virtual property bool             SupportsErrorFrameGeneration  { bool            get(void) sealed; };
    virtual property bool             SupportsDelayedTransmission   { bool            get(void) sealed; };

};

} // end of namespace Can
} // end of namespace Bal
} // end of namespace Vci4
} // end of namespace Ixxat
