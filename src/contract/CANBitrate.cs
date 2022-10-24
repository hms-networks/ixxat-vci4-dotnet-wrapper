/*****************************************************************************
 IXXAT Automation GmbH
******************************************************************************
 Summary : Declarations for the CAN bitrate value class.
 Compiler: Microsoft VC++ 8
******************************************************************************
 all rights reserved
*****************************************************************************/

/*************************************************************************
** namespace Ixxat.Vci4.Bal.Can
*************************************************************************/
namespace Ixxat.Vci4.Bal.Can 
{
  using System;

  /// <summary>
  /// Bitrate mode flags
  /// </summary>
  [Flags]
  public enum CanBitrateMode : int
  {
    /// <summary>
    ///   No CanBitrateMode is set. This is the default behaviour.
    /// </summary>
    None = 0x0,
    /// <summary>
    ///   Raw mode, all values will be written directly into the 
    ///   can controller's registers
    /// </summary>
    Raw = 0x00000001,
    /// <summary>
    ///   Triple sampling mode
    /// </summary>
    TripleSampling = 0x00000002,
  };

  //*****************************************************************************
  /// <summary>
  ///   Instances of <c>CanBitrate</c> represent a bit rate for CAN busses.
  ///   A CAN bit rate is defined of two bit timing register values: Btr0 and 
  ///   Btr1.
  ///   The standardized CiA bit timing register values are provided as static 
  ///   <c>CanBitrate</c> members like <c>Cia250KBit</c>.
  /// </summary>
  //*****************************************************************************
  public struct CanBitrate
  {
    const byte CAN_BT0_10KB = 0x31;
    const byte CAN_BT1_10KB = 0x1C;
    const byte CAN_BT0_20KB = 0x18;
    const byte CAN_BT1_20KB = 0x1C;
    const byte CAN_BT0_50KB = 0x09;
    const byte CAN_BT1_50KB = 0x1C;
    const byte CAN_BT0_100KB = 0x04;
    const byte CAN_BT1_100KB = 0x1C;
    const byte CAN_BT0_125KB = 0x03;
    const byte CAN_BT1_125KB = 0x1C;
    const byte CAN_BT0_250KB = 0x01;
    const byte CAN_BT1_250KB = 0x1C;
    const byte CAN_BT0_500KB = 0x00;
    const byte CAN_BT1_500KB = 0x1C;
    const byte CAN_BT0_800KB = 0x00;
    const byte CAN_BT1_800KB = 0x16;
    const byte CAN_BT0_1000KB = 0x00;
    const byte CAN_BT1_1000KB = 0x14;

    //--------------------------------------------------------------------
    // member variables
    //--------------------------------------------------------------------
    private byte    m_bBtr0;  // value for bit timing register 0
    private byte    m_bBtr1;  // value for bit timing register 1
    private string? m_sName;  // human readable name of the bit rate


    //--------------------------------------------------------------------
    // properties
    //--------------------------------------------------------------------

      //*****************************************************************************
      /// <summary>
      ///   Gets the value of the bus timing register 0. The value corresponds to 
      ///   the BTR0 register of the Phillips SJA 1000 CAN controller with a cycle 
      ///   frequency of 16 MHz. Further information on this is given in the 
      ///   data sheet of the SJA 1000.
      /// </summary>
      /// <returns>
      ///   Value for the bit timing register 0.
      /// </returns>
      //*****************************************************************************
      public byte Btr0
      {
        get
        {
          return( m_bBtr0 );
        }
      }

      //*****************************************************************************
      /// <summary>
      ///   Gets the value of the bus timing register 1. The value corresponds to 
      ///   the BTR1 register of the Phillips SJA 1000 CAN controller with a cycle 
      ///   frequency of 16 MHz. Further information on this is given in the 
      ///   data sheet of the SJA 1000.
      /// </summary>
      /// <returns>
      ///   Value for the bit timing register 1.
      /// </returns>
      //*****************************************************************************
      public byte Btr1
      {
        get
        {
          return( m_bBtr1 );
        }
      }

      //*****************************************************************************
      /// <summary>
      ///   Gets the 16 bit value of this CanBitrate structure.
      /// </summary>
      /// <returns>
      ///   The 16 bit value of this CanBitrate structure.
      /// </returns>
      //*****************************************************************************
      public short AsInt16
      {
        get
        {
          return (short)( (m_bBtr1 << 8) | m_bBtr0 );
        }
      }

      //*****************************************************************************
      /// <summary>
      ///   Gets the bit time of this CanBitrate.
      ///   (Bit time of this CanBitrate in clock ticks according to SJA1000.)
      /// </summary>
      /// <returns>
      ///   Bit time of this CanBitrate in clock ticks according to SJA1000.
      /// </returns>
      //*****************************************************************************
      public int Bittime
      {
        get
        {
          int brp = (m_bBtr0 & 0x3F);
          int ts1 = (m_bBtr1 & 0x0F);
          int ts2 = ((m_bBtr1 & 0x70) >> 4);
          return(2*(brp+1)*(ts1+ts2+3));
        }
      }

      //*****************************************************************************
      /// <summary>
      ///   Gets the name of this bitrate.
      /// </summary>
      /// <returns>
      ///   The name of this bitrate.
      /// </returns>
      /// <remarks>
      ///   This property returns either the user defined name of the bit rate,
      ///   if the bit rate was created with a name, or the name of the known
      ///   bit rate. For custom bit rates without a user defined name, the
      ///   property returns the bit timing value as numeric string.
      /// </remarks>
      //*****************************************************************************
      public string  Name
      {
        get
        {
          return( ToString() );
        }
      }


    //--------------------------------------------------------------------
    // static properties
    //--------------------------------------------------------------------

      //*****************************************************************************
      /// <summary>
      ///   Gets an empty bit timing value.
      /// </summary>
      /// <returns>
      ///   Empty bit timing value.
      /// </returns>
      //*****************************************************************************
      public static CanBitrate Empty
      {
        get
        {
          return new CanBitrate(0, 0, "<Empty>");
        }
      }

      //*****************************************************************************
      /// <summary>
      ///   Gets the bit timing value for the predefined CiA bit rate of 10 kbit/s.
      /// </summary>
      /// <returns>
      ///   Bit timing value for the predefined CiA 10 kbit/s.
      /// </returns>
      //*****************************************************************************
      public static CanBitrate Cia10KBit
      {
        get
        {
          return new CanBitrate(CAN_BT0_10KB, CAN_BT1_10KB, "CiA 10 kbit/s");
        }
      }

