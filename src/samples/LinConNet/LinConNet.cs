// SPDX-License-Identifier: MIT
//----------------------------------------------------------------------------
// Summary  : Demo application for the IXXAT VCI .NET-API.
//            This demo demonstrates the following VCI features
//              - adapter selection
//              - controller initialization
//              - creation of a message channel
//              - transmission/reception of LIN messages
// Copyright: Copyright (C) 2016-2022 HMS Technology Center Ravensburg GmbH, 
//            all rights reserved
//----------------------------------------------------------------------------

using System;
using System.Text;
using System.Collections;
using System.Threading;
using Ixxat.Vci4;
using Ixxat.Vci4.Bal;
using Ixxat.Vci4.Bal.Lin;

namespace Ixxat.Vci4.Samples.LinConNet
{
  //##########################################################################
  /// <summary>
  ///   This class provides the entry point for the IXXAT VCI .NET API
  ///   demo application.
  /// </summary>
  //##########################################################################
  class CanConNet
  {
    #region Member variables

    /// <summary>
    ///   Reference to the used VCI device.
    /// </summary>
    static IVciDevice? mDevice;

    /// <summary>
    ///   Reference to the LIN controller.
    /// </summary>
    static ILinControl? mLinCtl;

    /// <summary>
    ///   Reference to the LIN communication monitor.
    /// </summary>
    static ILinMonitor? mLinMon;

    /// <summary>
    ///   Reference to the message reader of the LIN monitor.
    /// </summary>
    static ILinMessageReader? mReader;

    /// <summary>
    ///   Thread that handles the message reception.
    /// </summary>
    static Thread? rxThread;

    /// <summary>
    ///   Quit flag for the receive thread.
    /// </summary>
    static long mMustQuit = 0;

    /// <summary>
    ///   Event that's set if at least one message was received.
    /// </summary>
    static AutoResetEvent? mRxEvent;

    #endregion

    #region Application entry point

    //************************************************************************
    /// <summary>
    ///   The entry point of this console application.
    /// </summary>
    //************************************************************************
    static void Main(string[] args)
    {
      Console.WriteLine(" >>>> VCI - .NET 2.0 - API Example V1.1 <<<<");
      Console.WriteLine(" initializes the LIN with 19200 bit/s as slave node");
      Console.WriteLine(" shows all received messages");
      Console.WriteLine(" Quit the application with ESC\n");

      Console.Write(" Select Adapter...\n");
      if (SelectDevice())
      {
        Console.WriteLine(" Select Adapter.......... OK !\n");

        ClearResponseTable();

        Console.Write(" Initialize LIN...\n");
        if (!InitSocket())
        {
          Console.WriteLine(" Initialize LIN............ FAILED !\n");
        }
        else
        {
          Console.WriteLine(" Initialize LIN............ OK !\n");

          //
          // start the receive thread
          //
          rxThread = new Thread(new ThreadStart(ReceiveThreadFunc));
          rxThread.Start();

          //
          // wait for keyboard hit
          //
          while (!Console.KeyAvailable)
          {
            Thread.Sleep(100);
          }

          //
          // tell receive thread to quit
          //
          Interlocked.Exchange(ref mMustQuit, 1);

          //
          // Wait for termination of receive thread
          //
          rxThread.Join();
        }

        Console.Write("\n Free VCI - Resources...\n");
        FinalizeApp();
        Console.WriteLine(" Free VCI - Resources........ OK !\n");
      }

      Console.Write(" Done");
      Console.ReadLine();
    }

    #endregion

    #region Device selection

    //************************************************************************
    /// <summary>
    ///   Selects the first LIN adapter.
    /// </summary>
    /// <return> true if succeeded, false otherwise</return>
    //************************************************************************
    static bool SelectDevice()
    {
      bool               succeeded     = false;
      IVciDeviceManager? deviceManager = null;
      IVciDeviceList?    deviceList    = null;
      IEnumerator?       deviceEnum    = null;

      try
      {
        //
        // Get device manager from VCI server
        //
        deviceManager = VciServer.Instance()!.DeviceManager;

        //
        // Get the list of installed VCI devices
        //
        deviceList = deviceManager.GetDeviceList();

        //
        // Get enumerator for the list of devices
        //
        deviceEnum = deviceList.GetEnumerator();

        //
        // Get first device
        //
        deviceEnum.MoveNext();
        mDevice = deviceEnum.Current as IVciDevice;

        if (null != mDevice)
        {
          //
          // print bus type and controller type of first controller
          //
          IVciCtrlInfo? info = mDevice.Equipment[0];
          Console.Write(" BusType    : {0}\n", info.BusType);
          Console.Write(" CtrlType   : {0}\n", info.ControllerType);

          // show the device name and serial number
          string serialNumberText = mDevice.UniqueHardwareId.ToString() ?? "<device id not available>";
          Console.Write(" Interface    : " + mDevice.Description + "\n");
          Console.Write(" Serial number: " + serialNumberText + "\n");

          succeeded = true;
        }
      }
      catch (Exception exc)
      {
        Console.WriteLine("Error: " + exc.Message);
      }
      finally
      {
        //
        // Dispose device manager ; it's no longer needed.
        //
        DisposeVciObject(deviceManager);

        //
        // Dispose device list ; it's no longer needed.
        //
        DisposeVciObject(deviceList);

        //
        // Dispose device list ; it's no longer needed.
        //
        DisposeVciObject(deviceEnum);
      }

      return succeeded;
    }

