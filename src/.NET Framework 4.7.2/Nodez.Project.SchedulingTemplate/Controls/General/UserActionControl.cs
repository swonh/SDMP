// Copyright (c) 2021-25, Sungwon Hong. All Rights Reserved. 
// This Source Code Form is subject to the terms of the Mozilla Public License, Version 2.0. 
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.

using Nodez.Sdmp.General.Controls;
using Nodez.Sdmp.General.DataModel;
using Nodez.Sdmp.Scheduling.Controls;
using Nodez.Sdmp.Scheduling.DataModel;
using Nodez.Sdmp.Scheduling.Enum;
using Nodez.Sdmp.Scheduling.Managers;
using System;
using System.Collections.Generic;

namespace Nodez.Project.SchedulingTemplate.Controls
{
    public class UserActionControl : ActionControl
    {
        private static readonly Lazy<UserActionControl> lazy = new Lazy<UserActionControl>(() => new UserActionControl());

        public static new UserActionControl Instance { get { return lazy.Value; } }

        public override List<StateActionMap> GetStateActionMaps(State state)
        {
            // Default Logic
            SchedulingState schedulingState = state as SchedulingState;
            SchedulingActionManager manager = SchedulingActionManager.Instance;

            SchedulingControl schedControl = SchedulingControl.Instance;
            ScheduleType schedType = schedControl.GetScheduleType();

            List<StateActionMap> maps = manager.GetStateActionMaps(schedulingState, schedType);

            return maps;
        }
    }
}
