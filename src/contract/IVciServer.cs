/*****************************************************************************
 IXXAT Automation GmbH
******************************************************************************
 Summary : Declarations for the VCI server object.
 Compiler: Microsoft VC++ 8
******************************************************************************
 all rights reserved
*****************************************************************************/

namespace Ixxat.Vci4 
{
  using System;

  //*****************************************************************************
  /// <summary>
  ///   This class represents the entry point for working with the VCI. 
  ///   Use <c>GetDriverManager</c> to get access to the installed VCI drivers.
  ///   Use <c>GetDeviceManager</c> to get access to the installed VCI devices.
  /// </summary>
  //*****************************************************************************
  public interface IVciServer
  {
    //--------------------------------------------------------------------
    // properties
    //--------------------------------------------------------------------

    //*****************************************************************************
    /// <summary>
    ///   Gets the version of the VCI server.
    /// </summary>
    /// <returns>
    ///   The version of the VCI server.
    /// </returns>
    /// <exception cref="VciException">
    ///   Thrown if getting the version number failed.
    /// </exception>
    //*****************************************************************************
    System.Version Version   { get; }

    //*****************************************************************************
    /// <summary>
    ///   Get access to the DeviceManager instance.
    /// </summary>
    /// <returns>
    ///   Main interface of the DeviceManager instance.
    /// </returns>
    /// <exception cref="VciException">
    ///   Thrown if getting DeviceManager instance failed.
    /// </exception>
    //*****************************************************************************
    IVciDeviceManager DeviceManager { get; }

    //*****************************************************************************
    /// <summary>
    ///   Get access to the MsgFactory instance. 
    ///   Use this instance to create message objects for transmission.
    /// </summary>
    /// <returns>
    ///   Main interface of the MsgFactory instance.
    /// </returns>
    /// <exception cref="VciException">
    ///   Thrown if getting MsgFactory instance failed.
    /// </exception>
    //*****************************************************************************
    IMessageFactory MsgFactory { get; }

    //*****************************************************************************
    /// <summary>
    /// Get VCI error message
    /// </summary>
    /// <param name="errorCode">Error code</param>
    /// <returns>Error string</returns>
    //*****************************************************************************
    string GetErrorMsg(int errorCode);
  };


}
