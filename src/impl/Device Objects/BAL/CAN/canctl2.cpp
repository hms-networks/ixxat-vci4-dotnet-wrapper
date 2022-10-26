// SPDX-License-Identifier: MIT
//----------------------------------------------------------------------------
// Summary  : Implementation of the CAN control class.
// Copyright: Copyright (C) 2016-2022 HMS Technology Center Ravensburg GmbH, 
//            all rights reserved
//----------------------------------------------------------------------------

#include "canctl2.hpp"
#include "vcinet.hpp"

using namespace System::Text;
using namespace Ixxat::Vci4::Bal::Can;

//*****************************************************************************
/// <summary>
///   Constructor for VCI CAN control objects.
/// </summary>
/// <param name="pBalObj">
///   Pointer to the native BAL object interface. 
///   This parameter must not be NULL.
/// </param>
/// <param name="portNumber">
///   Port number of the bus socket to open.
/// </param>
/// <param name="busTypeIndex">
///   Bus type related port number
///</param>
/// <exception cref="VciException">
///   Creation of CAN control socket failed.
/// </exception>
/// <exception cref="ArgumentNullException">
///   Native IBalObject was a null pointer.
/// </exception>
//*****************************************************************************
CanControl2::CanControl2(::IBalObject*  pBalObj
                      , Byte          portNumber
                      , Byte          busTypeIndex)
          : CanSocket2(pBalObj, portNumber, busTypeIndex)
{
  HRESULT         hResult;
  ::ICanControl2*  pCanCtl;

  m_pCanCtl = nullptr;

  if (nullptr != pBalObj)
  {
    hResult = pBalObj->OpenSocket( portNumber
                                 , IID_ICanControl2
                                 , (PVOID*) &pCanCtl);

    if (hResult == VCI_OK)
    {
      hResult = InitNew(pCanCtl);
      pCanCtl->Release();
    }

    if (hResult != VCI_OK)
    {
      throw gcnew VciException(VciServerImpl::Instance(), hResult);
    }
  }
  else
  {
    throw gcnew ArgumentNullException();
  }
}

//*****************************************************************************
/// <summary>
///   Destructor for VCI CAN control objects.
/// </summary>
//*****************************************************************************
CanControl2::~CanControl2()
{
  Cleanup();
}

//*****************************************************************************
/// <summary>
///   This method initializes a newly created CAN control object.
/// </summary>
/// <param name="pCanCtl">
///   Pointer to the native CAN control object.
///   This parameter must not be NULL.
/// </param>
/// <returns>
///   VCI_OK if succeeded, VCI_E_INVALIDARG otherwise.
/// </returns>
//*****************************************************************************
HRESULT CanControl2::InitNew(::ICanControl2* pCanCtl)
{
  HRESULT hResult;

  Cleanup();

  if (nullptr != pCanCtl)
  {
    m_pCanCtl = pCanCtl;
    m_pCanCtl->AddRef();
    hResult = VCI_OK;
  }
  else
  {
    hResult = VCI_E_INVALIDARG;
  }

  return( hResult );
}

//*****************************************************************************
/// <summary>
///   This method performs tasks associated with freeing, releasing, or
///   resetting unmanaged resources.
/// </summary>
//*****************************************************************************
void CanControl2::Cleanup(void)
{
  if (nullptr != m_pCanCtl)
  {
    m_pCanCtl->Release();
    m_pCanCtl = nullptr;
  }
}

