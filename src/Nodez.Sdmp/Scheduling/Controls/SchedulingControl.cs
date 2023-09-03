using Nodez.Sdmp.Enum;
using Nodez.Sdmp.General.Managers;
using Nodez.Sdmp.Scheduling.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nodez.Sdmp.Scheduling.Controls
{
    public class SchedulingControl
    {
        private static readonly Lazy<SchedulingControl> lazy = new Lazy<SchedulingControl>(() => new SchedulingControl());

        public static SchedulingControl Instance
        {
            get
            {
                if (ControlManager.Instance.RegisteredControls.TryGetValue(ControlType.SchedulingControl.ToString(), out object control))
                {
                    return (SchedulingControl)control;
                }
                else
                {
                    return lazy.Value;
                }
            }
        }

        public virtual ScheduleType GetScheduleType() 
        {
            return ScheduleType.EqpScheduling;
        }
    }
}
