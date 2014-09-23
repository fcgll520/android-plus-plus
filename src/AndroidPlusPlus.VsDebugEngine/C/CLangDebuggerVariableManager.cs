﻿////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

  public class CLangDebuggerVariableManager
  {

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    private readonly CLangDebugger m_debugger;

    private Dictionary<string, MiVariable> m_trackedVariables;

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public CLangDebuggerVariableManager (CLangDebugger debugger)
    {
      m_debugger = debugger;

      m_trackedVariables = new Dictionary<string, MiVariable> ();
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public CLangDebuggeeProperty CreatePropertyFromVariable (CLangDebuggeeStackFrame stackFrame, MiVariable variable)
    {
      LoggingUtils.PrintFunction ();

      return new CLangDebuggeeProperty (m_debugger, stackFrame, variable);
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public MiVariable CreateVariableFromExpression (CLangDebuggeeStackFrame stackFrame, string expression)
    {
      LoggingUtils.PrintFunction ();

      try
      {
        IDebugThread2 stackThread;

        uint stackThreadId;

        LoggingUtils.RequireOk (stackFrame.GetThread (out stackThread));

        LoggingUtils.RequireOk (stackThread.GetThreadId (out stackThreadId));

        //expression = expression.Replace (@"\", @"\\");

        //expression = expression.Replace ("->", ".");

        string command = string.Format ("-var-create --thread {0} --frame {1} - * \"{2}\"", stackThreadId, stackFrame.StackLevel, expression);

        MiResultRecord resultRecord = m_debugger.GdbClient.SendCommand (command);

        MiResultRecord.RequireOk (resultRecord, command);

        return new MiVariable (expression, resultRecord.Results);
      }
      catch (Exception e)
      {
        LoggingUtils.HandleException (e);

        return null;
      }
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public void CreateChildVariables (MiVariable parentVariable, int depth)
    {
      LoggingUtils.PrintFunction ();

      if ((depth > 0) && (parentVariable.HasChildren))
      {
        MiVariable [] evaluatedChildren = GetChildVariables (parentVariable, depth);

        foreach (MiVariable child in evaluatedChildren)
        {
          CreateChildVariables (child, depth - 1);

          parentVariable.AddChild (child);
        }
      }
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    private MiVariable [] GetChildVariables (MiVariable parentVariable, int depth)
    {
      LoggingUtils.PrintFunction ();

      List<MiVariable> childVariables = new List<MiVariable> ();

      if ((depth > 0) && (parentVariable.HasChildren))
      {
        string command = string.Format ("-var-list-children --all-values {0}", parentVariable.Name);

        MiResultRecord resultRecord = m_debugger.GdbClient.SendCommand (command);

        MiResultRecord.RequireOk (resultRecord, command);

        if (resultRecord.HasField ("children"))
        {
          List<MiResultValue> childrenList = resultRecord ["children"] [0] ["child"];

          for (int i = 0; i < childrenList.Count; ++i)
          {
            MiVariable childVariable = null;

            MiResultValueTuple childTuple = childrenList [i] as MiResultValueTuple;

            string variableName = childTuple ["name"] [0].GetString ();

            bool isPseudoChild = false;

            if (childTuple.HasField ("exp"))
            {
              string variableExpression = childTuple ["exp"] [0].GetString ();

              switch (variableExpression)
              {
                case "public":
                case "protected":
                case "private":
                {
                  isPseudoChild = true;

                  break;
                }
              }

              if (!string.IsNullOrEmpty (variableExpression))
              {
                childVariable = new MiVariable (variableName, variableExpression);

                childVariable.Populate (childTuple.Values);
              }
            }

            if (childVariable == null)
            {
              childVariable = new MiVariable (variableName, childTuple.Values);
            }

            if (isPseudoChild)
            {
              depth += 1; // need an additional level of children.

              MiVariable [] evaluatedChildren = GetChildVariables (childVariable, depth - 1);

              childVariables.AddRange (evaluatedChildren);
            }
            else
            {
              childVariables.Add (childVariable);
            }
          }
        }
      }

      return childVariables.ToArray ();
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public void UpdateVariable (MiVariable variable)
    {
      LoggingUtils.PrintFunction ();

      string command = string.Format ("-var-update --all-values {0}", variable.Name);

      MiResultRecord resultRecord = m_debugger.GdbClient.SendCommand (command);

      MiResultRecord.RequireOk (resultRecord, command);

      if (resultRecord.HasField ("changelist"))
      {
        variable.Populate (resultRecord ["changelist"]);
      }
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    private void DeleteGdbVariable (MiVariable gdbVariable)
    {
      LoggingUtils.PrintFunction ();

      string command = string.Format ("-var-delete {0}", gdbVariable.Name);

      MiResultRecord resultRecord = m_debugger.GdbClient.SendCommand (command);

      MiResultRecord.RequireOk (resultRecord, command);
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