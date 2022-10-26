// SPDX-License-Identifier: MIT
//----------------------------------------------------------------------------
// Summary  : Declarations for the CAN socket class.
// Copyright: Copyright (C) 2016-2022 HMS Technology Center Ravensburg GmbH, 
//            all rights reserved
//----------------------------------------------------------------------------

#pragma once

#include <vcisdk.h>
#include ".\cansoc.hpp"
#include "..\balobj.hpp"


namespace Ixxat {
  namespace Vci4 {
    namespace Bal {
      namespace Can {

//*****************************************************************************
/// <summary>
///   This class implements a CAN socket.
/// </summary>
//*****************************************************************************
private ref class CanSocket2 : public BalResource,
                               public ICanSocket2
{
  //--------------------------------------------------------------------
  // member variables
  //--------------------------------------------------------------------
  private:
    ::ICanSocket2*     m_pSocket;  // pointer to the native socket object
    PCANCAPABILITIES2  m_psCanCap; // CAN capabilities

  //--------------------------------------------------------------------
  // member functions
  //--------------------------------------------------------------------
  private:
    HRESULT InitNew               ( ::ICanSocket2* pSocket );
    void Cleanup                  ( void );

  protected:
    ::ICanSocket2*                GetNativeSocket ( );

  internal:
    CanSocket2                    ( ::IBalObject* pBalObj
                                  , Byte          portNumber 
                                  , Byte          busTypeIndex);
    ~CanSocket2                   ( );
  public:

  //--------------------------------------------------------------------
  // ICanSocket implementation
  //--------------------------------------------------------------------
  public:
    virtual property CanCtrlType      ControllerType                { CanCtrlType     get(void); };
    virtual property CanBusCouplings  BusCoupling                   { CanBusCouplings get(void); };
    virtual property CanFeatures      Features                      { CanFeatures     get(void); };
    virtual property UInt32           CanClockFrequency             { UInt32          get(void); };
    virtual property CanBitrate2      MinimumArbitrationBitrate     { CanBitrate2     get(void); };
    virtual property CanBitrate2      MaximumArbitrationBitrate     { CanBitrate2     get(void); };
    virtual property CanBitrate2      MinimumFastDataBitrate        { CanBitrate2     get(void); };
    virtual property CanBitrate2      MaximumFastDataBitrate        { CanBitrate2     get(void); };
    virtual property UInt32           TimeStampCounterClockFrequency{ UInt32          get(void); };
    virtual property UInt32           TimeStampCounterDivisor       { UInt32          get(void); };
    virtual property UInt32           CyclicMessageTimerClockFrequency { UInt32        get(void); };
    virtual property UInt32           CyclicMessageTimerDivisor     { UInt32          get(void); };
    virtual property UInt32           MaxCyclicMessageTicks         { UInt32          get(void); };
    virtual property UInt32           DelayedTXTimerClockFrequency  { UInt32          get(void); };
    virtual property UInt32           DelayedTXTimerDivisor         { UInt32          get(void); };
    virtual property UInt32           MaxDelayedTXTicks             { UInt32          get(void); };
    virtual property CanLineStatus2   LineStatus                    { CanLineStatus2  get(void); };
    virtual property bool             SupportsStdOrExtFrames        { bool            get(void); };
    virtual property bool             SupportsStdAndExtFrames       { bool            get(void); };
    virtual property bool             SupportsRemoteFrames          { bool            get(void); };
    virtual property bool             SupportsErrorFrames           { bool            get(void); };
    virtual property bool             SupportsBusLoadComputation    { bool            get(void); };
    virtual property bool             SupportsExactMessageFilter    { bool            get(void); };
    virtual property bool             SupportsListenOnlyMode        { bool            get(void); };
    virtual property bool             SupportsCyclicMessageScheduler{ bool            get(void); };
    virtual property bool             SupportsErrorFrameGeneration  { bool            get(void); };
    virtual property bool             SupportsDelayedTransmission   { bool            get(void); };
    virtual property bool             SupportsSingleShotMessages    { bool            get(void); };
    virtual property bool             SupportsHighPriorityMessages  { bool            get(void); };
    virtual property bool             SupportsAutoBaudrateDetection { bool            get(void); };
    virtual property bool             SupportsExtendedDataLength    { bool            get(void); };
    virtual property bool             SupportsFastDataRate          { bool            get(void); };
    virtual property bool             SupportsIsoCanFdFrames        { bool            get(void); };
    virtual property bool             SupportsNonIsoCanFdFrames     { bool            get(void); };
    virtual property bool             Supports64BitTimeStamps       { bool            get(void); };
};

} // end of namespace Can
} // end of namespace Bal
} // end of namespace Vci4
} // end of namespace Ixxat
