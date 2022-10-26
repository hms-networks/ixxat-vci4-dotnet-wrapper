/*****************************************************************************
 IXXAT Automation GmbH
******************************************************************************
 Summary : Message factory implementation
 Compiler: 
******************************************************************************
 all rights reserved
*****************************************************************************/

#include <windows.h>
#include "vcinet.hpp"
#include "./Device Objects/BAL/CAN/canmsg.hpp"
#include "./Device Objects/BAL/CAN/canmsg2.hpp"
#include "./Device Objects/BAL/LIN/linmsg.hpp"

namespace Ixxat {
  namespace Vci4 {

    using namespace System;
    using namespace Ixxat::Vci4::Bal::Can;
    using namespace Ixxat::Vci4::Bal::Lin;

    System::Object^ MsgFactory::CreateMsg(System::Type^ typ)
    {
      if (typ == ICanMessage::typeid)
        return gcnew CanMessage();
      else if (typ == ICanMessage2::typeid)
        return gcnew CanMessage2();
      else if (typ == ILinMessage::typeid)
        return gcnew LinMessage();
      else
      {
        throw gcnew ArgumentException("Requested type not supported", "typ");
      }
      return nullptr;
    }


  }
}