      //*****************************************************************************
      /// <summary>
      ///   Gets the bit timing value for the predefined CiA bit rate of 20 kbit/s.
      /// </summary>
      /// <returns>
      ///   Bit timing value for the predefined CiA 20 kbit/s.
      /// </returns>
      //*****************************************************************************
      public static CanBitrate Cia20KBit
      {
        get
        {
          return new CanBitrate(CAN_BT0_20KB, CAN_BT1_20KB, "CiA 20 kbit/s");
        }
      }

      //*****************************************************************************
      /// <summary>
      ///   Gets the bit timing value for the predefined CiA bit rate of 50 kbit/s.
      /// </summary>
      /// <returns>
      ///   Bit timing value for the predefined CiA 50 kbit/s.
      /// </returns>
      //*****************************************************************************
      public static CanBitrate Cia50KBit
      {
        get
        {
          return new CanBitrate(CAN_BT0_50KB, CAN_BT1_50KB, "CiA 50 kbit/s");
        }
      }

      //*****************************************************************************
      /// <summary>
      ///   Gets the bit timing value for the predefined CiA bit rate of 125 kbit/s.
      /// </summary>
      /// <returns>
      ///   Bit timing value for the predefined CiA 125 kbit/s.
      /// </returns>
      //*****************************************************************************
      public static CanBitrate Cia125KBit
      {
        get
        {
          return new CanBitrate(CAN_BT0_125KB, CAN_BT1_125KB, "CiA 125 kbit/s");
        }
      }

      //*****************************************************************************
      /// <summary>
      ///   Gets the bit timing value for the predefined CiA bit rate of 250 kbit/s.
      /// </summary>
      /// <returns>
      ///   Bit timing value for the predefined CiA 250 kbit/s.
      /// </returns>
      //*****************************************************************************
      public static CanBitrate Cia250KBit
      {
        get
        {
          return new CanBitrate(CAN_BT0_250KB, CAN_BT1_250KB, "CiA 250 kbit/s");
        }
      }

      //*****************************************************************************
      /// <summary>
      ///   Gets the bit timing value for the predefined CiA bit rate of 500 kbit/s.
      /// </summary>
      /// <returns>
      ///   Bit timing value for the predefined CiA 500 kbit/s.
      /// </returns>
      //*****************************************************************************
      public static CanBitrate Cia500KBit
      {
        get
        {
          return new CanBitrate(CAN_BT0_500KB, CAN_BT1_500KB, "CiA 500 kbit/s");
        }
      }

      //*****************************************************************************
      /// <summary>
      ///   Gets the bit timing value for the predefined CiA bit rate of 800 kbit/s.
      /// </summary>
      /// <returns>
      ///   Bit timing value for the predefined CiA 800 kbit/s.
      /// </returns>
      //*****************************************************************************
      public static CanBitrate Cia800KBit
      {
        get
        {
          return new CanBitrate(CAN_BT0_800KB, CAN_BT1_800KB, "CiA 800 kbit/s");
        }
      }

      //*****************************************************************************
      /// <summary>
      ///   Gets the bit timing value for the predefined CiA bit rate of 1000 kbit/s.
      /// </summary>
      /// <returns>
      ///   Bit timing value for the predefined CiA 1000 kbit/s.
      /// </returns>
      //*****************************************************************************
      public static CanBitrate Cia1000KBit
      {
        get
        {
          return new CanBitrate(CAN_BT0_1000KB, CAN_BT1_1000KB, "CiA 1000 kbit/s");
        }
      }

      //*****************************************************************************
      /// <summary>
      ///   Gets the bit timing value for the predefined bit rate of 100 kbit/s.
      /// </summary>
      /// <returns>
      ///   Bit timing value for the predefined 100 kbit/s.
      /// </returns>
      //*****************************************************************************
      public static CanBitrate _100KBit
      {
        get
        {
          return new CanBitrate(CAN_BT0_100KB, CAN_BT1_100KB, "100 kbit/s");
        }
      }

      //*****************************************************************************
      /// <summary>
      ///   Gets an array of all available CiA baud rates.
      /// </summary>
      /// <returns>
      ///   Array of all available CiA baud rates.
      /// </returns>
      //*****************************************************************************
      public static CanBitrate[] CiaBitRates
      {
        get
        {
          CanBitrate[] ciaRates = new CanBitrate[] { Cia10KBit,  
                                                     Cia20KBit,  
                                                     Cia50KBit,
                                                     Cia125KBit, 
                                                     Cia250KBit,
                                                     Cia500KBit, 
                                                     Cia800KBit, 
                                                     Cia1000KBit
                                                   };
          return ciaRates;
        }
      }


    //--------------------------------------------------------------------
    // member functions
    //--------------------------------------------------------------------

      //*****************************************************************************
      /// <summary>
      ///   Constructor for a new CanBitrate.
      /// </summary>
      /// <param name="bitTimingRegister0">
      ///   Value for bit timing register 0
      /// </param>
      /// <param name="bitTimingRegister1">
      ///   Value for bit timing register 1
      /// </param>
      //*****************************************************************************
      public CanBitrate(byte bitTimingRegister0, byte bitTimingRegister1)
      {
        m_bBtr0 = bitTimingRegister0;
        m_bBtr1 = bitTimingRegister1;
        m_sName = null;
      }

      //*****************************************************************************
      /// <summary>
      ///   Constructor for a new CanBitrate.
      /// </summary>
      /// <param name="bitTimingRegister0">
      ///   Value for bit timing register 0
      /// </param>
      /// <param name="bitTimingRegister1">
      ///   Value for bit timing register 1
      /// </param>
      /// <param name="name">
      ///   Human readable name of the bit rate.
      /// </param>
      //*****************************************************************************
      public CanBitrate(byte bitTimingRegister0, byte bitTimingRegister1, string  name)
      {
        m_bBtr0 = bitTimingRegister0;
        m_bBtr1 = bitTimingRegister1;
        m_sName = name;
      }

