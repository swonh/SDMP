// Copyright (c) 2021-24, Sungwon Hong. All Rights Reserved. 
// This Source Code Form is subject to the terms of the Mozilla Public License, Version 2.0. 
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.

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
