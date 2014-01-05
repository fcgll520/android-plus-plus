﻿////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell.Interop;

using AndroidPlusPlus.Common;

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

namespace AndroidPlusPlus.VsIntegratedPackage
{
  ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
  ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
  ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

  [ComVisible (true)]

  [Guid (Guids.guidAndroidPlusPlusProjectFactoryStringCLSID)]

  public class ProjectFactory : IVsProjectFactory
  {

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public ProjectFactory ()
    {
      Trace.WriteLine (string.Format ("Entering constructor for: ProjectFactory"));
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public int CanCreateProject (string pszFilename, uint grfCreateFlags, out int pfCanCreate)
    {
      // 
      // Determines whether a new project can be created.
      // 

      LoggingUtils.PrintFunction ();

      throw new NotImplementedException ();
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public int Close ()
    {
      // 
      // Shuts down the project factory.
      // 

      LoggingUtils.PrintFunction ();

      throw new NotImplementedException ();
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public int CreateProject (string pszFilename, string pszLocation, string pszName, uint grfCreateFlags, ref Guid iidProject, out IntPtr ppvProject, out int pfCanceled)
    {
      // 
      // Creates a new project.
      // 

      LoggingUtils.PrintFunction ();

      throw new NotImplementedException ();
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public int SetSite (Microsoft.VisualStudio.OLE.Interop.IServiceProvider psp)
    {
      // 
      // Initializes the project factory in the environment and allows the project to obtain services proffered by the environment.
      // 

      LoggingUtils.PrintFunction ();

      throw new NotImplementedException ();
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

  }

  ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
  ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
  ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

}

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
