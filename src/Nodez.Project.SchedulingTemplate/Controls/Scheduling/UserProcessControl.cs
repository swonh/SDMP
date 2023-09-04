// Copyright (c) 2023 Sungwon Hong. All Rights Reserved. 
// Licenced under the Mozilla Public License, Version 2.0.

using Nodez.Sdmp.Enum;
using Nodez.Sdmp.General.Managers;
using Nodez.Sdmp.Scheduling.Controls;
using Nodez.Sdmp.Scheduling.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nodez.Project.SchedulingTemplate.Controls
{
    public class UserProcessControl : ProcessControl
    {
        private static readonly Lazy<UserProcessControl> lazy = new Lazy<UserProcessControl>(() => new UserProcessControl());

        public static new UserProcessControl Instance { get { return lazy.Value; } }

        public override bool IsLoadable(Job job, Equipment eqp) 
        {
            return true;
        }
    }
}
