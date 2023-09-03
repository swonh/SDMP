using Nodez.Sdmp.Scheduling.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nodez.Sdmp.Scheduling.DataModel
{
    public class Arrange
    {
        public int Index { get; set; }

        public string ProductID { get; set; }

        public string PropertyID { get; set; }

        public string EqpID { get; set; }

        public double ProcTime { get; set; }

        public IArrangeData ArrangeData { get; set; }
    }
}
