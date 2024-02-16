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
  public class CanSocketTest2
    : VciDeviceTestBase
  {
    #region Member variables

    private Ixxat.Vci4.Bal.Can.ICanSocket2? mSocket;
    private Ixxat.Vci4.Bal.IBalObject? mBal;

    #endregion

    #region Test Initialize and Cleanup

    [TestInitialize]
    public void TestSetup()
    {
      Ixxat.Vci4.IVciDevice? device = GetDevice();
      mBal = device!.OpenBusAccessLayer();

      device!.Dispose();

      mSocket = mBal!.OpenSocket(0, typeof(Ixxat.Vci4.Bal.Can.ICanSocket2)) as Ixxat.Vci4.Bal.Can.ICanSocket2;
    }

    [TestCleanup]
    public void TestCleanup()
    {
      if (null != mSocket)
      {
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

    #region Property ControllerType Test methods

    [TestMethod]
    /// <summary>
    ///   ControllerType has constant value
    /// </summary>
    public void ControllerTypeIsConstant()
    {
      CanCtrlType refValue = mSocket!.ControllerType;
      
      CanCtrlType testValue = mSocket!.ControllerType;
      Assert.IsTrue(refValue == testValue);
    }

    [TestMethod]
    /// <summary>
    ///   ControllerType must throw ObjectDisposedException.
    /// </summary>
    [ExpectedException(typeof(ObjectDisposedException))]
    public void ControllerTypeMustThrowObjectDisposedException()
    {
      mSocket!.Dispose();
      CanCtrlType refValue = mSocket!.ControllerType;
    }

    #endregion

    #region Property BusCoupling Test methods

    [TestMethod]
    /// <summary>
    ///   BusCoupling has constant value
    /// </summary>
    public void BusCouplingIsConstant()
    {
      CanBusCouplings refValue = mSocket!.BusCoupling;

      CanBusCouplings testValue = mSocket!.BusCoupling;
      Assert.IsTrue(refValue == testValue);
    }

    [TestMethod]
    /// <summary>
    ///   BusCoupling must throw ObjectDisposedException.
    /// </summary>
    [ExpectedException(typeof(ObjectDisposedException))]
    public void BusCouplingMustThrowObjectDisposedException()
    {
      mSocket!.Dispose();
      CanBusCouplings refValue = mSocket!.BusCoupling;
    }

    #endregion

    #region Property Features Test methods

    [TestMethod]
    /// <summary>
    ///   Features has constant value
    /// </summary>
    public void FeaturesIsConstant()
    {
      CanFeatures refValue = mSocket!.Features;
      
      CanFeatures testValue = mSocket!.Features;
      Assert.IsTrue(refValue == testValue);
    }

    [TestMethod]
    /// <summary>
    ///   Features must throw ObjectDisposedException.
    /// </summary>
    [ExpectedException(typeof(ObjectDisposedException))]
    public void FeaturesMustThrowObjectDisposedException()
    {
      mSocket!.Dispose();
      CanFeatures refValue = mSocket!.Features;
    }

    #endregion

    #region Property CanClockFrequency Test methods

    [TestMethod]
    /// <summary>
    ///   CanClockFrequency has constant value
    /// </summary>
    public void CanClockFrequencyIsConstant()
    {
      uint refValue = mSocket!.CanClockFrequency;
      
      uint testValue = mSocket!.CanClockFrequency;
      Assert.IsTrue(refValue == testValue);
    }

    [TestMethod]
    /// <summary>
    ///   CanClockFrequency must throw ObjectDisposedException.
    /// </summary>
    [ExpectedException(typeof(ObjectDisposedException))]
    public void CanClockFrequencyMustThrowObjectDisposedException()
    {
      mSocket!.Dispose();
      uint refValue = mSocket!.CanClockFrequency;
    }

    #endregion

    #region Property MinimumArbitrationBitrate Test methods

    [TestMethod]
    /// <summary>
    ///   MinimumArbitrationBitrate has constant value
    /// </summary>
    public void MinimumArbitrationBitrateIsConstant()
    {
      CanBitrate2 refValue = mSocket!.MinimumArbitrationBitrate;
      
      CanBitrate2 testValue = mSocket!.MinimumArbitrationBitrate;
      Assert.IsTrue(refValue == testValue);
    }

    [TestMethod]
    /// <summary>
    ///   MinimumArbitrationBitrate must throw ObjectDisposedException.
    /// </summary>
    [ExpectedException(typeof(ObjectDisposedException))]
    public void MinimumArbitrationBitrateMustThrowObjectDisposedException()
    {
      mSocket!.Dispose();
      CanBitrate2 refValue = mSocket!.MinimumArbitrationBitrate;
    }

    #endregion

    #region Property MaximumArbitrationBitrate Test methods

    [TestMethod]
    /// <summary>
    ///   MaximumArbitrationBitrate has constant value
    /// </summary>
    public void MaximumArbitrationBitrateIsConstant()
    {
      CanBitrate2 refValue = mSocket!.MaximumArbitrationBitrate;
      
      CanBitrate2 testValue = mSocket!.MaximumArbitrationBitrate;
      Assert.IsTrue(refValue == testValue);
    }

    [TestMethod]
    /// <summary>
    ///   MaximumArbitrationBitrate must throw ObjectDisposedException.
    /// </summary>
    [ExpectedException(typeof(ObjectDisposedException))]
    public void MaximumArbitrationBitrateMustThrowObjectDisposedException()
    {
      mSocket!.Dispose();
      CanBitrate2 refValue = mSocket!.MaximumArbitrationBitrate;
    }

    #endregion

    #region Property MinimumFastDataBitrate Test methods

    [TestMethod]
    /// <summary>
    ///   MinimumFastDataBitrate has constant value
    /// </summary>
    public void MinimumFastDataBitrateIsConstant()
    {
      CanBitrate2 refValue = mSocket!.MinimumFastDataBitrate;
      
      CanBitrate2 testValue = mSocket!.MinimumFastDataBitrate;
      Assert.IsTrue(refValue == testValue);
    }

    [TestMethod]
    /// <summary>
    ///   MinimumFastDataBitrate must throw ObjectDisposedException.
    /// </summary>
    [ExpectedException(typeof(ObjectDisposedException))]
    public void MinimumFastDataBitrateMustThrowObjectDisposedException()
    {
      mSocket!.Dispose();
      CanBitrate2 refValue = mSocket!.MinimumFastDataBitrate;
    }

    #endregion

    #region Property MaximumFastDataBitrate Test methods

    [TestMethod]
    /// <summary>
    ///   MaximumFastDataBitrate has constant value
    /// </summary>
    public void MaximumFastDataBitrateIsConstant()
    {
      CanBitrate2 refValue = mSocket!.MaximumFastDataBitrate;
      
      CanBitrate2 testValue = mSocket!.MaximumFastDataBitrate;
      Assert.IsTrue(refValue == testValue);
    }

    [TestMethod]
    /// <summary>
    ///   MaximumFastDataBitrate must throw ObjectDisposedException.
    /// </summary>
    [ExpectedException(typeof(ObjectDisposedException))]
    public void MaximumFastDataBitrateMustThrowObjectDisposedException()
    {
      mSocket!.Dispose();
      CanBitrate2 refValue = mSocket!.MaximumFastDataBitrate;
    }

    #endregion

    #region Property TimeStampCounterClockFrequency Test methods

    [TestMethod]
    /// <summary>
    ///   TimeStampCounterClockFrequency has constant value
    /// </summary>
    public void TimeStampCounterClockFrequencyIsConstant()
    {
      uint refValue = mSocket!.TimeStampCounterClockFrequency;
      
      uint testValue = mSocket!.TimeStampCounterClockFrequency;
      Assert.IsTrue(refValue == testValue);
    }

    [TestMethod]
    /// <summary>
    ///   TimeStampCounterClockFrequency must throw ObjectDisposedException.
    /// </summary>
    [ExpectedException(typeof(ObjectDisposedException))]
    public void TimeStampCounterClockFrequencyMustThrowObjectDisposedException()
    {
      mSocket!.Dispose();
      uint refValue = mSocket!.TimeStampCounterClockFrequency;
    }

    #endregion

    #region Property TimeStampCounterDivisor Test methods

    [TestMethod]
    /// <summary>
    ///   TimeStampCounterDivisor has constant value
    /// </summary>
    public void TimeStampCounterDivisorIsConstant()
    {
      uint refValue = mSocket!.TimeStampCounterDivisor;
      
      uint testValue = mSocket!.TimeStampCounterDivisor;
      Assert.IsTrue(refValue == testValue);
    }

    [TestMethod]
    /// <summary>
    ///   TimeStampCounterDivisor must throw ObjectDisposedException.
    /// </summary>
    [ExpectedException(typeof(ObjectDisposedException))]
    public void TimeStampCounterDivisorMustThrowObjectDisposedException()
    {
      mSocket!.Dispose();
      uint refValue = mSocket!.TimeStampCounterDivisor;
    }

    #endregion

    #region Property CyclicMessageTimerClockFrequency Test methods

    [TestMethod]
    /// <summary>
    ///   CyclicMessageTimerClockFrequency has constant value
    /// </summary>
    public void CyclicMessageTimerClockFrequencyIsConstant()
    {
      uint refValue = mSocket!.CyclicMessageTimerClockFrequency;
      
      uint testValue = mSocket!.CyclicMessageTimerClockFrequency;
      Assert.IsTrue(refValue == testValue);
    }

    [TestMethod]
    /// <summary>
    ///   CyclicMessageTimerClockFrequency must throw ObjectDisposedException.
    /// </summary>
    [ExpectedException(typeof(ObjectDisposedException))]
    public void CyclicMessageTimerClockFrequencyMustThrowObjectDisposedException()
    {
      mSocket!.Dispose();
      uint refValue = mSocket!.CyclicMessageTimerClockFrequency;
    }

    #endregion

    #region Property CyclicMessageTimerDivisor Test methods

    [TestMethod]
    /// <summary>
    ///   CyclicMessageTimerDivisor has constant value
    /// </summary>
    public void CyclicMessageTimerDivisorIsConstant()
    {
      uint refValue = mSocket!.CyclicMessageTimerDivisor;
      
      uint testValue = mSocket!.CyclicMessageTimerDivisor;
      Assert.IsTrue(refValue == testValue);
    }

    [TestMethod]
    /// <summary>
    ///   CyclicMessageTimerDivisor must throw ObjectDisposedException.
    /// </summary>
    [ExpectedException(typeof(ObjectDisposedException))]
    public void CyclicMessageTimerDivisorMustThrowObjectDisposedException()
    {
      mSocket!.Dispose();
      uint refValue = mSocket!.CyclicMessageTimerDivisor;
    }

    #endregion

    #region Property MaxCyclicMessageTicks Test methods

    [TestMethod]
    /// <summary>
    ///   MaxCyclicMessageTicks has constant value
    /// </summary>
    public void MaxCyclicMessageTicksIsConstant()
    {
      uint refValue = mSocket!.MaxCyclicMessageTicks;
      
      uint testValue = mSocket!.MaxCyclicMessageTicks;
      Assert.IsTrue(refValue == testValue);
    }

    [TestMethod]
    /// <summary>
    ///   MaxCyclicMessageTicks must throw ObjectDisposedException.
    /// </summary>
    [ExpectedException(typeof(ObjectDisposedException))]
    public void MaxCyclicMessageTicksMustThrowObjectDisposedException()
    {
      mSocket!.Dispose();
      uint refValue = mSocket!.MaxCyclicMessageTicks;
    }

    #endregion

    #region Property DelayedTXTimerClockFrequency Test methods

    [TestMethod]
    /// <summary>
    ///   DelayedTXTimerClockFrequency has constant value
    /// </summary>
    public void DelayedTXTimerClockFrequencyIsConstant()
    {
      uint refValue = mSocket!.DelayedTXTimerClockFrequency;
      
      uint testValue = mSocket!.DelayedTXTimerClockFrequency;
      Assert.IsTrue(refValue == testValue);
    }

    [TestMethod]
    /// <summary>
    ///   DelayedTXTimerClockFrequency must throw ObjectDisposedException.
    /// </summary>
    [ExpectedException(typeof(ObjectDisposedException))]
    public void DelayedTXTimerClockFrequencyMustThrowObjectDisposedException()
    {
      mSocket!.Dispose();
      uint refValue = mSocket!.DelayedTXTimerClockFrequency;
    }

    #endregion

    #region Property DelayedTXTimerDivisor Test methods

    [TestMethod]
    /// <summary>
    ///   DelayedTXTimerDivisor has constant value
    /// </summary>
    public void DelayedTXTimerDivisorIsConstant()
    {
      uint refValue = mSocket!.DelayedTXTimerDivisor;
      
      uint testValue = mSocket!.DelayedTXTimerDivisor;
      Assert.IsTrue(refValue == testValue);
    }

    [TestMethod]
    /// <summary>
    ///   DelayedTXTimerDivisor must throw ObjectDisposedException.
    /// </summary>
    [ExpectedException(typeof(ObjectDisposedException))]
    public void DelayedTXTimerDivisorMustThrowObjectDisposedException()
    {
      mSocket!.Dispose();
      uint refValue = mSocket!.DelayedTXTimerDivisor;
    }

    #endregion

    #region Property MaxDelayedTXTicks Test methods

    [TestMethod]
    /// <summary>
    ///   MaxDelayedTXTicks has constant value
    /// </summary>
    public void MaxDelayedTXTicksIsConstant()
    {
      uint refValue = mSocket!.MaxDelayedTXTicks;
      
      uint testValue = mSocket!.MaxDelayedTXTicks;
      Assert.IsTrue(refValue == testValue);
    }

    [TestMethod]
    /// <summary>
    ///   MaxDelayedTXTicks must throw ObjectDisposedException.
    /// </summary>
    [ExpectedException(typeof(ObjectDisposedException))]
    public void MaxDelayedTXTicksMustThrowObjectDisposedException()
    {
      mSocket!.Dispose();
      uint refValue = mSocket!.MaxDelayedTXTicks;
    }

    #endregion

    #region Property LineStatus Test methods

    [TestMethod]
    /// <summary>
    ///   LineStatus has constant value
    /// </summary>
    public void LineStatusIsConstant()
    {
      CanCtrlStatus refValue = mSocket!.LineStatus.ControllerStatus;
      
      CanCtrlStatus testValue = mSocket!.LineStatus.ControllerStatus;
      Assert.IsTrue(refValue == testValue);
    }

    [TestMethod]
    /// <summary>
    ///   LineStatus must throw ObjectDisposedException.
    /// </summary>
    [ExpectedException(typeof(ObjectDisposedException))]
    public void LineStatusMustThrowObjectDisposedException()
    {
      mSocket!.Dispose();
      CanLineStatus2 refValue = mSocket!.LineStatus;
    }

    #endregion

    #region Property SupportsStdOrExtFrames Test methods

    [TestMethod]
    /// <summary>
    ///   SupportsStdOrExtFrames has constant value
    /// </summary>
    public void SupportsStdOrExtFramesIsConstant()
    {
      bool refValue = mSocket!.SupportsStdOrExtFrames;
      
      bool testValue = mSocket!.SupportsStdOrExtFrames;
      Assert.IsTrue(refValue == testValue);
    }

    [TestMethod]
    /// <summary>
    ///   SupportsStdOrExtFrames must throw ObjectDisposedException.
    /// </summary>
    [ExpectedException(typeof(ObjectDisposedException))]
    public void SupportsStdOrExtFramesMustThrowObjectDisposedException()
    {
      mSocket!.Dispose();
      bool refValue = mSocket!.SupportsStdOrExtFrames;
    }

    #endregion

    #region Property SupportsStdAndExtFrames Test methods

    [TestMethod]
    /// <summary>
    ///   SupportsStdAndExtFrames has constant value
    /// </summary>
    public void SupportsStdAndExtFramesIsConstant()
    {
      bool refValue = mSocket!.SupportsStdAndExtFrames;
      
      bool testValue = mSocket!.SupportsStdAndExtFrames;
      Assert.IsTrue(refValue == testValue);
    }

    [TestMethod]
    /// <summary>
    ///   SupportsStdAndExtFrames must throw ObjectDisposedException.
    /// </summary>
    [ExpectedException(typeof(ObjectDisposedException))]
    public void SupportsStdAndExtFramesMustThrowObjectDisposedException()
    {
      mSocket!.Dispose();
      bool refValue = mSocket!.SupportsStdAndExtFrames;
    }

    #endregion

    #region Property SupportsRemoteFrames Test methods

    [TestMethod]
    /// <summary>
    ///   SupportsRemoteFrames has constant value
    /// </summary>
    public void SupportsRemoteFramesIsConstant()
    {
      bool refValue = mSocket!.SupportsRemoteFrames;
      
      bool testValue = mSocket!.SupportsRemoteFrames;
      Assert.IsTrue(refValue == testValue);
    }

    [TestMethod]
    /// <summary>
    ///   SupportsRemoteFrames must throw ObjectDisposedException.
    /// </summary>
    [ExpectedException(typeof(ObjectDisposedException))]
    public void SupportsRemoteFramesMustThrowObjectDisposedException()
    {
      mSocket!.Dispose();
      bool refValue = mSocket!.SupportsRemoteFrames;
    }

    #endregion

    #region Property SupportsErrorFrames Test methods

    [TestMethod]
    /// <summary>
    ///   SupportsErrorFrames has constant value
    /// </summary>
    public void SupportsErrorFramesIsConstant()
    {
      bool refValue = mSocket!.SupportsErrorFrames;
      
      bool testValue = mSocket!.SupportsErrorFrames;
      Assert.IsTrue(refValue == testValue);
    }

    [TestMethod]
    /// <summary>
    ///   SupportsErrorFrames must throw ObjectDisposedException.
    /// </summary>
    [ExpectedException(typeof(ObjectDisposedException))]
    public void SupportsErrorFramesMustThrowObjectDisposedException()
    {
      mSocket!.Dispose();
      bool refValue = mSocket!.SupportsErrorFrames;
    }

    #endregion

    #region Property SupportsBusLoadComputation Test methods

    [TestMethod]
    /// <summary>
    ///   SupportsBusLoadComputation has constant value
    /// </summary>
    public void SupportsBusLoadComputationIsConstant()
    {
      bool refValue = mSocket!.SupportsBusLoadComputation;
      
      bool testValue = mSocket!.SupportsBusLoadComputation;
      Assert.IsTrue(refValue == testValue);
    }

    [TestMethod]
    /// <summary>
    ///   SupportsBusLoadComputation must throw ObjectDisposedException.
    /// </summary>
    [ExpectedException(typeof(ObjectDisposedException))]
    public void SupportsBusLoadComputationMustThrowObjectDisposedException()
    {
      mSocket!.Dispose();
      bool refValue = mSocket!.SupportsBusLoadComputation;
    }

    #endregion

    #region Property SupportsExactMessageFilter Test methods

    [TestMethod]
    /// <summary>
    ///   SupportsExactMessageFilter has constant value
    /// </summary>
    public void SupportsExactMessageFilterIsConstant()
    {
      bool refValue = mSocket!.SupportsExactMessageFilter;
      
      bool testValue = mSocket!.SupportsExactMessageFilter;
      Assert.IsTrue(refValue == testValue);
    }

    [TestMethod]
    /// <summary>
    ///   SupportsExactMessageFilter must throw ObjectDisposedException.
    /// </summary>
    [ExpectedException(typeof(ObjectDisposedException))]
    public void SupportsExactMessageFilterMustThrowObjectDisposedException()
    {
      mSocket!.Dispose();
      bool refValue = mSocket!.SupportsExactMessageFilter;
    }

    #endregion

    #region Property SupportsListenOnlyMode Test methods

    [TestMethod]
    /// <summary>
    ///   SupportsListenOnlyMode has constant value
    /// </summary>
    public void SupportsListenOnlyModeIsConstant()
    {
      bool refValue = mSocket!.SupportsListenOnlyMode;
      
      bool testValue = mSocket!.SupportsListenOnlyMode;
      Assert.IsTrue(refValue == testValue);
    }

    [TestMethod]
    /// <summary>
    ///   SupportsListenOnlyMode must throw ObjectDisposedException.
    /// </summary>
    [ExpectedException(typeof(ObjectDisposedException))]
    public void SupportsListenOnlyModeMustThrowObjectDisposedException()
    {
      mSocket!.Dispose();
      bool refValue = mSocket!.SupportsListenOnlyMode;
    }

    #endregion

    #region Property SupportsCyclicMessageScheduler Test methods

    [TestMethod]
    /// <summary>
    ///   SupportsCyclicMessageScheduler has constant value
    /// </summary>
    public void SupportsCyclicMessageSchedulerIsConstant()
    {
      bool refValue = mSocket!.SupportsCyclicMessageScheduler;
      
      bool testValue = mSocket!.SupportsCyclicMessageScheduler;
      Assert.IsTrue(refValue == testValue);
    }

    [TestMethod]
    /// <summary>
    ///   SupportsCyclicMessageScheduler must throw ObjectDisposedException.
    /// </summary>
    [ExpectedException(typeof(ObjectDisposedException))]
    public void SupportsCyclicMessageSchedulerMustThrowObjectDisposedException()
    {
      mSocket!.Dispose();
      bool refValue = mSocket!.SupportsCyclicMessageScheduler;
    }

    #endregion

    #region Property SupportsErrorFrameGeneration Test methods

    [TestMethod]
    /// <summary>
    ///   SupportsErrorFrameGeneration has constant value
    /// </summary>
    public void SupportsErrorFrameGenerationIsConstant()
    {
      bool refValue = mSocket!.SupportsErrorFrameGeneration;
      
      bool testValue = mSocket!.SupportsErrorFrameGeneration;
      Assert.IsTrue(refValue == testValue);
    }

    [TestMethod]
    /// <summary>
    ///   SupportsErrorFrameGeneration must throw ObjectDisposedException.
    /// </summary>
    [ExpectedException(typeof(ObjectDisposedException))]
    public void SupportsErrorFrameGenerationMustThrowObjectDisposedException()
    {
      mSocket!.Dispose();
      bool refValue = mSocket!.SupportsErrorFrameGeneration;
    }

    #endregion

    #region Property SupportsDelayedTransmission Test methods

    [TestMethod]
    /// <summary>
    ///   SupportsDelayedTransmission has constant value
    /// </summary>
    public void SupportsDelayedTransmissionIsConstant()
    {
      bool refValue = mSocket!.SupportsDelayedTransmission;
      
      bool testValue = mSocket!.SupportsDelayedTransmission;
      Assert.IsTrue(refValue == testValue);
    }

    [TestMethod]
    /// <summary>
    ///   SupportsDelayedTransmission must throw ObjectDisposedException.
    /// </summary>
    [ExpectedException(typeof(ObjectDisposedException))]
    public void SupportsDelayedTransmissionMustThrowObjectDisposedException()
    {
      mSocket!.Dispose();
      bool refValue = mSocket!.SupportsDelayedTransmission;
    }

    #endregion

    #region Property SupportsSingleShotMessages Test methods

    [TestMethod]
    /// <summary>
    ///   SupportsSingleShotMessages has constant value
    /// </summary>
    public void SupportsSingleShotMessagesIsConstant()
    {
      bool refValue = mSocket!.SupportsSingleShotMessages;
      
      bool testValue = mSocket!.SupportsSingleShotMessages;
      Assert.IsTrue(refValue == testValue);
    }

    [TestMethod]
    /// <summary>
    ///   SupportsSingleShotMessages must throw ObjectDisposedException.
    /// </summary>
    [ExpectedException(typeof(ObjectDisposedException))]
    public void SupportsSingleShotMessagesMustThrowObjectDisposedException()
    {
      mSocket!.Dispose();
      bool refValue = mSocket!.SupportsSingleShotMessages;
    }

    #endregion

    #region Property SupportsHighPriorityMessages Test methods

    [TestMethod]
    /// <summary>
    ///   SupportsHighPriorityMessages has constant value
    /// </summary>
    public void SupportsHighPriorityMessagesIsConstant()
    {
      bool refValue = mSocket!.SupportsHighPriorityMessages;
      
      bool testValue = mSocket!.SupportsHighPriorityMessages;
      Assert.IsTrue(refValue == testValue);
    }

    [TestMethod]
    /// <summary>
    ///   SupportsHighPriorityMessages must throw ObjectDisposedException.
    /// </summary>
    [ExpectedException(typeof(ObjectDisposedException))]
    public void SupportsHighPriorityMessagesMustThrowObjectDisposedException()
    {
      mSocket!.Dispose();
      bool refValue = mSocket!.SupportsHighPriorityMessages;
    }

    #endregion

    #region Property SupportsAutoBaudrateDetection Test methods

    [TestMethod]
    /// <summary>
    ///   SupportsAutoBaudrateDetection has constant value
    /// </summary>
    public void SupportsAutoBaudrateDetectionIsConstant()
    {
      bool refValue = mSocket!.SupportsAutoBaudrateDetection;
      
      bool testValue = mSocket!.SupportsAutoBaudrateDetection;
      Assert.IsTrue(refValue == testValue);
    }

    [TestMethod]
    /// <summary>
    ///   SupportsAutoBaudrateDetection must throw ObjectDisposedException.
    /// </summary>
    [ExpectedException(typeof(ObjectDisposedException))]
    public void SupportsAutoBaudrateDetectionMustThrowObjectDisposedException()
    {
      mSocket!.Dispose();
      bool refValue = mSocket!.SupportsAutoBaudrateDetection;
    }

    #endregion

    #region Property SupportsExtendedDataLength Test methods

    [TestMethod]
    /// <summary>
    ///   SupportsExtendedDataLength has constant value
    /// </summary>
    public void SupportsExtendedDataLengthIsConstant()
    {
      bool refValue = mSocket!.SupportsExtendedDataLength;
      
      bool testValue = mSocket!.SupportsExtendedDataLength;
      Assert.IsTrue(refValue == testValue);
    }

    [TestMethod]
    /// <summary>
    ///   SupportsExtendedDataLength must throw ObjectDisposedException.
    /// </summary>
    [ExpectedException(typeof(ObjectDisposedException))]
    public void SupportsExtendedDataLengthMustThrowObjectDisposedException()
    {
      mSocket!.Dispose();
      bool refValue = mSocket!.SupportsExtendedDataLength;
    }

    #endregion

    #region Property SupportsFastDataRate Test methods

    [TestMethod]
    /// <summary>
    ///   SupportsFastDataRate has constant value
    /// </summary>
    public void SupportsFastDataRateIsConstant()
    {
      bool refValue = mSocket!.SupportsFastDataRate;
      
      bool testValue = mSocket!.SupportsFastDataRate;
      Assert.IsTrue(refValue == testValue);
    }

    [TestMethod]
    /// <summary>
    ///   SupportsFastDataRate must throw ObjectDisposedException.
    /// </summary>
    [ExpectedException(typeof(ObjectDisposedException))]
    public void SupportsFastDataRateMustThrowObjectDisposedException()
    {
      mSocket!.Dispose();
      bool refValue = mSocket!.SupportsFastDataRate;
    }

    #endregion

    #region Property SupportsIsoCanFdFrames Test methods

    [TestMethod]
    /// <summary>
    ///   SupportsIsoCanFdFrames has constant value
    /// </summary>
    public void SupportsIsoCanFdFramesIsConstant()
    {
      bool refValue = mSocket!.SupportsIsoCanFdFrames;
      
      bool testValue = mSocket!.SupportsIsoCanFdFrames;
      Assert.IsTrue(refValue == testValue);
    }

    [TestMethod]
    /// <summary>
    ///   SupportsIsoCanFdFrames must throw ObjectDisposedException.
    /// </summary>
    [ExpectedException(typeof(ObjectDisposedException))]
    public void SupportsIsoCanFdFramesMustThrowObjectDisposedException()
    {
      mSocket!.Dispose();
      bool refValue = mSocket!.SupportsIsoCanFdFrames;
    }

    #endregion

    #region Property SupportsNonIsoCanFdFrames Test methods

    [TestMethod]
    /// <summary>
    ///   SupportsNonIsoCanFdFrames has constant value
    /// </summary>
    public void SupportsNonIsoCanFdFramesIsConstant()
    {
      bool refValue = mSocket!.SupportsNonIsoCanFdFrames;
      
      bool testValue = mSocket!.SupportsNonIsoCanFdFrames;
      Assert.IsTrue(refValue == testValue);
    }

    [TestMethod]
    /// <summary>
    ///   SupportsNonIsoCanFdFrames must throw ObjectDisposedException.
    /// </summary>
    [ExpectedException(typeof(ObjectDisposedException))]
    public void SupportsNonIsoCanFdFramesMustThrowObjectDisposedException()
    {
      mSocket!.Dispose();
      bool refValue = mSocket!.SupportsNonIsoCanFdFrames;
    }

    #endregion

    #region Property Supports64BitTimeStamps Test methods

    [TestMethod]
    /// <summary>
    ///   Supports64BitTimeStamps has constant value
    /// </summary>
    public void Supports64BitTimeStampsIsConstant()
    {
      bool refValue = mSocket!.Supports64BitTimeStamps;
      
      bool testValue = mSocket!.Supports64BitTimeStamps;
      Assert.IsTrue(refValue == testValue);
    }

    [TestMethod]
    /// <summary>
    ///   Supports64BitTimeStamps must throw ObjectDisposedException.
    /// </summary>
    [ExpectedException(typeof(ObjectDisposedException))]
    public void Supports64BitTimeStampsMustThrowObjectDisposedException()
    {
      mSocket!.Dispose();
      bool refValue = mSocket!.Supports64BitTimeStamps;
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
      CanBusCouplings coupling;
      ICanSocket? socket;
      using (socket = mBal!.OpenSocket(0, typeof(Ixxat.Vci4.Bal.Can.ICanSocket)) as Ixxat.Vci4.Bal.Can.ICanSocket)
      {
        coupling = socket!.BusCoupling;
      }

      // This call must throw an ObjectDisposedException
      coupling = socket!.BusCoupling;
    }

    #endregion

  }
}
