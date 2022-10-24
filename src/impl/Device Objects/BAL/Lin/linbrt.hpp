/*****************************************************************************
 IXXAT Automation GmbH
******************************************************************************
 Summary : Declarations for the LIN bitrate value class.
 Compiler: Microsoft VC++ 8
******************************************************************************
 all rights reserved
*****************************************************************************/

#pragma once

/*************************************************************************
** include files
*************************************************************************/
#include <vcinet.h>

/*************************************************************************
** namespace Ixxat.Vci4.Bal.Lin
*************************************************************************/
namespace Ixxat {
  namespace Vci4 {
    namespace Bal {
      namespace Lin {


//*****************************************************************************
/// <summary>
///   Instances of <c>LinBitrate</c> represent a bit rate for LIN busses.
///   The pedefined bitrates are provided as static <c>LinBitrate</c> members 
//    like <c>Lin1000Baud</c>.
/// </summary>
//*****************************************************************************
public value class LinBitrate
{
#pragma region Member variables
  //--------------------------------------------------------------------
  // member variables
  //--------------------------------------------------------------------
  private:
    UInt16 m_wBitrate; // contains the bitrate of a LinBitrate instance
    String^ m_sName;   // human readable name of the bit rate
  protected:
  internal:
  public:
#pragma endregion

#pragma region Properties
  //--------------------------------------------------------------------
  // properties
  //--------------------------------------------------------------------
  private:
  protected:
  public:
    //*****************************************************************************
    /// <summary>
    ///   Gets the 16 bit value of this LinBitrate structure.
    /// </summary>
    //*****************************************************************************
    property UInt16 AsUInt16
    {
      /// <summary>
      ///   Gets the 16 bit value of this LinBitrate structure.
      /// </summary>
      /// <returns>
      ///   The 16 bit value of this LinBitrate structure.
      /// </returns>
      UInt16 get(void)
      {
        return( m_wBitrate );
      }
    };

    //*****************************************************************************
    /// <summary>
    ///   Gets the bit time of this LinBitrate.
    /// </summary>
    /// <remarks>
    ///   This property returns either the user defined name of the bit rate,
    ///   if the bit rate was created with a name, or the name of the known
    ///   bit rate. For custom bit rates without a user defined name, the
    ///   property returns the bit timing value as numeric string.
    /// </remarks>
    //*****************************************************************************
    property String^ Name
    {
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
      String^ get(void)
      {
        return( ToString() );
      }
    };


  //--------------------------------------------------------------------
  // static properties
  //--------------------------------------------------------------------
  private:
  protected:
  public:
    //*****************************************************************************
    /// <summary>
    ///   Gets an undefined bit timing value.
    /// </summary>
    //*****************************************************************************
    property static LinBitrate Undefined
    {
      /// <summary>
      ///   Gets an undefined bit timing value.
      /// </summary>
      /// <returns>
      ///   Undefined bit timing value.
      /// </returns>
      LinBitrate get(void)
      {
        return LinBitrate(LIN_BITRATE_UNDEF, "Undefined");
      }
    };

    //*****************************************************************************
    /// <summary>
    ///   Gets the bit timing value used for automatic bitrate detection in 
    ///   <c>ILinControl.InitLine</c>.
    /// </summary>
    //*****************************************************************************
    property static LinBitrate AutoRate
    {
      /// <summary>
      ///   Gets the bit timing value used for automatic bitrate detection in 
      ///   <c>ILinControl.InitLine</c>.
      /// </summary>
      /// <returns>
      ///   Bit timing value used for automatic bitrate detection.
      /// </returns>
      LinBitrate get(void)
      {
        return LinBitrate(LIN_BITRATE_AUTO, "Automatic");
      }
    };

    //*****************************************************************************
    /// <summary>
    ///   Gets the bit timing value for the predefined lowest LIN bitrate.
    /// </summary>
    //*****************************************************************************
    property static LinBitrate MinBitrate
    {
      /// <summary>
      ///   Gets the bit timing value for the predefined lowest LIN bitrate.
      /// </summary>
      /// <returns>
      ///   Bit timing value for the predefined lowest LIN bitrate.
      /// </returns>
      LinBitrate get(void)
      {
        return LinBitrate(LIN_BITRATE_MIN, "Lowest");
      }
    };

    //*****************************************************************************
    /// <summary>
    ///   Gets the bit timing value for the predefined highest LIN bitrate.
    /// </summary>
    //*****************************************************************************
    property static LinBitrate MaxBitrate
    {
      /// <summary>
      ///   Gets the bit timing value for the predefined highest LIN bitrate.
      /// </summary>
      /// <returns>
      ///   Bit timing value for the predefined highest LIN bitrate.
      /// </returns>
      LinBitrate get(void)
      {
        return LinBitrate(LIN_BITRATE_MAX, "Highest");
      }
    };

    //*****************************************************************************
    /// <summary>
    ///   Gets the bit timing value for the predefined bit rate of 1000 bit/s.
    /// </summary>
    //*****************************************************************************
    property static LinBitrate Lin1000Bit
    {
      /// <summary>
      ///   Gets the bit timing value for the predefined bit rate of 1000 bit/s.
      /// </summary>
      /// <returns>
      ///   Bit timing value for the predefined bit rate of 1000 bit/s.
      /// </returns>
      LinBitrate get(void)
      {
        return LinBitrate(LIN_BITRATE_1000);
      }
    };

    //*****************************************************************************
    /// <summary>
    ///   Gets the bit timing value for the predefined bit rate of 1200 bit/s.
    /// </summary>
    //*****************************************************************************
    property static LinBitrate Lin1200Bit
    {
      /// <summary>
      ///   Gets the bit timing value for the predefined bit rate of 1200 bit/s.
      /// </summary>
      /// <returns>
      ///   Bit timing value for the predefined bit rate of 1200 bit/s.
      /// </returns>
      LinBitrate get(void)
      {
        return LinBitrate(LIN_BITRATE_1200);
      }
    };

    //*****************************************************************************
    /// <summary>
    ///   Gets the bit timing value for the predefined bit rate of 2400 bit/s.
    /// </summary>
    //*****************************************************************************
    property static LinBitrate Lin2400Bit
    {
      /// <summary>
      ///   Gets the bit timing value for the predefined bit rate of 2400 bit/s.
      /// </summary>
      /// <returns>
      ///   Bit timing value for the predefined bit rate of 2400 bit/s.
      /// </returns>
      LinBitrate get(void)
      {
        return LinBitrate(LIN_BITRATE_2400);
      }
    };

    //*****************************************************************************
    /// <summary>
    ///   Gets the bit timing value for the predefined bit rate of 4800 bit/s.
    /// </summary>
    //*****************************************************************************
    property static LinBitrate Lin4800Bit
    {
      /// <summary>
      ///   Gets the bit timing value for the predefined bit rate of 4800 bit/s.
      /// </summary>
      /// <returns>
      ///   Bit timing value for the predefined bit rate of 4800 bit/s.
      /// </returns>
      LinBitrate get(void)
      {
        return LinBitrate(LIN_BITRATE_4800);
      }
    };

    //*****************************************************************************
    /// <summary>
    ///   Gets the bit timing value for the predefined bit rate of 9600 bit/s.
    /// </summary>
    //*****************************************************************************
    property static LinBitrate Lin9600Bit
    {
      /// <summary>
      ///   Gets the bit timing value for the predefined bit rate of 9600 bit/s.
      /// </summary>
      /// <returns>
      ///   Bit timing value for the predefined bit rate of 9600 bit/s.
      /// </returns>
      LinBitrate get(void)
      {
        return LinBitrate(LIN_BITRATE_9600);
      }
    };

    //*****************************************************************************
    /// <summary>
    ///   Gets the bit timing value for the predefined bit rate of 10400 bit/s.
    /// </summary>
    //*****************************************************************************
    property static LinBitrate Lin10400Bit
    {
      /// <summary>
      ///   Gets the bit timing value for the predefined bit rate of 10400 bit/s.
      /// </summary>
      /// <returns>
      ///   Bit timing value for the predefined bit rate of 10400 bit/s.
      /// </returns>
      LinBitrate get(void)
      {
        return LinBitrate(LIN_BITRATE_10400);
      }
    };

    //*****************************************************************************
    /// <summary>
    ///   Gets the bit timing value for the predefined bit rate of 19200 bit/s.
    /// </summary>
    //*****************************************************************************
    property static LinBitrate Lin19200Bit
    {
      /// <summary>
      ///   Gets the bit timing value for the predefined bit rate of 19200 bit/s.
      /// </summary>
      /// <returns>
      ///   Bit timing value for the predefined bit rate of 19200 bit/s.
      /// </returns>
      LinBitrate get(void)
      {
        return LinBitrate(LIN_BITRATE_19200);
      }
    };

    //*****************************************************************************
    /// <summary>
    ///   Gets the bit timing value for the predefined bit rate of 20000 bit/s.
    /// </summary>
    //*****************************************************************************
    property static LinBitrate Lin20000Bit
    {
      /// <summary>
      ///   Gets the bit timing value for the predefined bit rate of 20000 bit/s.
      /// </summary>
      /// <returns>
      ///   Bit timing value for the predefined bit rate of 20000 bit/s.
      /// </returns>
      LinBitrate get(void)
      {
        return LinBitrate(LIN_BITRATE_20000);
      }
    };
#pragma endregion

#pragma region Member functions
  //--------------------------------------------------------------------
  // member functions
  //--------------------------------------------------------------------
  private:
  protected:
  public:
    //*****************************************************************************
    /// <summary>
    ///   Constructor for a new LinBitrate.
    /// </summary>
    /// <param name="bitrate">
    ///   The bitrate in bit/sec
    /// </param>
    //*****************************************************************************
    LinBitrate(UInt16 bitrate)
    {
      m_wBitrate = bitrate;
      m_sName = nullptr;
    };

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
    LinBitrate(UInt16 bitrate, String^ name)
    {
      m_wBitrate = bitrate;
      m_sName = name;
    };

    //*****************************************************************************
    /// <summary>
    ///   This method returns a String that represents the current timing value.
    /// </summary>
    /// <returns>
    ///   A String that represents the current bit timing value.
    /// </returns>
    //*****************************************************************************
    virtual String^ ToString() override
    {
      if (nullptr == m_sName)
      {
        m_sName = String::Format("{0} bit/s", m_wBitrate);
      }

      return( m_sName );
    };

    //*****************************************************************************
    /// <summary>
    ///   Determines whether the specified Object instances are equal.
    /// </summary>
    /// <pararm name ="value1">
    ///   Value 1.
    /// </pararm>
    /// <pararm name ="value2">
    ///   Value 2.
    /// </pararm>
    /// <returns>
    ///   true if value1 equals value2; otherwise, false.
    /// </returns>
    //*****************************************************************************
    static bool operator == (LinBitrate value1, LinBitrate value2)
    {
      return (value1.AsUInt16 == value2.AsUInt16);
    }

    //*****************************************************************************
    /// <summary>
    ///   Determines whether the specified Object instances are not equal.
    /// </summary>
    /// <pararm name ="value1">
    ///   Value 1.
    /// </pararm>
    /// <pararm name ="value2">
    ///   Value 2.
    /// </pararm>
    /// <returns>
    ///   true if value1 not equals value2; otherwise, false.
    /// </returns>
    //*****************************************************************************
    static bool operator != (LinBitrate value1, LinBitrate value2)
    {
      return !(value1 == value2);
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
    virtual bool Equals(Object^ obj) override
    {
      return *this == obj;
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
    virtual int GetHashCode () override
    {
      return (int)AsUInt16;
    }
#pragma endregion
};


} // end of namespace Lin
} // end of namespace Bal
} // end of namespace Vci4
} // end of namespace Ixxat
