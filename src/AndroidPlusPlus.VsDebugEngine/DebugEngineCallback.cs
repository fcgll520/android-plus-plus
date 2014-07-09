﻿////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Debugger.Interop;
using AndroidPlusPlus.Common;

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

namespace AndroidPlusPlus.VsDebugEngine
{

  ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
  ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
  ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

  interface DebugEngineCallbackInterface
  {
    void OnAsyncBreakComplete (DebuggeeThread thread);
    void OnBreakpoint (DebuggeeThread thread, ReadOnlyCollection<object> clients, uint address);
    void OnBreakpointBound (DebuggeeBreakpointPending pendingBreakpoint, uint address);
    void OnError (int errorCode);
    void OnException (DebuggeeThread thread, uint code);
    void OnLoadComplete(DebuggeeThread thread);
    void OnModuleLoad (DebuggeeModule module);
    void OnModuleUnload (DebuggeeModule module);
    void OnOutputString (string outputString);
    void OnProcessExit (uint exitCode);
    void OnProgramDestroy (uint exitCode);
    void OnStepComplete (DebuggeeThread thread);
    void OnSymbolSearch (DebuggeeModule module, string status, uint dwStatsFlags);
    void OnThreadExit (DebuggeeThread thread, uint exitCode);
    void OnThreadStart (DebuggeeThread thread);
  }

  ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
  ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
  ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

  public class DebugEngineCallback : DebugEngineCallbackInterface, IDebugEventCallback2
  {

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    private readonly IDebugEventCallback2 m_ad7EventCallback;

    private readonly CLangDebuggerCallback m_cLangEventCallback;

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public DebugEngineCallback (DebugEngine engine, IDebugEventCallback2 ad7EventCallback)
    {
      Engine = engine;

      m_ad7EventCallback = ad7EventCallback;

      m_cLangEventCallback = new CLangDebuggerCallback ();
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public DebugEngine Engine { get; protected set; }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    #region DebugEngineCallbackInterface Members

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public int Event (IDebugEngine2 pEngine, IDebugProcess2 pProcess, IDebugProgram2 pProgram, IDebugThread2 pThread, IDebugEvent2 pEvent, ref Guid riidEvent, uint dwAttrib)
    {
      LoggingUtils.Print ("[DebugEngineCallback] Event: " + riidEvent.ToString ());

      // 
      // 
      // 

      int handle = m_cLangEventCallback.Event (pEngine, pProcess, pProgram, pThread, pEvent, ref riidEvent, dwAttrib);

      /*if (handle != DebugEngineConstants.E_NOTIMPL)
      {
        return handle;
      }*/

      handle = m_ad7EventCallback.Event (pEngine, pProcess, pProgram, pThread, pEvent, ref riidEvent, dwAttrib);

      // 
      // (Managed Code) It is strongly advised that ReleaseComObject be invoked on the various interfaces that are passed to IDebugEventCallback2::Event.
      // 

      /*Marshal.ReleaseComObject (pEngine);

      Marshal.ReleaseComObject (pProcess);

      Marshal.ReleaseComObject (pProgram);

      Marshal.ReleaseComObject (pThread);

      Marshal.ReleaseComObject (pEvent);*/

      return handle;
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    #endregion

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    #region DebugEngineCallbackInterface Members

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public void OnAsyncBreakComplete (DebuggeeThread thread)
    {
      LoggingUtils.PrintFunction ();

      throw new NotImplementedException ();
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public void OnBreakpoint (DebuggeeThread thread, ReadOnlyCollection<object> clients, uint address)
    {
      LoggingUtils.PrintFunction ();

      throw new NotImplementedException ();
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public void OnBreakpointBound (DebuggeeBreakpointPending pendingBreakpoint, uint address)
    {
      LoggingUtils.PrintFunction ();

      throw new NotImplementedException ();
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public void OnError (int errorCode)
    {
      // 
      // IDebugErrorEvent2 is used to report error messages to the user when something goes wrong in the debug engine.
      // 

      LoggingUtils.PrintFunction ();

      throw new NotImplementedException ();
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public void OnException (DebuggeeThread thread, uint code)
    {
      LoggingUtils.PrintFunction ();

      throw new NotImplementedException ();
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public void OnLoadComplete (DebuggeeThread thread)
    {
      LoggingUtils.PrintFunction ();

      throw new NotImplementedException ();
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public void OnModuleLoad (DebuggeeModule module)
    {
      LoggingUtils.PrintFunction ();

      throw new NotImplementedException ();
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public void OnModuleUnload (DebuggeeModule module)
    {
      LoggingUtils.PrintFunction ();

      throw new NotImplementedException ();
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public void OnOutputString (string outputString)
    {
      LoggingUtils.PrintFunction ();

      throw new NotImplementedException ();
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public void OnProcessExit (uint exitCode)
    {
      LoggingUtils.PrintFunction ();

      throw new NotImplementedException ();
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public void OnProgramDestroy (uint exitCode)
    {
      LoggingUtils.PrintFunction ();

      throw new NotImplementedException ();
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public void OnStepComplete (DebuggeeThread thread)
    {
      LoggingUtils.PrintFunction ();

      throw new NotImplementedException ();
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public void OnSymbolSearch (DebuggeeModule module, string status, uint dwStatsFlags)
    {
      LoggingUtils.PrintFunction ();

      throw new NotImplementedException ();
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public void OnThreadExit (DebuggeeThread thread, uint exitCode)
    {
      LoggingUtils.PrintFunction ();

      throw new NotImplementedException ();
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public void OnThreadStart (DebuggeeThread thread)
    {
      LoggingUtils.PrintFunction ();

      throw new NotImplementedException ();
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    #endregion

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
