/*****************************************************************************
 IXXAT Automation GmbH
******************************************************************************
 Summary : Declarations for the Vci controller info value class.
 Compiler: Microsoft VC++ 8
******************************************************************************
 all rights reserved
*****************************************************************************/

#pragma once

/*************************************************************************
** include files
*************************************************************************/
#include <vcisdk.h>

#include ".\Bal\Can\cansoc.hpp"

/*************************************************************************
** namespace Ixxat.Vci4
*************************************************************************/
namespace Ixxat {
  namespace Vci4 {


/*************************************************************************
** used namespaces
*************************************************************************/
using namespace Ixxat::Vci4::Bal::Can;


//*****************************************************************************
/// <summary>
///   This struct contains the bus type and controller type of a device's
///   fieldbus controller. An array of such structs can be obtained from
///   property <c>IVciDevice.Equipment</c>.
/// </summary>
//*****************************************************************************
public ref class VciCtrlInfo : public IVciCtrlInfo
{
  //--------------------------------------------------------------------
  // member variables
  //--------------------------------------------------------------------
  private:
    VciBusType m_BusType;     // The bus type
    Object^    m_CtrlType;    // The boxed controller type

  //--------------------------------------------------------------------
  // Properties
  //--------------------------------------------------------------------
  public:
    //*****************************************************************************
    /// <summary>
    ///   Get's the type of the supported fieldbus.
    /// </summary>
    //*****************************************************************************
    virtual property VciBusType BusType 
    {
      VciBusType get()
      {
        return m_BusType;
      }
    };

    //*****************************************************************************
    /// <summary>
    ///   Get's the type of the fieldbus controller. Because the actual data type 
    ///   property depends in the bus type, the retrieved value is boxed into an
    ///   object reference. The calling method has to cast it to the appropriate
    ///   data type. For a CAN bus controller (<c>BusType</c> = 
    ///   <c>VciBusType.Can</c>) the actual data type of property 
    ///   <c>ControllerType</c> is <c>CanCtrlType</c>.
    ///   This property can be a null reference if the <c>BusType</c> is 
    ///   unknown.
    /// </summary>
    //*****************************************************************************
    virtual property Object^ ControllerType
    {
      Object^ get()
      {
        return m_CtrlType;
      }
    };


  //--------------------------------------------------------------------
  // Member functions
  //--------------------------------------------------------------------
  internal:
    //*****************************************************************************
    /// <summary>
    ///   ctor - Initializes the controller info value object
    /// </summary>
    /// <param name="wBusCtrlType">
    ///   Value that encapsulates the bus and the controller type
    ///   (see native macros VCI_BUS_TYPE and VCI_CTL_TYPE)
    /// </param>
    //*****************************************************************************
    VciCtrlInfo(UInt16 wBusCtrlType)
    {
      Initialize(wBusCtrlType);
    }

    //*****************************************************************************
    /// <summary>
    ///   Initializes the controller info value object
    /// </summary>
    /// <param name="wBusCtrlType">
    ///   Value that encapsulates the bus and the controller type
    ///   (see native macros VCI_BUS_TYPE and VCI_CTL_TYPE)
    /// </param>
    //*****************************************************************************
    void Initialize(UInt16 wBusCtrlType)
    {
      m_BusType  = VciBusType::Unknown;
      m_CtrlType = nullptr;

      // To avoid later occurance of exceptions we have to ensure the
      // validity of the bus and controller types against the managed
      // enum constants.

      //
      // Validate bus type
      // 
      Array^ aBusEnumValues = Enum::GetValues(VciBusType::typeid);
      if (nullptr != aBusEnumValues)
      {
        for (int idx = 0; idx < aBusEnumValues->Length; idx++)
        {
          int iEnumValue = (int)aBusEnumValues->GetValue(idx);
          if (VCI_BUS_TYPE(wBusCtrlType) == iEnumValue)
          {
            m_BusType = (VciBusType) VCI_BUS_TYPE(wBusCtrlType);
            break;
          }
        }
      }

      //
      // Validate controller type (only if bus type is valid)
      // 
      Array^ aCtrlEnumValues = nullptr;
      switch(m_BusType)
      {
      case VciBusType::Can: aCtrlEnumValues = Enum::GetValues(CanCtrlType::typeid); break;
      case VciBusType::FlexRay: break;
      case VciBusType::Lin: break;
      }
      
      if (nullptr != aCtrlEnumValues)
      {
        for (int idx = 0; idx < aCtrlEnumValues->Length; idx++)
        {
          Object^ rEnumValue = aCtrlEnumValues->GetValue(idx);
          if (VCI_CTL_TYPE(wBusCtrlType) == (int)rEnumValue)
          {
            m_CtrlType = rEnumValue;
            break;
          }
        }
      }
    }
  public:
    //*****************************************************************************
    /// <summary>
    ///   Determines whether the specified Object is equal to the current Object.
    /// </summary>
    /// <pararm name ="obj">
    ///   The Object to compare with the current Object.
    /// </pararm>
    /// <returns>
    ///   true if the specified Object is equal to the current Object; 
    ///   otherwise, false.
    /// </returns>
    //*****************************************************************************
    virtual bool Equals(Object^ obj) override
    {
      // Check for null values and compare run-time types.
      if (obj == nullptr || GetType() != obj->GetType()) 
        return false;

      VciCtrlInfo^ p = (VciCtrlInfo^)obj;

      return  ( VCI_BUS_CTRL((int)m_BusType, (int)p->m_CtrlType) == 
                VCI_BUS_CTRL((int)m_BusType, (int)p->m_CtrlType));
    }

    //*****************************************************************************
    /// <summary>
    ///   Serves as a hash function for a particular type. GetHashCode is suitable 
    ///   for use in hashing algorithms and data structures like a hash table.
    /// </summary>
    /// <returns>
    ///   A hash code for the current Object. 
    /// </returns>
    //*****************************************************************************
    virtual int GetHashCode () override
    {
      return VCI_BUS_CTRL((int)m_BusType, (int)m_CtrlType);
    }

};




} // end of namespace Vci4
} // end of namespace Ixxat
