﻿using Nodez.Data;
using Nodez.Data.Controls;
using Nodez.Data.DataModel;
using Nodez.Data.Interface;
using Nodez.Data.Managers;
using $safeprojectname$.Controls;
using $safeprojectname$.MyInputs;
using Nodez.Sdmp.Constants;
using Nodez.Sdmp.General.Solver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace $safeprojectname$
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

            List<object> controls = new List<object>() { boundControl, stateControl, solverControl, actionControl, transitionControl, dataControl, eventControl, logControl, approxControl };

            List<string> tableNames = inputsControl.GetInputFileNames();
            inputsManager.LoadInputs(tableNames);

            InputTable configData = inputsManager.GetInput(Constants.RUN_CONFIG);

            if (configData == null)
                return;

            List<RunConfig> runConfigs = configData.Rows().Cast<RunConfig>().ToList();

            foreach (RunConfig config in runConfigs)
            {
                GeneralSolver dpSolver = new GeneralSolver(config);
                dpSolver.Initialize(controls);
                dpSolver.Run();
            }
        }
    }

}