﻿// Copyright (c) 2021-24, Sungwon Hong. All Rights Reserved. 
// This Source Code Form is subject to the terms of the Mozilla Public License, Version 2.0. 
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.

using Nodez.Data;
using Nodez.Data.Managers;
using Nodez.Sdmp.Enum;
using Nodez.Sdmp.General.Controls;
using Nodez.Sdmp.General.DataModel;
using Nodez.Sdmp.General.Managers;
using Nodez.Sdmp.Routing.DataModel;
using Nodez.Sdmp.Routing.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nodez.Project.RoutingTemplate.Controls
{
    public class UserStateControl : StateControl
    {
        private static readonly Lazy<UserStateControl> lazy = new Lazy<UserStateControl>(() => new UserStateControl());

        public static new UserStateControl Instance { get { return lazy.Value; } }

        public override State GetInitialState()
        {
            // Default Logic
            RoutingState state = new RoutingState();
            state.Initialize();

            return state;
        }

        public override string GetKey(State state)
        {
            // Default Logic
            RoutingState routingState = state as RoutingState;
            string key = routingState.GetKey();

            return key;
        }

        public override Solution GetFeasibleSolution(State state)
        {
            // Default Logic
            RoutingDataManager manager = RoutingDataManager.Instance;

            RoutingState routingState = state as RoutingState;
            RoutingState copiedState = routingState.Clone() as RoutingState;
            copiedState.IsInitial = false;

            List<RoutingState> states = new List<RoutingState>();

            states.Add(routingState);
            states.AddRange(copiedState.GetBestStatesBackward().Cast<RoutingState>().ToList());

            while (copiedState.ActiveCount > 0)
            {
                Stage stage = new Stage(copiedState.Stage.Index + 1);
                copiedState.Stage = stage;

                foreach (KeyValuePair<int, VehicleStateInfo> item in copiedState.VehicleStateInfos)
                {
                    VehicleStateInfo info = item.Value;

                    if (info.IsDoneVisitNodes())
                        continue;

                    int vehicleIndex = item.Key;
                    RoutingDataManager.Instance.RoutingProblem.VehicleIndexMappings.TryGetValue(vehicleIndex, out Vehicle vehicle);

                    if (vehicle == null)
                        continue;

                    int currentNode = info.CurrentNodeIndex;

                    int minIdx = 0;
                    double minDist = Double.MaxValue;
                    for (int i = 1; i < info.NextVistableNodeFlag.Length; i++)
                    {
                        int flag = info.NextVistableNodeFlag[i];

                        if (flag == 0)
                            continue;

                        double dist = manager.GetDistance(currentNode, i);

                        if (minDist > dist)
                        {
                            minDist = dist;
                            minIdx = i;
                        }
                    }

                    RoutingDataManager.Instance.RoutingProblem.NodeIndexMappings.TryGetValue(minIdx, out Node node);

                    if (node.IsDepot)
                        continue;

                    Resource resource = vehicle.GetLoadableResource(node.Order.Product);

                    if (resource == null)
                        continue;

                    copiedState.VisitNode(node, vehicle, resource);

                    copiedState.CurrentBestValue += minDist;
                }

                states.Add(copiedState);
                copiedState = copiedState.Clone() as RoutingState;
            }

            foreach (KeyValuePair<int, VehicleStateInfo> item in copiedState.VehicleStateInfos)
            {
                VehicleStateInfo info = item.Value;

                if (info.IsActive == false)
                    continue;

                copiedState = copiedState.Clone() as RoutingState;
                copiedState.IsInitial = false;

                Stage stage = new Stage(copiedState.Stage.Index + 1);
                copiedState.Stage = stage;

                double dist = manager.GetDistance(info.CurrentNodeIndex, manager.RoutingProblem.Depot.Index);
                copiedState.ReturnToDepot(item.Key);

                copiedState.CurrentBestValue += dist;

                states.Add(copiedState);
            }

            Solution feasibleSol = new Solution(states);

            return feasibleSol;
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
