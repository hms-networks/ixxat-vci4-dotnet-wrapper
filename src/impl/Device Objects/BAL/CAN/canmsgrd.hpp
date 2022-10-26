/*****************************************************************************
 IXXAT Automation GmbH
******************************************************************************
 Summary : Declarations for the CAN message reader class.
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
private ref class CanMessageReader : public ICanMessageReader
{
  //--------------------------------------------------------------------
  // member variables
  //--------------------------------------------------------------------
  private:
    bool          m_isCanChannel2;
    PFIFOREADER   m_pRxFifo; // pointer to the native receive FIFO

  //--------------------------------------------------------------------
  // member functions
  //--------------------------------------------------------------------
  private:
    void Cleanup      ( void );

  internal:
    CanMessageReader  ( ::ICanChannel*    pCanChan );
    CanMessageReader  ( ::ICanChannel2*   pCanChan );
    ~CanMessageReader ( );

  //--------------------------------------------------------------------
  // ICanMessageReader implementation
  //--------------------------------------------------------------------
  public:
    virtual property UInt16 Capacity  { UInt16 get(void); };
    virtual property UInt16 FillCount { UInt16 get(void); };
    virtual property UInt16 Threshold { UInt16 get(void); 
                                        void   set(UInt16 threshold); };

    virtual void Lock();
    virtual void Unlock();
    virtual void AssignEvent ( AutoResetEvent^      fifoEvent );
    virtual void AssignEvent ( ManualResetEvent^    fifoEvent );
    virtual bool ReadMessage ( ICanMessage^%        message );
    virtual bool ReadMessage ( ICanMessage2^%       message );

    virtual int  ReadMessages( [Out] array<ICanMessage^>^%   msgarray );
    virtual int  ReadMessages( [Out] array<ICanMessage2^>^%  msgarray );
};


} // end of namespace Can
} // end of namespace Bal
} // end of namespace Vci4
} // end of namespace Ixxat

