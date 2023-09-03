using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nodez.Sdmp.Scheduling.DataModel
{
    public class PMSchedule
    {
        public string PMID { get; set; }

        public string EqpID { get; set; }

        public string ModuleID { get; set; }

        public double PmStartTime { get; set; }

        public double PmEndTime { get; set; }
    }
}
