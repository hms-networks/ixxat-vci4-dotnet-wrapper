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
  class CanChannelTest2
    : VciDeviceTestBase
  {
    #region Member variables

    private Ixxat.Vci4.Bal.Can.ICanChannel2? mSocket;
    private Ixxat.Vci4.Bal.IBalObject? mBal;

    #endregion

    #region Test Initialize and Cleanup

    [TestInitialize]
    public void TestSetup()
    {
      Ixxat.Vci4.IVciDevice? device = GetDevice();
      mBal = device!.OpenBusAccessLayer();

      device!.Dispose();

      mSocket = mBal!.OpenSocket(0, typeof(Ixxat.Vci4.Bal.Can.ICanChannel2)) as Ixxat.Vci4.Bal.Can.ICanChannel2;
    }

    [TestCleanup]
    public void TestCleanup()
    {
      if (null != mSocket)
      {
        mSocket!.Dispose();
        mSocket = null;
      }
      if (null != mBal)
      {
        mBal!.Dispose();
        mBal = null;
      }
    }

    #endregion

    #region Property ChannelStatus Test methods

    [TestMethod]
    /// <summary>
    ///   ChannelStatus before initialization
    /// </summary>
    [ExpectedException(typeof(ObjectDisposedException))]
    public void ChannelStatusBeforeInit()
    {
      CanChannelStatus refValue = mSocket!.ChannelStatus;
    }

    [TestMethod]
    /// <summary>
    ///   ChannelStatus after initialization
    /// </summary>
    public void ChannelStatusAfterInit()
    {
      mSocket!.Initialize(100, 100, 1, CanFilterModes.Pass, false);
      CanChannelStatus refValue = mSocket!.ChannelStatus;

      CanChannelStatus testValue = mSocket!.ChannelStatus;
      Assert.IsTrue(refValue.IsActivated == testValue.IsActivated);
    }

    [TestMethod]
    /// <summary>
    ///   ChannelStatus must throw ObjectDisposedException.
    /// </summary>
    [ExpectedException(typeof(ObjectDisposedException))]
    public void ChannelStatusMustThrowObjectDisposedException()
    {
      mSocket!.Dispose();
      CanChannelStatus refValue = mSocket!.ChannelStatus;
    }

    #endregion

    #region Initialize Test methods

    [TestMethod]
    /// <summary>
    ///   Valid Initialize calls
    /// </summary>
    public void InitializeValidCalls()
    {
      mSocket!.Initialize(100, 100, 1, CanFilterModes.Pass, false);
      mSocket!.Initialize(100, 100, 1, CanFilterModes.Pass, true);
    }

    [TestMethod]
    /// <summary>
    ///   Initialize with rxFifoSize 0
    /// </summary>
    [ExpectedException(typeof(VciException))]
    public void InitializeWithRxFifoSize0()
    {
      mSocket!.Initialize(0, 100, 1, CanFilterModes.Pass, false);
    }

    [TestMethod]
    /// <summary>
    ///   Initialize with txFifoSize 0
    /// </summary>
    public void InitializeWithTxFifoSize0()
    {
      mSocket!.Initialize(100, 0, 1, CanFilterModes.Pass, false);
      Assert.IsTrue(true);
    }

    [TestMethod]
    /// <summary>
    ///   Initialize with txFifoSize 0 and try to get writer
    /// </summary>
    [ExpectedException(typeof(VciException))]
    public void InitializeWithTxFifoSize0AndGetWriter()
    {
      mSocket!.Initialize(100, 0, 1, CanFilterModes.Pass, false);
      ICanMessageWriter writer = mSocket!.GetMessageWriter();
    }

    [TestMethod]
    /// <summary>
    ///   Initialize must throw ObjectDisposedException.
    /// </summary>
    [ExpectedException(typeof(ObjectDisposedException))]
    public void InitializeMustThrowObjectDisposedException()
    {
      mSocket!.Dispose();
      mSocket!.Initialize(100, 100, 1, CanFilterModes.Pass, false);
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
      mSocket!.Activate();
    }

    [TestMethod]
    /// <summary>
    ///   Activate valid calls
    /// </summary>
    public void ActivateValidCalls()
    {
      mSocket!.Initialize(100, 100, 1, CanFilterModes.Pass, false);

      mSocket!.Activate();
    }

    [TestMethod]
    /// <summary>
    ///   Activate second exlusive channel
    /// </summary>
    public void ActivateSecondExclusive()
    {
      mSocket!.Initialize(100, 100, 1, CanFilterModes.Pass, true);
      mSocket!.Activate();

      Ixxat.Vci4.Bal.Can.ICanChannel2? socket2;
      socket2 = mBal!.OpenSocket(0, typeof(Ixxat.Vci4.Bal.Can.ICanChannel2)) as Ixxat.Vci4.Bal.Can.ICanChannel2;
      try
      {
        socket2!.Initialize(100, 100, 1, CanFilterModes.Pass, true);
        socket2!.Activate();
        Assert.IsTrue(false);
      }
      catch (VciException)
      {
      }

      socket2!.Dispose();
    }

    [TestMethod]
    /// <summary>
    ///   Activate must throw ObjectDisposedException.
    /// </summary>
    [ExpectedException(typeof(ObjectDisposedException))]
    public void ActivateMustThrowObjectDisposedException()
    {
      mSocket!.Dispose();
      mSocket!.Activate();
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
      mSocket!.Deactivate();
    }

    [TestMethod]
    /// <summary>
    ///   Deactivate valid calls
    /// </summary>
    public void DeactivateValidCalls()
    {
      mSocket!.Initialize(100, 100, 1, CanFilterModes.Pass, false);

      mSocket!.Deactivate();
    }

    [TestMethod]
    /// <summary>
    ///   Deactivate must throw ObjectDisposedException.
    /// </summary>
    [ExpectedException(typeof(ObjectDisposedException))]
    public void DeactivateMustThrowObjectDisposedException()
    {
      mSocket!.Dispose();
      mSocket!.Deactivate();
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
      ICanMessageReader reader = mSocket!.GetMessageReader();
    }

    [TestMethod]
    /// <summary>
    ///   GetMessageReader valid calls
    /// </summary>
    public void GetMessageReaderValidCalls()
    {
      mSocket!.Initialize(100, 100, 1, CanFilterModes.Pass, false);
      
      ICanMessageReader reader = mSocket!.GetMessageReader();
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
      mSocket!.Dispose();
      ICanMessageReader reader = mSocket!.GetMessageReader();
    }

    #endregion

    #region GetMessageWriter Test methods

    [TestMethod]
    /// <summary>
    ///   GetMessageWriter before initialization
    /// </summary>
    [ExpectedException(typeof(ObjectDisposedException))]
    public void GetMessageWriterBeforeInit()
    {
      ICanMessageWriter reader = mSocket!.GetMessageWriter();
    }

    [TestMethod]
    /// <summary>
    ///   GetMessageWriter valid calls
    /// </summary>
    public void GetMessageWriterValidCalls()
    {
      mSocket!.Initialize(100, 100, 1, CanFilterModes.Pass, false);
      
      ICanMessageWriter reader = mSocket!.GetMessageWriter();
      Assert.IsNotNull(reader);
      reader.Dispose();
    }

    [TestMethod]
    /// <summary>
    ///   GetMessageWriter must throw ObjectDisposedException.
    /// </summary>
    [ExpectedException(typeof(ObjectDisposedException))]
    public void GetMessageWriterMustThrowObjectDisposedException()
    {
      mSocket!.Dispose();
      ICanMessageWriter reader = mSocket!.GetMessageWriter();
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
      mSocket!.Dispose();
      mSocket = null;

      ICanChannel2? channel;
      using (channel = mBal!.OpenSocket(0, typeof(Ixxat.Vci4.Bal.Can.ICanChannel2)) as Ixxat.Vci4.Bal.Can.ICanChannel2)
      {
        channel!.Deactivate();
      }

      // This call must throw an ObjectDisposedException
      channel!.Deactivate();
    }

    #endregion

  }
}
