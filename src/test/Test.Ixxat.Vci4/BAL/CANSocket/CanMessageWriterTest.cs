using System;
using System.Collections;
using System.Text;
using System.Threading;
using Ixxat.Vci4;
using Ixxat.Vci4.Bal;
using Ixxat.Vci4.Bal.Can;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Vci4Tests
{
  [TestClass]
  public class CanMessageWriterTest
    : VciDeviceTestBase
  {
    #region Member variables

    private Ixxat.Vci4.Bal.Can.ICanChannel mSocket;
    private Ixxat.Vci4.Bal.Can.ICanMessageWriter mWriter;

    #endregion

    #region Test Initialize and Cleanup

    [TestInitialize]
    public void TestSetup()
    {
      Ixxat.Vci4.IVciDevice device = GetDevice();
      Ixxat.Vci4.Bal.IBalObject bal = device.OpenBusAccessLayer();

      mSocket = bal.OpenSocket(0, typeof(Ixxat.Vci4.Bal.Can.ICanChannel)) as Ixxat.Vci4.Bal.Can.ICanChannel;
      mSocket.Initialize(10, 10, false);
      mSocket.Activate();

      bal.Dispose();
      device.Dispose();

      mWriter = mSocket.GetMessageWriter();
    }

    [TestCleanup]
    public void TestCleanup()
    {
      if (null != mWriter)
      {
        mWriter.Dispose();
        mWriter = null;
      }

      if (null != mSocket)
      {
        mSocket.Dispose();
        mSocket = null;
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
      ushort refValue = mWriter.Capacity;
      
      ushort testValue = mWriter.Capacity;
      Assert.IsTrue(refValue == testValue);
    }

    [TestMethod]
    /// <summary>
    ///   Capacity must not throw ObjectDisposedException.
    /// </summary>
    public void CapacityMustNotThrowObjectDisposedException()
    {
      mWriter.Dispose();
      ushort refValue = mWriter.Capacity;
      Assert.IsTrue(0 == refValue);
    }

    #endregion

    #region Property FreeCount Test methods

    [TestMethod]
    /// <summary>
    ///   FreeCount has constant value
    /// </summary>
    public void FreeCountIsConstant()
    {
      ushort refValue = mWriter.FreeCount;
      
      ushort testValue = mWriter.FreeCount;
      Assert.IsTrue(refValue == testValue);
    }

    [TestMethod]
    /// <summary>
    ///   FreeCount must not throw ObjectDisposedException.
    /// </summary>
    public void FreeCountMustNotThrowObjectDisposedException()
    {
      mWriter.Dispose();
      ushort refValue = mWriter.FreeCount;
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
      ushort refValue = mWriter.Threshold;
      
      ushort testValue = mWriter.Threshold;
      Assert.IsTrue(refValue == testValue);
      refValue++;
      mWriter.Threshold = refValue;
    }

    [TestMethod]
    /// <summary>
    ///   Threshold read must not throw ObjectDisposedException.
    /// </summary>
    public void ThresholdReadMustNotThrowObjectDisposedException()
    {
      mWriter.Dispose();
      ushort refValue = mWriter.Threshold;
      Assert.IsTrue(0 == refValue);
    }

    [TestMethod]
    /// <summary>
    ///   Threshold write must throw ObjectDisposedException.
    /// </summary>
    [ExpectedException(typeof(ObjectDisposedException))]
    public void ThresholdWriteMustThrowObjectDisposedException()
    {
      mWriter.Dispose();
      mWriter.Threshold = 5;
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

      mWriter.AssignEvent(autoEvent);
      mWriter.AssignEvent(manualEvent);
    }

    [TestMethod]
    /// <summary>
    ///   AssignEvent (auto) must throw ObjectDisposedException.
    /// </summary>
    [ExpectedException(typeof(ObjectDisposedException))]
    public void AssignEventAutoMustThrowObjectDisposedException()
    {
      mWriter.Dispose();

      AutoResetEvent changeEvent = new AutoResetEvent(false);
      mWriter.AssignEvent(changeEvent);
    }

    [TestMethod]
    /// <summary>
    ///   AssignEvent (manual) must throw ObjectDisposedException.
    /// </summary>
    [ExpectedException(typeof(ObjectDisposedException))]
    public void AssignEventManualMustThrowObjectDisposedException()
    {
      mWriter.Dispose();

      ManualResetEvent changeEvent = new ManualResetEvent(false);
      mWriter.AssignEvent(changeEvent);
    }

    #endregion

    #region SendMessage Test methods

    [TestMethod]
    /// <summary>
    ///   SendMessage returns true
    /// </summary>
    public void SendMessageReturnsTrue()
    {
      ICanMessage msg;
      IMessageFactory factory = VciServer.Instance().MsgFactory;
      msg = (ICanMessage)factory.CreateMsg(typeof(ICanMessage));

      Assert.IsTrue(mWriter.SendMessage(msg));
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
      mWriter.Dispose();
      mWriter = null;

      ICanMessageWriter writer;
      using (writer = mSocket.GetMessageWriter())
      {
        writer.AssignEvent(new AutoResetEvent(false));
      }

      // This call must throw an ObjectDisposedException
      writer.AssignEvent(new AutoResetEvent(false));
    }

    #endregion

  }
}
