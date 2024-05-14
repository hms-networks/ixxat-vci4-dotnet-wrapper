// SPDX-License-Identifier: MIT
//----------------------------------------------------------------------------
// Summary  : Declarations for the VCI server object.
// Copyright: Copyright (C) 2016-2022 HMS Technology Center Ravensburg GmbH, 
//            all rights reserved
//----------------------------------------------------------------------------

namespace Ixxat.Vci4
{
  using System;
  using System.IO;
  using System.Reflection;
  using System.Runtime.InteropServices;
  using Ixxat.Vci4.Bal.Can;
  using Ixxat.Vci4.Bal.Lin;


  //*****************************************************************************
  /// <summary>
  ///   This class represents the entry point for working with the VCI. 
  ///   Use <c>Instance</c> to get access to the VCI server singleton.
  /// </summary>
  //*****************************************************************************
  public class VciServer
  {
    //--------------------------------------------------------------------
    // member variables
    //--------------------------------------------------------------------

    // The singleton VciServer instance
    private static IVciServer? ms_instance = null;


    private const int PROCESSOR_ARCHITECTURE_AMD64 = 9;
    private const int PROCESSOR_ARCHITECTURE_INTEL = 0;
    private const int PROCESSOR_ARCHITECTURE_ARM = 5;
    private const int PROCESSOR_ARCHITECTURE_ARM64 = 12;

    [DllImport("kernel32")]
    private static extern void GetSystemInfo(ref SYSTEM_INFO lpSystemInfo);

    [DllImport("kernel32")]
    private static extern void GetNativeSystemInfo(ref SYSTEM_INFO lpSystemInfo);

    [StructLayout(LayoutKind.Sequential)]
    private struct SYSTEM_INFO
    {
      public short wProcessorArchitecture;
      public short wReserved;
      public int dwPageSize;
      public IntPtr lpMinimumApplicationAddress;
      public IntPtr lpMaximumApplicationAddress;
      public IntPtr dwActiveProcessorMask;
      public int dwNumberOfProcessors;
      public int dwProcessorType;
      public int dwAllocationGranularity;
      public short wProcessorLevel;
      public short wProcessorRevision;
    }

    /// <summary>
    /// Arch enumeration
    /// </summary>
    private enum Architecture
    {
      Unknown,
      x86,
      x64,
      arm32,
      arm64,
    }

    /// <summary>
    /// Turn content of system info into arch enumeration
    /// </summary>
    /// <param name="si">system info struct</param>
    /// <returns>arch enumeration</returns>
    /// <exception cref="PlatformNotSupportedException"></exception>
    private static Architecture GetArchitecture(ref SYSTEM_INFO si)
    {
      switch (si.wProcessorArchitecture)
      {
        case PROCESSOR_ARCHITECTURE_AMD64:
          return Architecture.x64;

        case PROCESSOR_ARCHITECTURE_ARM64:
          return Architecture.arm64;

        case PROCESSOR_ARCHITECTURE_ARM:
          return Architecture.arm32;

        case PROCESSOR_ARCHITECTURE_INTEL:
          return Architecture.x86;

        default:
          throw new PlatformNotSupportedException("Unsupported architecture: Only x86, x64, arm and arm64 supported.");
      }
    }

    /// <summary>
    /// return arch of current process
    /// </summary>
    private static Architecture ProcessArchitecture
    {
      get
      {
        var si = new SYSTEM_INFO();
        GetSystemInfo(ref si);
        return GetArchitecture(ref si);
      }
    }

    /// <summary>
    /// return arch of current machine
    /// </summary>
    private static Architecture MachineArchitecture
    {
      get
      {
        var si = new SYSTEM_INFO();
        GetNativeSystemInfo(ref si);
        return GetArchitecture(ref si);
      }
    }

