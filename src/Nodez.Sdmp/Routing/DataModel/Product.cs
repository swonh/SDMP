using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nodez.Sdmp.Routing.DataModel
{
    public class Product
    {
        public string ID { get; set; }

        public string Name { get; set; }

        public Product Clone() 
        {
            Product clone = (Product)this.MemberwiseClone();

            return clone;
        }
    }
}
