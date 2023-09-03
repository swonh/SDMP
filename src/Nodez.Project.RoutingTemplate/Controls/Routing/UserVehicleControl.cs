using Nodez.Sdmp.Routing.Controls;
using Nodez.Sdmp.Routing.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nodez.Project.RoutingTemplate.Controls
{
    public class UserVehicleControl : VehicleControl
    {
        private static readonly Lazy<UserVehicleControl> lazy = new Lazy<UserVehicleControl>(() => new UserVehicleControl());

        public static new UserVehicleControl Instance { get { return lazy.Value; } }

        public override List<Resource> GetLoadableResources(Product product, Vehicle vehicle)
        {
            return base.GetLoadableResources(product, vehicle);
        }

        public override Resource SelectResource(List<Resource> resources)
        {
            return base.SelectResource(resources);
        }
    }
}
