/*****************************************************************************
 IXXAT Automation GmbH
******************************************************************************
 Summary : Declarations for the BAL resource descriptor class.
 Compiler: Microsoft VC++ 8
******************************************************************************
 all rights reserved
*****************************************************************************/

namespace Ixxat.Vci4.Bal 
{
  using System;
  using System.Collections;

  //*****************************************************************************
  /// <summary>
  ///   <c>IBalResource</c> is used in two manners: On one hand it represents the 
  ///   physical existance of field bus controlles via property 
  ///   <c>IBalObject.Resources</c>. 
  ///   On the other hand it's the base interface for several functional socket 
  ///   interfaces that can be opened via method <c>IBalObject.OpenSocket</c>.
  ///   When no longer needed the BAL object has to be disposed using the 
  ///   IDisposable interface. 
  /// </summary>
  /// <remarks>
  ///   The VCI interfaces provide access to native driver resources. Because the 
  ///   .NET garbage collector is only designed to manage memory, but not 
  ///   native OS and driver resources the caller is responsible to release this 
  ///   resources via calling <c>IDisposable.Dispose()</c> when the object is no 
  ///   longer needed. Otherwise native memory and resource leaks may occure.
  /// </remarks>
  //*****************************************************************************
  public interface IBalResource : IDisposable
  {
    //*****************************************************************************
    /// <summary>
    ///   Gets the port number of the BAL bus socket.
    /// </summary>
    /// <returns>
    ///   The port number of the BAL bus socket.
    /// </returns>
    //*****************************************************************************
    byte       BusPort     { get; }

    //*****************************************************************************
    /// <summary>
    ///   Gets the bus type of the BAL bus socket.
    /// </summary>
    /// <returns>
    ///   The bus type of the BAL bus socket.
    /// </returns>
    //*****************************************************************************
    VciBusType BusType     { get; }

    //*****************************************************************************
    /// <summary>
    ///   Gets the name of the bus.
    /// </summary>
    /// <returns>
    ///   If succeeded the name of the bus.
    ///   If failed a null reference (Nothing in Visual Basic).
    /// </returns>
    //*****************************************************************************
    String     BusName     { get; }
  };


  //*****************************************************************************
  /// <summary>
  ///   This class implements the BAL resource collection. It can be used
  ///   to iterate through the available BAL resources or to directly access
  ///   one via it's collection index.
  /// </summary>
  //*****************************************************************************
  sealed public class BalResourceCollection : ReadOnlyCollectionBase
  {
    //*****************************************************************************
    /// <summary>
    ///   Get's the BAL socket identified by the specified collection index.
    /// </summary>
    /// <param name="index">
    ///   Index of the requested BAL socket within this collection of BAL 
    ///   sockets.
    ///</param>
    /// <exception cref="ArgumentOutOfRangeException">
    ///   The specified index is not a valid index in the list.
    /// </exception>
    //*****************************************************************************
    public IBalResource? this[int index]
    {
      get
      {
        return (IBalResource?)InnerList[index];
      }
    }

    //--------------------------------------------------------------------
    // member functions
    //--------------------------------------------------------------------

    //*****************************************************************************
    /// <summary>
    /// Create a BalResourceCollection from a given IBalResource array.
    /// </summary>
    /// <param name="resources">Array of IBalResource interfaces</param>
    //*****************************************************************************
    public BalResourceCollection(IBalResource[] resources)
    {
      InnerList.AddRange(resources);
    }

    /*
    /// <summary>
    /// 
    /// </summary>
    /// <param name="array"></param>
    /// <param name="index"></param>
    public void CopyTo(IBalResource[] array, Int32 index)
    {
      InnerList.CopyTo(array, index);
    }
    */
  };

}