      //*****************************************************************************
      /// <summary>
      ///   This method returns a String that represents the current timing value.
      /// </summary>
      /// <returns>
      ///   A String that represents the current bit timing value.
      /// </returns>
      //*****************************************************************************
      public override string  ToString()
      {
        if (null == m_sName)
        {
          m_sName = String.Format("{0:X2}:{1:X2}", m_bBtr0, m_bBtr1);
        }

        return( m_sName );
      }

      //*****************************************************************************
      /// <summary>
      ///   Determines whether the specified Object is equal to the current Object.
      /// </summary>
      /// <pararm name ="obj">
      ///   The Object to compare with the current Object.
      /// </pararm>
      /// <returns>
      ///   true if the specified Object is equal to the current Object; 
      ///   otherwise, false.
      /// </returns>
      //*****************************************************************************
      public override bool Equals(Object? obj)
      {
        // Check for null values and compare run-time types.
        if (!(obj is CanBitrate))
          return false;

        return Equals((CanBitrate)obj);
      }

      //*****************************************************************************
      /// <summary>
      ///   Determines whether the specified Object is equal to the current Object.
      /// </summary>
      /// <pararm name ="other">
      ///   The Object to compare with the current Object.
      /// </pararm>
      /// <returns>
      ///   true if the specified Object is equal to the current Object; 
      ///   otherwise, false.
      /// </returns>
      //*****************************************************************************
      public bool Equals(CanBitrate other)
      {
        return (Btr0 == other.Btr0) && (Btr1 == other.Btr1);
      }

      //*****************************************************************************
      /// <summary>
      ///   Comparison operator ==
      /// </summary>
      /// <pararm name ="lhs">
      ///   left hand side object to compare
      /// </pararm>
      /// <pararm name ="rhs">
      ///   right hand side object to compare
      /// </pararm>
      /// <returns>
      ///   true if the specified Object is equal to the current Object; 
      ///   otherwise, false.
      /// </returns>
      //*****************************************************************************
      public static bool operator ==(CanBitrate lhs, CanBitrate rhs)
      {
        return lhs.Equals(rhs);
      }

      //*****************************************************************************
      /// <summary>
      ///   Comparison operator !=
      /// </summary>
      /// <pararm name ="lhs">
      ///   left hand side object to compare
      /// </pararm>
      /// <pararm name ="rhs">
      ///   right hand side object to compare
      /// </pararm>
      /// <returns>
      ///   true if the specified Object is not equal to the current Object; 
      ///   otherwise, false.
      /// </returns>
      //*****************************************************************************
      public static bool operator !=(CanBitrate lhs, CanBitrate rhs)
      {
        return !lhs.Equals(rhs);
      }

      //*****************************************************************************
      /// <summary>
      ///   Serves as a hash function for a particular type. GetHashCode is suitable 
      ///   for use in hashing algorithms and data structures like a hash table.
      /// </summary>
      /// <returns>
      ///   A hash code for the current Object. 
      /// </returns>
      //*****************************************************************************
      public override int GetHashCode()
      {
        return (m_bBtr0 << 8) + m_bBtr1;
      }
  };

  //*****************************************************************************
  /// <summary>
  ///   Instances of <c>CanBitrate2</c> represent a bit rate for CAN and CAN-FD 
  ///   busses. A CAN/CAN-FD bit rate is defined of 6 bit timing register values: 
  ///   Mode, Prescaler, TimeSegment1, TimeSegment2, SJW and TransmitterDelay.
  ///   The standardized CiA bit timing register values are provided as static 
  ///   <c>CanBitrate2</c> members like <c>Cia250KBit</c>.
  /// </summary>
  /// <example>
  /// <code>
  ///    normal mode
  ///   
  ///   |------- Tbit ---------|
  ///   +------+-------+-------+
  ///   | SYNC | TSEG1 | TSEG2 |
  ///   +------+---- --+-------+
  ///   |     wTS1     | wTS2  |
  ///   +--------------+-------+
  ///                  |
  ///                  +-> Sample Point
  ///   
  ///   raw mode
  ///   
  ///   |-------- Tbit --------|
  ///   +------+-------+-------+
  ///   | SYNC | TSEG1 | TSEG2 |
  ///   +------+-------+-------+
  ///   |  1   |  wTS1 |  wTS2 |
  ///   +------+-------+-------+
  ///                  |
  ///                  +-> Sample Point
  ///   
  ///   SYNC  := Re-Synchronisation Segment
  ///   TSEG1 := Time Segment 1
  ///   TSEG2 := Time Segment 2
  ///   
  /// </code>
  /// </example>
  //*****************************************************************************
  public struct CanBitrate2
  {
    //--------------------------------------------------------------------
    // member variables
    //--------------------------------------------------------------------
    private uint    m_dwMode;
    private uint    m_dwBPS;  // bits per second or prescaler (see CAN_BTMODE_RAW)
    private ushort  m_wTS1;   // length of time segment 1 in quantas
    private ushort  m_wTS2;   // length of time segment 2 in quantas
    private ushort  m_wSJW;   // re-synchronisation jump width in quantas
    private ushort  m_wTDO;   // transceiver delay compensation offset in quantas
                              // (0 = disabled)
    private string? m_sName;  // human readable name of the bit rate

    //--------------------------------------------------------------------
    // properties
    //--------------------------------------------------------------------

      //*****************************************************************************
      /// <summary>
      ///   Gets the value of the bittiming mode. Mode can be native values, which 
      ///   will be written directly to the can controller register or logical values
      ///   which contains the bitrate and the sample point. These values will
      ///   be recalculated by the driver to match the can controller's settings.
      /// </summary>
      /// <returns>
      ///   Value for the bittiming mode.
      /// </returns>
      //*****************************************************************************
      public CanBitrateMode Mode
      {
        get
        {
          return( (CanBitrateMode)m_dwMode );
        }
      }

      //*****************************************************************************
      /// <summary>
      ///   Gets the value of the prescaler.
      /// </summary>
      /// <returns>
      ///   Value for the prescaler.
      /// </returns>
      //*****************************************************************************
      public uint Prescaler
      {
        get
        {
          return( m_dwBPS );
        }
      }

      //*****************************************************************************
      /// <summary>
      ///   Gets the value of the bus timing segment 1. The value is scaled in
      ///   can time quantas for the specific prescaler.
      /// </summary>
      /// <returns>
      ///   Value for the bit timing segment 1.
      /// </returns>
      //*****************************************************************************
      public ushort TimeSegment1
      {
        get
        {
          return( m_wTS1 );
        }
      }

