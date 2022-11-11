// SPDX-License-Identifier: MIT
//----------------------------------------------------------------------------
// Summary  : Declarations for VCIError class.
// Copyright: Copyright (C) 2016-2022 HMS Technology Center Ravensburg GmbH, 
//            all rights reserved
//----------------------------------------------------------------------------

namespace Ixxat.Vci4
{
  using System;

  //*****************************************************************************
  /// <summary>
  ///   VciError holds VCI error constants.
  /// </summary>
  //*****************************************************************************
  public class VciError
  {
    private const uint FACILITY_VCI   = 0x00010000;

    private const uint RESERVED_FLAG = 0x10000000;
    private const uint SEVERITY_MASK = 0xC0000000;
    private const uint CUSTOMER_FLAG = 0x20000000;

    private const uint SEVERITY_INFO = 0x40000000;
    private const uint SEVERITY_WARN = 0x80000000;
    private const uint SEVERITY_ERROR = 0xC0000000;

    private const uint SEV_VCI_ERROR = (SEVERITY_ERROR | CUSTOMER_FLAG | FACILITY_VCI);

    /// <summary>
    ///  The operation completed successfully.
    /// </summary>
    public const uint VCI_SUCCESS = 0;

    /// <summary>
    ///  The operation completed successfully.
    /// </summary>
    public const uint VCI_OK = VCI_SUCCESS;

    /// <summary>
    /// Unexpected failure
    /// </summary>
    public const uint VCI_E_UNEXPECTED = (SEV_VCI_ERROR | 0x0001);

    /// <summary>
    ///  Not implemented
    /// </summary>
    public const uint VCI_E_NOT_IMPLEMENTED = (SEV_VCI_ERROR | 0x0002);

    /// <summary>
    ///  Not enough storage is available to complete this operation.
    /// </summary>
    public const uint VCI_E_OUTOFMEMORY = (SEV_VCI_ERROR | 0x0003);

    /// <summary>
    ///  One or more parameters are invalid.
    /// </summary>
    public const uint VCI_E_INVALIDARG = (SEV_VCI_ERROR | 0x0004);

    /// <summary>
    ///  The object does not support the requested interface
    /// </summary>
    public const uint VCI_E_NOINTERFACE = (SEV_VCI_ERROR | 0x0005);

    /// <summary>
    ///  Invalid pointer
    /// </summary>
    public const uint VCI_E_INVPOINTER = (SEV_VCI_ERROR | 0x0006);

    /// <summary>
    ///  Invalid handle
    /// </summary>
    public const uint VCI_E_INVHANDLE = (SEV_VCI_ERROR | 0x0007);

    /// <summary>
    ///  Operation aborted
    /// </summary>
    public const uint VCI_E_ABORT = (SEV_VCI_ERROR | 0x0008);

    /// <summary>
    ///  Unspecified error
    /// </summary>
    public const uint VCI_E_FAIL = (SEV_VCI_ERROR | 0x0009);

    /// <summary>
    ///  Access is denied.
    /// </summary>
    public const uint VCI_E_ACCESSDENIED = (SEV_VCI_ERROR | 0x000A);

    /// <summary>
    ///  This operation returned because the timeout period expired.
    /// </summary>
    public const uint VCI_E_TIMEOUT = (SEV_VCI_ERROR | 0x000B);

    /// <summary>
    ///  The requested resource is currently busy.
    /// </summary>
    public const uint VCI_E_BUSY = (SEV_VCI_ERROR | 0x000C);

    /// <summary>
    ///  The data necessary to complete this operation is not yet available.
    /// </summary>
    public const uint VCI_E_PENDING = (SEV_VCI_ERROR | 0x000D);

    /// <summary>
    ///  No more data available.
    /// </summary>
    public const uint VCI_E_NO_DATA = (SEV_VCI_ERROR | 0x000E);

    /// <summary>
    ///  No more entries are available from an enumeration operation.
    /// </summary>
    public const uint VCI_E_NO_MORE_ITEMS = (SEV_VCI_ERROR | 0x000F);

    /// <summary>
    ///  The component is not initialized.
    /// </summary>
    public const uint VCI_E_NOT_INITIALIZED = (SEV_VCI_ERROR | 0x0010);

    /// <summary>
    ///  An attempt was made to reinitialize an already initialized component.
    /// </summary>
    public const uint VCI_E_ALREADY_INITIALIZED = (SEV_VCI_ERROR | 0x0011);

    /// <summary>
    ///  Receive queue empty.
    /// </summary>
    public const uint VCI_E_RXQUEUE_EMPTY = (SEV_VCI_ERROR | 0x0012);

    /// <summary>
    ///  Transmit queue full.
    /// </summary>
    public const uint VCI_E_TXQUEUE_FULL = (SEV_VCI_ERROR | 0x0013);

