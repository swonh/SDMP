// Copyright (c) 2021-24, Sungwon Hong. All Rights Reserved. 
// This Source Code Form is subject to the terms of the Mozilla Public License, Version 2.0. 
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.

using Nodez.Sdmp.Routing.Controls;
using Nodez.Sdmp.Routing.DataModel;
using Nodez.Sdmp.Routing.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nodez.Project.RoutingTemplate.Controls
{
    public class UserNodeControl : NodeControl
    {
        private static readonly Lazy<UserNodeControl> lazy = new Lazy<UserNodeControl>(() => new UserNodeControl());

        public static new UserNodeControl Instance { get { return lazy.Value; } }

        public override Dictionary<int, VehicleStateInfo> GetVisitableNodes(Dictionary<int, VehicleStateInfo> vehicleInfos)
        {
            return base.GetVisitableNodes(vehicleInfos);         
        }
    }
}
