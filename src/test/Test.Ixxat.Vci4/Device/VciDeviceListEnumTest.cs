using System;
using System.Collections;
using System.Text;
using System.Threading;
using Ixxat.Vci4;


namespace Vci4Tests
{
  [TestClass]
  public class VciDeviceListEnumTest
  {
    #region Member variables

    private IVciDeviceList? mList;
    private IEnumerator? mEnumerator;

    #endregion

    #region Test Fixture SetUp and TearDown

    [TestInitialize]
    public void TestSetup()
    {
      IVciDeviceManager manager = VciServer.Instance()!.DeviceManager;
      mList = manager!.GetDeviceList();
      mEnumerator = mList.GetEnumerator();
      manager!.Dispose();
    }

    [TestCleanup]
    public void TestCleanup()
    {
      if (null != mEnumerator)
      {
        (mEnumerator as IDisposable)!.Dispose();
        mEnumerator = null;
      }
      if (null != mList)
      {
        mList.Dispose();
        mList = null;
      }
    }

    #endregion

    #region Current Test methods

    [TestMethod]
    /// <summary>
    ///   Current must throw InvalidOperationException before MoveNext()
    /// </summary>
    [ExpectedException(typeof(InvalidOperationException))]
    public void CurrentThrowsInvalidOperationExceptionBeforeMoveNext()
    {
      object obj = mEnumerator!.Current;
    }

    [TestMethod]
    /// <summary>
    ///   Current must returns same value for multiple calls
    /// </summary>
    public void CurrentSameResultForMultiCall()
    {
      mEnumerator!.MoveNext();
      object firstResult = mEnumerator!.Current;

      Assert.IsTrue(firstResult.Equals(mEnumerator.Current));
    }

    [TestMethod]
    /// <summary>
    ///   Current must throw ObjectDisposedException.
    /// </summary>
    [ExpectedException(typeof(ObjectDisposedException))]
    public void CurrentMustThrowObjectDisposedException()
    {
      mEnumerator!.MoveNext();
      (mEnumerator as IDisposable)!.Dispose();

      object obj = mEnumerator!.Current;
    }

    [TestMethod]
    /// <summary>
    ///   Current must throw InvalidOperationException if enumerator
    ///   exeeded the end.
    /// </summary>
    [ExpectedException(typeof(InvalidOperationException))]
    public void CurrentThrowsInvalidOperationExceptionAtEnumEnd()
    {
      while (mEnumerator!.MoveNext()) ;
      object obj = mEnumerator!.Current;
    }

    #endregion

    #region MoveNext Test methods

    [TestMethod]
    /// <summary>
    ///   MoveNext result has to be true unless enumerator exeeded it's end.
    /// </summary>
    public void MoveNextReturnsTrueUntilEnd()
    {
      object? current = null;
      bool moveNextResult = false;

      do
      {
        moveNextResult = mEnumerator!.MoveNext();

        try 
        { 
          current = mEnumerator!.Current; 
        }
        catch (Exception) 
        {
          current = null;
        }

        if (null != current)
        {
          Assert.IsTrue(moveNextResult);
        }
        else
        {
          Assert.IsFalse(moveNextResult);
        }
      } while (current != null);

      Assert.IsFalse(mEnumerator.MoveNext());
    }

    [TestMethod]
    /// <summary>
    ///   MoveNext returns true after Reset()
    /// </summary>
    public void MoveNextReturnsTrueAfterReset()
    {
      while (mEnumerator!.MoveNext()) ;
      mEnumerator!.Reset();
      Assert.IsTrue(mEnumerator!.MoveNext());
      while (mEnumerator!.MoveNext()) ;
    }

    [TestMethod]
    /// <summary>
    ///   After MoveNext returns true the Current property must not be null.
    /// </summary>
    public void MoveNextIsTrueAndCurrentIsValid()
    {
      while (mEnumerator!.MoveNext())
      {
        Assert.IsNotNull(mEnumerator!.Current);
      }
    }

    [TestMethod]
    /// <summary>
    ///   MoveNext must throw ObjectDisposedException.
    /// </summary>
    [ExpectedException(typeof(ObjectDisposedException))]
    public void MoveNextMustThrowObjectDisposedException()
    {
      (mEnumerator as IDisposable)!.Dispose();
      mEnumerator!.MoveNext();
    }

    #endregion

    #region Reset Test methods

    [TestMethod]
    /// <summary>
    ///   Reset causes InvalidOperationException for property Current
    /// </summary>
    [ExpectedException(typeof(InvalidOperationException))]
    public void ResetCausesInvalidOperationExceptionForCurrent1()
    {
      mEnumerator!.Reset();
      object obj = mEnumerator!.Current;
    }

    [TestMethod]
    /// <summary>
    ///   Reset causes InvalidOperationException for property Current
    /// </summary>
    [ExpectedException(typeof(InvalidOperationException))]
    public void ResetCausesInvalidOperationExceptionForCurrent2()
    {
      mEnumerator!.MoveNext();
      mEnumerator!.Reset();
      object obj = mEnumerator!.Current;
    }

    [TestMethod]
    /// <summary>
    ///   Reset causes MoveNext to be true
    /// </summary>
    public void ResetCausesMoveNextToBeTrue()
    {
      while (mEnumerator!.MoveNext()) ;
      mEnumerator!.Reset();
      Assert.IsTrue(mEnumerator!.MoveNext());
      while (mEnumerator!.MoveNext()) ;
    }

    [TestMethod]
    /// <summary>
    ///   Reset must throw ObjectDisposedException.
    /// </summary>
    [ExpectedException(typeof(ObjectDisposedException))]
    public void ResetMustThrowObjectDisposedException()
    {
      (mEnumerator as IDisposable)!.Dispose();
      mEnumerator!.Reset();
    }

    #endregion
  }
}
