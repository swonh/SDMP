// Copyright (c) 2021-24, Sungwon Hong. All Rights Reserved. 
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

        public int[] ActiveNodeFlag { get; set; }

        public int ActiveCount { get; set; }

        public int CurrentVehicleIndex { get; set; }

        public int CurrentNodeIndex { get; set; }

        public RoutingState() : base()
        {
            this.VehicleStateInfos = new Dictionary<int, VehicleStateInfo>();
        }

        public void Initialize() 
        {
            this.InitVehicleStateInfos();
            this.InitActiveNodes();
        }

        public void InitVehicleStateInfos() 
        {
            RoutingDataManager manager = RoutingDataManager.Instance;
            int nodeCount = manager.RoutingProblem.Nodes.Count;

            Dictionary<int, VehicleStateInfo> infos = new Dictionary<int, VehicleStateInfo>();
            foreach (Vehicle vehicle in manager.RoutingProblem.Vehicles) 
            {
                VehicleStateInfo info = new VehicleStateInfo();
                info.CurrentNodeIndex = manager.RoutingProblem.Depot.Index;
                info.VisitedNodeFlag = new int[nodeCount];
                info.NextVistableNodeFlag = new int[nodeCount];
                info.RemainCapacity = new double[manager.RoutingProblem.Resources.Count + 1];
                info.IsActive = true;

                for (int i = 1; i < nodeCount; i++) 
                {
                    info.NextVistableNodeFlag[i] = 1;
                }

                info.VisitedNodeCount = 0;
                info.VistableNodeCount = nodeCount;

                foreach (Resource res in vehicle.Resources.Values)
                {
                    info.RemainCapacity[res.Index] = res.OrgCapacity;
                }

                infos.Add(vehicle.Index, info);
            }

            this.VehicleStateInfos = infos;
        }

        public void InitActiveNodes() 
        {
            RoutingDataManager manager = RoutingDataManager.Instance;
            int nodeCount = manager.RoutingProblem.Nodes.Count;

            this.ActiveNodeFlag = new int[nodeCount];

            for (int i = 1; i < nodeCount; i++)
            {
                this.ActiveNodeFlag[i] = 1;
            }

            // Except for Depot (-1)
            this.ActiveCount = nodeCount - 1;
        }

        public void CopyStateInfo(RoutingState state) 
        {
            Dictionary<int, VehicleStateInfo> infos = new Dictionary<int, VehicleStateInfo>();

            foreach (KeyValuePair<int, VehicleStateInfo> info in state.VehicleStateInfos) 
            {
                infos.Add(info.Key, info.Value.Clone());
            }

            this.VehicleStateInfos = infos;

            int[] active = state.ActiveNodeFlag;
            this.ActiveNodeFlag = new int[active.Length];
            this.ActiveCount = state.ActiveCount;
            this.CurrentVehicleIndex = state.CurrentVehicleIndex;
            this.CurrentNodeIndex = state.CurrentNodeIndex;

            Buffer.BlockCopy(active, 0, this.ActiveNodeFlag, 0, active.Length * sizeof(int));
        }

        public bool IsCapacityAvailable(Node nextNode, Vehicle vehicle, Resource resource)
        {
            int vehicleIndex = vehicle.Index;

            if (nextNode.Order.Quantity > this.VehicleStateInfos[vehicleIndex].RemainCapacity[resource.Index])
                return false;

            return true;
        }

        public bool CheckTimeWindow(Node nextNode, Vehicle vehicle) 
        {
            bool check = false;

            if (nextNode.TimeWindow.Item1 >= this.VehicleStateInfos[vehicle.Index].AvailableTime) 
            {
                check = true;
            }

            return check;
        }

        public void VisitNode(Node nextNode, Vehicle vehicle, Resource resource)
        {
            int nextNodeIndex = nextNode.Index;
            int vehicleIndex = vehicle.Index;
            int currentNodeIndex = this.VehicleStateInfos[vehicleIndex].CurrentNodeIndex;

            this.ActiveNodeFlag[nextNodeIndex] = 0;
            this.VehicleStateInfos[vehicleIndex].CurrentNodeIndex = nextNodeIndex;

            foreach (VehicleStateInfo info in this.VehicleStateInfos.Values)
            {
                info.NextVistableNodeFlag[nextNodeIndex] = 0;
                info.VistableNodeCount--;
            }

            double availableTime = this.VehicleStateInfos[vehicleIndex].AvailableTime;
            double transitTime = RoutingDataManager.Instance.GetTime(vehicle, currentNodeIndex, nextNodeIndex);
            double arrivalTime = availableTime + transitTime;

            this.VehicleStateInfos[vehicleIndex].VisitedNodeFlag[nextNodeIndex] = 1;
            this.VehicleStateInfos[vehicleIndex].VisitedNodeCount++;
            this.VehicleStateInfos[vehicleIndex].AvailableTime += Math.Max(nextNode.Order.ReadyTime, arrivalTime);

            if (nextNode.IsDelivery)
            {
                this.VehicleStateInfos[vehicleIndex].RemainCapacity[resource.Index] += nextNode.Order.Quantity;
                this.VehicleStateInfos[vehicleIndex].DeliveryCount++;
            }
            else 
            {
                this.VehicleStateInfos[vehicleIndex].RemainCapacity[resource.Index] -= nextNode.Order.Quantity;
                this.VehicleStateInfos[vehicleIndex].PickupCount++;
            }

            this.CurrentVehicleIndex = vehicleIndex;
            this.CurrentNodeIndex = nextNodeIndex;
            
            this.ActiveCount--;
        }

        public void ReturnToDepot(int vehicleIndex)
        {
            Depot depot = RoutingDataManager.Instance.RoutingProblem.Depot;
            Vehicle vehicle = RoutingDataManager.Instance.GetVehicle(vehicleIndex);

            double time = RoutingDataManager.Instance.GetTime(vehicle, this.VehicleStateInfos[vehicleIndex].CurrentNodeIndex, depot.Index);

            this.VehicleStateInfos[vehicleIndex].AvailableTime += time;
            this.VehicleStateInfos[vehicleIndex].CurrentNodeIndex = depot.Index;
            this.VehicleStateInfos[vehicleIndex].IsActive = false;

            this.CurrentNodeIndex = 0;
            this.CurrentVehicleIndex = vehicleIndex;
        }

        public string GetKey()
        {
            StringBuilder str = new StringBuilder();

            str.AppendFormat("Node:{0}:", this.CurrentNodeIndex);
            str.AppendFormat("Vehicle{0}:", this.CurrentVehicleIndex);

            foreach (var item in this.VehicleStateInfos)
            {
                str.AppendFormat("Route{0}:{1}", item.Key, item.Value.ToString());
            }

            return str.ToString();
        }

        public new RoutingState Clone() 
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
