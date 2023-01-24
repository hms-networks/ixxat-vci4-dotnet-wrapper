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
  class CanMessageTest
    : VciDeviceTestBase
  {
    #region Member variables

    private Ixxat.Vci4.Bal.Can.ICanChannel? mChannel;
    private Ixxat.Vci4.Bal.Can.ICanControl? mControl;
    private ICanMessageReader? mReader;
    private ICanMessageWriter? mWriter;

    private AutoResetEvent? mRxEvent;

    private ulong mTimestamp64;

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

    [TestInitialize]
    public void TestSetup()
    {
      Ixxat.Vci4.IVciDevice? device = GetDevice();
      Ixxat.Vci4.Bal.IBalObject bal;

      bal = device!.OpenBusAccessLayer();

      mControl = bal!.OpenSocket(0, typeof(Ixxat.Vci4.Bal.Can.ICanControl)) as Ixxat.Vci4.Bal.Can.ICanControl;
      mControl!.InitLine(CanOperatingModes.Standard | CanOperatingModes.Extended, CanBitrate.Cia500KBit);

      mChannel = bal!.OpenSocket(0, typeof(Ixxat.Vci4.Bal.Can.ICanChannel)) as Ixxat.Vci4.Bal.Can.ICanChannel;
      mChannel!.Initialize(10, 10, true);

      mReader = mChannel!.GetMessageReader();
      mRxEvent = new AutoResetEvent(false);
      mReader!.AssignEvent(mRxEvent);
      mReader!.Threshold = 1;

      ClearRxFifo(mReader);

      mWriter = mChannel!.GetMessageWriter();

      bal!.Dispose();
      device!.Dispose();

      mChannel!.Activate();
      ClearRxFifo(mReader);
      mControl!.StartLine();

      mTimestamp64 = 0;
    }

    [TestCleanup]
    public void TestCleanup()
    {
      if (null != mWriter)
      {
        mWriter!.Dispose();
        mWriter = null;
      }

      if (null != mReader)
      {
        mReader!.Dispose();
        mReader = null;
      }

      if (null != mChannel)
      {
        mChannel!.Deactivate();
        mChannel!.Dispose();
        mChannel = null;
      }

      if (null != mControl)
      {
        mControl!.ResetLine();
        mControl!.Dispose();
        mControl = null;
      }
    }

    #endregion

    #region Data frame tests

    [TestMethod]
    /// <summary>
    ///   Test data message with standard frame format
    /// </summary>
    public void CanMsgDataStd()
    {
      TestDataMessage(CANFrameFormat.Standard, 0x100);
    }

    [TestMethod]
    /// <summary>
    ///   Test data message with extended frame format
    /// </summary>
    public void CanMsgDataExt()
    {
      TestDataMessage(CANFrameFormat.Extended, 0x1FFFFFFE);
    }

    /// <summary>
    ///   Frame format independent test implementation for data messages.
    /// </summary>
    private void TestDataMessage(CANFrameFormat frameFormat, uint id)
    {
      bool fDataMessage = false;
      int iLoopCount = 10;

      ICanMessage? txMessage;
      IMessageFactory factory = VciServer.Instance()!.MsgFactory;
      txMessage = (ICanMessage)factory.CreateMsg(typeof(ICanMessage));

      txMessage.Identifier = id;
      txMessage.ExtendedFrameFormat = (CANFrameFormat.Extended == frameFormat);
      txMessage.DataLength = 3;
      txMessage[0] = 0x01;
      txMessage[1] = 0x05;
      txMessage[2] = 0xFF;

      // Send message
      mWriter!.SendMessage(txMessage);

      ICanMessage? rxMessage;

      //
      // Check controller start message
      //
      iLoopCount = 10;
      bool startMsgReceived = false;

      while (( fDataMessage == false) && (iLoopCount > 0 ))
      {
        while (!ReadMessage(out rxMessage)) 
        { 
          Thread.Sleep(50);
          // if (MustTerminate)
          // { return; }
        };

        if ( CanMsgFrameType.Info == rxMessage!.FrameType )
        {
          //
          // Check start message
          //
          // Receive Start message
          CanMsgInfoValue val = (CanMsgInfoValue)rxMessage[0];

          if (CanMsgInfoValue.Start == val)
            startMsgReceived = true;

          // s_context.WriteLine(rxMessage.FrameType.ToString() + " " + val.ToString());
        }
        else if (CanMsgFrameType.Data == rxMessage.FrameType)
        {
          //
          // Check echo message
          //
          // receive echo message
          Assert.IsTrue(txMessage.ExtendedFrameFormat == (CANFrameFormat.Extended == frameFormat));
          Assert.IsTrue(id == rxMessage.Identifier);
          Assert.IsTrue(3 == rxMessage.DataLength);
          Assert.IsTrue(0x01 == rxMessage[0]);
          Assert.IsTrue(0x05 == rxMessage[1]);
          Assert.IsTrue(0xFF == rxMessage[2]);
          Assert.IsFalse(rxMessage.PossibleOverrun);
          Assert.IsFalse(rxMessage.RemoteTransmissionRequest);
          fDataMessage = true;
          break;
        }
        else
        {
          // s_context.WriteLine(rxMessage.FrameType.ToString());
          // s_context.WriteLine(((CanCtrlStatus)rxMessage[0]).ToString());
        }

        iLoopCount --;
      }

      if (!startMsgReceived)
      {
        // Start message not received
        Assert.IsTrue(startMsgReceived);
      }

      Assert.IsTrue(true == fDataMessage);

      // Stop CAN line
      mControl!.StopLine();

      // Wait for stop message
      while (!ReadMessage(out rxMessage))
      {
        Thread.Sleep(50);
        // if (MustTerminate)
        // { return; }
      };
      Assert.IsTrue(CanMsgFrameType.Info == rxMessage!.FrameType);
      Assert.IsTrue((byte)CanMsgInfoValue.Stop == rxMessage![0]);
    }

    #endregion

    #region Remote frame tests

    [TestMethod]
    /// <summary>
    ///   Test remote message with standard frame format
    /// </summary>
    public void CanMsgRemoteStd()
    {
      TestRemoteMessage(CANFrameFormat.Standard, 0x100);
    }

    [TestMethod]
    /// <summary>
    ///   Test remote message with extended frame format
    /// </summary>
    public void CanMsgRemoteExt()
    {
      TestRemoteMessage(CANFrameFormat.Extended, 0x1FFFFFFE);
    }

    /// <summary>
    ///   Frame format independent test implementation for remote messages.
    /// </summary>
    private void TestRemoteMessage(CANFrameFormat frameFormat, uint id)
    {
      bool fDataMessage = false;
      int iLoopCount = 10;
      
      ICanMessage? txMessage;
      IMessageFactory? factory = VciServer.Instance()!.MsgFactory;
      txMessage = (ICanMessage)factory!.CreateMsg(typeof(ICanMessage));

      txMessage.Identifier = id;
      txMessage.ExtendedFrameFormat = (CANFrameFormat.Extended == frameFormat);
      txMessage.DataLength = 3;
      txMessage.RemoteTransmissionRequest = true;

      // Send message
      mWriter!.SendMessage(txMessage);

      ICanMessage? rxMessage;

      //
      // Check controller start message
      //
      iLoopCount = 10;

      while ((fDataMessage == false) && (iLoopCount > 0))
      {
        while (!ReadMessage(out rxMessage))
        {
          Thread.Sleep(50);
          // if (MustTerminate)
          // { return; }
        };

        if (CanMsgFrameType.Info == rxMessage!.FrameType)
        {
          //
          // Check start message
          //
          // Receive Start message
          Assert.IsTrue(CanMsgFrameType.Info == rxMessage.FrameType);
          Assert.IsTrue((byte)CanMsgInfoValue.Start == rxMessage[0]);
          // s_context.WriteLine(rxMessage.FrameType.ToString());
        }
        else if (CanMsgFrameType.Data == rxMessage.FrameType)
        {
          //
          // Check echo message
          //
          // receive echo message
          Assert.IsTrue(txMessage.ExtendedFrameFormat == (CANFrameFormat.Extended == frameFormat));
          Assert.IsTrue(id == rxMessage.Identifier);
          Assert.IsTrue(3 == rxMessage.DataLength);
          Assert.IsTrue(rxMessage.RemoteTransmissionRequest);
          Assert.IsFalse(rxMessage.PossibleOverrun);
          fDataMessage = true;
          break;
        }
        else
        {
          // s_context.WriteLine(rxMessage.FrameType.ToString());
          // s_context.WriteLine(((CanCtrlStatus)rxMessage[0]).ToString());
        }

        iLoopCount--;
      }

      Assert.IsTrue(true == fDataMessage);

      // Stop CAN line
      mControl!.StopLine();

      // Wait for stop message
      while (!ReadMessage(out rxMessage))
      {
        Thread.Sleep(50);
        // if (MustTerminate)
        // { return; }
      };
      Assert.IsTrue(CanMsgFrameType.Info == rxMessage!.FrameType);
      Assert.IsTrue((byte)CanMsgInfoValue.Stop == rxMessage![0]);
    }

    #endregion

    #region Helper methods

    /// <summary>
    ///   Reads a message from the message channel under consideration of 
    ///   timestamp overruns.
    /// </summary>
    bool ReadMessage(out ICanMessage? message)
    {
      bool result = false;

      if (!mReader!.ReadMessage(out message))
      {
        message = null;
      }
      else
      {
        if (CanMsgFrameType.TimeOverrun == message.FrameType)
        {
          mTimestamp64 += message.Identifier << 32;
        }
        else 
        {
          result = true;

          if (CanMsgFrameType.Data == message.FrameType)
          {
            ulong timestamp = (mTimestamp64 & 0xFFFFFFFF00000000) + message.TimeStamp;
            Assert.IsTrue(timestamp >= mTimestamp64);
            mTimestamp64 = timestamp;
          }
        }
      }

      return result;
    }

    #endregion
  }
}
