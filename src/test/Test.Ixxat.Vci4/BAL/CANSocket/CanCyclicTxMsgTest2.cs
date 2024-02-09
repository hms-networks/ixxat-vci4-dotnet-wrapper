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
  public class CanCyclicTxMsgTest2
    : VciDeviceTestBase
  {
    #region Member variables

    private Ixxat.Vci4.Bal.Can.ICanScheduler2? mScheduler;
    private Ixxat.Vci4.Bal.Can.ICanControl? mControl;

    #endregion

    #region Test Initialize and Cleanup

    [TestInitialize]
    public void TestSetup()
    {
      Ixxat.Vci4.IVciDevice? device = GetDevice();
      Ixxat.Vci4.Bal.IBalObject bal = device!.OpenBusAccessLayer();

      try
      {
        mControl = bal!.OpenSocket(0, typeof(Ixxat.Vci4.Bal.Can.ICanControl)) as Ixxat.Vci4.Bal.Can.ICanControl;
        mControl!.InitLine(CanOperatingModes.Standard | CanOperatingModes.Extended, CanBitrate.Cia500KBit);
        mControl!.StartLine();

        mScheduler = bal!.OpenSocket(0, typeof(Ixxat.Vci4.Bal.Can.ICanScheduler2)) as Ixxat.Vci4.Bal.Can.ICanScheduler2;
        if (null == mScheduler)
        {
          Assert.Inconclusive();
        }
      }
      catch (Exception)
      {
        // ICanScheduler is not supported !
        Assert.Inconclusive();
      }
      finally
      {
        bal!.Dispose();
        device!.Dispose();
      }
    }

      [TestCleanup]
    public void TestCleanup()
    {
      if (null != mScheduler)
      {
        mScheduler!.Reset();
        mScheduler!.Dispose();
        mScheduler = null;
      }

      if (null != mControl)
      {
        mControl!.ResetLine();
        mControl!.Dispose();
        mControl = null;
      }
    }

    #endregion

    #region Property Status

    [TestMethod]
    /// <summary>
    ///   Test Property Status in different object states.
    /// </summary>
    public void StatusPropertyTests()
    {
      ICanCyclicTXMsg2 message;
      message = mScheduler!.AddMessage();

      message.CycleTicks = 1;
      Assert.IsTrue(CanCyclicTXStatus.Empty == message.Status);

      message.Start(1);
      Thread.Sleep(500);
      mScheduler!.UpdateStatus();
      Assert.IsTrue(CanCyclicTXStatus.Done == message.Status);

      message.Start(0);
      Thread.Sleep(500);
      mScheduler!.UpdateStatus();
      Assert.IsTrue(CanCyclicTXStatus.Busy == message.Status);

      message.Stop();
      Thread.Sleep(500);
      mScheduler!.UpdateStatus();
      Assert.IsTrue(CanCyclicTXStatus.Done == message.Status);
    }


    #endregion

    #region Property AutoIncrementIndex

    [TestMethod]
    /// <summary>
    ///   Test property AutoIncrementIndex read access
    /// </summary>
    public void AutoIncrementIndexReadAccess()
    {
      ICanCyclicTXMsg2 message;
      message = mScheduler!.AddMessage();

      message.CycleTicks = 1;

      byte refValue = message.AutoIncrementIndex;

      refValue = message.AutoIncrementIndex;
    }

    [TestMethod]
    /// <summary>
    ///   Test property AutoIncrementIndex write access
    /// </summary>
    public void AutoIncrementIndexWriteAccess()
    {
      ICanCyclicTXMsg2 message;
      message = mScheduler!.AddMessage();

      message.CycleTicks = 1;

      // write before AddMessage
      message.AutoIncrementIndex = 4;
      Assert.IsTrue(4 == message.AutoIncrementIndex);

      // write after Start
      message.Start(0);
      message.AutoIncrementIndex = 0;
      Assert.IsTrue(0 == message.AutoIncrementIndex);

      // write after Stop
      message.Stop();
      message.AutoIncrementIndex = 4;
      Assert.IsTrue(4 == message.AutoIncrementIndex);
    }

    [TestMethod]
    /// <summary>
    ///   Test property DataLength invalid data length
    /// </summary>
    public void DataLengthValidDataLength()
    {
      ICanCyclicTXMsg2 message;
      message = mScheduler!.AddMessage();

      message.DataLength = 64;
    }

    [TestMethod]
    /// <summary>
    ///   Test property DataLength invalid data length
    /// </summary>
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void DataLengthInvalidDataLength()
    {
      ICanCyclicTXMsg2 message;
      message = mScheduler!.AddMessage();

      message.DataLength = 65;
    }

    [TestMethod]
    /// <summary>
    ///   Test property AutoIncrementIndex invalid index
    /// </summary>
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void AutoIncrementIndexInvalidIndex()
    {
      ICanCyclicTXMsg2 message;
      message = mScheduler!.AddMessage();

      message.AutoIncrementIndex = 64;
    }

    [TestMethod]
    /// <summary>
    ///   Test property AutoIncrementIndex valid indices
    /// </summary>
    public void AutoIncrementIndexValidIndex()
    {
      ICanCyclicTXMsg2 message;
      message = mScheduler!.AddMessage();

      message.AutoIncrementIndex = 0;
      message.AutoIncrementIndex = 63;
    }

    #endregion

    #region Property AutoIncrementMode

    [TestMethod]
    /// <summary>
    ///   Test property AutoIncrementMode read access
    /// </summary>
    public void AutoIncrementModeReadAccess()
    {
      ICanCyclicTXMsg2 message;
      message = mScheduler!.AddMessage();

      Assert.IsTrue(CanCyclicTXIncMode.NoInc == message.AutoIncrementMode);
    }

    [TestMethod]
    /// <summary>
    ///   Test property AutoIncrementMode write access
    /// </summary>
    public void AutoIncrementModeWriteAccess()
    {
      ICanCyclicTXMsg2 message;
      message = mScheduler!.AddMessage();

      message.CycleTicks = 1;

      Assert.IsTrue(CanCyclicTXIncMode.NoInc == message.AutoIncrementMode);
      // check internal dirty state bug
      // setting the same AutoIncrementMode cleared the internal dirty flag and
      // led to msg start to fail
      message.AutoIncrementMode = CanCyclicTXIncMode.NoInc;
      message.Start(0);
      message.Stop();

      // write before AddMessage
      message.AutoIncrementMode = CanCyclicTXIncMode.Inc16;
      Assert.IsTrue(CanCyclicTXIncMode.Inc16 == message.AutoIncrementMode);

      // write after Start
      message.Start(0);
      message.AutoIncrementMode = CanCyclicTXIncMode.IncId;
      Assert.IsTrue(CanCyclicTXIncMode.IncId == message.AutoIncrementMode);

      // write after Stop
      message.Stop();
      message.AutoIncrementMode = CanCyclicTXIncMode.Inc8;
      Assert.IsTrue(CanCyclicTXIncMode.Inc8 == message.AutoIncrementMode);
    }

    #endregion

    #region Property CycleTicks

    [TestMethod]
    /// <summary>
    ///   Test property CycleTicks read access
    /// </summary>
    public void CycleTicksReadAccess()
    {
      ICanCyclicTXMsg2 message;
      message = mScheduler!.AddMessage();

      message.CycleTicks = 1;

      ushort refValue = message.CycleTicks;

      refValue = message.CycleTicks;
    }

    [TestMethod]
    /// <summary>
    ///   Test property CycleTicks write access
    /// </summary>
    public void CycleTicksWriteAccess()
    {
      ICanCyclicTXMsg2 message;
      message = mScheduler!.AddMessage();

      message.CycleTicks = 1;

      // write before AddMessage
      message.CycleTicks = 2;
      Assert.IsTrue(2 == message.CycleTicks);

      // write after Start
      message.Start(0);
      message.CycleTicks = 1;
      Assert.IsTrue(1 == message.CycleTicks);

      // write after Stop
      message.Stop();
      message.CycleTicks = 2;
      Assert.IsTrue(2 == message.CycleTicks);
    }

    #endregion

    #region Property DataLength

    [TestMethod]
    /// <summary>
    ///   Test property DataLength read access
    /// </summary>
    public void DataLengthReadAccess()
    {
      ICanCyclicTXMsg2 message;
      message = mScheduler!.AddMessage();

      message.CycleTicks = 1;

      byte refValue = message.DataLength;

      refValue = message.DataLength;
    }

    [TestMethod]
    /// <summary>
    ///   Test property DataLength write access
    /// </summary>
    public void DataLengthWriteAccess()
    {
      ICanCyclicTXMsg2 message;
      message = mScheduler!.AddMessage();

      message.CycleTicks = 1;

      // write before AddMessage
      message.DataLength = 2;
      Assert.IsTrue(2 == message.DataLength);

      // write after Start
      message.Start(0);
      message.DataLength = 4;
      Assert.IsTrue(4 == message.DataLength);

      // write after Stop
      message.Stop();
      message.DataLength = 2;
      Assert.IsTrue(2 == message.DataLength);
    }

    [TestMethod]
    /// <summary>
    ///   Test property DataLength invalid write access
    /// </summary>
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void DataLengthInvalidWriteAccess()
    {
      ICanCyclicTXMsg2 message;
      message = mScheduler!.AddMessage();

      message.CycleTicks = 1;

      message.DataLength = 100;
    }

    #endregion

    #region Property ExtendedFrameFormat

    [TestMethod]
    /// <summary>
    ///   Test property ExtendedFrameFormat read access
    /// </summary>
    public void ExtendedFrameFormatReadAccess()
    {
      ICanCyclicTXMsg2 message;
      message = mScheduler!.AddMessage();

      message.CycleTicks = 1;

      bool refValue = message.ExtendedFrameFormat;

      refValue = message.ExtendedFrameFormat;
    }

    [TestMethod]
    /// <summary>
    ///   Test property ExtendedFrameFormat write access
    /// </summary>
    public void ExtendedFrameFormatWriteAccess()
    {
      ICanCyclicTXMsg2 message;
      message = mScheduler!.AddMessage();

      message.CycleTicks = 1;

      // write before AddMessage
      message.ExtendedFrameFormat = true;
      Assert.IsTrue(true == message.ExtendedFrameFormat);

      // write after Start
      message.Start(0);
      message.ExtendedFrameFormat = false;
      Assert.IsTrue(false == message.ExtendedFrameFormat);

      // write after Stop
      message.Stop();
      message.ExtendedFrameFormat = true;
      Assert.IsTrue(true == message.ExtendedFrameFormat);
    }

    #endregion

    #region Property Identifier

    [TestMethod]
    /// <summary>
    ///   Test property Identifier read access
    /// </summary>
    public void IdentifierReadAccess()
    {
      ICanCyclicTXMsg2 message;
      message = mScheduler!.AddMessage();

      message.CycleTicks = 1;

      uint refValue = message.Identifier;

      refValue = message.Identifier;
    }

    [TestMethod]
    /// <summary>
    ///   Test property Identifier write access
    /// </summary>
    public void IdentifierWriteAccess()
    {
      ICanCyclicTXMsg2 message;
      message = mScheduler!.AddMessage();

      message.CycleTicks = 1;

      // write before AddMessage
      message.Identifier = 0x100;
      Assert.IsTrue(0x100 == message.Identifier);

      // write after Start
      message.Start(0);
      message.Identifier = 0x200;
      Assert.IsTrue(0x200 == message.Identifier);

      // write after Stop
      message.Stop();
      message.Identifier = 0x100;
      Assert.IsTrue(0x100 == message.Identifier);
    }

    #endregion

    #region Property RemoteTransmissionRequest

    [TestMethod]
    /// <summary>
    ///   Test property RemoteTransmissionRequest read access
    /// </summary>
    public void RemoteTransmissionRequestReadAccess()
    {
      ICanCyclicTXMsg2 message;
      message = mScheduler!.AddMessage();

      message.CycleTicks = 1;

      bool refValue = message.RemoteTransmissionRequest;

      refValue = message.RemoteTransmissionRequest;
    }

    [TestMethod]
    /// <summary>
    ///   Test property RemoteTransmissionRequest write access
    /// </summary>
    public void RemoteTransmissionRequestWriteAccess()
    {
      ICanCyclicTXMsg2 message;
      message = mScheduler!.AddMessage();

      message.CycleTicks = 1;

      // write before AddMessage
      message.RemoteTransmissionRequest = true;
      Assert.IsTrue(true == message.RemoteTransmissionRequest);

      // write after Start
      message.Start(0);
      message.RemoteTransmissionRequest = false;
      Assert.IsTrue(false == message.RemoteTransmissionRequest);

      // write after Stop
      message.Stop();
      message.RemoteTransmissionRequest = true;
      Assert.IsTrue(true == message.RemoteTransmissionRequest);
    }

    #endregion

    #region Property SelfReceptionRequest

    [TestMethod]
    /// <summary>
    ///   Test property SelfReceptionRequest read access
    /// </summary>
    public void SelfReceptionRequestReadAccess()
    {
      ICanCyclicTXMsg2 message;
      message = mScheduler!.AddMessage();

      message.CycleTicks = 1;

      bool refValue = message.SelfReceptionRequest;

      refValue = message.SelfReceptionRequest;
    }

    [TestMethod]
    /// <summary>
    ///   Test property SelfReceptionRequest write access
    /// </summary>
    public void SelfReceptionRequestWriteAccess()
    {
      ICanCyclicTXMsg2 message;
      message = mScheduler!.AddMessage();

      message.CycleTicks = 1;

      // write before AddMessage
      message.SelfReceptionRequest = true;
      Assert.IsTrue(true == message.SelfReceptionRequest);

      // write after Start
      message.Start(0);
      message.SelfReceptionRequest = false;
      Assert.IsTrue(false == message.SelfReceptionRequest);

      // write after Stop
      message.Stop();
      message.SelfReceptionRequest = true;
      Assert.IsTrue(true == message.SelfReceptionRequest);
    }

    #endregion

    #region Property Indexer

    [TestMethod]
    /// <summary>
    ///   Test property Indexer read access
    /// </summary>
    public void IndexerReadAccess()
    {
      ICanCyclicTXMsg2 message;
      message = mScheduler!.AddMessage();

      message.CycleTicks = 1;

      byte refValue = message[0];

      refValue = message[0];
    }

    [TestMethod]
    /// <summary>
    ///   Test property Indexer invalid read access >63
    /// </summary>
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void IndexerInvalidReadAccess()
    {
      ICanCyclicTXMsg2 message;
      message = mScheduler!.AddMessage();

      byte refValue = message[100];
    }

    [TestMethod]
    /// <summary>
    ///   Test property Indexer invalid read access <0
    /// </summary>
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void IndexerInvalidReadAccess2()
    {
      ICanCyclicTXMsg2 message;
      message = mScheduler!.AddMessage();

      byte refValue = message[-1];
    }

    [TestMethod]
    /// <summary>
    ///   Test property Indexer write access
    /// </summary>
    public void IndexerWriteAccess()
    {
      ICanCyclicTXMsg2 message;
      message = mScheduler!.AddMessage();

      message.CycleTicks = 1;

      // write before AddMessage
      message[0] = 0xAA;
      Assert.IsTrue(0xAA == message[0]);

      // write after Start
      message.Start(0);
      message[0] = 0xF;
      Assert.IsTrue(0xF == message[0]);

      // write after Stop
      message.Stop();
      message[0] = 0xAA;
      Assert.IsTrue(0xAA == message[0]);
    }

    [TestMethod]
    /// <summary>
    ///   Test property Indexer invalid write access >63
    /// </summary>
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void IndexerInvalidWriteAccess()
    {
      ICanCyclicTXMsg2 message;
      message = mScheduler!.AddMessage();

      message[100] = 5; 
    }

    [TestMethod]
    /// <summary>
    ///   Test property Indexer invalid write access <0
    /// </summary>
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void IndexerInvalidWriteAccess2()
    {
      ICanCyclicTXMsg2 message;
      message = mScheduler!.AddMessage();

      message[-1] = 5;
    }

    #endregion

    #region Method Reset

    [TestMethod]
    /// <summary>
    ///   Test Reset() method
    /// </summary>
    public void ResetTest()
    {
      ICanCyclicTXMsg2 message;
      message = mScheduler!.AddMessage();

      // Call before AddMessage()
      message.Reset();
      message.CycleTicks = 1;

      // Call after Start
      message.Start(0);
      message.Reset();

      // Call after Start()
      message.CycleTicks = 1;
      message.Stop();
      message.Start(0);
      message.Reset();
    }

    #endregion

    #region Method Start

    [TestMethod]
    /// <summary>
    ///   Test Start() method
    /// </summary>
    public void StartTest()
    {
      ICanCyclicTXMsg2 message;
      message = mScheduler!.AddMessage();

      message.CycleTicks = 1;

      // Call after AddMessage()
      message.Start(0);
    }

    [TestMethod]
    /// <summary>
    ///   Test second successive Start() call
    /// </summary>
    public void StartSecondCall()
    {
      ICanCyclicTXMsg2 message;
      message = mScheduler!.AddMessage();

      message.CycleTicks = 1;

      message.Start(0);
      message.Start(0);
    }

    #endregion

    #region Method Stop

    [TestMethod]
    /// <summary>
    ///   Test Stop() method
    /// </summary>
    public void StopTest()
    {
      ICanCyclicTXMsg2 message;
      message = mScheduler!.AddMessage();

      message.CycleTicks = 1;

      // Call after AddMessage()
      message.Stop();

      // Call after Start()
      message.Start(0);
      message.Stop();
    }

    #endregion
  }
}
