/*****************************************************************************
 IXXAT Automation GmbH
******************************************************************************
 Summary : Implementation of the VCI device object class.
 Compiler: Microsoft VC++ 8
******************************************************************************
 all rights reserved
*****************************************************************************/

#include "devobj.hpp"
#include "vcinet.hpp"

using namespace Ixxat::Vci4;

//*****************************************************************************
/// <summary>
///   Constructor for VCI device objects.
/// </summary>
/// <param name="rDevInfo">
///   Reference to an initialized native VCI devcie info record.
///</param>
//*****************************************************************************
VciDevice::VciDevice(VCIDEVICEINFO& rDevInfo)
{
  m_pDevObj = nullptr;

  // We have to dynamically allocate the native struct because
  // native structs are no longer valid as member of managed classes !!
  m_psDevInf = new VCIDEVICEINFO;
  if (nullptr == m_psDevInf)
  {
    throw gcnew InsufficientMemoryException();
  }
  *m_psDevInf = rDevInfo;
}

//*****************************************************************************
/// <summary>
///   Destructor for VCI device objects.
/// </summary>
//*****************************************************************************
VciDevice::~VciDevice()
{
  if (nullptr != m_pDevObj)
  {
    m_pDevObj->Release();
    m_pDevObj = nullptr;
  }

  if (nullptr != m_psDevInf)
  {
    delete m_psDevInf;
    m_psDevInf = nullptr;
  }
}

//*****************************************************************************
/// <summary>
///   This method opens the VCI device object and retrieves a pointer
///   to it's native IVciDevice interface.
/// </summary>
/// <returns>
///   A pointer to the native IVciDevice interface of the opened device.
/// </returns>
//*****************************************************************************
::IVciDevice* VciDevice::OpenDevice()
{
  ::IVciDevice* pDevObj = nullptr;

  if (nullptr == m_pDevObj)
  {
    HRESULT              hResult = E_NOTIMPL;
    ::IVciDeviceManager *  pDevMan = nullptr;

    if (VciGetDeviceManagerFunc)
    {
      hResult = VciGetDeviceManagerFunc(&pDevMan);
      if (hResult == VCI_OK)
      {
        hResult = pDevMan->OpenDevice(m_psDevInf->VciObjectId, &pDevObj);
        if (hResult == VCI_OK)
        {
          m_pDevObj = pDevObj;
          m_pDevObj->AddRef();
        }

        pDevMan->Release();
      }
    }

    if (hResult != VCI_OK)
    {
      throw gcnew VciException(VciServerImpl::Instance(), hResult);
    }
  }
  else
  {
    m_pDevObj->AddRef();
    pDevObj = m_pDevObj;
  }

  return( pDevObj );
}

//*****************************************************************************
/// <summary>
///   Gets the unique VCI object id of the device.
/// </summary>
/// <returns>
///   Unique VCI object id of the device.
/// </returns>
/// <exception cref="ObjectDisposedException">
///   Object is already disposed.
/// </exception>
//*****************************************************************************
Int64 VciDevice::VciObjectId::get()
{
  if (nullptr == m_psDevInf)
  {
    throw gcnew ObjectDisposedException(this->GetType()->FullName);
  }

  return m_psDevInf->VciObjectId.AsInt64;
}

//*****************************************************************************
/// <summary>
///   Gets the ID of the device class. Each device driver identifies its device 
///   class in the form of a globally unique ID (GUID). Different adapters 
///   belong to different device classes. Applications can use the device 
///   class to distinguish between an IPC-I165/PCI and a PC-I04/PCI card, for 
///   example.
/// </summary>
/// <returns>
///   Class id of the VCI device object.
/// </returns>
/// <exception cref="ObjectDisposedException">
///   Object is already disposed.
/// </exception>
//*****************************************************************************
Guid VciDevice::DeviceClass::get()
{
  if (nullptr == m_psDevInf)
  {
    throw gcnew ObjectDisposedException(this->GetType()->FullName);
  }

  return System::Guid( m_psDevInf->DeviceClass.Data1,
                       m_psDevInf->DeviceClass.Data2,
                       m_psDevInf->DeviceClass.Data3,
                       m_psDevInf->DeviceClass.Data4[0],
                       m_psDevInf->DeviceClass.Data4[1],
                       m_psDevInf->DeviceClass.Data4[2],
                       m_psDevInf->DeviceClass.Data4[3],
                       m_psDevInf->DeviceClass.Data4[4],
                       m_psDevInf->DeviceClass.Data4[5],
                       m_psDevInf->DeviceClass.Data4[6],
                       m_psDevInf->DeviceClass.Data4[7] );
}

