using System;
using System.Collections;
using System.Text;
using System.Threading;
using Ixxat.Vci4;
using Ixxat.Vci4.Bal;
using Ixxat.Vci4.Bal.Can;

namespace Vci4Tests
{
  [TestClass]
  public class CanLineStatusTest2
    : VciDeviceTestBase
  {
    #region Member variables

    private CanLineStatus mStatus;

    #endregion

    #region Test Initialize and Cleanup

    [TestInitialize]
    public void TestSetup()
    {
      Ixxat.Vci4.IVciDevice? device = GetDevice();
      Ixxat.Vci4.Bal.IBalObject? bal = device!.OpenBusAccessLayer();
      Ixxat.Vci4.Bal.Can.ICanControl? socket = bal!.OpenSocket(0, typeof(Ixxat.Vci4.Bal.Can.ICanControl)) as Ixxat.Vci4.Bal.Can.ICanControl;

      socket!.InitLine(CanOperatingModes.Standard, CanBitrate.Cia500KBit);

      // wait time to actualize the status
      System.Threading.Thread.Sleep(500);

      mStatus = socket!.LineStatus;

      socket!.Dispose();
      bal!.Dispose();
      device!.Dispose();
    }

    #endregion

    #region Property Bitrate Test methods

    [TestMethod]
    /// <summary>
    ///   Bitrate has constant value
    /// </summary>
    public void BitrateIsConstant()
    {
      CanBitrate refValue = mStatus.Bitrate;
      Assert.IsTrue(CanBitrate.Cia500KBit == refValue);
    }

    #endregion

    #region Property Busload Test methods

    [TestMethod]
    /// <summary>
    ///   Busload has constant value
    /// </summary>
    public void BusloadIsConstant()
    {
      byte refValue = mStatus.Busload;
      Assert.IsTrue(0 == refValue);
    }

    #endregion

    #region Property ControllerStatus Test methods

    [TestMethod]
    /// <summary>
    ///   ControllerStatus has constant value
    /// </summary>
    public void ControllerStatusIsConstant()
    {
      CanCtrlStatus refValue = mStatus.ControllerStatus;
      Assert.IsTrue((refValue & CanCtrlStatus.InInit) == CanCtrlStatus.InInit);
      
      CanCtrlStatus testValue = mStatus.ControllerStatus;
      Assert.IsTrue(testValue == refValue);
    }

    #endregion

    #region Property HasDataOverrun Test methods

    [TestMethod]
    /// <summary>
    ///   HasDataOverrun has constant value
    /// </summary>
    public void HasDataOverrunIsConstant()
    {
      bool refValue = mStatus.HasDataOverrun;
      Assert.IsTrue(false == refValue);
    }

    #endregion

    #region Property HasErrorOverrun Test methods

    [TestMethod]
    /// <summary>
    ///   HasErrorOverrun has constant value
    /// </summary>
    public void HasErrorOverrunIsConstant()
    {
      bool refValue = mStatus.HasErrorOverrun;
      Assert.IsTrue(false == refValue);
    }

    #endregion

    #region Property IsBusOff Test methods

    [TestMethod]
    /// <summary>
    ///   IsBusOff has constant value
    /// </summary>
    public void IsBusOffIsConstant()
    {
      bool refValue = mStatus.IsBusOff;
      Assert.IsTrue(false == refValue);
    }

    #endregion

    #region Property IsErrModeEnabled Test methods

    [TestMethod]
    /// <summary>
    ///   IsErrModeEnabled has constant value
    /// </summary>
    public void IsErrModeEnabledIsConstant()
    {
      bool refValue = mStatus.IsErrorFramesEnabled;
      Assert.IsTrue(false == refValue);
    }

    #endregion

    #region Property IsExtModeEnabled Test methods

    [TestMethod]
    /// <summary>
    ///   IsExtModeEnabled has constant value
    /// </summary>
    public void IsExtModeEnabledIsConstant()
    {
      bool refValue = mStatus.IsModeExtended;
      Assert.IsTrue(false == refValue);
    }

    #endregion

    #region Property IsInInitMode Test methods

    [TestMethod]
    /// <summary>
    ///   IsInInitMode has constant value
    /// </summary>
    public void IsInInitModeIsConstant()
    {
      bool refValue = mStatus.IsInInitMode;
      Assert.IsTrue(true == refValue);
    }

    #endregion

    #region Property IsListenOnly Test methods

    [TestMethod]
    /// <summary>
    ///   IsListenOnly has constant value
    /// </summary>
    public void IsListenOnlyIsConstant()
    {
      bool refValue = mStatus.IsListenOnly;
      Assert.IsTrue(false == refValue);
    }

    #endregion

    #region Property IsLowSpeedEnabled Test methods

    [TestMethod]
    /// <summary>
    ///   IsLowSpeedEnabled has constant value
    /// </summary>
    public void IsLowSpeedEnabledIsConstant()
    {
      bool refValue = mStatus.IsLowSpeedEnabled;
      Assert.IsTrue(false == refValue);
    }

    #endregion

    #region Property IsStdModeEnabled Test methods

    [TestMethod]
    /// <summary>
    ///   IsStdModeEnabled has constant value
    /// </summary>
    public void IsStdModeEnabledIsConstant()
    {
      bool refValue = mStatus.IsModeStandard;
      Assert.IsTrue(true == refValue);
    }

    #endregion

    #region Property IsTransmitPending Test methods

    [TestMethod]
    /// <summary>
    ///   IsTransmitPending has constant value
    /// </summary>
    public void IsTransmitPendingIsConstant()
    {
      bool refValue = mStatus.IsTransmitPending;
      Assert.IsTrue(false == refValue);
    }

    #endregion

    #region Property OperatingMode Test methods

    [TestMethod]
    /// <summary>
    ///   OperatingMode has constant value
    /// </summary>
    public void OperatingModeIsConstant()
    {
      CanOperatingModes refValue = mStatus.OperatingMode;
      Assert.IsTrue(CanOperatingModes.Standard == refValue);
    }

    #endregion

  }
}
