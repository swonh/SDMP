// Copyright (c) 2021-25, Sungwon Hong. All Rights Reserved. 
// This Source Code Form is subject to the terms of the Mozilla Public License, Version 2.0. 
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.

using Nodez.Data.DataModel;
using Nodez.Data.Managers;
using SDMP.General.CRP.MyObjects;
using SDMP.General.CRP.MyOutputs;
using Nodez.Sdmp.General.Controls;
using Nodez.Sdmp.General.DataModel;
using Nodez.Sdmp.General.Managers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SDMP.General.CRP.Controls
{
    public class UserEventControl : EventControl
    {
        private static readonly Lazy<UserEventControl> lazy = new Lazy<UserEventControl>(() => new UserEventControl());

        public static new UserEventControl Instance { get { return lazy.Value; } }


        public override void OnBeginSolve()
        {

        }

        public override void OnDoneDataLoad()
        {

        }

        public override void OnVisitState(State state)
        {

        }

        public override void OnVisitToState(State fromState, State toState)
        {

        }

        public override void OnStageChanged(Stage stage)
        {

        }

        public override void OnDoneSolve()
        {
            OutputManager outputManager = OutputManager.Instance;

            OutputTable resultsTable = new OutputTable();

            SolutionManager solutionManager = SolutionManager.Instance;
            Solution bestSol = solutionManager.BestSolution;

            IOrderedEnumerable<KeyValuePair<int, State>> states = bestSol.States.OrderBy(x => x.Key);

            int seq = 1;
            foreach (KeyValuePair<int, State> item in states)
            {
                CRPState state = item.Value as CRPState;

                if (state.IsInitial)
                    continue;

                Result row = new Result();

                row.SEQUENCE = seq;
                row.CONVEYOR = state.CurrentConveyor.ConveyorNum;
                row.JOB = state.LastRetrievedJob.Number;
                row.COLOR = state.LastRetrievedJob.Color.ColorNumber;

                resultsTable.AddRow(row);

                seq++;
            }

            outputManager.SetOutput(resultsTable.Name, resultsTable);
            resultsTable.WriteToFile();
        }
    }
}
