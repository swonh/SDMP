using Nodez.Sdmp.Scheduling.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nodez.Sdmp.Scheduling.DataModel
{
    public class EqpGroup
    {
        public int Index { get; set; }

        public string GroupID { get; set; }

        public string GroupName { get; set; }

        public List<Equipment> EqpList { get; set; }

        public IEqpGroupData EqpGroupData { get; set; }

        public List<Arrange> ArrangeList { get; set; }
    }
}