      //*****************************************************************************
      /// <summary>
      ///   Gets the value of the bus timing segment 2. The value is scaled in
      ///   can time quantas for the specific prescaler.
      /// </summary>
      /// <returns>
      ///   Value for the bit timing segment 2.
      /// </returns>
      //*****************************************************************************
      public ushort TimeSegment2
      {
        get
        {
          return( m_wTS2 );
        }
      }

      //*****************************************************************************
      /// <summary>
      ///   Gets the value of the synchronisation jump bit width. The value is scaled in
      ///   can time quantas for the specific prescaler.
      /// </summary>
      /// <returns>
      ///   Value for the synchronisation jump bit width.
      /// </returns>
      //*****************************************************************************
      public ushort Sjw
      {
        get
        {
          return( m_wSJW );
        }
      }

      //*****************************************************************************
      /// <summary>
      ///   Gets the value of the transmitter delay offset. The value is scaled in
      ///   can time quantas for the specific prescaler.
      /// </summary>
      /// <returns>
      ///   Value for the transmitter delay offset.
      /// </returns>
      //*****************************************************************************
      public ushort TransmitterDelay
      {
        get
        {
          return( m_wTDO );
        }
      }


      //*****************************************************************************
      /// <summary>
      ///   Gets the bit time of this CanBitrate2.
      ///   (Bit time of this CanBitrate2 in clock ticks according to SJA1000.)
      /// </summary>
      /// <returns>
      ///   Bit time of this CanBitrate2 in clock ticks according to SJA1000.
      /// </returns>
      //*****************************************************************************
      public Int32 Bittime
      {
        get
        {
          // TODO: think about bit time representation
          return 0;
        }
      }

      //*****************************************************************************
      /// <summary>
      ///   Gets the name of this CanBitrate2.
      /// </summary>
      /// <returns>
      ///   The name of this bitrate.
      /// </returns>
      /// <remarks>
      ///   This property returns either the user defined name of the bit rate,
      ///   if the bit rate was created with a name, or the name of the known
      ///   bit rate. For custom bit rates without a user defined name, the
      ///   property returns the bit timing value as numeric string.
      /// </remarks>
      //*****************************************************************************
      public string  Name
      {
        get
        {
          return( ToString() );
        }
      }


    //--------------------------------------------------------------------
    // static properties
    //--------------------------------------------------------------------

      //*****************************************************************************
      /// <summary>
      ///   Gets an empty bit timing value.
      /// </summary>
      /// <returns>
      ///   Empty bit timing value.
      /// </returns>
      //*****************************************************************************
      public static CanBitrate2 Empty
      {
        get
        {
          return new CanBitrate2(CanBitrateMode.None, 0, 0, 0, 0, 0, "<Empty>");
        }
      }

      //*****************************************************************************
      /// <summary>
      ///   Gets the bit timing value for the predefined CiA bit rate of 10 kbit/s.
      /// </summary>
      /// <returns>
      ///   Bit timing value for the predefined CiA 10 kbit/s.
      /// </returns>
      //*****************************************************************************
      public static CanBitrate2 Cia10KBit
      {
        get
        {
          return new CanBitrate2(CanBitrateMode.None, 10000, 14, 2, 1, 0, "CiA 10 kbit/s");
        }
      }

      //*****************************************************************************
      /// <summary>
      ///   Gets the bit timing value for the predefined CiA bit rate of 20 kbit/s.
      /// </summary>
      /// <returns>
      ///   Bit timing value for the predefined CiA 20 kbit/s.
      /// </returns>
      //*****************************************************************************
      public static CanBitrate2 Cia20KBit
      {
        get
        {
          return new CanBitrate2(CanBitrateMode.None, 20000, 14, 2, 1, 0, "CiA 20 kbit/s");
        }
      }

      //*****************************************************************************
      /// <summary>
      ///   Gets the bit timing value for the predefined CiA bit rate of 50 kbit/s.
      /// </summary>
      /// <returns>
      ///   Bit timing value for the predefined CiA 50 kbit/s.
      /// </returns>
      //*****************************************************************************
      public static CanBitrate2 Cia50KBit
      {
        get
        {
          return new CanBitrate2(CanBitrateMode.None, 50000, 14, 2, 1, 0, "CiA 50 kbit/s");
        }
      }

      //*****************************************************************************
      /// <summary>
      ///   Gets the bit timing value for the predefined CiA bit rate of 125 kbit/s.
      /// </summary>
      /// <returns>
      ///   Bit timing value for the predefined CiA 125 kbit/s.
      /// </returns>
      //*****************************************************************************
      public static CanBitrate2 Cia125KBit
      {
        get
        {
          return new CanBitrate2(CanBitrateMode.None, 125000, 14, 2, 1, 0, "CiA 125 kbit/s");
        }
      }

      //*****************************************************************************
      /// <summary>
      ///   Gets the bit timing value for the predefined CiA bit rate of 250 kbit/s.
      /// </summary>
      /// <returns>
      ///   Bit timing value for the predefined CiA 250 kbit/s.
      /// </returns>
      //*****************************************************************************
      public static CanBitrate2 Cia250KBit
      {
        get
        {
          return new CanBitrate2(CanBitrateMode.None, 250000, 14, 2, 1, 0, "CiA 250 kbit/s");
        }
      }

      //*****************************************************************************
      /// <summary>
      ///   Gets the bit timing value for the predefined CiA bit rate of 500 kbit/s.
      /// </summary>
      /// <returns>
      ///   Bit timing value for the predefined CiA 500 kbit/s.
      /// </returns>
      //*****************************************************************************
      public static CanBitrate2 Cia500KBit
      {
        get
        {
          return new CanBitrate2(CanBitrateMode.None, 500000, 14, 2, 1, 0, "CiA 500 kbit/s");
        }
      }

      //*****************************************************************************
      /// <summary>
      ///   Gets the bit timing value for the predefined CiA bit rate of 800 kbit/s.
      /// </summary>
      /// <returns>
      ///   Bit timing value for the predefined CiA 800 kbit/s.
      /// </returns>
      //*****************************************************************************
      public static CanBitrate2 Cia800KBit
      {
        get
        {
          return new CanBitrate2(CanBitrateMode.None, 800000, 8, 2, 1, 0, "CiA 800 kbit/s");
        }
      }

