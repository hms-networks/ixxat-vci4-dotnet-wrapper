/*****************************************************************************
 IXXAT Automation GmbH
******************************************************************************
 Summary : Declarations for the LIN control class.
 Compiler: Microsoft VC++ 8
******************************************************************************
 all rights reserved
*****************************************************************************/

namespace Ixxat.Vci4.Bal.Lin
{
  using System;

  //*****************************************************************************
  /// <summary>
  ///   Struct that contains LIN bus initialization data that's used by 
  ///   <c>ILinControl.InitLine</c>
  /// </summary>
  //*****************************************************************************
  public struct LinInitLine
  {
    ///<summary>LIN operating mode</summary>
    public LinOperatingModes OperatingMode;
    ///<summary>LIN bitrate</summary>
    public LinBitrate Bitrate;
  };


  //*****************************************************************************
  /// <summary>
  ///   This interface represents a LIN control unit and is used to control a
  ///   LIN line. Controlling consists of initialisation, starting/stoping 
  ///   the LIN line and sending messages.
  ///   When no longer needed the LIN contol object has to be disposed using the 
  ///   IDisposable interface. 
  ///   A LIN control object can be got via method <c>IBalObject.OpenSocket()</c>. 
  ///   The LIN control cannot be opened twice at the same time. Therefore a
  ///   second try to open the LIN control via <c>IBalObject.OpenSocket()</c>
  ///   fails until the successfully opened LIN control object is explicitly
  ///   disposed.
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
  ///   // Open communication channel on first LIN socket
  ///   ILinControl control = bal.OpenSocket(0, typeof(ILinControl)) as ILinControl;
  ///   
  ///   // Initialize CAN line
  ///   LinInitLine initData;
  ///   initData.OperatingMode = LinOperatingModes.Slave;
  ///   initData.Bitrate = LinBitrate.Lin1000Bit;
  ///   control.InitLine(initData);
  ///   
  ///   // Use LIN line
  ///   // ...
  ///   
  ///   // Dispose control and BAL
  ///   control.Dispose();
  ///   bal.Dispose();
  ///   </code>
  /// </example>
  //*****************************************************************************
  public interface ILinControl : ILinSocket
  {
    //*****************************************************************************
    /// <summary>
    ///   This method initializes the LIN line in the specified operating mode
    ///   and bit transfer rate. The method also performs a reset of the LIN
    ///   controller hardware.
    /// </summary>
    /// <param name="initLine">
    ///   Specifies the operating mode and bit transfer rate 
    /// </param>
    /// <exception cref="VciException">
    ///   LIN line initialization failed.
    /// </exception>
    /// <exception cref="ObjectDisposedException">
    ///   Object is already disposed.
    /// </exception>
    //*****************************************************************************
    void InitLine( LinInitLine initLine );

    //*****************************************************************************
    /// <summary>
    ///   This method resets the LIN line to it's initial state. The method
    ///   aborts a currently busy transmit message and switches the LIN controller
    ///   into init mode.
    /// </summary>
    /// <exception cref="VciException">
    ///   Resetting LIN line failed.
    /// </exception>
    /// <exception cref="ObjectDisposedException">
    ///   Object is already disposed.
    /// </exception>
    //*****************************************************************************
    void  ResetLine   ( );

    //*****************************************************************************
    /// <summary>
    ///   This method starts the LIN line and switch it into running mode.
    ///   After starting the LIN line.
    /// </summary>
    /// <exception cref="VciException">
    ///   Starting LIN line failed.  
    /// </exception>
    /// <exception cref="ObjectDisposedException">
    ///   Object is already disposed. 
    /// </exception>
    //*****************************************************************************
    void  StartLine   ( );

    //*****************************************************************************
    /// <summary>
    ///   This method stops the LIN line an switches it into init mode. After
    ///   stopping the LIN controller no further LIN messages are transmitted.
    ///   Other than <c>ResetLine</c>, this method does not abort a currently 
    ///   busy transmit message.
    /// </summary>
    /// <exception cref="VciException">
    ///   Stopping LIN line failed.
    /// </exception>
    /// <exception cref="ObjectDisposedException">
    ///   Object is already disposed.  
    /// </exception>
    //*****************************************************************************
    void  StopLine    ( );

    //*****************************************************************************
    /// <summary>
    ///   This function either transmits the specified message directly to the LIN 
    ///   bus connected to the controller or enters the message in the response 
    ///   table of the controller.
    /// </summary>
    /// <param name="send">
    ///   true to force sending the message directly or false to enter the message
    ///   into the controller's response table.
    /// </param>
    /// <param name="message">
    ///   The message to be transmitted.
    /// </param>
    /// <exception cref="VciException">
    ///   Writing the message failed.
    /// </exception>
    /// <exception cref="ObjectDisposedException">
    ///   Object is already disposed.
    /// </exception>
    //*****************************************************************************
    void  WriteMessage(bool        send, 
                       ILinMessage message);
  };


}
