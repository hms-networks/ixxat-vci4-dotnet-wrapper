// SPDX-License-Identifier: MIT
//----------------------------------------------------------------------------
// Summary  : Implementation for the VCI server object.
// Copyright: Copyright (C) 2016-2022 HMS Technology Center Ravensburg GmbH, 
//            all rights reserved
//----------------------------------------------------------------------------

#include <windows.h>
#include "vcinet.hpp"

using namespace Ixxat::Vci4;
using namespace System::IO;

// function pointers for dynamically loaded VCI4 entry points
HANDLE hVciLib = NULL;
VciInitializeDyn VciInitializeFunc = NULL;
VciGetVersionDyn VciGetVersionFunc = NULL;
VciGetDeviceManagerDyn VciGetDeviceManagerFunc = NULL;
VciFormatErrorWDyn VciFormatErrorWFunc = NULL;
Vci3FormatErrorDyn Vci3FormatErrorFunc = NULL;

//*****************************************************************************
/// <summary>
///   Constructor for VCI server objects.
/// </summary>
//*****************************************************************************
VciServerImpl::VciServerImpl()
{
  // create the message factory
  m_msgFactory = gcnew Ixxat::Vci4::MsgFactory();

  Initialize();
}

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
Version^ VciServerImpl::Version::get(void)
{
  HRESULT         hResult = E_NOTIMPL;
  VCIVERSIONINFO  VerInfo = { 0 };
  
  if (VciGetVersionFunc)
  {
    hResult = VciGetVersionFunc(&VerInfo);
  }
  
  if (hResult != VCI_OK)
    throw gcnew VciException(VciServerImpl::Instance(), hResult);
  
  return gcnew System::Version(VerInfo.VciMajorVersion,
                               VerInfo.VciMinorVersion,
                               VerInfo.VciRevNumber,
                               VerInfo.VciBuildNumber);
}

//*****************************************************************************
/// <summary>
///   Gets the reference to a new VCI device manager instance.
///   When no longer needed the VCI device manager object has to be disposed 
///   using the IDisposable interface. 
/// </summary>
/// <returns>
///   A reference to the VCI device manager.
/// </returns>
/// <exception cref="VciException">
///   Thrown if getting the VCI device manager failed.
/// </exception>
/// <remarks>
///   The VCI interfaces provide access to native driver resources. Because the 
///   .NET garbage collector is only designed to manage memory, but not 
///   native OS and driver resources the caller is responsible to release this 
///   resources via calling <c>IDisposable.Dispose()</c> when the object is no 
///   longer needed. Otherwise native memory and resource leaks may occure.
/// </remarks>
/// <example>
///   <code>
///   IVciDeviceManager deviceManager = VciServer.GetDeviceManager();
///   // Use deviceManager here
///   // ...
///   // Dispose object to release native resources
///   deviceManager.Dispose();
///   </code>
/// </example>
//*****************************************************************************
Ixxat::Vci4::IVciDeviceManager^ VciServerImpl::DeviceManager::get(void)
{
  return( gcnew VciDeviceManager() );
}

Ixxat::Vci4::IMessageFactory^ VciServerImpl::MsgFactory::get(void)
{
  return( m_msgFactory );
}

String^ VciServerImpl::GetErrorMsg(int errorCode)
{
  if (VciFormatErrorWFunc)
  {
    WCHAR szBuffer[1024] = {0};
    VciFormatErrorWFunc( errorCode, szBuffer, (sizeof(szBuffer)/sizeof(WCHAR))-1 );
    return( gcnew String(szBuffer) );
  }
  else if (Vci3FormatErrorFunc)
  {
    CHAR szBuffer[1024] = {0};
    Vci3FormatErrorFunc( errorCode, szBuffer );
    return( gcnew String(szBuffer) );
  }

  String^ errmsg = String::Format("Internal error: VCIFormatError not available. Thrown errorCode: 0x{0:X}", errorCode);
  throw gcnew VciException(errmsg);
}

//*****************************************************************************
/// <summary>
///   The method initializes the vci server. It loads the vci dll dynamically
///   and tries to call VciInitialize
/// </summary>
//*****************************************************************************
void VciServerImpl::Initialize()
{
  HRESULT hResult;
  UINT len = 0;
  DWORD size;
  DWORD handle = 0;
  int aVersion[4];
  BYTE* versionInfo;
  LPCWSTR szDllName = L"vciapi.dll";
  VS_FIXEDFILEINFO* vsfi = NULL;

  // try to access the shared library
  if (NULL == hVciLib)
  {
    hVciLib = LoadLibrary(szDllName);
    if(NULL == hVciLib){
      FileLoadException^ e = gcnew FileLoadException();
      throw gcnew FileLoadException(e->Message, "vciapi.dll");
    }

    // read version information from shared library
    size = GetFileVersionInfoSize(szDllName, &handle);
    if(size > 0)
    {
      versionInfo = new BYTE[size];
      if (!GetFileVersionInfo(szDllName, handle, size, versionInfo))
      {
          delete[] versionInfo;
          throw gcnew FileLoadException("Could not retrieve the dll's product version!");
      }

      if(VerQueryValue(versionInfo, L"\\", (void**)&vsfi, &len)){
        aVersion[0] = HIWORD(vsfi->dwProductVersionMS);
        aVersion[1] = LOWORD(vsfi->dwProductVersionMS);
        aVersion[2] = HIWORD(vsfi->dwProductVersionLS);
        aVersion[3] = LOWORD(vsfi->dwProductVersionLS);

        // this wrapper works with VCI3 and VCI4
        if ((aVersion[0] < 3) && (aVersion[0] > 4))
        {
          throw gcnew FileLoadException("VCI version mismatch. Please install the VCI V3 or VCI V4 driver!");
        }
      }else{
        throw gcnew FileLoadException("Could not retrieve the dll's product version!");
      }

      delete[] versionInfo;
    }

    VciInitializeFunc = (VciInitializeDyn)GetProcAddress((HMODULE)hVciLib, "VciInitialize");

    // first try to load VciGetVersion2 which is available on VCI3 only
    VciGetVersionFunc = (VciGetVersionDyn)GetProcAddress((HMODULE)hVciLib, "VciGetVersion2");
    if (NULL == VciGetVersionFunc)
    {
      // if this fails we have a VCI4 installation and load VciGetVersion which
      // has the same signature
      VciGetVersionFunc = (VciGetVersionDyn)GetProcAddress((HMODULE)hVciLib, "VciGetVersion");
    }

    VciGetDeviceManagerFunc = (VciGetDeviceManagerDyn)GetProcAddress((HMODULE)hVciLib, "VciGetDeviceManager");

    // try to load VciFormatErrorW which is available on VCI4 only
    VciFormatErrorWFunc   = (VciFormatErrorWDyn)GetProcAddress((HMODULE)hVciLib, "VciFormatErrorW");
    // try to load VciFormatError which is available on VCI3 only
    Vci3FormatErrorFunc   = (Vci3FormatErrorDyn)GetProcAddress((HMODULE)hVciLib, "VciFormatError");

    hResult = VciInitializeFunc();

    if (hResult != VCI_OK)
    {
      throw gcnew VciException(VciServerImpl::Instance(), hResult);
    }
  }
}