      //*****************************************************************************
      /// <summary>
      ///   Gets the bit timing value for the predefined CiA bit rate of 1000 kbit/s.
      /// </summary>
      /// <returns>
      ///   Bit timing value for the predefined CiA 1000 kbit/s.
      /// </returns>
      //*****************************************************************************
      public static CanBitrate2 Cia1000KBit
      {
        get
        {
          return new CanBitrate2(CanBitrateMode.None, 1000000, 6, 2, 1, 0, "CiA 1000 kbit/s");
        }
      }

      //*****************************************************************************
      /// <summary>
      ///   Gets the bit timing value for the predefined bit rate of 100 kbit/s.
      /// </summary>
      /// <returns>
      ///   Bit timing value for the predefined 100 kbit/s.
      /// </returns>
      //*****************************************************************************
      public static CanBitrate2 _100KBit
      {
        get
        {
          return new CanBitrate2(CanBitrateMode.None, 100000, 14, 2, 1, 0, "100 kbit/s");
        }
      }

      //*****************************************************************************
      /// <summary>
      ///   Gets the bit timing value for the predefined bit rate of 833 kbit/s.
      ///   (raw, IFI CAN-FD specific)
      /// </summary>
      /// <returns>
      ///   Bit timing value for the predefined 833 kbit/s.
      /// </returns>
      //*****************************************************************************
      public static CanBitrate2 IFI833KBit
      {
        get
        {
          return new CanBitrate2(CanBitrateMode.Raw, 6, 12, 3, 3, 78, "IFI CAN-FD 1000 kbit/s");
        }
      }

      //*****************************************************************************
      /// <summary>
      ///   Gets the bit timing value for the predefined bit rate of 1000 kbit/s.
      ///   (raw, IFI CAN-FD specific)
      /// </summary>
      /// <returns>
      ///   Bit timing value for the predefined 1000 kbit/s.
      /// </returns>
      //*****************************************************************************
      public static CanBitrate2 IFI1000KBit
      {
        get
        {
          return new CanBitrate2(CanBitrateMode.Raw, 4, 15, 4, 4, 64, "IFI CAN-FD 1000 kbit/s");
        }
      }

      //*****************************************************************************
      /// <summary>
      ///   Gets the bit timing value for the predefined bit rate of 2000 kbit/s.
      ///   (raw, IFI CAN-FD specific)
      /// </summary>
      /// <returns>
      ///   Bit timing value for the predefined 2000 kbit/s.
      /// </returns>
      //*****************************************************************************
      public static CanBitrate2 IFI2000KBit
      {
        get
        {
          return new CanBitrate2(CanBitrateMode.Raw, 2, 15, 4, 4, 32, "IFI CAN-FD 2000 kbit/s");
        }
      }

      //*****************************************************************************
      /// <summary>
      ///   Gets the bit timing value for the predefined bit rate of 4000 kbit/s.
      ///   (raw, IFI CAN-FD specific)
      /// </summary>
      /// <returns>
      ///   Bit timing value for the predefined 4000 kbit/s.
      /// </returns>
      //*****************************************************************************
      public static CanBitrate2 IFI4000KBit
      {
        get
        {
          return new CanBitrate2(CanBitrateMode.Raw, 2, 7, 2, 2, 16, "IFI CAN-FD 4000 kbit/s");
        }
      }

      //*****************************************************************************
      /// <summary>
      ///   Gets the bit timing value for the predefined bit rate of 5000 kbit/s.
      ///   (raw, IFI CAN-FD specific)
      /// </summary>
      /// <returns>
      ///   Bit timing value for the predefined 5000 kbit/s.
      /// </returns>
      //*****************************************************************************
      public static CanBitrate2 IFI5000KBit
      {
        get
        {
          return new CanBitrate2(CanBitrateMode.Raw, 2, 5, 2, 2, 12, "IFI CAN-FD 5000 kbit/s");
        }
      }

      //*****************************************************************************
      /// <summary>
      ///   Gets the bit timing value for the predefined bit rate of 6667 kbit/s.
      ///   (raw, IFI CAN-FD specific)
      /// </summary>
      /// <returns>
      ///   Bit timing value for the predefined 6667 kbit/s.
      /// </returns>
      //*****************************************************************************
      public static CanBitrate2 IFI6667KBit
      {
        get
        {
          return new CanBitrate2(CanBitrateMode.Raw, 2, 3, 2, 2, 8, "IFI CAN-FD 6667 kbit/s");
        }
      }

      //*****************************************************************************
      /// <summary>
      ///   Gets the bit timing value for the predefined bit rate of 8000 kbit/s.
      ///   (raw, IFI CAN-FD specific)
      /// </summary>
      /// <returns>
      ///   Bit timing value for the predefined 8000 kbit/s.
      /// </returns>
      //*****************************************************************************
      public static CanBitrate2 IFI8000KBit
      {
        get
        {
          return new CanBitrate2(CanBitrateMode.Raw, 2, 3, 1, 1, 5, "IFI CAN-FD 8000 kbit/s");
        }
      }

      //*****************************************************************************
      /// <summary>
      ///   Gets the bit timing value for the predefined bit rate of 10000 kbit/s.
      ///   (raw, IFI CAN-FD specific)
      /// </summary>
      /// <returns>
      ///   Bit timing value for the predefined 10000 kbit/s.
      /// </returns>
      //*****************************************************************************
      public static CanBitrate2 IFI10000KBit
      {
        get
        {
          return new CanBitrate2(CanBitrateMode.Raw, 2, 2, 1, 1, 4, "IFI CAN-FD 10000 kbit/s");
        }
      }

      //*****************************************************************************
      /// <summary>
      ///   Gets the bit timing value for the predefined bit rate of 250 kbit/s.
      /// </summary>
      /// <returns>
      ///   Bit timing value for the predefined 250 kbit/s.
      /// </returns>
      //*****************************************************************************
      public static CanBitrate2 CANFD250KBit
      {
        get
        {
          return new CanBitrate2(CanBitrateMode.None, 250000, 6400, 1600, 1600, (ushort)((6400 + 1600) * 0.8), "CANFD 250 kbit/s");
        }
      }