//*****************************************************************************
/// <summary>
///   This method detects the actual bit rate of the CAN line to which the
///   controller is connected.
/// </summary>
/// <param name="operatingMode">
///   Operating mode of the CAN controller 
/// </param>
/// <param name="extendedMode">
///   Extended operating mode of the CAN controller 
/// </param>
/// <param name="timeout">
///   Timeout in milliseconds to wait between two successive receive messages.
/// </param>
/// <param name="bitrateTable">
///   One-dimensional array of initialized CanBitrate objects
///   which contains possible values for the bit timing register
///   to be tested.
/// </param>
/// <returns>
///   If the method succeeds it returns the index of the detected CanBitrate
///   entry within the specified array.
///   If the method detects no baud rate it returns -1.
/// </returns>
/// <remarks>
///   The method detects the actual bit rate beginning at the first entry
///   within the specified array and switches to the next entry until the
///   correct baud rate is detected or the table limit is reached. If the
///   time between two successive receive messages exceed the value specified
///   by the <paramref name="timeout"/> parameter, the method throws a 
///   <c>VciException</c>.
///   The total execution time of the method can be determined by the
///   following formula:
///   TotalExecutionTime [ms] = <paramref name="timeout"/> * <paramref name="bitrateTable"/>.Length
/// </remarks>
/// <exception cref="VciException">
///   VCI_E_TIMEOUT: Time between two successive receive messages exceed the value specified
///   by the <paramref name="timeout"/> parameter.
///   otherwise: see error message
/// </exception>
/// <exception cref="ObjectDisposedException">
///   Object is already disposed.
/// </exception>
//*****************************************************************************
int CanControl2::DetectBaud( CanOperatingModes         operatingMode
                           , CanExtendedOperatingModes extendedMode
                           , UInt16                    timeout
                           , array<CanFdBitrate>^     bitrateTable)
{
  HRESULT     hResult = VCI_E_INVALIDARG;
  CANBTPTABLE sBtpTab;
  int         iLength;
  int         iLowIdx;
  int         iResult = -1;

  if (nullptr == m_pCanCtl)
  {
    throw gcnew ObjectDisposedException(this->GetType()->FullName);
  }

  if (nullptr != bitrateTable)
  {
    iLength = bitrateTable->GetLength(0);
    iLowIdx = bitrateTable->GetLowerBound(0);
  }
  else
  {
    iLength = 0;
    iLowIdx = 0;
  }

  while (iLength > 0)
  {
    if (iLength <= CAN_BTP_TABEL_SIZE)
      sBtpTab.bCount = (UINT8) iLength;
    else
      sBtpTab.bCount = CAN_BTP_TABEL_SIZE;

    sBtpTab.bIndex = 0xFF;

    for (UINT8 i = 0; i < sBtpTab.bCount; i++)
    {
      sBtpTab.asBTP[i].sSdr.dwMode = (UINT32) bitrateTable[iLowIdx+i].StdBitrate.Mode;
      sBtpTab.asBTP[i].sSdr.dwBPS = bitrateTable[iLowIdx+i].StdBitrate.Prescaler;
      sBtpTab.asBTP[i].sSdr.wTS1 = bitrateTable[iLowIdx+i].StdBitrate.TimeSegment1;
      sBtpTab.asBTP[i].sSdr.wTS2 = bitrateTable[iLowIdx+i].StdBitrate.TimeSegment2;
      sBtpTab.asBTP[i].sSdr.wSJW = bitrateTable[iLowIdx+i].StdBitrate.Sjw;
      sBtpTab.asBTP[i].sSdr.wTDO = bitrateTable[iLowIdx+i].StdBitrate.TransmitterDelay;

      sBtpTab.asBTP[i].sFdr.dwMode = (UINT32) bitrateTable[iLowIdx+i].FastBitrate.Mode;
      sBtpTab.asBTP[i].sFdr.dwBPS = bitrateTable[iLowIdx+i].FastBitrate.Prescaler;
      sBtpTab.asBTP[i].sFdr.wTS1 = bitrateTable[iLowIdx+i].FastBitrate.TimeSegment1;
      sBtpTab.asBTP[i].sFdr.wTS2 = bitrateTable[iLowIdx+i].FastBitrate.TimeSegment2;
      sBtpTab.asBTP[i].sFdr.wSJW = bitrateTable[iLowIdx+i].FastBitrate.Sjw;
      sBtpTab.asBTP[i].sFdr.wTDO = bitrateTable[iLowIdx+i].FastBitrate.TransmitterDelay;
    }

    iLength -= sBtpTab.bCount;
    iLowIdx += sBtpTab.bCount;

    hResult = m_pCanCtl->DetectBaud((UINT8) operatingMode, (UINT8) extendedMode, timeout, &sBtpTab);

    if (hResult == VCI_OK)
    {
      iResult = sBtpTab.bIndex;
      break;
    }
  }

  if (hResult != VCI_OK)
  {
    throw gcnew VciException(VciServerImpl::Instance(), hResult);
  }

  return( iResult );
}

