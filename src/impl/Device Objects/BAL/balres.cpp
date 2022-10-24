/*****************************************************************************
 IXXAT Automation GmbH
******************************************************************************
 Summary : Implementation for the BAL resource descriptor class.
 Compiler: Microsoft VC++ 8
******************************************************************************
 all rights reserved
*****************************************************************************/

#include "balres.hpp"


using namespace Ixxat::Vci4;
using namespace Ixxat::Vci4::Bal;


//*****************************************************************************
/// <summary>
///   Constructor for BAL resource objects.
/// </summary>
/// <param name="portNumber">
///   Port number of the BAL resource.
///</param>
/// <param name="fieldBusType">
///   Type of the field bus (see <c>VciBusType</c> enumeration)
///</param>
/// <param name="busTypeIndex">
///   Bus type related port number
///</param>
//*****************************************************************************
BalResource::BalResource(Byte portNumber, VciBusType fieldBusType, Byte busTypeIndex)
{
  m_bPortNo       = portNumber;
  m_BusType       = fieldBusType;
  m_bBusTypeIndex = busTypeIndex;
}

//*****************************************************************************
/// <summary>
///   Destructor for BAL socket objects.
/// </summary>
/// <remarks>
///   Do not remove this empty destructor.
///   It forces providing the IDisposable interface for conformance with
///   all other VCI interfaces !
/// </remarks>
//*****************************************************************************
BalResource::~BalResource()
{
}

//*****************************************************************************
/// <summary>
///   Gets the bus type related port number
/// </summary>
/// <returns>
///   The bus type related port number
/// </returns>
//*****************************************************************************
Byte BalResource::BusTypeIndex::get()
{
  return m_bBusTypeIndex;
}

//*****************************************************************************
/// <summary>
///   Gets the port number of the BAL bus socket.
/// </summary>
/// <returns>
///   The port number of the BAL bus socket.
/// </returns>
//*****************************************************************************
Byte BalResource::BusPort::get()
{
  return( m_bPortNo );
}

//*****************************************************************************
/// <summary>
///   Gets the bus type of the BAL bus socket.
/// </summary>
/// <returns>
///   The bus type of the BAL bus socket.
/// </returns>
//*****************************************************************************
VciBusType BalResource::BusType::get()
{
  return( m_BusType );
}

//*****************************************************************************
/// <summary>
///   Gets the name of the bus.
/// </summary>
/// <returns>
///   If succeeded the name of the bus.
///   If failed a null reference (Nothing in Visual Basic).
/// </returns>
//*****************************************************************************
String^ BalResource::BusName::get()
{
  String^ pszBus;
  String^ pszName;

  pszBus = (m_bBusTypeIndex + 1).ToString();

  switch (m_BusType)
  {
    case VciBusType::Can:
     pszName = String::Concat("CAN-", pszBus);
     break;

    case VciBusType::FlexRay:
     pszName = String::Concat("FlexRay-", pszBus);
     break;

    case VciBusType::Lin:
      pszName = String::Concat("LIN-", pszBus);
      break;

    default:
     pszName = String::Concat("???-", pszBus);
     break;
  }

  return( pszName );
}

//*****************************************************************************
/// <summary>
///   Returns a String that represents the current Object.
/// </summary>
/// <returns>
///   A String that represents the current Object.
/// </returns>
//*****************************************************************************
String^ BalResource::ToString ()
{
  return BusName;
}


