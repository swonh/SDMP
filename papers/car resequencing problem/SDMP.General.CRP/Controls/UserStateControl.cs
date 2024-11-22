// Copyright (c) 2021-23, Sungwon Hong. All Rights Reserved. 
// This Source Code Form is subject to the terms of the Mozilla Public License, Version 2.0. 
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.

using Nodez.Data;
using Nodez.Data.Managers;
using SDMP.General.CRP.MyMethods;
using SDMP.General.CRP.MyObjects;
using Nodez.Sdmp.Enum;
using Nodez.Sdmp.General.Controls;
using Nodez.Sdmp.General.DataModel;
using Nodez.Sdmp.General.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SDMP.General.CRP.Controls
{
    public class UserStateControl : StateControl
    {
        private static readonly Lazy<UserStateControl> lazy = new Lazy<UserStateControl>(() => new UserStateControl());

        public static new UserStateControl Instance { get { return lazy.Value; } }

        public override State GetInitialState()
        {
            CRPData data = DataManager.Instance.Data as CRPData;
            CRPState initState = new CRPState();

            initState.SetStateInfo(data.CRPFactory.Conveyors);

            return initState;
        }

        public override string GetKey(State state)
        {
            CRPState crpState = state as CRPState;

            Dictionary<int, CRPConveyor> stateInfo = crpState.StateInfo;

            int convNum = 0;
            if (crpState.CurrentConveyor != null)
                convNum = crpState.CurrentConveyor.ConveyorNum;

            List<int> list = new List<int>();
            foreach (KeyValuePair<int, CRPConveyor> item in stateInfo)
            {
                list.Add(item.Value.JobCount);
            }

            list.Add(convNum);

            int[] arr = list.ToArray();

            string key = DataHelper.CreateKey(arr);

            return key;

        }

        public override Solution GetFeasibleSolution(State state)
        {
            CRPState crpState = state as CRPState;
            CRPState copiedState = crpState.Clone();
            copiedState.IsInitial = false;

            List<CRPState> states = new List<CRPState>();
            states.Add(crpState);
            states.AddRange(copiedState.GetBestStatesBackward().Cast<CRPState>().ToList());

            while (copiedState.JobCount > 0)
            {
                Stage stage = new Stage(copiedState.Stage.Index + 1);
                copiedState.Stage = stage;

                CRPJob lastJob = copiedState.LastRetrievedJob;

                CRPConveyor sameColorConv = copiedState.GetSameColorConveyor(lastJob);

                CRPJob retrievedJob = null;
                if (sameColorConv != null)
                {
                    retrievedJob = copiedState.RetrieveJob(sameColorConv.ConveyorNum);
                }
                else
                {
                    retrievedJob = copiedState.RetrieveJob();
                }

                double cost = 0;
                if (lastJob != null && lastJob.Color.ColorNumber != retrievedJob.Color.ColorNumber)
                    cost = 1;

                copiedState.BestValue += cost;

                states.Add(copiedState);
                copiedState = copiedState.Clone();
            }

            Solution feasibleSol = new Solution(states);

            return feasibleSol;
        }

        public override bool CanPruneByOptimality(State state, ObjectiveFunctionType objFuncType, double pruneTolerance)
        {
            BoundManager boundManager = BoundManager.Instance;

            double bestPrimalBound = boundManager.BestPrimalBound;
            double dualBound = state.DualBound;
            double bestValue = state.BestValue;

            double rootDualBound = boundManager.RootDualBound;

            if (objFuncType == ObjectiveFunctionType.Minimize)
            {
                if (dualBound < rootDualBound - bestValue)
                    dualBound = rootDualBound - bestValue;

                if (bestPrimalBound + pruneTolerance <= bestValue + dualBound)
                    return true;
                else
                    return false;
            }
            else
            {
                if (dualBound > rootDualBound - bestValue)
                    dualBound = rootDualBound - bestValue;

                if (bestPrimalBound >= bestValue + dualBound + pruneTolerance)
                    return true;
                else
                    return false;
            }
        }
    }

}