      //*****************************************************************************
      /// <summary>
      ///   Gets the bit timing value for the predefined bit rate of 500 kbit/s.
      /// </summary>
      /// <returns>
      ///   Bit timing value for the predefined 500 kbit/s.
      /// </returns>
      //*****************************************************************************
      public static CanBitrate2 CANFD500KBit
      {
        get
        {
          return new CanBitrate2(CanBitrateMode.None, 500000,  6400, 1600, 1600,  (ushort)((6400+1600) * 0.8), "CANFD 500 kbit/s");
        }
      }

      //*****************************************************************************
      /// <summary>
      ///   Gets the bit timing value for the predefined bit rate of 833 kbit/s.
      /// </summary>
      /// <returns>
      ///   Bit timing value for the predefined 833 kbit/s.
      /// </returns>
      //*****************************************************************************
      public static CanBitrate2 CANFD833KBit
      {
        get
        {
          return new CanBitrate2(CanBitrateMode.None, 833333, 1600, 400, 400, (ushort)((6400 + 1600) * 0.81), "CANFD 833 kbit/s");
        }
      }

      //*****************************************************************************
      /// <summary>
      ///   Gets the bit timing value for the predefined bit rate of 1000 kbit/s.
      /// </summary>
      /// <returns>
      ///   Bit timing value for the predefined 1000 kbit/s.
      /// </returns>
      //*****************************************************************************
      public static CanBitrate2 CANFD1000KBit
      {
        get
        {
          return new CanBitrate2(CanBitrateMode.None, 1000000, 1600, 400, 400, (ushort)((1600 + 400) * 0.8), "CANFD 1000 kbit/s");
        }
      }

      //*****************************************************************************
      /// <summary>
      ///   Gets the bit timing value for the predefined bit rate of 1538 kbit/s.
      /// </summary>
      /// <returns>
      ///   Bit timing value for the predefined 1538 kbit/s.
      /// </returns>
      //*****************************************************************************
      public static CanBitrate2 CANFD1538KBit
      {
        get
        {
          return new CanBitrate2(CanBitrateMode.None, 1538461, 1000, 300, 300, (ushort)((1000 + 300) * 0.8), "CANFD 1538 kbit/s");
        }
      }

      //*****************************************************************************
      /// <summary>
      ///   Gets the bit timing value for the predefined bit rate of 2000 kbit/s.
      /// </summary>
      /// <returns>
      ///   Bit timing value for the predefined 2000 kbit/s.
      /// </returns>
      //*****************************************************************************
      public static CanBitrate2 CANFD2000KBit
      {
        get
        {
          return new CanBitrate2(CanBitrateMode.None, 2000000, 1600, 400, 400, (ushort)((1600 + 400) * 0.8), "CANFD 2000 kbit/s");
        }
      }

      //*****************************************************************************
      /// <summary>
      ///   Gets the bit timing value for the predefined bit rate of 4000 kbit/s.
      /// </summary>
      /// <returns>
      ///   Bit timing value for the predefined 4000 kbit/s.
      /// </returns>
      //*****************************************************************************
      public static CanBitrate2 CANFD4000KBit
      {
        get
        {
          return new CanBitrate2(CanBitrateMode.None, 4000000, 800, 200, 200, (ushort)((800 + 200) * 0.8), "CANFD 4000 kbit/s");
        }
      }

      //*****************************************************************************
      /// <summary>
      ///   Gets the bit timing value for the predefined bit rate of 5000 kbit/s.
      /// </summary>
      /// <returns>
      ///   Bit timing value for the predefined 5000 kbit/s.
      /// </returns>
      //*****************************************************************************
      public static CanBitrate2 CANFD5000KBit
      {
        get
        {
          return new CanBitrate2(CanBitrateMode.None, 5000000, 600, 200, 200, (ushort)((600 + 200) * 0.75), "CANFD 5000 kbit/s");
        }
      }

      //*****************************************************************************
      /// <summary>
      ///   Gets the bit timing value for the predefined bit rate of 6666 kbit/s.
      /// </summary>
      /// <returns>
      ///   Bit timing value for the predefined 6666 kbit/s.
      /// </returns>
      //*****************************************************************************
      public static CanBitrate2 CANFD6667KBit
      {
        get
        {
          return new CanBitrate2(CanBitrateMode.None, 6666666, 400, 200, 200, (ushort)((400 + 200) * 0.67), "CANFD 6667 kbit/s");
        }
      }

      //*****************************************************************************
      /// <summary>
      ///   Gets the bit timing value for the predefined bit rate of 8000 kbit/s.
      /// </summary>
      /// <returns>
      ///   Bit timing value for the predefined 8000 kbit/s.
      /// </returns>
      //*****************************************************************************
      public static CanBitrate2 CANFD8000KBit
      {
        get
        {
          return new CanBitrate2(CanBitrateMode.None, 8000000, 400, 100, 100, (ushort)((400 + 100) * 0.5), "CANFD 8000 kbit/s");
        }
      }

      //*****************************************************************************
      /// <summary>
      ///   Gets the bit timing value for the predefined bit rate of 10000 kbit/s.
      /// </summary>
      /// <returns>
      ///   Bit timing value for the predefined 10000 kbit/s.
      /// </returns>
      //*****************************************************************************
      public static CanBitrate2 CANFD10000KBit
      {
        get
        {
          return new CanBitrate2(CanBitrateMode.None, 10000000, 300, 100, 100, (ushort)((300 + 100) * 0.5), "CANFD 10000 kbit/s");
        }
      }

      //*****************************************************************************
      /// <summary>
      ///   Gets an array of all available CANFD raw bit rates specific 
      ///   for IFI CANFD controllers.
      /// </summary>
      /// <returns>
      ///   Array of all available CiA baud rates.
      /// </returns>
      //*****************************************************************************
      public static CanBitrate2[] CanFdIFIBitRates
      {
        get
        {
          CanBitrate2[] fdRates = new CanBitrate2[] { IFI1000KBit,
                                                      IFI2000KBit,
                                                      IFI4000KBit,
                                                      IFI5000KBit,
                                                      IFI6667KBit,
                                                      IFI8000KBit,
                                                      IFI10000KBit
                                                    };
          return fdRates;
        }
      }

