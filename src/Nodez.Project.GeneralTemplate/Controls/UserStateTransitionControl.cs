// Copyright (c) 2023 Sungwon Hong. All Rights Reserved. 
// Licenced under the Mozilla Public License, Version 2.0.

using Nodez.Sdmp.General.Controls;
using Nodez.Sdmp.General.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nodez.Project.GeneralTemplate.Controls
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
