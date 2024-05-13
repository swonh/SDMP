// Copyright (c) 2021-23, Sungwon Hong. All Rights Reserved. 
// This Source Code Form is subject to the terms of the Mozilla Public License, Version 2.0. 
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.

using Nodez.Sdmp.Routing.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nodez.Sdmp.Routing.DataModel
{
    public class Vehicle
    {
        public int Index { get; set; }

        public string ID { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public double Speed { get; set; }

        public double Capacity { get; set; }

        public double ServiceTime { get; set; }

        public double FixedCost { get; set; }

        public double VariableCost { get; set; }

        public Dictionary<string, Resource> Resources { get; set; }

        public double TotalTravelDistance { get; set; }

        public double TotalTravelTime { get; set; }

        public Node CurrentNode { get; set; }

        public Resource GetLoadableResource(Product product)
        {
            foreach (Resource resource in Resources.Values)
            {
                if (resource.Product.ID == product.ID)
                    return resource;
            }

            return null;
        }

        public Dictionary<string, Resource> CopyResources(Vehicle clone)
        {
            if (this.Resources == null)
                return null;

            Dictionary<string, Resource> copied = new Dictionary<string, Resource>();

            foreach (KeyValuePair<string, Resource> item in clone.Resources)
            {
                copied.Add(item.Key, item.Value.Clone());
            }

            return copied;
        }

        public Node CopyCurrentNode(Vehicle clone) 
        {
            if (this.CurrentNode == null)
                return null;

            return clone.CurrentNode.Clone();
        }

        public void ReplaceResources(Dictionary<string, Resource> resources) 
        {
            this.Resources = resources;
        }

        public void ReplaceCurrentNode(Node currentNode) 
        {
            this.CurrentNode = currentNode;
        }

        public Vehicle Clone() 
        {
            Vehicle clone = (Vehicle)this.MemberwiseClone();

            Dictionary<string, Resource> resources = CopyResources(clone);
            Node currentNode = CopyCurrentNode(clone);

            clone.ReplaceResources(resources);
            clone.ReplaceCurrentNode(currentNode);

            return clone;
        }
    }
}