    #endregion

    #region Response table initialization

    /// <summary>
    /// Reset after start the response table.
    //  configure the LIN buffer to the standard 2.2
    /// Else the reception of LIN messages can be undefined.
    /// </summary>
    static void ClearResponseTable()
    {
      if (null != mLinCtl)
      {
        IMessageFactory? factory = VciServer.Instance()!.MsgFactory;
        ILinMessage? linMessage = (ILinMessage)factory.CreateMsg(typeof(ILinMessage));

        linMessage.ExtendedCrc = true;
        linMessage.SenderOfResponse = false;
        linMessage.MessageType = LinMessageType.Data;

        for (int i = 0; i < 59; i++)
        {
          linMessage.ProtId = (byte)i;
          if (i < 0x20)
          {
            linMessage.DataLength = 2;
          }
          else if (i < 0x30)
          {
            linMessage.DataLength = 4;
          }
          else
          {
            linMessage.DataLength = 8;
          }
          mLinCtl.WriteMessage(false, linMessage);
        }

        //  Frame identifiers 60 (0x3C) to 61 (0x3D) shall always use classic checksum
        //  (LIN-Spec 2.2 chapter 2.3.1.5)
        linMessage.DataLength = 8;
        linMessage.ProtId = 0x60;
        mLinCtl.WriteMessage(false, linMessage);
        linMessage.ProtId = 0x61;
        mLinCtl.WriteMessage(false, linMessage);
      }
    }

    #endregion

    #region Opening socket

    //************************************************************************
    /// <summary>
    ///   Opens the specified socket, creates a message monitor, initializes
    ///   and starts the LIN controller.
    /// </summary>
    /// <return> true if succeeded, false otherwise</return>
    //************************************************************************
    static bool InitSocket()
    {
      IBalObject? bal = null;
      bool succeeded = false;

      if (null == mDevice)
        return false;

      try
      {
        //
        // Open bus access layer
        //
        bal = mDevice.OpenBusAccessLayer();

        //
        // Look for a LIN socket resource
        //
        Byte portNo = 0xFF;
        foreach(IBalResource resource in bal.Resources)
        {
          if (resource.BusType == VciBusType.Lin)
          {
            // first LIN controller found -> exit loop
            portNo = resource.BusPort;
            resource.Dispose();
            break;
          }
          resource.Dispose();
        }

        // check if LIN controller found
        if (0xFF == portNo)
        {
          Console.WriteLine(" Error: No LIN controller found.");
          succeeded = false;
        }
        else
        {
          //
          // Open a message monitor for the LIN controller
          //
          mLinMon = bal.OpenSocket(portNo, typeof(ILinMonitor)) as ILinMonitor;
          if (null != mLinMon)
          {
            // Initialize the message monitor
            mLinMon.Initialize(1024, false);

            // Get a message reader object
            mReader = mLinMon.GetMessageReader();

            // Initialize message reader
            mReader.Threshold = 1;

            // Create and assign the event that's set if at least one message
            // was received.
            mRxEvent = new AutoResetEvent(false);
            mReader.AssignEvent(mRxEvent);

            // Activate the message monitor
            mLinMon.Activate();

            //
            // Open the LIN controller
            //
            mLinCtl = bal.OpenSocket(portNo, typeof(ILinControl)) as ILinControl;
            if (null != mLinCtl)
            {
              // Initialize the LIN controller
              LinInitLine initData = new LinInitLine();
              initData.Bitrate = LinBitrate.Lin19200Bit;
              initData.OperatingMode = LinOperatingModes.Slave;
              mLinCtl.InitLine(initData);

              //
              // print line status
              //
              Console.WriteLine(" LineStatus: {0}", mLinCtl.LineStatus);

              // Start the LIN controller
              mLinCtl.StartLine();
              succeeded = true;
            }
          }
        }
      }
      catch (Exception exc)
      {
        Console.WriteLine("Error: Initializing socket failed : " + exc.Message);
        succeeded = false;
      }
      finally
      {
        //
        // Dispose bus access layer
        //
        DisposeVciObject(bal);
      }

      return succeeded;
    }

    #endregion

    #region Message reception

