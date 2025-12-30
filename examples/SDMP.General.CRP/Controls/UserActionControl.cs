// Copyright (c) 2021-25, Sungwon Hong. All Rights Reserved. 
// This Source Code Form is subject to the terms of the Mozilla Public License, Version 2.0. 
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.

using Nodez.Data.Managers;
using SDMP.General.CRP.MyMethods;
using SDMP.General.CRP.MyObjects;
using Nodez.Sdmp.General.Controls;
using Nodez.Sdmp.General.DataModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SDMP.General.CRP.Controls
{
    public class UserActionControl : ActionControl
    {
        private static readonly Lazy<UserActionControl> lazy = new Lazy<UserActionControl>(() => new UserActionControl());

        public static new UserActionControl Instance { get { return lazy.Value; } }

        public override List<StateActionMap> GetStateActionMaps(State state)
        {
            List<StateActionMap> maps = new List<StateActionMap>();
            CRPData data = DataManager.Instance.Data as CRPData;

            CRPState fromState = state as CRPState;
            Dictionary<int, CRPConveyor> stateInfo = fromState.StateInfo;

            CRPJob lastJob = fromState.LastRetrievedJob;

            for (int i = 1; i <= CRPParameter.CONV_NUM; i++)
            {
                CRPConveyor conv = stateInfo[i];

                if (conv.JobCount <= 0)
                {
                    continue;
                }

                CRPState toState = new CRPState();
                toState.SetStateInfo(stateInfo);
                CRPJob toJob = toState.RetrieveJob(i);

                StateActionMap map = new StateActionMap();
                map.PreActionState = fromState;
                map.PostActionState = toState;

                double cost = 0;

                if (lastJob != null && lastJob.Color.ColorNumber != toJob.Color.ColorNumber)
                    cost = 1;

                map.Cost = cost;

                if (toState.JobCount <= 0) 
                {
                    toState.IsLastStage = true;
                }

                maps.Add(map);
            }

            return maps;
        }
    }
}
