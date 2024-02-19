using System;
using System.Collections;
using System.Text;
using System.Threading;
using Ixxat.Vci4;
using Ixxat.Vci4.Bal;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Vci4Tests
{
  [TestClass]
  public class BalObjectTest
    : VciDeviceTestBase
  {
    #region Member variables

    private Ixxat.Vci4.Bal.IBalObject mBal;

    #endregion

    #region Test Initialize and Cleanup

    [TestInitialize]
    public void TestSetup()
    {
      Ixxat.Vci4.IVciDevice device = GetDevice();

      mBal = device.OpenBusAccessLayer();

      device.Dispose();
    }

    [TestCleanup]
    public void TestCleanup()
    {
      if (null != mBal)
      {
        mBal.Dispose();
        mBal = null;
      }
    }

    #endregion

    #region Property FirmwareVersion Test methods

    [TestMethod]
    /// <summary>
    ///   FirmwareVersion has constant value
    /// </summary>
    public void FirmwareVersionIsConstant()
    {
      Version version = mBal.FirmwareVersion;
      
      Version testVersion = mBal.FirmwareVersion;
      Assert.IsNotNull(testVersion);
      Assert.IsTrue(version == testVersion);
    }

    [TestMethod]
    /// <summary>
    ///   FirmwareVersion must throw ObjectDisposedException.
    /// </summary>
    [ExpectedException(typeof(ObjectDisposedException))]
    public void FirmwareVersionMustThrowObjectDisposedException()
    {
      mBal.Dispose();
      Version version = mBal.FirmwareVersion;
    }

    #endregion

    #region Property Resources Test methods

    [TestMethod]
    /// <summary>
    ///   Resources has constant value
    /// </summary>
    public void ResourcesIsConstant()
    {
      BalResourceCollection refValue = mBal.Resources;
      Assert.IsNotNull(refValue);
      Assert.IsTrue(0 < refValue.Count);

      BalResourceCollection testValue = mBal.Resources;
      Assert.IsNotNull(refValue);
      Assert.IsTrue(refValue.Count == testValue.Count);
    }

    [TestMethod]
    /// <summary>
    ///   Resources must throw ObjectDisposedException.
    /// </summary>
    [ExpectedException(typeof(ObjectDisposedException))]
    public void ResourcesMustThrowObjectDisposedException()
    {
      mBal.Dispose();
      BalResourceCollection refValue = mBal.Resources;
    }

    #endregion

    #region OpenSocket Test methods

    [TestMethod]
    /// <summary>
    ///   OpenSocket must not return a null reference
    /// </summary>
    public void OpenSocketMustNotReturnNull()
    {
      Ixxat.Vci4.Bal.Can.ICanSocket socket = mBal.OpenSocket(0, typeof(Ixxat.Vci4.Bal.Can.ICanSocket)) as Ixxat.Vci4.Bal.Can.ICanSocket;
      Assert.IsNotNull(socket);
      socket.Dispose();
    }

    [TestMethod]
    /// <summary>
    ///   OpenSocket with invalid port number
    /// </summary>
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void OpenSocketWithInvalidPortNum()
    {
      IBalResource socket = mBal.OpenSocket(10, typeof(IBalResource));
    }

    [TestMethod]
    /// <summary>
    ///   OpenSocket with invalid socket type
    /// </summary>
    [ExpectedException(typeof(NotImplementedException))]
    public void OpenSocketWithInvalidSocketType()
    {
      IBalResource socket = mBal.OpenSocket(0, typeof(string));
    }

    [TestMethod]
    /// <summary>
    ///   OpenSocket ICanControl twice
    /// </summary>
    public void OpenSocketICanControlTwice()
    {
      Ixxat.Vci4.Bal.Can.ICanControl canCtrl1 = null;
      Ixxat.Vci4.Bal.Can.ICanControl canCtrl2 = null;

      canCtrl1 = mBal.OpenSocket(0, typeof(Ixxat.Vci4.Bal.Can.ICanControl)) as Ixxat.Vci4.Bal.Can.ICanControl;
      Assert.IsNotNull(canCtrl1);
      try
      {
        canCtrl2 = mBal.OpenSocket(0, typeof(Ixxat.Vci4.Bal.Can.ICanControl)) as Ixxat.Vci4.Bal.Can.ICanControl;
        if (canCtrl2 != null)
        {
          canCtrl2.Dispose();
        }
        Assert.IsTrue(false);
      }
      catch (VciException)
      {
      }

      canCtrl1.Dispose();
    }

    [TestMethod]
    /// <summary>
    ///   OpenSocket ICanControl twice
    /// </summary>
    public void OpenSocketICanControlTwice2()
    {
      Ixxat.Vci4.Bal.Can.ICanControl2 canCtrl1 = null;
      Ixxat.Vci4.Bal.Can.ICanControl2 canCtrl2 = null;

      canCtrl1 = mBal.OpenSocket(0, typeof(Ixxat.Vci4.Bal.Can.ICanControl2)) as Ixxat.Vci4.Bal.Can.ICanControl2;
      Assert.IsNotNull(canCtrl1);
      try
      {
        canCtrl2 = mBal.OpenSocket(0, typeof(Ixxat.Vci4.Bal.Can.ICanControl2)) as Ixxat.Vci4.Bal.Can.ICanControl2;
        if (canCtrl2 != null)
        {
          canCtrl2.Dispose();
        }
        Assert.IsTrue(false);
      }
      catch (VciException)
      {
      }

      canCtrl1.Dispose();
    }

    [TestMethod]
    /// <summary>
    ///   OpenSocket ICanScheduler twice
    /// </summary>
    public void OpenSocketICanSchedulerTwice()
    {
      // Because not all of the available fieldbus adapter support
      // the ICanScheduler socket we use the first call to determine this.

      Ixxat.Vci4.Bal.Can.ICanScheduler canSched1 = null;
      Ixxat.Vci4.Bal.Can.ICanScheduler canSched2 = null;

      try
      {
        // First OpenSocket
        canSched1 = mBal.OpenSocket(0, typeof(Ixxat.Vci4.Bal.Can.ICanScheduler)) as Ixxat.Vci4.Bal.Can.ICanScheduler;

        try
        {
          // Second OpenSocket
          canSched2 = mBal.OpenSocket(0, typeof(Ixxat.Vci4.Bal.Can.ICanScheduler)) as Ixxat.Vci4.Bal.Can.ICanScheduler;
          Assert.IsTrue(false);
        }
        catch (VciException)
        {
          // caught exception of second OpenSocket
          Assert.IsTrue(true);
        }
      }
      catch (NotImplementedException)
      {
        // caught exception of first OpenSocket
        Assert.IsTrue(true);
      }
      finally
      {
        if (null != canSched1)
        {
          canSched1.Dispose();
        }
        if (null != canSched2)
        {
          canSched2.Dispose();
        }
      }
    }

    [TestMethod]
    /// <summary>
    ///   OpenSocket ICanScheduler twice
    /// </summary>
    public void OpenSocketICanScheduler2Twice()
    {
      // Because not all of the available fieldbus adapter support
      // the ICanScheduler socket we use the first call to determine this.

      Ixxat.Vci4.Bal.Can.ICanScheduler2 canSched1 = null;
      Ixxat.Vci4.Bal.Can.ICanScheduler2 canSched2 = null;

      try
      {
        // First OpenSocket
        canSched1 = mBal.OpenSocket(0, typeof(Ixxat.Vci4.Bal.Can.ICanScheduler2)) as Ixxat.Vci4.Bal.Can.ICanScheduler2;

        try
        {
          // Second OpenSocket
          canSched2 = mBal.OpenSocket(0, typeof(Ixxat.Vci4.Bal.Can.ICanScheduler2)) as Ixxat.Vci4.Bal.Can.ICanScheduler2;
          Assert.IsTrue(false);
        }
        catch (VciException)
        {
          // caught exception of second OpenSocket
          Assert.IsTrue(true);
        }
      }
      catch (NotImplementedException)
      {
        // caught exception of first OpenSocket
        Assert.IsTrue(true);
      }
      finally
      {
        if (null != canSched1)
        {
          canSched1.Dispose();
        }
        if (null != canSched2)
        {
          canSched2.Dispose();
        }
      }
    }

    [TestMethod]
    /// <summary>
    ///   OpenSocket ICanSocket twice
    /// </summary>
    public void OpenSocketICanSocketTwice()
    {
      Ixxat.Vci4.Bal.Can.ICanSocket canSocket1;
      Ixxat.Vci4.Bal.Can.ICanSocket canSocket2;

      canSocket1 = mBal.OpenSocket(0, typeof(Ixxat.Vci4.Bal.Can.ICanSocket)) as Ixxat.Vci4.Bal.Can.ICanSocket;
      canSocket2 = mBal.OpenSocket(0, typeof(Ixxat.Vci4.Bal.Can.ICanSocket)) as Ixxat.Vci4.Bal.Can.ICanSocket;

      Assert.IsNotNull(canSocket1);
      Assert.IsNotNull(canSocket2);

      canSocket1.Dispose();
      canSocket2.Dispose();
    }

    [TestMethod]
    /// <summary>
    ///   OpenSocket ICanChannel twice
    /// </summary>
    public void OpenSocketICanChannelTwice()
    {
      Ixxat.Vci4.Bal.Can.ICanChannel canChannel1;
      Ixxat.Vci4.Bal.Can.ICanChannel canChannel2;

      canChannel1 = mBal.OpenSocket(0, typeof(Ixxat.Vci4.Bal.Can.ICanChannel)) as Ixxat.Vci4.Bal.Can.ICanChannel;
      canChannel2 = mBal.OpenSocket(0, typeof(Ixxat.Vci4.Bal.Can.ICanChannel)) as Ixxat.Vci4.Bal.Can.ICanChannel;

      Assert.IsNotNull(canChannel1);
      Assert.IsNotNull(canChannel2);

      canChannel1.Dispose();
      canChannel2.Dispose();
    }

    [TestMethod]
    /// <summary>
    ///   OpenSocket must throw ObjectDisposedException.
    /// </summary>
    [ExpectedException(typeof(ObjectDisposedException))]
    public void OpenSocketMustThrowObjectDisposedException()
    {
      mBal.Dispose();
      IBalResource socket = mBal.OpenSocket(0, typeof(IBalResource)) as IBalResource;
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
      IBalObject bal;
      BalResourceCollection resources;

      using (IVciDevice device = GetDevice())
      {
        using(bal = device.OpenBusAccessLayer())
        {
          resources = bal.Resources;
        }
      }

      // This call must throw an ObjectDisposedException
      resources = bal.Resources;
    }

    #endregion

  }
}
