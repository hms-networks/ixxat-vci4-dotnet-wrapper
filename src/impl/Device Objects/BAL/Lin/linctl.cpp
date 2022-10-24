/*****************************************************************************
 IXXAT Automation GmbH
******************************************************************************
 Summary : Implementation of the LIN control class.
 Compiler: Microsoft VC++ 8
******************************************************************************
 all rights reserved
*****************************************************************************/

/*************************************************************************
** include files
*************************************************************************/
#include "linctl.hpp"
#include "vcinet.hpp"


/*************************************************************************
** used namespaces
*************************************************************************/
using namespace Ixxat::Vci4::Bal::Lin;


//*****************************************************************************
/// <summary>
///   Constructor for VCI LIN control objects.
/// </summary>
/// <param name="pBalObj">
///   Pointer to the native BAL object interface. 
///   This parameter must not be NULL.
/// </param>
/// <param name="portNumber">
///   Port number of the bus socket to open.
/// </param>
/// <param name="busTypeIndex">
///   Bus type related port number
///</param>
/// <exception cref="VciException">
///   Creation of LIN control socket failed.
/// </exception>
/// <exception cref="ArgumentNullException">
///   Native IBalObject was a null pointer.
/// </exception>
//*****************************************************************************
LinControl::LinControl(::IBalObject*  pBalObj
                      , Byte          portNumber
                      , Byte          busTypeIndex)
          : LinSocket(pBalObj, portNumber, busTypeIndex)
{
  HRESULT         hResult;
  ::ILinControl*  pLinCtl;

  m_pLinCtl = nullptr;

  if (nullptr != pBalObj)
  {
    hResult = pBalObj->OpenSocket( portNumber
                                 , IID_ILinControl
                                 , (PVOID*) &pLinCtl);

    if (hResult == VCI_OK)
    {
      hResult = InitNew(pLinCtl);
      pLinCtl->Release();
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
///   Destructor for VCI CAN control objects.
/// </summary>
//*****************************************************************************
LinControl::~LinControl()
{
  Cleanup();
}

//*****************************************************************************
/// <summary>
///   This method initializes a newly created LIN control object.
/// </summary>
/// <param name="pLinCtl">
///   Pointer to the native LIN control object.
///   This parameter must not be NULL.
/// </param>
/// <returns>
///   VCI_OK if succeeded, VCI_E_INVALIDARG otherwise.
/// </returns>
//*****************************************************************************
HRESULT LinControl::InitNew(::ILinControl* pLinCtl)
{
  HRESULT hResult;

  Cleanup();

  if (nullptr != pLinCtl)
  {
    m_pLinCtl = pLinCtl;
    m_pLinCtl->AddRef();
    hResult = VCI_OK;
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
void LinControl::Cleanup(void)
{
  if (nullptr != m_pLinCtl)
  {
    m_pLinCtl->Release();
    m_pLinCtl = nullptr;
  }
}

//*****************************************************************************
/// <summary>
///   This method initializes the LIN line in the specified operating mode
///   and bit transfer rate. The method also performs a reset of the LIN
///   controller hardware.
/// </summary>
/// <param name="initLine">
///   Specifies the operating mode and bit transfer rate 
/// </param>
/// <exception cref="VciException">
///   LIN line initialization failed.
/// </exception>
/// <exception cref="ObjectDisposedException">
///   Object is already disposed.
/// </exception>
//*****************************************************************************
void LinControl::InitLine( LinInitLine initLine )
{
  HRESULT     hResult;
  LININITLINE sInit;

  if (nullptr != m_pLinCtl)
  {
    sInit.bOpMode = (UINT8)initLine.OperatingMode;
    sInit.wBitrate= initLine.Bitrate.AsUInt16;

    hResult = m_pLinCtl->InitLine(&sInit);
    if (hResult != VCI_OK)
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
///   This method resets the LIN line to it's initial state. The method
///   aborts a currently busy transmit message and switches the LIN controller
///   into init mode.
/// </summary>
/// <exception cref="VciException">
///   Resetting LIN line failed.
/// </exception>
/// <exception cref="ObjectDisposedException">
///   Object is already disposed.
/// </exception>
//*****************************************************************************
void LinControl::ResetLine(void)
{
  HRESULT hResult;

  if (nullptr != m_pLinCtl)
  {
    hResult = m_pLinCtl->ResetLine();
    if (hResult != VCI_OK)
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
///   This method starts the LIN line and switch it into running mode.
///   After starting the LIN line.
/// </summary>
/// <exception cref="VciException">
///   Starting LIN line failed.  
/// </exception>
/// <exception cref="ObjectDisposedException">
///   Object is already disposed. 
/// </exception>
//*****************************************************************************
void LinControl::StartLine(void)
{
  HRESULT hResult;

  if (nullptr != m_pLinCtl)
  {
    hResult = m_pLinCtl->StartLine();
    if (hResult != VCI_OK)
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
///   This method stops the LIN line an switches it into init mode. After
///   stopping the LIN controller no further LIN messages are transmitted.
///   Other than <c>ResetLine</c>, this method does not abort a currently 
///   busy transmit message.
/// </summary>
/// <exception cref="VciException">
///   Stopping LIN line failed.
/// </exception>
/// <exception cref="ObjectDisposedException">
///   Object is already disposed.  
/// </exception>
//*****************************************************************************
void LinControl::StopLine(void)
{
  HRESULT hResult;

  if (nullptr != m_pLinCtl)
  {
    hResult = m_pLinCtl->StopLine();
    if (hResult != VCI_OK)
    {
      throw gcnew VciException(VciServerImpl::Instance(), hResult);
    }
  }
  else
  {
    throw gcnew ObjectDisposedException(this->GetType()->FullName);
  }
}

mgdLINMSG ConvertToLINMSG(ILinMessage^ message)
{
  mgdLINMSG local;
  LinMessage^ castmsg = dynamic_cast<LinMessage^> (message);
  if (castmsg)
  {
    local = castmsg->ToValue();
  }
  else
  {
    throw gcnew ArgumentException("Parameter must be a Lin message", "message");
  }

  return local;
}

//*****************************************************************************
/// <summary>
///   This function either transmits the specified message directly to the LIN 
///   bus connected to the controller or enters the message in the response 
///   table of the controller.
/// </summary>
/// <param name="send">
///   true to force sending the message directly or false to enter the message
///   into the controller's response table.
/// </param>
/// <param name="message">
///   The message to be transmitted.
/// </param>
/// <exception cref="VciException">
///   Writing the message failed.
/// </exception>
/// <exception cref="ObjectDisposedException">
///   Object is already disposed.
/// </exception>
//*****************************************************************************
void LinControl::WriteMessage(bool         send, 
                              ILinMessage^ message)
{
  HRESULT hResult;

  if (nullptr != m_pLinCtl)
  {
    mgdLINMSG msg = ConvertToLINMSG(message);

    pin_ptr<mgdLINMSG> pMsg = &msg;
    hResult = m_pLinCtl->WriteMessage(send, (PLINMSG)pMsg);
    if (hResult != VCI_OK)
    {
      throw gcnew VciException(VciServerImpl::Instance(), hResult);
    }
  }
  else
  {
    throw gcnew ObjectDisposedException(this->GetType()->FullName);
  }
}
