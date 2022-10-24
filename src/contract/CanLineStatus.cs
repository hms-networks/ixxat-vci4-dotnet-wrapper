/*****************************************************************************
 IXXAT Automation GmbH
******************************************************************************
 Summary : Declarations for the CAN line status value class.
 Compiler: Microsoft VC++ 8
******************************************************************************
 all rights reserved
*****************************************************************************/

/*************************************************************************
** namespace Ixxat.Vci4.Bal.Can
*************************************************************************/
namespace Ixxat.Vci4.Bal.Can 
{

  /*************************************************************************
  ** used namespaces 
  *************************************************************************/
  using System;
  using System.Runtime.InteropServices;

  //*****************************************************************************
  /// <summary>
  ///   Enumeration of flag values that are used to specify or signalize the 
  ///   operating mode of a CAN controller (see <c>CanLineStatus</c>).
  /// </summary>
  //*****************************************************************************
  [Flags]
  public enum CanOperatingModes : byte
  {
    /// <summary>
    ///   Undefined
    /// </summary>
    Undefined = 0x00, 
    /// <summary>
    ///   Reception of 11-bit id messages
    /// </summary>
    Standard  = 0x01,  
    /// <summary>
    ///   Reception of 29-bit id messages
    /// </summary>
    Extended  = 0x02,
    /// <summary>
    ///   Mask to decide between Standard/Extended/Undefined mode
    /// </summary>
    ModeMask = Standard + Extended,
    /// <summary>
    ///   Enable reception of error frames
    /// </summary>
    ErrFrame  = 0x04,  
    /// <summary>
    ///   Listen only mode (TX passive)
    /// </summary>
    ListenOnly  = 0x08,  
    /// <summary>
    ///   Use low speed bus interface
    /// </summary>
    LowSpeed  = 0x10,  
    /// <summary>
    ///   autmatic bit rate detection
    /// </summary>
    AutoBaudrate  = 0x20, 
  };

  //*****************************************************************************
  /// <summary>
  ///   Enumeration of flag values that are used to specify or signalize the 
  ///   extended operating mode of a CAN controller (see <c>CanLineStatus2</c>).
  /// </summary>
  //*****************************************************************************
  [Flags]
  public enum CanExtendedOperatingModes : byte
  {
    /// <summary>
    ///   No extended operation
    /// </summary>
    Undefined = 0x00, 
    /// <summary>
    ///   Extended data length
    /// </summary>
    ExtendedDataLength = 0x01,  
    /// <summary>
    ///   Fast data bit rate
    /// </summary>
    FastDataRate  = 0x02, 
    /// <summary>
    ///   non-ISO conform CAN-FD frame
    /// </summary>
    NonIsoCanFd  = 0x04
  };

  //*****************************************************************************
  /// <summary>
  ///   Enumeration of flag values that are used to signalize the status mode of
  ///   a CAN controller (see <c>CanLineStatus</c>).
  /// </summary>
  //*****************************************************************************
  [Flags]
  public enum CanCtrlStatus : long
  {
    /// <summary>
    ///   Transmission pending
    /// </summary>
    TxPending = 0x00000001, 
    /// <summary>
    ///   Data overrun occurred
    /// </summary>
    Overrun   = 0x00000002,  
    /// <summary>
    ///   Error warning limit exceeded
    /// </summary>
    ErrLimit  = 0x00000004, 
    /// <summary>
    ///   Bus off status
    /// </summary>
    BusOff    = 0x00000008,  
    /// <summary>
    ///   Init mode active
    /// </summary>
    InInit    = 0x00000010,
    /// <summary>
    ///   Bus coupling error
    /// </summary>
    BusCErr  = 0x00000020   
  };


  //*****************************************************************************
  /// <summary>
  ///   <c>CanLineStatus</c> is used to signalize the status of a CAN 
  ///   controller. See interface <c>ICanSocket</c>.
  /// </summary>
  //*****************************************************************************
  public struct CanLineStatus
  {
    private CanOperatingModes bOpMode;  // current CAN operating mode
    private byte bBusLoad;              // average bus load in percent (0..100)
    private CanCtrlStatus status;       // status of the CAN controller (see CanCtrlStatus)
    private CanBitrate bitrate;         // current bitrate

