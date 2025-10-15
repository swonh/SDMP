// Copyright (c) 2021-25, Sungwon Hong. All Rights Reserved. 
// This Source Code Form is subject to the terms of the Mozilla Public License, Version 2.0. 
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.

using Nodez.Sdmp.Enum;
using Nodez.Sdmp.General.Managers;
using Nodez.Sdmp.Routing.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nodez.Sdmp.Routing.Controls
{
    public class VehicleControl
    {
        private static readonly Lazy<VehicleControl> lazy = new Lazy<VehicleControl>(() => new VehicleControl());

        public static VehicleControl Instance
        {
            get
            {
                if (ControlManager.Instance.RegisteredControls.TryGetValue(ControlType.VehicleControl.ToString(), out object control))
                {
                    return (VehicleControl)control;
                }
                else
                {
                    return lazy.Value;
                }
            }
        }

        public virtual Vehicle SelectVehicle(List<Vehicle> vehicles)
        {
            return vehicles.FirstOrDefault();
        }

        public virtual List<Resource> GetLoadableResources(Product product, Vehicle vehicle)
        {
            List<Resource> list = new List<Resource>();

            foreach (KeyValuePair<string, Resource> item in vehicle.Resources)
            {
                Resource res = item.Value;

                if (res.Product.ID != product.ID)
                    continue;

                list.Add(res);
            }

            return list;
        }

        public virtual Resource SelectResource(List<Resource> resources)
        {
            return resources.FirstOrDefault();
        }
    }
}
