﻿////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

namespace AndroidPlusPlus.Common
{
  ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
  ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
  ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

  public class AndroidDevice
  {

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    private Hashtable m_deviceProperties = new Hashtable ();

    private List<AndroidProcess> m_deviceProcesses = new List<AndroidProcess> ();

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public AndroidDevice (string deviceId)
    {
      ID = deviceId;
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public void Refresh ()
    {
      PopulateProperties ();

      PopulateProcesses ();
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public string GetProperty (string property)
    {
      lock (m_deviceProperties)
      {
        return (string)m_deviceProperties [property];
      }
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public AndroidProcess [] GetProcesses ()
    {
      lock (m_deviceProcesses)
      {
        return m_deviceProcesses.ToArray ();
      }
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public string Shell (string command, string arguments, int timeout = 1000)
    {
      using (SyncRedirectProcess adbShellCommand = AndroidAdb.AdbCommand (this, "shell", string.Format ("{0} {1}", command, arguments)))
      {
        adbShellCommand.StartAndWaitForExit (timeout);

        Trace.WriteLine ("[AndroidDevice] Shell: " + adbShellCommand.StandardOutput);

        return adbShellCommand.StandardOutput;
      }
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public bool Pull (string remotePath, string localPath)
    {
      using (SyncRedirectProcess adbPullCommand = AndroidAdb.AdbCommand (this, "pull", string.Format ("{0} {1}", StringUtils.ConvertPathWindowsToPosix (remotePath), StringUtils.ConvertPathWindowsToPosix (localPath))))
      {
        adbPullCommand.StartAndWaitForExit ();

        Trace.WriteLine ("[AndroidDevice] Pull: " + adbPullCommand.StandardOutput);

        if (adbPullCommand.StandardOutput.ToLower ().Contains ("not found"))
        {
          return false;
        }
      }

      return true;
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public bool Install (string filename, bool reinstall)
    {
      using (SyncRedirectProcess adbInstallCommand = AndroidAdb.AdbCommand (this, "install", ((reinstall) ? "-r " : "") + StringUtils.ConvertPathWindowsToPosix (filename)))
      {
        adbInstallCommand.StartAndWaitForExit ();

        Trace.WriteLine ("[AndroidDevice] Install: " + adbInstallCommand.StandardOutput);

        if (adbInstallCommand.StandardOutput.ToLower ().Contains ("success"))
        {
          return true;
        }
      }

      return false;
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public bool Uninstall (string package, bool keepCache)
    {
      using (SyncRedirectProcess adbUninstallCommand = AndroidAdb.AdbCommand (this, "install", ((keepCache) ? "-k " : "") + package))
      {
        adbUninstallCommand.StartAndWaitForExit ();

        Trace.WriteLine ("[AndroidDevice] Uninstall: " + adbUninstallCommand.StandardOutput);

        if (adbUninstallCommand.StandardOutput.ToLower ().Contains ("success"))
        {
          return true;
        }
      }

      return false;
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    private void PopulateProperties ()
    {
      string deviceGetProperties = Shell ("getprop", "");

      if (!String.IsNullOrEmpty (deviceGetProperties))
      {
        string [] getPropOutputLines = deviceGetProperties.Replace ("\r", "").Split (new char [] { '\n' });

        m_deviceProperties.Clear ();

        foreach (string line in getPropOutputLines)
        {
          if (!String.IsNullOrEmpty (line))
          {
            string [] segments = line.Split (new char [] { ' ' });

            string propName = segments [0].Trim (new char [] { '[', ']', ':' });

            string propValue = segments [1].Trim (new char [] { '[', ']', ':' });

            if ((!String.IsNullOrEmpty (propName)) && (!String.IsNullOrEmpty (propValue)))
            {
              m_deviceProperties.Add (propName, propValue);
            }
          }
        }
      }
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    private void PopulateProcesses ()
    {
      // 
      // Skip the first line, and read in tab-seperated process data.
      // 

      string deviceProcessList = Shell ("ps", "");

      if (!String.IsNullOrEmpty (deviceProcessList))
      {
        string [] processesOutputLines = deviceProcessList.Replace ("\r", "").Split (new char [] { '\n' });

        string processesRegExPattern = @"(?<user>[^ ]+)[ ]*(?<pid>[0-9]+)[ ]*(?<ppid>[0-9]+)[ ]*(?<vsize>[0-9]+)[ ]*(?<rss>[0-9]+)[ ]*(?<wchan>[A-Za-z0-9]+)[ ]*(?<pc>[A-Za-z0-9]+)[ ]*(?<s>[^ ]+)[ ]*(?<name>[^\r\n]+)";

        Regex regExMatcher = new Regex (processesRegExPattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);

        m_deviceProcesses.Clear ();

        for (uint i = 1; i < processesOutputLines.Length; ++i)
        {
          if (!String.IsNullOrEmpty (processesOutputLines [i]))
          {
            Match regExLineMatches = regExMatcher.Match (processesOutputLines [i]);

            string processUser = regExLineMatches.Result ("${user}");

            uint processId = uint.Parse (regExLineMatches.Result ("${pid}"));

            uint processPid = uint.Parse (regExLineMatches.Result ("${ppid}"));

            uint processVsize = uint.Parse (regExLineMatches.Result ("${vsize}"));

            uint processRss = uint.Parse (regExLineMatches.Result ("${rss}"));

            uint processWchan = Convert.ToUInt32 (regExLineMatches.Result ("${wchan}"), 16);

            uint processPc = Convert.ToUInt32 (regExLineMatches.Result ("${pc}"), 16);

            string processPcS = regExLineMatches.Result ("${s}");

            string processName = regExLineMatches.Result ("${name}");

            if ((!String.IsNullOrEmpty (processName)) && (!String.IsNullOrEmpty (processUser)))
            {
              AndroidProcess process = new AndroidProcess (this, processName, processId, processUser);

              m_deviceProcesses.Add (process);
            }
          }
        }
      }
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public string ID { get; protected set; }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public AndroidSettings.VersionCode SdkVersion { get; protected set; }

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
