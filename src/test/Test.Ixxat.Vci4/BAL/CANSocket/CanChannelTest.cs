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
  public class CanChannelTest
    : VciDeviceTestBase
  {
    #region Member variables

    private Ixxat.Vci4.Bal.Can.ICanChannel? mSocket;
    private Ixxat.Vci4.Bal.IBalObject? mBal;

    #endregion

    #region Test Initialize and Cleanup

    [TestInitialize]
    public void TestSetup()
    {
      Ixxat.Vci4.IVciDevice? device = GetDevice();
      mBal = device!.OpenBusAccessLayer();

      device!.Dispose();

      mSocket = mBal!.OpenSocket(0, typeof(Ixxat.Vci4.Bal.Can.ICanChannel)) as Ixxat.Vci4.Bal.Can.ICanChannel;
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

    /// <summary>
    ///   ChannelStatus after initialization
    /// </summary>
    [TestMethod]
    public void ChannelStatusAfterInit()
    {
      mSocket!.Initialize(100, 100, false);
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
      mSocket!.Initialize(100, 100, false);
      mSocket!.Initialize(100, 100, true);
    }

    [TestMethod]
    /// <summary>
    ///   Initialize with rxFifoSize 0
    /// </summary>
    [ExpectedException(typeof(VciException))]
    public void InitializeWithRxFifoSize0()
    {
      mSocket!.Initialize(0, 100, false);
    }

    [TestMethod]
    /// <summary>
    ///   Initialize with txFifoSize 0
    /// </summary>
    public void InitializeWithTxFifoSize0()
    {
      mSocket!.Initialize(100, 0, false);
      Assert.IsTrue(true);
    }

    [TestMethod]
    /// <summary>
    ///   Initialize with txFifoSize 0 and try to get writer
    /// </summary>
    [ExpectedException(typeof(VciException))]
    public void InitializeWithTxFifoSize0AndGetWriter()
    {
      mSocket!.Initialize(100, 0, false);
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
      mSocket!.Initialize(100, 100, false);
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

    /// <summary>
    ///   Activate valid calls
    /// </summary>
    [TestMethod]
    public void ActivateValidCalls()
    {
      mSocket!.Initialize(100, 100, false);

      mSocket!.Activate();
    }

    [TestMethod]
    /// <summary>
    ///   Activate second exlusive channel
    /// </summary>
    public void ActivateSecondExclusive()
    {
      mSocket!.Initialize(100, 100, true);
      mSocket!.Activate();

      Ixxat.Vci4.Bal.Can.ICanChannel? socket2;
      socket2 = mBal!.OpenSocket(0, typeof(Ixxat.Vci4.Bal.Can.ICanChannel)) as Ixxat.Vci4.Bal.Can.ICanChannel;
      try
      {
        socket2!.Initialize(100, 100, true);
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
      mSocket!.Initialize(100, 100, false);

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
      mSocket!.Initialize(100, 100, false);

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
      mSocket!.Initialize(100, 100, false);

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

      ICanChannel? channel;
      using (channel = mBal!.OpenSocket(0, typeof(Ixxat.Vci4.Bal.Can.ICanChannel)) as Ixxat.Vci4.Bal.Can.ICanChannel)
      {
        channel!.Deactivate();
      }

      // This call must throw an ObjectDisposedException
      channel!.Deactivate();
    }

    #endregion

  }
}