//*****************************************************************************
/// <summary>
///   This method initialize the CAN line in the specified operating mode
///   and bit transfer rate. The method also performs a reset of the CAN
///   controller hardware and disables the reception of CAN messages.  
/// </summary>
/// <param name="operatingMode">
///   Operating mode of the CAN controller 
/// </param>
/// <param name="extendedMode">
///   Extended operating mode of the CAN controller 
/// </param>
/// <param name="filterModeStd">
///   filter mode for standard Ids
/// </param>
/// <param name="cntIdsStd">
///   size of standard filter in Ids
/// </param>
/// <param name="filterModeExt">
///   filter mode for extended Ids
/// </param>
/// <param name="cntIdsExt">
///   size of extended filter in Ids
/// </param>
/// <param name="bitrate">
///   Bit timing value for the arbitration phase
/// </param>
/// <param name="extendedBitrate">
///   Bit timing value for the data phase
/// </param>
/// <remarks>
///   The <paramref name="operatingMode"/> parameter defines the operating mode 
///   of the CAN controller. The operating mode can be a combination of the 
///   following primary operating mode flags:
///   <list type="bullet">
///
///     <item>
///       <description><c>CanOperatingModes.Standard</c> - standard frame format (11 bit identifier)</description>
///     </item>
///     <item>
///       <description><c>CanOperatingModes.Extended</c> - extended frame format (29 bit identifier)</description>
///     </item>
///   </list>
///   
///   Optionally, the following flags can be combined with the primary operating
///   mode flags:
///   
///   <list type="bullet">
///     <item>
///       <description><c>CanOperatingModes.ListOnly</c> - listen only mode</description>
///     </item>
///     <item>
///       <description><c>CanOperatingModes.ErrFrame</c> - accept error frames</description>
///     </item>
///     <item>
///       <description><c>CanOperatingModes.LowSpeed</c> - use low speed bus interface</description>
///     </item>
///   </list>
///
///   The same procedure applies for the <paramref name="extendedMode"/> parameter. However instead
///   of using <c>CanOperatingModes</c> you need to use <c>CanExtendedOperatingModes</c>
///   
/// </remarks>
/// <exception cref="VciException">
///   CAN line initialization failed.
/// </exception>
/// <exception cref="ObjectDisposedException">
///   Object is already disposed.
/// </exception>
//*****************************************************************************
void CanControl2::InitLine( CanOperatingModes operatingMode
                          , CanExtendedOperatingModes extendedMode
                          , CanFilterModes filterModeStd 
                          , UInt32 cntIdsStd
                          , CanFilterModes filterModeExt
                          , UInt32 cntIdsExt
                          , CanBitrate2 bitrate
                          , CanBitrate2 extendedBitrate)
{
  HRESULT     hResult;
  CANINITLINE2 InitPara;

  if (nullptr != m_pCanCtl)
  {
    InitPara.bOpMode   = (UINT8) operatingMode;
    InitPara.bExMode   = (UINT8) extendedMode;

    InitPara.bSFMode   = (UINT8) filterModeStd;
    InitPara.dwSFIds   = cntIdsStd;
    InitPara.bEFMode   = (UINT8) filterModeExt;
    InitPara.dwEFIds   = cntIdsExt;
    
    InitPara.sBtpSdr.dwMode = (UInt32)bitrate.Mode;
    InitPara.sBtpSdr.dwBPS = bitrate.Prescaler;
    InitPara.sBtpSdr.wTS1 = bitrate.TimeSegment1;
    InitPara.sBtpSdr.wTS2 = bitrate.TimeSegment2;
    InitPara.sBtpSdr.wSJW = bitrate.Sjw;
    InitPara.sBtpSdr.wTDO = bitrate.TransmitterDelay;
    
    InitPara.sBtpFdr.dwMode = (UInt32)extendedBitrate.Mode;
    InitPara.sBtpFdr.dwBPS = extendedBitrate.Prescaler;
    InitPara.sBtpFdr.wTS1 = extendedBitrate.TimeSegment1;
    InitPara.sBtpFdr.wTS2 = extendedBitrate.TimeSegment2;
    InitPara.sBtpFdr.wSJW = extendedBitrate.Sjw;
    InitPara.sBtpFdr.wTDO = extendedBitrate.TransmitterDelay;    

    hResult = m_pCanCtl->InitLine(&InitPara);
    if (hResult != VCI_OK)
    {
      StringBuilder^ builder = gcnew StringBuilder();

      builder->AppendFormat("\nInitPara = {{ bOpMode={0}, bExMode={1}, bSFMode={2}, bEFMode={3}, dwSFIds={4}, dwEFIds={5},", 
        InitPara.bOpMode, InitPara.bExMode, InitPara.bSFMode, InitPara.bEFMode, InitPara.dwSFIds, InitPara.dwEFIds);

      builder->AppendFormat("\n  sBtpSdr = {{ dwMode={0}, dwBPS={1}, wTS1={2}, wTS2={3}, wSJW={4}, wTDO={5} }},", 
        InitPara.sBtpSdr.dwMode, InitPara.sBtpSdr.dwBPS, InitPara.sBtpSdr.wTS1, InitPara.sBtpSdr.wTS2, InitPara.sBtpSdr.wSJW, InitPara.sBtpSdr.wTDO);
        
      builder->AppendFormat("\n  sBtpFdr = {{ dwMode={0}, dwBPS={1}, wTS1={2}, wTS2={3}, wSJW={4}, wTDO={5} }}\n}}", 
        InitPara.sBtpFdr.dwMode, InitPara.sBtpFdr.dwBPS, InitPara.sBtpFdr.wTS1, InitPara.sBtpFdr.wTS2, InitPara.sBtpFdr.wSJW, InitPara.sBtpFdr.wTDO);

      throw gcnew VciException(VciServerImpl::Instance(), builder->ToString(), hResult);
    }
  }
  else
  {
    throw gcnew ObjectDisposedException(this->GetType()->FullName);
  }
}

