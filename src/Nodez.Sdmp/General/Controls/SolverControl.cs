// Copyright (c) 2023 Sungwon Hong. All Rights Reserved. 
// Licenced under the Mozilla Public License, Version 2.0.

using Nodez.Sdmp.Constants;
using Nodez.Sdmp.Enum;
using Nodez.Sdmp.General.Managers;
using Nodez.Sdmp.Interfaces;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Nodez.Sdmp.General.Controls
{
    public class SolverControl
    {
        private static readonly Lazy<SolverControl> lazy = new Lazy<SolverControl>(() => new SolverControl());

        public static SolverControl Instance
        {
            get
            {
                if (ControlManager.Instance.RegisteredControls.TryGetValue(ControlType.SolverControl.ToString(), out object control))
                {
                    return (SolverControl)control;
                }
                else
                {
                    return lazy.Value;
                }
            }
        }

        public virtual int GetRunMaxTime() 
        {
            return Int32.MaxValue;
        }


        public virtual ObjectiveFunctionType GetObjectiveFuntionType(IRunConfig runConfig)
        {
            ObjectiveFunctionType objectiveFunctionType = UtilityHelper.StringToEnum(runConfig.OBJECTIVE_FUNCTION_TYPE, ObjectiveFunctionType.Minimize);

            return objectiveFunctionType;
        }

        public virtual string GetProjectName()
        {
            return Assembly.GetExecutingAssembly().GetName().Name;
        }

        public virtual string GetOutputDirectoryPath(string solverName) 
        {
            return null;
        }

    }
}