      //*****************************************************************************
      /// <summary>
      ///   Gets an array of all available non raw CAN-FD bit rates.
      /// </summary>
      /// <returns>
      ///   Array of all available non raw CAN-FD bit rates.
      /// </returns>
      //*****************************************************************************
      public static CanBitrate2[] CanFdBitRates
      {
        get
        {
          CanBitrate2[] fdRates = new CanBitrate2[] { CANFD1000KBit,
                                                      CANFD2000KBit,
                                                      CANFD4000KBit,
                                                      CANFD5000KBit,
                                                      CANFD6667KBit,
                                                      CANFD8000KBit,
                                                      CANFD10000KBit
                                                    };
          return fdRates;
        }
      }

      //*****************************************************************************
      /// <summary>
      ///   Gets an array of all available CiA bit rates.
      /// </summary>
      /// <returns>
      ///   Array of all available CiA bit rates.
      /// </returns>
      //*****************************************************************************
      public static CanBitrate2[] CiaBitRates
      {
        get
        {
          CanBitrate2[] ciaRates = new CanBitrate2[] { Cia10KBit,  
                                                       Cia20KBit,  
                                                       Cia50KBit,
                                                       Cia125KBit, 
                                                       Cia250KBit,
                                                       Cia500KBit, 
                                                       Cia800KBit, 
                                                       Cia1000KBit
                                                     };
          return ciaRates;
        }
      }


    //--------------------------------------------------------------------
    // member functions
    //--------------------------------------------------------------------

      //*****************************************************************************
      /// <summary>
      ///   Constructor for a new CanBitrate2.
      /// </summary>
      /// <param name="BittimingMode">
      ///   Value for bit timing register 0.
      /// </param>
      /// <param name="Prescaler">
      ///   Bits per second or prescaler (see <c>CanBitrateMode</c>).
      /// </param>
      /// <param name="TimeSegment1">
      ///   Length of time segment 1 in quantas.
      /// </param>
      /// <param name="TimeSegment2">
      ///   Length of time segment 2 in quantas.
      /// </param>
      /// <param name="Sjw">
      ///   Re-synchronisation jump width in quantas.
      /// </param>
      /// <param name="TranmitterDelay">
      ///   Transceiver delay compensation offset in quantas.
      ///   (0 = disabled)
      /// </param>
      //*****************************************************************************
      public CanBitrate2(CanBitrateMode BittimingMode, uint   Prescaler, ushort TimeSegment1,
        ushort TimeSegment2, ushort Sjw, ushort TranmitterDelay)
      {
        m_dwMode = (UInt32)BittimingMode;
        m_dwBPS = Prescaler;
        m_wTS1 = TimeSegment1;
        m_wTS2 = TimeSegment2;
        m_wSJW = Sjw;
        m_wTDO = TranmitterDelay;
        m_sName = null;
      }

      //*****************************************************************************
      /// <summary>
      ///   Constructor for a new CanBitrate2.
      /// </summary>
      /// <param name="BittimingMode">
      ///   Value for bit timing register 0.
      /// </param>
      /// <param name="Prescaler">
      ///   Bits per second or prescaler (see <c>CanBitrateMode</c>).
      /// </param>
      /// <param name="TimeSegment1">
      ///   Length of time segment 1 in quantas.
      /// </param>
      /// <param name="TimeSegment2">
      ///   Length of time segment 2 in quantas.
      /// </param>
      /// <param name="Sjw">
      ///   Re-synchronisation jump width in quantas.
      /// </param>
      /// <param name="TranmitterDelay">
      ///   Transceiver delay compensation offset in quantas.
      ///   (0 = disabled)
      /// </param>
      /// <param name="name">
      ///   Human readable name of the bit rate. 
      /// </param>
      //*****************************************************************************
      public CanBitrate2(CanBitrateMode BittimingMode, uint   Prescaler, ushort TimeSegment1,
        ushort TimeSegment2, ushort Sjw, ushort TranmitterDelay, string  name)
      {
        m_dwMode = (UInt32)BittimingMode;
        m_dwBPS = Prescaler;
        m_wTS1 = TimeSegment1;
        m_wTS2 = TimeSegment2;
        m_wSJW = Sjw;
        m_wTDO = TranmitterDelay;
        m_sName = name;
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
        if (null == m_sName)
        {
          // TODO: think about representation
          m_sName = "not specified";
        }

        return( m_sName );
      }

      //*****************************************************************************
      /// <summary>
      ///   Determines whether the specified Object is equal to the current Object.
      /// </summary>
      /// <pararm name ="obj">
      ///   The Object to compare with the current Object.
      /// </pararm>
      /// <returns>
      ///   true if the specified Object is equal to the current Object; 
      ///   otherwise, false.
      /// </returns>
      //*****************************************************************************
      public override bool Equals(Object? obj)
      {
        // Check for null values and compare run-time types.
        if (!(obj is CanBitrate2))
          return false;

        return Equals((CanBitrate2)obj);
      }

      //*****************************************************************************
      /// <summary>
      ///   Determines whether the specified Object is equal to the current Object.
      /// </summary>
      /// <pararm name ="other">
      ///   The Object to compare with the current Object.
      /// </pararm>
      /// <returns>
      ///   true if the specified Object is equal to the current Object; 
      ///   otherwise, false.
      /// </returns>
      //*****************************************************************************
      public bool Equals(CanBitrate2 other)
      {
        return ((Mode == other.Mode) &&
                (Prescaler == other.Prescaler) &&
                (TimeSegment1 == other.TimeSegment1) &&
                (TimeSegment2 == other.TimeSegment2) &&
                (Sjw == other.Sjw) &&
                (TransmitterDelay == other.TransmitterDelay));
      }

      //*****************************************************************************
      /// <summary>
      ///   Comparison operator ==
      /// </summary>
      /// <pararm name ="lhs">
      ///   left hand side object to compare
      /// </pararm>
      /// <pararm name ="rhs">
      ///   right hand side object to compare
      /// </pararm>
      /// <returns>
      ///   true if the specified Object is equal to the current Object; 
      ///   otherwise, false.
      /// </returns>
      //*****************************************************************************
      public static bool operator ==(CanBitrate2 lhs, CanBitrate2 rhs)
      {
        return lhs.Equals(rhs);
      }

      //*****************************************************************************
      /// <summary>
      ///   Comparison operator !=
      /// </summary>
      /// <pararm name ="lhs">
      ///   left hand side object to compare
      /// </pararm>
      /// <pararm name ="rhs">
      ///   right hand side object to compare
      /// </pararm>
      /// <returns>
      ///   true if the specified Object is not equal to the current Object; 
      ///   otherwise, false.
      /// </returns>
      //*****************************************************************************
      public static bool operator !=(CanBitrate2 lhs, CanBitrate2 rhs)
      {
        return !lhs.Equals(rhs);
      }

