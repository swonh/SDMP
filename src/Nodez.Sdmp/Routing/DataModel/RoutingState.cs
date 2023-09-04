// Copyright (c) 2021-23, Sungwon Hong. All Rights Reserved. 
// This Source Code Form is subject to the terms of the Mozilla Public License, Version 2.0. 
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.

using Nodez.Sdmp.General.DataModel;
using Nodez.Sdmp.Routing.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nodez.Sdmp.Routing.DataModel
{
    public class RoutingState : State
    {
        public Dictionary<int, VehicleStateInfo> VehicleStateInfos { get; set; }

        public int[] ActiveCustomerFlag { get; set; }

        public int ActiveCount { get; set; }

        public int CurrentVehicleIndex { get; set; }

        public int CurrentCustomerIndex { get; set; }

        public RoutingState(string key)
        {
            this.Key = key;
        }

        public RoutingState()
        {
            this.PrevStates = new Dictionary<string, State>();
            this.PrevBestStates = new Dictionary<string, State>();

            this.VehicleStateInfos = new Dictionary<int, VehicleStateInfo>();
        }

        public void Initialize() 
        {
            this.InitVehicleStateInfos();
            this.InitActiveCustomers();
        }

        public void InitVehicleStateInfos() 
        {
            RoutingDataManager manager = RoutingDataManager.Instance;
            int customerCount = manager.RoutingProblem.Customers.Count;

            Dictionary<int, VehicleStateInfo> infos = new Dictionary<int, VehicleStateInfo>();
            foreach (Vehicle vehicle in manager.RoutingProblem.Vehicles) 
            {
                VehicleStateInfo info = new VehicleStateInfo();
                info.CurrentCustomerIndex = manager.RoutingProblem.Depot.Index;
                info.VisitedCustomerFlag = new int[customerCount + 1];
                info.NextVistableCustomerFlag = new int[customerCount + 1];
                info.RemainCapacity = new double[vehicle.Resources.Count + 1];
                info.IsActive = true;

                for (int i = 1; i < customerCount + 1; i++) 
                {
                    info.NextVistableCustomerFlag[i] = 1;
                }

                info.VisitedCustomerCount = 0;
                info.VistableCustomerCount = customerCount;

                foreach (Resource res in vehicle.Resources.Values)
                {
                    info.RemainCapacity[res.Index] = res.OrgCapacity;
                }

                infos.Add(vehicle.Index, info);
            }

            this.VehicleStateInfos = infos;
        }

        public void InitActiveCustomers() 
        {
            RoutingDataManager manager = RoutingDataManager.Instance;
            int customerCount = manager.RoutingProblem.Customers.Count;

            this.ActiveCustomerFlag = new int[customerCount + 1];

            for (int i = 1; i < customerCount + 1; i++)
            {
                this.ActiveCustomerFlag[i] = 1;
            }

            this.ActiveCount = customerCount;
        }

        public void CopyStateInfo(RoutingState state) 
        {
            Dictionary<int, VehicleStateInfo> infos = new Dictionary<int, VehicleStateInfo>();

            foreach (KeyValuePair<int, VehicleStateInfo> info in state.VehicleStateInfos) 
            {
                infos.Add(info.Key, info.Value.Clone());
            }

            this.VehicleStateInfos = infos;

            int[] active = state.ActiveCustomerFlag;
            this.ActiveCustomerFlag = new int[active.Length];
            this.ActiveCount = state.ActiveCount;
            this.CurrentVehicleIndex = state.CurrentVehicleIndex;
            this.CurrentCustomerIndex = state.CurrentCustomerIndex;

            Buffer.BlockCopy(active, 0, this.ActiveCustomerFlag, 0, active.Length * sizeof(int));
        }

        public bool IsCapacityAvailable(Customer nextCustomer, Vehicle vehicle, Resource resource)
        {
            int vehicleIndex = vehicle.Index;

            if (nextCustomer.Demand.Quantity > this.VehicleStateInfos[vehicleIndex].RemainCapacity[resource.Index])
                return false;

            return true;
        }

        public bool CheckTimeWindow(Customer nextCustomer, Vehicle vehicle) 
        {
            bool check = false;

            if (nextCustomer.TimeWindow.Item1 <= this.VehicleStateInfos[vehicle.Index].AvailableTime) 
            {
                check = true;
            }

            return check;
        }

        public void VisitCustomer(Customer nextCustomer, Vehicle vehicle, Resource resource)
        {
            int nextCustomerIndex = nextCustomer.Index;
            int vehicleIndex = vehicle.Index;
            int currentCustomerIndex = this.VehicleStateInfos[vehicleIndex].CurrentCustomerIndex;

            this.ActiveCustomerFlag[nextCustomerIndex] = 0;
            this.VehicleStateInfos[vehicleIndex].CurrentCustomerIndex = nextCustomerIndex;

            foreach (VehicleStateInfo info in this.VehicleStateInfos.Values)
            {
                info.NextVistableCustomerFlag[nextCustomerIndex] = 0;
                info.VistableCustomerCount--;
            }

            double time = RoutingDataManager.Instance.GetTime(currentCustomerIndex, nextCustomerIndex);

            this.VehicleStateInfos[vehicleIndex].VisitedCustomerFlag[nextCustomerIndex] = 1;
            this.VehicleStateInfos[vehicleIndex].VisitedCustomerCount++;
            this.VehicleStateInfos[vehicleIndex].AvailableTime += time;

            if (nextCustomer.IsDelivery)
            {
                this.VehicleStateInfos[vehicleIndex].RemainCapacity[resource.Index] += nextCustomer.Demand.Quantity;
            }
            else 
            {
                this.VehicleStateInfos[vehicleIndex].RemainCapacity[resource.Index] -= nextCustomer.Demand.Quantity;
            }

            this.CurrentVehicleIndex = vehicleIndex;
            this.CurrentCustomerIndex = nextCustomerIndex;
            

            this.ActiveCount--;
        }

        public void ReturnToDepot(int vehicleIndex)
        {
            Depot depot = RoutingDataManager.Instance.RoutingProblem.Depot;
            double time = RoutingDataManager.Instance.GetTime(this.VehicleStateInfos[vehicleIndex].CurrentCustomerIndex, depot.Index);

            this.VehicleStateInfos[vehicleIndex].AvailableTime += time;
            this.VehicleStateInfos[vehicleIndex].CurrentCustomerIndex = depot.Index;
            this.VehicleStateInfos[vehicleIndex].IsActive = false;

            this.CurrentCustomerIndex = 0;
            this.CurrentVehicleIndex = vehicleIndex;
        }

        public string GetKey()
        {
            StringBuilder str = new StringBuilder();

            str.AppendFormat("Customer:{0}:", this.CurrentCustomerIndex);
            str.AppendFormat("Vehicle{0}:", this.CurrentVehicleIndex);

            foreach (var item in this.VehicleStateInfos)
            {
                str.AppendFormat("Route{0}:{1}", item.Key, item.Value.ToString());
            }

            return str.ToString();
        }

        public RoutingState Clone() 
        {
            RoutingState clone = (RoutingState)this.MemberwiseClone();

            clone.CopyStateInfo(this);

            return clone;
        }

        public override string ToString() 
        {
            return this.Key;
        }

    }
}
