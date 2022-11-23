using Ixxat.Vci4;

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
      Version vciVersion = VciServer.Instance()!.Version;
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
      IVciDeviceManager mgr = VciServer.Instance()!.DeviceManager;
      Assert.IsNotNull(mgr);
      mgr.Dispose();
    }

  }
}