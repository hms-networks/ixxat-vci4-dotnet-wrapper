/*****************************************************************************
 IXXAT Automation GmbH
******************************************************************************
 Summary : Declarations for the VCI device manager object class.
 Compiler: Microsoft VC++ 8
******************************************************************************
 all rights reserved
*****************************************************************************/

namespace Ixxat.Vci4 
{
  using System;
  using System.ComponentModel;


  //*****************************************************************************
  /// <summary>
  ///   The message factory is used to create different message objects
  ///   used by the VCI interface.
  ///   Use the static <c>VciServer</c> class to get a message factory
  ///   object.
  /// </summary>
  /// <example>
  ///   <code>
  ///     IMessageFactory factory = VciServer.Instance().MsgFactory;
  ///
  ///     // create CAN message (for transmission)
  ///     ICanMessage canMsg = (ICanMessage)factory.CreateMsg(typeof(ICanMessage));
  ///     // ...
  ///
  ///     // create CAN-FD message (for transmission)
  ///     ICanMessage2 canMsg = (ICanMessage2)factory.CreateMsg(typeof(ICanMessage2));
  ///     // ...
  ///
  ///     // create LIN message (for transmission)
  ///     ILinMessage message = (ICanMessage)factory.CreateMsg(typeof(ILinMessage));
  ///     // ...
  ///   </code>
  /// </example>
  //*****************************************************************************
  public interface IMessageFactory
  {
    //*****************************************************************************
    /// <summary>
    ///   Creates a message object denoted by the message interface type.
    /// </summary>
    /// <param name="msgtyp">Use typeof(ICanMessage), typeof(ICanMessage2) or typeof(ILinMessage)</param>
    /// <returns>
    ///   Message object.
    /// </returns>
    /// <exception cref="ArgumentException">
    ///   Requested message type not supported.
    /// </exception>
    //*****************************************************************************
    System.Object CreateMsg(System.Type msgtyp);
  };

} 
