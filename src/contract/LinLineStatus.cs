/*****************************************************************************
 IXXAT Automation GmbH
******************************************************************************
 Summary : Declarations for the LIN line status value class.
 Compiler: Microsoft VC++ 8
******************************************************************************
 all rights reserved
*****************************************************************************/

namespace Ixxat.Vci4.Bal.Lin 
{

  using System;
  using System.Runtime.InteropServices;

  //*****************************************************************************
  /// <summary>
  ///   Enumeration of flag values that are used to specify or signalize the 
  ///   operating mode of a LIN controller (see <c>LinLineStatus</c>).
  /// </summary>
  //*****************************************************************************
  [Flags]
  public enum LinOperatingModes : int
  {
    /// <summary>
    ///   Indicates the LIN slave mode
    /// </summary>
    Slave  = 0x00,
    /// <summary>
    ///   Indicates the LIN master mode
    /// </summary>
    Master  = 0x01,
    /// <summary>
    ///   Enables reception of error frames
    /// </summary>
    Errors  = 0x02
  };


  //*****************************************************************************
  /// <summary>
  ///   Enumeration of flag values that are used to signalize the status mode of
  ///   a LIN controller (see <c>LinLineStatus</c>).
  /// </summary>
  //*****************************************************************************
  [Flags]
  public enum LinCtrlStatus : long
  {
    /// <summary>
    ///   Data overrun occurred
    /// </summary>
    Overrun   = 0x01,
    /// <summary>
    ///   Init mode active
    /// </summary>
    InInit    = 0x10
  };


  //*****************************************************************************
  /// <summary>
  ///   <c>LinLineStatus</c> is used to signalize the status of a LIN 
  ///   controller.
  /// </summary>
  //*****************************************************************************
  public struct LinLineStatus
  {
    private LinOperatingModes bOpMode;          // current operating mode
    private byte bBusLoad;                      // average bus load in percent (0..100)
    private LinCtrlStatus status;               // status of the controller (see LinCtrlStatus)
    private uint wBitrate;                      // bitrate
    
    //*****************************************************************************
    /// <summary>
    ///   Ctor - create a LinLineStatus object
    /// </summary>
    /// <param name="opmode">operating mode</param>
    /// <param name="busload">bus load</param>
    /// <param name="ctrlstat">controller status</param>
    /// <param name="bitrate">bitrate</param>
    //*****************************************************************************
    public LinLineStatus(LinOperatingModes opmode, byte busload, LinCtrlStatus ctrlstat, uint bitrate)
    {
      bOpMode = opmode;
      bBusLoad = busload;
      status = ctrlstat;
      wBitrate = bitrate;
    }

    //*****************************************************************************
    /// <summary>
    ///   This method returns a String that represents the current timing value.
    /// </summary>
    /// <returns>
    ///   A String that represents the current bit timing value.
    /// </returns>
    //*****************************************************************************
    public override string ToString()
    {
      return String.Format("opmode: {0}, busload: {1}, ctrlstat: {2}, bitrate: {3}", bOpMode, bBusLoad, status, wBitrate);
    }

    //*****************************************************************************
    /// <summary>
    ///   Gets the current operating mode of the LIN controller.
    /// </summary>
    /// <returns>
    ///   The current operating mode of the LIN controller.
    /// </returns>
    //*****************************************************************************
    public LinOperatingModes OperatingMode
    {
      get
      {
        return (bOpMode);
      }
    }

    //*****************************************************************************
    /// <summary>
    ///   Gets a value indicating whether the LIN controller is currently
    ///   operating in slave mode.
    /// </summary>
    /// <returns>
    ///   true if the LIN controller is currently operating in slave 
    ///   mode, otherwise false.
    /// </returns>
    //*****************************************************************************
    public bool IsInSlaveMode
    {
      get
      {
        return( (bOpMode & LinOperatingModes.Master) == 0 );
      }
    }

    //*****************************************************************************
    /// <summary>
    ///   Gets a value indicating whether the LIN controller is currently
    ///   operating in master mode.
    /// </summary>
    /// <returns>
    ///   true if the LIN controller is currently operating in master 
    ///   mode, otherwise false.
    /// </returns>
    //*****************************************************************************
    public bool IsInMasterMode
    {
      get
      {
        return ((bOpMode & LinOperatingModes.Master) != 0);
      }
    }

    //*****************************************************************************
    /// <summary>
    ///   Gets a value indicating if the reception of error frames is currently 
    ///   enabled.
    /// </summary>
    /// <returns>
    ///   true if the reception of error frames is currently enabled,
    ///   otherwise false.
    /// </returns>
    //*****************************************************************************
    public bool IsErrModeEnabled
    {
      get
      {
        return ((bOpMode & LinOperatingModes.Errors) != 0);
      }
    }

    //*****************************************************************************
    /// <summary>
    ///   Gets the current LIN controller status.
    /// </summary>
    /// <returns>
    ///   Current LIN controller status.
    /// </returns>
    //*****************************************************************************
    public LinCtrlStatus ControllerStatus
    {
      get
      {
        return (status);
      }
    }

    //*****************************************************************************
    /// <summary>
    ///   Gets a value indicating if a message was lost because there was
    ///   not enough free space for the message in the LIN controllers
    ///   internal message buffer.
    /// </summary>
    /// <returns>
    ///   true if a data overrun has occured, otherwise false.
    /// </returns>
    //*****************************************************************************
    public bool HasDataOverrun
    {
      get
      {
        return ((status & LinCtrlStatus.Overrun) == LinCtrlStatus.Overrun);
      }
    }

    //*****************************************************************************
    /// <summary>
    ///   Gets a value indicating whether the LIN controller is currently in init 
    ///   mode.
    /// </summary>
    /// <returns>
    ///   true if the LIN controller is in init mode, otherwise false.
    /// </returns>
    //*****************************************************************************
    public bool IsInInitMode
    {
      get
      {
        return ((status & LinCtrlStatus.InInit) == LinCtrlStatus.InInit);
      }
    }

    //*****************************************************************************
    /// <summary>
    ///   Gets the current bus load (0...100%).
    /// </summary>
    /// <returns>
    ///   Current bus load (0...100%).
    /// </returns>
    //*****************************************************************************
    public byte Busload
    {
      get
      {
        return (bBusLoad);
      }
    }

    //*****************************************************************************
    /// <summary>
    ///   Gets a value indicating whether the LIN controller is currently in init 
    ///   mode.
    /// </summary>
    /// <returns>
    ///   true if the LIN controller is in init mode, otherwise false.
    /// </returns>
    //*****************************************************************************
    public uint Bitrate
    {
      get
      {
        return (wBitrate);
      }
    }

  };


}