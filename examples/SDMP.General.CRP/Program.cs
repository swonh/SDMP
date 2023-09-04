using Nodez.Data;
using Nodez.Data.Controls;
using Nodez.Data.DataModel;
using Nodez.Data.Interface;
using Nodez.Data.Managers;
using SDMP.General.CRP.Controls;
using SDMP.General.CRP.MyInputs;
using SDMP.General.CRP.MyMethods;
using SDMP.General.CRP.MyObjects;
using Nodez.Sdmp.General.Solver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Nodez.Sdmp.Constants;

namespace SDMP.General.CRP
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
            UserMachineLearningControl mlControl = UserMachineLearningControl.Instance;

            List<object> controls = new List<object>() { boundControl, stateControl, solverControl, actionControl, transitionControl, dataControl, eventControl, logControl, approxControl, mlControl };

            List<string> tableNames = inputsControl.GetInputFileNames();
            inputsManager.LoadInputs(tableNames);

            List<RunConfig> runConfigs = inputsManager.GetInput(Constants.RUN_CONFIG).Rows().Cast<RunConfig>().ToList();

            runConfigs = runConfigs.OrderBy(x => x.RUN_SEQ).ToList();

            foreach (RunConfig config in runConfigs)
            {
                if (config.SOLVER_TYPE == "DP")
                {
                    GeneralSolver dpSolver = new GeneralSolver(config);
                    dpSolver.Initialize(controls);
                    dpSolver.Run();
                }
            }
        }
    }

}
