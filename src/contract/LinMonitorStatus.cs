// SPDX-License-Identifier: MIT
//----------------------------------------------------------------------------
// Summary  : Declarations for the LIN monitor status class.
// Copyright: Copyright (C) 2016-2022 HMS Technology Center Ravensburg GmbH, 
//            all rights reserved
//----------------------------------------------------------------------------

namespace Ixxat.Vci4.Bal.Lin
{
  using System;
  using System.Threading;
  using System.Runtime.InteropServices;


  //*****************************************************************************
  /// <summary>
  ///   <c>LinMonitorStatus</c> represents the status of a LIN monitor.
  /// </summary>
  //*****************************************************************************
  public struct LinMonitorStatus
  {
    private bool fActivated;              // true if the channel is activated
    private bool fRxOverrun;              // true if receive FIFO overrun occurs
    private byte bRxFifoLoad;     // receive FIFO load in percent (0..100)

    //*****************************************************************************
    /// <summary>
    ///   Ctor - create a LinLineStatus object
    /// </summary>
    /// <param name="activated">activated flag</param>
    /// <param name="rxoverrun">rx overrun flag</param>
    /// <param name="fifoload">fifo load</param>
    //*****************************************************************************
    public LinMonitorStatus(bool activated, bool rxoverrun, byte fifoload)
    {
      fActivated = activated;
      fRxOverrun = rxoverrun;
      bRxFifoLoad = fifoload;
    }

    //*****************************************************************************
    /// <summary>
    ///   This method returns a String that represents the current timing value.
    /// </summary>
    /// <returns>
    ///   A String that represents the current bit timing value.
    /// </returns>
    //*****************************************************************************
    public override string ToString()
    {
      return String.Format("active: {0}, overrun: {1}, fifoload: {2:X2}", fActivated, fRxOverrun, bRxFifoLoad);
    }

    //*****************************************************************************
    /// <summary>
    ///   Gets a value indicating whether the monitor is activated.
    /// </summary>
    /// <returns>
    ///   true if the monitor is activated, otherwise false.
    /// </returns>
    //*****************************************************************************
    public bool IsActivated
    {
      get
      {
        return (fActivated);
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
        return (fRxOverrun);
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

  };


}