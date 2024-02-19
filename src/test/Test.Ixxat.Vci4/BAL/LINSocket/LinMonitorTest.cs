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
  public class LinMonitorTest
    : VciDeviceTestBase
  {
    #region Member variables

    private const Byte mcInvalidPort = 0xFF;
    private Byte mLinPort;
    private Ixxat.Vci4.Bal.Lin.ILinMonitor mMonitor;
    private Ixxat.Vci4.Bal.IBalObject mBal;

    #endregion

    #region Test Initialize and Cleanup

    [TestInitialize]
    public void TestSetup()
    {
      Ixxat.Vci4.IVciDevice device = GetDevice();
      mBal = device.OpenBusAccessLayer();

      mLinPort = mcInvalidPort;
      foreach (IBalResource resource in mBal.Resources)
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

      mMonitor = mBal.OpenSocket(mLinPort, typeof(Ixxat.Vci4.Bal.Lin.ILinMonitor)) as Ixxat.Vci4.Bal.Lin.ILinMonitor;
    }

    [TestCleanup]
    public void TestCleanup()
    {
      if (null != mMonitor)
      {
        mMonitor.Dispose();
        mMonitor = null;
      }

      if (null != mBal)
      {
        mBal.Dispose();
        mBal = null;
      }
    }

    #endregion

    #region Property MonitorStatus Test methods

    [TestMethod]
    /// <summary>
    ///   MonitorStatus before initialization
    /// </summary>
    [ExpectedException(typeof(ObjectDisposedException))]
    public void MonitorStatusBeforeInit()
    {
      LinMonitorStatus refValue = mMonitor.MonitorStatus;
    }

    [TestMethod]
    /// <summary>
    ///   MonitorStatus after initialization
    /// </summary>
    public void MonitorStatusAfterInit()
    {
      mMonitor.Initialize(100, false);
      LinMonitorStatus refValue = mMonitor.MonitorStatus;
      
      LinMonitorStatus testValue = mMonitor.MonitorStatus;
      Assert.IsTrue(refValue.IsActivated == testValue.IsActivated);
    }

    [TestMethod]
    /// <summary>
    ///   MonitorStatus must throw ObjectDisposedException.
    /// </summary>
    [ExpectedException(typeof(ObjectDisposedException))]
    public void MonitorStatusMustThrowObjectDisposedException()
    {
      mMonitor.Dispose();
      LinMonitorStatus refValue = mMonitor.MonitorStatus;
    }

    #endregion

    #region Initialize Test methods

    [TestMethod]
    /// <summary>
    ///   Valid Initialize calls
    /// </summary>
    public void InitializeValidCalls()
    {
      mMonitor.Initialize(100, false);
      mMonitor.Initialize(100, true);
    }

    [TestMethod]
    /// <summary>
    ///   Initialize with rxFifoSize 0
    /// </summary>
    [ExpectedException(typeof(VciException))]
    public void InitializeWithRxFifoSize0()
    {
      mMonitor.Initialize(0, false);
    }

    [TestMethod]
    /// <summary>
    ///   Initialize must throw ObjectDisposedException.
    /// </summary>
    [ExpectedException(typeof(ObjectDisposedException))]
    public void InitializeMustThrowObjectDisposedException()
    {
      mMonitor.Dispose();
      mMonitor.Initialize(100, false);
    }

    #endregion

    #region Activate Test methods

    [TestMethod]
    /// <summary>
    ///   Activate before initialization
    /// </summary>
    [ExpectedException(typeof(ObjectDisposedException))]
    public void ActivateBeforeInit()
    {
      mMonitor.Activate();
    }

    [TestMethod]
    /// <summary>
    ///   Activate valid calls
    /// </summary>
    public void ActivateValidCalls()
    {
      mMonitor.Initialize(100, false);

      mMonitor.Activate();
    }

    [TestMethod]
    /// <summary>
    ///   Activate second exlusive channel
    /// </summary>
    public void ActivateSecondExclusive()
    {
      mMonitor.Initialize(100, true);
      mMonitor.Activate();

      Ixxat.Vci4.Bal.Lin.ILinMonitor socket2;
      socket2 = mBal.OpenSocket(mLinPort, typeof(Ixxat.Vci4.Bal.Lin.ILinMonitor)) as Ixxat.Vci4.Bal.Lin.ILinMonitor;
      try
      {
        socket2.Initialize(100, true);
        socket2.Activate();
        Assert.IsTrue(false);
      }
      catch (VciException)
      {
      }

      socket2.Dispose();
    }

    [TestMethod]
    /// <summary>
    ///   Activate must throw ObjectDisposedException.
    /// </summary>
    [ExpectedException(typeof(ObjectDisposedException))]
    public void ActivateMustThrowObjectDisposedException()
    {
      mMonitor.Dispose();
      mMonitor.Activate();
    }

    #endregion

    #region Deactivate Test methods

    [TestMethod]
    /// <summary>
    ///   Deactivate before initialization
    /// </summary>
    [ExpectedException(typeof(ObjectDisposedException))]
    public void DeactivateBeforeInit()
    {
      mMonitor.Deactivate();
    }

    [TestMethod]
    /// <summary>
    ///   Deactivate valid calls
    /// </summary>
    public void DeactivateValidCalls()
    {
      mMonitor.Initialize(100, false);
      
      mMonitor.Deactivate();
    }

    [TestMethod]
    /// <summary>
    ///   Deactivate must throw ObjectDisposedException.
    /// </summary>
    [ExpectedException(typeof(ObjectDisposedException))]
    public void DeactivateMustThrowObjectDisposedException()
    {
      mMonitor.Dispose();
      mMonitor.Deactivate();
    }

    #endregion

    #region GetMessageReader Test methods

    [TestMethod]
    /// <summary>
    ///   GetMessageReader before initialization
    /// </summary>
    [ExpectedException(typeof(ObjectDisposedException))]
    public void GetMessageReaderBeforeInit()
    {
      ILinMessageReader reader = mMonitor.GetMessageReader();
    }

    [TestMethod]
    /// <summary>
    ///   GetMessageReader valid calls
    /// </summary>
    public void GetMessageReaderValidCalls()
    {
      mMonitor.Initialize(100, false);

      ILinMessageReader reader = mMonitor.GetMessageReader();
      Assert.IsNotNull(reader);
      reader.Dispose();
    }

    [TestMethod]
    /// <summary>
    ///   GetMessageReader must throw ObjectDisposedException.
    /// </summary>
    [ExpectedException(typeof(ObjectDisposedException))]
    public void GetMessageReaderMustThrowObjectDisposedException()
    {
      mMonitor.Dispose();
      ILinMessageReader reader = mMonitor.GetMessageReader();
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
      mMonitor.Dispose();
      mMonitor = null;

      ILinMonitor channel;
      using (channel = mBal.OpenSocket(mLinPort, typeof(Ixxat.Vci4.Bal.Lin.ILinMonitor)) as Ixxat.Vci4.Bal.Lin.ILinMonitor)
      {
        channel.Deactivate();
      }

      // This call must throw an ObjectDisposedException
      channel.Deactivate();
    }

    #endregion
  }
}
