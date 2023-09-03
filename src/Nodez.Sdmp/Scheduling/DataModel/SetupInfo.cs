using Nodez.Sdmp.Scheduling.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nodez.Sdmp.Scheduling.DataModel
{
    public class SetupInfo
    {
        public string EqpID { get; set; }

        public string FromPropertyID { get; set; }

        public string ToPropertyID { get; set; }

        public double SetupTime { get; set; }

        public ISetupInfoData SetupInfoData { get; set; }
    }
}
