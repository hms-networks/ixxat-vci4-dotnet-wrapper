/*****************************************************************************
 IXXAT Automation GmbH
******************************************************************************
 Summary : Implementation of the BAL object class.
 Compiler: Microsoft VC++ 8
******************************************************************************
 all rights reserved
*****************************************************************************/

/*************************************************************************
** include files
*************************************************************************/
#include "balobj.hpp"
#include "vcinet.hpp"

#include ".\can\cansoc.hpp"
#include ".\can\cansoc2.hpp"
#include ".\can\canctl.hpp"
#include ".\can\canctl2.hpp"
#include ".\can\canchn.hpp"
#include ".\can\canchn2.hpp"
#include ".\can\canshd.hpp"
#include ".\can\canshd2.hpp"

#include ".\lin\linsoc.hpp"
#include ".\lin\linctl.hpp"
#include ".\lin\linmon.hpp"


/*************************************************************************
** used namespaces
*************************************************************************/
using namespace Ixxat::Vci4;
using namespace Ixxat::Vci4::Bal;
using namespace Ixxat::Vci4::Bal::Can;
using namespace Ixxat::Vci4::Bal::Lin;

#pragma warning(disable:4100)     // unreferenced formal parameter
#pragma warning(disable:4702)     // unreachable code

//*****************************************************************************
/// <summary>
///   Constructor for BAL objects.
/// </summary>
/// <param name="pDevice">
///   Pointer to the IVciDevice interface of the VCI device object.
///   This parameter must not be NULL.
///</param>
//*****************************************************************************
BalObject::BalObject(::IVciDevice* pDevice)
{
  HRESULT       hResult;
  ::IBalObject* pBalObj;

  m_pBalObj = nullptr;

  if (nullptr != pDevice)
  {
    hResult = pDevice->OpenComponent(CLSID_VCIBAL, IID_IBalObject, (PVOID*) &pBalObj);
    if (hResult == VCI_OK)
    {
      hResult = InitNew(pBalObj);
      pBalObj->Release();
    }
    
    if (hResult != VCI_OK)
    {
      throw gcnew VciException(VciServerImpl::Instance(), hResult);
    }
  }
  else
  {
    throw gcnew ArgumentNullException();
  }
}

//*****************************************************************************
/// <summary>
///   Destructor for BAL objects.
/// </summary>
//*****************************************************************************
BalObject::~BalObject()
{
  Cleanup();
}

//*****************************************************************************
/// <summary>
///   This method initializes a newly created BAL object.
/// </summary>
/// <param name="pBalObj">
///   Pointer to the native BAL object interface.
///   This parameter must not be NULL.
/// </param>
//*****************************************************************************
HRESULT BalObject::InitNew(::IBalObject* pBalObj)
{
  HRESULT hResult;

  Cleanup();

  if (nullptr != pBalObj)
  {
    // We have to dynamically allocate the native struct because
    // native structs are no longer valid as member of managed classes !!
    m_psBalInf = new BALFEATURES();
    if (nullptr == m_psBalInf)
    {
      throw gcnew InsufficientMemoryException();
    }

    hResult = pBalObj->GetFeatures(m_psBalInf);
    if (hResult == VCI_OK)
    {
      m_pBalObj = pBalObj;
      m_pBalObj->AddRef();

      UINT8 bBusTypeIndex = 0;
      UINT8 bPrevBusType = 0xFF;
      array<BalResource^>^ paSockets = gcnew array<BalResource^>(m_psBalInf->BusSocketCount);
      for (UINT8 idx = 0; idx < m_psBalInf->BusSocketCount; idx++)
      {
        UINT8 bType = VCI_BUS_TYPE(m_psBalInf->BusSocketType[idx]);
        if (bPrevBusType != bType)
        {
          bPrevBusType = bType;
          bBusTypeIndex = 0;
        }
        else
        {
          bBusTypeIndex++;
        }

        paSockets[idx] = gcnew BalResource(idx, (VciBusType) bType, bBusTypeIndex);
      }
      m_pSocCol = gcnew BalResourceCollection(paSockets);
    }
  }
  else
  {
    hResult = VCI_E_INVALIDARG;
  }

  return( hResult );
}

