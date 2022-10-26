/*****************************************************************************
 IXXAT Automation GmbH
******************************************************************************
 Summary : Implementation of the VCI device enumerator object class.
 Compiler: Microsoft VC++ 8
******************************************************************************
 all rights reserved
*****************************************************************************/

#include "devenu.hpp"
#include "vcinet.hpp"

#include "..\Device Objects\devobj.hpp"

using namespace Ixxat::Vci4;


/*##########################################################################*/
/*### Methods for VciDeviceList class                                    ###*/
/*##########################################################################*/

//*****************************************************************************
/// <summary>
///   Constructor for new VCI device list objects.
/// </summary>
/// <param name="pDeviceManager">
///   Pointer to the native device manager interface
///</param>
//*****************************************************************************
VciDeviceList::VciDeviceList(::IVciDeviceManager * pDeviceManager)
{
  HRESULT         hResult;
  IVciEnumDevice* pEnuDev;

  m_pEnuDev = nullptr;
  m_pDevMan = nullptr;

  if (nullptr != pDeviceManager)
  {
    pEnuDev = nullptr;
    hResult = pDeviceManager->EnumDevices(&pEnuDev);

    if (hResult == VCI_OK)
    {
      m_pEnuDev = pEnuDev;
      m_pDevMan = pDeviceManager;
      m_pDevMan->AddRef();
    }
    else
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
///   Destructor for VCI device list objects.
/// </summary>
/// <remarks>
///   The compiler generates a IDisposable.Dispose() of the destructor.
/// </remarks>
//*****************************************************************************
VciDeviceList::~VciDeviceList()
{
  if (nullptr != m_pEnuDev)
  {
    m_pEnuDev->Release();
    m_pEnuDev = nullptr;
  }

  if (nullptr != m_pDevMan)
  {
    m_pDevMan->Release();
    m_pDevMan = nullptr;
  }
}

//*****************************************************************************
/// <summary>
///   This method assigns an event object to the list. The event is
///   set to the signaled state whenever the contents of the device list
///   changes.
/// </summary>
/// <param name="changeEvent">
///   The event object which is to be set whenever the contents of the device 
///   list changes.
/// </param>
/// <exception cref="VciException">
///   Assigning the event failed.
/// </exception>
/// <exception cref="ArgumentNullException">
///   Parameter changeEvent was a null reference.
/// </exception>
/// <exception cref="ObjectDisposedException">
///   Object is already disposed.
/// </exception>
//*****************************************************************************
void VciDeviceList::AssignEvent(AutoResetEvent^ changeEvent)
{
  HRESULT hResult;
  
  if (nullptr == changeEvent)
  {
    throw gcnew ArgumentNullException();
  }

  if (nullptr != m_pEnuDev)
  {
    hResult = m_pEnuDev->AssignEvent((HANDLE) changeEvent->Handle);
    if (hResult != VCI_OK )
    {
      throw gcnew VciException(VciServerImpl::Instance(), hResult);
    }
  }
  else
  {
    throw gcnew ObjectDisposedException(this->GetType()->FullName);
  }
}

//*****************************************************************************
/// <summary>
///   This method assigns an event object to the list. The event is
///   set to the signaled state whenever the contents of the device list
///   changes.
/// </summary>
/// <param name="changeEvent">
///   The event object which is to be set whenever the contents of the device 
///   list changes.
/// </param>
/// <exception cref="VciException">
///   Assigning the event failed.
/// </exception>
/// <exception cref="ArgumentNullException">
///   Parameter changeEvent was a null reference.
/// </exception>
/// <exception cref="ObjectDisposedException">
///   Object is already disposed.
/// </exception>
//*****************************************************************************
void VciDeviceList::AssignEvent(ManualResetEvent^ changeEvent)
{
  HRESULT hResult;
  
  if (nullptr == changeEvent)
  {
    throw gcnew ArgumentNullException();
  }

  if (nullptr != m_pEnuDev)
  {
    hResult = m_pEnuDev->AssignEvent((HANDLE) changeEvent->Handle);
    if (hResult != VCI_OK )
    {
      throw gcnew VciException(VciServerImpl::Instance(), hResult);
    }
  }
  else
  {
    throw gcnew ObjectDisposedException(this->GetType()->FullName);
  }
}

//*****************************************************************************
/// <summary>
///   This method returns an enumerator that can iterate through the
///   list of installed VCI devices.
/// </summary>
/// <returns>
///   A reference to the IEnumerator interface which can be used to iterate 
///   through the list of installed device.
/// </returns>
/// <exception cref="ObjectDisposedException">
///   Object is already disposed.
/// </exception>
//*****************************************************************************
Collections::IEnumerator^ VciDeviceList::GetEnumerator()
{
  HRESULT           hResult;
  ::IVciEnumDevice* pNewEnu;
  Collections::IEnumerator^      pResult = nullptr;

  if (nullptr != m_pDevMan)
  {
    // To support providing several separate enumerator instances 
    // we have to get a new native enumerator here !!
    hResult = m_pDevMan->EnumDevices(&pNewEnu);
    if (VCI_OK == hResult)
    {
      pResult = gcnew VciDeviceEnumerator(pNewEnu);
    }
    else
    {
      throw gcnew VciException(VciServerImpl::Instance(), hResult);
    }
  }
  else
  {
    throw gcnew ObjectDisposedException(this->GetType()->FullName);
  }

  return( pResult );
}


/*##########################################################################*/
/*### Methods for VciDeviceEnumerator class                              ###*/
/*##########################################################################*/

//*****************************************************************************
/// <summary>
///   Constructor for new VCI device enumerators.
/// </summary>
/// <param name="pEnuDev">
///   Pointer to the native device enumerator interface
///</param>
//*****************************************************************************
VciDeviceEnumerator::VciDeviceEnumerator(::IVciEnumDevice* pEnuDev)
{
  m_pEnuDev = pEnuDev;
  m_pCurDev = nullptr;

  if (nullptr != m_pEnuDev)
  {
    m_pEnuDev->AddRef();
  }
  else
  {
    throw gcnew ArgumentNullException();
  }
}

//*****************************************************************************
/// <summary>
///   Destructor for VCI device enumerators.
/// </summary>
/// <remarks>
///   The compiler generates a IDisposable.Dispose() of the destructor.
/// </remarks>
//*****************************************************************************
VciDeviceEnumerator::~VciDeviceEnumerator()
{
  m_pCurDev = nullptr;
  
  if (nullptr != m_pEnuDev)
  {
    m_pEnuDev->Release();
    m_pEnuDev = nullptr;
  }
}

//*****************************************************************************
/// <summary>
///   Gets the current VCI device in the list.
/// </summary>
/// <returns>
///   A Reference to the current VCI device in the list.
/// </returns>
/// <remarks>
///   After an enumerator is created or after the <c>Reset</c> method is called, 
///   the <c>MoveNext</c> method must be called to advance the enumerator to the 
///   first element of the list before reading the value of the 
///   <c>Current</c> property; otherwise, <c>Current</c> is undefined.
///
///   <c>Current</c> also throws an exception if the last call to 
///   <c>MoveNext</c> returned false, which indicates the end of the list.
///
///   <c>Current</c> does not move the position of the enumerator, and 
///   consecutive calls to <c>Current</c> return the same object until either 
///   <c>MoveNext</c> or <c>Reset</c> is called.
/// </remarks>
/// <exception cref="InvalidOperationException">
///   The enumerator is positioned before the first element of the collection 
///   or after the last element. 
/// </exception>
/// <exception cref="ObjectDisposedException">
///   Object is already disposed.
/// </exception>
//*****************************************************************************
Object^ VciDeviceEnumerator::Current::get(void)
{
  if (nullptr == m_pEnuDev)
  {
    throw gcnew ObjectDisposedException(this->GetType()->FullName);
  }

  if (nullptr == m_pCurDev)
  {
    throw gcnew InvalidOperationException("Enumeration has not started. Call MoveNext.");
  }
  return( m_pCurDev );
}

//*****************************************************************************
/// <summary>
///   This method advances the enumerator to the next device of the
///   collection.
/// </summary>
/// <returns>
///   The method returns true if the enumerator was successfully advanced to
///   the next device. The method returns false if the enumerator has passed
///   the end of the list.
/// </returns>
/// <remarks>
///   After an enumerator is created or after a call to Reset, an enumerator is
///   positioned before the first element of the list, and the first call
///   to MoveNext moves the enumerator over the first element of the list.
///   After the end of the list is passed, subsequent calls to MoveNext
///   return false until Reset is called.
///   An enumerator remains valid as long as the list remains unchanged.
///   If changes are made to the list, such as adding, modifying or
///   deleting elements, the enumerator is irrecoverably invalidated and the
///   next call to MoveNext or Reset throws an InvalidOperationException.
///   
///   No guarantee exists that each iteration through the list of VCI devices
///   enumerates the same set of VCI devices or enumerates the VCI devices in
///   the same order.
/// </remarks>
/// <exception cref="ObjectDisposedException">
///   Object is already disposed.
/// </exception>
//*****************************************************************************
bool VciDeviceEnumerator::MoveNext()
{
  VCIDEVICEINFO DevInfo;

  m_pCurDev = nullptr;
  
  if (nullptr != m_pEnuDev)
  {
    if (m_pEnuDev->Next(1, &DevInfo, nullptr) == VCI_OK)
    {
      m_pCurDev = gcnew VciDevice(DevInfo);
    }
  }
  else
  {
    throw gcnew ObjectDisposedException(this->GetType()->FullName);
  }
  
  return( nullptr != m_pCurDev );
}

//*****************************************************************************
/// <summary>
///   This method sets the enumerator to its initial position, which is
///   before the first element in the collection.
/// </summary>
/// <remarks>
///   No guarantee exists that each iteration through the list of VCI devices
///   enumerates the same set of VCI devices or enumerates the VCI devices in
///   the same order.
/// </remarks>
/// <exception cref="ObjectDisposedException">
///   Object is already disposed.
/// </exception>
//*****************************************************************************
void VciDeviceEnumerator::Reset()
{
  m_pCurDev = nullptr;
  
  if (nullptr != m_pEnuDev)
  {
    m_pEnuDev->Reset();
  }
  else
  {
    throw gcnew ObjectDisposedException(this->GetType()->FullName);
  }
}
