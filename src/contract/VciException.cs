// SPDX-License-Identifier: MIT
//----------------------------------------------------------------------------
// Summary  : Application programming interface for the IXXAT VCI library.
// Copyright: Copyright (C) 2016-2022 HMS Technology Center Ravensburg GmbH, 
//            all rights reserved
//----------------------------------------------------------------------------

namespace Ixxat.Vci4 
{
  using System;
  using System.Runtime.Serialization;
  using System.Runtime.InteropServices;

  //*****************************************************************************
  /// <summary>
  ///   This class implements the basic VCI exception object.
  /// </summary>
  //*****************************************************************************
  [Serializable]
  public class VciException : Exception
  {
    //--------------------------------------------------------------------
    // properties
    //--------------------------------------------------------------------

    //*****************************************************************************
    /// <summary>
    /// create a VCI exception
    /// </summary>
    /// <param name="server">Server instance to get error string from</param>
    //*****************************************************************************
    public VciException(IVciServer server)
      : base(server.GetErrorMsg(Marshal.GetHRForLastWin32Error()))
    {
      this.HResult = Marshal.GetHRForLastWin32Error();
    }

    //*****************************************************************************
    /// <summary>
    /// create a VCI exception
    /// </summary>
    /// <param name="server">Server instance to get error string from</param>
    /// <param name="errorCode">Error code</param>
    //*****************************************************************************
    public VciException(IVciServer server, int errorCode)
      : base(server.GetErrorMsg(errorCode))
    {
      this.HResult = errorCode;
    }

    //*****************************************************************************
    /// <summary>
    /// create a VCI exception
    /// </summary>
    /// <param name="message">Error message</param>
    //*****************************************************************************
    public VciException(string message)        
      : base(message)
    {
      // this.HResult = 0;
    }

    //*****************************************************************************
    /// <summary>
    /// create a VCI exception
    /// </summary>
    /// <param name="server">Server instance to get error string from</param>
    /// <param name="msgappend">Message to append</param>
    /// <param name="errorCode">Error code</param>
    //*****************************************************************************
    public VciException(IVciServer server
                       , string msgappend 
                       , int    errorCode)
      : base(server.GetErrorMsg(errorCode) + msgappend)
    {
      this.HResult = errorCode;
    }

    //*****************************************************************************
    /// <summary>
    /// create a VCI exception
    /// </summary>
    /// <param name="message">Error message</param>
    /// <param name="innerException">Inner exception</param>
    //*****************************************************************************
    public VciException( string message
                       , Exception  innerException) 
      : base(message, innerException)
    {
    }
  };


}