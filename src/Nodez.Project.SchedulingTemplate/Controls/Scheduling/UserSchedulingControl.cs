// Copyright (c) 2021-25, Sungwon Hong. All Rights Reserved. 
// This Source Code Form is subject to the terms of the Mozilla Public License, Version 2.0. 
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.

using Nodez.Sdmp.Scheduling.Controls;
using Nodez.Sdmp.Scheduling.Enum;
using System;

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
