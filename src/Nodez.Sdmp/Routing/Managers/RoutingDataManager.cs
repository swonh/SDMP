// Copyright (c) 2021-24, Sungwon Hong. All Rights Reserved. 
// This Source Code Form is subject to the terms of the Mozilla Public License, Version 2.0. 
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.

using Nodez.Data.DataModel;
using Nodez.Data.Interface;
using Nodez.Data.Managers;
using Nodez.Sdmp.Constants;
using Nodez.Sdmp.Routing.DataModel;
using Nodez.Sdmp.Routing.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nodez.Sdmp.Routing.Managers
{
    public class RoutingDataManager
    {
        private static readonly Lazy<RoutingDataManager> lazy = new Lazy<RoutingDataManager>(() => new RoutingDataManager());

        public static RoutingDataManager Instance { get { return lazy.Value; } }

        public RoutingData RoutingData { get; private set; }

        public RoutingProblem RoutingProblem { get; private set; }

        public InputTable VehicleTable { get; private set; }

        public InputTable ResourceTable { get; private set; }

        public InputTable NodeTable { get; private set; }

        public InputTable DistanceInfoTable { get; private set; }

        public InputTable VehicleResourceTable { get; private set; }

        public InputTable ProductTable { get; private set; }

        public InputTable OrderTable { get; private set; }

        public InputTable RunOptionTable { get; private set; }

        public void InitializeRoutingData()
        {
            InputManager inputManager = InputManager.Instance;

            this.VehicleTable = inputManager.GetInput(Constants.Constants.VEHICLE);
            this.ResourceTable = inputManager.GetInput(Constants.Constants.RESOURCE);
            this.NodeTable = inputManager.GetInput(Constants.Constants.NODE);
            this.VehicleResourceTable = inputManager.GetInput(Constants.Constants.VEHICLE_RESOURCE);
            this.ProductTable = inputManager.GetInput(Constants.Constants.PRODUCT);
            this.OrderTable = inputManager.GetInput(Constants.Constants.ORDER);
            this.RunOptionTable = inputManager.GetInput(Constants.Constants.RUN_OPTION);
            this.DistanceInfoTable = inputManager.GetInput(Constants.Constants.DISTANCE_INFO);

            RoutingData routingData = new RoutingData();

            if (VehicleTable != null)
            {
                routingData.SetVehicleDataList(VehicleTable.Rows().Cast<IVehicleData>().ToList());
            }

            if (NodeTable != null)
            {
                routingData.SetNodeDataList(NodeTable.Rows().Cast<INodeData>().ToList());
            }

            if (DistanceInfoTable != null) 
            {
                routingData.SetDistanceInfoDataList(DistanceInfoTable.Rows().Cast<IDistanceInfoData>().ToList());
            }

            if (ResourceTable != null)
            {
                routingData.SetResourceDataList(ResourceTable.Rows().Cast<IResourceData>().ToList());
            }

            if (VehicleResourceTable != null)
            {
                routingData.SetVehicleResourceDataList(VehicleResourceTable.Rows().Cast<IVehicleResourceData>().ToList());
            }

            if (ProductTable != null)
            {
                routingData.SetProductDataList(ProductTable.Rows().Cast<IProductData>().ToList());
            }

            if (OrderTable != null)
            {
                routingData.SetOrderDataList(OrderTable.Rows().Cast<IOrderData>().ToList());
            }

            if (RunOptionTable != null) 
            {
                routingData.SetRunOptionData(RunOptionTable.Rows().Cast<IRunOptionData>().ToList());
            }

            this.RoutingData = routingData;
        }

        public void InitializeRoutingProblem()
        {
            if (this.RoutingData == null)
                return;

            if (this.RoutingData.VehicleDataList == null)
                return;

            if (this.RoutingData.NodeDataList == null)
                return;

            if (this.RoutingData.ResourceDataList == null)
                return;

            if (this.RoutingData.VehicleResourceDataList == null)
                return;

            if (this.RoutingData.OrderDataList == null)
                return;

            if (this.RoutingData.ProductDataList == null)
                return;

            if (this.RoutingData.RunOptionDataList == null)
                return;

            this.RoutingProblem = new RoutingProblem();

            this.RoutingProblem.SetRunOptionObjects(this.CreateRunOptions(this.RoutingData.RunOptionDataList));
            bool isUseDistInfo = Convert.ToBoolean(this.GetRunOptionValue(Constants.Constants.IS_USE_DISTANCE_INFO_DATA));
            string distanceMetric = this.GetRunOptionValue(Constants.Constants.DISTANCE_METRIC);
            this.RoutingProblem.SetDistanceInfoObjects(this.CreateDistanceInfos(this.RoutingData.NodeDataList, this.RoutingData.DistanceInfoDataList, isUseDistInfo, distanceMetric));

            this.RoutingProblem.SetProductObjects(this.CreateProducts(this.RoutingData.ProductDataList));
            this.RoutingProblem.SetOrderObjects(this.CreateOrders(this.RoutingData.OrderDataList));
            this.RoutingProblem.SetNodeObjects(this.CreateNodes(this.RoutingData.NodeDataList));
            this.RoutingProblem.SetDepotObjects(this.CreateDepot(this.RoutingData.NodeDataList));
            this.RoutingProblem.SetResourceObjects(this.CreateResources(this.RoutingData.ResourceDataList));
            this.RoutingProblem.SetVehicleObjects(this.CreateVehicles(this.RoutingData.VehicleDataList));
        }

        public string GetRunOptionValue(string optionName) 
        {
            this.RoutingProblem.RunOptionMappings.TryGetValue(optionName, out string value);

            return value;
        }

        private List<RunOption> CreateRunOptions(List<IRunOptionData> runOptionDataList) 
        {
            List<RunOption> runOptions = new List<RunOption>();

            foreach (IRunOptionData runOption in runOptionDataList)
            {
                RunOption option = new RunOption();

                option.OPTION_NAME = runOption.OPTION_NAME;
                option.OPTION_VALUE = runOption.OPTION_VALUE;

                runOptions.Add(option);
            }

            return runOptions;
        }

        private List<DistanceInfo> CreateDistanceInfos(List<INodeData> nodeDataList, List<IDistanceInfoData> distanceInfoDataList, bool isUseDistInfo, string distanceMetric)
        {
            List<DistanceInfo> distInfos = new List<DistanceInfo>();

            int index_i = 0;
            foreach (INodeData i in nodeDataList)
            {
                int index_j = 0;
                foreach (INodeData j in nodeDataList)
                {
                    DistanceInfo info = new DistanceInfo();

                    double dist = 0;
                    double time = 0;
                    if (isUseDistInfo)
                    {
                        IDistanceInfoData data = (IDistanceInfoData)this.DistanceInfoTable.FindRows(1, i.ID, j.ID).FirstOrDefault();

                        if (data != null)
                        {
                            dist = data.DISTANCE;
                        }
                    }
                    else
                    {
                        dist = UtilityHelper.CalculateEuclideanDistance(i.X_COORDINATE, i.Y_COORDINATE, j.X_COORDINATE, j.Y_COORDINATE);
                        dist = Math.Round(dist, 2);

                        if (distanceMetric == Constants.Constants.MANHATTAN)
                        {
                            dist = UtilityHelper.CalculateManhattanDistance(i.X_COORDINATE, i.Y_COORDINATE, j.X_COORDINATE, j.Y_COORDINATE);
                            dist = Math.Round(dist, 2);
                        }
                    }

                    info.FromNodeID = i.ID;
                    info.FromNodeIndex = index_i;
                    info.ToNodeID = j.ID;
                    info.ToNodeIndex = index_j;
                    info.Distance = dist;
                    info.Time = time;

                    distInfos.Add(info);

                    index_j++;
                }

                index_i++;
            }

            return distInfos;
        }

        private List<Vehicle> CreateVehicles(List<IVehicleData> vehicleDataList)
        {
            List<Vehicle> vehicles = new List<Vehicle>();

            int index = 1;
            foreach (IVehicleData item in vehicleDataList)
            {
                Vehicle vehicle = new Vehicle();

                vehicle.Index = index;
                vehicle.ID = item.ID;
                vehicle.Name = item.NAME;
                vehicle.Type = item.TYPE;
                vehicle.Resources = this.GetResourceMappings(item.ID);
                vehicle.Speed = item.SPEED;
                vehicle.Capacity = item.CAPACITY;
                vehicle.ServiceTime = item.SERVICE_TIME;
                vehicle.FixedCost = item.FIXED_COST;
                vehicle.VariableCost = item.VARIABLE_COST;

                vehicles.Add(vehicle);

                index++;
            }

            return vehicles;
        }

        private List<Resource> CreateResources(List<IResourceData> resourceDataList)
        {
            List<Resource> resources = new List<Resource>();

            int index = 1;
            foreach (IResourceData item in resourceDataList)
            {
                Resource res = new Resource();

                res.Index = index;
                res.ID = item.ID;
                res.Name = item.NAME;
                res.Product = this.GetProduct(item.PRODUCT_ID);
                res.OrgCapacity = item.CAPACITY;
                res.RemainCapacity = item.CAPACITY;

                resources.Add(res);

                index++;
            }

            return resources;
        }

        private Depot CreateDepot(List<INodeData> nodeDataList)
        {
            Depot depot = new Depot();

            foreach (INodeData item in nodeDataList)
            {
                bool isDepot = UtilityHelper.StringToBoolean(item.IS_DEPOT);

                if (isDepot == false)
                    continue;

                depot.ID = item.ID;
                depot.Name = item.NAME;
                depot.IsDepot = isDepot;
                depot.Index = 0;
            }

            return depot;
        }

        private List<Node> CreateNodes(List<INodeData> nodeDataList)
        {
            List<Node> nodes = new List<Node>();

            int index = 0;
            foreach (INodeData item in nodeDataList)
            {
                bool isDepot = UtilityHelper.StringToBoolean(item.IS_DEPOT);

                Node node = new Node();
                node.Index = index;
                node.ID = item.ID;
                node.Name = item.NAME;
                node.Order = this.GetOrder(item.ORDER_ID);
                node.X_Coordinate = item.X_COORDINATE;
                node.Y_Coordinate = item.Y_COORDINATE;
                node.TimeWindow = (item.START_TIME_WINDOW, item.END_TIME_WINDOW);
                node.IsVisited = false;
                node.VisitedVehicle = null;
                node.IsDelivery = UtilityHelper.StringToBoolean(item.IS_DELIVERY);
                node.IsDepot = isDepot;

                nodes.Add(node);

                if (node.IsDepot == false)
                {
                    if (node.IsDelivery)
                        node.Order.DeliveryNode = node;
                    else
                        node.Order.PickupNode = node;

                    if (node.Order.PickupNode != null && node.Order.DeliveryNode != null)
                        node.Order.Distance = GetDistance(node.Order.PickupNode.Index, node.Order.DeliveryNode.Index);
                }

                index++;
            }

            return nodes;
        }

        private List<Order> CreateOrders(List<IOrderData> orderDataList) 
        {
            List<Order> orders = new List<Order>();

            int index = 1;
            foreach (IOrderData item in orderDataList)
            {
                Order order = new Order();

                order.Index = index;
                order.ID = item.ID;
                order.Name = item.NAME;
                order.OrderTime = item.ORDER_TIME;
                order.Product = this.GetProduct(item.PRODUCT_ID);
                order.PickupNode = this.GetNode(item.PICKUP_NODE_ID);
                order.DeliveryNode = this.GetNode(item.DELIVERY_NODE_ID);
                order.ProcessTime = item.PROCESS_TIME;
                order.Deadline = item.DEADLINE;
                order.Quantity = item.ORDER_QTY;
          
                orders.Add(order);

                index++;
            }

            return orders;
        }

        private List<Product> CreateProducts(List<IProductData> productDataList)
        {
            List<Product> products = new List<Product>();

            foreach (IProductData item in productDataList)
            {
                Product product = new Product();

                product.ID = item.ID;
                product.Name = item.NAME;

                products.Add(product);
            }

            return products;
        }

        public Dictionary<string, Resource> GetResourceMappings(string vehicleID)
        {
            Dictionary<string, Resource> resourceMappings = new Dictionary<string, Resource>();

            List<IInputRow> rows = this.VehicleResourceTable.FindRows(1, vehicleID);

            foreach (IVehicleResourceData row in rows)
            {
                if (resourceMappings.ContainsKey(row.RESOURCE_ID))
                    continue;

                Resource res = this.GetResource(row.RESOURCE_ID);

                if (res == null)
                    continue;

                resourceMappings.Add(row.RESOURCE_ID, res);
            }

            return resourceMappings;
        }

        public Resource GetResource(string resourceID)
        {
            this.RoutingProblem.ResourceMappings.TryGetValue(resourceID, out Resource resource);

            return resource;
        }

        public Order GetOrder(string orderID)
        {
            if (string.IsNullOrEmpty(orderID))
                return null;

            this.RoutingProblem.OrderMappings.TryGetValue(orderID, out Order order);

            return order;
        }

        public Order GetOrder(int orderIndex)
        {
            this.RoutingProblem.OrderIndexMappings.TryGetValue(orderIndex, out Order order);

            return order;
        }

        public Product GetProduct(string prodID)
        {
            this.RoutingProblem.ProductMappings.TryGetValue(prodID, out Product product);

            return product;
        }

        public Node GetNode(int nodeIndex) 
        {
            this.RoutingProblem.NodeIndexMappings.TryGetValue(nodeIndex, out Node node);

            return node;
        }

        public Node GetNode(string nodeID)
        {
            this.RoutingProblem.NodeMappings.TryGetValue(nodeID, out Node node);

            return node;
        }

        public Node GetPickupNodeByOrderID(string orderID)
        {
            this.RoutingProblem.PickupNodeOrderIDMappings.TryGetValue(orderID, out Node node);

            return node;
        }

        public Node GetDeliveryNodeByOrderID(string orderID)
        {
            this.RoutingProblem.DeliveryNodeOrderIDMappings.TryGetValue(orderID, out Node node);

            return node;
        }

        public Vehicle GetVehicle(int vehicelIndex)
        {
            this.RoutingProblem.VehicleIndexMappings.TryGetValue(vehicelIndex, out Vehicle vehicle);

            return vehicle;
        }

        public double GetDistance(int fromNodeIndex, int toNodeIndex) 
        {
            ValueTuple<int, int> key = (fromNodeIndex, toNodeIndex);
            if (this.RoutingProblem.DistanceInfoIndexMappings.TryGetValue(key, out DistanceInfo info))
                return info.Distance;

            return 0;
        }

        public double GetTime(Vehicle vehicle, int fromNodeIndex, int toNodeIndex)
        {
            ValueTuple<int, int> key = (fromNodeIndex, toNodeIndex);
            if (this.RoutingProblem.DistanceInfoIndexMappings.TryGetValue(key, out DistanceInfo info))
            {
                 return Math.Round((info.Distance / vehicle.Speed) + vehicle.ServiceTime);
            }

            return 0;
        }
    }
}
