/*****************************************************************************
 IXXAT Automation GmbH
******************************************************************************
 Summary : Declarations for the VCI server object.
 Compiler: Microsoft VC++ 8
******************************************************************************
 all rights reserved
*****************************************************************************/

/*************************************************************************
** namespace Ixxat.Vci4
*************************************************************************/
namespace Ixxat.Vci4 
{
  using System;
  using System.IO;
  using System.Reflection;
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
    private static IVciServer?           ms_instance = null;

    //*****************************************************************************
    /// <summary>
    ///   Dynamically loads the platform dependent (x86, x64) server component.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    ///   Loading server component failed.
    /// </exception>
    //*****************************************************************************
    static private void LoadServer() 
    {
      bool isRunningAs64Bit = (IntPtr.Size == 8);

      string assemblyName = "vcinet" + (isRunningAs64Bit ? ".x64" : ".x86") + ".dll";

      Assembly? entryassembly = Assembly.GetEntryAssembly();
      if (entryassembly == null)
        throw new InvalidOperationException("GetEntryAssembly did not return a valid assembly reference");

      string? localToLoaderPath = Path.GetDirectoryName(new System.Uri(Assembly.GetCallingAssembly().Location).LocalPath);
      string? localToExePath = Path.GetDirectoryName(new System.Uri(entryassembly.Location).LocalPath);

      // check if file exists local to exe
      string archSpecificPath = Path.Combine(localToExePath ?? "", assemblyName);
      if (!File.Exists(archSpecificPath))
      {
        // second try local to loader
        archSpecificPath = Path.Combine(localToLoaderPath ?? "", assemblyName);
        if (!File.Exists(archSpecificPath))
        {
          throw new InvalidOperationException(
            String.Format("Did not find platform specific assembly ({0}) in {1} or {2}. Check your installation.",
              assemblyName, localToExePath, localToLoaderPath));
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
    /// <exception cref="InvalidOperationException">
    ///   Loading server component failed.
    /// </exception>
    //*****************************************************************************
    public static IVciServer? Instance()
    {
      if (ms_instance == null)
      {
        LoadServer();
      }
      return ms_instance;
    }
  };


}

