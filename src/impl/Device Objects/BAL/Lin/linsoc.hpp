/*****************************************************************************
 IXXAT Automation GmbH
******************************************************************************
 Summary : Declarations for the LIN socket class.
 Compiler: Microsoft VC++ 8
******************************************************************************
 all rights reserved
*****************************************************************************/

#pragma once

#include <vcisdk.h>
#include "..\balobj.hpp"
#include "..\balres.hpp"


namespace Ixxat {
  namespace Vci4 {
    namespace Bal {
      namespace Lin {

//*****************************************************************************
/// <summary>
///   This class implements a LIN socket.
/// </summary>
//*****************************************************************************
private ref class LinSocket : public BalResource,
                              public ILinSocket
{
  //--------------------------------------------------------------------
  // member variables
  //--------------------------------------------------------------------
  private:
    ::ILinSocket*     m_pSocket;  // pointer to the native socket object
    PLINCAPABILITIES  m_psLinCap; // LIN capabilities

  //--------------------------------------------------------------------
  // member functions
  //--------------------------------------------------------------------
  private:
    HRESULT InitNew               ( ::ILinSocket* pSocket );
    void Cleanup                  ( void );

  protected:
    ::ILinSocket* GetNativeSocket ( );

  internal:
    LinSocket                     ( ::IBalObject* pBalObj
                                  , Byte          portNumber 
                                  , Byte          busTypeIndex);
    ~LinSocket                    ( );

  //--------------------------------------------------------------------
  // ILinSocket implementation
  //--------------------------------------------------------------------
  public:
    virtual property LinLineStatus  LineStatus                  { LinLineStatus get(void); };
    virtual property LinFeatures    Features                    { LinFeatures   get(void); };
    virtual property bool           SupportsMasterMode          { bool          get(void); };
    virtual property bool           SupportsAutorate            { bool          get(void); };
    virtual property bool           SupportsErrorFrames         { bool          get(void); };
    virtual property bool           SupportsBusLoadComputation  { bool          get(void); };
    virtual property UInt32         ClockFrequency              { UInt32        get(void); };
    virtual property UInt32         TimeStampCounterDivisor     { UInt32        get(void); };
};

} // end of namespace Lin
} // end of namespace Bal
} // end of namespace Vci4
} // end of namespace Ixxat
