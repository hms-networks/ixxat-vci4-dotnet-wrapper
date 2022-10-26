// SPDX-License-Identifier: MIT
//----------------------------------------------------------------------------
// Summary  : Application programming interface for the IXXAT VCI library.
// Copyright: Copyright (C) 2016-2022 HMS Technology Center Ravensburg GmbH, 
//            all rights reserved
//----------------------------------------------------------------------------

namespace Ixxat.Vci4 
{ 
  //*****************************************************************************
  /// <summary>
  ///   Enumeration of VCI bus types
  /// </summary>
  //*****************************************************************************
  public enum VciBusType
  {
    /// <summary>
    ///   reserved / unknown
    /// </summary>
    Unknown = 0,  
    /// <summary>
    ///   CAN bus
    /// </summary>
    Can     = 1,  
    /// <summary>
    ///   FlexRay bus
    /// </summary>
    FlexRay = 4,
    /// <summary>
    ///   LIN bus
    /// </summary>
    Lin     = 2
  };
   

}