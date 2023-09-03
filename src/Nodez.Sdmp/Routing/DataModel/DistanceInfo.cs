using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nodez.Sdmp.Routing.DataModel
{
    public class DistanceInfo
    {
        public string FromCustomerID { get; set; }

        public int FromCustomerIndex { get; set; }

        public string ToCustomerID { get; set; }

        public int ToCustomerIndex { get; set; }

        public double Distance { get; set; }

        public double Time { get; set; }
    }
}
