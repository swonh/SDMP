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

        public List<ICustomerData> CustomerDataList { get; private set; }

        public List<IDistanceInfoData> DistanceInfoDataList { get; private set; }

        public ICustomerData DepotData { get; private set; }

        public List<IResourceData> ResourceDataList { get; private set; }

        public List<IVehicleResourceData> VehicleResourceDataList { get; private set; }

        public List<IDemandData> DemandDataList { get; private set; }

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

        public void SetCustomerDataList(List<ICustomerData> customerDataList)
        {
            this.CustomerDataList = customerDataList;
        }

        public void SetDepotDataList(ICustomerData depotData)
        {
            this.DepotData = depotData;
        }

        public void SetResourceDataList(List<IResourceData> resourceDataList)
        {
            this.ResourceDataList = resourceDataList;
        }

        public void SetDemandDataList(List<IDemandData> demandDataList)
        {
            this.DemandDataList = demandDataList;
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
