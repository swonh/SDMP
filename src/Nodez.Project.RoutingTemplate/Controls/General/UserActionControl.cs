// Copyright (c) 2023 Sungwon Hong. All Rights Reserved. 
// Licenced under the Mozilla Public License, Version 2.0.

using Nodez.Data.Managers;
using Nodez.Sdmp.General.Controls;
using Nodez.Sdmp.General.DataModel;
using Nodez.Sdmp.Routing.DataModel;
using Nodez.Sdmp.Routing.Managers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nodez.Project.RoutingTemplate.Controls
{
    public class UserActionControl : ActionControl
    {
        private static readonly Lazy<UserActionControl> lazy = new Lazy<UserActionControl>(() => new UserActionControl());

        public static new UserActionControl Instance { get { return lazy.Value; } }

        public override List<StateActionMap> GetStateActionMaps(State state)
        {
            // Default Logic
            RoutingState routingState = state as RoutingState;
            RoutingActionManager manager = RoutingActionManager.Instance;

            List<StateActionMap> maps = manager.GetStateActionMaps(routingState);

            return maps;
        }
    }
}
