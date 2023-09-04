// Copyright (c) 2021-23, Sungwon Hong. All Rights Reserved. 
// This Source Code Form is subject to the terms of the Mozilla Public License, Version 2.0. 
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.

using Nodez.Sdmp.Enum;
using Nodez.Sdmp.General.Managers;
using Nodez.Sdmp.Scheduling.Controls;
using Nodez.Sdmp.Scheduling.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nodez.Project.SchedulingTemplate.Controls
{
    public class UserSchedulingControl : SchedulingControl
    {
        private static readonly Lazy<UserSchedulingControl> lazy = new Lazy<UserSchedulingControl>(() => new UserSchedulingControl());

        public static new UserSchedulingControl Instance { get { return lazy.Value; } }

        public override ScheduleType GetScheduleType() 
        {
            return ScheduleType.EqpScheduling;
        }
    }
}
