/*****************************************************************************
 IXXAT Automation GmbH
******************************************************************************
 Summary : Declarations for the VCI device object class.
 Compiler: Microsoft VC++ 8
******************************************************************************
 all rights reserved
*****************************************************************************/

#pragma once

#include <vcisdk.h>
#include ".\ctrlinf.hpp"
#include ".\Bal\balobj.hpp"


namespace Ixxat {
  namespace Vci4 {

//*****************************************************************************
/// <summary>
///   This class implements a VCI device object.
/// </summary>
//*****************************************************************************
private ref class VciDevice : public IVciDevice
{
  //--------------------------------------------------------------------
  // member variables
  //--------------------------------------------------------------------
  private:
    ::IVciDevice*   m_pDevObj;  // pointer to the native device object
    PVCIDEVICEINFO  m_psDevInf; // pointer to the native device information

  //--------------------------------------------------------------------
  // member functions
  //--------------------------------------------------------------------
  private:
    ::IVciDevice* OpenDevice();

  internal:
    VciDevice( VCIDEVICEINFO& rDevInfo );
    ~VciDevice();

  public:
    virtual String^ ToString() override;

  //--------------------------------------------------------------------
  // IVciDriver implementation
  //--------------------------------------------------------------------
  public:
    virtual property Int64                VciObjectId       { Int64               get(void); };
    virtual property Guid                 DeviceClass       { Guid                get(void); };
    virtual property Version^             DriverVersion     { Version^            get(void); };
    virtual property Version^             HardwareVersion   { Version^            get(void); };
    virtual property Object^              UniqueHardwareId  { Object^             get(void); };
    virtual property String^              Description       { String^             get(void); };
    virtual property String^              Manufacturer      { String^             get(void); };
    virtual property array<IVciCtrlInfo^>^ Equipment        { array<IVciCtrlInfo^>^ get(void); };

    virtual Ixxat::Vci4::Bal::IBalObject^ OpenBusAccessLayer();
};

} // end of namespace Vci4
} // end of namespace Ixxat
