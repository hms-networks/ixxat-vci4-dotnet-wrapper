/*****************************************************************************
 IXXAT Automation GmbH
******************************************************************************
 Summary : Application programming interface for the IXXAT VCI library.
 Compiler: Microsoft VC++ 8
******************************************************************************
 all rights reserved
*****************************************************************************/


/*************************************************************************
** used namespaces
*************************************************************************/
using System;
using System.Runtime.Serialization;
using System.Runtime.InteropServices;

/*************************************************************************
** data types
*************************************************************************/

namespace Ixxat.Vci4 
{

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