//*****************************************************************************
/// <summary>
///   This method reset the CAN line to it's initial state. The method
///   aborts a currently busy transmit message and switch the CAN controller
///   into init mode. The method additionally clears the standard and
///   extended mode ID filter. 
/// </summary>
/// <exception cref="VciException">
///   Resetting CAN line failed.  
/// </exception>
/// <exception cref="ObjectDisposedException">
///   Object is already disposed. 
/// </exception>
//*****************************************************************************
void CanControl2::ResetLine(void)
{
  HRESULT hResult;

  if (nullptr != m_pCanCtl)
  {
    hResult = m_pCanCtl->ResetLine();
    if (hResult != VCI_OK)
    {
      throw gcnew VciException(VciServerImpl::Instance(), hResult);
    }
  }
  else
  {
    throw gcnew ObjectDisposedException(this->GetType()->FullName);
  }
}

//*****************************************************************************
/// <summary>
///   This method starts the CAN line and switch it into running mode.
///   After starting the CAN line, CAN messages can be transmitted over
///   the message channel.  
/// </summary>
/// <exception cref="VciException">
///   Starting CAN line failed.  
/// </exception>
/// <exception cref="ObjectDisposedException">
///   Object is already disposed. 
/// </exception>
//*****************************************************************************
void CanControl2::StartLine(void)
{
  HRESULT hResult;

  if (nullptr != m_pCanCtl)
  {
    hResult = m_pCanCtl->StartLine();
    if (hResult != VCI_OK)
    {
      throw gcnew VciException(VciServerImpl::Instance(), hResult);
    }
  }
  else
  {
    throw gcnew ObjectDisposedException(this->GetType()->FullName);
  }
}

//*****************************************************************************
/// <summary>
///   This method stops the CAN line an switch it into init mode. After
///   stopping the CAN controller no further CAN messages are transmitted
///   over the message channel. Other than <c>ResetLine</c>, this method does
///   not abort a currently busy transmit message and does not clear the
///   standard and extended mode ID filter.
/// </summary>
/// <exception cref="VciException">
///   Stopping CAN line failed.
/// </exception>
/// <exception cref="ObjectDisposedException">
///   Object is already disposed.  
/// </exception>
//*****************************************************************************
void CanControl2::StopLine(void)
{
  HRESULT hResult;

  if (nullptr != m_pCanCtl)
  {
    hResult = m_pCanCtl->StopLine();
    if (hResult != VCI_OK)
    {
      throw gcnew VciException(VciServerImpl::Instance(), hResult);
    }
  }
  else
  {
    throw gcnew ObjectDisposedException(this->GetType()->FullName);
  }
}

