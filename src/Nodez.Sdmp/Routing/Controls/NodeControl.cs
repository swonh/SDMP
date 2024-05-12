// Copyright (c) 2021-23, Sungwon Hong. All Rights Reserved. 
// This Source Code Form is subject to the terms of the Mozilla Public License, Version 2.0. 
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.

using Nodez.Sdmp.Constants;
using Nodez.Sdmp.Enum;
using Nodez.Sdmp.General.Managers;
using Nodez.Sdmp.Routing.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nodez.Sdmp.Routing.Controls
{
    public class NodeControl
    {
        private static readonly Lazy<NodeControl> lazy = new Lazy<NodeControl>(() => new NodeControl());

        public static NodeControl Instance
        {
            get
            {
                if (ControlManager.Instance.RegisteredControls.TryGetValue(ControlType.NodeControl.ToString(), out object control))
                {
                    return (NodeControl)control;
                }
                else
                {
                    return lazy.Value;
                }
            }
        }

        public virtual Dictionary<int, VehicleStateInfo> GetVisitableNodes(Dictionary<int, VehicleStateInfo> vehicleInfos) 
        {
            return vehicleInfos;
        }
    }
}