    //*****************************************************************************
    /// <summary>
    ///   Dynamically loads the platform dependent (x86, x64) server component.
    /// 
    ///   You could either specify
    ///     - no path or relative path
    ///         -> try to load relative to EXE and 
    ///            second try relative to Loader DLL
    ///     - absolute path
    ///         -> try to load from given path
    /// 
    /// </summary>
    /// <exception cref="InvalidOperationException">
    ///   Loading server component failed.
    /// </exception>
    //*****************************************************************************
    static private void LoadServer(string assemblyloadpath) 
    {
      Architecture arch = ProcessArchitecture;

      string archstr = "unknown";
      switch (arch)
      {
        case Architecture.x86: archstr = "x86"; break;
        case Architecture.x64: archstr = "x64"; break;
        case Architecture.arm32: archstr = "arm"; break;
        case Architecture.arm64: archstr = "arm64"; break;
      }

      string assemblyName = "vcinet\\" + archstr + "\\vcinet." + archstr + ".dll";

      string archSpecificPath;

      // Either
      //   - no assemblyloadpath is specified or 
      //   - assemblyloadpath is relative to the exe file
      // then try to load
      //   - first relative to EXE
      //   - second relative to loader DLL
      // If an absolute assemblyloadpath is specified
      //   - try to load from there
      if (string.IsNullOrEmpty(assemblyloadpath) || !Path.IsPathRooted(assemblyloadpath))
      {
        Assembly? entryassembly = Assembly.GetEntryAssembly();
        if (entryassembly == null)
          throw new InvalidOperationException("GetEntryAssembly did not return a valid assembly reference");

        string? localToLoaderPath = Path.GetDirectoryName(new System.Uri(Assembly.GetCallingAssembly().Location).LocalPath);
        string? localToExePath = Path.GetDirectoryName(new System.Uri(entryassembly.Location).LocalPath);

        // check if file exists local to exe
        string archSpecificPath_1 = archSpecificPath = Path.Combine(localToExePath ?? "", assemblyloadpath, assemblyName);
        if (!File.Exists(archSpecificPath_1))
        {
          // second try local to loader
          string archSpecificPath_2 = archSpecificPath = Path.Combine(localToLoaderPath ?? "", assemblyloadpath, assemblyName);
          if (!File.Exists(archSpecificPath_2))
          {
            throw new InvalidOperationException(
              String.Format("Did not find platform specific assembly ({0}) in {1} or {2}. Check your installation.",
                assemblyName, archSpecificPath_1, archSpecificPath_2));
          }
        }
      }
      else
      {
        // Absolute assemblyloadpath is given here
        // do not use vcinet subdirectory here
        assemblyName = archstr + "\\vcinet." + archstr + ".dll";

        // check if file exists on the user specified path
        archSpecificPath = Path.Combine(assemblyloadpath, assemblyName);
        if (!File.Exists(archSpecificPath))
        {
          throw new InvalidOperationException(
            String.Format("Did not find platform specific assembly ({0}) in {1}. Check your installation.",
              assemblyName, assemblyloadpath));
        }
      }

      Assembly assembly = System.Reflection.Assembly.LoadFile(archSpecificPath);
      Type? servimpl = assembly.GetType("Ixxat.Vci4.VciServerImpl");
      if (servimpl == null)
        throw new InvalidOperationException("Native component error: type 'Ixxat.Vci4.VciServerImpl' not found");

      MethodInfo? instance = servimpl.GetMethod("Instance", BindingFlags.Public | BindingFlags.Static);
      if (instance == null)
        throw new InvalidOperationException("Native component error: function 'Ixxat.Vci4.VciServerImpl:Instance' not found.");

      ms_instance = (IVciServer?)instance.Invoke(null, null);
      if (ms_instance == null)
        throw new InvalidOperationException("Native component error: function 'Ixxat.Vci4.VciServerImpl:Instance' did not return a valid instance.");
    }

    //*****************************************************************************
    /// <summary>
    ///   Returns the VCI server singleton.
    /// </summary>
    /// <remarks>
    ///   Path to mixed assemblies is automatically determined from the 
    ///   EXE or the Loader assembly.
    /// </remarks>
    /// <exception cref="InvalidOperationException">
    ///   Loading server component failed.
    /// </exception>
    //*****************************************************************************
    public static IVciServer? Instance()
    {
      return Instance("");
    }

    //*****************************************************************************
    /// <summary>
    ///   Returns the VCI server singleton.
    /// </summary>
    /// <param name="assemblyloadpath">
    ///   specify path where to load the necessary mixed assemblies from 
    ///   (vcinet.x64.dll, vcinet.x86.dll)
    /// </param>
    /// <exception cref="InvalidOperationException">
    ///   Loading server component failed.
    /// </exception>
    //*****************************************************************************
    public static IVciServer? Instance(string assemblyloadpath)
    {
      if (ms_instance == null)
      {
        LoadServer(assemblyloadpath);
      }
      return ms_instance;
    }
  };


}

