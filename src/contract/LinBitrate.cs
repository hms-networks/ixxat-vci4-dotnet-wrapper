/*****************************************************************************
 IXXAT Automation GmbH
******************************************************************************
 Summary : Declarations for the LIN bitrate value class.
 Compiler: Microsoft VC++ 8
******************************************************************************
 all rights reserved
*****************************************************************************/


/*************************************************************************
** namespace Ixxat.Vci4.Bal.Lin
*************************************************************************/
namespace Ixxat.Vci4.Bal.Lin
{
  /*************************************************************************
  ** used namespace
  *************************************************************************/
  using System;

  //*****************************************************************************
  /// <summary>
  ///   Instances of <c>LinBitrate</c> represent a bit rate for LIN busses.
  ///   The predefined bitrates are provided as static <c>LinBitrate</c> members 
  ///   like <c>Lin1000Baud</c>.
  /// </summary>
  //*****************************************************************************
  public struct LinBitrate
  {
    const int LIN_BITRATE_UNDEF = 65535;
    const int LIN_BITRATE_AUTO  = 0;
    const int LIN_BITRATE_MIN   = 1000;
    const int LIN_BITRATE_MAX   = 20000;
                                    
    const int LIN_BITRATE_1000  = 1000;
    const int LIN_BITRATE_1200  = 1200;
    const int LIN_BITRATE_2400  = 2400;
    const int LIN_BITRATE_4800  = 4800;
    const int LIN_BITRATE_9600  = 9600;
    const int LIN_BITRATE_10400 = 10400;
    const int LIN_BITRATE_19200 = 19200;
    const int LIN_BITRATE_20000 = 20000;

  #region Member variables
    //--------------------------------------------------------------------
    // member variables
    //--------------------------------------------------------------------
      private ushort  m_wBitrate;   // contains the bitrate of a LinBitrate instance
      private string? m_sName;      // human readable name of the bit rate

  #endregion

  #region Properties
    //--------------------------------------------------------------------
    // properties
    //--------------------------------------------------------------------

