// Copyright (c) 2021-23, Sungwon Hong. All Rights Reserved. 
// This Source Code Form is subject to the terms of the Mozilla Public License, Version 2.0. 
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.

using SDMP.General.CRP.MyObjects;
using Nodez.Sdmp.Constants;
using Nodez.Sdmp.General.Controls;
using Nodez.Sdmp.General.DataModel;
using Nodez.Sdmp.General.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDMP.General.CRP.Controls
{
    public class UserLogControl : LogControl
    {
        private static readonly Lazy<UserLogControl> lazy = new Lazy<UserLogControl>(() => new UserLogControl());

        public static new UserLogControl Instance { get { return lazy.Value; } }

        public override void WriteOptimalLog()
        {
            SolutionManager solutionManager = SolutionManager.Instance;
            Solution optSol = solutionManager.OptimalSolution;

            Console.WriteLine(Constants.LINE);
            Console.WriteLine(string.Format("Optimal Objective Value: {0}", optSol.Value));
            this.WriteSolution(optSol);
            Console.WriteLine(Constants.LINE);
        }

        public override void WriteBestSolutionLog()
        {
            SolutionManager solutionManager = SolutionManager.Instance;
            Solution bestSol = solutionManager.BestSolution;

            Console.WriteLine(Constants.LINE);
            Console.WriteLine(string.Format("Objective Value: {0}", bestSol.Value));
            this.WriteSolution(bestSol);
            Console.WriteLine(Constants.LINE);
        }

        public override void WriteSolution(Solution solution)
        {
            StringBuilder jobStr = new StringBuilder();
            StringBuilder colorStr = new StringBuilder();
            StringBuilder convStr = new StringBuilder();

            IOrderedEnumerable<KeyValuePair<int, State>> states = solution.States.OrderBy(x => x.Key);

            foreach (KeyValuePair<int, State> item in states)
            {
                CRPState state = item.Value as CRPState;

                if (state.IsInitial)
                    continue;

                jobStr.Append(state.LastRetrievedJob.Number);
                colorStr.Append(state.LastRetrievedJob.Color.ColorNumber);
                convStr.Append(state.CurrentConveyor.ConveyorNum);

                if (state.JobCount > 0)
                {
                    jobStr.Append("-");
                    colorStr.Append("-");
                    convStr.Append("-");
                }
            }

            Console.WriteLine(string.Format("Job Sequence: {0}", jobStr.ToString()));
            Console.WriteLine(string.Format("Color Sequence: {0}", colorStr.ToString()));
            Console.WriteLine(string.Format("Conveyor Sequence: {0}", convStr.ToString()));
        }

        public override void WritePruneLog(State state)
        {
            //Console.WriteLine("Prune => StateIndex:{0}, State:{1}, Stage:{2}, DualBound:{3}, BestValue:{4}, BestPrimalBound:{5}", state.Index, state.ToString(), state.Stage.Index, state.DualBound, state.BestValue, BoundManager.Instance.BestPrimalBound);
        }
    }
}
