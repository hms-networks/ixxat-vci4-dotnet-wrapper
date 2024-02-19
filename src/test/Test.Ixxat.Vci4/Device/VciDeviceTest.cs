using System;
using System.Collections;
using Ixxat.Vci4;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Vci4Tests
{
  [TestClass]
  public class VciDeviceTests 
    : VciDeviceTestBase
  {
    #region Member variables

    Ixxat.Vci4.IVciDevice mDevice = null;

    #endregion

    #region Test Initialize and Cleanup

    [TestInitialize]
    public void TestSetup()
    {
      mDevice = GetDevice();
    }

    [TestCleanup]
    public void TestCleanup()
    {
      if (null != mDevice)
      {
        mDevice.Dispose();
        mDevice = null;
      }
    }

    #endregion

    [TestMethod]
    /// <summary>
    ///   Description has same value for multiple calls.
    /// </summary>
    public void DeviceDescriptionHasConstantValue()
    {
      String refValue = mDevice.Description;
      Assert.IsNotNull(refValue);
      Assert.IsTrue(0 < refValue.Length);

      String testValue = mDevice.Description;
      Assert.IsNotNull(testValue);
      Assert.IsTrue(testValue == refValue);
    }

    [TestMethod]
    /// <summary>
    ///   Description must throw ObjectDisposedException.
    /// </summary>
    [ExpectedException(typeof(ObjectDisposedException))]
    public void DescriptionMustThrowObjectDisposedException()
    {
      mDevice.Dispose();
      String refValue = mDevice.Description;
    }

    [TestMethod]
    /// <summary>
    ///   DeviceClass has same value for multiple calls.
    /// </summary>
    public void DeviceClassHasConstantValue()
    {
      Guid DeviceClass = mDevice.DeviceClass;
      Assert.IsTrue(DeviceClass == mDevice.DeviceClass);
    }

    [TestMethod]
    /// <summary>
    ///   DeviceClass must throw ObjectDisposedException.
    /// </summary>
    [ExpectedException(typeof(ObjectDisposedException))]
    public void DeviceClassMustThrowObjectDisposedException()
    {
      mDevice.Dispose();
      Guid DeviceClass = mDevice.DeviceClass;
    }

    [TestMethod]
    /// <summary>
    ///   UniqueHardwareId has constant value
    /// </summary>
    public void UniqueHardwareIdIsConstant()
    {
      object id = mDevice.UniqueHardwareId;
      Assert.IsNotNull(id);
      Assert.IsTrue((typeof(string) == id.GetType()) || (typeof(Guid) == id.GetType()));
      if (typeof(string) == id.GetType())
      {
        Assert.IsTrue(0 < (id as string).Length);
      }

      object testId = mDevice.UniqueHardwareId;
      Assert.IsNotNull(testId);
      Assert.IsTrue(id.Equals(testId));
    }

    [TestMethod]
    /// <summary>
    ///   UniqueHardwareId must throw ObjectDisposedException.
    /// </summary>
    [ExpectedException(typeof(ObjectDisposedException))]
    public void UniqueHardwareIdMustThrowObjectDisposedException()
    {
      mDevice.Dispose();
      object id = mDevice.UniqueHardwareId;
    }

    [TestMethod]
    /// <summary>
    ///   DriverVersion has constant value
    /// </summary>
    public void DriverVersionIsConstant()
    {
      Version version = mDevice.DriverVersion;

      Version testVersion = mDevice.DriverVersion;
      Assert.IsNotNull(testVersion);
      Assert.IsTrue(version == testVersion);
    }

    [TestMethod]
    /// <summary>
    ///   DriverVersion must throw ObjectDisposedException.
    /// </summary>
    [ExpectedException(typeof(ObjectDisposedException))]
    public void DriverVersionMustThrowObjectDisposedException()
    {
      mDevice.Dispose();
      Version version = mDevice.DriverVersion;
    }

    [TestMethod]
    /// <summary>
    ///   HardwareVersion has constant value
    /// </summary>
    public void HardwareVersionIsConstant()
    {
      Version version = mDevice.HardwareVersion;

      Version testVersion = mDevice.HardwareVersion;
      Assert.IsNotNull(testVersion);
      Assert.IsTrue(version == testVersion);
    }

    [TestMethod]
    /// <summary>
    ///   HardwareVersion must throw ObjectDisposedException.
    /// </summary>
    [ExpectedException(typeof(ObjectDisposedException))]
    public void HardwareVersionMustThrowObjectDisposedException()
    {
      mDevice.Dispose();
      Version version = mDevice.HardwareVersion;
    }

    [TestMethod]
    /// <summary>
    ///   Manufacturer has same value for multiple calls.
    /// </summary>
    public void ManufacturerHasConstantValue()
    {
      String refValue = mDevice.Manufacturer;
      Assert.IsNotNull(refValue);
      Assert.IsTrue(0 < refValue.Length);

      String testValue = mDevice.Manufacturer;
      Assert.IsNotNull(testValue);
      Assert.IsTrue(testValue == refValue);
    }

    [TestMethod]
    /// <summary>
    ///   Manufacturer must throw ObjectDisposedException.
    /// </summary>
    [ExpectedException(typeof(ObjectDisposedException))]
    public void ManufacturerMustThrowObjectDisposedException()
    {
      mDevice.Dispose();
      String refValue = mDevice.Manufacturer;
    }

    [TestMethod]
    /// <summary>
    ///   VciObjectId has constant value
    /// </summary>
    public void VciObjectIdIsConstant()
    {
      long id = mDevice.VciObjectId;

      long testId = mDevice.VciObjectId;
      Assert.IsNotNull(testId);
      Assert.IsTrue(id == testId);
    }

    [TestMethod]
    /// <summary>
    ///   VciObjectId must throw ObjectDisposedException.
    /// </summary>
    [ExpectedException(typeof(ObjectDisposedException))]
    public void VciObjectIdMustThrowObjectDisposedException()
    {
      mDevice.Dispose();
      long id = mDevice.VciObjectId;
    }

    [TestMethod]
    /// <summary>
    ///   Equipment has same value for multiple calls.
    /// </summary>
    public void EquipmentHasConstantValue()
    {
      IVciCtrlInfo[] refValue = mDevice.Equipment;
      Assert.IsNotNull(refValue);
      Assert.IsTrue(0 < refValue.Length);

      IVciCtrlInfo[] testValue = mDevice.Equipment;
      Assert.IsNotNull(testValue);
      Assert.IsTrue(refValue.Length == testValue.Length);
    }

    [TestMethod]
    /// <summary>
    ///   Equipment must throw ObjectDisposedException.
    /// </summary>
    [ExpectedException(typeof(ObjectDisposedException))]
    public void EquipmentMustThrowObjectDisposedException()
    {
      mDevice.Dispose();
      IVciCtrlInfo[] refValue = mDevice.Equipment;
    }

    [TestMethod]
    /// <summary>
    ///   OpenBusAccessLayer returns valid reference.
    /// </summary>
    public void OpenBusAccessLayerReturnsValidRef()
    {
      Ixxat.Vci4.Bal.IBalObject bal = mDevice.OpenBusAccessLayer();
      Assert.IsNotNull(bal);
      bal.Dispose();
    }

    [TestMethod]
    /// <summary>
    ///   OpenBusAccessLayer must throw ObjectDisposedException.
    /// </summary>
    [ExpectedException(typeof(ObjectDisposedException))]
    public void OpenBusAccessLayerMustThrowObjectDisposedException()
    {
      mDevice.Dispose();
      Ixxat.Vci4.Bal.IBalObject bal = mDevice.OpenBusAccessLayer();
    }

    [TestMethod]
    /// <summary>
    ///   Tests compilation and functionality of using statement
    /// </summary>
    [ExpectedException(typeof(ObjectDisposedException))]
    public void TestUsingStatement()
    {
      Version version;
      IVciDevice device;
      using (device = GetDevice())
      {
        version = device.DriverVersion;
      }

      // This call must throw an ObjectDisposedException
      version = device.DriverVersion;
    }


  }
}