//*****************************************************************************
/// <summary>
///   This method performs tasks associated with freeing, releasing, or
///   resetting unmanaged resources.
/// </summary>
//*****************************************************************************
void BalObject::Cleanup(void)
{
  m_pSocCol = nullptr;

  if (nullptr != m_pBalObj)
  {
    m_pBalObj->Release();
    m_pBalObj = nullptr;
  }

  if (nullptr != m_psBalInf)
  {
    delete m_psBalInf;
    m_psBalInf = nullptr;
  }
}

//*****************************************************************************
/// <summary>
///   Gets a <c>BalResourceCollection</c> that can be used to iterate through
///   the available BAL resources or to directly access such one via a
///   collection index.
/// </summary>
/// <returns>
///   A reference to the collection of BAL resources.
/// </returns>
/// <exception cref="ObjectDisposedException">
///   Object is already disposed.
/// </exception>
//*****************************************************************************
BalResourceCollection^ BalObject::Resources::get(void)
{
  if (nullptr == m_pBalObj)
  {
    throw gcnew ObjectDisposedException(this->GetType()->FullName);
  }

  return m_pSocCol;
}

  //*****************************************************************************
  /// <summary>
  ///   This method opens the specified bus socket.
  /// </summary>
  /// <param name="portNumber">
  ///   Number of the bus socket to open. This parameter must be within the 
  ///   range of 0 to <c>Resources.Count</c> - 1.
  /// </param>
  /// <param name="socketType">
  ///   Type of the bus socket to open. The supported socket types
  ///   are depending on the <c>BusType</c> of the BAL resource specified by the 
  ///   <c>portNumber</c> parameter.
  ///   I.e. for a CAN bus socket the following <c>socketTypes</c> are supported:
  ///     ICanSocket, 
  ///     ICanControl, 
  ///     ICanChannel, 
  ///     ICanScheduler.
  ///   It's possible have several socketType open at the same time (i.e.
  ///   ICanControl and ICanChannel).
  /// </param>
  /// <returns>
  ///   If the method succeeds it returns the opened bus socket object as 
  ///   <c>IBalResource</c> reference. This reference can be casted to
  ///   the type specified by parameter <paramref name="socketType"/>.
  ///   If the method fails it returns a null reference (Nothing in
  ///   VisualBasic).
  ///   When no longer needed the returned socket object has to be disposed using 
  ///   the IDisposable interface. 
  /// </returns>
  /// <remarks>
  ///   The type of the bus socket is implicitly specified by the
  ///   <c>portNumber</c> parameter (see <c>IBalResource.BusType</c> property).
  ///
  ///   The VCI interfaces provide access to native driver resources. Because the 
  ///   .NET garbage collector is only designed to manage memory, but not 
  ///   native OS and driver resources the caller is responsible to release this 
  ///   resources via calling <c>IDisposable.Dispose()</c> when the object is no 
  ///   longer needed. Otherwise native memory and resource leaks may occure.
  /// </remarks>
  /// <exception cref="VciException">
  ///   Opening socket failed.
  /// </exception>
  /// <exception cref="ObjectDisposedException">
  ///   Object is already disposed.
  /// </exception>
  /// <exception cref="ArgumentOutOfRangeException">
  ///   The specified port number is out of range.
  /// </exception>
  /// <exception cref="NotImplementedException">
  ///   There's no implementation for the specified <paramref name="socketType"/>.
  /// </exception>
  //*****************************************************************************
