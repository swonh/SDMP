// Copyright (c) 2021-25, Sungwon Hong. All Rights Reserved. 
// This Source Code Form is subject to the terms of the Mozilla Public License, Version 2.0. 
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.

using Nodez.Sdmp.Routing.Managers;
using System;

namespace Nodez.Sdmp.Routing.DataModel
{
    public class Node
    {
        public int Index { get; set; }

        public string ID { get; set; }

        public string Name { get; set; }

        public Order Order { get; set; }

        public double X_Coordinate { get; set; }

        public double Y_Coordinate { get; set; }

        public ValueTuple<double, double> TimeWindow { get; set; }

        public bool IsVisited { get; set; }

        public Vehicle VisitedVehicle { get; set; }

        public bool IsDelivery { get; set; }

        public bool IsDepot { get; set; }

        public Order CopyOrder(Node clone)
        {
            if (this.Order == null)
                return null;

            return clone.Order.Clone();
        }

        public Vehicle CopyVisitedVehicle(Node clone)
        {
            if (this.VisitedVehicle == null)
                return null;

            return clone.VisitedVehicle.Clone();
        }

        public void ReplaceOrder(Order order)
        {
            this.Order = order;
        }

        public void ReplaceVisitedVehicle(Vehicle visitedVehicle)
        {
            this.VisitedVehicle = visitedVehicle;
        }

        public double GetDistance(Node toNode)
        {
            RoutingDataManager manager = RoutingDataManager.Instance;

            ValueTuple<string, string> key = (this.ID, toNode.ID);
            if (manager.RoutingProblem.DistanceInfoMappings.TryGetValue(key, out DistanceInfo info))
                return info.Distance;

            return 0;
        }

        public Node Clone()
        {
            Node clone = (Node)this.MemberwiseClone();

            Order order = CopyOrder(clone);
            Vehicle visitedVehicle = CopyVisitedVehicle(clone);

            clone.ReplaceOrder(order);
            clone.ReplaceVisitedVehicle(visitedVehicle);

            return clone;
        }

        public override string ToString()
        {
            return this.ID;
        }

    }
}
