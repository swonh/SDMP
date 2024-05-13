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
    public class Order
    {
        public int Index { get; set; }

        public string ID { get; set; }

        public string Name { get; set; }

        public double OrderTime { get; set; }

        public Node PickupNode { get; set; }

        public Node DeliveryNode { get; set; }

        public double ProcessTime { get; set; }

        public double ReadyTime { get { return OrderTime + ProcessTime; } }

        public double Deadline { get; set; }

        public Product Product { get; set; }

        public double Quantity { get; set; }

        public double Distance { get; set; }

        public Node CopyDeliveryNode(Order clone)
        {
            if (this.DeliveryNode == null)
                return null;

            return clone.DeliveryNode.Clone();
        }

        public void ReplaceDeliveryNode(Node deliveryNode)
        {
            this.DeliveryNode = deliveryNode;
        }

        public Node CopyPickupNode(Order clone)
        {
            if (this.PickupNode == null)
                return null;

            return clone.PickupNode.Clone();
        }

        public void ReplacePickupNode(Node pickupNode)
        {
            this.PickupNode = pickupNode;
        }

        public Product CopyProduct(Order clone)
        {
            if (this.Product == null)
                return null;

            return clone.Product.Clone();
        }

        public void ReplaceProduct(Product product)
        {
            this.Product = product;
        }

        public Order Clone()
        {
            Order clone = (Order)this.MemberwiseClone();

            Product product = CopyProduct(clone);
            clone.ReplaceProduct(product);

            Node deliveryNode = CopyDeliveryNode(clone);
            clone.ReplaceDeliveryNode(deliveryNode);

            Node pickupNode = CopyPickupNode(clone);
            clone.ReplacePickupNode(pickupNode);

            return clone;
        }

    }
}
