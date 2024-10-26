﻿// Copyright (c) 2021-24, Sungwon Hong. All Rights Reserved. 
// This Source Code Form is subject to the terms of the Mozilla Public License, Version 2.0. 
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.

using Nodez.Data;
using Nodez.Data.Controls;
using Nodez.Data.DataModel;
using Nodez.Data.Interface;
using Nodez.Data.Managers;
using Nodez.Project.SchedulingTemplate.Controls;
using Nodez.Project.SchedulingTemplate.MyInputs;
using Nodez.Project.SchedulingTemplate.MyObjects;
using Nodez.Sdmp.Constants;
using Nodez.Sdmp.General.Solver;
using Nodez.Sdmp.Scheduling.Solver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Nodez.Project.SchedulingTemplate
{
    class Program
    {
        static void Main(string[] args)
        {
            InputManager inputsManager = InputManager.Instance;
            InputControl inputsControl = InputControl.Instance;

            UserBoundControl boundControl = UserBoundControl.Instance;
            UserStateControl stateControl = UserStateControl.Instance;
            UserSolverControl solverControl = UserSolverControl.Instance;
            UserActionControl actionControl = UserActionControl.Instance;
            UserStateTransitionControl transitionControl = UserStateTransitionControl.Instance;
            UserDataControl dataControl = UserDataControl.Instance;
            UserEventControl eventControl = UserEventControl.Instance;
            UserLogControl logControl = UserLogControl.Instance;
            UserApproximationControl approxControl = UserApproximationControl.Instance;
            UserSchedulingControl schedControl = UserSchedulingControl.Instance;
            UserMachineLearningControl mlControl = UserMachineLearningControl.Instance;

            List<object> controls = new List<object>() { boundControl, stateControl, solverControl, actionControl, transitionControl, dataControl, eventControl, logControl, approxControl, schedControl, mlControl };

            List<string> tableNames = inputsControl.GetInputFileNames();
            inputsManager.LoadInputs(tableNames);

            InputTable configData = inputsManager.GetInput(Constants.RUN_CONFIG);

            if (configData == null)
                return;

            List<RunConfig> runConfigs = configData.Rows().Cast<RunConfig>().ToList();

            foreach (RunConfig config in runConfigs)
            {
                SchedulingSolver dpSolver = new SchedulingSolver(config);
                dpSolver.Initialize(controls);
                dpSolver.Run();
            }
        }
    }

}
