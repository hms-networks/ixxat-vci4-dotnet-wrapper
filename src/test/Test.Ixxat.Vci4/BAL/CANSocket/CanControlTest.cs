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
  public class CanControlTest
    : VciDeviceTestBase
  {
    #region Member variables

    private Ixxat.Vci4.Bal.Can.ICanControl? mSocket;
    private Ixxat.Vci4.Bal.IBalObject? mBal;

    #endregion

    #region Test Initialize and Cleanup

    [TestInitialize]
    public void TestSetup()
    {
      Ixxat.Vci4.IVciDevice? device = GetDevice();
      mBal = device!.OpenBusAccessLayer();

      device!.Dispose();

      mSocket = mBal!.OpenSocket(0, typeof(Ixxat.Vci4.Bal.Can.ICanControl)) as Ixxat.Vci4.Bal.Can.ICanControl;
    }

    [TestCleanup]
    public void TestCleanup()
    {
      if (null != mSocket)
      {
        try
        {
          mSocket!.ResetLine();
        }
        catch (Exception)
        {
        }

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

    #region DetectBaud Test methods

    [TestMethod]
    /// <summary>
    ///   DetectBaud must throw VciException (Timeout)
    /// </summary>
    [ExpectedException(typeof(VciException))]
    public void DetectBaudMustThrowTimeoutExc()
    {
      int result = mSocket!.DetectBaud(0, CanBitrate.CiaBitRates);
    }

    [TestMethod]
    /// <summary>
    ///   DetectBaud with empty bitrate array
    /// </summary>
    [ExpectedException(typeof(VciException))]
    public void DetectBaudWithEmptyBitrateArray()
    {
      CanBitrate[] bitrates = new CanBitrate[] { };
      int result = mSocket!.DetectBaud(1, bitrates);
    }

    [TestMethod]
    /// <summary>
    ///   DetectBaud must throw ObjectDisposedException.
    /// </summary>
    [ExpectedException(typeof(ObjectDisposedException))]
    public void DetectBaudMustThrowObjectDisposedException()
    {
      mSocket!.Dispose();
      int result = mSocket!.DetectBaud(0, CanBitrate.CiaBitRates);
    }

    #endregion

    #region InitLine Test methods

    [TestMethod]
    /// <summary>
    ///   InitLine with standard frame format
    /// </summary>
    public void InitLineWithStandardFormat()
    {
      mSocket!.InitLine(CanOperatingModes.Standard, CanBitrate.Cia1000KBit);
    }

    [TestMethod]
    /// <summary>
    ///   InitLine with std/ext frame format
    /// </summary>
    public void InitLineWithStdExtFormat()
    {
      if (mSocket!.SupportsStdAndExtFrames)
      {
        mSocket!.InitLine(CanOperatingModes.Standard | CanOperatingModes.Extended
                        , CanBitrate.Cia1000KBit);
      }
      else
      {
        try
        {
          mSocket!.InitLine(CanOperatingModes.Standard | CanOperatingModes.Extended
                          , CanBitrate.Cia1000KBit);
          Assert.IsTrue(false);
        }
        catch (VciException)
        {
        }
      }
    }

    [TestMethod]
    /// <summary>
    ///   InitLine must throw ObjectDisposedException.
    /// </summary>
    [ExpectedException(typeof(ObjectDisposedException))]
    public void InitLineMustThrowObjectDisposedException()
    {
      mSocket!.Dispose();
      mSocket!.InitLine(CanOperatingModes.Standard, CanBitrate.Cia1000KBit);
    }

    #endregion

    #region StartLine Test methods

    [TestMethod]
    /// <summary>
    ///   StartLine after Init
    /// </summary>
    public void StartLineAfterInit()
    {
      mSocket!.InitLine(CanOperatingModes.Standard, CanBitrate.Cia1000KBit);

      mSocket!.StartLine();
    }

    [TestMethod]
    /// <summary>
    ///   StartLine before Init
    /// </summary>
    [ExpectedException(typeof(VciException))]
    public void StartLineBeforeInit()
    {
      mSocket!.StartLine();
    }

    [TestMethod]
    /// <summary>
    ///   StartLine must throw ObjectDisposedException.
    /// </summary>
    [ExpectedException(typeof(ObjectDisposedException))]
    public void StartLineMustThrowObjectDisposedException()
    {
      mSocket!.Dispose();
      mSocket!.StartLine();
    }

    #endregion

    #region StopLine Test methods

    [TestMethod]
    /// <summary>
    ///   StopLine after Init
    /// </summary>
    public void StopLineAfterInit()
    {
      mSocket!.InitLine(CanOperatingModes.Standard, CanBitrate.Cia1000KBit);

      mSocket!.StopLine();
    }

    [TestMethod]
    /// <summary>
    ///   StopLine before Init
    /// </summary>
    public void StopLineBeforeInit()
    {
      mSocket!.StopLine();
    }

    [TestMethod]
    /// <summary>
    ///   StopLine must throw ObjectDisposedException.
    /// </summary>
    [ExpectedException(typeof(ObjectDisposedException))]
    public void StopLineMustThrowObjectDisposedException()
    {
      mSocket!.Dispose();
      mSocket!.StopLine();
    }

    #endregion

    #region ResetLine Test methods

    [TestMethod]
    /// <summary>
    ///   ResetLine after Init
    /// </summary>
    public void ResetLineAfterInit()
    {
      mSocket!.InitLine(CanOperatingModes.Standard, CanBitrate.Cia1000KBit);

      mSocket!.ResetLine();
    }

    [TestMethod]
    /// <summary>
    ///   ResetLine before Init
    /// </summary>
    public void ResetLineBeforeInit()
    {
      mSocket!.ResetLine();
    }

    [TestMethod]
    /// <summary>
    ///   ResetLine must throw ObjectDisposedException.
    /// </summary>
    [ExpectedException(typeof(ObjectDisposedException))]
    public void ResetLineMustThrowObjectDisposedException()
    {
      mSocket!.Dispose();
      mSocket!.ResetLine();
    }

    #endregion

    #region SetAccFilter Test methods

    [TestMethod]
    /// <summary>
    ///   SetAccFilter after Init
    /// </summary>
    public void SetAccFilterAfterInit()
    {
      mSocket!.InitLine(CanOperatingModes.Standard, CanBitrate.Cia1000KBit);

      mSocket!.SetAccFilter(CanFilter.Std, 0, 0xFFF);
      mSocket!.SetAccFilter(CanFilter.Ext, 0, 0x3FFFFFFF);
    }

    [TestMethod]
    /// <summary>
    ///   SetAccFilter Std before Init
    /// </summary>
    [ExpectedException(typeof(VciException))]
    public void SetAccFilterStdBeforeInit()
    {
      mSocket!.SetAccFilter(CanFilter.Std, 0, 0xFFF);
    }

    [TestMethod]
    /// <summary>
    ///   SetAccFilter Ext before Init
    /// </summary>
    [ExpectedException(typeof(VciException))]
    public void SetAccFilterExtBeforeInit()
    {
      mSocket!.SetAccFilter(CanFilter.Ext, 0, 0x3FFFFFFF);
    }

    [TestMethod]
    /// <summary>
    ///   SetAccFilter must throw ObjectDisposedException.
    /// </summary>
    [ExpectedException(typeof(ObjectDisposedException))]
    public void SetAccFilterMustThrowObjectDisposedException()
    {
      mSocket!.Dispose();
      mSocket!.SetAccFilter(CanFilter.Ext, 0, 0x3FFFFFFF);
    }

    #endregion

    #region AddFilterIds Test methods

    [TestMethod]
    /// <summary>
    ///   AddFilterIds Std after Init
    /// </summary>
    public void AddFilterStdIdsAfterInit()
    {
      mSocket!.InitLine(CanOperatingModes.Standard, CanBitrate.Cia1000KBit);

      mSocket!.AddFilterIds(CanFilter.Std, 0, 0xFFF);
    }

    [TestMethod]
    /// <summary>
    ///   AddFilterIds Ext after Init
    /// </summary>
    public void AddFilterExtIdsAfterInit()
    {
      mSocket!.InitLine(CanOperatingModes.Extended, CanBitrate.Cia1000KBit);

      mSocket!.AddFilterIds(CanFilter.Ext, 0, 0x3FFFFFFF);
    }

    [TestMethod]
    /// <summary>
    ///   AddFilterIds Std before Init
    /// </summary>
    [ExpectedException(typeof(VciException))]
    public void AddFilterIdsStdBeforeInit()
    {
      mSocket!.AddFilterIds(CanFilter.Std, 0, 0xFFF);
    }

    [TestMethod]
    /// <summary>
    ///   AddFilterIds Ext before Init
    /// </summary>
    [ExpectedException(typeof(VciException))]
    public void AddFilterIdsExtBeforeInit()
    {
      mSocket!.AddFilterIds(CanFilter.Ext, 0, 0x3FFFFFFF);
    }

    [TestMethod]
    /// <summary>
    ///   AddFilterIds must throw ObjectDisposedException.
    /// </summary>
    [ExpectedException(typeof(ObjectDisposedException))]
    public void AddFilterIdsMustThrowObjectDisposedException()
    {
      mSocket!.Dispose();
      mSocket!.AddFilterIds(CanFilter.Ext, 0, 0x3FFFFFFF);
    }

    #endregion

    #region RemFilterIds Test methods

    [TestMethod]
    /// <summary>
    ///   RemFilterIds std after Init
    /// </summary>
    public void RemFilterStdIdsAfterInit()
    {
      mSocket!.InitLine(CanOperatingModes.Standard, CanBitrate.Cia1000KBit);
      
      mSocket!.RemFilterIds(CanFilter.Std, 0, 0xFFF);
    }

    [TestMethod]
    /// <summary>
    ///   RemFilterIds ext after Init
    /// </summary>
    public void RemFilterIdsExtAfterInit()
    {
      mSocket!.InitLine(CanOperatingModes.Extended, CanBitrate.Cia1000KBit);

      mSocket!.RemFilterIds(CanFilter.Ext, 0, 0x3FFFFFFF);
    }

    [TestMethod]
    /// <summary>
    ///   RemFilterIds Std before Init
    /// </summary>
    [ExpectedException(typeof(VciException))]
    public void RemFilterIdsStdBeforeInit()
    {
      mSocket!.RemFilterIds(CanFilter.Std, 0, 0xFFF);
    }

    [TestMethod]
    /// <summary>
    ///   RemFilterIds Ext before Init
    /// </summary>
    [ExpectedException(typeof(VciException))]
    public void RemFilterIdsExtBeforeInit()
    {
      mSocket!.RemFilterIds(CanFilter.Ext, 0, 0x3FFFFFFF);
    }

    [TestMethod]
    /// <summary>
    ///   RemFilterIds must throw ObjectDisposedException.
    /// </summary>
    [ExpectedException(typeof(ObjectDisposedException))]
    public void RemFilterIdsMustThrowObjectDisposedException()
    {
      mSocket!.Dispose();
      mSocket!.RemFilterIds(CanFilter.Ext, 0, 0x3FFFFFFF);
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

      CanCtrlType ctrlType;
      ICanControl? control;
      using (control = mBal!.OpenSocket(0, typeof(Ixxat.Vci4.Bal.Can.ICanControl)) as Ixxat.Vci4.Bal.Can.ICanControl)
      {
        ctrlType = control!.ControllerType;
      }

      // This call must throw an ObjectDisposedException
      ctrlType = control!.ControllerType;
    }

    #endregion

  }
}
