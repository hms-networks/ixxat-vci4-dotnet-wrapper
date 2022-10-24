/*****************************************************************************
 IXXAT Automation GmbH
******************************************************************************
 Summary : Declarations for the LIN message reader class.
 Compiler: Microsoft VC++ 8
******************************************************************************
 all rights reserved
*****************************************************************************/

#pragma once

/*************************************************************************
** include files
*************************************************************************/
#include <vcisdk.h>

#include "linmsg.hpp"

/*************************************************************************
** namespace Ixxat.Vci4.Bal.Lin
*************************************************************************/
namespace Ixxat {
  namespace Vci4 {
    namespace Bal {
      namespace Lin {


/*************************************************************************
** used namespace
*************************************************************************/
using namespace System::Threading;
using namespace System::Runtime::InteropServices;


//*****************************************************************************
/// <summary>
///   This class implements a LIN message reader.
/// </summary>
//*****************************************************************************
private ref class LinMessageReader : public ILinMessageReader
{
  //--------------------------------------------------------------------
  // member variables
  //--------------------------------------------------------------------
  private:
    ::IFifoReader* m_pRxFifo; // pointer to the native receive FIFO

  //--------------------------------------------------------------------
  // member functions
  //--------------------------------------------------------------------
  private:
    void Cleanup      ( void );

  internal:
    LinMessageReader  ( ::ILinMonitor* pLinMon );
    ~LinMessageReader ( );

  //--------------------------------------------------------------------
  // ICanMessageReader implementation
  //--------------------------------------------------------------------
  public:
    virtual property UInt16 Capacity  { UInt16 get(void); };
    virtual property UInt16 FillCount { UInt16 get(void); };
    virtual property UInt16 Threshold { UInt16 get(void); 
                                        void   set(UInt16 threshold); };

    virtual void AssignEvent ( AutoResetEvent^      fifoEvent );
    virtual void AssignEvent ( ManualResetEvent^    fifoEvent );
    virtual bool ReadMessage ( [Out] ILinMessage^%          message );

    virtual int  ReadMessages( [Out] array<ILinMessage^>^%  msgarray );
};


} // end of namespace Lin
} // end of namespace Bal
} // end of namespace Vci4
} // end of namespace Ixxat

