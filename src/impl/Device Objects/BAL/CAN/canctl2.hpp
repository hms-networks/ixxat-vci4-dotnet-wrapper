/*****************************************************************************
 IXXAT Automation GmbH
******************************************************************************
 Summary : Declarations for the CAN control class.
 Compiler: Microsoft VC++ 8
******************************************************************************
 all rights reserved
*****************************************************************************/

#pragma once

/*************************************************************************
** include files
*************************************************************************/
#include <vcisdk.h>

#include "cansoc2.hpp"
#include "canchn2.hpp"

/*************************************************************************
** namespace Ixxat.Vci4.Bal.Can
*************************************************************************/
namespace Ixxat {
  namespace Vci4 {
    namespace Bal {
      namespace Can {


//*****************************************************************************
/// <summary>
///   This class implements a CAN control socket.
/// </summary>
//*****************************************************************************
private ref class CanControl2 : public CanSocket2
                              , public ICanControl2 
{
  //--------------------------------------------------------------------
  // member variables
  //--------------------------------------------------------------------
  private:
    ::ICanControl2* m_pCanCtl; // pointer to the native control object


  //--------------------------------------------------------------------
  // member functions
  //--------------------------------------------------------------------
  private:
    HRESULT InitNew ( ::ICanControl2*  pCanCtl );
    void Cleanup    ( void );

  internal:
    CanControl2      ( ::IBalObject* pBalObj
                    , Byte          portNumber 
                    , Byte          busTypeIndex);
    ~CanControl2     ( );


  //--------------------------------------------------------------------
  // ICanSocket implementation
  //--------------------------------------------------------------------
  public:
    virtual int   DetectBaud  ( CanOperatingModes operatingMode
                              , CanExtendedOperatingModes extendedMode
                              , UInt16 timeout
                              , array<CanFdBitrate>^ bitrateTable );
    virtual void  InitLine    ( CanOperatingModes operatingMode
                              , CanExtendedOperatingModes extendedMode
                              , CanFilterModes filterModeStd 
                              , UInt32 cntIdsStd
                              , CanFilterModes filterModeExt
                              , UInt32 cntIdsExt
                              , CanBitrate2 bitrate
                              , CanBitrate2 extendedBitrate );
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


