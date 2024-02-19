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
  public class BalResourceCollectionTest
    : VciDeviceTestBase
  {
    #region Member variables

    private BalResourceCollection mCollection = null;

    #endregion

    #region Test Initialize and Cleanup

    [TestInitialize]
    public void TestSetup()
    {
      Ixxat.Vci4.IVciDevice device = GetDevice();
      Ixxat.Vci4.Bal.IBalObject bal = device.OpenBusAccessLayer();

      mCollection = bal.Resources;

      bal.Dispose();
      device.Dispose();
    }

    #endregion

    #region Property Item Test methods

    [TestMethod]
    /// <summary>
    ///   Item must not be a null reference
    /// </summary>
    public void ItemMustNotBeNull()
    {
      for (int index = 0; index < mCollection.Count; ++index)
      {
        IBalResource res = mCollection[index];
        Assert.IsNotNull(res);
        res.Dispose();
      }
    }

    [TestMethod]
    /// <summary>
    ///   Item must throw ArgumentOutOfRangeException.
    /// </summary>
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void ItemMustThrowArgumentOutOfRangeException()
    {
      IBalResource res = mCollection[mCollection.Count];
    }

    #endregion
  }
}
