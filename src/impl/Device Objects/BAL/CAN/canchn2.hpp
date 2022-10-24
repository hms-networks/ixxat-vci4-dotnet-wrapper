/*****************************************************************************
 IXXAT Automation GmbH
******************************************************************************
 Summary : Declarations for the CAN channel class.
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
#include "canmsgrd.hpp"
#include "canmsgwr.hpp"

/*************************************************************************
** namespace Ixxat.Vci4.Bal.Can
*************************************************************************/
namespace Ixxat {
  namespace Vci4 {
    namespace Bal {
      namespace Can {


//*****************************************************************************
/// <summary>
///   This class implements a CAN channel.
/// </summary>
//*****************************************************************************
private ref class CanChannel2 : public CanSocket2
                              , public ICanChannel2
{
  //--------------------------------------------------------------------
  // member variables
  //--------------------------------------------------------------------
  private:
    ::ICanChannel2* m_pCanChn; // pointer to the native channel object
    bool            m_fExOpen; // channel is opened in exclusive mode


  //--------------------------------------------------------------------
  // member functions
  //--------------------------------------------------------------------
  private:
    bool InitNew( bool fExclusive );
    void Cleanup( void );

  internal:
    CanChannel2  ( ::IBalObject* pBalObj
                , Byte          bPortNo 
                , Byte          busTypeIndex);
    ~CanChannel2 ( );


  //--------------------------------------------------------------------
  // ICanChannel implementation
  //--------------------------------------------------------------------
  public:

    virtual property CanChannelStatus ChannelStatus { CanChannelStatus get(void); };

    virtual ICanMessageReader^ GetMessageReader(void);
    virtual ICanMessageWriter^ GetMessageWriter(void);

    virtual void Initialize( UInt16 receiveFifoSize
                           , UInt16 transmitFifoSize
                           , UINT32 filterSize
                           , CanFilterModes filterMode
                           , bool   exclusive );
    virtual void Activate  ( void );
    virtual void Deactivate( void );

    virtual UINT8 GetFilterMode(CanFilter bSelect);

    virtual UINT8 SetFilterMode(CanFilter      bSelect,
                                CanFilterModes bMode);

    virtual void SetAccFilter(CanFilter bSelect,
                              UINT32 dwCode,
                              UINT32 dwMask);

    virtual void AddFilterIds(CanFilter bSelect,
                              UINT32 dwCode,
                              UINT32 dwMask);

    virtual void RemFilterIds(CanFilter bSelect,
                              UINT32 dwCode,
                              UINT32 dwMask);
};


} // end of namespace Can
} // end of namespace Bal
} // end of namespace Vci4
} // end of namespace Ixxat

