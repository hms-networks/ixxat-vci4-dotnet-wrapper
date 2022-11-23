using System;
using System.Collections;
using Ixxat.Vci4;

namespace Vci4Tests
{
  public enum CANFrameFormat
  {
    Standard,
    Extended
  }

  public class VciDeviceTestBase
  {
    #region Misc Member functions

    /// <summary>
    ///   Called to get the configured device
    /// </summary>
    protected IVciDevice? GetDevice()
    {
      // Actually we do not consider the test configuration and retrieve
      // the first available device.
      Ixxat.Vci4.IVciDevice? device = null;

      IVciDeviceManager manager = VciServer.Instance()!.DeviceManager;
      IVciDeviceList list = manager.GetDeviceList();
      IEnumerator enu = list.GetEnumerator();

      enu.MoveNext();
      device = enu.Current as IVciDevice;

      (enu as IDisposable)!.Dispose();
      list.Dispose();
      manager.Dispose();

      // WriteLogLine("USING Device" + device.Description);
      return device;
    }

    #endregion

  }
}