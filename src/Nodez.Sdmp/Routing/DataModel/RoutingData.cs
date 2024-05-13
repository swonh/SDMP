// Copyright (c) 2021-23, Sungwon Hong. All Rights Reserved. 
// This Source Code Form is subject to the terms of the Mozilla Public License, Version 2.0. 
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.

using Nodez.Data.Interface;
using Nodez.Sdmp.Routing.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nodez.Sdmp.Routing.DataModel
{
    public class RoutingData : IData
    {
        public List<IVehicleData> VehicleDataList { get; private set; }

        public List<INodeData> NodeDataList { get; private set; }

        public List<IDistanceInfoData> DistanceInfoDataList { get; private set; }

        public INodeData DepotData { get; private set; }

        public List<IResourceData> ResourceDataList { get; private set; }

        public List<IVehicleResourceData> VehicleResourceDataList { get; private set; }

        public List<IOrderData> OrderDataList { get; private set; }

        public List<IProductData> ProductDataList { get; private set; }

        public List<IRunOptionData> RunOptionDataList { get; private set; }

        public void SetRunOptionData(List<IRunOptionData> runOptionDataList)
        {
            this.RunOptionDataList = runOptionDataList;
        }

        public void SetVehicleDataList(List<IVehicleData> vehicleDataList)
        {
            this.VehicleDataList = vehicleDataList;
        }

        public void SetDistanceInfoDataList(List<IDistanceInfoData> distanceInfoDataList) 
        {
            this.DistanceInfoDataList = distanceInfoDataList;
        }

        public void SetNodeDataList(List<INodeData> nodeDataList)
        {
            this.NodeDataList = nodeDataList;
        }

        public void SetDepotDataList(INodeData depotData)
        {
            this.DepotData = depotData;
        }

        public void SetResourceDataList(List<IResourceData> resourceDataList)
        {
            this.ResourceDataList = resourceDataList;
        }

        public void SetOrderDataList(List<IOrderData> orderDataList)
        {
            this.OrderDataList = orderDataList;
        }

        public void SetProductDataList(List<IProductData> productDataList)
        {
            this.ProductDataList = productDataList;
        }

        public void SetVehicleResourceDataList(List<IVehicleResourceData> vehicleResourceDataList) 
        {
            this.VehicleResourceDataList = vehicleResourceDataList;
        }
    }
}
