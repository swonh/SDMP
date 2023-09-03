using Nodez.Sdmp.Scheduling.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nodez.Sdmp.Scheduling.DataModel
{
    public class Process
    {
        public string ProcessID { get; set; }

        public Dictionary<int, Tuple<Equipment, double>> TaskList { get; set; }

        public IProcessData ProcessData { get; set; }

        public Process() 
        {
            this.TaskList = new Dictionary<int, Tuple<Equipment, double>>();
        }
    }
}
