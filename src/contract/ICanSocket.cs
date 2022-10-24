/*****************************************************************************
 IXXAT Automation GmbH
******************************************************************************
 Summary : Declarations for the CAN socket class.
 Compiler: Microsoft VC++ 8
******************************************************************************
 all rights reserved
*****************************************************************************/

/*************************************************************************
** namespace Ixxat.Vci4.Bal.Can
*************************************************************************/
namespace Ixxat.Vci4.Bal.Can 
{
  using System;

  //*****************************************************************************
  /// <summary>
  ///   Enumeration of flag values that are used to signalize the CAN bus 
  ///   coupling (see <c>ICanSocket</c>).
  /// </summary>
  //*****************************************************************************
  [Flags]
  public enum CanBusCouplings : int
  {
    /// <summary>
    ///   Undefined
    /// </summary>
    Undefined = 0x0000, 
    /// <summary>
    ///   Low speed coupling
    /// </summary>
    LowSpeed  = 0x0001,  
    /// <summary>
    ///   High speed coupling
    /// </summary>
    HighSpeed = 0x0002  
  };


  //*****************************************************************************
  /// <summary>
  ///   Enumeration of values that are used to signalize the type of a CAN 
  ///   controller (see <c>ICanSocket</c>).
  /// </summary>
  //*****************************************************************************
  public enum CanCtrlType : int
  {
    /// <summary>
    ///   Unknown (possibly MC internal)
    /// </summary>
    Unknown         = 0, 
    /// <summary>
    ///   Intel 82527
    /// </summary>
    Intel82527      = 1,   
    /// <summary>
    ///   Intel 82C200
    /// </summary>
    Intel82C200     = 2,  
    /// <summary>
    ///   Intel 81C90
    /// </summary>
    Intel81C90      = 3,   
    /// <summary>
    ///   Intel 82C92
    /// </summary>
    Intel81C92      = 4,   
    /// <summary>
    ///   Philips SJA 1000
    /// </summary>
    SJA1000         = 5, 
    /// <summary>
    ///   Infinion 82C900 (TwinCAN)
    /// </summary>
    Infineon82C900  = 6,  
    /// <summary>
    ///   Motorola TOUCAN
    /// </summary>
    TouCAN          = 7,
    /// <summary>
    ///   Freescale Star12 MSCAN
    /// </summary>
    msCAN           = 8,
    /// <summary>
    ///   Freescale Coldfire FLEXCAN
    /// </summary>
    FLEXCAN         = 9,
    /// <summary>
    ///   IFI Can (Altera FPGA)
    /// </summary>
    IFI_CAN         = 10,
    /// <summary>
    ///   Bosch C_CAN
    /// </summary>
    C_CAN           = 11,
    /// <summary>
    ///   ST BX_CAN
    /// </summary>
    bxCAN           = 12,
    /// <summary>
    ///   IFI CAN FD (Altera FPGA)
    /// </summary>
    IFI_CAN_FD      = 13,
    /// <summary>
    ///   Bosch M_CAN
    /// </summary>
    M_CAN           = 14
  };


  //*****************************************************************************
  /// <summary>
  ///   Enumeration of values that are used for filter selection to specify
  ///   a CAN message filter. See interface <c>ICanControl</c>.
  /// </summary>
  //*****************************************************************************
  public enum CanFilter : int
  {
    /// <summary>
    ///   Select standard filter (11-bit)
    /// </summary>
    Std = 1,  
    /// <summary>
    ///   Select extended filter (29-bit)
    /// </summary>
    Ext = 2  
  };


  //*****************************************************************************
  /// <summary>
  ///   Enumeration with predefined CAN acceptance filter mask settings.
  /// </summary>
  //*****************************************************************************
  public enum CanAccMask : uint
  {
    /// <summary>
    ///   Acceptance mask to accept all CAN IDs
    /// </summary>
    All  = 0x00000000,
    /// <summary>
    ///   Acceptance mask to reject all CAN IDs
    /// </summary>
    None = 0xFFFFFFFF
  };


