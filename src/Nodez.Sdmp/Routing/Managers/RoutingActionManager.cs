// Copyright (c) 2023 Sungwon Hong. All Rights Reserved. 
// Licenced under the Mozilla Public License, Version 2.0.

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

                if (vehicleStateInfo.IsDoneVisitCustomers())
                    continue;

                for (int i = 0; i < vehicleStateInfo.NextVistableCustomerFlag.Length; i++)
                {
                    int flag = vehicleStateInfo.NextVistableCustomerFlag[i];

                    if (flag == 0)
                        continue;

                    General.DataModel.StateActionMap tran = new General.DataModel.StateActionMap();

                    RoutingState toState = new RoutingState();
                    toState.CopyStateInfo(state);

                    dataManager.RoutingProblem.CustomerIndexMappings.TryGetValue(i, out Customer customer);

                    if (customer == null)
                        continue;

                    Resource resource = vehicle.GetLoadableResource(customer.Demand.Product);

                    if (resource == null)
                        continue;

                    if (toState.IsCapacityAvailable(customer, vehicle, resource) == false)
                        continue;

                    if (toState.CheckTimeWindow(customer, vehicle) == false)
                        continue;

                    toState.VisitCustomer(customer, vehicle, resource);

                    tran.PreActionState = state;
                    tran.PostActionState = toState;
                    tran.Cost = dataManager.GetDistance(vehicleStateInfo.CurrentCustomerIndex, i);

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
                    tran.Cost = dataManager.GetDistance(vehicleStateInfo.CurrentCustomerIndex, depot.Index);

                    maps.Add(tran);
                }

                if (maps.Count == 1)
                    maps.FirstOrDefault().PostActionState.IsLastStage = true;
            }

            return maps;
        }
    }
}
