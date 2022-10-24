/*****************************************************************************
 IXXAT Automation GmbH
******************************************************************************
 Summary : Declarations for the Vci controller info value class.
 Compiler: Microsoft VC++ 8
******************************************************************************
 all rights reserved
*****************************************************************************/


/*************************************************************************
** namespace Ixxat.Vci4
*************************************************************************/
namespace Ixxat.Vci4 
{


  /*************************************************************************
  ** used namespaces
  *************************************************************************/
  using System;
  using Ixxat.Vci4.Bal.Can;


  //*****************************************************************************
  /// <summary>
  ///   This struct contains the bus type and controller type of a device's
  ///   fieldbus controller. An array of such structs can be obtained from
  ///   property <c>IVciDevice.Equipment</c>.
  /// </summary>
  /// <example>
  ///   <code>
  ///     //
  ///     // Get device manager from VCI server
  ///     //
  ///     deviceManager = VciServer.Instance().DeviceManager;
  ///
  ///     //
  ///     // Get the list of installed VCI devices
  ///     //
  ///     deviceList = deviceManager.GetDeviceList();
  ///
  ///     //
  ///     // Get enumerator for the list of devices
  ///     //
  ///     deviceEnum = deviceList.GetEnumerator();
  ///
  ///     //
  ///     // Get first device
  ///     //
  ///     deviceEnum.MoveNext();
  ///     mDevice = deviceEnum.Current as IVciDevice;
  ///
  ///     //
  ///     // print bus type and controller type of first controller
  ///     //
  ///     IVciCtrlInfo info = mDevice.Equipment[0];
  ///     Console.Write(" BusType    : {0}\n", info.BusType);
  ///     Console.Write(" CtrlType   : {0}\n", info.ControllerType);
  ///   </code>
  /// </example>
  //*****************************************************************************
  public interface IVciCtrlInfo
  {
    //--------------------------------------------------------------------
    // Properties
    //--------------------------------------------------------------------

    //*****************************************************************************
    /// <summary>
    ///   Get's the type of the supported fieldbus.
    /// </summary>
    //*****************************************************************************
    VciBusType BusType
    {
      get;
    }

    //*****************************************************************************
    /// <summary>
    ///   Get's the type of the fieldbus controller. Because the actual data type 
    ///   property depends in the bus type, the retrieved value is boxed into an
    ///   object reference. The calling method has to cast it to the appropriate
    ///   data type. For a CAN bus controller (<c>BusType</c> = 
    ///   <c>VciBusType.Can</c>) the actual data type of property 
    ///   <c>ControllerType</c> is <c>CanCtrlType</c>.
    ///   This property can be a null reference if the <c>BusType</c> is 
    ///   unknown.
    /// </summary>
    //*****************************************************************************
    Object ControllerType
    {
      get;
    }
  };


}