IBalResource^ BalObject::OpenSocket( Byte   portNumber
                                   , Type^  socketType)
{
  IBalResource^ pSocket = nullptr;

  if (nullptr != m_pBalObj)
  {
    if (portNumber < m_psBalInf->BusSocketCount)
    {
      Byte bBusTypeIndex = ((BalResource^)m_pSocCol->default[portNumber])->BusTypeIndex;
      switch (VCI_BUS_TYPE(m_psBalInf->BusSocketType[portNumber]))
      {
        //----------------------------------------------------------------
        case VCI_BUS_CAN:
        //----------------------------------------------------------------
        {
          // ICanSocket
          if (socketType->Equals(Ixxat::Vci4::Bal::Can::ICanSocket::typeid))
          {
            pSocket = gcnew CanSocket(m_pBalObj, portNumber, bBusTypeIndex);
          }
          // ICanSocket2
          else if (socketType->Equals(Ixxat::Vci4::Bal::Can::ICanSocket2::typeid))
          {
            pSocket = gcnew CanSocket2(m_pBalObj, portNumber, bBusTypeIndex);
          }
          // ICanControl
          else if (socketType->Equals(Ixxat::Vci4::Bal::Can::ICanControl::typeid))
          {
            pSocket = gcnew CanControl(m_pBalObj, portNumber, bBusTypeIndex);
          }
          // ICanControl2
          else if (socketType->Equals(Ixxat::Vci4::Bal::Can::ICanControl2::typeid))
          {
            pSocket = gcnew CanControl2(m_pBalObj, portNumber, bBusTypeIndex);
          }
          // ICanChannel
          else if (socketType->Equals(Ixxat::Vci4::Bal::Can::ICanChannel::typeid))
          {
            pSocket = gcnew CanChannel(m_pBalObj, portNumber, bBusTypeIndex);
          }
          // ICanChannel2
          else if (socketType->Equals(Ixxat::Vci4::Bal::Can::ICanChannel2::typeid))
          {
            pSocket = gcnew CanChannel2(m_pBalObj, portNumber, bBusTypeIndex);
          }
          // ICanScheduler
          else if (socketType->Equals(Ixxat::Vci4::Bal::Can::ICanScheduler::typeid))
          {
            pSocket = gcnew CanScheduler(m_pBalObj, portNumber, bBusTypeIndex);
          }
          // ICanScheduler2
          else if (socketType->Equals(Ixxat::Vci4::Bal::Can::ICanScheduler2::typeid))
          {
            pSocket = gcnew CanScheduler2(m_pBalObj, portNumber, bBusTypeIndex);
          }
          else
          {
            throw gcnew NotImplementedException();
          }
        } break;

        //----------------------------------------------------------------
        case VCI_BUS_LIN:
        //----------------------------------------------------------------
        {
          // ILinSocket
          if (socketType->Equals(Ixxat::Vci4::Bal::Lin::ILinSocket::typeid))
          {
            pSocket = gcnew LinSocket(m_pBalObj, portNumber, bBusTypeIndex);
          }
          // ILinControl
          else if (socketType->Equals(Ixxat::Vci4::Bal::Lin::ILinControl::typeid))
          {
            pSocket = gcnew LinControl(m_pBalObj, portNumber, bBusTypeIndex);
          }
          // ILinMonitor
          else if (socketType->Equals(Ixxat::Vci4::Bal::Lin::ILinMonitor::typeid))
          {
            pSocket = gcnew LinMonitor(m_pBalObj, portNumber, bBusTypeIndex);
          }
        } break;

        //----------------------------------------------------------------
        case VCI_BUS_FXR:
        //----------------------------------------------------------------
        // not implemented

        //----------------------------------------------------------------
        default:
        //----------------------------------------------------------------
        {
          throw gcnew NotImplementedException();
          break;
        }
      }
    }
    else
    {
      throw gcnew ArgumentOutOfRangeException("portNumber");
    }
  }
  else
  {
    throw gcnew ObjectDisposedException(this->GetType()->FullName);
  }

  return( pSocket );
}

//*****************************************************************************
/// <summary>
///   Gets the firmware version.
/// </summary>
/// <returns>
///   The firmware version.
/// </returns>
/// <exception cref="ObjectDisposedException">
///   Object is already disposed.
/// </exception>
//*****************************************************************************
Version^ BalObject::FirmwareVersion::get()
{
  if (nullptr == m_psBalInf)
  {
    throw gcnew ObjectDisposedException(this->GetType()->FullName);
  }

  return gcnew Version( m_psBalInf->FwMajorVersion
                      , m_psBalInf->FwMinorVersion);
}

#pragma warning(default:4100)     // unreferenced formal parameter
#pragma warning(default:4702)     // unreachable code
