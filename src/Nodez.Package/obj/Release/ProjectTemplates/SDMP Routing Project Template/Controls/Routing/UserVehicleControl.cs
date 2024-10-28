// Copyright (c) 2021-24, Sungwon Hong. All Rights Reserved. 
// This Source Code Form is subject to the terms of the Mozilla Public License, Version 2.0. 
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.

using Nodez.Sdmp.Routing.Controls;
using Nodez.Sdmp.Routing.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace $safeprojectname$.Controls
{
    public class UserVehicleControl : VehicleControl
    {
        private static readonly Lazy<UserVehicleControl> lazy = new Lazy<UserVehicleControl>(() => new UserVehicleControl());

        public static new UserVehicleControl Instance { get { return lazy.Value; } }

        public override List<Resource> GetLoadableResources(Product product, Vehicle vehicle)
        {
            return base.GetLoadableResources(product, vehicle);
        }

        public override Resource SelectResource(List<Resource> resources)
        {
            return base.SelectResource(resources);
        }
    }
}