      //*****************************************************************************
      /// <summary>
      ///   Gets the 16 bit value of this LinBitrate structure.
      /// </summary>
      /// <returns>
      ///   The 16 bit value of this LinBitrate structure.
      /// </returns>
      //*****************************************************************************
      public ushort AsUInt16
      {
        get
        {
          return( m_wBitrate );
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
      public string Name
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
      ///   Gets an undefined bit timing value.
      /// </summary>
      /// <returns>
      ///   Undefined bit timing value.
      /// </returns>
      //*****************************************************************************
      public static LinBitrate Undefined
      {
        get
        {
          return new LinBitrate(LIN_BITRATE_UNDEF, "Undefined");
        }
      }

      //*****************************************************************************
      /// <summary>
      ///   Gets the bit timing value used for automatic bitrate detection in 
      ///   <c>ILinControl.InitLine</c>.
      /// </summary>
      /// <returns>
      ///   Bit timing value used for automatic bitrate detection.
      /// </returns>
      //*****************************************************************************
      public static LinBitrate AutoRate
      {
        get
        {
          return new LinBitrate(LIN_BITRATE_AUTO, "Automatic");
        }
      }

      //*****************************************************************************
      /// <summary>
      ///   Gets the bit timing value for the predefined lowest LIN bitrate.
      /// </summary>
      /// <returns>
      ///   Bit timing value for the predefined lowest LIN bitrate.
      /// </returns>
      //*****************************************************************************
      public static LinBitrate MinBitrate
      {
        get
        {
          return new LinBitrate(LIN_BITRATE_MIN, "Lowest");
        }
      }

      //*****************************************************************************
      /// <summary>
      ///   Gets the bit timing value for the predefined highest LIN bitrate.
      /// </summary>
      /// <returns>
      ///   Bit timing value for the predefined highest LIN bitrate.
      /// </returns>
      //*****************************************************************************
      public static LinBitrate MaxBitrate
      {
        get
        {
          return new LinBitrate(LIN_BITRATE_MAX, "Highest");
        }
      }

      //*****************************************************************************
      /// <summary>
      ///   Gets the bit timing value for the predefined bit rate of 1000 bit/s.
      /// </summary>
      /// <returns>
      ///   Bit timing value for the predefined bit rate of 1000 bit/s.
      /// </returns>
      //*****************************************************************************
      public static LinBitrate Lin1000Bit
      {
        get
        {
          return new LinBitrate(LIN_BITRATE_1000);
        }
      }

      //*****************************************************************************
      /// <summary>
      ///   Gets the bit timing value for the predefined bit rate of 1200 bit/s.
      /// </summary>
      /// <returns>
      ///   Bit timing value for the predefined bit rate of 1200 bit/s.
      /// </returns>
      //*****************************************************************************
      public static LinBitrate Lin1200Bit
      {
        get
        {
          return new LinBitrate(LIN_BITRATE_1200);
        }
      }

      //*****************************************************************************
      /// <summary>
      ///   Gets the bit timing value for the predefined bit rate of 2400 bit/s.
      /// </summary>
      /// <returns>
      ///   Bit timing value for the predefined bit rate of 2400 bit/s.
      /// </returns>
      //*****************************************************************************
      public static LinBitrate Lin2400Bit
      {
        get
        {
          return new LinBitrate(LIN_BITRATE_2400);
        }
      }

      //*****************************************************************************
      /// <summary>
      ///   Gets the bit timing value for the predefined bit rate of 4800 bit/s.
      /// </summary>
      /// <returns>
      ///   Bit timing value for the predefined bit rate of 4800 bit/s.
      /// </returns>
      //*****************************************************************************
      public static LinBitrate Lin4800Bit
      {
        get
        {
          return new LinBitrate(LIN_BITRATE_4800);
        }
      }

      //*****************************************************************************
      /// <summary>
      ///   Gets the bit timing value for the predefined bit rate of 9600 bit/s.
      /// </summary>
      /// <returns>
      ///   Bit timing value for the predefined bit rate of 9600 bit/s.
      /// </returns>
      //*****************************************************************************
      public static LinBitrate Lin9600Bit
      {
        get
        {
          return new LinBitrate(LIN_BITRATE_9600);
        }
      }

      //*****************************************************************************
      /// <summary>
      ///   Gets the bit timing value for the predefined bit rate of 10400 bit/s.
      /// </summary>
      /// <returns>
      ///   Bit timing value for the predefined bit rate of 10400 bit/s.
      /// </returns>
      //*****************************************************************************
      public static LinBitrate Lin10400Bit
      {
        get
        {
          return new LinBitrate(LIN_BITRATE_10400);
        }
      }

      //*****************************************************************************
      /// <summary>
      ///   Gets the bit timing value for the predefined bit rate of 19200 bit/s.
      /// </summary>
      /// <returns>
      ///   Bit timing value for the predefined bit rate of 19200 bit/s.
      /// </returns>
      //*****************************************************************************
      public static LinBitrate Lin19200Bit
      {
        get
        {
          return new LinBitrate(LIN_BITRATE_19200);
        }
      }

      //*****************************************************************************
      /// <summary>
      ///   Gets the bit timing value for the predefined bit rate of 20000 bit/s.
      /// </summary>
      /// <returns>
      ///   Bit timing value for the predefined bit rate of 20000 bit/s.
      /// </returns>
      //*****************************************************************************
      public static LinBitrate Lin20000Bit
      {
        get
        {
          return new LinBitrate(LIN_BITRATE_20000);
        }
      }
  #endregion

  #region Member functions
    //--------------------------------------------------------------------
    // member functions
    //--------------------------------------------------------------------

      //*****************************************************************************
      /// <summary>
      ///   Constructor for a new LinBitrate.
      /// </summary>
      /// <param name="bitrate">
      ///   The bitrate in bit/sec
      /// </param>
      //*****************************************************************************
      public LinBitrate(ushort bitrate)
      {
        m_wBitrate = bitrate;
        m_sName = null;
      }

      //*****************************************************************************
      /// <summary>
      ///   Constructor for a new LinBitrate.
      /// </summary>
      /// <param name="bitrate">
      ///   The bitrate in bit/sec
      /// </param>
      /// <param name="name">
      ///   Human readable name of the bit rate.
      /// </param>
      //*****************************************************************************
      public LinBitrate(ushort bitrate, string name)
      {
        m_wBitrate = bitrate;
        m_sName = null;
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
          m_sName = String.Format("{0} bit/s", m_wBitrate);
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
        if (!(obj is LinBitrate))
          return false;

        return Equals((LinBitrate)obj);
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
      public bool Equals(LinBitrate other)
      {
        return (AsUInt16 == other.AsUInt16);
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
      public static bool operator ==(LinBitrate lhs, LinBitrate rhs)
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
      public static bool operator !=(LinBitrate lhs, LinBitrate rhs)
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
        return (int)AsUInt16;
      }
  #endregion
  };


}