    //*****************************************************************************
    /// <summary>
    ///   Ctor - create a CanLineStatus object
    /// </summary>
    /// <param name="opmode">operating mode</param>
    /// <param name="busload">bus load</param>
    /// <param name="ctrlstat">controller status</param>
    /// <param name="rate">bitrate</param>
    //*****************************************************************************
    public CanLineStatus(CanOperatingModes opmode, byte busload, CanCtrlStatus ctrlstat, CanBitrate rate)
    {
      bOpMode = opmode;
      bBusLoad = busload;
      status = ctrlstat;
      bitrate = rate;
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
      return String.Format("opmode: {0}, busload: {1}, ctrlstat: {2}, bitrate: {3}", bOpMode, bBusLoad, status, bitrate);
    }

    //*****************************************************************************
    /// <summary>
    ///   Gets the current operating mode of the CAN controller.
    /// </summary>
    /// <returns>
    ///   The current operating mode of the CAN controller.
    /// </returns>
    //*****************************************************************************
    public CanOperatingModes OperatingMode
    {
      get
      {
        return (bOpMode);
      }
    }

    //*****************************************************************************
    /// <summary>
    ///   Gets a value indicating whether the CAN controller is currently
    ///   operating in standard (11-bit) frame mode.
    /// </summary>
    /// <returns>
    ///   true if the CAN controller is currently operating in standard frame 
    ///   mode, otherwise false.
    /// </returns>
    //*****************************************************************************
    public bool IsModeUndefined
    {
      get
      {
        return ((bOpMode & CanOperatingModes.ModeMask) == CanOperatingModes.Undefined);
      }
    }

    //*****************************************************************************
    /// <summary>
    ///   Gets a value indicating whether the CAN controller is currently
    ///   operating in standard (11-bit) frame mode.
    /// </summary>
    /// <returns>
    ///   true if the CAN controller is currently operating in standard frame 
    ///   mode, otherwise false.
    /// </returns>
    //*****************************************************************************
    public bool IsModeStandard
    {
      get
      {
        return ((bOpMode & CanOperatingModes.Standard) == CanOperatingModes.Standard);
      }
    }