    /// <summary>
    ///  The data was too large to fit into the specified buffer.
    /// </summary>
    public const uint VCI_E_BUFFER_OVERFLOW = (SEV_VCI_ERROR | 0x0014);

    /// <summary>
    ///  The component is not in a valid state to perform this request.
    /// </summary>
    public const uint VCI_E_INVALID_STATE = (SEV_VCI_ERROR | 0x0015);

    /// <summary>
    ///  The object already exists.
    /// </summary>
    public const uint VCI_E_OBJECT_ALREADY_EXISTS = (SEV_VCI_ERROR | 0x0016);

    /// <summary>
    ///  An attempt was made to access an array outside of its bounds.
    /// </summary>
    public const uint VCI_E_INVALID_INDEX = (SEV_VCI_ERROR | 0x0017);

    /// <summary>
    ///  The end-of-file marker has been reached.
    ///  There is no valid data in the file beyond this marker.
    /// </summary>
    public const uint VCI_E_END_OF_FILE = (SEV_VCI_ERROR | 0x0018);

    /// <summary>
    /// Attempt to send a message to a disconnected communication port.
    /// </summary>
    public const uint VCI_E_DISCONNECTED = (SEV_VCI_ERROR | 0x0019);

    /// <summary>
    /// Invalid firmware version or version not supported.
    /// Check driver version and/or update firmware.
    /// </summary>
    public const uint VCI_E_INVALID_FIRMWARE = (SEV_VCI_ERROR | 0x001A);

    /// <summary>
    /// Invalid firmware version or version not supported.
    /// Check driver version and/or update firmware.
    /// </summary>
    public const uint VCI_E_WRONG_FLASHFWVERSION = VCI_E_INVALID_FIRMWARE;

    /// <summary>
    /// Invalid license.
    /// </summary>
    public const uint VCI_E_INVALID_LICENSE = (SEV_VCI_ERROR | 0x001B);

    /// <summary>
    /// There is no license available.
    /// </summary>
    public const uint VCI_E_NO_SUCH_LICENSE = (SEV_VCI_ERROR | 0x001C);

    /// <summary>
    /// The time limited license has expired.
    /// </summary>
    public const uint VCI_E_LICENSE_EXPIRED = (SEV_VCI_ERROR | 0x001D);

    /// <summary>
    /// The service request exceeds the license quota.
    /// </summary>
    public const uint VCI_E_LICENSE_QUOTA_EXCEEDED = (SEV_VCI_ERROR | 0x001E);

    /// <summary>
    /// Invalid bit timing parameter.
    /// </summary>
    public const uint VCI_E_INVALID_TIMING = (SEV_VCI_ERROR | 0x001F);

    /// <summary>
    /// The resource requested is already in use.
    /// </summary>
    public const uint VCI_E_IN_USE = (SEV_VCI_ERROR | 0x0020);

    /// <summary>
    ///  A device which does not exist was specified.
    /// </summary>
    public const uint VCI_E_NO_SUCH_DEVICE = (SEV_VCI_ERROR | 0x0021);

    /// <summary>
    ///  The device is not connected.
    /// </summary>
    public const uint VCI_E_DEVICE_NOT_CONNECTED = (SEV_VCI_ERROR | 0x0022);

    /// <summary>
    /// The device is not ready for use.
    /// </summary>
    public const uint VCI_E_DEVICE_NOT_READY = (SEV_VCI_ERROR | 0x0023);

    /// <summary>
    /// Mismatch between the type of object required by the operation
    /// and the type of object specified in the request.
    /// </summary>
    public const uint VCI_E_TYPE_MISMATCH = (SEV_VCI_ERROR | 0x0024);

    /// <summary>
    /// The request is not supported.
    /// </summary>
    public const uint VCI_E_NOT_SUPPORTED = (SEV_VCI_ERROR | 0x0025);

    /// <summary>
    /// The attempt to insert the object ID in the index failed
    /// because the object ID is already in the index.
    /// </summary>
    public const uint VCI_E_DUPLICATE_OBJECTID = (SEV_VCI_ERROR | 0x0026);

    /// <summary>
    /// The specified object ID was not found.
    /// </summary>
    public const uint VCI_E_OBJECTID_NOT_FOUND = (SEV_VCI_ERROR | 0x0027);

    /// <summary>
    /// The requested operation was called from a wrong execution level.
    /// </summary>
    public const uint VCI_E_WRONG_LEVEL = (SEV_VCI_ERROR | 0x0028);

    /// <summary>
    /// Incompatible version of the VCI device driver.
    /// </summary>
    public const uint VCI_E_WRONG_DRV_VERSION = (SEV_VCI_ERROR | 0x0029);

    /// <summary>
    /// Indicates there are no more LUIDs to allocate.
    /// </summary>
    public const uint VCI_E_LUIDS_EXHAUSTED = (SEV_VCI_ERROR | 0x002A);

  };
  
}
  
