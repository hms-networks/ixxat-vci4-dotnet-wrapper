/*****************************************************************************
 IXXAT Automation GmbH
******************************************************************************
 Summary : Declarations for the CAN socket class.
 Compiler: Microsoft VC++ 8
******************************************************************************
 all rights reserved
*****************************************************************************/

namespace Ixxat.Vci4.Bal.Can 
{
  using System;

//*****************************************************************************
/// <summary>
///   <c>ICanSocket2</c> provides the properties and capabilities of a
///   CAN-FD controller.
///   When no longer needed the CAN control object has to be disposed using the 
///   IDisposable interface. 
///   A CAN socket object can be got via method <c>IBalObject.OpenSocket()</c>.
///   Additionally <c>ICanSocket2</c> is the base interface for several other
///   CAN bus specific socket interfaces like <c>ICanControl2</c>,
///   <c>ICanScheduler2</c> and <c>ICanChannel2</c>.
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
///   ICanSocket2 socket = bal.OpenSocket(0, typeof(ICanSocket2)) as ICanSocket2;
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
public interface ICanSocket2 : IBalResource
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
  ///   Gets the can clock frequency in Hz.
  /// </summary>
  /// <exception cref="ObjectDisposedException">
  ///   Object is already disposed.
  /// </exception>
  //*****************************************************************************
  uint             CanClockFrequency                { get; }
  
  //*****************************************************************************
  /// <summary>
  ///   Gets the minimum bit timing values for arbitration bit rate.
  /// </summary>
  /// <exception cref="ObjectDisposedException">
  ///   Object is already disposed.
  /// </exception>
  //*****************************************************************************
  CanBitrate2      MinimumArbitrationBitrate        { get; }

  //*****************************************************************************
  /// <summary>
  ///   Gets the maximum bit timing values for arbitration bit rate.
  /// </summary>
  /// <exception cref="ObjectDisposedException">
  ///   Object is already disposed.
  /// </exception>
  //*****************************************************************************
  CanBitrate2      MaximumArbitrationBitrate         { get; }

  //*****************************************************************************
  /// <summary>
  ///   Gets the minimum bit timing values for fast data bit rate.
  /// </summary>
  /// <exception cref="ObjectDisposedException">
  ///   Object is already disposed.
  /// </exception>
  //*****************************************************************************
  CanBitrate2      MinimumFastDataBitrate            { get; }

  //*****************************************************************************
  /// <summary>
  ///   Gets the maximum bit timing values for fast data bit rate.
  /// </summary>
  /// <exception cref="ObjectDisposedException">
  ///   Object is already disposed.
  /// </exception>
  //*****************************************************************************
  CanBitrate2      MaximumFastDataBitrate            { get; }

  //*****************************************************************************
  /// <summary>
  ///   Gets the clock frequency of the time stamp counter [Hz]
  /// </summary>
  /// <exception cref="ObjectDisposedException">
  ///   Object is already disposed.
  /// </exception>
  //*****************************************************************************
  uint             TimeStampCounterClockFrequency   { get; }

  //*****************************************************************************
  /// <summary>
  ///   Gets the divisor factor of the time stamp counter. 
  ///   The time stamp counter provides the time stamp for CAN messages. 
  ///   The frequency of the time stamp counter is calculated from the frequency 
  ///   of the can clock timer (<c>CanClockFrequency</c>) divided by the value 
  ///   specified here.
  /// </summary>
  /// <exception cref="ObjectDisposedException">
  ///   Object is already disposed.
  /// </exception>
  //*****************************************************************************
  uint             TimeStampCounterDivisor       { get; }

  //*****************************************************************************
  /// <summary>
  ///   Gets the clock frequency of cyclic message scheduler [Hz]
  /// </summary>
  /// <exception cref="ObjectDisposedException">
  ///   Object is already disposed.
  /// </exception>
  //*****************************************************************************
  uint             CyclicMessageTimerClockFrequency { get; }

  //*****************************************************************************
  /// <summary>
  ///   Gets the divisor factor for the timer of the cyclic transmit list
  ///   (See <c>ICanScheduler2</c>. The frequency of this timer is calculated 
  ///   from the frequency of the can clock timer (<c>CanClockFrequency</c>) divided 
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
  ///   Gets the clock frequency of the delayed message transmitter [Hz]
  /// </summary>
  /// <exception cref="ObjectDisposedException">
  ///   Object is already disposed.
  /// </exception>
  //*****************************************************************************
  uint             DelayedTXTimerClockFrequency   { get; }

  //*****************************************************************************
  /// <summary>
  ///   Gets the divisor factor for the timer used for delayed transmission of 
  ///   messages. The frequency of this timer is calculated from the frequency 
  ///   of the can clock timer (<c>CanClockFrequency</c>) divided by the value 
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
  CanLineStatus2    LineStatus                   { get; }

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

  //*****************************************************************************
  /// <summary>
  ///   Gets a value indicating if the CAN socket supports single shot messages
  /// </summary>
  /// <exception cref="ObjectDisposedException">
  ///   Object is already disposed.
  /// </exception>
  //*****************************************************************************
  bool             SupportsSingleShotMessages    { get; }

  //*****************************************************************************
  /// <summary>
  ///   Gets a value indicating if the CAN socket supports high priority messages
  /// </summary>
  /// <exception cref="ObjectDisposedException">
  ///   Object is already disposed.
  /// </exception>
  //*****************************************************************************
  bool             SupportsHighPriorityMessages  { get; }

  //*****************************************************************************
  /// <summary>
  ///   Gets a value indicating if the CAN socket supports auto baudrate detection 
    /// </summary>
  /// <exception cref="ObjectDisposedException">
  ///   Object is already disposed.
  /// </exception>
  //*****************************************************************************
  bool             SupportsAutoBaudrateDetection { get; }

  //*****************************************************************************
  /// <summary>
  ///   Gets a value indicating if the CAN socket supports extended data length 
  ///   messages.
  /// </summary>
  /// <exception cref="ObjectDisposedException">
  ///   Object is already disposed.
  /// </exception>
  //*****************************************************************************
  bool             SupportsExtendedDataLength    { get; }

  //*****************************************************************************
  /// <summary>
  ///   Gets a value indicating if the CAN socket supports fast data bit rate 
  ///   messages.
  /// </summary>
  /// <exception cref="ObjectDisposedException">
  ///   Object is already disposed.
  /// </exception>
  //*****************************************************************************
  bool             SupportsFastDataRate          { get; }

  //*****************************************************************************
  /// <summary>
  ///   Gets a value indicating if the CAN socket supports the ISO CAN-FD 
  ///   format.
  /// </summary>
  /// <exception cref="ObjectDisposedException">
  ///   Object is already disposed.
  /// </exception>
  //*****************************************************************************
  bool             SupportsIsoCanFdFrames        { get; }

  //*****************************************************************************
  /// <summary>
  ///   Gets a value indicating if the CAN socket supports the non-ISO CAN-FD
  ///   format.
  /// </summary>
  /// <exception cref="ObjectDisposedException">
  ///   Object is already disposed.
  /// </exception>
  //*****************************************************************************
  bool             SupportsNonIsoCanFdFrames     { get; }

  //*****************************************************************************
  /// <summary>
  ///   Gets a value indicating if the CAN socket supports 64-bit timestamps
  /// </summary>
  /// <exception cref="ObjectDisposedException">
  ///   Object is already disposed.
  /// </exception>
  //*****************************************************************************
  bool             Supports64BitTimeStamps       { get; }
};


}