//*****************************************************************************
/// <summary>
///   Gets the version of the VCI device driver.
/// </summary>
/// <returns>
///   Version of the VCI device driver.
/// </returns>
/// <exception cref="ObjectDisposedException">
///   Object is already disposed.
/// </exception>
//*****************************************************************************
Version^ VciDevice::DriverVersion::get()
{
  if (nullptr == m_psDevInf)
  {
    throw gcnew ObjectDisposedException(this->GetType()->FullName);
  }

  return gcnew Version( m_psDevInf->DriverMajorVersion
                      , m_psDevInf->DriverMinorVersion
                      , m_psDevInf->DriverReleaseVersion
                      , m_psDevInf->DriverBuildVersion);
}

//*****************************************************************************
/// <summary>
///   Gets the version of the VCI device hardware.
/// </summary>
/// <returns>
///   Version of the VCI device hardware.
/// </returns>
/// <exception cref="ObjectDisposedException">
///   Object is already disposed.
/// </exception>
//*****************************************************************************
Version^ VciDevice::HardwareVersion::get()
{
  if (nullptr == m_psDevInf)
  {
    throw gcnew ObjectDisposedException(this->GetType()->FullName);
  }

  return gcnew Version( m_psDevInf->HardwareMajorVersion
                      , m_psDevInf->HardwareMinorVersion
                      , m_psDevInf->HardwareBranchVersion
                      , m_psDevInf->HardwareBuildVersion);
}

//*****************************************************************************
/// <summary>
///   Gets the unique ID of the adapter. Each adapter has a unique ID that can 
///   be used to distinguish between two PC-I04/PCI cards, for example. 
///   Because this value can be either a GUID or a string with the serial 
///   number the retrieved value is either a string reference or a boxed Guid 
///   instance. 
/// </summary>
/// <returns>
///   Unique hardware id of the device.
/// </returns>
/// <exception cref="ObjectDisposedException">
///   Object is already disposed.
/// </exception>
//*****************************************************************************
Object^ VciDevice::UniqueHardwareId::get()
{
  Object^ rHardwareId = nullptr;

  if (nullptr == m_psDevInf)
  {
    throw gcnew ObjectDisposedException(this->GetType()->FullName);
  }

  if ( ('H' == m_psDevInf->UniqueHardwareId.AsChar[0]) &&
       ('W' == m_psDevInf->UniqueHardwareId.AsChar[1]) )
  {
    rHardwareId = gcnew String( m_psDevInf->UniqueHardwareId.AsChar
                              , 0
                              , sizeof(m_psDevInf->UniqueHardwareId.AsChar));
  }
  else
  {
    rHardwareId = System::Guid( m_psDevInf->UniqueHardwareId.AsGuid.Data1,
                                m_psDevInf->UniqueHardwareId.AsGuid.Data2,
                                m_psDevInf->UniqueHardwareId.AsGuid.Data3,
                                m_psDevInf->UniqueHardwareId.AsGuid.Data4[0],
                                m_psDevInf->UniqueHardwareId.AsGuid.Data4[1],
                                m_psDevInf->UniqueHardwareId.AsGuid.Data4[2],
                                m_psDevInf->UniqueHardwareId.AsGuid.Data4[3],
                                m_psDevInf->UniqueHardwareId.AsGuid.Data4[4],
                                m_psDevInf->UniqueHardwareId.AsGuid.Data4[5],
                                m_psDevInf->UniqueHardwareId.AsGuid.Data4[6],
                                m_psDevInf->UniqueHardwareId.AsGuid.Data4[7] );
  } 

  return rHardwareId;
}