  //*****************************************************************************
  /// <summary>
  ///   Enumeration with predefined CAN acceptance filter code settings.
  /// </summary>
  //*****************************************************************************
  public enum CanAccCode : uint
  {
    /// <summary>
    ///   Acceptance code to accept all CAN IDs
    /// </summary>
    All  = 0x00000000,
    /// <summary>
    ///   Acceptance code to reject all CAN IDs
    /// </summary>
    None  = 0x80000000
  };


  //*****************************************************************************
  /// <summary>
  ///   Enumeration of flag values that are used to signalize the features 
  ///   supported by a CAN controller (see <c>ICanSocket</c>).
  /// </summary>
  //*****************************************************************************
  [Flags]
  public enum CanFeatures 
  {
    /// <summary>
    ///   11 OR 29 bit (exclusive)
    /// </summary>
    StdOrExt    = 0x00000001,  
    /// <summary>
    ///   11 AND 29 bit (simultaneous)
    /// </summary>
    StdAndExt   = 0x00000002, 
    /// <summary>
    ///   Reception of remote frames
    /// </summary>
    RemoteFrame = 0x00000004,  
    /// <summary>
    ///   Reception of error frames
    /// </summary>
    ErrFrame    = 0x00000008,  
    /// <summary>
    ///   Bus load measurement in percent
    /// </summary>
    Busload     = 0x00000010,   
    /// <summary>
    ///   Listen only mode
    /// </summary>
    IdFilter    = 0x00000020,  
    /// <summary>
    ///   Cyclic message scheduler
    /// </summary>
    ListOnly    = 0x00000040,  
    /// <summary>
    ///   Cyclic message scheduler
    /// </summary>
    Scheduler   = 0x00000080, 
    /// <summary>
    ///   Error frame generation
    /// </summary>
    GenErrFrame = 0x00000100, 
    /// <summary>
    ///   Delayed message transmitter
    /// </summary>
    DelayedTX   = 0x00000200,  
      /// <summary>
    ///   Single shot mode
    /// </summary>
    SingleShot   = 0x00000400,
    /// <summary>
    ///   High priority message
    /// </summary>
    HighPriorityMsg = 0x00000800,
    /// <summary>
    ///   Automatic bit rate detection
    /// </summary>
    AutoBaudrate = 0x00001000,
    /// <summary>
    ///   Extended data length (CAN-FD)
    /// </summary>
    ExtendedDataLength = 0x00002000,
    /// <summary>
    ///   Fast data bit rate (CAN-FD)
    /// </summary>
    FastDataRate = 0x00004000,
    /// <summary>
    ///   ISO conform frame (CAN-FD)
    /// </summary>
    IsoCanFd = 0x00008000,
    /// <summary>
    ///   non-ISO conform frame (CAN-FD)
    /// </summary>
    NonIsoCanFd = 0x00010000,
    /// <summary>
    ///   64-bit time stamp counter
    /// </summary>
    LongBitTimeStamp = 0x00020000
  };


  //*****************************************************************************
  /// <summary>
  ///   <c>ICanSocket</c> provides the properties and capabilities of a
  ///   CAN controller.
  ///   When no longer needed the CAN contol object has to be disposed using the 
  ///   IDisposable interface. 
  ///   A CAN socket object can be got via method <c>IBalObject.OpenSocket()</c>.
  ///   Additionally <c>ICanSocket</c> is the base interface for several other
  ///   CAN bus specific socket interfaces like <c>ICanControl</c>,
  ///   <c>ICanScheduler</c> and <c>ICanChannel</c>.
  /// </summary>
  /// <remarks>
  ///   The VCI interfaces provide access to native driver resources. Because the 
  ///   .NET garbage collector is only designed to manage memory, but not 
  ///   native OS and driver resources the caller is responsible to release this 
  ///   resources via calling <c>IDisposable.Dispose()</c> when the object is no 
  ///   longer needed. Otherwise native memory and resource leaks may occure.
  /// </remarks>
  /// <example>
  ///   <code>
  ///   IBalObject bal = ...
  ///   // Open first CAN socket
  ///   ICanSocket socket = bal.OpenSocket(0, typeof(ICanSocket)) as ICanSocket;
  ///   
  ///   // Use socket
  ///   // ...
  ///   
  ///   // Dispose socket an BAL
  ///   socket.Dispose();
  ///   bal.Dispose();
  ///   </code>
  /// </example>
  //*****************************************************************************
  public interface ICanSocket : IBalResource
  {
    //*****************************************************************************
    /// <summary>
    ///   Gets the type of controller used by the CAN socket.
    /// </summary>
    /// <exception cref="ObjectDisposedException">
    ///   Object is already disposed.
    /// </exception>
    //*****************************************************************************
    CanCtrlType      ControllerType                { get; }