    //************************************************************************
    /// <summary>
    /// Print a Lin message
    /// </summary>
    /// <param name="linMessage"></param>
    //************************************************************************
    static void PrintMessage(ILinMessage linMessage)
    {
      switch (linMessage.MessageType)
      {
        //
        // show data frames
        //
        case LinMessageType.Data:
          {
            Console.Write("\nTime: {0,10}  ID: {1,3}  DLC: {2,1}  Data:",
                          linMessage.TimeStamp,
                          linMessage.ProtId,
                          linMessage.DataLength);

            for (int index = 0; index < linMessage.DataLength; index++)
            {
              Console.Write(" {0,2:X}", linMessage[index]);
            }
            break;
          }

        //
        // show informational frames
        //
        case LinMessageType.Info:
          {
            switch ((LinMsgInfoValue)linMessage[0])
            {
              case LinMsgInfoValue.Start:
                Console.Write("\nLIN started...");
                break;
              case LinMsgInfoValue.Stop:
                Console.Write("\nLIN stopped...");
                break;
              case LinMsgInfoValue.Reset:
                Console.Write("\nLIN reseted...");
                break;
            }
            break;
          }

        //
        // show error frames
        //
        case LinMessageType.Error:
          {
            switch ((LinMsgError)linMessage[0])
            {
              case LinMsgError.Bit:
                Console.Write("\nbit error...");
                break;
              case LinMsgError.Crc:
                Console.Write("\nCRC error...");
                break;
              case LinMsgError.Other:
                Console.Write("\nother error...");
                break;
              case LinMsgError.NoBus:
                Console.Write("\nno bus activity...");
                break;
              case LinMsgError.Parity:
                Console.Write("\nparity error of the identifier...");
                break;
              case LinMsgError.SlaveNoResponse:
                Console.Write("\nslave does not respond...");
                break;
              case LinMsgError.Sync:
                Console.Write("\ninvalid synchronization field...");
                break;
            }
            break;
          }
      }
    }

    //************************************************************************
    /// <summary>
    /// Demonstrate reading messages via MsgReader::ReadMessages() function
    /// </summary>
    //************************************************************************
    static void ReadMultipleMsgsViaReadMessages()
    {
      if ((null == mReader) ||
          (null == mRxEvent))
            return;

      ILinMessage[] msgArray;

      do
      {
        // Wait 100 msec for a message reception
        if (mRxEvent.WaitOne(100, false))
        {
          if (mReader.ReadMessages(out msgArray) > 0)
          {
            foreach (ILinMessage entry in msgArray)
            {
              PrintMessage(entry);
            }
          }
        }
      } while (0 == mMustQuit);
    }

    //************************************************************************
    /// <summary>
    /// Demonstrate reading messages via MsgReader::ReadMessage() function
    /// </summary>
    //************************************************************************
    static void ReadMsgsViaReadMessage()
    {
      if ((null == mReader) ||
          (null == mRxEvent))
            return;

      ILinMessage linMessage;

      do
      {
        // Wait 100 msec for a message reception
        if (mRxEvent.WaitOne(100, false))
        {
          // read a CAN message from the receive FIFO
          while (mReader.ReadMessage(out linMessage))
          {
            PrintMessage(linMessage);
          }
        }
      } while (0 == mMustQuit);
    }

    //************************************************************************
    /// <summary>
    ///   This method is the works as receive thread.
    /// </summary>
    //************************************************************************
    static void ReceiveThreadFunc()
    {
      ReadMsgsViaReadMessage();
      //
      // alternative: use ReadMultipleMsgsViaReadMessages();
      //
    }

    #endregion

    #region Utility methods

    //************************************************************************
    /// <summary>
    ///   Finalizes the application
    /// </summary>
    //************************************************************************
    static void FinalizeApp()
    {
      //
      // Dispose all hold VCI objects.
      //

      // Dispose message reader
      DisposeVciObject(mReader);

      // Dispose LIN monitor
      DisposeVciObject(mLinMon);

      // Dispose LIN controller
      DisposeVciObject(mLinCtl);

      // Dispose VCI device
      DisposeVciObject(mDevice);
    }


    //************************************************************************
    /// <summary>
    ///   This method tries to dispose the specified object.
    /// </summary>
    /// <param name="obj">
    ///   Reference to the object to be disposed.
    /// </param>
    /// <remarks>
    ///   The VCI interfaces provide access to native driver resources.
    ///   Because the .NET garbage collector is only designed to manage memory,
    ///   but not native OS and driver resources the application itself is
    ///   responsible to release these resources via calling
    ///   IDisposable.Dispose() for the obects obtained from the VCI API
    ///   when these are no longer needed.
    ///   Otherwise native memory and resource leaks may occure.
    /// </remarks>
    //************************************************************************
    static void DisposeVciObject(object? obj)
    {
      if (null != obj)
      {
        IDisposable? dispose = obj as IDisposable;
        if (null != dispose)
        {
          dispose.Dispose();
          obj = null;
        }
      }
    }

    #endregion
  }
}