      //*****************************************************************************
      /// <summary>
      ///   Serves as a hash function for a particular type. GetHashCode is suitable 
      ///   for use in hashing algorithms and data structures like a hash table.
      /// </summary>
      /// <returns>
      ///   A hash code for the current Object. 
      /// </returns>
      //*****************************************************************************
      public override int GetHashCode()
      {
        return (int)(m_dwMode + m_dwBPS + m_wTS1 + m_wTS2 + m_wSJW + m_wTDO);
      }
  };


  //*****************************************************************************
  /// <summary>
  ///   Instances of <c>CanFdBitrate</c> represent a CAN-FD bit rate.
  ///   It consists of two bitrates, one for the standard bitrate (Sdr)
  ///   and one for the fast bitrate (Fdr)
  /// </summary>
  //*****************************************************************************
  public struct CanFdBitrate
  {
    private CanBitrate2 m_StdBitrate;    // standard bitrate
    private CanBitrate2 m_FastBitrate;    // fast bitrate

    //*****************************************************************************
    /// <summary>
    /// Property to get standard bitrate
    /// </summary>
    /// <returns>
    ///   Standard bitrate.
    /// </returns>
    //*****************************************************************************
    public CanBitrate2 StdBitrate
    {
      get
      {
        return( m_StdBitrate );
      }
    }

    //*****************************************************************************
    /// <summary>
    /// Property to get fast bitrate
    /// </summary>
    /// <returns>
    ///   Fast bitrate.
    /// </returns>
    //*****************************************************************************
    public CanBitrate2 FastBitrate
    {
      get
      {
        return( m_FastBitrate );
      }
    }

    //*****************************************************************************
    /// <summary>
    ///   Constructor for a CanFdBitrate.
    /// </summary>
    /// <param name="stdBitrate">
    ///   Standard bitrate
    /// </param>
    /// <param name="fastBitrate">
    ///   Fast bitrate
    /// </param>
    //*****************************************************************************
    public CanFdBitrate(CanBitrate2 stdBitrate, CanBitrate2 fastBitrate)
    {
      m_StdBitrate  = stdBitrate;
      m_FastBitrate = fastBitrate;
    }

    //*****************************************************************************
    /// <summary>
    ///   Copy constructor for a CanFdBitrate.
    /// </summary>
    /// <param name="bitrate">
    ///   Bitrate to copy from
    /// </param>
    //*****************************************************************************
    public CanFdBitrate(CanBitrate2 bitrate)
    {
      m_StdBitrate  = bitrate;
      m_FastBitrate = bitrate;
    }

    //*****************************************************************************
    /// <summary>
    ///   Gets an array of all available CAN CiA fast data bitrates.
    /// </summary>
    /// <returns>
    ///   Array of all available CiA baud rates.
    /// </returns>
    //*****************************************************************************
    public static CanFdBitrate[] CiaBitRates
    {
      get
      {
        CanFdBitrate[] ciaRates = new CanFdBitrate[] { new CanFdBitrate(CanBitrate2.Cia10KBit),
                                                       new CanFdBitrate(CanBitrate2.Cia20KBit),  
                                                       new CanFdBitrate(CanBitrate2.Cia50KBit),
                                                       new CanFdBitrate(CanBitrate2.Cia125KBit), 
                                                       new CanFdBitrate(CanBitrate2.Cia250KBit),
                                                       new CanFdBitrate(CanBitrate2.Cia500KBit), 
                                                       new CanFdBitrate(CanBitrate2.Cia800KBit), 
                                                       new CanFdBitrate(CanBitrate2.Cia1000KBit)
                                                     };
        return ciaRates;
      }
    }

    //*****************************************************************************
    /// <summary>
    ///   Gets an array of common CANFD bitrates for short lines
    /// </summary>
    /// <returns>
    ///   Array of all available CiA baud rates.
    /// </returns>
    //*****************************************************************************
    public static CanFdBitrate[] ShortLineCANFDBitRates
    {
      get
      {
        CanFdBitrate[] ciaRates = new CanFdBitrate[] { new CanFdBitrate(CanBitrate2.CANFD500KBit, CanBitrate2.CANFD1000KBit),
                                                       new CanFdBitrate(CanBitrate2.CANFD500KBit, CanBitrate2.CANFD2000KBit),
                                                       new CanFdBitrate(CanBitrate2.CANFD500KBit, CanBitrate2.CANFD4000KBit),
                                                       new CanFdBitrate(CanBitrate2.CANFD500KBit, CanBitrate2.CANFD5000KBit),
                                                       new CanFdBitrate(CanBitrate2.CANFD500KBit, CanBitrate2.CANFD6667KBit),
                                                       new CanFdBitrate(CanBitrate2.CANFD500KBit, CanBitrate2.CANFD8000KBit),
                                                       new CanFdBitrate(CanBitrate2.CANFD500KBit, CanBitrate2.CANFD10000KBit)
                                                     };
        return ciaRates;
      }
    }

    //*****************************************************************************
    /// <summary>
    ///   Gets an array of common CANFD bitrates for long lines
    /// </summary>
    /// <returns>
    ///   Array of all available CiA baud rates.
    /// </returns>
    //*****************************************************************************
    public static CanFdBitrate[] LongLineCANFDBitRates
    {
      get
      {
        CanFdBitrate[] ciaRates = new CanFdBitrate[] { new CanFdBitrate(CanBitrate2.CANFD250KBit, CanBitrate2.CANFD500KBit),
                                                       new CanFdBitrate(CanBitrate2.CANFD250KBit, CanBitrate2.CANFD833KBit),
                                                       new CanFdBitrate(CanBitrate2.CANFD250KBit, CanBitrate2.CANFD1000KBit),
                                                       new CanFdBitrate(CanBitrate2.CANFD250KBit, CanBitrate2.CANFD1538KBit),
                                                       new CanFdBitrate(CanBitrate2.CANFD250KBit, CanBitrate2.CANFD2000KBit),
                                                       new CanFdBitrate(CanBitrate2.CANFD250KBit, CanBitrate2.CANFD4000KBit)
                                                     };
        return ciaRates;
      }
    }
  }



}