// Copyright (c) 2021-24, Sungwon Hong. All Rights Reserved. 
// This Source Code Form is subject to the terms of the Mozilla Public License, Version 2.0. 
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.

using Nodez.Data;
using Nodez.Data.Managers;
using Nodez.Sdmp.Enum;
using Nodez.Sdmp.General.Controls;
using Nodez.Sdmp.General.DataModel;
using Nodez.Sdmp.General.Managers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nodez.Project.GeneralTemplate.Controls
{
    public class UserStateControl : StateControl
    {
        private static readonly Lazy<UserStateControl> lazy = new Lazy<UserStateControl>(() => new UserStateControl());

        public static new UserStateControl Instance { get { return lazy.Value; } }

        public override State GetInitialState()
        {
            return null;
        }

        public override string GetKey(State state)
        {
            return state.Index.ToString();

        }

        public override Solution GetFeasibleSolution(State state)
        {
            return null;
        }

        public override bool CanPruneByOptimality(State state, ObjectiveFunctionType objFuncType, double pruneTolerance)
        {
            BoundManager boundManager = BoundManager.Instance;

            double bestPrimalBound = boundManager.BestPrimalBound;
            double dualBound = state.DualBound;
            double bestValue = state.CurrentBestValue;

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
