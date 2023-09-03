using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nodez.Sdmp.Routing.DataModel
{
    public class Depot : Customer
    {
        public new Depot Clone() 
        {
            return base.Clone() as Depot;
        }
    }
}
