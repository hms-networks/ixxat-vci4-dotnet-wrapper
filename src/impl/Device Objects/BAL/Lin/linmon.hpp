/*****************************************************************************
 IXXAT Automation GmbH
******************************************************************************
 Summary : Declarations for the LIN channel class.
 Compiler: Microsoft VC++ 8
******************************************************************************
 all rights reserved
*****************************************************************************/

#pragma once

/*************************************************************************
** include files
*************************************************************************/
#include <vcisdk.h>

#include "linsoc.hpp"
#include "linmsgrd.hpp"

/*************************************************************************
** namespace Ixxat.Vci4.Bal.Lin
*************************************************************************/
namespace Ixxat {
  namespace Vci4 {
    namespace Bal {
      namespace Lin {


//*****************************************************************************
/// <summary>
///   This class implements a LIN monitor.
/// </summary>
//*****************************************************************************
private ref class LinMonitor : public LinSocket
                             , public ILinMonitor
{
  //--------------------------------------------------------------------
  // member variables
  //--------------------------------------------------------------------
  private:
    ::ILinMonitor* m_pLinMon; // pointer to the native monitor object
    bool           m_fExOpen; // monitor is opened in exclusive mode

  //--------------------------------------------------------------------
  // member functions
  //--------------------------------------------------------------------
  private:
    bool InitNew( bool fExclusive );
    void Cleanup( void );

  internal:
    LinMonitor  ( ::IBalObject* pBalObj
                , Byte          bPortNo 
                , Byte          busTypeIndex);
    ~LinMonitor ( );
  public:

  //--------------------------------------------------------------------
  // ILinMonitor implementation
  //--------------------------------------------------------------------
  public:

    virtual property LinMonitorStatus MonitorStatus { LinMonitorStatus get(void); };

    virtual ILinMessageReader^ GetMessageReader(void);

    virtual void Initialize( UInt16 receiveFifoSize
                           , bool   exclusive );
    virtual void Activate  ( void );
    virtual void Deactivate( void );
};


} // end of namespace Lin
} // end of namespace Bal
} // end of namespace Vci4
} // end of namespace Ixxat

