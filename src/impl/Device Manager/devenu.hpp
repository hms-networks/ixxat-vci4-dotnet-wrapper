/*****************************************************************************
 IXXAT Automation GmbH
******************************************************************************
 Summary : Declarations for the VCI device enumerator object class.
 Compiler: Microsoft VC++ 8
******************************************************************************
 all rights reserved
*****************************************************************************/

#pragma once

/*************************************************************************
** include files
*************************************************************************/
#include <vcisdk.h>

#include "..\Device Objects\devobj.hpp"

/*************************************************************************
** used namespaces
*************************************************************************/
using namespace System::Threading;
using namespace System::Collections;

/*************************************************************************
** namespace Ixxat.Vci4
*************************************************************************/
namespace Ixxat {
  namespace Vci4 {

//*****************************************************************************
/// <summary>
///   This class represents the list of installed VCI devices.
///   To observe changes within this list use the <c>AssignEvent</c> methods 
///   to register an event. This event is set to the signaled state whenever 
///   the contents of the device list changes.
/// </summary>
//*****************************************************************************
private ref class VciDeviceList : public IVciDeviceList
{
  //--------------------------------------------------------------------
  // member variables
  //--------------------------------------------------------------------
  private:
    ::IVciDeviceManager * m_pDevMan; // native IVciDeviceManager interface
    ::IVciEnumDevice    * m_pEnuDev; // native IVciEnumDevice interface

  //--------------------------------------------------------------------
  // member functions
  //--------------------------------------------------------------------
  internal:
    VciDeviceList ( ::IVciDeviceManager * pDeviceManager );
    ~VciDeviceList( );

  public:
    virtual void AssignEvent( AutoResetEvent^   changeEvent );
    virtual void AssignEvent( ManualResetEvent^ changeEvent );

  //--------------------------------------------------------------------
  // IEnumerable implementation
  //--------------------------------------------------------------------
  public:
    virtual Collections::IEnumerator^ GetEnumerator();
};


//*****************************************************************************
/// <summary>
///   This class is used to enumerate the a list of VCI device objects.
/// </summary>
//*****************************************************************************
private ref class VciDeviceEnumerator : public Collections::IEnumerator
{
  //--------------------------------------------------------------------
  // member variables
  //--------------------------------------------------------------------
  private:
    ::IVciEnumDevice* m_pEnuDev; // IVciEnumDevice interface
    IVciDevice^       m_pCurDev; // current device object

  //--------------------------------------------------------------------
  // member functions
  //--------------------------------------------------------------------
  internal:
    VciDeviceEnumerator( ::IVciEnumDevice* pEnuDev );
    ~VciDeviceEnumerator();

  //--------------------------------------------------------------------
  // IEnumerator implementation
  //--------------------------------------------------------------------
  public:
    virtual property Object^ Current { Object^ get(void); };
    virtual bool MoveNext (void);
    virtual void Reset    (void);
};


} // end of namespace Vci4
} // end of namespace Ixxat