//*****************************************************************************
/// <summary>
///   This method sets the global acceptance filter. The global acceptance
///   filter enables the reception of CAN message identifiers specified by
///   the bit patterns passed in <paramref name="code"/> and 
///   <paramref name="mask"/>. The message IDs enabled by this method are 
///   always accepted, even if the specified IDs are not registered within 
///   the filter list (see also <c>AddFilterIds</c>). The method can only be 
///    called if the CAN controller is in 'init' mode.  
/// </summary>
/// <param name="select">
///   Filter selection. This parameter can be either <c>CanFilter::Std</c>
///   to select the 11-bit acceptance filter, or <c>CanFilter::Ext</c> to
///   select the 29-bit acceptance filter.
/// </param>
/// <param name="code">
///   Acceptance code inclusive RTR bit. 
/// </param>
/// <param name="mask">
///   Mask that specifies the relevant bits within <paramref name="code"/>. 
///   Relevant bits are specified by a 1 in the corresponding bit position,
///   non relevant bits are 0. 
/// </param>
/// <remarks>
///   The acceptance filter is defined by the acceptance code and acceptance 
///   mask. The bit pattern of CANIDs to be received are defined by the 
///   acceptance code. The corresponding acceptance mask allow to define 
///   certain bit positions to be don't care (bit x = 0). The values in 
///   <paramref name="code"/> and <paramref name="mask"/> have the following 
///   format:
///   <code>
///   select = CanFilter::Std
///   
///        +----+----+----+----+ ~ +----+----+ ~ +---+---+---+---+
///    bit | 31 | 30 | 29 | 28 |   | 13 | 12 |   | 3 | 2 | 1 | 0 |
///        +----+----+----+----+ ~ +----+----+ ~ +---+---+---+---+
///        |  0 |  0 |  0 |  0 |   |  0 |ID11|   |ID2|ID1|ID0|RTR|
///        +----+----+----+----+ ~ +----+----+ ~ +---+---+---+---+
///   
///   select = CanFilter::Ext
///   
///        +----+----+----+----+ ~ +----+----+ ~ +---+---+---+---+
///    bit | 31 | 30 | 29 | 28 |   | 13 | 12 |   | 3 | 2 | 1 | 0 |
///        +----+----+----+----+ ~ +----+----+ ~ +---+---+---+---+
///        |  0 |  0 |ID28|ID27|   |ID12|ID11|   |ID2|ID1|ID0|RTR|
///        +----+----+----+----+ ~ +----+----+ ~ +---+---+---+---+
///   </code>
/// </remarks>
/// <example>
///   The following example demonstates how to compute the 
///   <paramref name="code"/> and <paramref name="mask"/> values to enable 
///   the standard IDs in the range from 0x100 to 0x103 whereas RTR is 0.
///   <code>
///    code   = 001 0001 1000 0
///    mask   = 111 1111 1100 1
///    result = 001 0001 10xx 0
///   
///    enabled IDs:
///             001 0000 0000 0 (0x100, RTR = 0)
///             001 0000 0001 0 (0x101, RTR = 0)
///             001 0000 0010 0 (0x102, RTR = 0)
///             001 0000 0011 0 (0x103, RTR = 0)
///   </code>
/// </example>
/// <exception cref="VciException">
///   Setting acceptance filter failed.
/// </exception>
/// <exception cref="ObjectDisposedException">
///   Object is already disposed.  
/// </exception>
//*****************************************************************************
void CanControl2::SetAccFilter( CanFilter  select
                             , UInt32     code
                             , UInt32     mask)
{
  HRESULT hResult;

  if (nullptr != m_pCanCtl)
  {
    hResult = m_pCanCtl->SetAccFilter((UINT8) select, code, mask);
    if (hResult != VCI_OK)
    {
      throw gcnew VciException(VciServerImpl::Instance(), hResult);
    }
  }
  else
  {
    throw gcnew ObjectDisposedException(this->GetType()->FullName);
  }
}

