/*****************************************************************************
 IXXAT Automation GmbH
******************************************************************************
 Summary : Declarations for the CAN channel status class.
 Compiler: Microsoft VC++ 8
******************************************************************************
 all rights reserved
*****************************************************************************/

/*************************************************************************
** namespace Ixxat.Vci4.Bal.Can
*************************************************************************/
namespace Ixxat.Vci4.Bal.Can 
{

  /*************************************************************************
  ** used namespace
  *************************************************************************/
  using System.Threading;
  using System.Runtime.InteropServices;

  //*****************************************************************************
  /// <summary>
  ///   <c>CanChannelStatus</c> represents the status of a CAN channel.
  ///   See interface <c>ICanChannel</c>.
  /// </summary>
  //*****************************************************************************
  public struct CanChannelStatus
  {
    private bool    fActivated;         // TRUE if the channel is activated
    private bool    fRxOverrun;         // TRUE if receive FIFO overrun occured
    private byte    bRxFifoLoad;        // receive FIFO load in percent (0..100)
    private byte    bTxFifoLoad;        // transmit FIFO load in percent (0..100)

    //*****************************************************************************
    /// <summary>
    /// Ctor - create a CanChannelStatus object
    /// </summary>
    /// <param name="activated">activated flag</param>
    /// <param name="rxOverrun">overrun flag</param>
    /// <param name="rxFifoLoad">rx fifo load</param>
    /// <param name="txFifoLoad">rx fifo load</param>
    //*****************************************************************************
    public CanChannelStatus(bool activated, bool rxOverrun, byte rxFifoLoad, byte txFifoLoad)
    {
      fActivated = activated;
      fRxOverrun = rxOverrun;
      bRxFifoLoad = rxFifoLoad;
      bTxFifoLoad = txFifoLoad;
    }

    //--------------------------------------------------------------------
    // properties
    //--------------------------------------------------------------------

    //*****************************************************************************
    /// <summary>
    ///   Gets a value indicating whether the channel is activated.
    /// </summary>
    /// <returns>
    ///   true if the channel is activated, otherwise false.
    /// </returns>
    //*****************************************************************************
    public bool IsActivated
    {
      get
      {
        return( fActivated );
      }
    }

    //*****************************************************************************
    /// <summary>
    ///   Gets a value indicating if a message was lost because there was
    ///   not enough free space for the message in the receive FIFO.
    /// </summary>
    /// <returns>
    ///   true if a data overrun has occured, otherwise false.
    /// </returns>
    //*****************************************************************************
    public bool HasFifoOverrun
    {
      get
      {
        return( fRxOverrun );
      }
    }

    //*****************************************************************************
    /// <summary>
    ///   Gets the current load level of the receive FIFO in percent.
    /// </summary>
    /// <returns>
    ///   Current load level of the receive FIFO in percent (0...100%).
    /// </returns>
    //*****************************************************************************
    public byte ReceiveFifoLoad
    {
      get
      {
        return (bRxFifoLoad);
      }
    }

    //*****************************************************************************
    /// <summary>
    ///   Gets the current load level of the transmit FIFO in percent.
    /// </summary>
    /// <returns>
    ///   Current load level of the transmit FIFO in percent (0...100%).
    /// </returns>
    //*****************************************************************************
    public byte TransmitFifoLoad
    {
      get
      {
        return (bTxFifoLoad);
      }
    }

  };


}