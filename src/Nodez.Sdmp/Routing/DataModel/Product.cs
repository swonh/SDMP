// Copyright (c) 2023 Sungwon Hong. All Rights Reserved. 
// Licenced under the Mozilla Public License, Version 2.0.

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
