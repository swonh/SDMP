// Copyright (c) 2021-23, Sungwon Hong. All Rights Reserved. 
// This Source Code Form is subject to the terms of the Mozilla Public License, Version 2.0. 
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.

using Nodez.Data;
using Nodez.Data.Controls;
using Nodez.Data.DataModel;
using Nodez.Data.Interface;
using Nodez.Data.Managers;
using Nodez.Project.RoutingTemplate.Controls;
using Nodez.Project.RoutingTemplate.MyInputs;
using Nodez.Sdmp.Constants;
using Nodez.Sdmp.General.Solver;
using Nodez.Sdmp.Routing.Solver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Nodez.Project.RoutingTemplate
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
            UserCustomerControl customerControl = UserCustomerControl.Instance;

            List<object> controls = new List<object>() { boundControl, stateControl, solverControl, actionControl, transitionControl, dataControl, eventControl, logControl, approxControl };

            List<string> tableNames = inputsControl.GetInputFileNames();
            inputsManager.LoadInputs(tableNames);

            InputTable configData = inputsManager.GetInput(Constants.RUN_CONFIG);

            if (configData == null)
                return;

            List<RunConfig> runConfigs = configData.Rows().Cast<RunConfig>().ToList();

            foreach (RunConfig config in runConfigs)
            {
                RoutingSolver dpSolver = new RoutingSolver(config);
                dpSolver.Initialize(controls);
                dpSolver.Run();
            }
        }
    }

}
