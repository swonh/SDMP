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

        public double Speed { get; set; }

        public Dictionary<string, Resource> Resources { get; set; }

        public double TotalTravelDistance { get; set; }

        public double TotalTravelTime { get; set; }

        public Customer CurrentCustomer { get; set; }

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

        public Customer CopyCurrentCustomer(Vehicle clone) 
        {
            if (this.CurrentCustomer == null)
                return null;

            return clone.CurrentCustomer.Clone();
        }

        public void ReplaceResources(Dictionary<string, Resource> resources) 
        {
            this.Resources = resources;
        }

        public void ReplaceCurrentCustomer(Customer currentCustomer) 
        {
            this.CurrentCustomer = currentCustomer;
        }

        public Vehicle Clone() 
        {
            Vehicle clone = (Vehicle)this.MemberwiseClone();

            Dictionary<string, Resource> resources = CopyResources(clone);
            Customer currentCustomer = CopyCurrentCustomer(clone);

            clone.ReplaceResources(resources);
            clone.ReplaceCurrentCustomer(currentCustomer);

            return clone;
        }
    }
}
