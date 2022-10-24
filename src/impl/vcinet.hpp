/*****************************************************************************
 IXXAT Automation GmbH
******************************************************************************
 Summary : Declarations for the VCI server object.
 Compiler: Microsoft VC++ 8
******************************************************************************
 all rights reserved
*****************************************************************************/

#pragma once

/*************************************************************************
** include files
*************************************************************************/

#include ".\Device Manager\devman.hpp"

typedef HRESULT (VCIAPI *VciInitializeDyn)();
typedef HRESULT (VCIAPI *VciGetVersionDyn)(OUT PVCIVERSIONINFO pVersInfo);
typedef HRESULT (VCIAPI *VciGetDeviceManagerDyn)(OUT ::IVciDeviceManager** ppDevMan);
typedef HRESULT (VCIAPI *VciFormatErrorWDyn)(IN HRESULT hrError, OUT PWCHAR pszError, IN  UINT32 dwLength);
typedef HRESULT (VCIAPI *Vci3FormatErrorDyn)(IN HRESULT hrError, OUT PCHAR pszError);

extern HANDLE hVciLib;
extern VciInitializeDyn VciInitializeFunc;
extern VciGetVersionDyn VciGetVersionFunc;
extern VciGetDeviceManagerDyn VciGetDeviceManagerFunc;
extern VciFormatErrorWDyn VciFormatErrorWFunc;
extern Vci3FormatErrorDyn Vci3FormatErrorFunc;

/*************************************************************************
** namespace Ixxat.Vci4
*************************************************************************/
namespace Ixxat {
  namespace Vci4 {

//*****************************************************************************
/// <summary>
///   This class represents the entry point for working with the VCI. 
///   Use <c>DeviceManager</c> to get access to the installed VCI devices.
/// </summary>
//*****************************************************************************
public ref class VciServerImpl sealed : IVciServer
{
  private:
    static VciServerImpl^ ms_instance = nullptr;

  public:
    static VciServerImpl^ Instance()
    {
      if (nullptr == ms_instance)
      {
        ms_instance = gcnew VciServerImpl();
      }
      return ms_instance;
    }

  private:
    IMessageFactory^ m_msgFactory;

  //--------------------------------------------------------------------
  // properties
  //--------------------------------------------------------------------
  public:
    //*****************************************************************************
    /// <summary>
    ///   Gets the version of the VCI server.
    /// </summary>
    /// <returns>
    ///   The version of the VCI server.
    /// </returns>
    /// <exception cref="VciException">
    ///   Thrown if getting the version number failed.
    /// </exception>
    //*****************************************************************************
    virtual property System::Version^   Version   { System::Version^  get(void); };

    virtual property IVciDeviceManager^ DeviceManager { IVciDeviceManager^  get(void); };

    virtual property IMessageFactory^   MsgFactory    { IMessageFactory^    get(void); };

    virtual String^  GetErrorMsg(int errorCode);

  //--------------------------------------------------------------------
  // member functions
  //--------------------------------------------------------------------
  protected:
    VciServerImpl();

  protected:
    void Initialize();
};

ref class MsgFactory : public IMessageFactory
{

public:
  virtual System::Object^ CreateMsg(System::Type^ typ);

};


} // end of namespace Vci4
} // end of namespace Ixxat