//*****************************************************************************
/// <summary>
///   This method registers the specified CAN message identifier or group
///   of identifiers at the specified filter list. IDs registered within the
///   filter list are accepted for reception. The method can only be called 
///   if the CAN controller is in 'init' mode.
/// </summary>
/// <param name="select">
///   Filter selection. This parameter can be either <c>CanFilter::Std</c>
///   to select the 11-bit filter list, or <c>CanFilter::Ext</c> to
///   select the 29-bit filter list.
/// </param>
/// <param name="code">
///   Message identifier (inclusive RTR) to add to the filter list.  
/// </param>
/// <param name="mask">
///   Mask that specifies the relevant bits within <paramref name="code"/>. 
///   Relevant bits are specified by a 1 in the corresponding bit position,
///   non relevant bits are 0. 
/// </param>
/// <example>
///   The following example demonstates how to compute the 
///   <paramref name="code"/> and <paramref name="mask"/> values to register 
///   the standard IDs in the range from 0x518 to 0x51B whereas RTR is 1.
///   <code>
///     code   = 0101 0001 1000 1
///     mask   = 0111 1111 1100 1
///     result = 0101 0001 10xx 1
/// 
///     IDs registered by this method:
///              0101 0001 1000 1 (0x518, RTR = 1)
///              0101 0001 1001 1 (0x519, RTR = 1)
///              0101 0001 1010 1 (0x51A, RTR = 1)
///              0101 0001 1011 1 (0x51B, RTR = 1)
///   </code>
/// </example>
/// <exception cref="VciException">
///   Registering filter Ids failed.  
/// </exception>
/// <exception cref="ObjectDisposedException">
///   Object is already disposed.  
/// </exception>
//*****************************************************************************
void CanControl2::AddFilterIds( CanFilter  select
                             , UInt32     code
                             , UInt32     mask)
{
  HRESULT hResult;

  if (nullptr != m_pCanCtl)
  {
    hResult = m_pCanCtl->AddFilterIds((UINT8) select, code, mask);
    if (hResult != VCI_OK)
    {
      throw gcnew VciException(VciServerImpl::Instance(), hResult);
    }
  }
  else
  {
    throw gcnew ObjectDisposedException(this->GetType()->FullName);
  }
}

//*****************************************************************************
/// <summary>
///   This method removes the specified CAN message identifier or group
///   of identifiers from the specified filter list. The method can only be
///   called if the CAN controller is in 'init' mode.
/// </summary>
/// <param name="select">
///   Filter selection. This parameter can be either <c>CanFilter::Std</c>
///   to select the 11-bit filter list, or <c>CanFilter::Ext</c> to
///   select the 29-bit filter list.
/// </param>
/// <param name="code">
///   Message identifier (inclusive RTR) to remove from the filter list. 
/// </param>
/// <param name="mask">
///   Mask that specifies the relevant bits within <paramref name="code"/>. 
///   Relevant bits are specified by a 1 in the corresponding bit position,
///   non relevant bits are 0. 
/// </param>
/// <example>
///   The following example demonstates how to compute the 
///   <paramref name="code"/> and <paramref name="mask"/> values to remove 
///   the standard IDs in the range from 0x518 to 0x51B whereas RTR is 1.
///   <code>
///     code   = 0101 0001 1000 1
///     mask   = 0111 1111 1100 1
///     result = 0101 0001 10xx 1
/// 
///     IDs removed by this method:
///              0101 0001 1000 1 (0x518, RTR = 1)
///              0101 0001 1001 1 (0x519, RTR = 1)
///              0101 0001 1010 1 (0x51A, RTR = 1)
///              0101 0001 1011 1 (0x51B, RTR = 1)
///   </code>
/// </example>
/// <exception cref="VciException">
///   Removing filter Ids failed.  
/// </exception>
/// <exception cref="ObjectDisposedException">
///   Object is already disposed.  
/// </exception>
//*****************************************************************************
void CanControl2::RemFilterIds( CanFilter  select
                             , UInt32     code
                             , UInt32     mask)
{
  HRESULT hResult;

  if (nullptr != m_pCanCtl)
  {
    hResult = m_pCanCtl->RemFilterIds((UINT8) select, code, mask);
    if (hResult != VCI_OK)
    {
      throw gcnew VciException(VciServerImpl::Instance(), hResult);
    }
  }
  else
  {
    throw gcnew ObjectDisposedException(this->GetType()->FullName);
  }
}