    //*****************************************************************************
    /// <summary>
    ///   Gets the type of bus coupling used by the CAN controller.
    /// </summary>
    /// <exception cref="ObjectDisposedException">
    ///   Object is already disposed.
    /// </exception>
    //*****************************************************************************
    CanBusCouplings  BusCoupling                   { get; }

    //*****************************************************************************
    /// <summary>
    ///   Gets the frequency to the primary timer in Hz.
    /// </summary>
    /// <exception cref="ObjectDisposedException">
    ///   Object is already disposed.
    /// </exception>
    //*****************************************************************************
    uint             ClockFrequency                { get; }

    //*****************************************************************************
    /// <summary>
    ///   Gets the divisor factor of the time stamp counter. 
    ///   The time stamp counter provides the time stamp for CAN messages. 
    ///   The frequency of the time stamp counter is calculated from the frequency 
    ///   of the primary timer (<c>ClockFrequency</c>) divided by the value 
    ///   specified here.
    /// </summary>
    /// <exception cref="ObjectDisposedException">
    ///   Object is already disposed.
    /// </exception>
    //*****************************************************************************
    uint             TimeStampCounterDivisor       { get; }

    //*****************************************************************************
    /// <summary>
    ///   Gets the divisor factor for the timer of the cyclic transmit list
    ///   (See <c>ICanScheduler</c>. The frequency of this timer is calculated 
    ///   from the frequency of the primary timer (<c>ClockFrequency</c>) divided 
    ///   by the value specified here. If no cyclic transmit list is available, 
    ///   property <c>CyclicMessageTimerDivisor</c> has the value 0.
    /// </summary>
    /// <exception cref="ObjectDisposedException">
    ///   Object is already disposed.
    /// </exception>
    //*****************************************************************************
    uint             CyclicMessageTimerDivisor     { get; }

    //*****************************************************************************
    /// <summary>
    ///   Gets the maximum cycle time of the CAN message scheduler in number of 
    ///   ticks.
    /// </summary>
    /// <exception cref="ObjectDisposedException">
    ///   Object is already disposed.
    /// </exception>
    //*****************************************************************************
    uint             MaxCyclicMessageTicks         { get; }

    //*****************************************************************************
    /// <summary>
    ///   Gets the divisor factor for the timer used for delayed transmission of 
    ///   messages. The frequency of this timer is calculated from the frequency 
    ///   of the primary timer (<c>ClockFrequency</c>) divided by the value 
    ///   specified here. If delayed transmission is not supported by the 
    ///   adapter, property <c>DelayedTXTimerDivisor</c> has the value 0.
    /// </summary>
    /// <exception cref="ObjectDisposedException">
    ///   Object is already disposed.
    /// </exception>
    //*****************************************************************************
    uint             DelayedTXTimerDivisor         { get; }

    //*****************************************************************************
    /// <summary>
    ///   Gets the maximum delay time of the delayed CAN message transmitter in 
    ///   number of ticks.
    /// </summary>
    /// <exception cref="ObjectDisposedException">
    ///   Object is already disposed.
    /// </exception>
    //*****************************************************************************
    uint             MaxDelayedTXTicks             { get; }

