
using System;
using System.Collections.Generic;
using System.Text;
using Ixxat.Vci4;

namespace Vci4Tests
{
  [TestClass]
  public class VciDeviceMgrTests
  {

    [TestMethod]
    /// <summary>
    ///   GetDeviceList must not return a null reference.
    /// </summary>
    public void GetDeviceListNotNull()
    {
      IVciDeviceManager mgr = VciServer.Instance()!.DeviceManager;
      Assert.IsNotNull(mgr);

      IVciDeviceList list = mgr.GetDeviceList();
      Assert.IsNotNull(list);
      list.Dispose();

      mgr!.Dispose();
    }

    [TestMethod]
    /// <summary>
    ///   GetDeviceList throw ObjectDisposedException after Dispose().
    /// </summary>
    [ExpectedException(typeof(ObjectDisposedException))]
    public void GetDeviceListThrowsObjectDisposeException()
    {
      IVciDeviceManager mgr = VciServer.Instance()!.DeviceManager;
      mgr!.Dispose();
      IVciDeviceList list = mgr!.GetDeviceList();
    }

    [TestMethod]
    /// <summary>
    ///   Tests compilation and functionality of using statement
    /// </summary>
    [ExpectedException(typeof(ObjectDisposedException))]
    public void TestUsingStatement()
    {
      IVciDeviceManager manager;
      using (manager = VciServer.Instance()!.DeviceManager)
      {
        IVciDeviceList list = manager!.GetDeviceList();
        list.Dispose();
      }

      // This call must throw an ObjectDisposedException
      manager!.GetDeviceList();
    }
  }
}
