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
  class CanChannelStatusTest
    : VciDeviceTestBase
  {
    #region Member variables

    private CanChannelStatus mStatus;

    #endregion

    #region Test Initialize and Cleanup

    /// <summary>
    ///   helper method to clear the RxFifo
    ///   (Test preparations)
    /// </summary>
    public void ClearRxFifo(Ixxat.Vci4.Bal.Can.ICanMessageReader mReader)
    {
      // clear the receive message queue
      ICanMessage rxMessage;

      while (mReader!.ReadMessage(out rxMessage))
      {
        // if (MustTerminate)
        // { return; }
      };
    }

    /// <summary>
    ///   Method that's called before execution of the first test. 
    ///   (Test preparations)
    /// </summary>
    public void TestFixtureSetup()
    {
      Ixxat.Vci4.IVciDevice? device = GetDevice();
      Ixxat.Vci4.Bal.IBalObject? bal;
      Ixxat.Vci4.Bal.Can.ICanChannel? socket;
      Ixxat.Vci4.Bal.Can.ICanMessageReader? reader;

      bal = device!.OpenBusAccessLayer();
      socket = bal!.OpenSocket(0, typeof(Ixxat.Vci4.Bal.Can.ICanChannel)) as Ixxat.Vci4.Bal.Can.ICanChannel;
      socket!.Initialize(10, 10, false);
      socket!.Activate();

      reader = socket!.GetMessageReader();
      ClearRxFifo(reader);

      mStatus = socket!.ChannelStatus;

      reader!.Dispose();
      socket!.Dispose();
      bal!.Dispose();
      device!.Dispose();
    }

    #endregion

    #region Property HasFifoOverrun Test methods

    [TestMethod]
    /// <summary>
    ///   HasFifoOverrun has constant value
    /// </summary>
    public void HasFifoOverrunIsConstant()
    {
      bool refValue = mStatus.HasFifoOverrun;
      Assert.IsTrue(false == refValue);
    }

    #endregion

    #region Property IsActivated Test methods

    [TestMethod]
    /// <summary>
    ///   IsActivated has constant value
    /// </summary>
    public void IsActivatedIsConstant()
    {
      bool refValue = mStatus.IsActivated;
      Assert.IsTrue(true == refValue);
    }

    #endregion

    #region Property ReceiveFifoLoad Test methods

    [TestMethod]
    /// <summary>
    ///   ReceiveFifoLoad has constant value
    /// </summary>
    public void ReceiveFifoLoadIsConstant()
    {
      byte refValue = mStatus.ReceiveFifoLoad;

      byte bValue = mStatus.ReceiveFifoLoad;
      Assert.IsTrue(bValue == refValue);
    }

    #endregion

    #region Property TransmitFifoLoad Test methods

    [TestMethod]
    /// <summary>
    ///   TransmitFifoLoad has constant value
    /// </summary>
    public void TransmitFifoLoadIsConstant()
    {
      byte refValue = mStatus.TransmitFifoLoad;
      Assert.IsTrue(0 == refValue);
    }

    #endregion

  }
}