    //*****************************************************************************
    /// <summary>
    ///   Gets a value indicating whether the CAN controller is currently
    ///   operating in extended (29-bit) frame mode.
    /// </summary>
    /// <returns>
    ///   true if the CAN controller is currently operating in extended frame 
    ///   mode, otherwise false.
    /// </returns>
    //*****************************************************************************
    public bool IsModeExtended
    {
      get
      {
        return ((bOpMode & CanOperatingModes.Extended) == CanOperatingModes.Extended);
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
    public bool IsErrorFramesEnabled
    {
      get
      {
        return ((bOpMode & CanOperatingModes.ErrFrame) == CanOperatingModes.ErrFrame);
      }
    }

    //*****************************************************************************
    /// <summary>
    ///  Gets a value indicating whether the CAN controller is currently
    ///  operating in listen only mode.
    /// </summary>
    /// <returns>
    ///   true if the CAN controller is currently operating in listen only mode, 
    ///   otherwise false.
    /// </returns>
    //*****************************************************************************
    public bool IsListenOnly
    {
      get
      {
        return ((bOpMode & CanOperatingModes.ListenOnly) == CanOperatingModes.ListenOnly);
      }
    }

    //*****************************************************************************
    /// <summary>
    ///  Gets a value indicating whether the CAN controller uses the
    ///  low speed bus interface.
    /// </summary>
    /// <returns>
    ///   true if the CAN controller uses the low speed bus interface, 
    ///   otherwise false.
    /// </returns>
    //*****************************************************************************
    public bool IsLowSpeedEnabled
    {
      get
      {
        return ((bOpMode & CanOperatingModes.LowSpeed) == CanOperatingModes.LowSpeed);
      }
    }

    //*****************************************************************************
    /// <summary>
    ///  Gets a value indicating whether the CAN controller uses the
    ///  automatic bitrate detection.
    /// </summary>
    /// <returns>
    ///   true if the CAN controller uses the automatic bitrate detection, 
    ///   otherwise false.
    /// </returns>
    //*****************************************************************************
    public bool IsAutomaticBitrateDetectionEnabled
    {
      get
      {
        return ((bOpMode & CanOperatingModes.AutoBaudrate) == CanOperatingModes.AutoBaudrate);
      }
    }

    //*****************************************************************************
    /// <summary>
    ///  Gets the current bit timing value of the CAN controller.
    /// </summary>
    /// <returns>
    ///   Current CAN bit timing.
    /// </returns>
    //*****************************************************************************
    public CanBitrate Bitrate
    {
      get
      {
        return (bitrate);
      }
    }

    //*****************************************************************************
    /// <summary>
    ///   Gets the current CAN controller status.
    /// </summary>
    /// <returns>
    ///   Current CAN controller status.
    /// </returns>
    //*****************************************************************************
    public CanCtrlStatus ControllerStatus
    {
      get
      {
        return (status);
      }
    }

    //*****************************************************************************
    /// <summary>
    ///   Gets a value indicating if the CAN controller is currently
    ///   transmitting (sending) a CAN message.
    /// </summary>
    /// <returns>
    ///   true if the CAN controller is currently transmitting a CAN message, 
    ///   otherwise false.
    /// </returns>
    //*****************************************************************************
    public bool IsTransmitPending
    {
      get
      {
        return ((status & CanCtrlStatus.TxPending) == CanCtrlStatus.TxPending);
      }
    }

    //*****************************************************************************
    /// <summary>
    ///   Gets a value indicating if a message was lost because there was
    ///   not enough free space for the message in the CAN controllers
    ///   internal message buffer.
    /// </summary>
    /// <returns>
    ///   true if a data overrun has occurred, otherwise false.
    /// </returns>
    //*****************************************************************************
    public bool HasDataOverrun
    {
      get
      {
        return ((status & CanCtrlStatus.Overrun) == CanCtrlStatus.Overrun);
      }
    }

    //*****************************************************************************
    /// <summary>
    ///   Gets a value indicating if a error counter has reached or
    ///   exceeded the predefined error warning limit.
    /// </summary>
    /// <returns>
    ///   true if an error counter has reached or exceeded the predefined error 
    ///   warning limit, otherwise false.
    /// </returns>
    //*****************************************************************************
    public bool HasErrorOverrun
    {
      get
      {
        return ((status & CanCtrlStatus.ErrLimit) == CanCtrlStatus.ErrLimit);
      }
    }

    //*****************************************************************************
    /// <summary>
    ///   Gets a value indicating whether the CAN controller is involved
    ///   in bus activities.
    /// </summary>
    /// <returns>
    ///   true if the CAN controller is not involved in bus activities, 
    ///   otherwise false.
    /// </returns>
    //*****************************************************************************
    public bool IsBusOff
    {
      get
      {
        return ((status & CanCtrlStatus.BusOff) == CanCtrlStatus.BusOff);
      }
    }

    //*****************************************************************************
    /// <summary>
    ///   Gets a value indicating whether the CAN controller is currently in init 
    ///   mode.
    /// </summary>
    /// <returns>
    ///   true if the CAN controller is in init mode, otherwise false.
    /// </returns>
    //*****************************************************************************
    public bool IsInInitMode
    {
      get
      {
        return ((status & CanCtrlStatus.InInit) == CanCtrlStatus.InInit);
      }
    }

    //*****************************************************************************
    /// <summary>
    ///   Gets a value indicating that the CAN controller signals an Error on his
    ///   bus coupling
    /// </summary>
    /// <returns>
    ///   true if bus coupling error on the CAN controller is signaled,
    ///   otherwise false.
    /// </returns>
    //*****************************************************************************
    public bool IsBusCErr
    {
      get
      {
        return ((status & CanCtrlStatus.BusCErr) == CanCtrlStatus.BusCErr);
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

  };


  /*****************************************************************************
   * Managed CAN controller bit timing register
   ****************************************************************************/

  //*****************************************************************************
  /// <summary>
  ///   <c>CanLineStatus2</c> is used to signalize the status of a CAN 
  ///   controller. See interface <c>ICanSocket2</c>.
  /// </summary>
  //*****************************************************************************
  public struct CanLineStatus2
  {
    private CanOperatingModes bOpMode;          // current CAN operating mode
    private CanExtendedOperatingModes bExMode;  // current CAN extended operating mode
    private byte bBusLoad;                      // average bus load in percent (0..100)
    private CanCtrlStatus status;               // status of the CAN controller (see CanCtrlStatus)
    private CanBitrate2 stdBitrate;             // current standard bitrate
    private CanBitrate2 fastBitrate;            // current fast data bitrate

    //*****************************************************************************
    /// <summary>
    ///   Ctor - create a CanLineStatus2 object
    /// </summary>
    /// <param name="opmode">operating mode</param>
    /// <param name="exmode">extended operating mode</param>
    /// <param name="busload">bus load</param>
    /// <param name="ctrlstat">controller status</param>
    /// <param name="rate">bitrate</param>
    /// <param name="fastrate">fast bitrate</param>
    //*****************************************************************************
    public CanLineStatus2(CanOperatingModes opmode, CanExtendedOperatingModes exmode, byte busload, CanCtrlStatus ctrlstat, CanBitrate2 rate, CanBitrate2 fastrate)
    {
      bOpMode = opmode;
      bExMode = exmode;
      bBusLoad = busload;
      status = ctrlstat;
      stdBitrate = rate;
      fastBitrate = fastrate;
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
      return String.Format("opmode: {0}, exmode: {1}, busload: {2}, ctrlstat: {3}, stdbitrate: {4}, fastbitrate: {5}"
        , bOpMode, bExMode, bBusLoad, status, stdBitrate, fastBitrate);
    }

    //*****************************************************************************
    /// <summary>
    ///   Gets the current operating mode of the CAN controller.
    /// </summary>
    /// <returns>
    ///   The current operating mode of the CAN controller.
    /// </returns>
    //*****************************************************************************
    public CanOperatingModes OperatingMode
    {
      get
      {
        return (bOpMode);
      }
    }
    
    //*****************************************************************************
    /// <summary>
    ///   Gets the current operating mode of the CAN controller.
    /// </summary>
    /// <returns>
    ///   The current operating mode of the CAN controller.
    /// </returns>
    //*****************************************************************************
    public CanExtendedOperatingModes ExtendedOperatingMode
    {
      get
      {
        return (bExMode);
      }
    }

    //*****************************************************************************
    /// <summary>
    ///   Gets a value indicating whether the CAN controller is currently
    ///   operating in standard (11-bit) frame mode.
    /// </summary>
    /// <returns>
    ///   true if the CAN controller is currently operating in standard frame 
    ///   mode, otherwise false.
    /// </returns>
    //*****************************************************************************
    public bool IsStdModeEnabled
    {
      get
      {
        return ((bOpMode & CanOperatingModes.Standard) == CanOperatingModes.Standard);
      }
    }

    //*****************************************************************************
    /// <summary>
    ///   Gets a value indicating whether the CAN controller is currently
    ///   operating in extended (29-bit) frame mode.
    /// </summary>
    /// <returns>
    ///   true if the CAN controller is currently operating in extended frame 
    ///   mode, otherwise false.
    /// </returns>
    //*****************************************************************************
    public bool IsExtModeEnabled
    {
      get
      {
        return ((bOpMode & CanOperatingModes.ErrFrame) == CanOperatingModes.ErrFrame);
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
        return ((bOpMode & CanOperatingModes.ErrFrame) == CanOperatingModes.ErrFrame);
      }
    }

    //*****************************************************************************
    /// <summary>
    ///  Gets a value indicating whether the CAN controller is currently
    ///  operating in listen only mode.
    /// </summary>
    /// <returns>
    ///   true if the CAN controller is currently operating in listen only mode, 
    ///   otherwise false.
    /// </returns>
    //*****************************************************************************
    public bool IsListenOnly
    {
      get
      {
        return ((bOpMode & CanOperatingModes.ListenOnly) == CanOperatingModes.ListenOnly);
      }
    }

    //*****************************************************************************
    /// <summary>
    ///  Gets a value indicating whether the CAN controller uses the
    ///  low speed bus interface.
    /// </summary>
    /// <returns>
    ///   true if the CAN controller uses the low speed bus interface, 
    ///   otherwise false.
    /// </returns>
    //*****************************************************************************
    public bool IsLowSpeedEnabled
    {
      get
      {
        return ((bOpMode & CanOperatingModes.LowSpeed) == CanOperatingModes.LowSpeed);
      }
    }

    //*****************************************************************************
    /// <summary>
    ///  Gets a value indicating whether the CAN controller uses the
    ///  automatic bitrate detection.
    /// </summary>
    /// <returns>
    ///   true if the CAN controller uses the automatic bitrate detection, 
    ///   otherwise false.
    /// </returns>
    //*****************************************************************************
    public bool IsAutomaticBitrateDetectionEnabled
    {
      get
      {
        return ((bOpMode & CanOperatingModes.AutoBaudrate) == CanOperatingModes.AutoBaudrate);
      }
    }

    //*****************************************************************************
    /// <summary>
    ///  Gets a value indicating whether the CAN controller uses the
    ///  extended data length.
    /// </summary>
    /// <returns>
    ///   true if the CAN controller uses the extended data length, 
    ///   otherwise false.
    /// </returns>
    //*****************************************************************************
    public bool IsExtendedDataLengthEnabled
    {
      get
      {
        return ((bExMode & CanExtendedOperatingModes.ExtendedDataLength) == CanExtendedOperatingModes.ExtendedDataLength);
      }
    }

    //*****************************************************************************
    /// <summary>
    ///  Gets a value indicating whether the CAN controller uses the
    ///  fast data bit rate.
    /// </summary>
    /// <returns>
    ///   true if the CAN controller uses the fast data bit rate, 
    ///   otherwise false.
    /// </returns>
    //*****************************************************************************
    public bool IsFastDataEnabled
    {
      get
      {
        return ((bExMode & CanExtendedOperatingModes.FastDataRate) == CanExtendedOperatingModes.FastDataRate);
      }
    }

    //*****************************************************************************
    /// <summary>
    ///  Gets a value indicating whether the CAN controller uses the
    ///  ISO can fd standard.
    /// </summary>
    /// <returns>
    ///   true if the CAN controller uses the ISO can fd standard, 
    ///   otherwise false.
    /// </returns>
    //*****************************************************************************
    public bool IsNonIsoCanFdEnabled
    {
      get
      {
        return ((bExMode & CanExtendedOperatingModes.NonIsoCanFd) == CanExtendedOperatingModes.NonIsoCanFd);
      }
    }

    //*****************************************************************************
    /// <summary>
    ///  Gets the current bit timing value of the CAN controller.
    /// </summary>
    /// <returns>
    ///   Current CAN bit timing.
    /// </returns>
    //*****************************************************************************
    public CanBitrate2 Bitrate
    {
      get
      {
        return (stdBitrate);
      }
    }

    //*****************************************************************************
    /// <summary>
    ///  Gets the current extended bit timing value of the CAN controller.
    /// </summary>
    /// <returns>
    ///   Current extended CAN bit timing.
    /// </returns>
    //*****************************************************************************
    public CanBitrate2 FastBitrate
    {
      get
      {
        return (fastBitrate);
      }
    }

    //*****************************************************************************
    /// <summary>
    ///   Gets the current CAN controller status.
    /// </summary>
    /// <returns>
    ///   Current CAN controller status.
    /// </returns>
    //*****************************************************************************
    public CanCtrlStatus ControllerStatus
    {
      get
      {
        return (status);
      }
    }

    //*****************************************************************************
    /// <summary>
    ///   Gets a value indicating if the CAN controller is currently
    ///   transmitting (sending) a CAN message.
    /// </summary>
    /// <returns>
    ///   true if the CAN controller is currently transmitting a CAN message, 
    ///   otherwise false.
    /// </returns>
    //*****************************************************************************
    public bool IsTransmitPending
    {
      get
      {
        return ((status & CanCtrlStatus.TxPending) == CanCtrlStatus.TxPending);
      }
    }

    //*****************************************************************************
    /// <summary>
    ///   Gets a value indicating if a message was lost because there was
    ///   not enough free space for the message in the CAN controllers
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
        return ((status & CanCtrlStatus.Overrun) == CanCtrlStatus.Overrun);
      }
    }

    //*****************************************************************************
    /// <summary>
    ///   Gets a value indicating if a error counter has reached or
    ///   exceeded the predefined error warning limit.
    /// </summary>
    /// <returns>
    ///   true if an error counter has reached or exceeded the predefined error 
    ///   warning limit, otherwise false.
    /// </returns>
    //*****************************************************************************
    public bool HasErrorOverrun
    {
      get
      {
        return ((status & CanCtrlStatus.ErrLimit) == CanCtrlStatus.ErrLimit);
      }
    }

    //*****************************************************************************
    /// <summary>
    ///   Gets a value indicating whether the CAN controller is involved
    ///   in bus activities.
    /// </summary>
    /// <returns>
    ///   true if the CAN controller is not involved in bus activities, 
    ///   otherwise false.
    /// </returns>
    //*****************************************************************************
    public bool IsBusOff
    {
      get
      {
        return ((status & CanCtrlStatus.BusOff) == CanCtrlStatus.BusOff);
      }
    }

    //*****************************************************************************
    /// <summary>
    ///   Gets a value indicating whether the CAN controller is currently in init 
    ///   mode.
    /// </summary>
    /// <returns>
    ///   true if the CAN controller is in init mode, otherwise false.
    /// </returns>
    //*****************************************************************************
    public bool IsInInitMode
    {
      get
      {
        return ((status & CanCtrlStatus.InInit) == CanCtrlStatus.InInit);
      }
    }

    //*****************************************************************************
    /// <summary>
    ///   Gets a value indicating that the CAN controller signals an Error on his
    ///   bus coupling
    /// </summary>
    /// <returns>
    ///   true if bus coupling error on the CAN controller is signaled,
    ///   otherwise false.
    /// </returns>
    //*****************************************************************************
    public bool IsBusCErr
    {
      get
      {
        return ((status & CanCtrlStatus.BusCErr) == CanCtrlStatus.BusCErr);
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

  };


}