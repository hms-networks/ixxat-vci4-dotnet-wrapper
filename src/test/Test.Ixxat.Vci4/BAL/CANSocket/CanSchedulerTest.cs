using System;
using System.Collections;
using System.Text;
using System.Threading;
using Ixxat.Vci4;
using Ixxat.Vci4.Bal;
using Ixxat.Vci4.Bal.Can;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Vci4Tests
{
  [TestClass]
  public class CanSchedulerTest
    : VciDeviceTestBase
  {
    #region Member variables

    private Ixxat.Vci4.Bal.Can.ICanControl   mControl;
    private Ixxat.Vci4.Bal.Can.ICanScheduler mScheduler;
    private Ixxat.Vci4.Bal.IBalObject mBal;

    #endregion

    #region Test Initialize and Cleanup

    [TestInitialize]
    public void TestSetup()
    {
      Ixxat.Vci4.IVciDevice device = GetDevice();

      try
      {
        mBal = device.OpenBusAccessLayer();

        // Test Support of ICanScheduler
        mScheduler = mBal.OpenSocket(0, typeof(Ixxat.Vci4.Bal.Can.ICanScheduler)) as Ixxat.Vci4.Bal.Can.ICanScheduler;
        if (null == mScheduler)
        {
          Assert.Inconclusive();
        }

        // inserted code to initialize the CAN controller
        // This is neccessary because the CAN@net only supports the Scheduler funtionality in the Init mode 
        mControl = mBal.OpenSocket(0, typeof(Ixxat.Vci4.Bal.Can.ICanControl)) as Ixxat.Vci4.Bal.Can.ICanControl;
        mControl.InitLine(CanOperatingModes.Standard, CanBitrate.Cia125KBit);
      }
      catch (Exception)
      {
        // ICanScheduler is not supported !
        Assert.Inconclusive();
      }
      finally
      {
        device.Dispose();
      }
    }

    [TestCleanup]
    public void TestCleanup()
    {
      if (null != mScheduler)
      {
        mScheduler.Dispose();
        mScheduler = null;
      }

      if (null != mControl)
      {
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

    #region AddMessage Test methods

    [TestMethod]
    /// <summary>
    ///   AddMessage must not throw an exception for a valid call.
    /// </summary>
    public void AddMessageValidCall()
    {
      ICanCyclicTXMsg message;
      message = mScheduler.AddMessage();

      message.CycleTicks = 1;
    }

    #endregion

    #region StartMessage Test methods


    [TestMethod]
    /// <summary>
    ///   StartMessage must not throw Exception for repeatCount = 0.
    /// </summary>
    public void StartMessageNoExcForRepZero()
    {
      ICanCyclicTXMsg message;
      message = mScheduler.AddMessage();

      message.CycleTicks = 1;

      message.Start(0);
      message.Stop();
    }


    #endregion

    #region StopMessage Test methods

    [TestMethod]
    /// <summary>
    ///   StopMessage must not throw Exception for registered message.
    /// </summary>
    public void StopMessageNoExcForRegMsg()
    {
      ICanCyclicTXMsg message;
      message = mScheduler.AddMessage();

      message.CycleTicks = 1;

      message.Stop();
    }

    #endregion

    #region Reset Test methods

    [TestMethod]
    /// <summary>
    ///   Reset before Dispose must not throw Exception.
    /// </summary>
    public void ResetBeforeDisposeMustNotThrowException()
    {
      mScheduler.Reset();
    }

    [TestMethod]
    /// <summary>
    ///   Reset must throw ObjectDisposedException.
    /// </summary>
    [ExpectedException(typeof(ObjectDisposedException))]
    public void ResetMustThrowObjectDisposedException()
    {
      mScheduler.Dispose();

      mScheduler.Reset();
    }

    #endregion

    #region Suspend Test methods

    [TestMethod]
    /// <summary>
    ///   Suspend before Dispose must not throw Exception.
    /// </summary>
    public void SuspendBeforeDisposeMustNotThrowException()
    {
      mScheduler.Suspend();
    }

    [TestMethod]
    /// <summary>
    ///   Suspend must throw ObjectDisposedException.
    /// </summary>
    [ExpectedException(typeof(ObjectDisposedException))]
    public void SuspendMustThrowObjectDisposedException()
    {
      mScheduler.Dispose();

      mScheduler.Suspend();
    }

    #endregion

    #region Resume Test methods

    [TestMethod]
    /// <summary>
    ///   Resume before Dispose must not throw Exception.
    /// </summary>
    public void ResumeBeforeDisposeMustNotThrowException()
    {
      mScheduler.Resume();
    }

    [TestMethod]
    /// <summary>
    ///   Resume must throw ObjectDisposedException.
    /// </summary>
    [ExpectedException(typeof(ObjectDisposedException))]
    public void ResumeMustThrowObjectDisposedException()
    {
      mScheduler.Dispose();

      mScheduler.Resume();
    }

    #endregion

    #region UpdateStatus Test methods

    [TestMethod]
    /// <summary>
    ///   UpdateStatus before Dispose must not throw Exception.
    /// </summary>
    public void UpdateStatusBeforeDisposeMustNotThrowException()
    {
      mScheduler.UpdateStatus();
    }

    [TestMethod]
    /// <summary>
    ///   UpdateStatus must not throw ObjectDisposedException.
    /// </summary>
    public void UpdateStatusMustNotThrowObjectDisposedException()
    {
      mScheduler.Dispose();

      mScheduler.UpdateStatus();
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
      mScheduler.Dispose();
      mScheduler = null;

      uint divisor;
      ICanScheduler scheduler;
      using (scheduler = mBal.OpenSocket(0, typeof(Ixxat.Vci4.Bal.Can.ICanScheduler)) as Ixxat.Vci4.Bal.Can.ICanScheduler)
      {
        divisor = scheduler.DelayedTXTimerDivisor;
      }

      // This call must throw an ObjectDisposedException
      divisor = scheduler.DelayedTXTimerDivisor;
    }

    #endregion

  }
}
