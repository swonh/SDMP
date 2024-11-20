// Copyright (c) 2021-24, Sungwon Hong. All Rights Reserved. 
// This Source Code Form is subject to the terms of the Mozilla Public License, Version 2.0. 
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nodez.Sdmp.Routing.DataModel
{
    public class RoutingProblem
    {
        public List<Vehicle> Vehicles { get; private set; }

        public List<Resource> Resources { get; private set; }

        public List<Node> Nodes { get; private set; }

        public Depot Depot { get; private set; }

        public List<Order> Orders { get; private set; }

        public List<Product> Products { get; private set; }

        public List<DistanceInfo> DistanceInfos { get; private set; }

        public List<RunOption> RunOptions { get; private set; }

        public Dictionary<string, Vehicle> VehicleMappings { get; private set; }

        public Dictionary<int, Vehicle> VehicleIndexMappings { get; private set; }

        public Dictionary<string, Resource> ResourceMappings { get; private set; }

        public Dictionary<string, Node> NodeMappings { get; private set; }

        public Dictionary<int, Node> NodeIndexMappings { get; private set; }

        public Dictionary<string, Node> PickupNodeOrderIDMappings { get; private set; }

        public Dictionary<string, Node> DeliveryNodeOrderIDMappings { get; private set; }

        public Dictionary<string, Order> OrderMappings { get; private set; }

        public Dictionary<int, Order> OrderIndexMappings { get; private set; }

        public Dictionary<string, Product> ProductMappings { get; private set; }

        public Dictionary<ValueTuple<string, string>, DistanceInfo> DistanceInfoMappings { get; private set; }

        public Dictionary<ValueTuple<int, int>, DistanceInfo> DistanceInfoIndexMappings { get; private set; }

        public Dictionary<string, string> RunOptionMappings { get; private set; }

        public RoutingProblem() 
        {
            this.Vehicles = new List<Vehicle>();
            this.Resources = new List<Resource>();
            this.Nodes = new List<Node>();
            this.Depot = new Depot();
            this.Orders = new List<Order>();
            this.Products = new List<Product>();

            this.VehicleMappings = new Dictionary<string, Vehicle>();
            this.ResourceMappings = new Dictionary<string, Resource>();
            this.NodeMappings = new Dictionary<string, Node>();
            this.OrderMappings = new Dictionary<string, Order>();
            this.OrderIndexMappings = new Dictionary<int, Order>();
            this.PickupNodeOrderIDMappings = new Dictionary<string, Node>();
            this.DeliveryNodeOrderIDMappings = new Dictionary<string, Node>();
            this.ProductMappings = new Dictionary<string, Product>();
            this.DistanceInfoMappings = new Dictionary<ValueTuple<string, string>, DistanceInfo>();
            this.DistanceInfoIndexMappings = new Dictionary<ValueTuple<int, int>, DistanceInfo>();
            this.RunOptionMappings = new Dictionary<string, string>();

            this.NodeIndexMappings = new Dictionary<int, Node>();
            this.VehicleIndexMappings = new Dictionary<int, Vehicle>();
        }

        public void SetRunOptionObjects(List<RunOption> runOptions)
        {
            this.RunOptions = runOptions;
            this.SetRunOptionMappings();
        }

        public void SetVehicleObjects(List<Vehicle> vehicles) 
        {
            this.Vehicles = vehicles;
            this.SetVehicleMappings();
            this.SetVehicleIndexMappings();
        }

        public void SetResourceObjects(List<Resource> resources)
        {
            this.Resources = resources;
            this.SetResourceMappings();
        }

        public void SetNodeObjects(List<Node> nodes)
        {
            this.Nodes = nodes;
            this.SetNodeMappings();
            this.SetNodeIndexMappings();
            this.SetPickupNodeOrderIDMappings();
            this.SetDeliveryNodeOrderIDMappings();
        }

        public void SetDepotObjects(Depot depot)
        {
            this.Depot = depot;
        }

        public void SetOrderObjects(List<Order> orders)
        {
            this.Orders = orders;
            this.SetOrderMappings();
            this.SetOrderIndexMappings();
        }

        public void SetProductObjects(List<Product> products)
        {
            this.Products = products;
            this.SetProductMappings();
        }

        public void SetDistanceInfoObjects(List<DistanceInfo> distanceInfos)
        {
            this.DistanceInfos = distanceInfos;
            this.SetDistanceInfoMappings();
            this.SetDistanceInfoIndexMappings();
        }

        private void SetRunOptionMappings()
        {
            foreach (RunOption option in this.RunOptions)
            {
                if (this.RunOptionMappings.ContainsKey(option.OPTION_NAME))
                    continue;

                this.RunOptionMappings.Add(option.OPTION_NAME, option.OPTION_VALUE);
            }
        }

        private void SetDistanceInfoMappings() 
        {
            foreach (DistanceInfo info in this.DistanceInfos) 
            {
                ValueTuple<string, string> key = (info.FromNodeID, info.ToNodeID);

                if (this.DistanceInfoMappings.ContainsKey(key))
                    continue;

                this.DistanceInfoMappings.Add(key, info);
            }
        }

        private void SetDistanceInfoIndexMappings()
        {
            foreach (DistanceInfo info in this.DistanceInfos)
            {
                ValueTuple<int, int> key = (info.FromNodeIndex, info.ToNodeIndex);

                if (this.DistanceInfoIndexMappings.ContainsKey(key))
                    continue;

                this.DistanceInfoIndexMappings.Add(key, info);
            }
        }

        private void SetVehicleMappings()
        {
            foreach (Vehicle vehicle in this.Vehicles)
            {
                if (this.VehicleMappings.ContainsKey(vehicle.ID))
                    continue;

                this.VehicleMappings.Add(vehicle.ID, vehicle);
            }
        }

        private void SetVehicleIndexMappings() 
        {
            foreach (Vehicle vehicle in this.Vehicles)
            {
                if (this.VehicleIndexMappings.ContainsKey(vehicle.Index))
                    continue;

                this.VehicleIndexMappings.Add(vehicle.Index, vehicle);
            }
        }

        private void SetResourceMappings()
        {
            foreach (Resource resource in this.Resources)
            {
                if (this.ResourceMappings.ContainsKey(resource.ID))
                    continue;

                this.ResourceMappings.Add(resource.ID, resource);
            }
        }

        private void SetNodeMappings()
        {
            foreach (Node node in this.Nodes)
            {
                if (this.NodeMappings.ContainsKey(node.ID))
                    continue;

                this.NodeMappings.Add(node.ID, node);
            }
        }

        private void SetPickupNodeOrderIDMappings() 
        {
            foreach (Node node in this.Nodes)
            {
                if (node.Order == null)
                    continue;

                if (node.IsDelivery)
                    continue;

                if (this.PickupNodeOrderIDMappings.ContainsKey(node.Order.ID))
                    continue;

                this.PickupNodeOrderIDMappings.Add(node.Order.ID, node);
            }
        }

        private void SetDeliveryNodeOrderIDMappings()
        {
            foreach (Node node in this.Nodes)
            {
                if (node.Order == null)
                    continue;

                if (node.IsDelivery == false)
                    continue;

                if (this.DeliveryNodeOrderIDMappings.ContainsKey(node.Order.ID))
                    continue;

                this.DeliveryNodeOrderIDMappings.Add(node.Order.ID, node);
            }
        }

        private void SetNodeIndexMappings()
        {
            foreach (Node node in this.Nodes)
            {
                if (this.NodeIndexMappings.ContainsKey(node.Index))
                    continue;

                this.NodeIndexMappings.Add(node.Index, node);
            }
        }

        private void SetOrderMappings()
        {
            foreach (Order order in this.Orders)
            {
                if (this.OrderMappings.ContainsKey(order.ID))
                    continue;

                this.OrderMappings.Add(order.ID, order);
            }
        }

        private void SetOrderIndexMappings()
        {
            foreach (Order order in this.Orders)
            {
                if (this.OrderIndexMappings.ContainsKey(order.Index))
                    continue;

                this.OrderIndexMappings.Add(order.Index, order);
            }
        }

        private void SetProductMappings()
        {
            foreach (Product product in this.Products)
            {
                if (this.ProductMappings.ContainsKey(product.ID))
                    continue;

                this.ProductMappings.Add(product.ID, product);
            }
        }
    }
}
