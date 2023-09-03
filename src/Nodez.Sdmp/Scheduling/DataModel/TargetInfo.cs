using Nodez.Sdmp.Scheduling.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nodez.Sdmp.Scheduling.DataModel
{
    public class TargetInfo
    {
        public int Index { get; set; }

        public string TargetID { get; set; }

        public string ProcessID { get; set; }

        public string StepSeq { get; set; }

        public int TargetQty { get; set; }

        public ITargetInfoData TargetInfoData { get; set; }
    }
}
