using Nodez.Sdmp.Scheduling.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nodez.Sdmp.Scheduling.DataModel
{
    public class PlanInfo
    {
        public int PlanningHorizon { get; set; }

        public int PriorityLookahead { get; set; }

        public IPlanInfoData PlanInfoData { get; set; }
    }
}
