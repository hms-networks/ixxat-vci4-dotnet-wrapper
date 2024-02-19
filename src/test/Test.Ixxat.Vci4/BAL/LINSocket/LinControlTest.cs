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
  public class LinControlTest
    : VciDeviceTestBase
  {
    #region Member variables

    private const Byte mcInvalidPort = 0xFF;
    private Byte mLinPort;
    private Ixxat.Vci4.Bal.Lin.ILinControl mControl;
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

      mControl = mBal.OpenSocket(mLinPort, typeof(Ixxat.Vci4.Bal.Lin.ILinControl)) as Ixxat.Vci4.Bal.Lin.ILinControl;
    }

    [TestCleanup]
    public void TestCleanup()
    {
      if (null != mControl)
      {
        try
        {
          mControl.ResetLine();
        }
        catch (Exception)
        {
        }

        mControl.Dispose();
        mControl = null;
      }

      if (null != mBal)
      {
        mBal.Dispose();
        mBal = null;
      }
    }

    #endregion

    #region InitLine Test methods

    [TestMethod]
    /// <summary>
    ///   InitLine in slave mode
    /// </summary>
    public void InitLineSlaveMode()
    {
      LinInitLine initData;
      initData.OperatingMode = LinOperatingModes.Slave;

      initData.Bitrate = LinBitrate.Lin1000Bit;
      mControl.InitLine(initData);

      initData.Bitrate = LinBitrate.Lin1200Bit;
      mControl.InitLine(initData);

      initData.Bitrate = LinBitrate.Lin2400Bit;
      mControl.InitLine(initData);

      initData.Bitrate = LinBitrate.Lin4800Bit;
      mControl.InitLine(initData);

      initData.Bitrate = LinBitrate.Lin9600Bit;
      mControl.InitLine(initData);

      initData.Bitrate = LinBitrate.Lin10400Bit;
      mControl.InitLine(initData);

      initData.Bitrate = LinBitrate.Lin19200Bit;
      mControl.InitLine(initData);

      initData.Bitrate = LinBitrate.Lin20000Bit;
      mControl.InitLine(initData);
    }

    [TestMethod]
    /// <summary>
    ///   InitLine in master mode
    /// </summary>
    public void InitLineMasterMode()
    {
      LinInitLine initData;
      initData.OperatingMode = LinOperatingModes.Master;

      initData.Bitrate = LinBitrate.Lin1000Bit;
      mControl.InitLine(initData);

      initData.Bitrate = LinBitrate.Lin1200Bit;
      mControl.InitLine(initData);

      initData.Bitrate = LinBitrate.Lin2400Bit;
      mControl.InitLine(initData);

      initData.Bitrate = LinBitrate.Lin4800Bit;
      mControl.InitLine(initData);

      initData.Bitrate = LinBitrate.Lin9600Bit;
      mControl.InitLine(initData);

      initData.Bitrate = LinBitrate.Lin10400Bit;
      mControl.InitLine(initData);

      initData.Bitrate = LinBitrate.Lin19200Bit;
      mControl.InitLine(initData);

      initData.Bitrate = LinBitrate.Lin20000Bit;
      mControl.InitLine(initData);
    }

    [TestMethod]
    /// <summary>
    ///   InitLine with undefined bitrate
    /// </summary>
    [ExpectedException(typeof(VciException))]
    public void InitLineUndefinedBitrate()
    {
      LinInitLine initData;
      initData.OperatingMode = LinOperatingModes.Slave;
      initData.Bitrate = LinBitrate.Undefined;
      mControl.InitLine(initData);
    }

    [TestMethod]
    /// <summary>
    ///   InitLine with Autorate in slave mode
    /// </summary>
    public void InitLineAutorateSlaveMode()
    {
      LinInitLine initData;
      if (mControl.SupportsAutorate)
      {
        initData.OperatingMode = LinOperatingModes.Slave;
        initData.Bitrate = LinBitrate.AutoRate;
        mControl.InitLine(initData);
      }
    }

    [TestMethod]
    /// <summary>
    ///   InitLine with Autorate in master mode
    /// </summary>
    public void InitLineAutorateMasterMode()
    {
      LinInitLine initData;
      if ( mControl.SupportsAutorate )
      {
        initData.OperatingMode = LinOperatingModes.Master;
        initData.Bitrate = LinBitrate.AutoRate;
        mControl.InitLine(initData);
      }
    }

    [TestMethod]
    /// <summary>
    ///   InitLine must throw ObjectDisposedException.
    /// </summary>
    [ExpectedException(typeof(ObjectDisposedException))]
    public void InitLineMustThrowObjectDisposedException()
    {
      mControl.Dispose();

      LinInitLine initData;
      initData.OperatingMode = LinOperatingModes.Master;
      initData.Bitrate = LinBitrate.AutoRate;
      mControl.InitLine(initData);
    }

    #endregion

    #region StartLine Test methods

    [TestMethod]
    /// <summary>
    ///   StartLine after Init
    /// </summary>
    public void StartLineAfterInit()
    {
      LinInitLine initData;
      initData.OperatingMode = LinOperatingModes.Master;
      initData.Bitrate = LinBitrate.Lin1000Bit;
      mControl.InitLine(initData);
      mControl.StartLine();
    }

    [TestMethod]
    /// <summary>
    ///   StartLine before Init
    /// </summary>
    [ExpectedException(typeof(VciException))]
    public void StartLineBeforeInit()
    {
      mControl.StartLine();
    }

    [TestMethod]
    /// <summary>
    ///   StartLine must throw ObjectDisposedException.
    /// </summary>
    [ExpectedException(typeof(ObjectDisposedException))]
    public void StartLineMustThrowObjectDisposedException()
    {
      mControl.Dispose();
      mControl.StartLine();
    }

    #endregion

    #region StopLine Test methods

    [TestMethod]
    /// <summary>
    ///   StopLine after Init
    /// </summary>
    public void StopLineAfterInit()
    {
      LinInitLine initData;
      initData.OperatingMode = LinOperatingModes.Master;
      initData.Bitrate = LinBitrate.Lin1000Bit;
      mControl.InitLine(initData);
      mControl.StopLine();
    }

    [TestMethod]
    /// <summary>
    ///   StopLine before Init
    /// </summary>
    public void StopLineBeforeInit()
    {
      mControl.StopLine();
    }

    [TestMethod]
    /// <summary>
    ///   StopLine must throw ObjectDisposedException.
    /// </summary>
    [ExpectedException(typeof(ObjectDisposedException))]
    public void StopLineMustThrowObjectDisposedException()
    {
      mControl.Dispose();
      mControl.StopLine();
    }

    #endregion

    #region ResetLine Test methods

    [TestMethod]
    /// <summary>
    ///   ResetLine after Init
    /// </summary>
    public void ResetLineAfterInit()
    {
      LinInitLine initData;
      initData.OperatingMode = LinOperatingModes.Master;
      initData.Bitrate = LinBitrate.Lin1000Bit;
      mControl.InitLine(initData);
      mControl.ResetLine();
    }

    [TestMethod]
    /// <summary>
    ///   ResetLine before Init
    /// </summary>
    public void ResetLineBeforeInit()
    {
      mControl.ResetLine();
    }

    [TestMethod]
    /// <summary>
    ///   ResetLine must throw ObjectDisposedException.
    /// </summary>
    [ExpectedException(typeof(ObjectDisposedException))]
    public void ResetLineMustThrowObjectDisposedException()
    {
      mControl.Dispose();
      mControl.ResetLine();
    }

    #endregion

    #region WriteMessage Test methods

    [TestMethod]
    /// <summary>
    ///   WriteMessage before Init
    /// </summary>
    [ExpectedException(typeof(VciException))]
    public void WriteMessageBeforeInit()
    {
      ILinMessage msg;
      IMessageFactory factory = VciServer.Instance().MsgFactory;
      msg = (ILinMessage)factory.CreateMsg(typeof(ILinMessage));

      msg.MessageType = LinMessageType.Data;
      msg.ProtId = 1;
      msg.DataLength = 2;
      msg[0] = 1;
      msg[1] = 2;
      mControl.WriteMessage(true, msg);
    }

    [TestMethod]
    /// <summary>
    ///   WriteMessage with forced transmission
    /// </summary>
    public void WriteMessageForceSend()
    {
      LinInitLine initData;
      initData.OperatingMode = LinOperatingModes.Master;
      initData.Bitrate = LinBitrate.Lin1000Bit;
      mControl.InitLine(initData);

      ILinMessage msg;
      IMessageFactory factory = VciServer.Instance().MsgFactory;
      msg = (ILinMessage)factory.CreateMsg(typeof(ILinMessage));

      msg.MessageType = LinMessageType.Data;
      msg.ProtId = 1;
      msg.DataLength = 2;
      msg[0] = 1;
      msg[1] = 2;

      // WriteMessage may fail caused by internal buffer overflow.
      // So we try it 5 times.
      for(int attempt = 1; 5 > attempt; ++attempt)
      {
        try
        {
          msg[0] = (Byte)attempt;
          mControl.WriteMessage(true, msg);
          break;
        }
        catch (System.Exception e)
        {
          // s_context.WriteLine(String.Format("Attempt {0} failed", attempt));
          Thread.Sleep(0);
          if (5 < attempt)
          {
            throw e;
          }
        }
      }
    }

    [TestMethod]
    /// <summary>
    ///   WriteMessage buffer update
    /// </summary>
    public void WriteMessageBufferUpdate()
    {
      LinInitLine initData;
      initData.OperatingMode = LinOperatingModes.Master;
      initData.Bitrate = LinBitrate.Lin1000Bit;
      mControl.InitLine(initData);

      ILinMessage msg;
      IMessageFactory factory = VciServer.Instance().MsgFactory;
      msg = (ILinMessage)factory.CreateMsg(typeof(ILinMessage));

      msg.MessageType = LinMessageType.Data;
      msg.ProtId = 1;
      msg.DataLength = 2;
      msg[0] = 1;
      msg[1] = 2;

      mControl.WriteMessage(false, msg);
    }

    [TestMethod]
    /// <summary>
    ///   WriteMessage must throw ObjectDisposedException.
    /// </summary>
    [ExpectedException(typeof(ObjectDisposedException))]
    public void WriteMessageMustThrowObjectDisposedException()
    {
      mControl.Dispose();

      ILinMessage msg;
      IMessageFactory factory = VciServer.Instance().MsgFactory;
      msg = (ILinMessage)factory.CreateMsg(typeof(ILinMessage));

      msg.MessageType = LinMessageType.Data;
      msg.ProtId = 1;
      msg.DataLength = 2;
      msg[0] = 1;
      msg[1] = 2;
      mControl.WriteMessage(true, msg);
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
      mControl.Dispose();
      mControl = null;

      LinFeatures features;
      ILinControl control;
      using (control = mBal.OpenSocket(mLinPort, typeof(Ixxat.Vci4.Bal.Lin.ILinControl)) as Ixxat.Vci4.Bal.Lin.ILinControl)
      {
        features = control.Features;
      }

      // This call must throw an ObjectDisposedException
      features = control.Features;
    }

    #endregion

  }
}
