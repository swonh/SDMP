using Nodez.Sdmp.Enum;
using Nodez.Sdmp.General.Managers;
using Nodez.Sdmp.Scheduling.Controls;
using Nodez.Sdmp.Scheduling.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace $safeprojectname$.Controls
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
