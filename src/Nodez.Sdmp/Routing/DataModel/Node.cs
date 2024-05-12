// Copyright (c) 2021-23, Sungwon Hong. All Rights Reserved. 
// This Source Code Form is subject to the terms of the Mozilla Public License, Version 2.0. 
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.

using Nodez.Sdmp.Routing.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nodez.Sdmp.Routing.DataModel
{
    public class Node
    {
        public int Index { get; set; }

        public string ID { get; set; }

        public string Name { get; set; }

        public Demand Demand { get; set; }

        public Tuple<double, double> TimeWindow { get; set; }

        public bool IsVisited { get; set; }

        public Vehicle VisitedVehicle { get; set; }

        public bool IsDelivery { get; set; }

        public bool IsDepot { get; set; }

        public Demand CopyDemand(Customer clone)
        {
            if (this.Demand == null)
                return null;

            return clone.Demand.Clone();
        }

        public Vehicle CopyVisitedVehicle(Customer clone)
        {
            if (this.VisitedVehicle == null)
                return null;

            return clone.VisitedVehicle.Clone();
        }

        public void ReplaceDemand(Demand demand) 
        {
            this.Demand = demand;
        }

        public void ReplaceVisitedVehicle(Vehicle visitedVehicle) 
        {
            this.VisitedVehicle = visitedVehicle;
        }

        public double GetDistance(Node toNode) 
        {
            RoutingDataManager manager = RoutingDataManager.Instance;

            Tuple<string, string> key = Tuple.Create(this.ID, toNode.ID);
            if (manager.RoutingProblem.DistanceInfoMappings.TryGetValue(key, out DistanceInfo info))
                return info.Distance;

            return 0;
        }

        public Node Clone()
        {
            Node clone = (Node)this.MemberwiseClone();

            Demand demand = CopyDemand(clone);
            Vehicle visitedVehicle = CopyVisitedVehicle(clone);

            clone.ReplaceDemand(demand);
            clone.ReplaceVisitedVehicle(visitedVehicle);

            return clone;
        }

        public override string ToString() 
        {
            return this.ID;
        }

    }
}
