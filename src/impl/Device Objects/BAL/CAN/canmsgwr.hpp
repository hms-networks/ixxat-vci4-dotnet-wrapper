/*****************************************************************************
 IXXAT Automation GmbH
******************************************************************************
 Summary : Declarations for the CAN message writer class.
 Compiler: Microsoft VC++ 8
******************************************************************************
 all rights reserved
*****************************************************************************/

#pragma once

#include <vcisdk.h>
#include "canmsg.hpp"
#include "canmsg2.hpp"


namespace Ixxat {
  namespace Vci4 {
    namespace Bal {
      namespace Can {

using namespace System::Threading;
using namespace System::Runtime::InteropServices;


//*****************************************************************************
/// <summary>
///   This class implements a CAN message reader.
/// </summary>
//*****************************************************************************
private ref class CanMessageWriter : public ICanMessageWriter
{
  //--------------------------------------------------------------------
  // member variables
  //--------------------------------------------------------------------
  private:
    bool          m_isCanChannel2;
    ::IFifoWriter* m_pTxFifo; // pointer to the native transmit FIFO

  //--------------------------------------------------------------------
  // member functions
  //--------------------------------------------------------------------
  private:
    void Cleanup      ( void );

  internal:
    CanMessageWriter  ( ::ICanChannel*    pCanChan );
    CanMessageWriter  ( ::ICanChannel2*   pCanChan );
    ~CanMessageWriter ( );

  //--------------------------------------------------------------------
  // ICanMessageWriter implementation
  //--------------------------------------------------------------------
  public:
    virtual property UInt16 Capacity  { UInt16 get(void); };
    virtual property UInt16 FreeCount { UInt16 get(void); };
    virtual property UInt16 Threshold { UInt16 get(void); 
                                        void   set(UInt16 threshold); };

    virtual void Lock();
    virtual void Unlock();
    virtual void AssignEvent ( AutoResetEvent^      fifoEvent );
    virtual void AssignEvent ( ManualResetEvent^    fifoEvent );
    virtual bool SendMessage ( ICanMessage^         message );
    virtual bool SendMessage ( ICanMessage2^        message );
};


} // end of namespace Can
} // end of namespace Bal
} // end of namespace Vci4
} // end of namespace Ixxat
