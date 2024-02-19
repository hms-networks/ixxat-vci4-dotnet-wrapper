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
  public class BalResourceTest
    : VciDeviceTestBase
  {
    #region Member variables

    private IBalResource mResource;

    #endregion

    #region Test Initialize and Cleanup

    [TestInitialize]
    public void TestSetup()
    {
      Ixxat.Vci4.IVciDevice device = GetDevice();
      Ixxat.Vci4.Bal.IBalObject bal = device.OpenBusAccessLayer();

      mResource = bal.Resources[0];

      bal.Dispose();
      device.Dispose();
    }

    #endregion

    #region Property BusName Test methods

    [TestMethod]
    /// <summary>
    ///   BusName has same value for multiple calls.
    /// </summary>
    public void BusNameHasConstantValue()
    {
      String refValue = mResource.BusName;
      Assert.IsNotNull(refValue);
      Assert.IsTrue(0 < refValue.Length);

      String testValue = mResource.BusName;
      Assert.IsNotNull(testValue);
      Assert.IsTrue(testValue == refValue);
    }

    [TestMethod]
    /// <summary>
    ///   BusName must not throw ObjectDisposedException.
    /// </summary>
    public void BusNameMustNotThrowObjectDisposedException()
    {
      String refValue = mResource.BusName;
    }

    #endregion

    #region Property BusPort Test methods

    [TestMethod]
    /// <summary>
    ///   BusPort has constant value
    /// </summary>
    public void BusPortIsConstant()
    {
      Byte refValue = mResource.BusPort;

      Byte testValue = mResource.BusPort;
      Assert.IsTrue(refValue == testValue);
    }

    [TestMethod]
    /// <summary>
    ///   BusPort must not throw ObjectDisposedException.
    /// </summary>
    public void BusPortMustThrowObjectDisposedException()
    {
      mResource.Dispose();
      Byte refValue = mResource.BusPort;
    }

    #endregion

    #region Property BusType Test methods

    [TestMethod]
    /// <summary>
    ///   VciBusType has constant value
    /// </summary>
    public void VciBusTypeIsConstant()
    {
      VciBusType refValue = mResource.BusType;

      VciBusType testValue = mResource.BusType;
      Assert.IsTrue(refValue == testValue);
    }

    [TestMethod]
    /// <summary>
    ///   VciBusType must throw ObjectDisposedException.
    /// </summary>
    public void VciBusTypeMustThrowObjectDisposedException()
    {
      mResource.Dispose();
      VciBusType refValue = mResource.BusType;
    }

    #endregion

    #region Using Statement Test methods

    [TestMethod]
    /// <summary>
    ///   Tests compilation and functionality of using statement
    /// </summary>
    public void TestUsingStatement()
    {
      string name;

      using (IVciDevice device = GetDevice())
      {
        using(IBalObject bal = device.OpenBusAccessLayer())
        {
          using(IBalResource resource = bal.Resources[0])
          {
            name = resource.BusName;
          }
        }
      }

      // This call must throw an ObjectDisposedException.
      // ==> We cannot do this test here because this interface does
      //     not throw any exception !!
      //name = resource.BusName;
    }

    #endregion

  }
}