    //*****************************************************************************
    /// <summary>
    ///   Gets the current status of the CAN line.
    /// </summary>
    /// <exception cref="VciException">
    ///   Getting CAN line status failed.
    /// </exception>
    /// <exception cref="ObjectDisposedException">
    ///   Object is already disposed.
    /// </exception>
    //*****************************************************************************
    CanLineStatus    LineStatus                    { get; }

    //*****************************************************************************
    /// <summary>
    ///   Gets a flag field indicating the features supported by the CAN 
    ///   controller.
    /// </summary>
    /// <exception cref="ObjectDisposedException">
    ///   Object is already disposed.
    /// </exception>
    //*****************************************************************************
    CanFeatures      Features                      { get; }

    //*****************************************************************************
    /// <summary>
    ///   Gets a value indicating if the CAN socket supports standard (11-bit) 
    ///   and extended (29-bit) format exclusively.
    /// </summary>
    /// <exception cref="ObjectDisposedException">
    ///   Object is already disposed.
    /// </exception>
    //*****************************************************************************
    bool             SupportsStdOrExtFrames        { get; }

    //*****************************************************************************
    /// <summary>
    ///   Gets a value indicating if the CAN socket supports standard (11-bit) 
    ///   and extended (29-bit) message frames simultanously.
    /// </summary>
    /// <exception cref="ObjectDisposedException">
    ///   Object is already disposed.
    /// </exception>
    //*****************************************************************************
    bool             SupportsStdAndExtFrames       { get; }

    //*****************************************************************************
    /// <summary>
    ///   Gets a value indicating if the CAN socket supports remote transfer 
    ///   requests.
    /// </summary>
    /// <exception cref="ObjectDisposedException">
    ///   Object is already disposed.
    /// </exception>
    //*****************************************************************************
    bool             SupportsRemoteFrames          { get; }

    //*****************************************************************************
    /// <summary>
    ///   Gets a value indicating if the CAN socket supports error frames.
    /// </summary>
    /// <exception cref="ObjectDisposedException">
    ///   Object is already disposed.
    /// </exception>
    //*****************************************************************************
    bool             SupportsErrorFrames           { get; }

    //*****************************************************************************
    /// <summary>
    ///   Gets a value indicating if the CAN socket supports bus load computation.
    /// </summary>
    /// <exception cref="ObjectDisposedException">
    ///   Object is already disposed.
    /// </exception>
    //*****************************************************************************
    bool             SupportsBusLoadComputation    { get; }

    //*****************************************************************************
    /// <summary>
    ///   Gets a value indicating if the CAN socket supports exact filtering of 
    ///   CAN messages.
    /// </summary>
    /// <exception cref="ObjectDisposedException">
    ///   Object is already disposed.
    /// </exception>
    //*****************************************************************************
    bool             SupportsExactMessageFilter    { get; }

    //*****************************************************************************
    /// <summary>
    ///   Gets a value indicating if the CAN socket supports listen only mode.
    /// </summary>
    /// <exception cref="ObjectDisposedException">
    ///   Object is already disposed.
    /// </exception>
    //*****************************************************************************
    bool             SupportsListenOnlyMode        { get; }

    //*****************************************************************************
    /// <summary>
    ///   Gets a value indicating if the CAN socket supports a cyclic message 
    ///   scheduler.
    /// </summary>
    /// <exception cref="ObjectDisposedException">
    ///   Object is already disposed.
    /// </exception>
    //*****************************************************************************
    bool             SupportsCyclicMessageScheduler{ get; }

    //*****************************************************************************
    /// <summary>
    ///   Gets a value indicating if the CAN socket supports the generation of 
    ///   error message frames.
    /// </summary>
    /// <exception cref="ObjectDisposedException">
    ///   Object is already disposed.
    /// </exception>
    //*****************************************************************************
    bool             SupportsErrorFrameGeneration  { get; }

    //*****************************************************************************
    /// <summary>
    ///   Gets a value indicating if the CAN socket supports delayed transmission 
    ///   of CAN message frames.
    /// </summary>
    /// <exception cref="ObjectDisposedException">
    ///   Object is already disposed.
    /// </exception>
    //*****************************************************************************
    bool             SupportsDelayedTransmission   { get; }
  };


}