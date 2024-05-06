// Copyright (c) 2021-23, Sungwon Hong. All Rights Reserved. 
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

        public InputTable CustomerTable { get; private set; }

        public InputTable DistanceInfoTable { get; private set; }

        public InputTable VehicleResourceTable { get; private set; }

        public InputTable ProductTable { get; private set; }

        public InputTable DemandTable { get; private set; }

        public InputTable RunOptionTable { get; private set; }

        public void InitializeRoutingData()
        {
            InputManager inputManager = InputManager.Instance;

            this.VehicleTable = inputManager.GetInput(Constants.Constants.VEHICLE);
            this.ResourceTable = inputManager.GetInput(Constants.Constants.RESOURCE);
            this.CustomerTable = inputManager.GetInput(Constants.Constants.CUSTOMER);
            this.VehicleResourceTable = inputManager.GetInput(Constants.Constants.VEHICLE_RESOURCE);
            this.ProductTable = inputManager.GetInput(Constants.Constants.PRODUCT);
            this.DemandTable = inputManager.GetInput(Constants.Constants.DEMAND);
            this.RunOptionTable = inputManager.GetInput(Constants.Constants.RUN_OPTION);
            this.DistanceInfoTable = inputManager.GetInput(Constants.Constants.DISTANCE_INFO);

            RoutingData routingData = new RoutingData();

            if (VehicleTable != null)
            {
                routingData.SetVehicleDataList(VehicleTable.Rows().Cast<IVehicleData>().ToList());
            }

            if (CustomerTable != null)
            {
                routingData.SetCustomerDataList(CustomerTable.Rows().Cast<ICustomerData>().ToList());
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

            if (DemandTable != null)
            {
                routingData.SetDemandDataList(DemandTable.Rows().Cast<IDemandData>().ToList());
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

            if (this.RoutingData.CustomerDataList == null)
                return;

            if (this.RoutingData.ResourceDataList == null)
                return;

            if (this.RoutingData.VehicleResourceDataList == null)
                return;

            if (this.RoutingData.DemandDataList == null)
                return;

            if (this.RoutingData.ProductDataList == null)
                return;

            if (this.RoutingData.RunOptionDataList == null)
                return;

            this.RoutingProblem = new RoutingProblem();

            this.RoutingProblem.SetRunOptionObjects(this.CreateRunOptions(this.RoutingData.RunOptionDataList));
            this.RoutingProblem.SetProductObjects(this.CreateProducts(this.RoutingData.ProductDataList));
            this.RoutingProblem.SetDemandObjects(this.CreateDemands(this.RoutingData.DemandDataList));
            this.RoutingProblem.SetResourceObjects(this.CreateResources(this.RoutingData.ResourceDataList));
            this.RoutingProblem.SetVehicleObjects(this.CreateVehicles(this.RoutingData.VehicleDataList));
            this.RoutingProblem.SetCustomerObjects(this.CreateCustomers(this.RoutingData.CustomerDataList));
            this.RoutingProblem.SetDepotObjects(this.CreateDepot(this.RoutingData.CustomerDataList));

            bool isUseDistInfo = Convert.ToBoolean(this.GetRunOptionValue(Constants.Constants.IS_USE_DISTANCE_INFO_DATA));
            string distanceMetric = this.GetRunOptionValue(Constants.Constants.DISTANCE_METRIC);
            this.RoutingProblem.SetDistanceInfoObjects(this.CreateDistanceInfos(this.RoutingData.CustomerDataList, this.RoutingData.DistanceInfoDataList, isUseDistInfo, distanceMetric));
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

        private List<DistanceInfo> CreateDistanceInfos(List<ICustomerData> customerDataList, List<IDistanceInfoData> distanceInfoDataList, bool isUseDistInfo, string distanceMetric)
        {
            List<DistanceInfo> distInfos = new List<DistanceInfo>();

            int index_i = 0;
            foreach (ICustomerData i in customerDataList)
            {
                int index_j = 0;
                foreach (ICustomerData j in customerDataList)
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
                            time = data.TIME;
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

                    info.FromCustomerID = i.ID;
                    info.FromCustomerIndex = index_i;
                    info.ToCustomerID = j.ID;
                    info.ToCustomerIndex = index_j;
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
                vehicle.Resources = this.GetResourceMappings(item.ID);
                vehicle.Speed = item.SPEED;

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

        private Depot CreateDepot(List<ICustomerData> customerDataList)
        {
            Depot depot = new Depot();

            foreach (ICustomerData item in customerDataList)
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

        private List<Customer> CreateCustomers(List<ICustomerData> customerDataList)
        {
            List<Customer> customers = new List<Customer>();

            int index = 1;
            foreach (ICustomerData item in customerDataList)
            {
                bool isDepot = UtilityHelper.StringToBoolean(item.IS_DEPOT);

                if (isDepot)
                    continue;

                Customer cus = new Customer();
                cus.Index = index;
                cus.ID = item.ID;
                cus.Name = item.NAME;
                cus.Demand = this.GetDemand(item.DEMAND_ID);
                cus.TimeWindow = Tuple.Create(item.START_TIME_WINDOW, item.END_TIME_WINDOW);
                cus.IsVisited = false;
                cus.VisitedVehicle = null;
                cus.IsDelivery = UtilityHelper.StringToBoolean(item.IS_DELIVERY);
                cus.IsDepot = isDepot;

                customers.Add(cus);

                index++;              
            }

            return customers;
        }

        private List<Demand> CreateDemands(List<IDemandData> demandDataList) 
        {
            List<Demand> demands = new List<Demand>();

            foreach (IDemandData item in demandDataList)
            {
                Demand demand = new Demand();

                demand.ID = item.ID;
                demand.Name = item.NAME;
                demand.Product = this.GetProduct(item.PRODUCT_ID);
                demand.Quantity = item.QUANTITY;
          
                demands.Add(demand);
            }

            return demands;
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

        public Demand GetDemand(string demandID)
        {
            this.RoutingProblem.DemandMappings.TryGetValue(demandID, out Demand demand);

            return demand;
        }

        public Product GetProduct(string prodID)
        {
            this.RoutingProblem.ProductMappings.TryGetValue(prodID, out Product product);

            return product;
        }

        public Customer GetCustomer(int customerIndex) 
        {
            this.RoutingProblem.CustomerIndexMappings.TryGetValue(customerIndex, out Customer customer);

            return customer;
        }

        public Vehicle GetVehicle(int vehicelIndex)
        {
            this.RoutingProblem.VehicleIndexMappings.TryGetValue(vehicelIndex, out Vehicle vehicle);

            return vehicle;
        }

        public double GetDistance(int fromCustomerIndex, int toCustomerIndex) 
        {
            Tuple<int, int> key = Tuple.Create(fromCustomerIndex, toCustomerIndex);
            if (this.RoutingProblem.DistanceInfoIndexMappings.TryGetValue(key, out DistanceInfo info))
                return info.Distance;

            return 0;
        }

        public double GetTime(int fromCustomerIndex, int toCustomerIndex)
        {
            Tuple<int, int> key = Tuple.Create(fromCustomerIndex, toCustomerIndex);
            if (this.RoutingProblem.DistanceInfoIndexMappings.TryGetValue(key, out DistanceInfo info))
                return info.Time;

            return 0;
        }
    }
}
