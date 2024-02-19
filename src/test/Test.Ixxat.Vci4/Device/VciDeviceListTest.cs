using System;
using System.Collections;
using System.Text;
using System.Threading;
using Ixxat.Vci4;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Vci4Tests
{
  [TestClass]
  public class VciDeviceListTest
  {
    IVciDeviceList mList = null;

    [TestInitialize]
    public void TestSetup()
    {
      IVciDeviceManager manager = VciServer.Instance().DeviceManager;
      mList = manager.GetDeviceList();
      manager.Dispose();
    }

    [TestCleanup]
    public void TestCleanup()
    {
      if (null != mList)
      {
        mList.Dispose();
        mList = null;
      }
    }

    #region AssignEvent Test methods

    [TestMethod]
    /// <summary>
    ///   AssignEvent must not throw an Exception.
    /// </summary>
    public void AssignEventMustNotThrowException()
    {
      ManualResetEvent manualEvent = new ManualResetEvent(false);
      AutoResetEvent autoEvent = new AutoResetEvent(false);

      mList.AssignEvent(autoEvent);
      mList.AssignEvent(manualEvent);
    }

    [TestMethod]
    /// <summary>
    ///   AssignEvent (auto) must throw ObjectDisposedException.
    /// </summary>
    [ExpectedException(typeof(ObjectDisposedException))]
    public void AssignEventAutoMustThrowObjectDisposedException()
    {
      mList.Dispose();

      AutoResetEvent changeEvent = new AutoResetEvent(false);
      mList.AssignEvent(changeEvent);
    }

    [TestMethod]
    /// <summary>
    ///   AssignEvent (manual) must throw ObjectDisposedException.
    /// </summary>
    [ExpectedException(typeof(ObjectDisposedException))]
    public void AssignEventManualMustThrowObjectDisposedException()
    {
      mList.Dispose();

      ManualResetEvent changeEvent = new ManualResetEvent(false);
      mList.AssignEvent(changeEvent);
    }

    #endregion

    #region GetEnumerator Test methods

    [TestMethod]
    /// <summary>
    ///   GetEnumerator must not return Null-Reference.
    /// </summary>
    public void GetEnumeratorMustNotReturnNull()
    {
      IEnumerator enumerator = mList.GetEnumerator();
      Assert.IsNotNull(enumerator);
    }

    [TestMethod]
    /// <summary>
    ///   GetEnumerator must throw ObjectDisposedException.
    /// </summary>
    [ExpectedException(typeof(ObjectDisposedException))]
    public void GetEnumeratorMustThrowObjectDisposedException()
    {
      mList.Dispose();

      mList.GetEnumerator();
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
      using (IVciDeviceManager manager = VciServer.Instance().DeviceManager)
      {
        IVciDeviceList list;
        using (list = manager.GetDeviceList())
        {
          list.AssignEvent(new AutoResetEvent(false));
        }

        // This call must throw an ObjectDisposedException
        list.AssignEvent(new AutoResetEvent(false));
      }

    }

    #endregion

  }
}
