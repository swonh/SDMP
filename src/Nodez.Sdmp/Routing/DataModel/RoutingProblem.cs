﻿// Copyright (c) 2021-23, Sungwon Hong. All Rights Reserved. 
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

        public List<Customer> Customers { get; private set; }

        public Depot Depot { get; private set; }

        public List<Demand> Demands { get; private set; }

        public List<Product> Products { get; private set; }

        public List<DistanceInfo> DistanceInfos { get; private set; }

        public List<RunOption> RunOptions { get; private set; }

        public Dictionary<string, Vehicle> VehicleMappings { get; private set; }

        public Dictionary<int, Vehicle> VehicleIndexMappings { get; private set; }

        public Dictionary<string, Resource> ResourceMappings { get; private set; }

        public Dictionary<string, Customer> CustomerMappings { get; private set; }

        public Dictionary<int, Customer> CustomerIndexMappings { get; private set; }

        public Dictionary<string, Demand> DemandMappings { get; private set; }

        public Dictionary<string, Product> ProductMappings { get; private set; }

        public Dictionary<Tuple<string, string>, DistanceInfo> DistanceInfoMappings { get; private set; }

        public Dictionary<Tuple<int, int>, DistanceInfo> DistanceInfoIndexMappings { get; private set; }

        public Dictionary<string, string> RunOptionMappings { get; private set; }

        public RoutingProblem() 
        {
            this.Vehicles = new List<Vehicle>();
            this.Resources = new List<Resource>();
            this.Customers = new List<Customer>();
            this.Depot = new Depot();
            this.Demands = new List<Demand>();
            this.Products = new List<Product>();

            this.VehicleMappings = new Dictionary<string, Vehicle>();
            this.ResourceMappings = new Dictionary<string, Resource>();
            this.CustomerMappings = new Dictionary<string, Customer>();
            this.DemandMappings = new Dictionary<string, Demand>();
            this.ProductMappings = new Dictionary<string, Product>();
            this.DistanceInfoMappings = new Dictionary<Tuple<string, string>, DistanceInfo>();
            this.DistanceInfoIndexMappings = new Dictionary<Tuple<int, int>, DistanceInfo>();
            this.RunOptionMappings = new Dictionary<string, string>();

            this.CustomerIndexMappings = new Dictionary<int, Customer>();
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

        public void SetCustomerObjects(List<Customer> customers)
        {
            this.Customers = customers;
            this.SetCustomerMappings();
            this.SetCustomerIndexMappings();
        }

        public void SetDepotObjects(Depot depot)
        {
            this.Depot = depot;
        }

        public void SetDemandObjects(List<Demand> demands)
        {
            this.Demands = demands;
            this.SetDemandMappings();
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
                Tuple<string, string> key = Tuple.Create(info.FromCustomerID, info.ToCustomerID);

                if (this.DistanceInfoMappings.ContainsKey(key))
                    continue;

                this.DistanceInfoMappings.Add(key, info);
            }
        }

        private void SetDistanceInfoIndexMappings()
        {
            foreach (DistanceInfo info in this.DistanceInfos)
            {
                Tuple<int, int> key = Tuple.Create(info.FromCustomerIndex, info.ToCustomerIndex);

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

        private void SetCustomerMappings()
        {
            foreach (Customer customer in this.Customers)
            {
                if (this.CustomerMappings.ContainsKey(customer.ID))
                    continue;

                this.CustomerMappings.Add(customer.ID, customer);
            }
        }

        private void SetCustomerIndexMappings()
        {
            foreach (Customer customer in this.Customers)
            {
                if (this.CustomerIndexMappings.ContainsKey(customer.Index))
                    continue;

                this.CustomerIndexMappings.Add(customer.Index, customer);
            }
        }

        private void SetDemandMappings()
        {
            foreach (Demand demand in this.Demands)
            {
                if (this.DemandMappings.ContainsKey(demand.ID))
                    continue;

                this.DemandMappings.Add(demand.ID, demand);
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
