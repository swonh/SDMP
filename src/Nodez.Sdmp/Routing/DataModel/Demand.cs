// Copyright (c) 2021-23, Sungwon Hong. All Rights Reserved. 
// This Source Code Form is subject to the terms of the Mozilla Public License, Version 2.0. 
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nodez.Sdmp.Routing.DataModel
{
    public class Demand
    {
        public string ID { get; set; }

        public string Name { get; set; }

        public Product Product { get; set; }

        public double Quantity { get; set; }

        public Product CopyProduct(Demand clone) 
        {
            if (this.Product == null)
                return null;

            return clone.Product.Clone();
        }

        public void ReplaceProduct(Product product) 
        {
            this.Product = product;
        }

        public Demand Clone() 
        {
            Demand clone = (Demand)this.MemberwiseClone();

            Product product = CopyProduct(clone);

            clone.ReplaceProduct(product);

            return clone;
        }
    }
}
