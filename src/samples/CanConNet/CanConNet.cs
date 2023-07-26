// SPDX-License-Identifier: MIT
//----------------------------------------------------------------------------
// Summary  : Demo application for the IXXAT VCI .NET-API.
//            This demo demonstrates the following VCI features
//              - adapter selection
//              - controller initialization
//              - creation of a message channel
//              - transmission/reception of CAN messages
// Copyright: Copyright (C) 2016-2022 HMS Technology Center Ravensburg GmbH, 
//            all rights reserved
//----------------------------------------------------------------------------

using System;
using System.Text;
using System.Collections;
using System.Threading;
using Ixxat.Vci4;
using Ixxat.Vci4.Bal;
using Ixxat.Vci4.Bal.Can;

namespace CanConNet
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
    ///   Reference to the CAN controller.
    /// </summary>
    static ICanControl? mCanCtl;

    /// <summary>
    ///   Reference to the CAN message communication channel.
    /// </summary>
    static ICanChannel? mCanChn;

    /// <summary>
    ///   Reference to the CAN message scheduler.
    /// </summary>
    static ICanScheduler? mCanSched;

    /// <summary>
    ///   Reference to the message writer of the CAN message channel.
    /// </summary>
    static ICanMessageWriter? mWriter;

    /// <summary>
    ///   Reference to the message reader of the CAN message channel.
    /// </summary>
    static ICanMessageReader? mReader;

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
      Console.WriteLine(" initializes the CAN with 125 kBaud");
      Console.WriteLine(" starts a cyclic message object with id 200H");
      Console.WriteLine(" key 'c' starts/stops a cyclic message object with id 200H");
      Console.WriteLine(" key 't' sends a message with id 100H");
      Console.WriteLine(" shows all received messages");
      Console.WriteLine(" Quit the application with ESC\n");

      Console.Write(" Select Adapter...\n");
      if (SelectDevice())
      {
        Console.WriteLine(" Select Adapter.......... OK !\n");

        Console.Write(" Initialize CAN...\n");

        if (!InitSocket(0))
        {
          Console.WriteLine(" Initialize CAN............ FAILED !\n");
        }
        else
        {
          Console.WriteLine(" Initialize CAN............ OK !\n");

          //
          // start the receive thread
          //
          rxThread = new Thread(new ThreadStart(ReceiveThreadFunc));
          rxThread.Start();

          //
          // add a cyclic message when schduler is available
          //
          ICanCyclicTXMsg? cyclicMsg = null;
          if (null != mCanSched)
          {
            //
            // start a cyclic object
            //
            cyclicMsg = mCanSched.AddMessage();

            cyclicMsg.Identifier = 200;
            cyclicMsg.CycleTicks = 100;
            cyclicMsg.DataLength = 8;
            cyclicMsg.SelfReceptionRequest = true;

            for (Byte i = 0; i < cyclicMsg.DataLength; i++)
            {
                cyclicMsg[i] = i;
            }
          }

          //
          // wait for keyboard hit transmit  CAN-Messages cyclically
          //
          ConsoleKeyInfo cki = new ConsoleKeyInfo();

          Console.WriteLine(" Press T to transmit single message.");
          if (null != mCanSched)
          {
            Console.WriteLine(" Press C to start/stop cyclic message.");
          }
          else
          {
            Console.WriteLine(" Cyclic messages not supported.");
          }
          Console.WriteLine(" Press ESC to exit.");
          do
          {
            while (!Console.KeyAvailable)
            {
              Thread.Sleep(10);
            }
            cki = Console.ReadKey(true);
            if (cki.Key == ConsoleKey.T)
            {
              TransmitData();
            }
            else if (cki.Key == ConsoleKey.C)
            {
              if (null != cyclicMsg)
              {
                if (cyclicMsg.Status != CanCyclicTXStatus.Busy)
                {
                  cyclicMsg.Start(0);
                }
                else
                {
                  cyclicMsg.Stop();
                }
              }
            }
          } while (cki.Key != ConsoleKey.Escape);

          if (null != cyclicMsg)
          {
            //
            // stop cyclic message
            //
            cyclicMsg.Stop();
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
    ///   Selects the first CAN adapter.
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

    #region Opening socket

    //************************************************************************
    /// <summary>
    ///   Opens the specified socket, creates a message channel, initializes
    ///   and starts the CAN controller.
    /// </summary>
    /// <param name="canNo">
    ///   Number of the CAN controller to open.
    /// </param>
    /// <returns>
    ///   A value indicating if the socket initialization succeeded or failed.
    /// </returns>
    //************************************************************************
    static bool InitSocket(Byte canNo)
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
        // Open a message channel for the CAN controller
        //
        mCanChn = bal.OpenSocket(canNo, typeof(ICanChannel)) as ICanChannel;

        if (null != mCanChn)
        {
          //
          // check if device supports the cyclic message scheduler
          //
          if (mCanChn.Features.HasFlag(CanFeatures.Scheduler))
          {
            //
            // Open the scheduler of the CAN controller
            //
            mCanSched = bal.OpenSocket(canNo, typeof(ICanScheduler)) as ICanScheduler;
          }

          // Initialize the message channel
          mCanChn.Initialize(1024, 128, false);

          // Get a message reader object
          mReader = mCanChn.GetMessageReader();

          // Initialize message reader
          mReader.Threshold = 1;

          // Create and assign the event that's set if at least one message
          // was received.
          mRxEvent = new AutoResetEvent(false);
          mReader.AssignEvent(mRxEvent);

          // Get a message wrtier object
          mWriter = mCanChn.GetMessageWriter();

          // Initialize message writer
          mWriter.Threshold = 1;

          // Activate the message channel
          mCanChn.Activate();

          //
          // Open the CAN controller
          //
          mCanCtl = bal.OpenSocket(canNo, typeof(ICanControl)) as ICanControl;

          if (null != mCanCtl)
          {
            // Initialize the CAN controller
            mCanCtl.InitLine(CanOperatingModes.Standard |
              CanOperatingModes.Extended |
              CanOperatingModes.ErrFrame,
              CanBitrate.Cia125KBit);

            //
            // print line status
            //
            Console.WriteLine(" LineStatus: {0}", mCanCtl.LineStatus);

            // Set the acceptance filter for std identifiers
            mCanCtl.SetAccFilter(CanFilter.Std,
                                 (uint)CanAccCode.All, (uint)CanAccMask.All);

            // Set the acceptance filter for ext identifiers
            mCanCtl.SetAccFilter(CanFilter.Ext,
                                 (uint)CanAccCode.All, (uint)CanAccMask.All);

            // Start the CAN controller
            mCanCtl.StartLine();

            succeeded = true;
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

    #region Message transmission

    /// <summary>
    ///   Transmits a CAN message with ID 0x100.
    /// </summary>
    static void TransmitData()
    {
      if (null == mWriter)
        return;

      IMessageFactory factory = VciServer.Instance()!.MsgFactory;
      ICanMessage canMsg = (ICanMessage)factory.CreateMsg(typeof(ICanMessage));

      canMsg.TimeStamp  = 0;
      canMsg.Identifier = 0x100;
      canMsg.FrameType  = CanMsgFrameType.Data;
      canMsg.DataLength = 8;
      canMsg.SelfReceptionRequest = true;  // show this message in the console window

      for (Byte i = 0; i < canMsg.DataLength; i++)
      {
        canMsg[i] = i;
      }

      // Write the CAN message into the transmit FIFO
      mWriter.SendMessage(canMsg);
    }

    #endregion

    #region Message reception

    //************************************************************************
    /// <summary>
    /// Print a CAN message
    /// </summary>
    /// <param name="canMessage"></param>
    //************************************************************************
    static void PrintMessage(ICanMessage canMessage)
    {
      switch (canMessage.FrameType)
      {
        //
        // show data frames
        //
        case CanMsgFrameType.Data:
          {
            if (!canMessage.RemoteTransmissionRequest)
            {
              Console.Write("\nTime: {0,10}  ID: {1,3:X}  DLC: {2,1}  Data:",
                            canMessage.TimeStamp,
                            canMessage.Identifier,
                            canMessage.DataLength);

              for (int index = 0; index < canMessage.DataLength; index++)
              {
                Console.Write(" {0,2:X}", canMessage[index]);
              }
            }
            else
            {
              Console.Write("\nTime: {0,10}  ID: {1,3:X}  DLC: {2,1}  Remote Frame",
                            canMessage.TimeStamp,
                            canMessage.Identifier,
                            canMessage.DataLength);
            }
            break;
          }

        //
        // show informational frames
        //
        case CanMsgFrameType.Info:
          {
            switch ((CanMsgInfoValue)canMessage[0])
            {
              case CanMsgInfoValue.Start:
                Console.Write("\nCAN started...");
                break;
              case CanMsgInfoValue.Stop:
                Console.Write("\nCAN stopped...");
                break;
              case CanMsgInfoValue.Reset:
                Console.Write("\nCAN reseted...");
                break;
            }
            break;
          }

        //
        // show error frames
        //
        case CanMsgFrameType.Error:
          {
            switch ((CanMsgError)canMessage[0])
            {
              case CanMsgError.Stuff:
                Console.Write("\nstuff error...");
                break;
              case CanMsgError.Form:
                Console.Write("\nform error...");
                break;
              case CanMsgError.Acknowledge:
                Console.Write("\nacknowledgment error...");
                break;
              case CanMsgError.Bit:
                Console.Write("\nbit error...");
                break;
              case CanMsgError.Fdb:
                Console.Write("\nfast data bit error...");
                break;
              case CanMsgError.Crc:
                Console.Write("\nCRC error...");
                break;
              case CanMsgError.Dlc:
                Console.Write("\nData length error...");
                break;
              case CanMsgError.Other:
                Console.Write("\nother error...");
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

      ICanMessage[] msgArray;

      do
      {
        // Wait 100 msec for a message reception
        if (mRxEvent.WaitOne(100, false))
        {
          if (mReader.ReadMessages(out msgArray) > 0)
          {
            foreach (ICanMessage entry in msgArray)
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

      ICanMessage canMessage;

      do
      {
        // Wait 100 msec for a message reception
        if (mRxEvent.WaitOne(100, false))
        {
          // read a CAN message from the receive FIFO
          while (mReader.ReadMessage(out canMessage))
          {
            PrintMessage(canMessage);
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

      // Dispose message writer
      DisposeVciObject(mWriter);

      // Dispose CAN channel
      DisposeVciObject(mCanChn);

      // Dispose CAN controller
      DisposeVciObject(mCanCtl);

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
