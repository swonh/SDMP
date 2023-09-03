using Nodez.Sdmp.Scheduling.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nodez.Sdmp.Scheduling.Managers
{
    public class SchedulingBoundManager
    {
        private static readonly Lazy<SchedulingBoundManager> lazy = new Lazy<SchedulingBoundManager>(() => new SchedulingBoundManager());

        public static SchedulingBoundManager Instance { get { return lazy.Value; } }

        public double GetDualBound(SchedulingState state)
        {
            return 0;
        }
    }
}
