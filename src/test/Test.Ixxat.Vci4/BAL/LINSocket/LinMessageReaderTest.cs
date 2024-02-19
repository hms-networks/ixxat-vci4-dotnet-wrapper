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
  public class LinMessageReaderTest
    : VciDeviceTestBase
  {
    #region Member variables

    private const Byte mcInvalidPort = 0xFF;
    private Byte mLinPort;
    private Ixxat.Vci4.Bal.Lin.ILinMonitor mChannel;
    private Ixxat.Vci4.Bal.Lin.ILinMessageReader mReader;

    #endregion

    #region Test Initialize and Cleanup

    [TestInitialize]
    public void TestSetup()
    {
      Ixxat.Vci4.IVciDevice device = GetDevice();
      Ixxat.Vci4.Bal.IBalObject bal = device.OpenBusAccessLayer();

      mLinPort = mcInvalidPort;
      foreach (IBalResource resource in bal.Resources)
      {
        if (VciBusType.Lin == resource.BusType)
        {
          mLinPort = resource.BusPort;
          mChannel = bal.OpenSocket(mLinPort, typeof(Ixxat.Vci4.Bal.Lin.ILinMonitor)) as Ixxat.Vci4.Bal.Lin.ILinMonitor;
          mChannel.Initialize(10, false);
          mChannel.Activate();
          break;
        }
        resource.Dispose();
      }

      bal.Dispose();
      device.Dispose();

      if (mcInvalidPort == mLinPort)
      {
        // Test ignored because the device has no LIN controller
        Assert.Inconclusive();
      }

      mReader = mChannel.GetMessageReader();
    }

    [TestCleanup]
    public void TestCleanup()
    {
      if (null != mReader)
      {
        mReader.Dispose();
        mReader = null;
      }

      if (null != mChannel)
      {
        mChannel.Dispose();
        mChannel = null;
      }
    }

    #endregion

    #region Property Capacity Test methods

    [TestMethod]
    /// <summary>
    ///   Capacity has constant value
    /// </summary>
    public void CapacityIsConstant()
    {
      ushort refValue = mReader.Capacity;
      
      ushort testValue = mReader.Capacity;
      Assert.IsTrue(refValue == testValue);
    }

    [TestMethod]
    /// <summary>
    ///   Capacity must not throw ObjectDisposedException.
    /// </summary>
    public void CapacityMustNotThrowObjectDisposedException()
    {
      mReader.Dispose();
      ushort refValue = mReader.Capacity;
      Assert.IsTrue(0 == refValue);
    }

    #endregion

    #region Property FillCount Test methods

    [TestMethod]
    /// <summary>
    ///   FillCount has constant value
    /// </summary>
    public void FillCountIsConstant()
    {
      ushort refValue = mReader.FillCount;
      
      ushort testValue = mReader.FillCount;
      Assert.IsTrue(refValue == testValue);
    }

    [TestMethod]
    /// <summary>
    ///   FillCount must not throw ObjectDisposedException.
    /// </summary>
    public void FillCountMustNotThrowObjectDisposedException()
    {
      mReader.Dispose();
      ushort refValue = mReader.FillCount;
      Assert.IsTrue(0 == refValue);
    }

    #endregion

    #region Property Threshold Test methods

    [TestMethod]
    /// <summary>
    ///   Threshold has constant value
    /// </summary>
    public void ThresholdIsConstant()
    {
      ushort refValue = mReader.Threshold;
      
      ushort testValue = mReader.Threshold;
      Assert.IsTrue(refValue == testValue);
      refValue++;
      mReader.Threshold = refValue;
    }

    [TestMethod]
    /// <summary>
    ///   Threshold read must not throw ObjectDisposedException.
    /// </summary>
    public void ThresholdReadMustNotThrowObjectDisposedException()
    {
      mReader.Dispose();
      ushort refValue = mReader.Threshold;
      Assert.IsTrue(0 == refValue);
    }

    [TestMethod]
    /// <summary>
    ///   Threshold write must throw ObjectDisposedException.
    /// </summary>
    [ExpectedException(typeof(ObjectDisposedException))]
    public void ThresholdWriteMustThrowObjectDisposedException()
    {
      mReader.Dispose();
      mReader.Threshold = 5;
    }

    #endregion

    #region AssignEvent Test methods

    [TestMethod]
    /// <summary>
    ///   AssignEvent must not throw an Exception.
    /// </summary>
    public void AssignEventMustNotThrowException()
    {
      ManualResetEvent manualEvent = new ManualResetEvent(false);
      AutoResetEvent autoEvent = new AutoResetEvent(false);

      mReader.AssignEvent(autoEvent);
      mReader.AssignEvent(manualEvent);
    }

    [TestMethod]
    /// <summary>
    ///   AssignEvent (auto) must throw ObjectDisposedException.
    /// </summary>
    [ExpectedException(typeof(ObjectDisposedException))]
    public void AssignEventAutoMustThrowObjectDisposedException()
    {
      mReader.Dispose();

      AutoResetEvent changeEvent = new AutoResetEvent(false);
      mReader.AssignEvent(changeEvent);
    }

    [TestMethod]
    /// <summary>
    ///   AssignEvent (manual) must throw ObjectDisposedException.
    /// </summary>
    [ExpectedException(typeof(ObjectDisposedException))]
    public void AssignEventManualMustThrowObjectDisposedException()
    {
      mReader.Dispose();

      ManualResetEvent changeEvent = new ManualResetEvent(false);
      mReader.AssignEvent(changeEvent);
    }

    #endregion

    #region ReadMessage Test methods

    /// <summary>
    ///   ReadMessage returns false
    /// </summary>
    [TestMethod]
    public void ReadMessageReturnsFalse()
    {
      ILinMessage message;
      Assert.IsFalse(mReader.ReadMessage(out message));
    }

    [TestMethod]
    /// <summary>
    ///   ReadMessage must throw ObjectDisposedException.
    /// </summary>
    [ExpectedException(typeof(ObjectDisposedException))]
    public void ReadMessageMustThrowObjectDisposedException()
    {
      mReader.Dispose();
      ILinMessage message;
      mReader.ReadMessage(out message);
    }

    #endregion

    #region ReadMessages Test methods

    [TestMethod]
    /// <summary>
    ///   ReadMessages returns zero
    /// </summary>
    public void ReadMessagesReturnsZero()
    {
      ILinMessage[] messages = new ILinMessage[5];
      
      Assert.IsTrue(0 == mReader.ReadMessages(out messages));
    }

    [TestMethod]
    /// <summary>
    ///   ReadMessages must throw ObjectDisposedException.
    /// </summary>
    [ExpectedException(typeof(ObjectDisposedException))]
    public void ReadMessagesMustThrowObjectDisposedException()
    {
      mReader.Dispose();
      ILinMessage[] messages = new ILinMessage[5];
      mReader.ReadMessages(out messages);
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
      mReader.Dispose();
      mReader = null;

      ILinMessageReader reader;
      using (reader = mChannel.GetMessageReader())
      {
        reader.AssignEvent(new AutoResetEvent(false));
      }

      // This call must throw an ObjectDisposedException
      reader.AssignEvent(new AutoResetEvent(false));
    }

    #endregion

  }
}
