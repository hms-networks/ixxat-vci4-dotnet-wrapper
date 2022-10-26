/*****************************************************************************
 IXXAT Automation GmbH
******************************************************************************
 Summary : Declarations for the LIN control class.
 Compiler: Microsoft VC++ 8
******************************************************************************
 all rights reserved
*****************************************************************************/

#pragma once

#include <vcisdk.h>
#include "linsoc.hpp"
#include "linmsg.hpp"


namespace Ixxat {
  namespace Vci4 {
    namespace Bal {
      namespace Lin {


//*****************************************************************************
/// <summary>
///   This class implements a LIN control socket.
/// </summary>
//*****************************************************************************
private ref class LinControl : public LinSocket
                             , public ILinControl 
{
  //--------------------------------------------------------------------
  // member variables
  //--------------------------------------------------------------------
  private:
    ::ILinControl* m_pLinCtl; // pointer to the native control object


  //--------------------------------------------------------------------
  // member functions
  //--------------------------------------------------------------------
  private:
    HRESULT InitNew ( ::ILinControl*  pLinCtl );
    void Cleanup    ( void );

  internal:
    LinControl      ( ::IBalObject* pBalObj
                    , Byte          portNumber 
                    , Byte          busTypeIndex);
    ~LinControl     ( );

  //--------------------------------------------------------------------
  // ILinControl implementation
  //--------------------------------------------------------------------
  public:
    virtual void InitLine    (LinInitLine  initLine );
    virtual void ResetLine   (void );
    virtual void StartLine   (void );
    virtual void StopLine    (void );
    virtual void WriteMessage(bool         send, 
                              ILinMessage^ message);
};


} // end of namespace Lin
} // end of namespace Bal
} // end of namespace Vci4
} // end of namespace Ixxat


