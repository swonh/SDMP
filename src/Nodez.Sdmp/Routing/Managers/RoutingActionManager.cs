// Copyright (c) 2021-24, Sungwon Hong. All Rights Reserved. 
// This Source Code Form is subject to the terms of the Mozilla Public License, Version 2.0. 
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.

using Nodez.Sdmp.General.DataModel;
using Nodez.Sdmp.Routing.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nodez.Sdmp.Routing.Managers
{
    public class RoutingActionManager
    {
        private static readonly Lazy<RoutingActionManager> lazy = new Lazy<RoutingActionManager>(() => new RoutingActionManager());

        public static RoutingActionManager Instance { get { return lazy.Value; } }

        public List<General.DataModel.StateActionMap> GetStateActionMaps(RoutingState state) 
        {
            RoutingDataManager dataManager = RoutingDataManager.Instance;
            List<General.DataModel.StateActionMap> maps = new List<General.DataModel.StateActionMap>();

            foreach (KeyValuePair<int, VehicleStateInfo> stateInfo in state.VehicleStateInfos)
            {
                int vehicleIndex = stateInfo.Key;
                dataManager.RoutingProblem.VehicleIndexMappings.TryGetValue(vehicleIndex, out Vehicle vehicle);

                if (vehicle == null)
                    continue;

                VehicleStateInfo vehicleStateInfo = stateInfo.Value;

                if (vehicleStateInfo.IsDoneVisitNodes())
                    continue;

                for (int i = 0; i < vehicleStateInfo.NextVistableNodeFlag.Length; i++)
                {
                    int flag = vehicleStateInfo.NextVistableNodeFlag[i];

                    if (flag == 0)
                        continue;

                    General.DataModel.StateActionMap tran = new General.DataModel.StateActionMap();

                    RoutingState toState = new RoutingState();
                    toState.CopyStateInfo(state);

                    dataManager.RoutingProblem.NodeIndexMappings.TryGetValue(i, out Node node);

                    if (node == null)
                        continue;

                    Resource resource = vehicle.GetLoadableResource(node.Order.Product);

                    if (resource == null)
                        continue;

                    if (toState.IsCapacityAvailable(node, vehicle, resource) == false)
                        continue;

                    if (toState.CheckTimeWindow(node, vehicle) == false)
                        continue;

                    toState.VisitNode(node, vehicle, resource);

                    tran.PreActionState = state;
                    tran.PostActionState = toState;
                    tran.Cost = dataManager.GetDistance(vehicleStateInfo.CurrentNodeIndex, i);

                    maps.Add(tran);
                }
            }

            if (state.ActiveCount <= 0)
            {
                Depot depot = RoutingDataManager.Instance.RoutingProblem.Depot;
                foreach (KeyValuePair<int, VehicleStateInfo> stateInfo in state.VehicleStateInfos)
                {
                    int vehicleIndex = stateInfo.Key;
                    VehicleStateInfo vehicleStateInfo = stateInfo.Value;

                    if (vehicleStateInfo.IsActive == false)
                        continue;

                    General.DataModel.StateActionMap tran = new General.DataModel.StateActionMap();

                    RoutingState toState = new RoutingState();
                    toState.CopyStateInfo(state);

                    toState.ReturnToDepot(vehicleIndex);

                    tran.PreActionState = state;
                    tran.PostActionState = toState;
                    tran.Cost = dataManager.GetDistance(vehicleStateInfo.CurrentNodeIndex, depot.Index);

                    maps.Add(tran);
                }

                if (maps.Count == 1)
                    maps.FirstOrDefault().PostActionState.IsLastStage = true;
            }

            return maps;
        }
    }
}
