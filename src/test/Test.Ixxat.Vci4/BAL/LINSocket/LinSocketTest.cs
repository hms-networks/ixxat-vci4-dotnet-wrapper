using System;
using System.Collections;
using System.Text;
using System.Threading;
using Ixxat.Vci4;
using Ixxat.Vci4.Bal;
using Ixxat.Vci4.Bal.Lin;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Vci4Tests
{
  [TestClass]
  public class LinSocketTest
    : VciDeviceTestBase
  {
    #region Member variables

    private const Byte mcInvalidPort = 0xFF;
    private Byte mLinPort;
    private Ixxat.Vci4.Bal.Lin.ILinSocket mSocket;
    private Ixxat.Vci4.Bal.IBalObject mBal;

    #endregion

    #region Test Initialize and Cleanup

    [TestInitialize]
    public void TestSetup()
    {
      Ixxat.Vci4.IVciDevice device = GetDevice();
      mBal = device.OpenBusAccessLayer();

      mLinPort = mcInvalidPort;
      foreach(IBalResource resource in mBal.Resources)
      {
        if (VciBusType.Lin == resource.BusType)
        {
          mLinPort = resource.BusPort;
          break;
        }
        resource.Dispose();
      }

      device.Dispose();

      if (mcInvalidPort == mLinPort)
      {
        // Test ignored because the device has no LIN controller
        Assert.Inconclusive();
      }

      mSocket = mBal.OpenSocket(mLinPort, typeof(Ixxat.Vci4.Bal.Lin.ILinSocket)) as Ixxat.Vci4.Bal.Lin.ILinSocket;
    }

    [TestCleanup]
    public void TestCleanup()
    {
      if (null != mSocket)
      {
        mSocket.Dispose();
        mSocket = null;
      }

      if (null != mBal)
      {
        mBal.Dispose();
        mBal = null;
      }
    }

    #endregion

    #region Property ClockFrequency Test methods

    [TestMethod]
    /// <summary>
    ///   ClockFrequency has constant value
    /// </summary>
    public void ClockFrequencyIsConstant()
    {
      uint refValue = mSocket.ClockFrequency;
      
      uint testValue = mSocket.ClockFrequency;
      Assert.IsTrue(refValue == testValue);
    }

    [TestMethod]
    /// <summary>
    ///   ClockFrequency must throw ObjectDisposedException.
    /// </summary>
    [ExpectedException(typeof(ObjectDisposedException))]
    public void ClockFrequencyMustThrowObjectDisposedException()
    {
      mSocket.Dispose();
      uint refValue = mSocket.ClockFrequency;
    }

    #endregion

    #region Property TimeStampCounterDivisor Test methods

    [TestMethod]
    /// <summary>
    ///   TimeStampCounterDivisor has constant value
    /// </summary>
    public void TimeStampCounterDivisorIsConstant()
    {
      uint refValue = mSocket.TimeStampCounterDivisor;
      
      uint testValue = mSocket.TimeStampCounterDivisor;
      Assert.IsTrue(refValue == testValue);
    }

    [TestMethod]
    /// <summary>
    ///   TimeStampCounterDivisor must throw ObjectDisposedException.
    /// </summary>
    [ExpectedException(typeof(ObjectDisposedException))]
    public void TimeStampCounterDivisorMustThrowObjectDisposedException()
    {
      mSocket.Dispose();
      uint refValue = mSocket.TimeStampCounterDivisor;
    }

    #endregion

    #region Property Features Test methods

    [TestMethod]
    /// <summary>
    ///   Features has constant value
    /// </summary>
    public void FeaturesIsConstant()
    {
      LinFeatures refValue = mSocket.Features;
      
      LinFeatures testValue = mSocket.Features;
      Assert.IsTrue(refValue == testValue);
    }

    [TestMethod]
    /// <summary>
    ///   Features must throw ObjectDisposedException.
    /// </summary>
    [ExpectedException(typeof(ObjectDisposedException))]
    public void FeaturesMustThrowObjectDisposedException()
    {
      mSocket.Dispose();
      LinFeatures refValue = mSocket.Features;
    }

    #endregion

    #region Property LineStatus Test methods

    [TestMethod]
    /// <summary>
    ///   LineStatus has constant value
    /// </summary>
    public void LineStatusIsConstant()
    {
      LinLineStatus refValue = mSocket.LineStatus;

      LinLineStatus testValue = mSocket.LineStatus;
      Assert.IsTrue(refValue.OperatingMode == testValue.OperatingMode);
      Assert.IsTrue(refValue.Bitrate == testValue.Bitrate);
    }

    [TestMethod]
    /// <summary>
    ///   LineStatus must throw ObjectDisposedException.
    /// </summary>
    [ExpectedException(typeof(ObjectDisposedException))]
    public void LineStatusMustThrowObjectDisposedException()
    {
      mSocket.Dispose();
      LinLineStatus refValue = mSocket.LineStatus;
    }

    #endregion

    #region Property SupportsAutorate Test methods

    [TestMethod]
    /// <summary>
    ///   SupportsAutorate has constant value
    /// </summary>
    public void SupportsAutorateIsConstant()
    {
      bool refValue = mSocket.SupportsAutorate;
      
      bool testValue = mSocket.SupportsAutorate;
      Assert.IsTrue(refValue == testValue);
    }

    [TestMethod]
    /// <summary>
    ///   SupportsAutorate must throw ObjectDisposedException.
    /// </summary>
    [ExpectedException(typeof(ObjectDisposedException))]
    public void SupportsAutorateMustThrowObjectDisposedException()
    {
      mSocket.Dispose();
      bool refValue = mSocket.SupportsAutorate;
    }

    #endregion

    #region Property SupportsBusLoadComputation Test methods

    [TestMethod]
    /// <summary>
    ///   SupportsBusLoadComputation has constant value
    /// </summary>
    public void SupportsBusLoadComputationIsConstant()
    {
      bool refValue = mSocket.SupportsBusLoadComputation;
      
      bool testValue = mSocket.SupportsBusLoadComputation;
      Assert.IsTrue(refValue == testValue);
    }

    [TestMethod]
    /// <summary>
    ///   SupportsBusLoadComputation must throw ObjectDisposedException.
    /// </summary>
    [ExpectedException(typeof(ObjectDisposedException))]
    public void SupportsBusLoadComputationMustThrowObjectDisposedException()
    {
      mSocket.Dispose();
      bool refValue = mSocket.SupportsBusLoadComputation;
    }

    #endregion

    #region Property SupportsErrorFrames Test methods

    [TestMethod]
    /// <summary>
    ///   SupportsErrorFrames has constant value
    /// </summary>
    public void SupportsErrorFramesIsConstant()
    {
      bool refValue = mSocket.SupportsErrorFrames;
      
      bool testValue = mSocket.SupportsErrorFrames;
      Assert.IsTrue(refValue == testValue);
    }

    [TestMethod]
    /// <summary>
    ///   SupportsErrorFrames must throw ObjectDisposedException.
    /// </summary>
    [ExpectedException(typeof(ObjectDisposedException))]
    public void SupportsErrorFramesMustThrowObjectDisposedException()
    {
      mSocket.Dispose();
      bool refValue = mSocket.SupportsErrorFrames;
    }

    #endregion

    #region Property SupportsMaterMode Test methods

    [TestMethod]
    /// <summary>
    ///   SupportsMasterMode has constant value
    /// </summary>
    public void SupportsMasterModeIsConstant()
    {
      bool refValue = mSocket.SupportsMasterMode;
      
      bool testValue = mSocket.SupportsMasterMode;
      Assert.IsTrue(refValue == testValue);
    }

    [TestMethod]
    /// <summary>
    ///   SupportsMasterMode must throw ObjectDisposedException.
    /// </summary>
    [ExpectedException(typeof(ObjectDisposedException))]
    public void SupportsMasterModeMustThrowObjectDisposedException()
    {
      mSocket.Dispose();
      bool refValue = mSocket.SupportsMasterMode;
    }

    #endregion

    #region Using Statement Test methods

    [TestMethod]
    /// <summary>
    ///   Tests compilation and functionality of using statement
    /// </summary>
    [ExpectedException(typeof(ObjectDisposedException))]
    public void TestUsingStatement()
    {
      LinFeatures features;
      ILinSocket socket;
      using (socket = mBal.OpenSocket(mLinPort, typeof(Ixxat.Vci4.Bal.Lin.ILinSocket)) as Ixxat.Vci4.Bal.Lin.ILinSocket)
      {
        features = socket.Features;
      }

      // This call must throw an ObjectDisposedException
      features = socket.Features;
    }

    #endregion
  }
}
