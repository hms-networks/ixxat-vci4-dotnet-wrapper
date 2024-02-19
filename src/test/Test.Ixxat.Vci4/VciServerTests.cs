using Ixxat.Vci4;
using Ixxat.Vci4.Bal.Can;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Vci4Tests
{
  [TestClass]
  public class VciServerTests
  {
    [TestMethod]
    /// <summary>
    ///   Tests if VCI version is accessible
    /// </summary>
    public void TestVciServerVersion()
    {
      System.Version vciVersion = VciServer.Instance().Version;
      Assert.IsNotNull(vciVersion);
      Assert.AreEqual(vciVersion.Major, 4);
      Assert.AreEqual(vciVersion.Minor, 0);
    }

    [TestMethod]
    /// <summary>
    ///   Test getting the VCI device manager
    /// </summary>
    public void TestGetDeviceManager()
    {
      IVciDeviceManager mgr = VciServer.Instance().DeviceManager;
      Assert.IsNotNull(mgr);
      mgr.Dispose();
    }

    [TestMethod]
    /// <summary>
    ///   Test getting the message factory and create a message
    /// </summary>
    public void TestGetMsgFactory()
    {
      IVciServer srv = VciServer.Instance();
      Assert.IsNotNull(srv);

      IMessageFactory factory = srv.MsgFactory;
      Assert.IsNotNull(factory);

      ICanMessage txMessage;
      txMessage = (ICanMessage)factory.CreateMsg(typeof(ICanMessage));
      txMessage.Identifier = 100;
      txMessage.ExtendedFrameFormat = true;
      txMessage.DataLength = 3;
      txMessage[0] = 0x01;
      txMessage[1] = 0x05;
      txMessage[2] = 0xFF;
    }
  }
}