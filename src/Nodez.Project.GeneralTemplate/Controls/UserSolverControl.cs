﻿// Copyright (c) 2021-24, Sungwon Hong. All Rights Reserved. 
// This Source Code Form is subject to the terms of the Mozilla Public License, Version 2.0. 
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.

using Nodez.Sdmp.Constants;
using Nodez.Sdmp.General.Controls;
using Nodez.Sdmp.Enum;
using System;
using System.Collections.Generic;
using System.Text;
using Nodez.Sdmp.Interfaces;
using Nodez.Sdmp;
using System.Reflection;
using Nodez.Sdmp.General.Managers;

namespace Nodez.Project.GeneralTemplate.Controls
{
    public class UserSolverControl : SolverControl
    {
        private static readonly Lazy<UserSolverControl> lazy = new Lazy<UserSolverControl>(() => new UserSolverControl());

        public static new UserSolverControl Instance { get { return lazy.Value; } }

        public override int GetRunMaxTime()
        {
            return Int32.MaxValue;
        }

        public override ObjectiveFunctionType GetObjectiveFuntionType(IRunConfig runConfig)
        {
            ObjectiveFunctionType objectiveFunctionType = UtilityHelper.StringToEnum(runConfig.OBJECTIVE_FUNCTION_TYPE, ObjectiveFunctionType.Minimize);

            return objectiveFunctionType;
        }

        public override string GetProjectName()
        {
            return Assembly.GetExecutingAssembly().GetName().Name;
        }

        public override string GetOutputDirectoryPath(string solverName)
        {
            string engineStartTime = SolverManager.Instance.GetEngineStartTime(solverName).ToString("yyyyMMdd_HHmmss");

            string dirName = string.Format("{0}", engineStartTime);

            string dirPath = string.Format(@"..\..\Output\{0}\", dirName);

            return dirPath;
        }
    }
}