//*****************************************************************************
/// <summary>
///   Gets the device description string.
/// </summary>
/// <returns>
///   The device description string.
/// </returns>
/// <exception cref="ObjectDisposedException">
///   Object is already disposed.
/// </exception>
//*****************************************************************************
String^ VciDevice::Description::get()
{
  if (nullptr == m_psDevInf)
  {
    throw gcnew ObjectDisposedException(this->GetType()->FullName);
  }

  return gcnew String((char*)m_psDevInf->Description);
}

//*****************************************************************************
/// <summary>
///   Gets the device manufacturer string.
/// </summary>
/// <returns>
///   The device manufacturer string.
/// </returns>
/// <exception cref="ObjectDisposedException">
///   Object is already disposed.
/// </exception>
//*****************************************************************************
String^ VciDevice::Manufacturer::get()
{
  if (nullptr == m_psDevInf)
  {
    throw gcnew ObjectDisposedException(this->GetType()->FullName);
  }

  return gcnew String((char*)m_psDevInf->Manufacturer);
}

//*****************************************************************************
/// <summary>
///   Gets a description of the hardware equipment of the device.
/// </summary>
/// <returns>
///   The retrieved array contains a <c>VciCtrlInfo</c> for each 
///   existing fieldbus controller.
/// </returns>
/// <exception cref="ObjectDisposedException">
///   Object is already disposed.
/// </exception>
//*****************************************************************************
array<IVciCtrlInfo^>^ VciDevice::Equipment::get(void)
{
  HRESULT              hResult;
  ::IVciDevice*        pDevObj;
  VCIDEVICECAPS        sDevCap;
  array<VciCtrlInfo^>^  aCtrlInfos = nullptr;

  if (nullptr == m_psDevInf)
  {
    throw gcnew ObjectDisposedException(this->GetType()->FullName);
  }

  pDevObj = OpenDevice();

  if (nullptr != pDevObj)
  {
    try
    {
      hResult = pDevObj->GetDeviceCaps(&sDevCap);
      if (hResult == VCI_OK)
      {
        aCtrlInfos = gcnew array<VciCtrlInfo^>(sDevCap.BusCtrlCount);
        for (UINT8 idx = 0; idx < sDevCap.BusCtrlCount; idx++)
        {
          aCtrlInfos[idx] = gcnew VciCtrlInfo(sDevCap.BusCtrlTypes[idx]);
        }
      }
    }
    finally
    {
      pDevObj->Release();
    }
  }

  return( aCtrlInfos );
}

//*****************************************************************************
/// <summary>
///   This method is called to open the Bus Access Layer.
/// </summary>
/// <returns>
///   If succeeded a reference to the Bus Access Layer, otherwise a null 
///   reference (Nothing in VisualBasic).
///   When no longer needed the BAL object has to be disposed using the 
///   IDisposable interface. 
/// </returns>
/// <remarks>
///   The VCI interfaces provide access to native driver resources. Because the 
///   .NET garbage collector is only designed to manage memory, but not 
///   native OS and driver resources the caller is responsible to release this 
///   resources via calling <c>IDisposable.Dispose()</c> when the object is no 
///   longer needed. Otherwise native memory and resource leaks may occure.
/// </remarks>
/// <exception cref="ObjectDisposedException">
///   Object is already disposed.
/// </exception>
//*****************************************************************************
Ixxat::Vci4::Bal::IBalObject^ VciDevice::OpenBusAccessLayer()
{
  ::IVciDevice*                 pDevObj;
  Ixxat::Vci4::Bal::BalObject^  pBal;

  if (nullptr == m_psDevInf)
  {
    throw gcnew ObjectDisposedException(this->GetType()->FullName);
  }

  pDevObj = OpenDevice();

  pBal = nullptr;

  if (nullptr != pDevObj)
  {
    try
    {
      pBal = gcnew Ixxat::Vci4::Bal::BalObject(pDevObj);
    }
    finally
    {
      pDevObj->Release();
    }
  }

  return( pBal );
}

//*****************************************************************************
/// <summary>
///   Returns a String that represents the current Object.
/// </summary>
/// <returns>
///   A String that represents the current Object.
/// </returns>
//*****************************************************************************
String^ VciDevice::ToString ()
{
  return String::Format("[{0:X16}] {1} - {2}", (UInt64)VciObjectId, Manufacturer, Description);
}
