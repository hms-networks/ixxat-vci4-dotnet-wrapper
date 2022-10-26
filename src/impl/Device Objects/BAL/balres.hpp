// SPDX-License-Identifier: MIT
//----------------------------------------------------------------------------
// Summary  : Declarations for the BAL resource descriptor class.
// Copyright: Copyright (C) 2016-2022 HMS Technology Center Ravensburg GmbH, 
//            all rights reserved
//----------------------------------------------------------------------------

#pragma once

#include <vcisdk.h>


namespace Ixxat {
  namespace Vci4 {
    namespace Bal {

using namespace System;

//*****************************************************************************
/// <summary>
///   This class implements a BAL resource descriptor object.
/// </summary>
//*****************************************************************************
private ref class BalResource : public IBalResource
{
  //--------------------------------------------------------------------
  // member variables
  //--------------------------------------------------------------------
  private:
    Byte       m_bPortNo;       // port number of the bus socket
    VciBusType m_BusType;       // bus type (CAN, FLX, etc.)
    Byte       m_bBusTypeIndex; // bus type related port number

  //--------------------------------------------------------------------
  // properties
  //--------------------------------------------------------------------
  internal:
    property Byte BusTypeIndex { Byte get(void); };

  //--------------------------------------------------------------------
  // member functions
  //--------------------------------------------------------------------
  internal:
    BalResource ( Byte        portNumber
                , VciBusType  fieldBusType
                , Byte        busTypeIndex);
   ~BalResource ( );

  public:
    virtual String^ ToString() override;
    //*****************************************************************************
    /// <summary>
    ///   Determines whether the specified Object instances are equal.
    /// </summary>
    /// <pararm name ="value1">
    ///   Value 1.
    /// </pararm>
    /// <pararm name ="value2">
    ///   Value 2.
    /// </pararm>
    /// <returns>
    ///   true if value1 equals value2; otherwise, false.
    /// </returns>
    //*****************************************************************************
    static bool operator == (BalResource^ value1, BalResource^ value2)
    {
      if(Object::ReferenceEquals(value1, nullptr) && Object::ReferenceEquals(value2, nullptr))
        return true;

      if(Object::ReferenceEquals(value1, nullptr) || Object::ReferenceEquals(value2, nullptr))
        return false;

      return  ( (value1->m_bPortNo == value2->m_bPortNo) &&
                (value1->m_BusType == value2->m_BusType) );
    }

    //*****************************************************************************
    /// <summary>
    ///   Determines whether the specified Object instances are not equal.
    /// </summary>
    /// <pararm name ="value1">
    ///   Value 1.
    /// </pararm>
    /// <pararm name ="value2">
    ///   Value 2.
    /// </pararm>
    /// <returns>
    ///   true if value1 not equals value2; otherwise, false.
    /// </returns>
    //*****************************************************************************
    static bool operator != (BalResource^ value1, BalResource^ value2)
    {
      return !(value1 == value2);
    }

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
      return obj == this;
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
      return ( (((int)m_BusType) << 8) + (int)m_bPortNo );
    }


  //--------------------------------------------------------------------
  // IVciDriver implementation
  //--------------------------------------------------------------------
  public:
    virtual property Byte       BusPort  { Byte        get(void); };
    virtual property VciBusType BusType  { VciBusType  get(void); };
    virtual property String^    BusName  { String^     get(void); };
};


} // end of namespace Bal
} // end of namespace Vci4
} // end of namespace Ixxat
