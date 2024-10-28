// Copyright (c) 2021-24, Sungwon Hong. All Rights Reserved. 
// This Source Code Form is subject to the terms of the Mozilla Public License, Version 2.0. 
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.

using Nodez.Sdmp.General.Controls;
using Nodez.Sdmp.General.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace $safeprojectname$.Controls
{
    public class UserStateTransitionControl : StateTransitionControl
    {
        private static readonly Lazy<UserStateTransitionControl> lazy = new Lazy<UserStateTransitionControl>(() => new UserStateTransitionControl());

        public static new UserStateTransitionControl Instance { get { return lazy.Value; } }

        public override List<StateTransition> GetStateTransitions(StateActionMap stateActionMap)
        {
            return base.GetStateTransitions(stateActionMap);
        }
    }
}
